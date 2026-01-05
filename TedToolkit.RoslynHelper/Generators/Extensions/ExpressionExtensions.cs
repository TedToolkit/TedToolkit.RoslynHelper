// -----------------------------------------------------------------------
// <copyright file="ExpressionExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

using TedToolkit.RoslynHelper.Generators.Types;

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
        public TypeParameterExpression Generic(params IExpression[] types)
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
        /// Make Null
        /// </summary>
        public ArrayExpression Array
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new(expression);
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
        /// <returns>result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IExpression ToExpression()
        {
            if (_typeAlias.TryGetValue(type, out var s))
                return s;

            if (type.IsArray)
                return type.GetElementType()!.ToExpression().Array;

            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    return Nullable.GetUnderlyingType(type)!.ToExpression().Null;

                return SimpleType()
                    .Generic([.. type.GetGenericArguments().Select(ToExpression),]);
            }

            return SimpleType();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            IExpression SimpleType()
            {
                var name = new SimpleNameExpression(type.Name.Split('`')[0]);
                if (string.IsNullOrEmpty(type.Namespace))
                    return name;

                return new MemberAccessExpression(new SimpleNameExpression(type.Namespace), name);
            }
        }
    }

    private static readonly Dictionary<Type, SimpleNameExpression> _typeAlias = new()
    {
        { typeof(bool), new("bool") },
        { typeof(byte), new("byte") },
        { typeof(char), new("char") },
        { typeof(decimal), new("decimal") },
        { typeof(double), new("double") },
        { typeof(float), new("float") },
        { typeof(int), new("int") },
        { typeof(long), new("long") },
        { typeof(object), new("object") },
        { typeof(sbyte), new("sbyte") },
        { typeof(short), new("short") },
        { typeof(string), new("string") },
        { typeof(uint), new("uint") },
        { typeof(ulong), new("ulong") },
        { typeof(ushort), new("ushort") },
        { typeof(void), new("void") },
    };
}