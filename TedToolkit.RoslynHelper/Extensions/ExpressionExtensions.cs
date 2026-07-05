// -----------------------------------------------------------------------
// <copyright file="ExpressionExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

using TedToolkit.RoslynHelper.Syntaxes;

namespace TedToolkit.RoslynHelper;

/// <summary>
/// The extensions for the <see cref="IExpression"/>.
/// </summary>
#pragma warning disable CA1708
public static class ExpressionExtensions
#pragma warning restore CA1708
{
#pragma warning disable CA1034
    extension(IExpression expression)
#pragma warning restore CA1034
    {
        /// <summary>
        /// Generic the items.
        /// </summary>
        /// <param name="types">types.</param>
        /// <returns>expression.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TypeParameterExpression Generic(params DataType[] types)
        {
            return new(expression, types);
        }

        /// <summary>
        /// Gets make Null.
        /// </summary>
        public NullExpression Null
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new(expression);
            }
        }

        /// <summary>
        /// Gets make Not.
        /// </summary>
        public NotExpression Not
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new(expression);
            }
        }

        /// <summary>
        /// Gets make Ref.
        /// </summary>
        public RefExpression Ref
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new(expression);
            }
        }

        /// <summary>
        /// Gets make Ref.
        /// </summary>
        public RefReadonlyExpression RefReadonly
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new(expression);
            }
        }

        /// <summary>
        /// Gets add ().
        /// </summary>
        public ParenthesizedExpression Wrap
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new(expression);
            }
        }

        /// <summary>
        /// Gets add ().
        /// </summary>
        public ParenthesizedExpression Parenthesized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new(expression);
            }
        }

        /// <summary>
        /// Gets return.
        /// </summary>
        public ReturnStatement Return
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new(expression);
            }
        }

        /// <summary>
        /// Gets if.
        /// </summary>
        public IfStatement If
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new(expression);
            }
        }

        /// <summary>
        /// Gets new.
        /// </summary>
        public ObjectCreationExpression New
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new(new DataType(expression));
            }
        }

        /// <summary>
        /// Gets throw.
        /// </summary>
        public ThrowExpression Throw
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new(expression);
            }
        }

        /// <summary>
        /// Gets using.
        /// </summary>
        public UsingStatement Using
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new(expression);
            }
        }

        /// <summary>
        /// For each.
        /// </summary>
        /// <param name="type">type.</param>
        /// <param name="identifier">name.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ForEachStatement ForEach(scoped in DataType type, string identifier)
        {
            return new(type, identifier, expression);
        }

        /// <summary>
        /// For each.
        /// </summary>
        /// <param name="type">type.</param>
        /// <param name="identifier">name.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ForEachStatement ForEach(Type type, string identifier)
        {
            return new(DataType.FromType(type), identifier, expression);
        }

        /// <summary>
        /// For each.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="identifier">name.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ForEachStatement ForEach<T>(string identifier)
        {
            return new(DataType.FromType<T>(), identifier, expression);
        }

        /// <summary>
        /// Invoke.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public InvocationExpression Invoke()
        {
            return new(expression);
        }

        /// <summary>
        /// Cast.
        /// </summary>
        /// <param name="type">type.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CastExpression Cast(scoped in DataType type)
        {
            return new(type, expression);
        }

        /// <summary>
        /// Cast.
        /// </summary>
        /// <param name="type">type.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CastExpression Cast(Type type)
        {
            return new(DataType.FromType(type), expression);
        }

        /// <summary>
        /// Cast.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public CastExpression Cast<T>()
        {
            return new(DataType.FromType<T>(), expression);
        }

        /// <summary>
        /// Creates a postfix unary expression, such as <c>value++</c>.
        /// </summary>
        /// <param name="operator">The postfix operator.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PostfixUnaryExpression Postfix(string @operator)
        {
            return new(expression, @operator);
        }

        /// <summary>
        /// Creates a prefix unary expression, such as <c>!value</c>.
        /// </summary>
        /// <param name="operator">The prefix operator.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PrefixUnaryExpression Prefix(string @operator)
        {
            return new(@operator, expression);
        }

        /// <summary>
        /// Creates a binary expression, such as <c>left + right</c>.
        /// </summary>
        /// <param name="operator">The binary operator.</param>
        /// <param name="right">The right operand.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression Operator(string @operator, IExpression right)
        {
            return new(@operator, expression, right);
        }

        /// <summary>
        /// Assign. Equivalent to <c>left = right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression Assign(IExpression right)
        {
            return expression.Operator("=", right);
        }

        /// <summary>
        /// Add assign. Equivalent to <c>left += right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression AddAssign(IExpression right)
        {
            return expression.Operator("+=", right);
        }

        /// <summary>
        /// Subtract assign. Equivalent to <c>left -= right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression SubtractAssign(IExpression right)
        {
            return expression.Operator("-=", right);
        }

        /// <summary>
        /// Multiply assign. Equivalent to <c>left *= right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression MultiplyAssign(IExpression right)
        {
            return expression.Operator("*=", right);
        }

        /// <summary>
        /// Divide assign. Equivalent to <c>left /= right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression DivideAssign(IExpression right)
        {
            return expression.Operator("/=", right);
        }

        /// <summary>
        /// Modulo assign. Equivalent to <c>left %= right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression ModuloAssign(IExpression right)
        {
            return expression.Operator("%=", right);
        }

        /// <summary>
        /// Bitwise and assign. Equivalent to <c>left &amp;= right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression BitwiseAndAssign(IExpression right)
        {
            return expression.Operator("&=", right);
        }

        /// <summary>
        /// Bitwise or assign. Equivalent to <c>left |= right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression BitwiseOrAssign(IExpression right)
        {
            return expression.Operator("|=", right);
        }

        /// <summary>
        /// Exclusive or assign. Equivalent to <c>left ^= right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression ExclusiveOrAssign(IExpression right)
        {
            return expression.Operator("^=", right);
        }

        /// <summary>
        /// Left shift assign. Equivalent to <c>left &lt;&lt;= right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression LeftShiftAssign(IExpression right)
        {
            return expression.Operator("<<=", right);
        }

        /// <summary>
        /// Right shift assign. Equivalent to <c>left &gt;&gt;= right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression RightShiftAssign(IExpression right)
        {
            return expression.Operator(">>=", right);
        }

        /// <summary>
        /// Coalesce assign. Equivalent to <c>left ??= right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression CoalesceAssign(IExpression right)
        {
            return expression.Operator("??=", right);
        }

        /// <summary>
        /// Add. Equivalent to <c>left + right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression Add(IExpression right)
        {
            return expression.Operator("+", right);
        }

        /// <summary>
        /// Subtract. Equivalent to <c>left - right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression Subtract(IExpression right)
        {
            return expression.Operator("-", right);
        }

        /// <summary>
        /// Multiply. Equivalent to <c>left * right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression Multiply(IExpression right)
        {
            return expression.Operator("*", right);
        }

        /// <summary>
        /// Divide. Equivalent to <c>left / right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression Divide(IExpression right)
        {
            return expression.Operator("/", right);
        }

        /// <summary>
        /// Modulo. Equivalent to <c>left % right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression Modulo(IExpression right)
        {
            return expression.Operator("%", right);
        }

        /// <summary>
        /// Bitwise and. Equivalent to <c>left &amp; right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression BitwiseAnd(IExpression right)
        {
            return expression.Operator("&", right);
        }

        /// <summary>
        /// Bitwise or. Equivalent to <c>left | right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression BitwiseOr(IExpression right)
        {
            return expression.Operator("|", right);
        }

        /// <summary>
        /// Exclusive or. Equivalent to <c>left ^ right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression ExclusiveOr(IExpression right)
        {
            return expression.Operator("^", right);
        }

        /// <summary>
        /// Left shift. Equivalent to <c>left &lt;&lt; right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression LeftShift(IExpression right)
        {
            return expression.Operator("<<", right);
        }

        /// <summary>
        /// Right shift. Equivalent to <c>left &gt;&gt; right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression RightShift(IExpression right)
        {
            return expression.Operator(">>", right);
        }

        /// <summary>
        /// Equal to. Equivalent to <c>left == right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression EqualTo(IExpression right)
        {
            return expression.Operator("==", right);
        }

        /// <summary>
        /// Not equal to. Equivalent to <c>left != right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression NotEqualTo(IExpression right)
        {
            return expression.Operator("!=", right);
        }

        /// <summary>
        /// Greater than. Equivalent to <c>left &gt; right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression GreaterThan(IExpression right)
        {
            return expression.Operator(">", right);
        }

        /// <summary>
        /// Less than. Equivalent to <c>left &lt; right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression LessThan(IExpression right)
        {
            return expression.Operator("<", right);
        }

        /// <summary>
        /// Greater than or equal to. Equivalent to <c>left &gt;= right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression GreaterThanOrEqualTo(IExpression right)
        {
            return expression.Operator(">=", right);
        }

        /// <summary>
        /// Less than or equal to. Equivalent to <c>left &lt;= right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression LessThanOrEqualTo(IExpression right)
        {
            return expression.Operator("<=", right);
        }

        /// <summary>
        /// And. Equivalent to <c>left &amp;&amp; right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression And(IExpression right)
        {
            return expression.Operator("&&", right);
        }

        /// <summary>
        /// Or. Equivalent to <c>left || right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression Or(IExpression right)
        {
            return expression.Operator("||", right);
        }

        /// <summary>
        /// Coalesce. Equivalent to <c>left ?? right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression Coalesce(IExpression right)
        {
            return expression.Operator("??", right);
        }

        /// <summary>
        /// Range to. Equivalent to <c>left..right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression RangeTo(IExpression right)
        {
            return expression.Operator("..", right);
        }

        /// <summary>
        /// Is. Equivalent to <c>left is right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression Is(IExpression right)
        {
            return expression.Operator("is", right);
        }

        /// <summary>
        /// As. Equivalent to <c>left as right</c>.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression As(IExpression right)
        {
            return expression.Operator("as", right);
        }

        /// <summary>
        /// Unary plus. Equivalent to <c>+value</c>.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PrefixUnaryExpression UnaryPlus()
        {
            return expression.Prefix("+");
        }

        /// <summary>
        /// Negate. Equivalent to <c>-value</c>.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PrefixUnaryExpression Negate()
        {
            return expression.Prefix("-");
        }

        /// <summary>
        /// Logical not. Equivalent to <c>!value</c>.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NotExpression LogicalNot()
        {
            return expression.Not;
        }

        /// <summary>
        /// Bitwise not. Equivalent to <c>~value</c>.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PrefixUnaryExpression BitwiseNot()
        {
            return expression.Prefix("~");
        }

        /// <summary>
        /// Address of. Equivalent to <c>&amp;value</c>.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PrefixUnaryExpression AddressOf()
        {
            return expression.Prefix("&");
        }

        /// <summary>
        /// Pointer indirection. Equivalent to <c>*value</c>.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PrefixUnaryExpression PointerIndirection()
        {
            return expression.Prefix("*");
        }

        /// <summary>
        /// Index from end. Equivalent to <c>^value</c>.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PrefixUnaryExpression IndexFromEnd()
        {
            return expression.Prefix("^");
        }

        /// <summary>
        /// Await. Equivalent to <c>await value</c>.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PrefixUnaryExpression Await()
        {
            return expression.Prefix("await");
        }

        /// <summary>
        /// Suppress nullable warning. Equivalent to <c>value!</c>.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PostfixUnaryExpression SuppressNullableWarning()
        {
            return expression.Postfix("!");
        }

        /// <summary>
        /// Pre increment. Equivalent to <c>++value</c>.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PrefixUnaryExpression PreIncrement()
        {
            return expression.Prefix("++");
        }

        /// <summary>
        /// Pre decrement. Equivalent to <c>--value</c>.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PrefixUnaryExpression PreDecrement()
        {
            return expression.Prefix("--");
        }

        /// <summary>
        /// Post increment. Equivalent to <c>value++</c>.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PostfixUnaryExpression PostIncrement()
        {
            return expression.Postfix("++");
        }

        /// <summary>
        /// Post decrement. Equivalent to <c>value--</c>.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PostfixUnaryExpression PostDecrement()
        {
            return expression.Postfix("--");
        }

        /// <summary>
        /// Gets an operator proxy for the overloadable operators.
        /// </summary>
        public ExpressionOperatorProxy Op
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new(expression);
            }
        }

        /// <summary>
        /// Create the sub items.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MemberAccessExpression Sub(IExpression right)
        {
            return new(expression, right);
        }

        /// <summary>
        /// Create the sub items.
        /// </summary>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        /// <exception cref="ArgumentNullException">right is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MemberAccessExpression Sub(string right)
        {
            if (right is null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            return new(expression, right.ToSimpleName());
        }
    }

#pragma warning disable CA1034
    extension(bool value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
        {
            return new(value);
        }
    }

#pragma warning disable CA1034
    extension(string value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
        {
            return new(value);
        }

        /// <summary>
        /// To Expression.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SimpleNameExpression ToSimpleName()
        {
            return new(value);
        }
    }

#pragma warning disable CA1034
    extension(char value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
        {
            return new(value);
        }
    }

#pragma warning disable CA1034
    extension(byte value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
        {
            return new(value);
        }
    }

#pragma warning disable CA1034
    extension(sbyte value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
        {
            return new(value);
        }
    }

#pragma warning disable CA1034
    extension(short value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
        {
            return new(value);
        }
    }

#pragma warning disable CA1034
    extension(ushort value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
        {
            return new(value);
        }
    }

#pragma warning disable CA1034
    extension(int value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
        {
            return new(value);
        }
    }

#pragma warning disable CA1034
    extension(uint value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
        {
            return new(value);
        }
    }

#pragma warning disable CA1034
    extension(long value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
        {
            return new(value);
        }
    }

#pragma warning disable CA1034
    extension(ulong value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
        {
            return new(value);
        }
    }

#pragma warning disable CA1034
    extension(float value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
        {
            return new(value);
        }
    }

#pragma warning disable CA1034
    extension(double value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
        {
            return new(value);
        }
    }

#pragma warning disable CA1034
    extension(decimal value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
        {
            return new(value);
        }
    }

#pragma warning disable CA1034
    extension<TEnum>(TEnum value)
        where TEnum : struct, System.Enum
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MemberAccessExpression ToExpression()
        {
            return new(DataType.FromType<TEnum>().Type, value.ToString().ToSimpleName());
        }
    }

#pragma warning disable CA1034
    extension(Type type)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression.
        /// </summary>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IExpression ToExpression()
        {
            return DataType.FromType(type).Type;
        }
    }
}