using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using TedToolkit.RoslynHelper.Syntaxes;
using TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

namespace TedToolkit.RoslynHelper.Tests;

internal sealed class AsyncCompositionTests
{
    /// <summary>
    /// Async methods and local functions preserve metadata, return types, and conditional compilation.
    /// </summary>
    [Test]
    public async Task Should_compile_async_method_with_attributed_local_function()
    {
        var local = new Method("ReadAsync", new ReturnType(DataType.ValueTaskOf(DataType.Int)))
            .Static.Async
            .AddAttribute(SourceComposer.Attribute<ObsoleteAttribute>())
            .AddStatement(DataType.ValueTaskOf(DataType.Int).New.AddArguments(42.ToLiteral())
                .ConfigureAwait(false).Await().Return);
        var method = SourceComposer<AsyncCompositionTests>.Method("ExecuteAsync", new ReturnType(DataType.TaskOf(DataType.Int)))
            .Public.Async
            .AddRootDescription(new DescriptionSummary(new DescriptionText("Reads a value asynchronously.")))
            .AddCondition(PreprocessorExpression.Symbol("ASYNC"))
            .AddStatement(local)
            .AddStatement("ReadAsync".ToSimpleName().Invoke().ConfigureAwait(false).Await().Return);
        var file = new SourceFile { DisableWarnings = false, NullableContext = NullableContextOptions.Enable }
            .AddMember(new TypeDeclaration("Worker", TypeDeclarationType.CLASS).Public.AddMember(method));
        var code = "#define ASYNC\n" + file.ToCode();
        var compilation = RoslynTestHelper.CreateCompilation(code);
        var symbol = RoslynTestHelper.GetMethod(compilation, "Worker", "ExecuteAsync");
        var localSyntax = compilation.SyntaxTrees.Single().GetRoot().DescendantNodes()
            .OfType<LocalFunctionStatementSyntax>().Single();
        var localSymbol = (IMethodSymbol)compilation.GetSemanticModel(localSyntax.SyntaxTree).GetDeclaredSymbol(localSyntax)!;

        await Assert.That(symbol.IsAsync).IsTrue();
        await Assert.That(symbol.ReturnType.ToDisplayString()).IsEqualTo("System.Threading.Tasks.Task<int>");
        await Assert.That(symbol.GetAttributes().Single().AttributeClass!.Name).IsEqualTo("GeneratedCodeAttribute");
        await Assert.That(symbol.GetDocumentationCommentXml()).Contains("Reads a value asynchronously.");
        await Assert.That(localSymbol.IsAsync).IsTrue();
        await Assert.That(localSymbol.ReturnType.ToDisplayString()).IsEqualTo("System.Threading.Tasks.ValueTask<int>");
        await Assert.That(localSymbol.GetAttributes().Single().AttributeClass!.Name).IsEqualTo("ObsoleteAttribute");
    }

    /// <summary>
    /// Await remains independent of ConfigureAwait and explicit parentheses preserve expression grouping.
    /// </summary>
    [Test]
    public async Task Should_compile_await_expressions_with_explicit_configuration_and_grouping()
    {
        var task = "task".ToSimpleName();
        var fallback = "fallback".ToSimpleName();
        var resultLength = task.Coalesce(fallback).Parenthesized
            .ConfigureAwait("capture".ToSimpleName()).Await().Parenthesized.Sub("Length");
        var method = new Method("LengthAsync", new ReturnType(DataType.TaskOf(DataType.Int))).Public.Static.Async
            .AddParameter(new Parameter(DataType.TaskOf(DataType.String).Null, "task"))
            .AddParameter(new Parameter(DataType.TaskOf(DataType.String), "fallback"))
            .AddParameter(new Parameter(DataType.Bool, "capture"))
            .AddStatement(resultLength.Return);
        RoslynTestHelper.CreateCompilation("class Worker { " + method.ToCode() + " }");

        await Assert.That(task.Await().ToCode()).IsEqualTo("await task");
        await Assert.That(task.ConfigureAwait(false).Await().ToCode()).IsEqualTo("await task.ConfigureAwait(false)");
        await Assert.That(task.ConfigureAwait(true).ToCode()).IsEqualTo("task.ConfigureAwait(true)");
        await Assert.That(resultLength.ToCode()).IsEqualTo("(await (task ?? fallback).ConfigureAwait(capture)).Length");
    }

    /// <summary>
    /// A task-like expression can be awaited without requiring a ConfigureAwait method.
    /// </summary>
    [Test]
    public async Task Should_compile_plain_await_and_non_generic_async_return_types()
    {
        var body = "global::System.Threading.Tasks.Task.Yield".ToSimpleName().Invoke().Await();
        var taskMethod = new Method("YieldAsync", new ReturnType(DataType.Task)).Public.Async.AddStatement(body);
        var valueTaskMethod = new Method("YieldValueAsync", new ReturnType(DataType.ValueTask)).Public.Async.AddStatement(body);
        RoslynTestHelper.CreateCompilation("class Worker { " + taskMethod.ToCode() + valueTaskMethod.ToCode() + " }");

        await Assert.That(body.ToCode()).IsEqualTo("await global::System.Threading.Tasks.Task.Yield()");
        taskMethod.IsAsync = false;
        await Assert.That(taskMethod.ToCode().Contains("async ", StringComparison.Ordinal)).IsFalse();
    }

    /// <summary>
    /// Async iteration and disposal compose as statements, including empty embedded statements.
    /// </summary>
    [Test]
    public async Task Should_compile_await_foreach_and_await_using()
    {
        var iteration = "items".ToSimpleName().ForEach(DataType.Int, "item").Await
            .AddStatement("global::System.Console.WriteLine".ToSimpleName().Invoke("item".ToSimpleName()));
        var lifetime = "resource".ToSimpleName().Using.Await.AddStatement(iteration);
        var method = new Method("ConsumeAsync", new ReturnType(DataType.Task)).Public.Async
            .AddParameter(new Parameter(new DataType("global::System.IAsyncDisposable"), "resource"))
            .AddParameter(new Parameter(new DataType("global::System.Collections.Generic.IAsyncEnumerable").Generic(DataType.Int), "items"))
            .AddStatement(lifetime)
            .AddStatement("items".ToSimpleName().ForEach(DataType.Int, "unused").Await)
            .AddStatement("resource".ToSimpleName().Using.Await);
        var code = method.ToCode();
        RoslynTestHelper.CreateCompilation("class Worker { " + code + " }");

        await Assert.That(code).Contains("await using (resource)");
        await Assert.That(code).Contains("await foreach (int item in items)");
        lifetime.IsAwait = false;
        iteration.IsAwait = false;
        await Assert.That(lifetime.ToCode().Contains("await ", StringComparison.Ordinal)).IsFalse();
    }

    /// <summary>
    /// Each task factory owns its mutable wrapper and accepts Roslyn-derived result types.
    /// </summary>
    [Test]
    public async Task Should_create_independent_task_types_from_symbols()
    {
        var compilation = RoslynTestHelper.CreateCompilation("namespace Demo { public class Result { } }");
        var resultType = DataType.FromSymbol(RoslynTestHelper.GetNamedType(compilation, "Demo.Result"));
        var nullableTask = DataType.TaskOf(resultType).Null;
        var taskArray = DataType.Task.Array;
        var valueTaskArray = DataType.ValueTaskOf(resultType).Array;
        var nullableValueTask = DataType.ValueTask.Null;

        await Assert.That(nullableTask.ToCode()).IsEqualTo("global::System.Threading.Tasks.Task<global::Demo.Result>?");
        await Assert.That(taskArray.ToCode()).IsEqualTo("global::System.Threading.Tasks.Task[]");
        await Assert.That(valueTaskArray.ToCode()).IsEqualTo("global::System.Threading.Tasks.ValueTask<global::Demo.Result>[]");
        await Assert.That(nullableValueTask.ToCode()).IsEqualTo("global::System.Threading.Tasks.ValueTask?");
        await Assert.That(DataType.Task.ToCode()).IsEqualTo("global::System.Threading.Tasks.Task");
        await Assert.That(DataType.ValueTask.ToCode()).IsEqualTo("global::System.Threading.Tasks.ValueTask");
        await Assert.That(DataType.TaskOf(resultType).ToCode()).IsEqualTo("global::System.Threading.Tasks.Task<global::Demo.Result>");
        await Assert.That(DataType.ValueTaskOf(resultType).ToCode()).IsEqualTo("global::System.Threading.Tasks.ValueTask<global::Demo.Result>");
        await Assert.That(resultType.ToCode()).IsEqualTo("global::Demo.Result");
    }

    /// <summary>
    /// Positional conveniences preserve order alongside existing named and ref arguments.
    /// </summary>
    [Test]
    public async Task Should_compile_mixed_argument_styles_and_object_creation()
    {
        IExpression[] initial = [1.ToLiteral(), 2.ToLiteral()];
        var call = "Sum".ToSimpleName().Invoke(initial)
            .AddArgument(new Argument("value".ToSimpleName()).Ref)
            .AddArguments(3.ToLiteral())
            .AddArgument(new Argument(4.ToLiteral()) { ParameterName = "last" });
        var creation = new DataType("global::System.Version").New.AddArgument(1.ToLiteral()).AddArguments(2.ToLiteral());
        RoslynTestHelper.CreateCompilation("""
            class Worker {
                static int Sum(int first, int second, ref int value, int fourth, int last) => first + second + value + fourth + last;
                void Run() { int value = 0;
            """ + call.ToCode() + "; var version = " + creation.ToCode() + "; } }");

        await Assert.That(call.ToCode()).IsEqualTo("Sum(1, 2, ref value, 3, last: 4)");
        await Assert.That(creation.ToCode()).IsEqualTo("new global::System.Version(1, 2)");
        await Assert.That("Work".ToSimpleName().Invoke().ToCode()).IsEqualTo("Work()");
        await Assert.That("Work".ToSimpleName().Invoke(Array.Empty<IExpression>()).ToCode()).IsEqualTo("Work()");
    }
}