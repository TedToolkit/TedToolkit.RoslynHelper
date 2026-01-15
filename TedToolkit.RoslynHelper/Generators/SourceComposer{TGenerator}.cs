// -----------------------------------------------------------------------
// <copyright file="SourceComposer{TGenerator}.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Generators;
#pragma warning disable CA1000

/// <summary>
/// Generator
/// </summary>
/// <typeparam name="TGenerator">Your generator</typeparam>
public static class SourceComposer<TGenerator>
{
    private static readonly LiteralExpression
        _toolName = typeof(TGenerator).GetToolName();

    private static readonly LiteralExpression _version =
        typeof(TGenerator).GetVersion();

    private static void AddGeneratorAttribute<T>(ref T item)
        where T : class, IAttributes
    {
        item.AddGeneratorAttribute(_toolName, _version);
    }

    /// <summary>
    /// Create a <see langword="class"/>
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <returns>class</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypeDeclaration Class(string identifier)
        => TypeDeclaration(identifier, TypeDeclarationType.CLASS);

    /// <summary>
    /// Create a <see langword="struct"/>
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <returns>class</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypeDeclaration Struct(string identifier)
        => TypeDeclaration(identifier, TypeDeclarationType.STRUCT);

    /// <summary>
    /// Create a <see langword="ref"/> <see langword="struct"/>
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <returns>class</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypeDeclaration RefStruct(string identifier)
        => TypeDeclaration(identifier, TypeDeclarationType.REF_STRUCT);

    /// <summary>
    /// Create a <see langword="record"/>
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <returns>class</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypeDeclaration Record(string identifier)
        => TypeDeclaration(identifier, TypeDeclarationType.RECORD);

    /// <summary>
    /// Create a <see langword="record"/> <see langword="struct"/>
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <returns>class</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypeDeclaration RecordStruct(string identifier)
        => TypeDeclaration(identifier, TypeDeclarationType.RECORD_STRUCT);

    /// <summary>
    /// Create a <see langword="interface"/>
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <returns>class</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypeDeclaration Interface(string identifier)
        => TypeDeclaration(identifier, TypeDeclarationType.INTERFACE);

    private static TypeDeclaration TypeDeclaration(string identifier, TypeDeclarationType type)
    {
        var instance = new TypeDeclaration(identifier, type);
        AddGeneratorAttribute(ref instance);
        return instance;
    }

    /// <summary>
    /// Create the Event
    /// </summary>
    /// <param name="type">type</param>
    /// <param name="identifier">identifier</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Event Event(DataType type, string identifier)
    {
        var instance = new Event(type, identifier);
        AddGeneratorAttribute(ref instance);
        return instance;
    }

    /// <summary>
    /// Create the Event
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="identifier">identifier</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Event Event<T>(string identifier)
        => Event(typeof(T), identifier);

    /// <summary>
    /// Create the Field
    /// </summary>
    /// <param name="type">type</param>
    /// <param name="identifier">identifier</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Field Field(DataType type, string identifier)
    {
        var instance = new Field(type, identifier);
        AddGeneratorAttribute(ref instance);
        return instance;
    }

    /// <summary>
    /// Create the Field
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="identifier">identifier</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Field Field<T>(string identifier)
        => Field(typeof(T), identifier);

    /// <summary>
    /// Create the Accessor
    /// </summary>
    /// <param name="type">accessor type</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Accessor Accessor(AccessorType type)
    {
        var instance = new Accessor(type);
        AddGeneratorAttribute(ref instance);
        return instance;
    }

    /// <summary>
    /// Create the property
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="identifier">parameter name</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Property Property<T>(string identifier)
        => Property(DataType.FromType<T>(), identifier);

    /// <summary>
    /// Create the property
    /// </summary>
    /// <param name="type">return returnType</param>
    /// <param name="identifier">parameter name</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Property Property(DataType type, string identifier)
    {
        var instance = new Property(type, identifier);
        AddGeneratorAttribute(ref instance);
        return instance;
    }

    /// <summary>
    /// Create the conversion
    /// </summary>
    /// <param name="type">return returnType</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Conversion ImplicitConversionTo(DataType type)
    {
        var instance = new Conversion(type, false, true);
        AddGeneratorAttribute(ref instance);
        return instance;
    }

    /// <summary>
    /// Create the conversion
    /// </summary>
    /// <param name="type">return returnType</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Conversion ImplicitConversionFrom(DataType type)
    {
        var instance = new Conversion(type, true, true);
        AddGeneratorAttribute(ref instance);
        return instance;
    }

    /// <summary>
    /// Create the conversion
    /// </summary>
    /// <param name="type">return returnType</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Conversion ExplicitConversionTo(DataType type)
    {
        var instance = new Conversion(type, false, false);
        AddGeneratorAttribute(ref instance);
        return instance;
    }

    /// <summary>
    /// Create the conversion
    /// </summary>
    /// <param name="type">return returnType</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Conversion ExplicitConversionFrom(DataType type)
    {
        var instance = new Conversion(type, true, false);
        AddGeneratorAttribute(ref instance);
        return instance;
    }

    /// <summary>
    /// Create the method
    /// </summary>
    /// <param name="identifier">parameter name</param>
    /// <param name="returnType">return returnType</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Method Method(string identifier, ReturnType? returnType = null)
    {
        var instance = new Method(identifier, returnType);
        AddGeneratorAttribute(ref instance);
        return instance;
    }

    /// <summary>
    /// Create the method
    /// </summary>
    /// <param name="identifier">parameter name</param>
    /// <param name="returnType">return returnType</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Syntaxes.Delegate Delegate(string identifier, ReturnType? returnType = null)
    {
        var instance = new Syntaxes.Delegate(identifier, returnType);
        AddGeneratorAttribute(ref instance);
        return instance;
    }

    /// <summary>
    /// Create a constructor
    /// </summary>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Constructor Constructor()
    {
        var instance = new Constructor();
        AddGeneratorAttribute(ref instance);
        return instance;
    }

    /// <summary>
    /// Create an operator
    /// </summary>
    /// <param name="returnType">return type</param>
    /// <param name="identifier">parameter name</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Operator Operator(ReturnType returnType, string identifier)
    {
        var instance = new Operator(returnType, identifier);
        AddGeneratorAttribute(ref instance);
        return instance;
    }

    /// <summary>
    /// Create the Enum
    /// </summary>
    /// <param name="identifier">enum name</param>
    /// <param name="dataType">return type</param>
    /// <returns>enum</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Syntaxes.Enum Enum(string identifier, DataType? dataType = null)
    {
        var instance = new Syntaxes.Enum(identifier, dataType);
        AddGeneratorAttribute(ref instance);
        return instance;
    }

    /// <summary>
    /// Create an enum member
    /// </summary>
    /// <param name="identifier">enum name</param>
    /// <param name="value">default value</param>
    /// <returns>enum member</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Syntaxes.EnumMember EnumMember(string identifier, IExpression? value = null)
    {
        var instance = new Syntaxes.EnumMember(identifier, value);
        AddGeneratorAttribute(ref instance);
        return instance;
    }

    /// <summary>
    /// Create an indexer
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <returns>indexer</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Indexer Indexer<T>()
        => Indexer(DataType.FromType<T>());

    /// <summary>
    /// Create an indexer
    /// </summary>
    /// <param name="type">the type</param>
    /// <returns>indexer</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Indexer Indexer(DataType type)
    {
        var instance = new Indexer(type);
        AddGeneratorAttribute(ref instance);
        return instance;
    }
}