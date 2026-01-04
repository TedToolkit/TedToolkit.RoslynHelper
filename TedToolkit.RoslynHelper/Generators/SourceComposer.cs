// -----------------------------------------------------------------------
// <copyright file="SourceComposer.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

using TedToolkit.RoslynHelper.Generators.Delegates;

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
    /// Create a <see langword="class"/>
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <param name="result">result</param>
    /// <returns>class</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TypeDeclaration Class(string identifier, in TypeDeclaration result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Identifier = identifier;
        instance.Type = TypeDeclarationType.CLASS;
        return ref instance;
    }

    /// <summary>
    /// Create a <see langword="struct"/>
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <param name="result">result</param>
    /// <returns>class</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TypeDeclaration Struct(string identifier, in TypeDeclaration result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Identifier = identifier;
        instance.Type = TypeDeclarationType.STRUCT;
        return ref instance;
    }

    /// <summary>
    /// Create a <see langword="ref"/> <see langword="struct"/>
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <param name="result">result</param>
    /// <returns>class</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TypeDeclaration RefStruct(string identifier, in TypeDeclaration result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Identifier = identifier;
        instance.Type = TypeDeclarationType.REF_STRUCT;
        return ref instance;
    }

    /// <summary>
    /// Create a <see langword="record"/>
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <param name="result">result</param>
    /// <returns>class</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TypeDeclaration Record(string identifier, in TypeDeclaration result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Identifier = identifier;
        instance.Type = TypeDeclarationType.RECORD;
        return ref instance;
    }

    /// <summary>
    /// Create a <see langword="record"/> <see langword="struct"/>
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <param name="result">result</param>
    /// <returns>class</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref TypeDeclaration RecordStruct(string identifier, in TypeDeclaration result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Identifier = identifier;
        instance.Type = TypeDeclarationType.RECORD_STRUCT;
        return ref instance;
    }

    /// <summary>
    /// Create the parameter
    /// </summary>
    /// <param name="identifier">parameter name</param>
    /// <param name="modifier">modifier</param>
    /// <param name="result">result</param>
    /// <typeparam name="T">type of the parameter</typeparam>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Parameter Parameter<T>(string identifier, ModifierHandler<Parameter>? modifier = null,
        in Parameter result = default)
    {
        return ref Parameter(Type<T>(), identifier, modifier, result);
    }

    /// <summary>
    /// Create the parameter
    /// </summary>
    /// <param name="type">the type</param>
    /// <param name="identifier">parameter name</param>
    /// <param name="modifier">modifier</param>
    /// <param name="result">result</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Parameter Parameter(Type type, string identifier, ModifierHandler<Parameter>? modifier = null,
        in Parameter result = default)
    {
        return ref Parameter(Type(type), identifier, modifier, result);
    }

    /// <summary>
    /// Create the parameter
    /// </summary>
    /// <param name="type">the type</param>
    /// <param name="identifier">parameter name</param>
    /// <param name="modifier">modifier</param>
    /// <param name="result">result</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Parameter Parameter(
        scoped in MemberAccess type,
        string identifier,
        ModifierHandler<Parameter>? modifier = null,
        in Parameter result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Identifier = identifier;
        instance.Type = type;
        modifier?.Invoke(ref instance);
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
        return ref instance;
    }

    /// <summary>
    /// Create the returnType
    /// </summary>
    /// <param name="type">the Type</param>
    /// <param name="result">result</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref ReturnType ReturnType(
        scoped in MemberAccess type,
        in ReturnType result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Type = type;
        return ref instance;
    }

    /// <summary>
    /// Get the type.
    /// </summary>
    /// <param name="result">result</param>
    /// <typeparam name="T">type</typeparam>
    /// <returns>member access.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref MemberAccess Type<T>(in MemberAccess result = default)
        => ref Type(typeof(T), result);

    /// <summary>
    /// Get the type.
    /// </summary>
    /// <param name="type">type</param>
    /// <param name="result">result</param>
    /// <returns>member access.</returns>
    /// <exception cref="ArgumentNullException">The <paramref name="type"/> is null</exception>
    public static ref MemberAccess Type(Type type, in MemberAccess result = default)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type));

        ref var instance = ref Unsafe.AsRef(in result);
        if (_typeAlias.TryGetValue(type, out var s))
        {
            instance.Items.Add(s);
            return ref instance;
        }

        instance.Alias = "global";
        instance.Items.Add(type.Namespace);
        MemberAccessItem item = default;
        instance.Items.Add(TypeItem(type, ref item));

        return ref instance;
    }

    private static ref MemberAccessItem TypeItem(Type type, ref MemberAccessItem result)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type));

        if (type.IsGenericType)
        {
            if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                ref var item = ref TypeItem(Nullable.GetUnderlyingType(type)!, ref result);
                item.IsNull = true;
            }
            else
            {
                result.Identifier = type.Name.Split('`')[0];
                foreach (var genericArgument in type.GetGenericArguments())
                    result.Types.Add(Type(genericArgument));
            }
        }
        else if (type.IsArray)
        {
            ref var item = ref TypeItem(type.GetElementType()!, ref result);
            item.IsArray = true;
        }
        else
        {
            result.Identifier = type.Name.Split('`')[0];
        }

        return ref result;
    }

    private static readonly Dictionary<Type, string> _typeAlias = new()
    {
        { typeof(bool), "bool" },
        { typeof(byte), "byte" },
        { typeof(char), "char" },
        { typeof(decimal), "decimal" },
        { typeof(double), "double" },
        { typeof(float), "float" },
        { typeof(int), "int" },
        { typeof(long), "long" },
        { typeof(object), "object" },
        { typeof(sbyte), "sbyte" },
        { typeof(short), "short" },
        { typeof(string), "string" },
        { typeof(uint), "uint" },
        { typeof(ulong), "ulong" },
        { typeof(ushort), "ushort" },
        { typeof(void), "void" },
    };
}
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type