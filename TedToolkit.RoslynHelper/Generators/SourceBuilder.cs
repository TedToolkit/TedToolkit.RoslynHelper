// -----------------------------------------------------------------------
// <copyright file="SourceBuilder.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

using Cysharp.Text;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The Source Builder
/// </summary>
public record struct SourceBuilder : IDisposable
{
    /// <summary>
    /// The Builder
    /// </summary>
    private Utf16ValueStringBuilder _stringBuilder;

    /// <summary>
    /// The cound of the indent.
    /// </summary>
    private byte _indentCount;

    /// <summary>
    /// Create a source builder
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SourceBuilder()
    {
        _stringBuilder = ZString.CreateStringBuilder();
        _indentCount = 0;
    }

    /// <inheritdoc />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Dispose()
        => _stringBuilder.Dispose();

    /// <summary>
    /// Indent
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Indent()
        => _indentCount++;

    /// <summary>
    /// Dedent
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Dedent()
        => _indentCount--;

    /// <inheritdoc cref="Utf16ValueStringBuilder.AppendLine()"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendLine()
    {
        _stringBuilder.AppendLine();
        _stringBuilder.Append('\t', _indentCount);
    }

    /// <inheritdoc cref="Utf16ValueStringBuilder.AppendLine(char)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendLine(char value)
    {
        _stringBuilder.Append(value);
        _stringBuilder.AppendLine();
        _stringBuilder.Append('\t', _indentCount);
    }

    /// <inheritdoc cref="Utf16ValueStringBuilder.AppendLine(string)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void AppendLine(string value)
    {
        _stringBuilder.Append(value);
        _stringBuilder.AppendLine();
        _stringBuilder.Append('\t', _indentCount);
    }

    /// <inheritdoc cref="Utf16ValueStringBuilder.Append(string)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(string value)
        => _stringBuilder.Append(value);

    /// <inheritdoc cref="Utf16ValueStringBuilder.Append(char)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Append(char value)
        => _stringBuilder.Append(value);

    /// <summary>
    /// Begin a block
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void BeginBlock()
    {
        AppendLine();
        Append('{');
        Indent();
    }

    /// <summary>
    /// End a block
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void EndBlock()
    {
        Dedent();
        AppendLine();
        Append('}');
    }

    /// <summary>
    /// To Code.
    /// </summary>
    /// <returns>codes</returns>
    public string ToCode()
        => _stringBuilder.ToString();
}