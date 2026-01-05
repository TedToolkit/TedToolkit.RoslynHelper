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
/// The extensions for the <see cref="IExpression"/>
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
        /// <param name="types">types</param>
        /// <returns>expression</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TypeParameterExpression Generic(params DataType[] types)
            => new(expression, types);

        /// <summary>
        /// Make Null
        /// </summary>
        public NullExpression Null
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(expression);
        }

        /// <summary>
        /// Create the sub items
        /// </summary>
        /// <param name="right">right</param>
        /// <returns>result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MemberAccessExpression Sub(IExpression right)
            => new(expression, right);

        /// <summary>
        /// Create the sub items
        /// </summary>
        /// <param name="right">right</param>
        /// <returns>result</returns>
        /// <exception cref="ArgumentNullException">right is null</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public MemberAccessExpression Sub(string right)
        {
            if (right is null)
                throw new ArgumentNullException(nameof(right));

            return new(expression, right.ToSimpleName());
        }
    }

#pragma warning disable CA1034
    extension(string value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression
        /// </summary>
        /// <returns>result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
            => new(value);

        /// <summary>
        /// To Expression
        /// </summary>
        /// <returns>result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SimpleNameExpression ToSimpleName()
            => new(value);
    }

#pragma warning disable CA1034
    extension(char value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression
        /// </summary>
        /// <returns>result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
            => new(value);
    }

#pragma warning disable CA1034
    extension(byte value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression
        /// </summary>
        /// <returns>result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
            => new(value);
    }

#pragma warning disable CA1034
    extension(sbyte value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression
        /// </summary>
        /// <returns>result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
            => new(value);
    }

#pragma warning disable CA1034
    extension(short value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression
        /// </summary>
        /// <returns>result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
            => new(value);
    }

#pragma warning disable CA1034
    extension(ushort value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression
        /// </summary>
        /// <returns>result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
            => new(value);
    }

#pragma warning disable CA1034
    extension(int value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression
        /// </summary>
        /// <returns>result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
            => new(value);
    }

#pragma warning disable CA1034
    extension(uint value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression
        /// </summary>
        /// <returns>result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
            => new(value);
    }

#pragma warning disable CA1034
    extension(long value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression
        /// </summary>
        /// <returns>result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
            => new(value);
    }

#pragma warning disable CA1034
    extension(ulong value)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression
        /// </summary>
        /// <returns>result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public LiteralExpression ToLiteral()
            => new(value);
    }

#pragma warning disable CA1034
    extension(Type type)
#pragma warning restore CA1034
    {
        /// <summary>
        /// To Expression
        /// </summary>
        /// <param name="result">result</param>
        /// <returns>result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref DataType ToExpression(in DataType result = default)
            => ref DataTypes.FromType(type, result);
    }
}