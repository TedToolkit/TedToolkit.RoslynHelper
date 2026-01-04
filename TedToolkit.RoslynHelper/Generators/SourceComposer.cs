// -----------------------------------------------------------------------
// <copyright file="SourceComposer.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using System.Runtime.CompilerServices;

using Cysharp.Text;

using TedToolkit.RoslynHelper.Generators.Delegates;
using TedToolkit.RoslynHelper.Generators.Types;

using Attribute = System.Attribute;

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
    /// <param name="nameSpace">nameSpace</param>
    /// <param name="result">result</param>
    /// <returns>class</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref SourceFile File(string fileName, MemberAccess nameSpace, in SourceFile result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.FileName = fileName;
        instance.NameSpace = nameSpace;
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
        return ref Parameter(Type<T>(), identifier, result);
    }

    /// <summary>
    /// Create the parameter
    /// </summary>
    /// <param name="type">the type</param>
    /// <param name="identifier">parameter name</param>
    /// <param name="result">result</param>
    /// <returns>parameter</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Parameter Parameter(Type type, string identifier,
        in Parameter result = default)
    {
        return ref Parameter(Type(type), identifier, result);
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
        scoped in MemberAccess type,
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
        where T : Attribute
    {
        return ref Attribute(Type<T>(), result);
    }

    /// <summary>
    /// Create an attribute.
    /// </summary>
    /// <param name="type">Type</param>
    /// <param name="result">result</param>
    /// <returns>attribute</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Types.Attribute Attribute(
        Type type,
        in Types.Attribute result = default)
    {
        return ref Attribute(Type(type), result);
    }

    /// <summary>
    /// Create an attribute.
    /// </summary>
    /// <param name="type">Type</param>
    /// <param name="result">result</param>
    /// <returns>attribute</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Types.Attribute Attribute(
        scoped in MemberAccess type,
        in Types.Attribute result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Type = type;
        return ref instance;
    }

    /// <summary>
    /// Create the argument
    /// </summary>
    /// <param name="variable">variable</param>
    /// <param name="result">result</param>
    /// <returns>result</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Argument Argument(
        scoped in MemberAccess variable,
        in Argument result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Variable = variable;
        return ref instance;
    }

    /// <inheritdoc cref="Argument(in MemberAccess, in TedToolkit.RoslynHelper.Generators.Types.Argument)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Argument Argument(
        string variable,
        in Argument result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Variable = ZString.Concat('"', variable, '"');
        return ref instance;
    }

    /// <inheritdoc cref="Argument(in MemberAccess, in TedToolkit.RoslynHelper.Generators.Types.Argument)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Argument Argument(
        char variable,
        in Argument result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Variable = ZString.Concat('\'', variable, '\'');
        return ref instance;
    }

    /// <inheritdoc cref="Argument(in MemberAccess, in TedToolkit.RoslynHelper.Generators.Types.Argument)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Argument Argument(
        int variable,
        in Argument result = default)
    {
        return ref Argument((MemberAccess)variable.ToString(CultureInfo.InvariantCulture), result);
    }

    /// <inheritdoc cref="Argument(in MemberAccess, in TedToolkit.RoslynHelper.Generators.Types.Argument)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Argument Argument(
        long variable,
        in Argument result = default)
    {
        return ref Argument((MemberAccess)variable.ToString(CultureInfo.InvariantCulture), result);
    }

    /// <inheritdoc cref="Argument(in MemberAccess, in TedToolkit.RoslynHelper.Generators.Types.Argument)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Argument Argument(
        uint variable,
        in Argument result = default)
    {
        return ref Argument((MemberAccess)variable.ToString(CultureInfo.InvariantCulture), result);
    }

    /// <inheritdoc cref="Argument(in MemberAccess, in TedToolkit.RoslynHelper.Generators.Types.Argument)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Argument Argument(
        ulong variable,
        in Argument result = default)
    {
        return ref Argument((MemberAccess)variable.ToString(CultureInfo.InvariantCulture), result);
    }

    /// <inheritdoc cref="Argument(in MemberAccess, in TedToolkit.RoslynHelper.Generators.Types.Argument)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Argument Argument(
        byte variable,
        in Argument result = default)
    {
        return ref Argument((MemberAccess)variable.ToString(CultureInfo.InvariantCulture), result);
    }

    /// <inheritdoc cref="Argument(in MemberAccess, in TedToolkit.RoslynHelper.Generators.Types.Argument)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Argument Argument(
        sbyte variable,
        in Argument result = default)
    {
        return ref Argument((MemberAccess)variable.ToString(CultureInfo.InvariantCulture), result);
    }

    /// <inheritdoc cref="Argument(in MemberAccess, in TedToolkit.RoslynHelper.Generators.Types.Argument)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Argument Argument(
        short variable,
        in Argument result = default)
    {
        return ref Argument((MemberAccess)variable.ToString(CultureInfo.InvariantCulture), result);
    }

    /// <inheritdoc cref="Argument(in MemberAccess, in TedToolkit.RoslynHelper.Generators.Types.Argument)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Argument Argument(
        ushort variable,
        in Argument result = default)
    {
        return ref Argument((MemberAccess)variable.ToString(CultureInfo.InvariantCulture), result);
    }

    /// <inheritdoc cref="Argument(in MemberAccess, in TedToolkit.RoslynHelper.Generators.Types.Argument)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Argument Argument(
        double variable,
        in Argument result = default)
    {
        return ref Argument((MemberAccess)variable.ToString(CultureInfo.InvariantCulture), result);
    }

    /// <inheritdoc cref="Argument(in MemberAccess, in TedToolkit.RoslynHelper.Generators.Types.Argument)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Argument Argument(
        float variable,
        in Argument result = default)
    {
        return ref Argument((MemberAccess)variable.ToString(CultureInfo.InvariantCulture), result);
    }

    /// <inheritdoc cref="Argument(in MemberAccess, in TedToolkit.RoslynHelper.Generators.Types.Argument)"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref Argument Argument(
        decimal variable,
        in Argument result = default)
    {
        return ref Argument((MemberAccess)variable.ToString(CultureInfo.InvariantCulture), result);
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
        scoped in MemberAccess type,
        StorageKind storageKind = StorageKind.NONE,
        in ReturnType result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Type = type;
        instance.StorageKind = storageKind;
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