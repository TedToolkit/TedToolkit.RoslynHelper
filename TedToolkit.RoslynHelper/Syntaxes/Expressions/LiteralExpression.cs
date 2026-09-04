// -----------------------------------------------------------------------
// <copyright file="LiteralExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;

using Cysharp.Text;

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// The literal Expression.
/// </summary>
public sealed class LiteralExpression : IExpression
{
    private readonly string _value;

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append(_value);
    }

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
    {
        ToCode(ref builder);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from string.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(string value)
    {
        _value = ZString.Concat('"', value?.ToValidLiteral(), '"');
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from char.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(char value)
    {
        _value = ZString.Concat('\'', value.ToValidLiteral(), '\'');
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from byte.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(byte value)
    {
        _value = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from sbyte.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(sbyte value)
    {
        _value = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from short.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(short value)
    {
        _value = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from ushort.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(ushort value)
    {
        _value = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from int.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(int value)
    {
        _value = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from uint.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(uint value)
    {
        _value = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from long.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(long value)
    {
        _value = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from ulong.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(ulong value)
    {
        _value = value.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from float.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(float value)
    {
        _value = value switch
        {
            float.NaN => "global::System.Single.NaN",
            float.PositiveInfinity => "global::System.Single.PositiveInfinity",
            float.NegativeInfinity => "global::System.Single.NegativeInfinity",
            _ => $"{value.ToString("R", CultureInfo.InvariantCulture)}F",
        };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from double.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(double value)
    {
        _value = value switch
        {
            double.NaN => "global::System.Double.NaN",
            double.PositiveInfinity => "global::System.Double.PositiveInfinity",
            double.NegativeInfinity => "global::System.Double.NegativeInfinity",
            _ => $"{value.ToString("R", CultureInfo.InvariantCulture)}D",
        };
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from decimal.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(decimal value)
    {
        _value = $"{value.ToString(CultureInfo.InvariantCulture)}M";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LiteralExpression"/> class.
    /// Create from bool.
    /// </summary>
    /// <param name="value">value.</param>
    public LiteralExpression(bool value)
    {
        _value = (value ? "true" : "false").ToString(CultureInfo.InvariantCulture);
    }
#pragma warning disable CA2225

    /// <inheritdoc cref="LiteralExpression(char)"/>
    public static implicit operator LiteralExpression(char value)
    {
        return new(value);
    }

    /// <inheritdoc cref="LiteralExpression(string)"/>
    public static implicit operator LiteralExpression(string value)
    {
        return new(value);
    }

    /// <inheritdoc cref="LiteralExpression(byte)"/>
    public static implicit operator LiteralExpression(byte value)
    {
        return new(value);
    }

    /// <inheritdoc cref="LiteralExpression(sbyte)"/>
    public static implicit operator LiteralExpression(sbyte value)
    {
        return new(value);
    }

    /// <inheritdoc cref="LiteralExpression(short)"/>
    public static implicit operator LiteralExpression(short value)
    {
        return new(value);
    }

    /// <inheritdoc cref="LiteralExpression(ushort)"/>
    public static implicit operator LiteralExpression(ushort value)
    {
        return new(value);
    }

    /// <inheritdoc cref="LiteralExpression(int)"/>
    public static implicit operator LiteralExpression(int value)
    {
        return new(value);
    }

    /// <inheritdoc cref="LiteralExpression(uint)"/>
    public static implicit operator LiteralExpression(uint value)
    {
        return new(value);
    }

    /// <inheritdoc cref="LiteralExpression(long)"/>
    public static implicit operator LiteralExpression(long value)
    {
        return new(value);
    }

    /// <inheritdoc cref="LiteralExpression(ulong)"/>
    public static implicit operator LiteralExpression(ulong value)
    {
        return new(value);
    }

    /// <inheritdoc cref="LiteralExpression(float)"/>
    public static implicit operator LiteralExpression(float value)
    {
        return new(value);
    }

    /// <inheritdoc cref="LiteralExpression(double)"/>
    public static implicit operator LiteralExpression(double value)
    {
        return new(value);
    }

    /// <inheritdoc cref="LiteralExpression(decimal)"/>
    public static implicit operator LiteralExpression(decimal value)
    {
        return new(value);
    }

    /// <inheritdoc cref="LiteralExpression(bool)"/>
    public static implicit operator LiteralExpression(bool value)
    {
        return new(value);
    }
#pragma warning restore CA2225
}