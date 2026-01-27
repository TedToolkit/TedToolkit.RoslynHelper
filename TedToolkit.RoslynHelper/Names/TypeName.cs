// -----------------------------------------------------------------------
// <copyright file="TypeName.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.CodeAnalysis;

namespace TedToolkit.RoslynHelper.Names;

/// <summary>
///     Get the type name.
/// </summary>
public class TypeName : TypeParametersName<ITypeSymbol>
{
    private readonly Lazy<string> _lazySafeName;

    private static readonly Regex _regex = new(@"[.\[\]<>,\s:]");

    /// <summary>
    /// Initializes a new instance of the <see cref="TypeName"/> class.
    /// The type name.
    /// </summary>
    /// <param name="typeSymbol">Type symbol.</param>
    internal TypeName(ITypeSymbol typeSymbol)
        : base(typeSymbol)
    {
        _lazySafeName = new(() => _regex.Replace(FullNameNoGlobal,
"_") + "_" + GetHashName(FullName, 8));
    }

    /// <summary>
    ///     Gets the safe name.
    /// </summary>
    public string SafeName
        => _lazySafeName.Value;

    private static string GetHashName(string input, int count)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        return string.Concat(hashBytes.Take(count).Select(b => chars[b % chars.Length]));
    }

    /// <inheritdoc/>
    private protected override IEnumerable<ITypeParameterSymbol> GetTypeParameters(ITypeSymbol symbol)
    {
        return GetTypeParameterSymbols(symbol);

        static IEnumerable<ITypeParameterSymbol> GetTypeParameterSymbols(ITypeSymbol symbol)
        {
            if (symbol is ITypeParameterSymbol typeParameterSymbol)
                yield return typeParameterSymbol;

            if (symbol is not INamedTypeSymbol namedTypeSymbol)
                yield break;

            foreach (var typeParameter in namedTypeSymbol.TypeArguments.SelectMany(GetTypeParameterSymbols))
                yield return typeParameter;
        }
    }
}