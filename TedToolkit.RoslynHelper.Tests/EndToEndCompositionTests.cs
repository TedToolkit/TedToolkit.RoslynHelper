using TedToolkit.RoslynHelper.Syntaxes;

namespace TedToolkit.RoslynHelper.Tests;

using static SourceComposer;
using static SourceComposer<EndToEndCompositionTests>;

internal sealed class EndToEndCompositionTests
{
    /// <summary>
    /// Verifies that Roslyn-derived symbols can be composed into a generated source file end to end.
    /// </summary>
    [Test]
    public async Task Should_compose_generated_source_from_roslyn_symbols_end_to_end()
    {
        const string source = """
using System;
using System.Collections.Generic;

namespace Consumer;

[AttributeUsage(AttributeTargets.Method)]
public sealed class DemoAttribute : Attribute
{
    public DemoAttribute(Type kind)
    {
    }
}

public static class Factory
{
    [Demo(typeof(List<int>))]
    public static TResult Create<TResult>(this string text, bool? enabled = null)
        where TResult : class, new()
    {
        throw new NotImplementedException();
    }
}
""";

        var compilation = RoslynTestHelper.CreateCompilation(source, additionalReferences: [RoslynTestHelper.CreateProjectReference()]);
        var methodSymbol = RoslynTestHelper.GetMethod(compilation, "Consumer.Factory", "Create");
        var generatedMethod = SourceComposer<EndToEndCompositionTests>.Method(methodSymbol.Name,
                new ReturnType(DataType.FromSymbol(methodSymbol.ReturnType, compilation)))
            .Public
            .Static
            .AddAttribute(SourceComposer.Attribute(methodSymbol.GetAttributes().Single(), compilation))
            .AddTypeParameter(SourceComposer.TypeParameter(methodSymbol.TypeParameters[0], compilation))
            .AddParameter(SourceComposer.Parameter(methodSymbol.Parameters[0], compilation))
            .AddParameter(SourceComposer.Parameter(methodSymbol.Parameters[1], compilation))
            .AddStatement(new ObjectCreationExpression(DataType.FromSymbol(methodSymbol.ReturnType, compilation)).Return);
        var code = TestRenderers.Render(
            File()
                .AddNameSpace(NameSpace("Generated")
                    .AddMember(SourceComposer<EndToEndCompositionTests>.Class("Factory")
                        .Public
                        .Static
                        .AddMember(generatedMethod))));

        await Assert.That(code).Contains("[global::Consumer.DemoAttribute(typeof(global::System.Collections.Generic.List<int>))]");
        var generatedCompilation = RoslynTestHelper.CreateCompilation(code,
            additionalReferences: [RoslynTestHelper.CreateReferenceFromSource(source, "Consumer")]);
        var generatedSymbol = RoslynTestHelper.GetMethod(generatedCompilation, "Generated.Factory", "Create");
        await Assert.That(generatedSymbol.ReturnType.Name).IsEqualTo("TResult");
        await Assert.That(generatedSymbol.IsExtensionMethod).IsTrue();
        await Assert.That(generatedSymbol.TypeParameters.Single().HasConstructorConstraint).IsTrue();
        await Assert.That(code).Contains("public static TResult Create<");
        await Assert.That(code).Contains("this string text");
        await Assert.That(code).Contains("bool? enabled = default");
        await Assert.That(code).Contains("where TResult: class, new()");
    }
}