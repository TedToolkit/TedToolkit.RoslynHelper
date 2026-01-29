// -----------------------------------------------------------------------
// <copyright file="DataType.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

using Cysharp.Text;

using Microsoft.CodeAnalysis;

using TedToolkit.RoslynHelper.Extensions;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The data Type.
/// </summary>
/// <param name="type">expression to the type.</param>
public sealed class DataType(IExpression type) :
    IStorageKind,
    IToCode
{
    /// <summary>
    /// Gets or sets the Type.
    /// </summary>
    public IExpression Type { get; set; } = type;

    /// <inheritdoc />
    public StorageKind StorageKind { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether is Array.
    /// </summary>
    public bool IsArray { get; set; }

    /// <summary>
    /// Gets make Array.
    /// </summary>
    public DataType Array
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            IsArray = true;
            return this;
        }
    }

    /// <summary>
    /// Gets make Null.
    /// </summary>
    public DataType Null
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            Type = Type.Null;
            return this;
        }
    }

    /// <summary>
    /// Gets create an instance.
    /// </summary>
    public ObjectCreationExpression New
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(this);
    }

    /// <summary>
    /// Gets or sets the pointer counter.
    /// </summary>
    public int PointCounter { get; set; }

    /// <summary>
    /// Gets pointer.
    /// </summary>
#pragma warning disable CA1720
    public DataType Pointer
#pragma warning restore CA1720
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            PointCounter++;
            return this;
        }
    }

    /// <summary>
    /// Generic the items.
    /// </summary>
    /// <param name="types">types.</param>
    /// <returns>expression.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DataType Generic(params DataType[] types)
    {
        Type = Type.Generic(types);
        return this;
    }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddStorageKind(ref builder);
        Type.ToCode(ref builder);
        if (IsArray)
            builder.Append("[]");

        if (PointCounter > 0)
            builder.Append('*', PointCounter);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataType"/> class.
    /// Create from the symbol.
    /// </summary>
    /// <param name="type">type symbol.</param>
    /// <param name="alias">alias.</param>
    public DataType(ITypeSymbol type, string alias = "")
        : this(string.IsNullOrEmpty(alias)
            ? type?.FullName ?? throw new ArgumentNullException(nameof(type))
            : ZString.Concat(alias, "::", type?.FullName ?? throw new ArgumentNullException(nameof(type))))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataType"/> class.
    /// Create from name.
    /// </summary>
    /// <param name="name">name.</param>
    public DataType(string name)
        : this(new SimpleNameExpression(name))
    {
    }

    /// <summary>
    /// Implicit convert.
    /// </summary>
    /// <param name="value">value.</param>
    /// <returns>result.</returns>
    public static implicit operator DataType(Type value)
        => FromType(value);

    /// <summary>
    /// From Type.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="alias">alias.</param>
    /// <returns>Expression.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DataType FromType<T>(string alias = "")
        => FromType(typeof(T), alias);

    /// <summary>
    /// From the type.
    /// </summary>
    /// <param name="type">type.</param>
    /// <param name="alias">alias.</param>
    /// <returns>result.</returns>
    /// <exception cref="ArgumentNullException">type is null.</exception>
    public static DataType FromType(Type type, string alias = "")
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type));

        if (_typeAlias.TryGetValue(type, out var s))
            return s();

        if (type.IsArray)
            return FromType(type.GetElementType()!).Array;

        if (type.IsGenericType)
        {
            if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return FromType(Nullable.GetUnderlyingType(type)!).Null;

            return new(SimpleType()
                .Generic([
                    .. type.GetGenericArguments()
                        .Where(i => !i.IsGenericParameter)
                        .Select(t => FromType(t, alias)),
                ]));
        }

        return new(SimpleType());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IExpression SimpleType()
        {
            var name = new SimpleNameExpression(type.Name.Split('`')[0].Replace("&", ""));

            if (type.DeclaringType is not null)
                return new MemberAccessExpression(FromType(type.DeclaringType).Type, name);

            if (string.IsNullOrEmpty(type.Namespace))
                return AddAlias(name);

            return AddAlias(new MemberAccessExpression(new SimpleNameExpression(type.Namespace), name));
        }

        IExpression AddAlias(IExpression expression)
        {
            if (string.IsNullOrEmpty(alias))
                return expression;

            return new AliasExpression(alias, expression);
        }
    }

    /// <summary>
    /// Gets <see langword="var"/>.
    /// </summary>
    public static DataType Var
        => new(new SimpleNameExpression("var"));
#pragma warning disable CA1720
    /// <summary>
    /// Gets <see langword="string"/>.
    /// </summary>
    public static DataType String
        => new(new SimpleNameExpression("string"));

    /// <summary>
    /// Gets <see langword="char"/>.
    /// </summary>
    public static DataType Char
        => new(new SimpleNameExpression("char"));

    /// <summary>
    /// Gets <see langword="byte"/>.
    /// </summary>
    public static DataType Byte
        => new(new SimpleNameExpression("byte"));

    /// <summary>
    /// Gets <see langword="sbyte"/>.
    /// </summary>
    public static DataType Sbyte
        => new(new SimpleNameExpression("sbyte"));

    /// <summary>
    /// Gets <see langword="short"/>.
    /// </summary>
    public static DataType Short
        => new(new SimpleNameExpression("short"));

    /// <summary>
    /// Gets <see langword="ushort"/>.
    /// </summary>
    public static DataType Ushort
        => new(new SimpleNameExpression("ushort"));

    /// <summary>
    /// Gets <see langword="int"/>.
    /// </summary>
    public static DataType Int
        => new(new SimpleNameExpression("int"));

    /// <summary>
    /// Gets <see langword="uint"/>.
    /// </summary>
    public static DataType Uint
        => new(new SimpleNameExpression("uint"));

    /// <summary>
    /// Gets <see langword="long"/>.
    /// </summary>
    public static DataType Long
        => new(new SimpleNameExpression("long"));

    /// <summary>
    /// Gets <see langword="ulong"/>.
    /// </summary>
    public static DataType Ulong
        => new(new SimpleNameExpression("ulong"));

    /// <summary>
    /// Gets <see langword="bool"/>.
    /// </summary>
    public static DataType Bool
        => new(new SimpleNameExpression("bool"));

    /// <summary>
    /// Gets <see langword="double"/>.
    /// </summary>
    public static DataType Double
        => new(new SimpleNameExpression("double"));

    /// <summary>
    /// Gets <see langword="float"/>.
    /// </summary>
    public static DataType Float
        => new(new SimpleNameExpression("float"));

    /// <summary>
    /// Gets <see langword="decimal"/>.
    /// </summary>
    public static DataType Decimal
        => new(new SimpleNameExpression("decimal"));

    /// <summary>
    /// Gets <see langword="object"/>.
    /// </summary>
    public static DataType Object
        => new(new SimpleNameExpression("object"));

    /// <summary>
    /// Gets <see langword="void"/>.
    /// </summary>
    public static DataType Void
        => new(new SimpleNameExpression("void"));
#pragma warning restore CA1720

    private static readonly Dictionary<Type, Func<DataType>> _typeAlias = new()
    {
        { typeof(bool), () => Bool },
        { typeof(byte), () => Byte },
        { typeof(char), () => Char },
        { typeof(decimal), () => Decimal },
        { typeof(double), () => Double },
        { typeof(float), () => Float },
        { typeof(int), () => Int },
        { typeof(long), () => Long },
        { typeof(object), () => Object },
        { typeof(sbyte), () => Sbyte },
        { typeof(short), () => Short },
        { typeof(string), () => String },
        { typeof(uint), () => Uint },
        { typeof(ulong), () => Ulong },
        { typeof(ushort), () => Ushort },
        { typeof(void), () => Void },
    };
}