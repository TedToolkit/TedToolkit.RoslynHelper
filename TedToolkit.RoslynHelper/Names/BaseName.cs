// -----------------------------------------------------------------------
// <copyright file="BaseName.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace TedToolkit.RoslynHelper.Names;

/// <inheritdoc />
[Obsolete("Do not use this method, try to use the generators instead!")]
public abstract class BaseName<T> : IName<T>
    where T : ISymbol
{
    private readonly Lazy<string> _lazyFullName;

    private readonly Lazy<string> _lazySummaryName;

    private readonly Lazy<string> _lazyFullNameNoGlobal;

    private readonly Lazy<string> _lazyFullNameNull;

    private readonly Lazy<string> _lazyMiniName;

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseName{T}"/> class.
    /// Create the base name.
    /// </summary>
    /// <param name="symbol">the symbol.</param>
    private protected BaseName(T symbol)
    {
        Symbol = symbol;
        _lazyFullName = new(() => symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
        _lazyFullNameNull = new(() => symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat
            .WithMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier)));
        _lazyFullNameNoGlobal = new(() => symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat
            .WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted)));
        _lazyMiniName = new(() => symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
        _lazySummaryName = new(GetSummaryName);
    }

    /// <inheritdoc />
    public T Symbol { get; }

    /// <inheritdoc />
    public string Name
    {
        get
        {
            return Symbol.Name;
        }
    }

    /// <inheritdoc />
    public string MiniName
    {
        get
        {
            return _lazyMiniName.Value;
        }
    }

    /// <inheritdoc />
    public string FullNameNoGlobal
    {
        get
        {
            return _lazyFullNameNoGlobal.Value;
        }
    }

    /// <inheritdoc />
    public string FullName
    {
        get
        {
            return _lazyFullName.Value;
        }
    }

    /// <inheritdoc />
    public string FullNameNull
    {
        get
        {
            return _lazyFullNameNull.Value;
        }
    }

    /// <inheritdoc />
    public string SummaryName
    {
        get
        {
            return _lazySummaryName.Value;
        }
    }

    /// <summary>
    /// Get the summary name.
    /// </summary>
    /// <returns>summary string.</returns>
    private protected virtual string GetSummaryName()
    {
        return ToSummary(Symbol.OriginalDefinition
            .ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat));
    }

    private static string ToSummary(string name)
    {
        return name.Replace('<', '{').Replace('>', '}');
    }
}