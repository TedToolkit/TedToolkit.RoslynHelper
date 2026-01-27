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

    /// <summary>
    ///     Print a node to string.
    /// </summary>
    /// <param name="node">node.</param>
    /// <returns>string.</returns>
    public static string NodeToString(this SyntaxNode node)
    {
        using var stringWriter = new StringWriter();
        node.NormalizeWhitespace().WriteTo(stringWriter);
        return stringWriter.ToString();
    }
}