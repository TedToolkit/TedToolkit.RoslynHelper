// -----------------------------------------------------------------------
// <copyright file="DataType.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

using Microsoft.CodeAnalysis;

using TedToolkit.RoslynHelper.Extensions;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The data Type
/// </summary>
/// <param name="type">expression to the type</param>
public sealed class DataType(IExpression type) :
    IStorageKind,
    IToCode
{
    /// <summary>
    /// The Type
    /// </summary>
    public IExpression Type { get; set; } = type;

    /// <inheritdoc />
    public StorageKind StorageKind { get; set; }

    /// <summary>
    /// Is Array
    /// </summary>
    public bool IsArray { get; set; }

    /// <summary>
    /// Make Array
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
    /// Make Null
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
    /// Create an instance
    /// </summary>
    public ObjectCreationExpression New
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(this);
    }

    /// <summary>
    /// The pointer counter
    /// </summary>
    public int PointCounter { get; set; }

    /// <summary>
    /// Pointer
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
    /// <param name="types">types</param>
    /// <returns>expression</returns>
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
    /// Create from the symbol
    /// </summary>
    /// <param name="type">type symbol</param>
    public DataType(ITypeSymbol type)
        : this(type?.FullName ?? throw new ArgumentNullException(nameof(type)))
    {
    }

    /// <summary>
    /// Create from name
    /// </summary>
    /// <param name="name">name</param>
    public DataType(string name)
        : this(new SimpleNameExpression(name))
    {
    }

    /// <summary>
    /// Implicit convert
    /// </summary>
    /// <param name="value">value</param>
    /// <returns>result</returns>
    public static implicit operator DataType(Type value)
        => FromType(value);

    /// <summary>
    /// From Type
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <returns>Expression</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DataType FromType<T>()
        => FromType(typeof(T));

    /// <summary>
    /// From the type
    /// </summary>
    /// <param name="type">type</param>
    /// <returns>result</returns>
    /// <exception cref="ArgumentNullException">type is null</exception>
    public static DataType FromType(Type type)
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
                        .Select(FromType),
                ]));
        }

        return new(SimpleType());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IExpression SimpleType()
        {
            var name = new SimpleNameExpression(type.Name.Split('`')[0]);

            if (type.DeclaringType is not null)
                return new MemberAccessExpression(FromType(type.DeclaringType).Type, name);

            if (string.IsNullOrEmpty(type.Namespace))
                return name;

            return new MemberAccessExpression(new SimpleNameExpression(type.Namespace), name);
        }
    }

    /// <summary>
    /// <see langword="var"/>
    /// </summary>
    public static DataType Var
        => new(new SimpleNameExpression("var"));
#pragma warning disable CA1720
    /// <summary>
    /// <see langword="string"/>
    /// </summary>
    public static DataType String
        => new(new SimpleNameExpression("string"));

    /// <summary>
    /// <see langword="char"/>
    /// </summary>
    public static DataType Char
        => new(new SimpleNameExpression("char"));

    /// <summary>
    /// <see langword="byte"/>
    /// </summary>
    public static DataType Byte
        => new(new SimpleNameExpression("byte"));

    /// <summary>
    /// <see langword="sbyte"/>
    /// </summary>
    public static DataType Sbyte
        => new(new SimpleNameExpression("sbyte"));

    /// <summary>
    /// <see langword="short"/>
    /// </summary>
    public static DataType Short
        => new(new SimpleNameExpression("short"));

    /// <summary>
    /// <see langword="ushort"/>
    /// </summary>
    public static DataType Ushort
        => new(new SimpleNameExpression("ushort"));

    /// <summary>
    /// <see langword="int"/>
    /// </summary>
    public static DataType Int
        => new(new SimpleNameExpression("int"));

    /// <summary>
    /// <see langword="uint"/>
    /// </summary>
    public static DataType Uint
        => new(new SimpleNameExpression("uint"));

    /// <summary>
    /// <see langword="long"/>
    /// </summary>
    public static DataType Long
        => new(new SimpleNameExpression("long"));

    /// <summary>
    /// <see langword="ulong"/>
    /// </summary>
    public static DataType Ulong
        => new(new SimpleNameExpression("ulong"));

    /// <summary>
    /// <see langword="bool"/>
    /// </summary>
    public static DataType Bool
        => new(new SimpleNameExpression("bool"));

    /// <summary>
    /// <see langword="double"/>
    /// </summary>
    public static DataType Double
        => new(new SimpleNameExpression("double"));

    /// <summary>
    /// <see langword="float"/>
    /// </summary>
    public static DataType Float
        => new(new SimpleNameExpression("float"));

    /// <summary>
    /// <see langword="decimal"/>
    /// </summary>
    public static DataType Decimal
        => new(new SimpleNameExpression("decimal"));

    /// <summary>
    /// <see langword="object"/>
    /// </summary>
    public static DataType Object
        => new(new SimpleNameExpression("object"));

    /// <summary>
    /// <see langword="void"/>
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