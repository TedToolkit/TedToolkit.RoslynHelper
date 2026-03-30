// -----------------------------------------------------------------------
// <copyright file="SymbolExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

using TedToolkit.RoslynHelper.Names;

namespace TedToolkit.RoslynHelper.Extensions;

/// <summary>
///     Extensions for symbol.
/// </summary>
[Obsolete("Do not use this method, try to use the generators instead!")]
public static class SymbolExtensions
{
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

    /// <summary>
    ///     Get the type name.
    /// </summary>
    /// <param name="symbol">Symbol.</param>
    /// <returns>Name.</returns>
    public static TypeName GetName(this ITypeSymbol symbol)
    {
        return new(symbol);
    }

    /// <summary>
    ///     Get the type name.
    /// </summary>
    /// <param name="symbol">Symbol.</param>
    /// <returns>Name.</returns>
    public static TypeParamName GetName(this ITypeParameterSymbol symbol)
    {
        return new(symbol);
    }

    /// <summary>
    ///     Get the type name.
    /// </summary>
    /// <param name="symbol">Symbol.</param>
    /// <returns>Name.</returns>
    /// <exception cref="ArgumentNullException">symbol is null.</exception>
    public static MethodName GetName(this IMethodSymbol symbol)
    {
        if (symbol is null)
        {
            throw new ArgumentNullException(nameof(symbol));
        }

        return new(symbol);
    }

    /// <summary>
    ///     Get the type name.
    /// </summary>
    /// <param name="symbol">Symbol.</param>
    /// <returns>Name.</returns>
    /// <exception cref="ArgumentNullException">symbol is null.</exception>
    public static ParameterName GetName(this IParameterSymbol symbol)
    {
        if (symbol is null)
        {
            throw new ArgumentNullException(nameof(symbol));
        }

        return new(symbol);
    }

    /// <summary>
    ///     Get the type name.
    /// </summary>
    /// <param name="symbols">Symbols.</param>
    /// <returns>result.</returns>
    public static IEnumerable<TypeParamName> GetNames(this IEnumerable<ITypeParameterSymbol> symbols)
    {
        return symbols.Select(GetName);
    }

    /// <summary>
    ///     Get the type name.
    /// </summary>
    /// <param name="symbols">Symbols.</param>
    /// <returns>result.</returns>
    public static IEnumerable<ParameterName> GetNames(this IEnumerable<IParameterSymbol> symbols)
    {
        return symbols.Select(GetName);
    }

    /// <summary>
    ///     Get the extension methods for.
    /// </summary>
    /// <param name="compilation">compilation.</param>
    /// <returns>result.</returns>
    /// <exception cref="ArgumentNullException">compilation is null.</exception>
    public static IReadOnlyDictionary<ISymbol?, IMethodSymbol[]> GetAllExtensionMethods(this Compilation compilation)
    {
        if (compilation is null)
        {
            throw new ArgumentNullException(nameof(compilation));
        }

        return compilation.GlobalNamespace.GetAllStaticClasses()
            .SelectMany(c => c.GetMembers())
            .OfType<IMethodSymbol>()
            .Where(m => m is { IsStatic: true, IsExtensionMethod: true, Parameters.Length: > 0, })
            .GroupBy(m => m.Parameters[0].Type.ReplaceWithNestedOriginalDefinition(), SymbolEqualityComparer.Default)
            .ToDictionary(m => m.Key, m => m.ToArray(), SymbolEqualityComparer.Default);
    }

    /// <summary>
    ///     Get all static classes in a namespace.
    /// </summary>
    /// <param name="namespaceSymbol">namespace.</param>
    /// <returns>result.</returns>
    public static IEnumerable<INamedTypeSymbol> GetAllStaticClasses(this INamespaceSymbol namespaceSymbol)
    {
        return namespaceSymbol.GetAllTypes().Where(t => t.IsStatic && t.TypeKind == TypeKind.Class);
    }

    /// <summary>
    ///     Get all types in a namespace.
    /// </summary>
    /// <param name="namespaceSymbol">namespace.</param>
    /// <returns>result.</returns>
    /// <exception cref="ArgumentNullException">namespaceSymbol is null.</exception>
    public static IEnumerable<INamedTypeSymbol> GetAllTypes(this INamespaceSymbol namespaceSymbol)
    {
        if (namespaceSymbol is null)
        {
            throw new ArgumentNullException(nameof(namespaceSymbol));
        }

        return GetAllTypesPrivate(namespaceSymbol);

        IEnumerable<INamedTypeSymbol> GetAllTypesPrivate(INamespaceSymbol namespaceSymbol)
        {
            foreach (var typeMember in namespaceSymbol.GetTypeMembers())
            {
                yield return typeMember;
            }

            foreach (var nestedNamespace in namespaceSymbol.GetNamespaceMembers())
            {
                foreach (var nestTypeMember in GetAllTypesPrivate(nestedNamespace))
                {
                    yield return nestTypeMember;
                }
            }
        }
    }

    /// <summary>
    ///     Replace the type with nested original definition.
    /// </summary>
    /// <param name="symbol">symbol.</param>
    /// <returns>result.</returns>
    /// <exception cref="ArgumentNullException">symbol is null.</exception>
    public static ITypeSymbol ReplaceWithNestedOriginalDefinition(this ITypeSymbol symbol)
    {
        if (symbol is null)
        {
            throw new ArgumentNullException(nameof(symbol));
        }

        if (symbol is not INamedTypeSymbol { IsGenericType: true, } named)
        {
            return symbol;
        }

        if (named.TypeArguments.All(t => t.TypeKind is TypeKind.TypeParameter))
        {
            return symbol.OriginalDefinition;
        }

        var newArgs = named.TypeArguments
            .Select(ReplaceWithNestedOriginalDefinition)
            .ToArray();
        return named.OriginalDefinition.Construct(newArgs);
    }
}