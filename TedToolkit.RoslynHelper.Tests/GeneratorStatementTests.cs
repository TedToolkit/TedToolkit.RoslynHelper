using TedToolkit.RoslynHelper.Syntaxes;
using TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

namespace TedToolkit.RoslynHelper.Tests;

internal sealed class GeneratorStatementTests
{
    /// <summary>
    /// Verifies that statement wrappers add semicolons around raw expressions.
    /// </summary>
    [Test]
    public async Task Should_render_basic_statement_shapes()
    {
        await Assert.That(TestRenderers.Render(new Statement("work".ToSimpleName()))).IsEqualTo("work;");
        await Assert.That(TestRenderers.Render(new ReturnStatement())).IsEqualTo("return;");
        await Assert.That(TestRenderers.Render(new ReturnStatement(1.ToLiteral()))).IsEqualTo("return 1;");
    }

    /// <summary>
    /// Verifies that conditional, using, and foreach statements render their bodies as blocks.
    /// </summary>
    [Test]
    public async Task Should_render_block_based_statements()
    {
        var ifStatement = new IfStatement("ready".ToSimpleName())
            .AddStatement("return".ToSimpleName());
        var usingStatement = new UsingStatement("resource".ToSimpleName())
            .AddStatement("dispose".ToSimpleName());
        var foreachStatement = new ForEachStatement(DataType.Var, "item", "items".ToSimpleName())
            .AddStatement("yield".ToSimpleName());

        await Assert.That(TestRenderers.Render(ifStatement)).IsEqualTo("if (ready)\n{\n\treturn;\n}");
        await Assert.That(TestRenderers.Render(usingStatement)).IsEqualTo("using (resource)\n{\n\tdispose;\n}");
        await Assert.That(TestRenderers.Render(foreachStatement)).IsEqualTo("foreach (var item in items)\n{\n\tyield;\n}");
    }

    /// <summary>
    /// Verifies that if statements can render chained else-if and else branches.
    /// </summary>
    [Test]
    public async Task Should_render_if_statement_with_else_if_and_else()
    {
        var statement = new IfStatement("ready".ToSimpleName())
            .AddStatement("work".ToSimpleName())
            .ElseIf("retry".ToSimpleName())
            .AddStatement("retryWork".ToSimpleName())
            .Else()
            .AddStatement("fallback".ToSimpleName());

        await Assert.That(TestRenderers.Render(statement)).IsEqualTo(
            "if (ready)\n{\n\twork;\n}\nelse if (retry)\n{\n\tretryWork;\n}\nelse\n{\n\tfallback;\n}");
    }

    /// <summary>
    /// Verifies that if statements can render a trailing else branch without else-if branches.
    /// </summary>
    [Test]
    public async Task Should_render_if_statement_with_else_only()
    {
        var statement = new IfStatement("ready".ToSimpleName())
            .AddStatement("work".ToSimpleName())
            .Else()
            .AddStatement("fallback".ToSimpleName());

        await Assert.That(TestRenderers.Render(statement)).IsEqualTo(
            "if (ready)\n{\n\twork;\n}\nelse\n{\n\tfallback;\n}");
    }

    /// <summary>
    /// Verifies that try statements combine try, catch, and finally sections in order.
    /// </summary>
    [Test]
    public async Task Should_render_try_statement_with_catches_and_finally()
    {
        var statement = new TryStatement()
            .AddStatement("work".ToSimpleName())
            .AddCatch(new CatchClause(DataType.FromType<Exception>(), "ex").AddStatement("throw".ToSimpleName()))
            .AddFinally(new FinallyClause().AddStatement("cleanup".ToSimpleName()));

        await Assert.That(TestRenderers.Render(statement)).IsEqualTo(
            "try\n{\n\twork;\n}\ncatch(global::System.Exception ex)\n{\n\tthrow;\n}\nfinally\n{\n\tcleanup;\n}");
    }

    /// <summary>
    /// Verifies that switch statements preserve section breaks between multiple sections.
    /// </summary>
    [Test]
    public async Task Should_render_switch_statement_with_multiple_sections()
    {
        var statement = new SwitchStatement("value".ToSimpleName())
            .AddSection(new SwitchSection()
                .AddLabel(new SwitchLabel(1.ToLiteral()))
                .AddStatement("break".ToSimpleName()))
            .AddSection(new SwitchSection()
                .AddLabel(new SwitchLabel())
                .AddStatement(new ReturnStatement()));

        await Assert.That(TestRenderers.Render(statement)).IsEqualTo(
            "switch (value)\n{\n\tcase 1:\n\t\tbreak;\n\t\n\tdefault:\n\t\treturn;\n}");
    }

    /// <summary>
    /// Verifies that the simple statement wrapper can be surrounded by conditional compilation.
    /// </summary>
    [Test]
    public async Task Should_render_conditional_compilation_statement()
    {
        var statement = new Statement("work".ToSimpleName())
            .AddCondition(PreprocessorExpression.Debug);

        await Assert.That(TestRenderers.Render(statement)).IsEqualTo("#if DEBUG\nwork;\n#endif");
    }

    /// <summary>
    /// Verifies that block-based statements can also be surrounded by conditional compilation.
    /// </summary>
    [Test]
    public async Task Should_render_conditional_compilation_block_statement()
    {
        var statement = new IfStatement("ready".ToSimpleName())
            .AddCondition(PreprocessorExpression.Debug)
            .AddStatement("work".ToSimpleName());

        await Assert.That(TestRenderers.Render(statement)).IsEqualTo(
            "#if DEBUG\nif (ready)\n{\n\twork;\n}\n#endif");
    }

    /// <summary>
    /// Verifies that statement conditional compilation blocks render full if/elif/else chains.
    /// </summary>
    [Test]
    public async Task Should_render_statement_conditional_compilation_with_else_if_and_else()
    {
        var statement = new ConditionalCompilationStatement(PreprocessorExpression.Debug)
            .AddStatement("work".ToSimpleName())
            .ElseIf(PreprocessorExpression.Trace)
            .AddStatement("trace".ToSimpleName())
            .Else()
            .AddStatement("fallback".ToSimpleName());

        await Assert.That(TestRenderers.Render(statement)).IsEqualTo(
            "#if DEBUG\nwork;\n#elif TRACE\ntrace;\n#else\nfallback;\n#endif");
    }

    /// <summary>
    /// Verifies that statement conditional compilation blocks render an else branch without elif branches.
    /// </summary>
    [Test]
    public async Task Should_render_statement_conditional_compilation_with_else_only()
    {
        var statement = new ConditionalCompilationStatement(PreprocessorExpression.Debug)
            .AddStatement("work".ToSimpleName())
            .Else()
            .AddStatement("fallback".ToSimpleName());

        await Assert.That(TestRenderers.Render(statement)).IsEqualTo(
            "#if DEBUG\nwork;\n#else\nfallback;\n#endif");
    }

    /// <summary>
    /// Verifies that statement conditional compilation blocks reject invalid branch ordering.
    /// </summary>
    [Test]
    public async Task Should_reject_invalid_statement_conditional_compilation_branch_order()
    {
        await Assert.That(() => new ConditionalCompilationStatement(PreprocessorExpression.Debug)
                .Else()
                .ElseIf(PreprocessorExpression.Trace))
            .Throws<InvalidOperationException>();

        await Assert.That(() => new ConditionalCompilationStatement(PreprocessorExpression.Debug)
                .Else()
                .Else())
            .Throws<InvalidOperationException>();
    }

    /// <summary>
    /// Verifies that statement conditional compilation blocks preserve normal statement rendering within each branch.
    /// </summary>
    [Test]
    public async Task Should_render_multiple_statements_inside_conditional_compilation_branch()
    {
        var statement = new ConditionalCompilationStatement(PreprocessorExpression.Debug)
            .AddStatement("work".ToSimpleName())
            .AddStatement(new IfStatement("ready".ToSimpleName())
                .AddStatement("return".ToSimpleName()))
            .Else()
            .AddStatement(new ReturnStatement())
            .AddStatement("fallback".ToSimpleName());

        await Assert.That(TestRenderers.Render(statement)).IsEqualTo(
            "#if DEBUG\nwork;\nif (ready)\n{\n\treturn;\n}\n#else\nreturn;\nfallback;\n#endif");
    }

    /// <summary>
    /// Verifies that all concrete statements participate in the shared conditional compilation abstraction.
    /// </summary>
    [Test]
    public async Task Should_have_all_statement_types_derive_from_conditional_compilation_syntax()
    {
        var statementTypes = typeof(Statement).Assembly.GetTypes()
            .Where(type => type is { IsAbstract: false, IsClass: true } &&
                           type.Namespace == typeof(Statement).Namespace &&
                           typeof(IStatement).IsAssignableFrom(type));
        var allSupportConditionalCompilation = statementTypes.All(type =>
            typeof(ConditionalCompilationSyntax).IsAssignableFrom(type));

        await Assert.That(allSupportConditionalCompilation).IsTrue();
    }
}
