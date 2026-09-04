using Microsoft.CodeAnalysis;

using TedToolkit.RoslynHelper.Syntaxes;
using TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

namespace TedToolkit.RoslynHelper.Tests;

internal sealed class MemberCompilationTests
{
    /// <summary>
    /// Generic type arguments belong to the owner type, never to its constructor name.
    /// </summary>
    [Test]
    [Arguments("Widget", "Widget`1")]
    [Arguments("class", "class`1")]
    public async Task Should_compile_generic_constructors(string identifier, string metadataName)
    {
        var type = new TypeDeclaration(identifier, TypeDeclarationType.CLASS).Public
            .AddTypeParameter(new TypeParameter("T"))
            .AddMember(new Constructor().Public)
            .AddMember(new ConditionalCompilationMember(PreprocessorExpression.True)
                .AddMember(new Constructor().Public.AddParameter(new Parameter(DataType.Int, "value"))));
        var compilation = RoslynTestHelper.CreateCompilation(type.ToCode());
        var constructors = RoslynTestHelper.GetNamedType(compilation, metadataName).InstanceConstructors;
        await Assert.That(constructors.Length).IsEqualTo(2);
        await Assert.That(constructors.All(constructor => constructor.DeclaredAccessibility == Microsoft.CodeAnalysis.Accessibility.Public)).IsTrue();
    }

    /// <summary>
    /// Abstract declarations use semicolons while concrete empty methods retain their bodies.
    /// </summary>
    [Test]
    public async Task Should_compile_abstract_and_concrete_methods()
    {
        var type = new TypeDeclaration("Worker", TypeDeclarationType.CLASS).Public.Abstract
            .AddMember(new Method("Read", new ReturnType(DataType.Int)).Public.Abstract)
            .AddMember(new Method("Reset").Public);
        var compilation = RoslynTestHelper.CreateCompilation(type.ToCode());
        await Assert.That(RoslynTestHelper.GetMethod(compilation, "Worker", "Read").IsAbstract).IsTrue();
        await Assert.That(RoslynTestHelper.GetMethod(compilation, "Worker", "Reset").IsAbstract).IsFalse();
    }

    /// <summary>
    /// Named arguments must escape keywords just as parameter declarations do.
    /// </summary>
    [Test]
    [Arguments("class")]
    [Arguments("event")]
    [Arguments("value")]
    [Arguments("@class")]
    public async Task Should_compile_named_arguments_with_keyword_names(string name)
    {
        var parameter = new Parameter(DataType.Int, name);
        var method = new Method("Echo", new ReturnType(DataType.Int)).Static
            .AddParameter(parameter).AddStatement(parameter.Name.Return);
        var invocation = "Echo".ToSimpleName().Invoke().AddArgument(new Argument(42.ToLiteral()) { ParameterName = name });
        var caller = new Method("Call", new ReturnType(DataType.Int)).Static.AddStatement(invocation.Return);
        var compilation = RoslynTestHelper.CreateCompilation("class Worker { " + method.ToCode() + caller.ToCode() + " }");
        await Assert.That(RoslynTestHelper.GetMethod(compilation, "Worker", "Echo").Parameters.Single().Name)
            .IsEqualTo(name.TrimStart('@'));
    }
}