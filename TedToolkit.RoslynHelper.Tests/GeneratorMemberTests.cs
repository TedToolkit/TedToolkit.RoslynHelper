using TedToolkit.RoslynHelper.Syntaxes;
using TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

using Delegate = TedToolkit.RoslynHelper.Syntaxes.Delegate;
using Enum = TedToolkit.RoslynHelper.Syntaxes.Enum;

namespace TedToolkit.RoslynHelper.Tests;

internal sealed class GeneratorMemberTests
{
    /// <summary>
    /// Verifies that type declarations render kind, base types, parameters, and nested members.
    /// </summary>
    [Test]
    public async Task Should_render_type_declaration_variants()
    {
        var typeDeclaration = new TypeDeclaration("Sample", TypeDeclarationType.REF_STRUCT)
            .Public
            .Readonly
            .Partial
            .AddBaseType<IDisposable>()
            .AddTypeParameter(new TypeParameter("T").AddStructConstraint())
            .AddParameter(new Parameter(DataType.Int, "count").AddDefault(1.ToLiteral()))
            .AddMember(new Field(DataType.Int, "count"));

        var record = new TypeDeclaration("Payload", TypeDeclarationType.RECORD_STRUCT);
        var @interface = new TypeDeclaration("IWorker", TypeDeclarationType.INTERFACE);

        await Assert.That(TestRenderers.Render(typeDeclaration)).Contains("public readonly ref partial struct Sample<");
        await Assert.That(TestRenderers.Render(typeDeclaration)).Contains("int count = 1)");
        await Assert.That(TestRenderers.Render(typeDeclaration)).Contains("global::System.IDisposable");
        await Assert.That(TestRenderers.Render(typeDeclaration)).Contains("where T: struct");
        await Assert.That(TestRenderers.Render(record)).IsEqualTo("record struct Payload;");
        await Assert.That(TestRenderers.Render(@interface)).IsEqualTo("interface IWorker;");
    }

    /// <summary>
    /// Verifies that type declarations can be wrapped in conditional compilation directives.
    /// </summary>
    [Test]
    public async Task Should_render_type_declaration_conditional_compilation()
    {
        var typeDeclaration = new TypeDeclaration("Sample", TypeDeclarationType.CLASS)
            .AddCondition(PreprocessorExpression.Debug)
            .AddMember(new Field(DataType.Int, "count"));

        await Assert.That(TestRenderers.Render(typeDeclaration)).IsEqualTo(
            "#if DEBUG\nclass Sample\n{\n\tint count;\n}\n#endif");
    }

    /// <summary>
    /// Verifies that base types can be rendered with per-item conditional compilation directives.
    /// </summary>
    [Test]
    public async Task Should_render_type_declaration_base_types_with_conditions()
    {
        var typeDeclaration = new TypeDeclaration("Sample", TypeDeclarationType.CLASS)
            .AddBaseType<IDisposable>(condition: PreprocessorExpression.Debug)
            .AddBaseType(DataType.FromType<IAsyncDisposable>());

        await Assert.That(TestRenderers.Render(typeDeclaration)).IsEqualTo(
            "class Sample :\n#if DEBUG\n\tglobal::System.IDisposable,\n#endif\n\tglobal::System.IAsyncDisposable;");
    }

    /// <summary>
    /// Verifies that methods render their descriptions, modifiers, parameters, and bodies.
    /// </summary>
    [Test]
    public async Task Should_render_methods_and_delegates()
    {
        var method = new Method("Run", new ReturnType(DataType.Int))
            .Public
            .Static
            .Readonly
            .Virtual
            .AddParameter(new Parameter(DataType.Int, "count"))
            .AddTypeParameter(new TypeParameter("T").AddClassConstraint())
            .AddRootDescription(new DescriptionSummary(new DescriptionText("Executes.")))
            .AddStatement(1.ToLiteral().Return);
        var partialMethod = new Method("Partial").Partial;
        var externMethod = new Method("Import").Extern;
        var @delegate = new Delegate("Handler", new ReturnType(DataType.Bool))
            .Public
            .Unsafe
            .AddParameter(new Parameter(DataType.String, "message"));

        await Assert.That(TestRenderers.Render(method)).Contains("public static readonly virtual int Run<");
        await Assert.That(TestRenderers.Render(method)).Contains("int count)");
        await Assert.That(TestRenderers.Render(method)).Contains("where T: class");
        await Assert.That(TestRenderers.Render(method)).Contains("return 1;");
        await Assert.That(TestRenderers.Render(partialMethod)).IsEqualTo("partial void Partial();");
        await Assert.That(TestRenderers.Render(externMethod)).IsEqualTo("extern void Import();");
        await Assert.That(TestRenderers.Render(@delegate)).IsEqualTo("public unsafe delegate bool Handler(\n\tstring message);");
    }

    /// <summary>
    /// Verifies that methods can be wrapped in conditional compilation directives.
    /// </summary>
    [Test]
    public async Task Should_render_method_conditional_compilation()
    {
        var method = new Method("Run")
            .AddCondition(PreprocessorExpression.Debug)
            .AddStatement("Execute".ToSimpleName());

        await Assert.That(TestRenderers.Render(method)).IsEqualTo(
            "#if DEBUG\nvoid Run()\n{\n\tExecute;\n}\n#endif");
    }

    /// <summary>
    /// Verifies that constructors, operators, and conversions render against their owner type.
    /// </summary>
    [Test]
    public async Task Should_render_constructor_operator_and_conversion_members()
    {
        var constructor = new Constructor
        {
            Owner = "Widget",
        }.Public.AddInitializer(new ConstructorInitializer(false))
            .AddStatement("init".ToSimpleName());
        var @operator = new Operator(new ReturnType(DataType.Int), "+")
            .AddParameter(new Parameter(DataType.Int, "left"))
            .AddParameter(new Parameter(DataType.Int, "right"))
            .AddStatement(0.ToLiteral().Return);
        var conversion = new Conversion(DataType.String, false, true)
        {
            Owner = "Widget",
        }.RefReadonly
            .AddStatement("value".ToSimpleName().Return);

        await Assert.That(TestRenderers.Render(constructor)).IsEqualTo("public Widget() : this()\n{\n\tinit;\n}");
        await Assert.That(TestRenderers.Render(@operator)).IsEqualTo(
            "public static int operator +(\n\tint left,\n\tint right)\n{\n\treturn 0;\n}");
        await Assert.That(TestRenderers.Render(conversion)).IsEqualTo(
            "public static implicit operator string(ref readonly Widget value)\n{\n\treturn value;\n}");
    }

    /// <summary>
    /// Verifies that field, property, event, and indexer members include their modifiers and bodies.
    /// </summary>
    [Test]
    public async Task Should_render_stateful_members()
    {
        var field = new Field(DataType.Int, "count").Internal.Static.Readonly.AddDefault(1.ToLiteral());
        var property = new Property(DataType.String, "Name").Protected.Override
            .AddAccessor(new Accessor(AccessorType.GET))
            .AddDefault("demo".ToLiteral());
        var @event = new Event(DataType.FromType<Action>(), "Changed").Private.Static
            .AddAccessor(new Accessor(AccessorType.ADD).AddStatement("subscribe".ToSimpleName()))
            .AddAccessor(new Accessor(AccessorType.REMOVE).AddStatement("unsubscribe".ToSimpleName()));
        var indexer = new Indexer(DataType.Int).Internal.Readonly
            .AddParameter(new Parameter(DataType.Int, "index"))
            .AddAccessor(new Accessor(AccessorType.GET).AddStatement(1.ToLiteral().Return));

        await Assert.That(TestRenderers.Render(field)).IsEqualTo("internal static readonly int count = 1;");
        await Assert.That(TestRenderers.Render(property)).Contains("protected override string Name");
        await Assert.That(TestRenderers.Render(property)).Contains("} = \"demo\";");
        await Assert.That(TestRenderers.Render(@event)).Contains("private static event global::System.Action Changed");
        await Assert.That(TestRenderers.Render(indexer)).IsEqualTo(
            "internal readonly int this[\n\tint index]\n{\n\tget\n\t{\n\t\treturn 1;\n\t}\n}");
    }

    /// <summary>
    /// Verifies that stateful members can be wrapped in conditional compilation directives.
    /// </summary>
    [Test]
    public async Task Should_render_conditional_compilation_for_stateful_members()
    {
        var field = new Field(DataType.Int, "count")
            .AddCondition(PreprocessorExpression.Debug);

        await Assert.That(TestRenderers.Render(field)).IsEqualTo("#if DEBUG\nint count;\n#endif");
    }

    /// <summary>
    /// Verifies that member conditional compilation blocks render full if/elif/else chains.
    /// </summary>
    [Test]
    public async Task Should_render_member_conditional_compilation_with_else_if_and_else()
    {
        var block = new ConditionalCompilationMember(PreprocessorExpression.Debug)
            .AddMember(new Field(DataType.Int, "count"))
            .ElseIf(PreprocessorExpression.Trace)
            .AddMember(new Field(DataType.Int, "traceCount"))
            .Else()
            .AddMember(new Field(DataType.Int, "fallbackCount"));

        await Assert.That(TestRenderers.Render(block)).IsEqualTo(
            "#if DEBUG\nint count;\n#elif TRACE\nint traceCount;\n#else\nint fallbackCount;\n#endif");
    }

    /// <summary>
    /// Verifies that member conditional compilation blocks render an else branch without elif branches.
    /// </summary>
    [Test]
    public async Task Should_render_member_conditional_compilation_with_else_only()
    {
        var block = new ConditionalCompilationMember(PreprocessorExpression.Debug)
            .AddMember(new Field(DataType.Int, "count"))
            .Else()
            .AddMember(new Field(DataType.Int, "fallbackCount"));

        await Assert.That(TestRenderers.Render(block)).IsEqualTo(
            "#if DEBUG\nint count;\n#else\nint fallbackCount;\n#endif");
    }

    /// <summary>
    /// Verifies that member conditional compilation blocks reject invalid branch ordering.
    /// </summary>
    [Test]
    public async Task Should_reject_invalid_member_conditional_compilation_branch_order()
    {
        await Assert.That(() => new ConditionalCompilationMember(PreprocessorExpression.Debug)
                .Else()
                .ElseIf(PreprocessorExpression.Trace))
            .Throws<InvalidOperationException>();

        await Assert.That(() => new ConditionalCompilationMember(PreprocessorExpression.Debug)
                .Else()
                .Else())
            .Throws<InvalidOperationException>();
    }

    /// <summary>
    /// Verifies that member conditional compilation blocks preserve owner-sensitive nested members.
    /// </summary>
    [Test]
    public async Task Should_propagate_owner_into_member_conditional_compilation_block()
    {
        var type = new TypeDeclaration("Widget", TypeDeclarationType.CLASS)
            .AddMember(new ConditionalCompilationMember(PreprocessorExpression.Debug)
                .AddMember(new Constructor()
                    .AddStatement("init".ToSimpleName())));

        await Assert.That(TestRenderers.Render(type)).IsEqualTo(
            "class Widget\n{\n\t#if DEBUG\n\tWidget()\n\t{\n\t\tinit;\n\t}\n\t#endif\n}");
    }

    /// <summary>
    /// Verifies that member conditional compilation blocks preserve member spacing within each branch.
    /// </summary>
    [Test]
    public async Task Should_render_multiple_members_inside_conditional_compilation_branch()
    {
        var block = new ConditionalCompilationMember(PreprocessorExpression.Debug)
            .AddMember(new Field(DataType.Int, "count"))
            .AddMember(new Property(DataType.String, "Name")
                .AddAccessor(new Accessor(AccessorType.GET)))
            .Else()
            .AddMember(new Field(DataType.Int, "fallbackCount"))
            .AddMember(new Method("Fallback")
                .AddStatement("fallbackCount".ToSimpleName().Return));

        await Assert.That(TestRenderers.Render(block)).IsEqualTo(
            "#if DEBUG\nint count;\n\nstring Name\n{\n\tget;\n}\n#else\nint fallbackCount;\n\nvoid Fallback()\n{\n\treturn fallbackCount;\n}\n#endif");
    }

    /// <summary>
    /// Verifies that enum and extension members render nested payloads and constraints.
    /// </summary>
    [Test]
    public async Task Should_render_enum_and_extension_members()
    {
        var @enum = new Enum("State", DataType.Int).Public
            .AddEnumMember(new EnumMember("Ready", 1.ToLiteral()))
            .AddEnumMember(new EnumMember("Done"));
        var extension = new Extension(new Parameter(DataType.String, "text").This)
            .AddTypeParameter(new TypeParameter("T").AddNotNullConstraint())
            .AddMember(new Method("Trimmed").AddStatement("text".ToSimpleName().Return));

        await Assert.That(TestRenderers.Render(@enum)).Contains("public enum State : int");
        await Assert.That(TestRenderers.Render(@enum)).Contains("Ready = 1,");
        await Assert.That(TestRenderers.Render(@enum)).Contains("Done,");
        await Assert.That(TestRenderers.Render(extension)).Contains("extension<");
        await Assert.That(TestRenderers.Render(extension)).Contains(">(this string text)");
        await Assert.That(TestRenderers.Render(extension)).Contains("where T: notnull");
        await Assert.That(TestRenderers.Render(extension)).Contains("return text;");
    }
}
