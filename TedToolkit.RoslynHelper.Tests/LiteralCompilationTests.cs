using System.Globalization;

using Microsoft.CodeAnalysis;

using TedToolkit.RoslynHelper.Syntaxes;

namespace TedToolkit.RoslynHelper.Tests;

internal sealed class LiteralCompilationTests
{
    /// <summary>
    /// Strings must compile back to their original value, including escape-like text and line separators.
    /// </summary>
    [Test]
    [Arguments("")]
    [Arguments("C:\\temp\\new\\file.cs")]
    [Arguments("\\n is text, not a newline")]
    [Arguments("\"quoted\" and 'single'")]
    [Arguments("first\r\nsecond")]
    [Arguments("\0\a\b\f\t\v")]
    [Arguments("中文😀")]
    [Arguments("line\u0085next\u2028next\u2029end")]
    [Arguments("trailing\\")]
    public async Task Should_round_trip_string_literals(string value)
    {
        var field = GetConstant("string", new LiteralExpression(value));
        await Assert.That(field.ConstantValue).IsEqualTo(value);
    }

    /// <summary>
    /// Character literals require escaping independently of string delimiters.
    /// </summary>
    [Test]
    [Arguments('\'')]
    [Arguments('\\')]
    [Arguments('"')]
    [Arguments('\r')]
    [Arguments('\n')]
    [Arguments('\0')]
    [Arguments('\t')]
    [Arguments('\u2028')]
    [Arguments('\u2029')]
    [Arguments('中')]
    public async Task Should_round_trip_character_literals(char value)
    {
        var field = GetConstant("char", new LiteralExpression(value));
        await Assert.That(field.ConstantValue).IsEqualTo(value);
    }

    /// <summary>
    /// Single-precision literals support finite boundaries and non-finite values.
    /// </summary>
    [Test]
    [Arguments(float.NaN)]
    [Arguments(float.PositiveInfinity)]
    [Arguments(float.NegativeInfinity)]
    [Arguments(float.MaxValue)]
    [Arguments(float.MinValue)]
    [Arguments(float.Epsilon)]
    [Arguments(-0.0f)]
    public async Task Should_round_trip_float_literals(float value)
    {
        var actual = (float)GetConstant("float", new LiteralExpression(value)).ConstantValue!;
        if (float.IsNaN(value))
        {
            await Assert.That(float.IsNaN(actual)).IsTrue();
        }
        else
        {
            await Assert.That(BitConverter.SingleToInt32Bits(actual)).IsEqualTo(BitConverter.SingleToInt32Bits(value));
        }
    }

    /// <summary>
    /// Double-precision literals support finite boundaries and non-finite values.
    /// </summary>
    [Test]
    [Arguments(double.NaN)]
    [Arguments(double.PositiveInfinity)]
    [Arguments(double.NegativeInfinity)]
    [Arguments(double.MaxValue)]
    [Arguments(double.MinValue)]
    [Arguments(double.Epsilon)]
    [Arguments(-0.0d)]
    public async Task Should_round_trip_double_literals(double value)
    {
        var actual = (double)GetConstant("double", new LiteralExpression(value)).ConstantValue!;
        if (double.IsNaN(value))
        {
            await Assert.That(double.IsNaN(actual)).IsTrue();
        }
        else
        {
            await Assert.That(BitConverter.DoubleToInt64Bits(actual)).IsEqualTo(BitConverter.DoubleToInt64Bits(value));
        }
    }

    /// <summary>
    /// Numeric generation is independent of the caller's decimal separator.
    /// </summary>
    [Test]
    [Arguments("fr-FR")]
    [Arguments("de-DE")]
    public async Task Should_preserve_numeric_values_across_cultures(string culture)
    {
        var previous = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(culture);
            await Assert.That(GetConstant("double", new LiteralExpression(1234.5d)).ConstantValue).IsEqualTo(1234.5d);
            await Assert.That(GetConstant("float", new LiteralExpression(1234.5f)).ConstantValue).IsEqualTo(1234.5f);
            await Assert.That(GetConstant("decimal", new LiteralExpression(1234.5m)).ConstantValue).IsEqualTo(1234.5m);
        }
        finally
        {
            CultureInfo.CurrentCulture = previous;
        }
    }

    private static IFieldSymbol GetConstant(string type, IExpression expression)
    {
        var compilation = RoslynTestHelper.CreateCompilation($"class Sample {{ public const {type} Value = {expression.ToCode()}; }}");
        return (IFieldSymbol)RoslynTestHelper.GetNamedType(compilation, "Sample").GetMembers("Value").Single();
    }

    /// <summary>
    /// Integral-looking doubles must remain doubles in overload resolution.
    /// </summary>
    [Test]
    public async Task Should_preserve_double_type_without_a_target_type()
    {
        var code = "class Sample { public object Read() => " + new LiteralExpression(1d).ToCode() + "; }";
        var compilation = RoslynTestHelper.CreateCompilation(code);
        var tree = compilation.SyntaxTrees.Single();
        var expression = tree.GetRoot().DescendantNodes().OfType<Microsoft.CodeAnalysis.CSharp.Syntax.ArrowExpressionClauseSyntax>().Single().Expression;
        await Assert.That(compilation.GetSemanticModel(tree).GetTypeInfo(expression).Type!.SpecialType).IsEqualTo(SpecialType.System_Double);
    }
}