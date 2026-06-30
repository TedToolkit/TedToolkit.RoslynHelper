using TedToolkit.RoslynHelper.Syntaxes;

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
}
