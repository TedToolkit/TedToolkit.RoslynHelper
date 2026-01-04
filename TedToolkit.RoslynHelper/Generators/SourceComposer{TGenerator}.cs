// -----------------------------------------------------------------------
// <copyright file="SourceComposer{TGenerator}.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

using TedToolkit.RoslynHelper.Generators.Types;

namespace TedToolkit.RoslynHelper.Generators;
#pragma warning disable CA1000

/// <summary>
/// Generator
/// </summary>
/// <typeparam name="TGenerator">Your generator</typeparam>
public static class SourceComposer<TGenerator>
{
    private static readonly string _toolName = typeof(TGenerator).FullName ?? typeof(TGenerator).Name;

    private static readonly string _version = typeof(TGenerator).Assembly.GetName().Version.ToString();

    private static void AddGeneratorAttribute<T>(ref T item)
        where T : struct, IAttributes
    {
        item.AddAttribute(SourceComposer.Attribute<GeneratedCodeAttribute>()
            .AddArgument(SourceComposer.Argument(_toolName))
            .AddArgument(SourceComposer.Argument(_version)));
    }

    /// <summary>
    /// Create a <see langword="class"/>
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <param name="result">result</param>
    /// <returns>class</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TypeDeclaration Class(string identifier, in TypeDeclaration result = default)
        => ref TypeDeclaration(identifier, TypeDeclarationType.CLASS, result);

    /// <summary>
    /// Create a <see langword="struct"/>
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <param name="result">result</param>
    /// <returns>class</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TypeDeclaration Struct(string identifier, in TypeDeclaration result = default)
        => ref TypeDeclaration(identifier, TypeDeclarationType.STRUCT, result);

    /// <summary>
    /// Create a <see langword="ref"/> <see langword="struct"/>
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <param name="result">result</param>
    /// <returns>class</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TypeDeclaration RefStruct(string identifier, in TypeDeclaration result = default)
        => ref TypeDeclaration(identifier, TypeDeclarationType.REF_STRUCT, result);

    /// <summary>
    /// Create a <see langword="record"/>
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <param name="result">result</param>
    /// <returns>class</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TypeDeclaration Record(string identifier, in TypeDeclaration result = default)
        => ref TypeDeclaration(identifier, TypeDeclarationType.RECORD, result);

    /// <summary>
    /// Create a <see langword="record"/> <see langword="struct"/>
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <param name="result">result</param>
    /// <returns>class</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TypeDeclaration RecordStruct(string identifier, in TypeDeclaration result = default)
        => ref TypeDeclaration(identifier, TypeDeclarationType.RECORD_STRUCT, result);

    /// <summary>
    /// Create a <see langword="interface"/>
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <param name="result">result</param>
    /// <returns>class</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TypeDeclaration Interface(string identifier, in TypeDeclaration result = default)
        => ref TypeDeclaration(identifier, TypeDeclarationType.INTERFACE, result);

    private static ref TypeDeclaration TypeDeclaration(string identifier, TypeDeclarationType type,
        in TypeDeclaration result)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Identifier = identifier;
        instance.Type = type;
        AddGeneratorAttribute(ref instance);
        return ref instance;
    }

    /// <summary>
    /// Create the Event
    /// </summary>
    /// <param name="type">type</param>
    /// <param name="identifier">identifier</param>
    /// <param name="result">result</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Event Event(
        MemberAccess type,
        string identifier,
        in Event result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Type = type;
        instance.Identifier = identifier;
        AddGeneratorAttribute(ref instance);
        return ref instance;
    }

    /// <summary>
    /// Create the Event
    /// </summary>
    /// <param name="type">type</param>
    /// <param name="identifier">identifier</param>
    /// <param name="result">result</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Event Event(
        Type type,
        string identifier,
        in Event result = default)
    {
        return ref Event(SourceComposer.Type(type), identifier, result);
    }

    /// <summary>
    /// Create the Event
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="identifier">identifier</param>
    /// <param name="result">result</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Event Event<T>(
        string identifier,
        in Event result = default)
    {
        return ref Event(SourceComposer.Type<T>(), identifier, result);
    }

    /// <summary>
    /// Create the Field
    /// </summary>
    /// <param name="type">type</param>
    /// <param name="identifier">identifier</param>
    /// <param name="result">result</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Field Field(
        MemberAccess type,
        string identifier,
        in Field result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Type = type;
        instance.Identifier = identifier;
        AddGeneratorAttribute(ref instance);
        return ref instance;
    }

    /// <summary>
    /// Create the Field
    /// </summary>
    /// <param name="type">type</param>
    /// <param name="identifier">identifier</param>
    /// <param name="result">result</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Field Field(
        Type type,
        string identifier,
        in Field result = default)
    {
        return ref Field(SourceComposer.Type(type), identifier, result);
    }

    /// <summary>
    /// Create the Field
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="identifier">identifier</param>
    /// <param name="result">result</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Field Field<T>(
        string identifier,
        in Field result = default)
    {
        return ref Field(SourceComposer.Type<T>(), identifier, result);
    }

    /// <summary>
    /// Create the Accessor
    /// </summary>
    /// <param name="type">accessor type</param>
    /// <param name="result">result</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Accessor Accessor(
        AccessorType type,
        in Accessor result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Type = type;
        AddGeneratorAttribute(ref instance);
        return ref instance;
    }

    /// <summary>
    /// Create the property
    /// </summary>
    /// <param name="identifier">parameter name</param>
    /// <param name="type">return returnType</param>
    /// <param name="result">result</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Property Property(
        string identifier,
        scoped in MemberAccess type,
        in Property result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Identifier = identifier;
        instance.Type = type;
        AddGeneratorAttribute(ref instance);
        return ref instance;
    }

    /// <summary>
    /// Create the method
    /// </summary>
    /// <param name="identifier">parameter name</param>
    /// <param name="returnType">return returnType</param>
    /// <param name="result">result</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Method Method(
        string identifier,
        scoped in ReturnType? returnType = null,
        in Method result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Identifier = identifier;
        instance.ReturnType = returnType;
        AddGeneratorAttribute(ref instance);
        return ref instance;
    }
}