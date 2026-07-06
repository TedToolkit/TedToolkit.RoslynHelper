// -----------------------------------------------------------------------
// <copyright file="ExpressionOperatorProxy.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// A small wrapper that exposes the subset of C# operators that can forward to <see cref="IExpression"/> helpers.
/// </summary>
/// <param name="expression">expression.</param>
#pragma warning disable CA1815, CA2225
public readonly ref struct ExpressionOperatorProxy(IExpression expression)
{
    /// <summary>
    /// Gets the wrapped expression.
    /// </summary>
    private readonly IExpression _expression = expression;

    /// <summary>
    /// Unary plus.
    /// </summary>
    /// <param name="value">value.</param>
    /// <returns>result.</returns>
    public static PrefixUnaryExpression operator +(in ExpressionOperatorProxy value)
    {
        return value._expression.UnaryPlus();
    }

    /// <summary>
    /// Negate.
    /// </summary>
    /// <param name="value">value.</param>
    /// <returns>result.</returns>
    public static PrefixUnaryExpression operator -(in ExpressionOperatorProxy value)
    {
        return value._expression.Negate();
    }

    /// <summary>
    /// Logical not.
    /// </summary>
    /// <param name="value">value.</param>
    /// <returns>result.</returns>
    public static NotExpression operator !(in ExpressionOperatorProxy value)
    {
        return value._expression.LogicalNot();
    }

    /// <summary>
    /// Bitwise not.
    /// </summary>
    /// <param name="value">value.</param>
    /// <returns>result.</returns>
    public static PrefixUnaryExpression operator ~(in ExpressionOperatorProxy value)
    {
        return value._expression.BitwiseNot();
    }

    /// <summary>
    /// Add.
    /// </summary>
    /// <param name="left">left.</param>
    /// <param name="right">right.</param>
    /// <returns>result.</returns>
    public static BinaryExpression operator +(in ExpressionOperatorProxy left, IExpression right)
    {
        return left._expression.Add(right);
    }

    /// <summary>
    /// Subtract.
    /// </summary>
    /// <param name="left">left.</param>
    /// <param name="right">right.</param>
    /// <returns>result.</returns>
    public static BinaryExpression operator -(in ExpressionOperatorProxy left, IExpression right)
    {
        return left._expression.Subtract(right);
    }

    /// <summary>
    /// Multiply.
    /// </summary>
    /// <param name="left">left.</param>
    /// <param name="right">right.</param>
    /// <returns>result.</returns>
    public static BinaryExpression operator *(in ExpressionOperatorProxy left, IExpression right)
    {
        return left._expression.Multiply(right);
    }

    /// <summary>
    /// Divide.
    /// </summary>
    /// <param name="left">left.</param>
    /// <param name="right">right.</param>
    /// <returns>result.</returns>
    public static BinaryExpression operator /(in ExpressionOperatorProxy left, IExpression right)
    {
        return left._expression.Divide(right);
    }

    /// <summary>
    /// Modulo.
    /// </summary>
    /// <param name="left">left.</param>
    /// <param name="right">right.</param>
    /// <returns>result.</returns>
    public static BinaryExpression operator %(in ExpressionOperatorProxy left, IExpression right)
    {
        return left._expression.Modulo(right);
    }

    /// <summary>
    /// And.
    /// </summary>
    /// <param name="left">left.</param>
    /// <param name="right">right.</param>
    /// <returns>result.</returns>
    public static BinaryExpression operator &(in ExpressionOperatorProxy left, IExpression right)
    {
        return left._expression.BitwiseAnd(right);
    }

    /// <summary>
    /// Or.
    /// </summary>
    /// <param name="left">left.</param>
    /// <param name="right">right.</param>
    /// <returns>result.</returns>
    public static BinaryExpression operator |(in ExpressionOperatorProxy left, IExpression right)
    {
        return left._expression.BitwiseOr(right);
    }

    /// <summary>
    /// Exclusive or.
    /// </summary>
    /// <param name="left">left.</param>
    /// <param name="right">right.</param>
    /// <returns>result.</returns>
    public static BinaryExpression operator ^(in ExpressionOperatorProxy left, IExpression right)
    {
        return left._expression.ExclusiveOr(right);
    }

    /// <summary>
    /// Left shift.
    /// </summary>
    /// <param name="left">left.</param>
    /// <param name="count">count.</param>
    /// <returns>result.</returns>
    public static BinaryExpression operator <<(in ExpressionOperatorProxy left, int count)
    {
        return left._expression.LeftShift(new LiteralExpression(count));
    }

    /// <summary>
    /// Right shift.
    /// </summary>
    /// <param name="left">left.</param>
    /// <param name="count">count.</param>
    /// <returns>result.</returns>
    public static BinaryExpression operator >>(in ExpressionOperatorProxy left, int count)
    {
        return left._expression.RightShift(new LiteralExpression(count));
    }
}
#pragma warning restore CA1815, CA2225