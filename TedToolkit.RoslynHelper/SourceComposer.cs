// -----------------------------------------------------------------------
// <copyright file="SourceComposer.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;
using System.Runtime.CompilerServices;

using Microsoft.CodeAnalysis;

using TedToolkit.RoslynHelper.Syntaxes;

namespace TedToolkit.RoslynHelper;

/// <summary>
/// The Source Composer.
/// </summary>
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
public static class SourceComposer
{
    /// <summary>
    /// Create a file.
    /// </summary>
    /// <returns>file.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static SourceFile File()
    {
        return new();
    }

    /// <summary>
    /// Create a namespace.
    /// </summary>
    /// <param name="nameSpace">the namespace.</param>
    /// <returns>namespace.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NameSpace NameSpace(IExpression nameSpace)
    {
        return new(nameSpace);
    }

    /// <summary>
    /// Create a namespace.
    /// </summary>
    /// <param name="nameSpace">the namespace.</param>
    /// <returns>namespace.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NameSpace NameSpace(in ReadOnlySpan<string> nameSpace)
    {
        return new(nameSpace);
    }

    /// <summary>
    /// Create a namespace.
    /// </summary>
    /// <param name="nameSpace">the namespace.</param>
    /// <returns>namespace.</returns>
    /// <exception cref="ArgumentNullException">the namespace is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static NameSpace NameSpace(string nameSpace)
    {
        return new(nameSpace);
    }

    /// <summary>
    /// Create an argument by parameter info.
    /// </summary>
    /// <param name="parameterInfo">the info.</param>
    /// <returns>argument.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Argument Argument(ParameterInfo parameterInfo)
    {
        return Syntaxes.Argument.FromInfo(parameterInfo);
    }

    /// <summary>
    /// Create an argument.
    /// </summary>
    /// <param name="variable">the variable.</param>
    /// <returns>argument.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Argument Argument(IExpression variable)
    {
        return new(variable);
    }

    /// <summary>
    /// Create the parameter.
    /// </summary>
    /// <param name="identifier">parameter name.</param>
    /// <typeparam name="T">type of the parameter.</typeparam>
    /// <returns>parameter.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Parameter Parameter<T>(string identifier)
    {
        return Parameter(typeof(T), identifier);
    }

    /// <summary>
    /// Create the parameter.
    /// </summary>
    /// <param name="type">the type.</param>
    /// <param name="identifier">parameter name.</param>
    /// <returns>parameter.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Parameter Parameter(Type type, string identifier)
    {
        return new(type, identifier);
    }

    /// <summary>
    /// Create the parameter.
    /// </summary>
    /// <param name="type">the type.</param>
    /// <param name="identifier">parameter name.</param>
    /// <returns>parameter.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Parameter Parameter(DataType type, string identifier)
    {
        return new(type, identifier);
    }

    /// <summary>
    /// Create the parameter.
    /// </summary>
    /// <param name="parameterSymbol">the parameter symbol.</param>
    /// <param name="compilation">compilation.</param>
    /// <returns>parameter.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Parameter Parameter(IParameterSymbol parameterSymbol, Compilation? compilation = null)
    {
        return Syntaxes.Parameter.FromSymbol(parameterSymbol, compilation);
    }

    /// <summary>
    /// Create the parameter.
    /// </summary>
    /// <param name="parameterSymbol">the parameter symbol.</param>
    /// <param name="type">data type.</param>
    /// <param name="compilation">compilation.</param>
    /// <returns>parameter.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Parameter Parameter(IParameterSymbol parameterSymbol, DataType type, Compilation? compilation = null)
    {
        return Syntaxes.Parameter.FromSymbol(parameterSymbol, type, compilation);
    }

    /// <summary>
    /// Create the parameter.
    /// </summary>
    /// <param name="parameterInfo">the parameter info.</param>
    /// <param name="alias">alias.</param>
    /// <returns>parameter.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Parameter Parameter(ParameterInfo parameterInfo, string alias = "global")
    {
        return Syntaxes.Parameter.FromInfo(parameterInfo, alias);
    }

    /// <summary>
    /// Create the parameter.
    /// </summary>
    /// <param name="parameterInfo">the parameter info.</param>
    /// <param name="type">data type.</param>
    /// <returns>parameter.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Parameter Parameter(ParameterInfo parameterInfo, DataType type)
    {
        return Syntaxes.Parameter.FromInfo(parameterInfo, type);
    }

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
    {
        return new(type);
    }

    /// <summary>
    /// Create an attribute.
    /// </summary>
    /// <param name="data">attribute data.</param>
    /// <param name="compilation">compilation.</param>
    /// <returns>attribute.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Syntaxes.Attribute Attribute(AttributeData data, Compilation? compilation = null)
    {
        return Syntaxes.Attribute.FromSymbol(data, compilation);
    }

    /// <summary>
    /// Create the returnType.
    /// </summary>
    /// <param name="type">the Type.</param>
    /// <returns>parameter.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ReturnType ReturnType(DataType type)
    {
        return new(type);
    }

    /// <summary>
    /// Create a type parameter.
    /// </summary>
    /// <param name="identifier">identifier.</param>
    /// <returns>attribute.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypeParameter TypeParameter(string identifier)
    {
        return new(identifier);
    }

    /// <summary>
    /// Create a type parameter.
    /// </summary>
    /// <param name="symbol">symbol.</param>
    /// <param name="compilation">compilation.</param>
    /// <returns>attribute.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypeParameter TypeParameter(ITypeParameterSymbol symbol, Compilation? compilation = null)
    {
        return Syntaxes.TypeParameter.FromSymbol(symbol, compilation);
    }

    /// <summary>
    /// Create an extension.
    /// </summary>
    /// <param name="parameter">parameter.</param>
    /// <returns>extension.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Extension Extension(Parameter parameter)
    {
        return new(parameter);
    }
}
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type