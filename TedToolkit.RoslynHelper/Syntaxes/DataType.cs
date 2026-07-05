// -----------------------------------------------------------------------
// <copyright file="DataType.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

using Cysharp.Text;

using Microsoft.CodeAnalysis;

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// The data Type.
/// </summary>
/// <param name="type">expression to the type.</param>
public sealed class DataType(IExpression type) :
    IStorageKind,
    IToCode,
    ICref
{
    /// <summary>
    /// Gets the Type.
    /// </summary>
    public IExpression Type { get; private set; } = type;

    /// <inheritdoc />
    public StorageKind StorageKind { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether is Array.
    /// </summary>
    public bool IsArray { get; set; }

    /// <summary>
    /// Gets make Array.
    /// </summary>
    public DataType Array
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            IsArray = true;
            return this;
        }
    }

    /// <summary>
    /// Gets make Null.
    /// </summary>
    public DataType Null
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            Type = Type.Null;
            return this;
        }
    }

    /// <summary>
    /// Gets create an instance.
    /// </summary>
    public ObjectCreationExpression New
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            return new(this);
        }
    }

    /// <summary>
    /// Gets or sets the pointer counter.
    /// </summary>
    public int PointCounter { get; set; }

    /// <summary>
    /// Gets pointer.
    /// </summary>
#pragma warning disable CA1720
    public DataType Pointer
#pragma warning restore CA1720
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            PointCounter++;
            return this;
        }
    }

    /// <summary>
    /// Generic the items.
    /// </summary>
    /// <param name="types">types.</param>
    /// <returns>expression.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public DataType Generic(params DataType[] types)
    {
        Type = Type.Generic(types);
        return this;
    }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddStorageKind(ref builder);
        Type.ToCode(ref builder);
        if (IsArray)
        {
            builder.Append("[]");
        }

        if (PointCounter <= 0)
        {
            return;
        }

        builder.Append('*', PointCounter);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataType"/> class.
    /// Create from name.
    /// </summary>
    /// <param name="name">name.</param>
    public DataType(string name)
        : this(new SimpleNameExpression(name))
    {
    }

    /// <summary>
    /// Create from a symbol.
    /// </summary>
    /// <param name="symbol">the symbol.</param>
    /// <param name="compilation">compilation.</param>
    /// <returns>result.</returns>
    /// <exception cref="ArgumentNullException">throw if symbol is null.</exception>
    public static DataType FromSymbol(ITypeSymbol symbol, Compilation? compilation = null)
    {
        if (symbol is null)
        {
            throw new ArgumentNullException(nameof(symbol));
        }

        if (symbol is IArrayTypeSymbol arrayType)
        {
            return FromSymbol(arrayType.ElementType, compilation).Array;
        }

        if (symbol is IPointerTypeSymbol pointerType)
        {
            return FromSymbol(pointerType.PointedAtType, compilation).Pointer;
        }

        if (symbol is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T, } nullableType)
        {
            return FromSymbol(nullableType.TypeArguments[0], compilation).Null;
        }

        if (symbol is INamedTypeSymbol { IsTupleType: true, } tupleType)
        {
            var tupleExpression = new TupleExpression();
            foreach (var tupleTypeTupleElement in tupleType.TupleElements)
            {
                tupleExpression.AddItem(FromSymbol(tupleTypeTupleElement.Type, compilation), tupleTypeTupleElement.Name);
            }

            return new(tupleExpression);
        }

        if (symbol.TypeKind is TypeKind.TypeParameter or TypeKind.Error)
        {
            return new(symbol.Name.ToSimpleName());
        }

        if (_specialTypeAlias.TryGetValue(symbol.SpecialType, out var specialTypeFactory))
        {
            var dataType = specialTypeFactory();
            if (symbol.NullableAnnotation is NullableAnnotation.Annotated)
            {
                dataType = dataType.Null;
            }

            return dataType;
        }

        var name = ZString.Concat(symbol.GetAlias(compilation), "::", symbol.FullName);
        if (symbol is not INamedTypeSymbol { IsGenericType: true, } namedType)
        {
            return new(name.ToSimpleName());
        }

        var index = name.IndexOf('<');
        if (index < 0)
        {
            return new(name.ToSimpleName());
        }

        return new(name.Substring(0, index).ToSimpleName()
            .Generic(namedType.TypeArguments.Select(i => FromSymbol(i, compilation)).ToArray()));
    }

    /// <summary>
    /// Implicit convert.
    /// </summary>
    /// <param name="value">value.</param>
    /// <returns>result.</returns>
    public static implicit operator DataType(Type value)
    {
        return FromType(value);
    }

    /// <summary>
    /// From Type.
    /// </summary>
    /// <typeparam name="T">Type.</typeparam>
    /// <param name="alias">alias.</param>
    /// <returns>Expression.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DataType FromType<T>(string alias = "global")
    {
        return FromType(typeof(T), alias);
    }

    /// <summary>
    /// From the type.
    /// </summary>
    /// <param name="type">type.</param>
    /// <param name="alias">alias.</param>
    /// <returns>result.</returns>
    /// <exception cref="ArgumentNullException">type is null.</exception>
    public static DataType FromType(Type type, string alias = "global")
    {
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        type = type.IsByRef ? type.GetElementType() ?? type : type;

        if (_typeAlias.TryGetValue(type, out var s))
        {
            return s();
        }

        if (type.IsPointer)
        {
            return FromType(type.GetElementType()!).Pointer;
        }

        if (type.IsArray)
        {
            return FromType(type.GetElementType()!).Array;
        }

        if (type.IsGenericType)
        {
            if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return FromType(Nullable.GetUnderlyingType(type)!).Null;
            }

            return new(SimpleType()
                .Generic([
                    .. type.GetGenericArguments()
                        .Where(i => !i.IsGenericParameter)
                        .Select(t => FromType(t, alias)),
                ]));
        }

        return new(SimpleType());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IExpression SimpleType()
        {
            var name = new SimpleNameExpression(type.Name.Split('`')[0].Replace("&", ""));

            if (type.DeclaringType is not null)
            {
                return new MemberAccessExpression(FromType(type.DeclaringType).Type, name);
            }

            if (string.IsNullOrEmpty(type.Namespace))
            {
                return AddAlias(name);
            }

            return AddAlias(new MemberAccessExpression(new SimpleNameExpression(type.Namespace), name));
        }

        IExpression AddAlias(IExpression expression)
        {
            if (string.IsNullOrEmpty(alias))
            {
                return expression;
            }

            return new AliasExpression(alias, expression);
        }
    }

    /// <summary>
    /// Gets <see langword="var"/>.
    /// </summary>
    public static DataType Var
    {
        get
        {
            return new(new SimpleNameExpression("var"));
        }
    }
#pragma warning disable CA1720
    /// <summary>
    /// Gets <see langword="string"/>.
    /// </summary>
    public static DataType String
    {
        get
        {
            return new(new SimpleNameExpression("string"));
        }
    }

    /// <summary>
    /// Gets <see langword="char"/>.
    /// </summary>
    public static DataType Char
    {
        get
        {
            return new(new SimpleNameExpression("char"));
        }
    }

    /// <summary>
    /// Gets <see langword="byte"/>.
    /// </summary>
    public static DataType Byte
    {
        get
        {
            return new(new SimpleNameExpression("byte"));
        }
    }

    /// <summary>
    /// Gets <see langword="sbyte"/>.
    /// </summary>
    public static DataType Sbyte
    {
        get
        {
            return new(new SimpleNameExpression("sbyte"));
        }
    }

    /// <summary>
    /// Gets <see langword="short"/>.
    /// </summary>
    public static DataType Short
    {
        get
        {
            return new(new SimpleNameExpression("short"));
        }
    }

    /// <summary>
    /// Gets <see langword="ushort"/>.
    /// </summary>
    public static DataType Ushort
    {
        get
        {
            return new(new SimpleNameExpression("ushort"));
        }
    }

    /// <summary>
    /// Gets <see langword="int"/>.
    /// </summary>
    public static DataType Int
    {
        get
        {
            return new(new SimpleNameExpression("int"));
        }
    }

    /// <summary>
    /// Gets <see langword="uint"/>.
    /// </summary>
    public static DataType Uint
    {
        get
        {
            return new(new SimpleNameExpression("uint"));
        }
    }

    /// <summary>
    /// Gets <see langword="long"/>.
    /// </summary>
    public static DataType Long
    {
        get
        {
            return new(new SimpleNameExpression("long"));
        }
    }

    /// <summary>
    /// Gets <see langword="ulong"/>.
    /// </summary>
    public static DataType Ulong
    {
        get
        {
            return new(new SimpleNameExpression("ulong"));
        }
    }

    /// <summary>
    /// Gets <see langword="bool"/>.
    /// </summary>
    public static DataType Bool
    {
        get
        {
            return new(new SimpleNameExpression("bool"));
        }
    }

    /// <summary>
    /// Gets <see langword="double"/>.
    /// </summary>
    public static DataType Double
    {
        get
        {
            return new(new SimpleNameExpression("double"));
        }
    }

    /// <summary>
    /// Gets <see langword="float"/>.
    /// </summary>
    public static DataType Float
    {
        get
        {
            return new(new SimpleNameExpression("float"));
        }
    }

    /// <summary>
    /// Gets <see langword="decimal"/>.
    /// </summary>
    public static DataType Decimal
    {
        get
        {
            return new(new SimpleNameExpression("decimal"));
        }
    }

    /// <summary>
    /// Gets <see langword="object"/>.
    /// </summary>
    public static DataType Object
    {
        get
        {
            return new(new SimpleNameExpression("object"));
        }
    }

    /// <summary>
    /// Gets <see langword="void"/>.
    /// </summary>
    public static DataType Void
    {
        get
        {
            return new(new SimpleNameExpression("void"));
        }
    }
#pragma warning restore CA1720

    private static readonly Dictionary<Type, Func<DataType>> _typeAlias = new()
    {
        { typeof(bool), () => Bool },
        { typeof(byte), () => Byte },
        { typeof(char), () => Char },
        { typeof(decimal), () => Decimal },
        { typeof(double), () => Double },
        { typeof(float), () => Float },
        { typeof(int), () => Int },
        { typeof(long), () => Long },
        { typeof(object), () => Object },
        { typeof(sbyte), () => Sbyte },
        { typeof(short), () => Short },
        { typeof(string), () => String },
        { typeof(uint), () => Uint },
        { typeof(ulong), () => Ulong },
        { typeof(ushort), () => Ushort },
        { typeof(void), () => Void },
    };

    private static readonly Dictionary<SpecialType, Func<DataType>> _specialTypeAlias = new()
    {
        { SpecialType.System_Boolean, () => Bool },
        { SpecialType.System_Byte, () => Byte },
        { SpecialType.System_Char, () => Char },
        { SpecialType.System_Decimal, () => Decimal },
        { SpecialType.System_Double, () => Double },
        { SpecialType.System_Single, () => Float },
        { SpecialType.System_Int16, () => Short },
        { SpecialType.System_Int32, () => Int },
        { SpecialType.System_Int64, () => Long },
        { SpecialType.System_Object, () => Object },
        { SpecialType.System_SByte, () => Sbyte },
        { SpecialType.System_String, () => String },
        { SpecialType.System_UInt16, () => Ushort },
        { SpecialType.System_UInt32, () => Uint },
        { SpecialType.System_UInt64, () => Ulong },
        { SpecialType.System_Void, () => Void },
    };

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
    {
        Type.ToCref(ref builder);
    }
}