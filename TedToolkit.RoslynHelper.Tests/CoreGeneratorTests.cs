using System.Reflection;

namespace TedToolkit.RoslynHelper.Tests;

using static SourceComposer;
using static SourceComposer<CoreGeneratorTests>;

internal sealed class CoreGeneratorTests
{
    /// <summary>
    /// Verifies that SourceBuilder emits indentation and block delimiters in order.
    /// </summary>
    [Test]
    public async Task Should_render_indented_blocks_when_building_nested_content()
    {
        using var builder = new SourceBuilder();

        builder.Append("if (ready)");
        builder.BeginBlock();
        builder.AppendLine();
        builder.Append("return;");
        builder.EndBlock();

        await Assert.That(TestRenderers.Normalize(builder.ToCode())).IsEqualTo("if (ready)\n{\n\treturn;\n}");
    }

    /// <summary>
    /// Verifies that SourceFile renders attributes before namespaces and preserves member layout.
    /// </summary>
    [Test]
    public async Task Should_render_file_with_attributes_and_namespace_members()
    {
        var code = TestRenderers.Render(
            File()
                .AddAttribute(Attribute<ObsoleteAttribute>())
                .AddNameSpace(NameSpace("Demo.Space")
                    .AddMember(Class("Sample").Public)));

        await Assert.That(code).Contains("[assembly:global::System.ObsoleteAttribute]");
        await Assert.That(code).Contains("namespace Demo.Space");
        await Assert.That(code).Contains(
            "[global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"global::TedToolkit.RoslynHelper.Tests.CoreGeneratorTests\", \"1.0.0.0\")]");
        await Assert.That(code).Contains("public class Sample;");
    }

    /// <summary>
    /// Verifies that SourceComposer namespace overloads produce the same namespace path.
    /// </summary>
    [Test]
    public async Task Should_render_same_namespace_when_using_string_and_span_overloads()
    {
        var fromString = TestRenderers.Render(NameSpace("One.Two"));
        var parts = new[] { "One", "Two", };
        var fromSpan = TestRenderers.Render(NameSpace(parts));

        await Assert.That(fromString).IsEqualTo(fromSpan);
    }

    /// <summary>
    /// Verifies that generic SourceComposer factories stamp generated members with a GeneratedCode attribute.
    /// </summary>
    [Test]
    [Arguments("Class")]
    [Arguments("Struct")]
    [Arguments("Method")]
    [Arguments("Property")]
    [Arguments("Field")]
    public async Task Should_add_generator_attribute_when_using_generic_source_composer(string factoryName)
    {
        var code = factoryName switch
        {
            "Class" => TestRenderers.Render(Class("Example")),
            "Struct" => TestRenderers.Render(Struct("Example")),
            "Method" => TestRenderers.Render(Method("Example")),
            "Property" => TestRenderers.Render(Property<int>("Example")),
            "Field" => TestRenderers.Render(Field<int>("Example")),
            _ => throw new InvalidOperationException(factoryName),
        };

        await Assert.That(code).Contains(
            "[global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"global::TedToolkit.RoslynHelper.Tests.CoreGeneratorTests\", \"1.0.0.0\")]");
    }

    /// <summary>
    /// Verifies that string helper methods escape keywords and string literals for generated code.
    /// </summary>
    [Test]
    public async Task Should_escape_keywords_and_literals_when_normalizing_strings()
    {
        await Assert.That("class".ToValidIdentifier()).IsEqualTo("@class");
        await Assert.That("say \"hi\"\n".ToValidLiteral()).IsEqualTo("say \\\"hi\\\"\\n");
        await Assert.That('\n'.ToValidLiteral()).IsEqualTo("\\n");
    }

    /// <summary>
    /// Verifies that GetToolName and GetVersion resolve metadata from a generator type.
    /// </summary>
    [Test]
    public async Task Should_resolve_generator_metadata_from_type_extensions()
    {
        await Assert.That(typeof(CoreGeneratorTests).GetToolName())
            .IsEqualTo("global::TedToolkit.RoslynHelper.Tests.CoreGeneratorTests");
        await Assert.That(typeof(CoreGeneratorTests).GetVersion()).IsEqualTo("1.0.0.0");
    }

    /// <summary>
    /// Verifies that ToCode throws for null items instead of silently succeeding.
    /// </summary>
    [Test]
    public async Task Should_throw_when_rendering_null_code_item()
    {
        IToCode? item = null;

        await Assert.That(() => item!.ToCode()).Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that Parameter and Argument factory methods preserve ref and default metadata from reflection.
    /// </summary>
    [Test]
    public async Task Should_create_parameter_and_argument_from_parameter_info()
    {
        var parameterInfo = typeof(CoreGeneratorReflectionTargets)
            .GetMethod(nameof(CoreGeneratorReflectionTargets.ParameterTarget), BindingFlags.Public | BindingFlags.Static)!
            .GetParameters()[0];
        var argumentInfo = typeof(CoreGeneratorReflectionTargets)
            .GetMethod(nameof(CoreGeneratorReflectionTargets.ArgumentTarget), BindingFlags.Public | BindingFlags.Static)!
            .GetParameters()[0];

        var parameter = TestRenderers.Render(Parameter(parameterInfo));
        var argument = TestRenderers.Render(Argument(argumentInfo));

        await Assert.That(parameter).IsEqualTo("int @value = 10");
        await Assert.That(argument).IsEqualTo("ref value");
    }
}

internal static class CoreGeneratorReflectionTargets
{
    public static void ParameterTarget(int value = 10)
    {
    }

    public static void ArgumentTarget(ref int value)
    {
        value++;
    }
}
