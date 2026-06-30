using System.Reflection;

using TedToolkit.RoslynHelper.Syntaxes;

namespace TedToolkit.RoslynHelper.Tests;

using static SourceComposer;

internal sealed class GeneratorSyntaxTests
{
    /// <summary>
    /// Verifies that accessors render modifiers, attributes, and bodies in declaration order.
    /// </summary>
    [Test]
    public async Task Should_render_accessor_with_modifiers_attributes_and_statements()
    {
        var code = TestRenderers.Render(
            new Accessor(AccessorType.GET)
                .Private
                .Readonly
                .Unsafe
                .AddAttribute(Attribute<ObsoleteAttribute>())
                .AddStatement(10.ToLiteral().Return));

        await Assert.That(code).IsEqualTo(
            "[global::System.ObsoleteAttribute]\nprivate readonly unsafe get\n{\n\treturn 10;\n}");
    }

    /// <summary>
    /// Verifies that arguments render named parameters together with storage modifiers.
    /// </summary>
    [Test]
    public async Task Should_render_argument_with_name_and_storage_kind()
    {
        var code = TestRenderers.Render(
            new Argument("value".ToSimpleName())
            {
                ParameterName = "item",
            }.RefReadonly);

        await Assert.That(code).IsEqualTo("item: ref readonly value");
    }

    /// <summary>
    /// Verifies that attributes support modifiers, positional arguments, and named arguments.
    /// </summary>
    [Test]
    public async Task Should_render_attribute_with_modifier_and_named_arguments()
    {
        var code = TestRenderers.Render(
            Attribute<ObsoleteAttribute>()
                .AddModifier(AttributeModifier.RETURN)
                .AddArgument(Argument("legacy".ToLiteral()))
                .AddNamedArgument("error", true.ToLiteral()));

        await Assert.That(code).IsEqualTo("return:global::System.ObsoleteAttribute(\"legacy\", error = true)");
    }

    /// <summary>
    /// Verifies that catch clauses include the identifier only when one is supplied.
    /// </summary>
    [Test]
    public async Task Should_render_catch_clause_with_optional_identifier()
    {
        var withIdentifier = TestRenderers.Render(
            new CatchClause(DataType.FromType<InvalidOperationException>(), "ex")
                .AddStatement("throw".ToSimpleName()));
        var withoutIdentifier = TestRenderers.Render(
            new CatchClause(DataType.FromType<Exception>())
                .AddStatement("throw".ToSimpleName()));

        await Assert.That(withIdentifier).IsEqualTo(
            "catch(global::System.InvalidOperationException ex)\n{\n\tthrow;\n}");
        await Assert.That(withoutIdentifier).IsEqualTo(
            "catch(global::System.Exception)\n{\n\tthrow;\n}");
    }

    /// <summary>
    /// Verifies that collection elements prepend the spread operator when requested.
    /// </summary>
    [Test]
    public async Task Should_render_collection_element_with_spread_prefix()
    {
        var code = TestRenderers.Render(new CollectionElement("items".ToSimpleName()).Spread);

        await Assert.That(code).IsEqualTo("..items");
    }

    /// <summary>
    /// Verifies that constructor initializers switch between this and base targets.
    /// </summary>
    [Test]
    public async Task Should_render_constructor_initializer_for_this_and_base()
    {
        var thisInitializer = TestRenderers.Render(
            new ConstructorInitializer(false).AddArgument(Argument(1.ToLiteral())));
        var baseInitializer = TestRenderers.Render(
            new ConstructorInitializer(true).AddArgument(Argument(2.ToLiteral())));

        await Assert.That(thisInitializer).IsEqualTo(" : this(1)");
        await Assert.That(baseInitializer).IsEqualTo(" : base(2)");
    }

    /// <summary>
    /// Verifies that custom statements and expressions delegate their output directly to the callback.
    /// </summary>
    [Test]
    public async Task Should_render_custom_statement_and_expression_from_callback()
    {
        var customStatement = TestRenderers.Render(new Custom("yield break;"));
        var customExpression = TestRenderers.Render(new CustomExpression("stackalloc int[4]"));

        await Assert.That(customStatement).IsEqualTo("yield break;");
        await Assert.That(customExpression).IsEqualTo("stackalloc int[4]");
    }

    /// <summary>
    /// Verifies that DataType composes storage modifiers, nullability, arrays, pointers, and generics.
    /// </summary>
    [Test]
    public async Task Should_render_complex_data_type_shapes()
    {
        var nullablePointerArray = TestRenderers.Render(DataType.Int.Null.Array.Pointer);
        var generic = TestRenderers.Render(DataType.FromType<Dictionary<string, int>>());
        var created = TestRenderers.Render(DataType.FromType<int>().New);

        await Assert.That(nullablePointerArray).IsEqualTo("int?[]*");
        await Assert.That(generic).IsEqualTo("global::System.Collections.Generic.Dictionary<string, int>");
        await Assert.That(created).IsEqualTo("new int()");
    }

    /// <summary>
    /// Verifies that enum members include descriptions, attributes, and explicit values.
    /// </summary>
    [Test]
    public async Task Should_render_enum_member_with_metadata_and_value()
    {
        var code = TestRenderers.Render(
            new EnumMember("Ready", 1.ToLiteral())
                .AddRootDescription(new DescriptionSummary(new DescriptionText("State")))
                .AddAttribute(Attribute<ObsoleteAttribute>()));

        await Assert.That(code).IsEqualTo(
            "/// <summary>\n/// State\n/// </summary>\n[global::System.ObsoleteAttribute]\nReady = 1,");
    }

    /// <summary>
    /// Verifies that finally clauses render their statement bodies inside a block.
    /// </summary>
    [Test]
    public async Task Should_render_finally_clause_body()
    {
        var code = TestRenderers.Render(
            new FinallyClause()
                .AddStatement("cleanup".ToSimpleName()));

        await Assert.That(code).IsEqualTo("finally\n{\n\tcleanup;\n}");
    }

    /// <summary>
    /// Verifies that namespaces created from strings reject null input.
    /// </summary>
    [Test]
    public async Task Should_throw_when_namespace_string_is_null()
    {
        await Assert.That(() => new NameSpace((string)null!)).Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that parameters preserve modifiers, defaults, descriptions, and keyword escaping.
    /// </summary>
    [Test]
    public async Task Should_render_parameter_with_keyword_name_and_description()
    {
        var parameter = Parameter(DataType.String.ScopedIn, "class")
            .This
            .AddDefault("demo".ToLiteral())
            .AddDescription(new DescriptionText("The source value."));

        await Assert.That(TestRenderers.Render(parameter)).IsEqualTo("this scoped in string @class = \"demo\"");
        await Assert.That(TestRenderers.RenderRootDescription(parameter.ToRoot())).IsEqualTo(
            "/// <param name=\"class\">\n/// The source value.\n/// </param>\n");
    }

    /// <summary>
    /// Verifies that parameters created from reflection preserve extension and params metadata.
    /// </summary>
    [Test]
    public async Task Should_create_parameter_from_reflection_for_extension_and_params_cases()
    {
        var extensionParameter = typeof(GeneratorSyntaxReflectionTargets)
            .GetMethod(nameof(GeneratorSyntaxReflectionTargets.ExtensionTarget), BindingFlags.Public | BindingFlags.Static)!
            .GetParameters()[0];
        var paramsParameter = typeof(GeneratorSyntaxReflectionTargets)
            .GetMethod(nameof(GeneratorSyntaxReflectionTargets.ParamsTarget), BindingFlags.Public | BindingFlags.Static)!
            .GetParameters()[0];

        await Assert.That(TestRenderers.Render(Parameter(extensionParameter))).IsEqualTo("this string text");
        await Assert.That(TestRenderers.Render(Parameter(paramsParameter))).IsEqualTo("params int[] values");
    }

    /// <summary>
    /// Verifies that return types surface their XML description wrapper.
    /// </summary>
    [Test]
    public async Task Should_render_return_type_description()
    {
        var returnType = ReturnType(DataType.Int).AddDescription(new DescriptionText("The count."));

        await Assert.That(TestRenderers.Render(returnType)).IsEqualTo("int");
        await Assert.That(TestRenderers.RenderRootDescription(returnType.ToRoot())).IsEqualTo(
            "/// <returns>\n/// The count.\n/// </returns>\n");
    }

    /// <summary>
    /// Verifies that switch labels and sections render case clauses and nested statements together.
    /// </summary>
    [Test]
    public async Task Should_render_switch_label_and_section_content()
    {
        var section = new SwitchSection()
            .AddLabel(new SwitchLabel(1.ToLiteral(), "ready".ToSimpleName()))
            .AddLabel(new SwitchLabel())
            .AddStatement("break".ToSimpleName());

        await Assert.That(TestRenderers.Render(new SwitchLabel(2.ToLiteral()))).IsEqualTo("case 2:");
        await Assert.That(TestRenderers.Render(section)).IsEqualTo(
            "\ncase 1 when ready:\ndefault:\n\tbreak;");
    }

    /// <summary>
    /// Verifies that type parameters render storage, descriptions, and multiple constraints.
    /// </summary>
    [Test]
    public async Task Should_render_type_parameter_with_constraints_and_description()
    {
        var typeParameter = TypeParameter("T").In
            .AddDescription(new DescriptionText("The item type."))
            .AddClassConstraint()
            .AddNotNullConstraint()
            .AddConstraint<IDisposable>()
            .AddNewConstraint();
        var declaration = new TypeDeclaration("Box", TypeDeclarationType.CLASS)
            .AddTypeParameter(typeParameter);

        await Assert.That(TestRenderers.Render(typeParameter)).IsEqualTo("in T");
        await Assert.That(TestRenderers.RenderRootDescription(typeParameter.ToRoot())).IsEqualTo(
            "/// <typeparam name=\"T\">\n/// The item type.\n/// </typeparam>\n");
        await Assert.That(TestRenderers.Render(declaration)).Contains(
            "\n\twhere T: class, notnull, global::System.IDisposable, new()");
    }

    /// <summary>
    /// Verifies that variable expressions support const declarations and default assignments.
    /// </summary>
    [Test]
    public async Task Should_render_variable_expression_with_const_and_default()
    {
        var code = TestRenderers.Render(
            new VariableExpression(DataType.Int, "value").Const.AddDefault(10.ToLiteral()));

        await Assert.That(code).IsEqualTo("const int @value = 10");
    }

}

internal static class GeneratorSyntaxReflectionTargets
{
    public static void ExtensionTarget(this string text)
    {
    }

    public static void ParamsTarget(params int[] values)
    {
    }
}
