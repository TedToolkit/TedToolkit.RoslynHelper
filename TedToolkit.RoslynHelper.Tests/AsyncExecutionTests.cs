using System.Reflection;
using System.Runtime.Loader;

using TedToolkit.RoslynHelper.Syntaxes;

namespace TedToolkit.RoslynHelper.Tests;

internal sealed class AsyncExecutionTests
{
    /// <summary>
    /// Generated Task and ValueTask methods await pending work and preserve results, failures, and cancellation.
    /// </summary>
    [Test]
    [Arguments(false, "success")]
    [Arguments(true, "success")]
    [Arguments(false, "failure")]
    [Arguments(true, "failure")]
    [Arguments(false, "cancellation")]
    [Arguments(true, "cancellation")]
    public async Task Should_execute_generated_async_forwarders(bool valueTask, string outcome)
    {
        var returnType = valueTask ? DataType.ValueTaskOf(DataType.Int) : DataType.TaskOf(DataType.Int);
        var method = new Method("Run", new ReturnType(returnType)).Public.Static.Async
            .AddParameter(new Parameter(DataType.TaskOf(DataType.Int), "pending"))
            .AddStatement("pending".ToSimpleName().ConfigureAwait(false).Await().Return);
        var code = new SourceFile().AddMember(new TypeDeclaration("Worker", TypeDeclarationType.CLASS).Public.Static.AddMember(method)).ToCode();
        var context = new AssemblyLoadContext(Guid.NewGuid().ToString(), isCollectible: true);
        try
        {
            var assembly = Load(context, code);
            var entry = assembly.GetType("Worker")!.GetMethod("Run")!;
            var pending = new TaskCompletionSource<int>(TaskCreationOptions.RunContinuationsAsynchronously);
            var result = valueTask
                ? entry.CreateDelegate<Func<Task<int>, ValueTask<int>>>()(pending.Task).AsTask()
                : entry.CreateDelegate<Func<Task<int>, Task<int>>>()(pending.Task);
            await Assert.That(result.IsCompleted).IsFalse();
            using var cancellation = new CancellationTokenSource();
            var failure = new InvalidOperationException("original failure");

            switch (outcome)
            {
                case "success":
                    pending.SetResult(42);
                    await Assert.That(await result.WaitAsync(TimeSpan.FromSeconds(10))).IsEqualTo(42);
                    break;
                case "failure":
                    pending.SetException(failure);
                    var error = await CaptureFailure(result);
                    await Assert.That(ReferenceEquals(error, failure)).IsTrue();
                    await Assert.That(result.IsFaulted).IsTrue();
                    break;
                case "cancellation":
                    cancellation.Cancel();
                    pending.SetCanceled(cancellation.Token);
                    var canceled = (OperationCanceledException)await CaptureFailure(result);
                    await Assert.That(canceled.CancellationToken).IsEqualTo(cancellation.Token);
                    await Assert.That(result.IsCanceled).IsTrue();
                    break;
            }
        }
        finally
        {
            context.Unload();
        }
    }

    /// <summary>
    /// Generated await foreach and await using enumerate and asynchronously dispose even after failure.
    /// </summary>
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Should_await_resource_disposal_after_async_enumeration(bool fail)
    {
        var sum = "sum".ToSimpleName();
        var method = new Method("Run", new ReturnType(DataType.TaskOf(DataType.Int))).Public.Static.Async
            .AddStatement(new VariableExpression(DataType.Int, "sum").AddDefault(0.ToLiteral()))
            .AddStatement(new VariableExpression(DataType.Var, "resource").AddDefault(new DataType("Resource").New))
            .AddStatement("resource".ToSimpleName().Using.Await
                .AddStatement("resource".ToSimpleName().Sub("Items").Invoke(fail.ToLiteral())
                    .ForEach(DataType.Int, "item").Await.AddStatement(sum.AddAssign("item".ToSimpleName()))))
            .AddStatement(sum.Return);
        var generated = new TypeDeclaration("Worker", TypeDeclarationType.CLASS).Public.Static.AddMember(method).ToCode();
        const string resource = """
            public sealed class Resource : System.IAsyncDisposable
            {
                public static int Disposed;
                public async System.Collections.Generic.IAsyncEnumerable<int> Items(bool fail)
                {
                    await System.Threading.Tasks.Task.Yield();
                    yield return 1;
                    if (fail) throw new System.InvalidOperationException("enumeration failed");
                    yield return 2;
                }
                public async System.Threading.Tasks.ValueTask DisposeAsync()
                {
                    await System.Threading.Tasks.Task.Yield();
                    Disposed++;
                }
            }
            """;
        var context = new AssemblyLoadContext(Guid.NewGuid().ToString(), isCollectible: true);
        try
        {
            var assembly = Load(context, resource + generated);
            var run = assembly.GetType("Worker")!.GetMethod("Run")!.CreateDelegate<Func<Task<int>>>();
            if (fail)
            {
                var error = await CaptureFailure(run());
                await Assert.That(error.Message).IsEqualTo("enumeration failed");
            }
            else
            {
                await Assert.That(await run().WaitAsync(TimeSpan.FromSeconds(10))).IsEqualTo(3);
            }

            await Assert.That((int)assembly.GetType("Resource")!.GetField("Disposed")!.GetValue(null)!).IsEqualTo(1);
        }
        finally
        {
            context.Unload();
        }
    }

    private static Assembly Load(AssemblyLoadContext context, string code)
    {
        var compilation = RoslynTestHelper.CreateCompilation(code, Guid.NewGuid().ToString("N"));
        using var stream = new MemoryStream();
        var result = compilation.Emit(stream);
        if (!result.Success)
        {
            throw new InvalidOperationException(string.Join(Environment.NewLine, result.Diagnostics));
        }

        stream.Position = 0;
        return context.LoadFromStream(stream);
    }

    private static async Task<Exception> CaptureFailure(Task task)
    {
        try
        {
            await task.WaitAsync(TimeSpan.FromSeconds(10));
        }
        catch (Exception error)
        {
            return error;
        }

        throw new InvalidOperationException("Expected the generated method to fail.");
    }
}