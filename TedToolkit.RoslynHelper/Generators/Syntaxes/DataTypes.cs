// -----------------------------------------------------------------------
// <copyright file="DataTypes.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// For the DataTypes.
/// </summary>
public static class DataTypes
{
    /// <summary>
    /// <see langword="var"/>
    /// </summary>
    public static DataType Var { get; } = new(new SimpleNameExpression("var"));
#pragma warning disable CA1720
    /// <summary>
    /// <see langword="string"/>
    /// </summary>
    public static DataType String { get; } = new(new SimpleNameExpression("string"));

    /// <summary>
    /// <see langword="char"/>
    /// </summary>
    public static DataType Char { get; } = new(new SimpleNameExpression("char"));

    /// <summary>
    /// <see langword="byte"/>
    /// </summary>
    public static DataType Byte { get; } = new(new SimpleNameExpression("byte"));

    /// <summary>
    /// <see langword="sbyte"/>
    /// </summary>
    public static DataType Sbyte { get; } = new(new SimpleNameExpression("sbyte"));

    /// <summary>
    /// <see langword="short"/>
    /// </summary>
    public static DataType Short { get; } = new(new SimpleNameExpression("short"));

    /// <summary>
    /// <see langword="ushort"/>
    /// </summary>
    public static DataType Ushort { get; } = new(new SimpleNameExpression("ushort"));

    /// <summary>
    /// <see langword="int"/>
    /// </summary>
    public static DataType Int { get; } = new(new SimpleNameExpression("int"));

    /// <summary>
    /// <see langword="uint"/>
    /// </summary>
    public static DataType Uint { get; } = new(new SimpleNameExpression("uint"));

    /// <summary>
    /// <see langword="long"/>
    /// </summary>
    public static DataType Long { get; } = new(new SimpleNameExpression("long"));

    /// <summary>
    /// <see langword="ulong"/>
    /// </summary>
    public static DataType Ulong { get; } = new(new SimpleNameExpression("ulong"));

    /// <summary>
    /// <see langword="bool"/>
    /// </summary>
    public static DataType Bool { get; } = new(new SimpleNameExpression("bool"));
#pragma warning restore CA1720

    /// <summary>
    /// From Type
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="result">result</param>
    /// <returns>Expression</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref DataType FromType<T>(in DataType result = default)
        => ref FromType(typeof(T), result);

    /// <summary>
    /// From the type
    /// </summary>
    /// <param name="type">type</param>
    /// <param name="result">result</param>
    /// <returns>result</returns>
    public static ref DataType FromType(Type type, in DataType result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Type = FromTypePrivate(type);
        return ref instance;
    }

    /// <summary>
    /// From Type
    /// </summary>
    /// <param name="type">Type</param>
    /// <returns>Expression</returns>
    /// <exception cref="ArgumentNullException">type is null</exception>
    public static IExpression FromTypePrivate(Type type)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type));

        if (_typeAlias.TryGetValue(type, out var s))
            return s;

        if (type.IsArray)
            return FromTypePrivate(type.GetElementType()!).Array;

        if (type.IsGenericType)
        {
            if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return FromTypePrivate(Nullable.GetUnderlyingType(type)!).Null;

            return SimpleType()
                .Generic([.. type.GetGenericArguments().Select(FromTypePrivate),]);
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