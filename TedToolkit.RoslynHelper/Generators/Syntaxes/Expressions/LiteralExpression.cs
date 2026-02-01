// -----------------------------------------------------------------------
// <copyright file="LiteralExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;

using Cysharp.Text;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The literal Expression.
/// </summary>
public sealed class LiteralExpression : IExpression
{
    private readonly string _value;

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
        => builder.Append(_value);

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
        => ToCode(ref builder);

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from string.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(string value)
        => _value = ZString.Concat('"', value?.Replace(@"\", @"\\") ?? value, '"');

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from char.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(char value)
        => _value = ZString.Concat('\'', value, '\'');

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from byte.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(byte value)
        => _value = value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from sbyte.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(sbyte value)
        => _value = value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from short.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(short value)
        => _value = value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from ushort.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(ushort value)
        => _value = value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from int.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(int value)
        => _value = value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from uint.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(uint value)
        => _value = value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from long.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(long value)
        => _value = value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from ulong.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(ulong value)
        => _value = value.ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from bool.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(bool value)
        => _value = value.ToString(CultureInfo.InvariantCulture);
#pragma warning disable CA2225

    /// <inheritdoc cref="LiteralExpression(char)"/>
    public static implicit operator LiteralExpression(char value)
        => new(value);

    /// <inheritdoc cref="LiteralExpression(string)"/>
    public static implicit operator LiteralExpression(string value)
        => new(value);

    /// <inheritdoc cref="LiteralExpression(byte)"/>
    public static implicit operator LiteralExpression(byte value)
        => new(value);

    /// <inheritdoc cref="LiteralExpression(sbyte)"/>
    public static implicit operator LiteralExpression(sbyte value)
        => new(value);

    /// <inheritdoc cref="LiteralExpression(short)"/>
    public static implicit operator LiteralExpression(short value)
        => new(value);

    /// <inheritdoc cref="LiteralExpression(ushort)"/>
    public static implicit operator LiteralExpression(ushort value)
        => new(value);

    /// <inheritdoc cref="LiteralExpression(int)"/>
    public static implicit operator LiteralExpression(int value)
        => new(value);

    /// <inheritdoc cref="LiteralExpression(uint)"/>
    public static implicit operator LiteralExpression(uint value)
        => new(value);

    /// <inheritdoc cref="LiteralExpression(long)"/>
    public static implicit operator LiteralExpression(long value)
        => new(value);

    /// <inheritdoc cref="LiteralExpression(ulong)"/>
    public static implicit operator LiteralExpression(ulong value)
        => new(value);

    /// <inheritdoc cref="LiteralExpression(bool)"/>
    public static implicit operator LiteralExpression(bool value)
        => new(value);
#pragma warning restore CA2225
}