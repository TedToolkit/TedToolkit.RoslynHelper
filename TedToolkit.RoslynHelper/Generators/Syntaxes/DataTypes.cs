// -----------------------------------------------------------------------
// <copyright file="DataTypes.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

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

    /// <summary>
    /// <see langword="double"/>
    /// </summary>
    public static DataType Double { get; } = new(new SimpleNameExpression("double"));

    /// <summary>
    /// <see langword="float"/>
    /// </summary>
    public static DataType Float { get; } = new(new SimpleNameExpression("float"));

    /// <summary>
    /// <see langword="decimal"/>
    /// </summary>
    public static DataType Decimal { get; } = new(new SimpleNameExpression("decimal"));

    /// <summary>
    /// <see langword="object"/>
    /// </summary>
    public static DataType Object { get; } = new(new SimpleNameExpression("object"));

    /// <summary>
    /// <see langword="void"/>
    /// </summary>
    public static DataType Void { get; } = new(new SimpleNameExpression("void"));
#pragma warning restore CA1720
}