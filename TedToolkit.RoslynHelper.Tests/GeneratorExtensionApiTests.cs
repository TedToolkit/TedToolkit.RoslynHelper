using TedToolkit.RoslynHelper.Syntaxes;

namespace TedToolkit.RoslynHelper.Tests;

internal sealed class GeneratorExtensionApiTests
{
    /// <summary>
    /// Verifies that member modifier extensions update the generated declaration order.
    /// </summary>
    [Test]
    public async Task Should_apply_member_modifier_extensions()
    {
        var code = TestRenderers.Render(
            new Method("Run")
                .Public
                .Static
                .Unsafe
                .Partial
                .SealedOverride);

        await Assert.That(code).IsEqualTo("public static sealed override unsafe partial void Run();");
    }

    /// <summary>
    /// Verifies that default and description extensions add the expected XML and initializer output.
    /// </summary>
    [Test]
    public async Task Should_apply_default_and_description_extensions()
    {
        var property = new Property(DataType.String, "Name")
            .AddRootDescription(new DescriptionSummary(new DescriptionText("Display name.")))
            .AddAccessor(new Accessor(AccessorType.GET))
            .AddNull();

        var code = TestRenderers.Render(property);

        await Assert.That(code).Contains("/// <summary>");
        await Assert.That(code).Contains("Name");
        await Assert.That(code).Contains("} = null;");
    }

    /// <summary>
    /// Verifies that expression convenience extensions compose into the expected concrete syntax nodes.
    /// </summary>
    [Test]
    public async Task Should_apply_expression_convenience_extensions()
    {
        var expression = "items".ToSimpleName()
            .Sub("Count")
            .Operator("+", 1.ToLiteral())
            .Wrap
            .Throw;
        var invocation = typeof(string).ToExpression()
            .Sub("Concat")
            .Invoke()
            .AddArgument(new Argument("a".ToLiteral()))
            .AddArgument(new Argument("b".ToLiteral()));
        var cast = "value".ToSimpleName().Cast<int>();
        var foreachStatement = "items".ToSimpleName().ForEach<int>("value")
            .AddStatement("value".ToSimpleName().Return);

        await Assert.That(TestRenderers.Render(expression)).IsEqualTo("throw (items.Count + 1)");
        await Assert.That(TestRenderers.Render(invocation)).IsEqualTo("string.Concat(\"a\", \"b\")");
        await Assert.That(TestRenderers.Render(cast)).IsEqualTo("(int)value");
        await Assert.That(TestRenderers.Render(foreachStatement)).Contains("foreach (int @value in items)");
    }

    /// <summary>
    /// Verifies that direct-named binary helpers expose the common operators without string literals.
    /// </summary>
    [Test]
    public async Task Should_apply_direct_named_expression_operator_helpers()
    {
        var left = "left".ToSimpleName();
        var right = "right".ToSimpleName();
        var type = typeof(string).ToExpression();

        await Assert.That(TestRenderers.Render(left.Assign(right))).IsEqualTo("left = right");
        await Assert.That(TestRenderers.Render(left.AddAssign(right))).IsEqualTo("left += right");
        await Assert.That(TestRenderers.Render(left.SubtractAssign(right))).IsEqualTo("left -= right");
        await Assert.That(TestRenderers.Render(left.MultiplyAssign(right))).IsEqualTo("left *= right");
        await Assert.That(TestRenderers.Render(left.DivideAssign(right))).IsEqualTo("left /= right");
        await Assert.That(TestRenderers.Render(left.ModuloAssign(right))).IsEqualTo("left %= right");
        await Assert.That(TestRenderers.Render(left.BitwiseAndAssign(right))).IsEqualTo("left &= right");
        await Assert.That(TestRenderers.Render(left.BitwiseOrAssign(right))).IsEqualTo("left |= right");
        await Assert.That(TestRenderers.Render(left.ExclusiveOrAssign(right))).IsEqualTo("left ^= right");
        await Assert.That(TestRenderers.Render(left.LeftShiftAssign(right))).IsEqualTo("left <<= right");
        await Assert.That(TestRenderers.Render(left.RightShiftAssign(right))).IsEqualTo("left >>= right");
        await Assert.That(TestRenderers.Render(left.CoalesceAssign(right))).IsEqualTo("left ??= right");
        await Assert.That(TestRenderers.Render(left.Add(right))).IsEqualTo("left + right");
        await Assert.That(TestRenderers.Render(left.Subtract(right))).IsEqualTo("left - right");
        await Assert.That(TestRenderers.Render(left.Multiply(right))).IsEqualTo("left * right");
        await Assert.That(TestRenderers.Render(left.Divide(right))).IsEqualTo("left / right");
        await Assert.That(TestRenderers.Render(left.Modulo(right))).IsEqualTo("left % right");
        await Assert.That(TestRenderers.Render(left.BitwiseAnd(right))).IsEqualTo("left & right");
        await Assert.That(TestRenderers.Render(left.BitwiseOr(right))).IsEqualTo("left | right");
        await Assert.That(TestRenderers.Render(left.ExclusiveOr(right))).IsEqualTo("left ^ right");
        await Assert.That(TestRenderers.Render(left.LeftShift(right))).IsEqualTo("left << right");
        await Assert.That(TestRenderers.Render(left.RightShift(right))).IsEqualTo("left >> right");
        await Assert.That(TestRenderers.Render(left.EqualTo(right))).IsEqualTo("left == right");
        await Assert.That(TestRenderers.Render(left.NotEqualTo(right))).IsEqualTo("left != right");
        await Assert.That(TestRenderers.Render(left.GreaterThan(right))).IsEqualTo("left > right");
        await Assert.That(TestRenderers.Render(left.LessThan(right))).IsEqualTo("left < right");
        await Assert.That(TestRenderers.Render(left.GreaterThanOrEqualTo(right))).IsEqualTo("left >= right");
        await Assert.That(TestRenderers.Render(left.LessThanOrEqualTo(right))).IsEqualTo("left <= right");
        await Assert.That(TestRenderers.Render(left.And(right))).IsEqualTo("left && right");
        await Assert.That(TestRenderers.Render(left.Or(right))).IsEqualTo("left || right");
        await Assert.That(TestRenderers.Render(left.Coalesce(right))).IsEqualTo("left ?? right");
        await Assert.That(TestRenderers.Render(left.RangeTo(right))).IsEqualTo("left .. right");
        await Assert.That(TestRenderers.Render(left.Is(type))).IsEqualTo("left is string");
        await Assert.That(TestRenderers.Render(left.As(type))).IsEqualTo("left as string");
    }

    /// <summary>
    /// Verifies that direct-named unary and postfix helpers expose the common operators without string literals.
    /// </summary>
    [Test]
    public async Task Should_apply_direct_named_expression_unary_helpers()
    {
        var value = "value".ToSimpleName();

        await Assert.That(TestRenderers.Render(value.UnaryPlus())).IsEqualTo("+ value");
        await Assert.That(TestRenderers.Render(value.Negate())).IsEqualTo("- value");
        await Assert.That(TestRenderers.Render(value.LogicalNot())).IsEqualTo("!value");
        await Assert.That(TestRenderers.Render(value.BitwiseNot())).IsEqualTo("~ value");
        await Assert.That(TestRenderers.Render(value.AddressOf())).IsEqualTo("& value");
        await Assert.That(TestRenderers.Render(value.PointerIndirection())).IsEqualTo("* value");
        await Assert.That(TestRenderers.Render(value.IndexFromEnd())).IsEqualTo("^ value");
        await Assert.That(TestRenderers.Render(value.Await())).IsEqualTo("await value");
        await Assert.That(TestRenderers.Render(value.SuppressNullableWarning())).IsEqualTo("value !");
        await Assert.That(TestRenderers.Render(value.PreIncrement())).IsEqualTo("++ value");
        await Assert.That(TestRenderers.Render(value.PreDecrement())).IsEqualTo("-- value");
        await Assert.That(TestRenderers.Render(value.PostIncrement())).IsEqualTo("value ++");
        await Assert.That(TestRenderers.Render(value.PostDecrement())).IsEqualTo("value --");
    }

    /// <summary>
    /// Verifies that the operator proxy forwards the overloadable operators to the direct-named helper methods.
    /// </summary>
    [Test]
    public async Task Should_apply_expression_operator_proxy_helpers()
    {
        var left = "left".ToSimpleName().Op;
        var right = "right".ToSimpleName();

        await Assert.That(TestRenderers.Render(+left)).IsEqualTo("+ left");
        await Assert.That(TestRenderers.Render(-left)).IsEqualTo("- left");
        await Assert.That(TestRenderers.Render(!left)).IsEqualTo("!left");
        await Assert.That(TestRenderers.Render(~left)).IsEqualTo("~ left");
        await Assert.That(TestRenderers.Render(left + right)).IsEqualTo("left + right");
        await Assert.That(TestRenderers.Render(left - right)).IsEqualTo("left - right");
        await Assert.That(TestRenderers.Render(left * right)).IsEqualTo("left * right");
        await Assert.That(TestRenderers.Render(left / right)).IsEqualTo("left / right");
        await Assert.That(TestRenderers.Render(left % right)).IsEqualTo("left % right");
        await Assert.That(TestRenderers.Render(left & right)).IsEqualTo("left & right");
        await Assert.That(TestRenderers.Render(left | right)).IsEqualTo("left | right");
        await Assert.That(TestRenderers.Render(left ^ right)).IsEqualTo("left ^ right");
        await Assert.That(TestRenderers.Render(left << 2)).IsEqualTo("left << 2");
        await Assert.That(TestRenderers.Render(left >> 2)).IsEqualTo("left >> 2");
    }

    /// <summary>
    /// Verifies that owner extensions append parameters, arguments, members, and statements in insertion order.
    /// </summary>
    [Test]
    public async Task Should_apply_owner_extensions_in_insertion_order()
    {
        var method = new Method("Compose")
            .AddParameter(new Parameter(DataType.Int, "left"))
            .AddParameter(new Parameter(DataType.Int, "right"))
            .AddStatement("left".ToSimpleName().Operator("+", "right".ToSimpleName()).Return);
        var invocation = "Sum".ToSimpleName().Invoke()
            .AddArgument(new Argument(1.ToLiteral()))
            .AddArgument(new Argument(2.ToLiteral()));
        var declaration = new TypeDeclaration("Container", TypeDeclarationType.CLASS)
            .AddMember(method)
            .AddMember(new Field(DataType.Int, "total"));

        await Assert.That(TestRenderers.Render(method)).Contains("Compose(");
        await Assert.That(TestRenderers.Render(method)).Contains("int left,");
        await Assert.That(TestRenderers.Render(method)).Contains("int right)");
        await Assert.That(TestRenderers.Render(invocation)).IsEqualTo("Sum(1, 2)");
        await Assert.That(TestRenderers.Render(declaration)).Contains("int total;");
    }

    /// <summary>
    /// Verifies that attribute and variable convenience extensions expose generator metadata and variable names.
    /// </summary>
    [Test]
    public async Task Should_apply_attribute_and_variable_extensions()
    {
        var field = new Field(DataType.Int, "count")
            .AddGeneratorAttribute(typeof(GeneratorExtensionApiTests));
        var variable = new VariableExpression(DataType.Int, "class");

        await Assert.That(TestRenderers.Render(field)).Contains(
            "[global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"global::TedToolkit.RoslynHelper.Tests.GeneratorExtensionApiTests\", \"1.0.0.0\")]");
        await Assert.That(variable.Name.ToCode()).IsEqualTo("@class");
    }
}
