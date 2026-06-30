using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Tests;

internal sealed class GeneratorExpressionTests
{
    /// <summary>
    /// Verifies that simple wrapper expressions preserve their expected operator placement.
    /// </summary>
    [Test]
    public async Task Should_render_wrapper_expressions_with_expected_operator_order()
    {
        await Assert.That(TestRenderers.Render(new AliasExpression("global", "Value".ToSimpleName())))
            .IsEqualTo("global::Value");
        await Assert.That(TestRenderers.Render(new MemberAccessExpression("left".ToSimpleName(), "Right".ToSimpleName())))
            .IsEqualTo("left.Right");
        await Assert.That(TestRenderers.Render(new BinaryExpression("+", 1.ToLiteral(), 2.ToLiteral())))
            .IsEqualTo("1 + 2");
        await Assert.That(TestRenderers.Render(new CastExpression(DataType.Int, "value".ToSimpleName())))
            .IsEqualTo("(int)value");
        await Assert.That(TestRenderers.Render(new NullExpression("value".ToSimpleName())))
            .IsEqualTo("value?");
        await Assert.That(TestRenderers.Render(new NotExpression("ready".ToSimpleName())))
            .IsEqualTo("!ready");
        await Assert.That(TestRenderers.Render(new RefExpression("item".ToSimpleName())))
            .IsEqualTo("ref item");
        await Assert.That(TestRenderers.Render(new RefReadonlyExpression("item".ToSimpleName())))
            .IsEqualTo("ref readonly item");
        await Assert.That(TestRenderers.Render(new PrefixUnaryExpression("-", 1.ToLiteral())))
            .IsEqualTo("- 1");
        await Assert.That(TestRenderers.Render(new PostfixUnaryExpression("count".ToSimpleName(), "++")))
            .IsEqualTo("count ++");
        await Assert.That(TestRenderers.Render(new ParenthesizedExpression("value".ToSimpleName())))
            .IsEqualTo("(value)");
    }

    /// <summary>
    /// Verifies that literal expressions escape supported primitive values correctly.
    /// </summary>
    [Test]
    public async Task Should_render_literal_expressions_for_supported_primitive_types()
    {
        await Assert.That(TestRenderers.Render(new LiteralExpression("line\n\"quote\"")))
            .IsEqualTo("\"line\\n\\\"quote\\\"\"");
        await Assert.That(TestRenderers.Render(new LiteralExpression('\n')))
            .IsEqualTo("'\\n'");
        await Assert.That(TestRenderers.Render(new LiteralExpression((byte)1))).IsEqualTo("1");
        await Assert.That(TestRenderers.Render(new LiteralExpression((sbyte)2))).IsEqualTo("2");
        await Assert.That(TestRenderers.Render(new LiteralExpression((short)3))).IsEqualTo("3");
        await Assert.That(TestRenderers.Render(new LiteralExpression((ushort)4))).IsEqualTo("4");
        await Assert.That(TestRenderers.Render(new LiteralExpression(5))).IsEqualTo("5");
        await Assert.That(TestRenderers.Render(new LiteralExpression(6U))).IsEqualTo("6");
        await Assert.That(TestRenderers.Render(new LiteralExpression(7L))).IsEqualTo("7");
        await Assert.That(TestRenderers.Render(new LiteralExpression(8UL))).IsEqualTo("8");
        await Assert.That(TestRenderers.Render(new LiteralExpression(true))).IsEqualTo("true");
    }

    /// <summary>
    /// Verifies that invocation and object creation expressions render arguments and initializers together.
    /// </summary>
    [Test]
    public async Task Should_render_invocation_and_object_creation_expressions()
    {
        var invocation = new InvocationExpression("Run".ToSimpleName())
            .AddArgument(new Argument(1.ToLiteral()))
            .AddArgument(new Argument("name".ToLiteral())
            {
                ParameterName = "label",
            });
        var objectCreation = new ObjectCreationExpression(DataType.FromType<List<int>>())
            .AddArgument(new Argument(10.ToLiteral()))
            .AddVariable("Capacity", 10.ToLiteral())
            .AddVariable("Count", 1.ToLiteral());

        await Assert.That(TestRenderers.Render(invocation)).IsEqualTo("Run(1, label: \"name\")");
        await Assert.That(TestRenderers.Render(objectCreation)).IsEqualTo(
            "new global::System.Collections.Generic.List<int>(10)\n{\n\tCapacity = 10,\n\tCount = 1,\n}");
    }

    /// <summary>
    /// Verifies that collection and tuple expressions keep their element ordering.
    /// </summary>
    [Test]
    public async Task Should_render_collection_and_tuple_expressions()
    {
        var collection = new CollectionExpression()
            .AddElement(1.ToLiteral())
            .AddElement(new CollectionElement("others".ToSimpleName()).Spread);
        var tuple = new TupleExpression()
            .AddItem(DataType.Int, "count")
            .AddItem("name".ToSimpleName());

        await Assert.That(TestRenderers.Render(collection)).IsEqualTo("[\n\t1,\n\t..others,\n]");
        await Assert.That(TestRenderers.Render(tuple)).IsEqualTo("(int count, name)");
    }

    /// <summary>
    /// Verifies that type parameter expressions use angle brackets in code and braces in cref output.
    /// </summary>
    [Test]
    public async Task Should_render_type_parameter_expression_for_code_and_cref()
    {
        var expression = new TypeParameterExpression("Task".ToSimpleName(), DataType.Int, DataType.String);
        var builder = new SourceBuilder();

        try
        {
            expression.ToCref(ref builder);

            await Assert.That(TestRenderers.Render(expression)).IsEqualTo("Task<int, string>");
            await Assert.That(TestRenderers.Normalize(builder.ToCode())).IsEqualTo("Task{int, string}");
        }
        finally
        {
            builder.Dispose();
        }
    }

    /// <summary>
    /// Verifies that throw and simple name expressions expose their direct textual form.
    /// </summary>
    [Test]
    public async Task Should_render_throw_and_simple_name_expressions()
    {
        await Assert.That(TestRenderers.Render(SimpleNameExpression.Null)).IsEqualTo("null");
        await Assert.That(TestRenderers.Render(SimpleNameExpression.Default)).IsEqualTo("default");
        await Assert.That(TestRenderers.Render(new ThrowExpression())).IsEqualTo("throw");
        await Assert.That(TestRenderers.Render(new ThrowExpression("error".ToSimpleName()))).IsEqualTo("throw error");
    }
}
