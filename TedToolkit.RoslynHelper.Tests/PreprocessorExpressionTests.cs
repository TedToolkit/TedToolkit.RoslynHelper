using TedToolkit.RoslynHelper.Syntaxes;
using TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

namespace TedToolkit.RoslynHelper.Tests;

internal sealed class PreprocessorExpressionTests
{
    [Test]
    public async Task Should_render_simple_symbols_and_literals()
    {
        await Assert.That(TestRenderers.Render(PreprocessorExpression.Symbol("DEBUG")))
            .IsEqualTo("DEBUG");
        await Assert.That(TestRenderers.Render(PreprocessorExpression.Debug))
            .IsEqualTo("DEBUG");
        await Assert.That(TestRenderers.Render(PreprocessorExpression.Trace))
            .IsEqualTo("TRACE");
        await Assert.That(TestRenderers.Render(PreprocessorExpression.True))
            .IsEqualTo("true");
        await Assert.That(TestRenderers.Render(PreprocessorExpression.False))
            .IsEqualTo("false");
    }

    [Test]
    public async Task Should_render_logical_operators_with_csharp_precedence()
    {
        var expression = !PreprocessorExpression.Symbol("DEBUG") && "TRACE" || false;

        await Assert.That(TestRenderers.Render(expression))
            .IsEqualTo("!DEBUG && TRACE || false");
    }

    [Test]
    public async Task Should_add_parentheses_only_when_required_by_precedence()
    {
        var andOverOr = (PreprocessorExpression.Symbol("DEBUG") || "TRACE") && "RELEASE";
        var notOverOr = !(PreprocessorExpression.Symbol("DEBUG") || "TRACE");
        var samePrecedence = (PreprocessorExpression.Symbol("DEBUG") || "TRACE") || "RELEASE";

        await Assert.That(TestRenderers.Render(andOverOr))
            .IsEqualTo("(DEBUG || TRACE) && RELEASE");
        await Assert.That(TestRenderers.Render(notOverOr))
            .IsEqualTo("!(DEBUG || TRACE)");
        await Assert.That(TestRenderers.Render(samePrecedence))
            .IsEqualTo("DEBUG || TRACE || RELEASE");
    }

    [Test]
    public async Task Should_validate_preprocessor_symbol_names()
    {
        await Assert.That(() => PreprocessorExpression.Symbol("1DEBUG"))
            .Throws<ArgumentException>();
        await Assert.That(() => PreprocessorExpression.Symbol(string.Empty))
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task Should_support_implicit_symbol_and_boolean_conversions()
    {
        PreprocessorExpression symbol = "DEBUG";
        PreprocessorExpression enabled = true;
        PreprocessorExpression disabled = false;

        await Assert.That(TestRenderers.Render(symbol))
            .IsEqualTo("DEBUG");
        await Assert.That(TestRenderers.Render(enabled))
            .IsEqualTo("true");
        await Assert.That(TestRenderers.Render(disabled))
            .IsEqualTo("false");
    }

    [Test]
    public async Task Should_expose_predefined_target_framework_symbols()
    {
        await Assert.That(TestRenderers.Render(PreprocessorExpression.Net))
            .IsEqualTo("NET");
        await Assert.That(TestRenderers.Render(PreprocessorExpression.Net100))
            .IsEqualTo("NET10_0");
        await Assert.That(TestRenderers.Render(PreprocessorExpression.Net80OrGreater))
            .IsEqualTo("NET8_0_OR_GREATER");
        await Assert.That(TestRenderers.Render(PreprocessorExpression.NetStandard20))
            .IsEqualTo("NETSTANDARD2_0");
        await Assert.That(TestRenderers.Render(PreprocessorExpression.Windows))
            .IsEqualTo("WINDOWS");
    }

    [Test]
    public async Task Should_render_platform_version_symbols()
    {
        await Assert.That(TestRenderers.Render(PreprocessorExpression.PlatformVersion("ios", "15.1")))
            .IsEqualTo("IOS15_1");
        await Assert.That(TestRenderers.Render(PreprocessorExpression.PlatformVersionOrGreater("windows", "10.0")))
            .IsEqualTo("WINDOWS10_0_OR_GREATER");
    }
}
