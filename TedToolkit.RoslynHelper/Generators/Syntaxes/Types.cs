using System.Runtime.CompilerServices;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// For the Types.
/// </summary>
public static class Types
{
    /// <summary>
    /// <see langword="var"/>
    /// </summary>
    public static SimpleNameExpression Var { get; } = new("var");
#pragma warning disable CA1720
    /// <summary>
    /// <see langword="string"/>
    /// </summary>
    public static SimpleNameExpression String { get; } = new("string");

    /// <summary>
    /// <see langword="char"/>
    /// </summary>
    public static SimpleNameExpression Char { get; } = new("char");

    /// <summary>
    /// <see langword="byte"/>
    /// </summary>
    public static SimpleNameExpression Byte { get; } = new("byte");

    /// <summary>
    /// <see langword="sbyte"/>
    /// </summary>
    public static SimpleNameExpression Sbyte { get; } = new("sbyte");

    /// <summary>
    /// <see langword="short"/>
    /// </summary>
    public static SimpleNameExpression Short { get; } = new("short");

    /// <summary>
    /// <see langword="ushort"/>
    /// </summary>
    public static SimpleNameExpression Ushort { get; } = new("ushort");

    /// <summary>
    /// <see langword="int"/>
    /// </summary>
    public static SimpleNameExpression Int { get; } = new("int");

    /// <summary>
    /// <see langword="uint"/>
    /// </summary>
    public static SimpleNameExpression Uint { get; } = new("uint");

    /// <summary>
    /// <see langword="long"/>
    /// </summary>
    public static SimpleNameExpression Long { get; } = new("long");

    /// <summary>
    /// <see langword="ulong"/>
    /// </summary>
    public static SimpleNameExpression Ulong { get; } = new("ulong");

    /// <summary>
    /// <see langword="bool"/>
    /// </summary>
    public static SimpleNameExpression Bool { get; } = new("bool");
#pragma warning restore CA1720

    /// <summary>
    /// From Type
    /// </summary>
    /// <typeparam name="T">Type</typeparam>
    /// <returns>Expression</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IExpression FromType<T>()
        => FromType(typeof(T));

    /// <summary>
    /// From Type
    /// </summary>
    /// <param name="type">Type</param>
    /// <returns>Expression</returns>
    /// <exception cref="ArgumentNullException">type is null</exception>
    public static IExpression FromType(Type type)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type));

        if (_typeAlias.TryGetValue(type, out var s))
            return s;

        if (type.IsArray)
            return FromType(type.GetElementType()!).Array;

        if (type.IsGenericType)
        {
            if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                return FromType(Nullable.GetUnderlyingType(type)!).Null;

            return SimpleType()
                .Generic([.. type.GetGenericArguments().Select(FromType),]);
        }

        return SimpleType();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IExpression SimpleType()
        {
            var name = new SimpleNameExpression(type.Name.Split('`')[0]);
            if (string.IsNullOrEmpty(type.Namespace))
                return name;

            return new MemberAccessExpression(new SimpleNameExpression(type.Namespace), name);
        }
    }

    private static readonly Dictionary<Type, SimpleNameExpression> _typeAlias = new()
    {
        { typeof(bool), new("bool") },
        { typeof(byte), new("byte") },
        { typeof(char), new("char") },
        { typeof(decimal), new("decimal") },
        { typeof(double), new("double") },
        { typeof(float), new("float") },
        { typeof(int), new("int") },
        { typeof(long), new("long") },
        { typeof(object), new("object") },
        { typeof(sbyte), new("sbyte") },
        { typeof(short), new("short") },
        { typeof(string), new("string") },
        { typeof(uint), new("uint") },
        { typeof(ulong), new("ulong") },
        { typeof(ushort), new("ushort") },
        { typeof(void), new("void") },
    };
}