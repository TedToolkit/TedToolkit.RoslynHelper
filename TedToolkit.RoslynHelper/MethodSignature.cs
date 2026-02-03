// -----------------------------------------------------------------------
// <copyright file="MethodSignature.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.CodeAnalysis;

namespace TedToolkit.RoslynHelper;

/// <summary>
///     The signature of a method.
/// </summary>
/// <param name="methodSymbol">The method symbol.</param>
[Obsolete("Do not use this method, try to use the generators instead!")]
public readonly struct MethodSignature(IMethodSymbol methodSymbol) : IEquatable<MethodSignature>
{
    /// <summary>
    ///     Gets or sets a value indicating whether should check the equality with containing type.
    /// </summary>
    public static bool EqualityWithContainingType { get; set; } = true;

    /// <summary>
    ///     Gets the name of the method.
    /// </summary>
    public string MethodName { get; } = methodSymbol.Name;

    /// <summary>
    ///     Gets the containing type.
    /// </summary>
    public ITypeSymbol ContainingType { get; } = methodSymbol.IsExtensionMethod
        ? methodSymbol.Parameters[0].Type.OriginalDefinition
        : methodSymbol.ContainingType.OriginalDefinition;

    /// <summary>
    ///     Gets tye parameter types.
    /// </summary>
    public IReadOnlyList<ITypeSymbol> ParameterTypes { get; } = methodSymbol.Parameters
        .Skip(methodSymbol.IsExtensionMethod ? 1 : 0)
        .Select(p => p.Type.OriginalDefinition)
        .ToArray();

    /// <summary>
    ///     Gets the Ref Kinds.
    /// </summary>
    public IReadOnlyList<RefKind> RefKinds { get; } = methodSymbol.Parameters
        .Skip(methodSymbol.IsExtensionMethod ? 1 : 0)
        .Select(i => i.RefKind)
        .ToArray();

    /// <summary>
    ///     Gets the type Parameter counts.
    /// </summary>
    public int TypeArgumentsCount { get; } =
        methodSymbol.TypeArguments.Length + methodSymbol.ContainingType.TypeArguments.Length;

    /// <inheritdoc />
    public bool Equals(MethodSignature other)
    {
        if (!MethodName.Equals(other.MethodName, StringComparison.Ordinal))
            return false;

        if (!TypeArgumentsCount.Equals(other.TypeArgumentsCount))
            return false;

        if (EqualityWithContainingType
            && !ContainingType.Equals(other.ContainingType, SymbolEqualityComparer.Default))
        {
            return false;
        }

        if (!ParameterTypes.Count.Equals(other.ParameterTypes.Count))
            return false;

        for (var i = 0; i < ParameterTypes.Count; i++)
        {
            var thisType = ParameterTypes[i];
            var otherType = other.ParameterTypes[i];
            if (thisType.TypeKind == TypeKind.TypeParameter
                && otherType.TypeKind == TypeKind.TypeParameter)
            {
                continue;
            }

            if (RefKinds[i] != other.RefKinds[i])
                return false;

            if (!thisType.Equals(otherType, SymbolEqualityComparer.Default))
                return false;
        }

        return true;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
        => obj is MethodSignature other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = MethodName.GetHashCode();
            hashCode = (hashCode * 397) ^ ParameterTypes.Count.GetHashCode();
            hashCode = (hashCode * 397) ^ TypeArgumentsCount.GetHashCode();
            return hashCode;
        }
    }

    /// <summary>
    /// Equal.
    /// </summary>
    /// <param name="left">left.</param>
    /// <param name="right">right.</param>
    /// <returns>result.</returns>
    public static bool operator ==(in MethodSignature left, in MethodSignature right)
        => left.Equals(right);

    /// <summary>
    /// Not Equal.
    /// </summary>
    /// <param name="left">left.</param>
    /// <param name="right">right.</param>
    /// <returns>result.</returns>
    public static bool operator !=(in MethodSignature left, in MethodSignature right)
        => !(left == right);
}