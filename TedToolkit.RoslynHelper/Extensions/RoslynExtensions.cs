// -----------------------------------------------------------------------
// <copyright file="RoslynExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace TedToolkit.RoslynHelper.Extensions;

/// <summary>
///     The default Roslyn Extensions.
/// </summary>
public static class RoslynExtensions
{
    /// <summary>
    ///     Get the base types and this type.
    /// </summary>
    /// <param name="type">Type.</param>
    /// <returns>Result.</returns>
    public static IEnumerable<ITypeSymbol> GetBaseTypesAndThis(this ITypeSymbol type)
    {
        var current = type;
        while (current is not null)
        {
            yield return current;
            current = current.BaseType;
        }
    }

    /// <summary>
    ///     Get the base types.
    /// </summary>
    /// <param name="type">type.</param>
    /// <returns>result.</returns>
    public static IEnumerable<ITypeSymbol> GetBaseTypes(this ITypeSymbol type)
    {
        var current = type?.BaseType;
        while (current is not null)
        {
            yield return current;
            current = current.BaseType;
        }
    }

#pragma warning disable CA1034
    extension(ISymbol symbol)
#pragma warning restore CA1034
    {
        /// <summary>
        /// Gets full name of the symbol.
        /// </summary>
#pragma warning disable S2325
        public string FullName
#pragma warning restore S2325
        {
            get
            {
                return symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat
                    .WithMiscellaneousOptions(SymbolDisplayMiscellaneousOptions.IncludeNullableReferenceTypeModifier)
                    .WithGlobalNamespaceStyle(SymbolDisplayGlobalNamespaceStyle.Omitted));
            }
        }
    }
}