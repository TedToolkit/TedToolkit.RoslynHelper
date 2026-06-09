// -----------------------------------------------------------------------
// <copyright file="ExpressionExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Generators;

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
        /// Postfix.
        /// </summary>
        /// <param name="operator">postfix.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PostfixUnaryExpression Postfix(string @operator)
        {
            return new(expression, @operator);
        }

        /// <summary>
        /// Prefix.
        /// </summary>
        /// <param name="operator">prefix.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PrefixUnaryExpression Prefix(string @operator)
        {
            return new(@operator, expression);
        }

        /// <summary>
        /// Prefix.
        /// </summary>
        /// <param name="operator">prefix.</param>
        /// <param name="right">right.</param>
        /// <returns>result.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BinaryExpression Operator(string @operator, IExpression right)
        {
            return new(@operator, expression, right);
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