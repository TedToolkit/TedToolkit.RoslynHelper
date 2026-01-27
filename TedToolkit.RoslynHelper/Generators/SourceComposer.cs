// -----------------------------------------------------------------------
// <copyright file="SourceComposer.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

using Microsoft.CodeAnalysis;

using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The Source Composer.
/// </summary>
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
public static class SourceComposer
{
    /// <summary>
    /// Create a file.
    /// </summary>
    /// <returns>class.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SourceFile File()
        => new();

    /// <summary>
    /// Create a namespace.
    /// </summary>
    /// <param name="nameSpace">the namespace.</param>
    /// <returns>namespace.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NameSpace NameSpace(IExpression nameSpace)
        => new(nameSpace);

    /// <summary>
    /// Create a namespace.
    /// </summary>
    /// <param name="nameSpace">the namespace.</param>
    /// <returns>namespace.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NameSpace NameSpace(in ReadOnlySpan<string> nameSpace)
        => new(nameSpace);

    /// <summary>
    /// Create a namespace.
    /// </summary>
    /// <param name="nameSpace">the namespace.</param>
    /// <returns>namespace.</returns>
    /// <exception cref="ArgumentNullException">the namespace is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NameSpace NameSpace(string nameSpace)
        => new(nameSpace);

    /// <summary>
    /// Create an argument.
    /// </summary>
    /// <param name="variable">the variable.</param>
    /// <returns>namespace.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Argument Argument(IExpression variable)
        => new(variable);

    /// <summary>
    /// Create the parameter.
    /// </summary>
    /// <param name="identifier">parameter name.</param>
    /// <typeparam name="T">type of the parameter.</typeparam>
    /// <returns>parameter.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Parameter Parameter<T>(string identifier)
        => Parameter(typeof(T), identifier);

    /// <summary>
    /// Create the parameter.
    /// </summary>
    /// <param name="type">the type.</param>
    /// <param name="identifier">parameter name.</param>
    /// <returns>parameter.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Parameter Parameter(Type type, string identifier)
        => new(type, identifier);

    /// <summary>
    /// Create the parameter.
    /// </summary>
    /// <param name="type">the type.</param>
    /// <param name="identifier">parameter name.</param>
    /// <returns>parameter.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Parameter Parameter(DataType type, string identifier)
        => new(type, identifier);

    /// <summary>
    /// Create the parameter.
    /// </summary>
    /// <param name="parameterSymbol">the parameter symbol.</param>
    /// <returns>parameter.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Parameter Parameter(IParameterSymbol parameterSymbol)
        => Syntaxes.Parameter.FromSymbol(parameterSymbol);

    /// <summary>
    /// Create the parameter.
    /// </summary>
    /// <param name="parameterSymbol">the parameter symbol.</param>
    /// <param name="type">data type.</param>
    /// <returns>parameter.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Parameter Parameter(IParameterSymbol parameterSymbol, DataType type)
        => Syntaxes.Parameter.FromSymbol(parameterSymbol, type);

    /// <summary>
    /// Create an attribute.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <returns>attribute.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Syntaxes.Attribute Attribute<T>()
        where T : System.Attribute
    {
        return new(typeof(T));
    }

    /// <summary>
    /// Create an attribute.
    /// </summary>
    /// <param name="type">Type.</param>
    /// <returns>attribute.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Syntaxes.Attribute Attribute(DataType type)
        => new(type);

    /// <summary>
    /// Create the returnType.
    /// </summary>
    /// <param name="type">the Type.</param>
    /// <returns>parameter.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReturnType ReturnType(DataType type)
        => new(type);

    /// <summary>
    /// Create a type parameter.
    /// </summary>
    /// <param name="identifier">identifier.</param>
    /// <returns>attribute.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypeParameter TypeParameter(string identifier)
        => new(identifier);
}
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type