using TedToolkit.RoslynHelper.Syntaxes;

namespace TedToolkit.RoslynHelper.Tests;

internal sealed class PublicApiEdgeCaseTests
{
    /// <summary>
    /// Verifies that public guard clauses reject null inputs for factory APIs.
    /// </summary>
    [Test]
    public async Task Should_throw_argument_null_for_public_factory_guard_clauses()
    {
        await Assert.That(() => DataType.FromType((Type)null!)).Throws<ArgumentNullException>();
        await Assert.That(() => DataType.FromSymbol(null!)).Throws<ArgumentNullException>();
        await Assert.That(() => Parameter.FromSymbol(null!)).Throws<ArgumentNullException>();
        await Assert.That(() => Parameter.FromInfo(null!)).Throws<ArgumentNullException>();
        await Assert.That(() => TedToolkit.RoslynHelper.Syntaxes.Attribute.FromSymbol(null!)).Throws<ArgumentNullException>();
        await Assert.That(() => new TypeDeclaration("Demo", TypeDeclarationType.CLASS).AddBaseType((Type)null!))
            .Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that DataType.FromType handles by-ref, pointer, array, nested, and generic shapes.
    /// </summary>
    [Test]
    public async Task Should_render_complex_runtime_types_when_creating_data_type_from_type()
    {
        var byRef = TestRenderers.Render(DataType.FromType(typeof(int).MakeByRefType()));
        var pointer = TestRenderers.Render(DataType.FromType(typeof(int*)));
        var array = TestRenderers.Render(DataType.FromType(typeof(Dictionary<string, int>[])));
        var nested = TestRenderers.Render(DataType.FromType(typeof(LocalNestedType.Container.Inner)));

        await Assert.That(byRef).IsEqualTo("int");
        await Assert.That(pointer).IsEqualTo("int*");
        await Assert.That(array).IsEqualTo("global::System.Collections.Generic.Dictionary<string, int>[]");
        await Assert.That(nested).IsEqualTo("global::TedToolkit.RoslynHelper.Tests.LocalNestedType.Container.Inner");
    }

    /// <summary>
    /// Verifies that SourceComposer factories preserve explicit overrides and keyword escaping.
    /// </summary>
    [Test]
    public async Task Should_render_keyword_sensitive_public_api_compositions()
    {
        var parameter = SourceComposer.Parameter(DataType.String, "event").AddDefault("demo".ToLiteral());
        var field = SourceComposer<PublicApiEdgeCaseTests>.Field<int>("class").Private.Const.AddDefault(1.ToLiteral());
        var method = SourceComposer<PublicApiEdgeCaseTests>.Method("namespace")
            .AddParameter(parameter)
            .AddStatement("event".ToSimpleName().Return);

        await Assert.That(TestRenderers.Render(parameter)).IsEqualTo("string @event = \"demo\"");
        await Assert.That(TestRenderers.Render(field)).Contains("private const int @class = 1;");
        await Assert.That(TestRenderers.Render(method)).Contains("void @namespace(");
    }
}

internal static class LocalNestedType
{
    internal static class Container
    {
        internal sealed class Inner
        {
        }
    }
}
