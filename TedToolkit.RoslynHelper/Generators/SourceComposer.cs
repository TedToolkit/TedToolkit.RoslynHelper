// -----------------------------------------------------------------------
// <copyright file="SourceComposer.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using System.Runtime.CompilerServices;

using Cysharp.Text;

using TedToolkit.RoslynHelper.Generators.Types;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The Source Composer
/// </summary>
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
public static class SourceComposer
{
    /// <summary>
    /// Create a file
    /// </summary>
    /// <param name="fileName">file name</param>
    /// <param name="result">result</param>
    /// <returns>class</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref SourceFile File(string fileName, in SourceFile result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.FileName = fileName;
        return ref instance;
    }

    /// <summary>
    /// Create a namespace
    /// </summary>
    /// <param name="nameSpace">the namespace</param>
    /// <param name="result">result</param>
    /// <returns>namespace</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref NameSpace NameSpace(IExpression nameSpace, in NameSpace result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Name = nameSpace;
        return ref instance;
    }

    /// <summary>
    /// Create an argument
    /// </summary>
    /// <param name="variable">the variable</param>
    /// <param name="result">result</param>
    /// <returns>namespace</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Argument Argument(IExpression variable, in Argument result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Variable = variable;
        return ref instance;
    }

    /// <summary>
    /// Create the parameter
    /// </summary>
    /// <param name="identifier">parameter name</param>
    /// <param name="result">result</param>
    /// <typeparam name="T">type of the parameter</typeparam>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Parameter Parameter<T>(string identifier,
        in Parameter result = default)
    {
        return ref Parameter(typeof(T), identifier, result);
    }

    /// <summary>
    /// Create the parameter
    /// </summary>
    /// <param name="type">the type</param>
    /// <param name="identifier">parameter name</param>
    /// <param name="result">result</param>
    /// <returns>parameter</returns>
    /// <exception cref="ArgumentNullException">type is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Parameter Parameter(Type type, string identifier,
        in Parameter result = default)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type));

        return ref Parameter(type.ToExpression(), identifier, result);
    }

    /// <summary>
    /// Create the parameter
    /// </summary>
    /// <param name="type">the type</param>
    /// <param name="identifier">parameter name</param>
    /// <param name="result">result</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Parameter Parameter(
        IExpression type,
        string identifier,
        in Parameter result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Identifier = identifier;
        instance.Type = type;
        return ref instance;
    }

    /// <summary>
    /// Create an attribute.
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="result">result</param>
    /// <returns>attribute</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Types.Attribute Attribute<T>(
        in Types.Attribute result = default)
        where T : System.Attribute
    {
        return ref Attribute(typeof(T), result);
    }

    /// <summary>
    /// Create an attribute.
    /// </summary>
    /// <param name="type">Type</param>
    /// <param name="result">result</param>
    /// <returns>attribute</returns>
    /// <exception cref="ArgumentNullException">type is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Types.Attribute Attribute(
        Type type,
        in Types.Attribute result = default)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type));

        return ref Attribute(type.ToExpression(), result);
    }

    /// <summary>
    /// Create an attribute.
    /// </summary>
    /// <param name="type">Type</param>
    /// <param name="result">result</param>
    /// <returns>attribute</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Types.Attribute Attribute(
        IExpression type,
        in Types.Attribute result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Type = type;
        return ref instance;
    }

    /// <summary>
    /// Create the returnType
    /// </summary>
    /// <param name="type">the Type</param>
    /// <param name="storageKind">ref type</param>
    /// <param name="result">result</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref ReturnType ReturnType(
        scoped in MemberAccessExpression type,
        StorageKind storageKind = StorageKind.NONE,
        in ReturnType result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Type = type;
        instance.StorageKind = storageKind;
        return ref instance;
    }
}
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type