// -----------------------------------------------------------------------
// <copyright file="DataType.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The data Type
/// </summary>
/// <param name="Type">expression to the type</param>
public record struct DataType(IExpression Type) :
    IStorageKind,
    IToCode
{
    /// <inheritdoc />
    public StorageKind StorageKind { get; set; }

    /// <summary>
    /// Is Array
    /// </summary>
    public bool IsArray { get; set; }

    /// <summary>
    /// The pointer counter
    /// </summary>
    public int PointCounter { get; set; }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddStorageKind(ref builder);
        Type.ToCode(ref builder);
        if (IsArray)
            builder.Append("[]");

        if (PointCounter > 0)
            builder.Append('*', PointCounter);
    }

    /// <summary>
    /// From Type
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <param name="result">result</param>
    /// <returns>Expression</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref DataType FromType<T>(in DataType result = default)
        => ref FromType(typeof(T), result);

    /// <summary>
    /// From the type
    /// </summary>
    /// <param name="type">type</param>
    /// <param name="result">result</param>
    /// <returns>result</returns>
    /// <exception cref="ArgumentNullException">type is null</exception>
    public static ref DataType FromType(Type type, in DataType result = default)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type));

        ref var instance = ref Unsafe.AsRef(in result);
        if (_typeAlias.TryGetValue(type, out var s))
        {
            instance = s;
            return ref instance;
        }

        if (type.IsArray)
        {
            _ = FromType(type.GetElementType()!, result).Array;
            return ref instance;
        }

        if (type.IsGenericType)
        {
            if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                _ = FromType(Nullable.GetUnderlyingType(type)!).Null;
                return ref instance;
            }

            instance.Type = SimpleType()
                .Generic([.. type.GetGenericArguments().Select(i => FromType(i)),]);
            return ref instance;
        }

        instance.Type = SimpleType();
        return ref instance;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IExpression SimpleType()
        {
            var name = new SimpleNameExpression(type.Name.Split('`')[0]);
            if (string.IsNullOrEmpty(type.Namespace))
                return name;

            return new MemberAccessExpression(new SimpleNameExpression(type.Namespace), name);
        }
    }

    private static readonly Dictionary<Type, DataType> _typeAlias = new()
    {
        { typeof(bool), DataTypes.Bool },
        { typeof(byte), DataTypes.Byte },
        { typeof(char), DataTypes.Char },
        { typeof(decimal), DataTypes.Decimal },
        { typeof(double), DataTypes.Double },
        { typeof(float), DataTypes.Float },
        { typeof(int), DataTypes.Int },
        { typeof(long), DataTypes.Long },
        { typeof(object), DataTypes.Object },
        { typeof(sbyte), DataTypes.Sbyte },
        { typeof(short), DataTypes.Short },
        { typeof(string), DataTypes.String },
        { typeof(uint), DataTypes.Uint },
        { typeof(ulong), DataTypes.Ulong },
        { typeof(ushort), DataTypes.Ushort },
        { typeof(void), DataTypes.Void },
    };
}