// -----------------------------------------------------------------------
// <copyright file="GeneralExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;

using Cysharp.Text;

using Microsoft.CodeAnalysis;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// Some general things.
/// </summary>
#pragma warning disable CA1708
public static class GeneralExtensions
#pragma warning restore CA1708
{
    private static class ArrayAccessor<T>
    {
        public static readonly FieldInfo ItemsField = typeof(List<T>)
#pragma warning disable S3011
            .GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic)!;
#pragma warning restore S3011
    }

    /// <summary>
    /// As span.
    /// </summary>
    /// <param name="list">the list.</param>
    /// <typeparam name="T">Data.</typeparam>
    /// <returns>span.</returns>
    internal static Span<T> AsSpan<T>(this List<T> list)
    {
        var items = (T[])ArrayAccessor<T>.ItemsField.GetValue(list);
        return new(items, 0, list.Count);
    }

#pragma warning disable S2325, CA1034
    extension(string value)
    {
        /// <summary>
        /// To the argument name.
        /// </summary>
        /// <returns>argument Name.</returns>
        public string ToValidIdentifier()
            => _keywords.Contains(value) ? ZString.Concat('@', value) : value;

        /// <summary>
        /// To the valid literal.
        /// </summary>
        /// <returns>result.</returns>
        public string ToValidLiteral()
        {
            foreach (var keyValuePair in _specialChars)
            {
                value = value.Replace(keyValuePair.Key.ToString(), keyValuePair.Value);
            }

            return value;
        }
    }

    extension(char value)
    {
        /// <summary>
        /// To the valid literal.
        /// </summary>
        /// <returns>result.</returns>
        public string ToValidLiteral()
        {
            if (_specialChars.TryGetValue(value, out var result))
            {
                return result;
            }

            return value.ToString();
        }
    }

    private static readonly Dictionary<char, string> _specialChars = new()
    {
        // { '\'', @"\'" },
        // { '\\', @"\\" },
        // { '\0', @"\0" },
        // { '\a', @"\a" },
        // { '\b', @"\b" },
        // { '\f', @"\f" },
        // { '\r', @"\r" },
        // { '\t', @"\t" },
        // { '\v', @"\v" },
        { '\"', "\\\"" },
        { '\n', @"\n" },
    };

    private static readonly HashSet<string> _keywords = new(StringComparer.Ordinal)
    {
        "abstract",
        "as",
        "base",
        "bool",
        "break",
        "byte",
        "case",
        "catch",
        "char",
        "checked",
        "class",
        "const",
        "continue",
        "decimal",
        "default",
        "delegate",
        "do",
        "double",
        "else",
        "enum",
        "event",
        "explicit",
        "extern",
        "false",
        "finally",
        "fixed",
        "float",
        "for",
        "foreach",
        "goto",
        "if",
        "implicit",
        "in",
        "int",
        "interface",
        "internal",
        "is",
        "lock",
        "long",
        "namespace",
        "new",
        "null",
        "object",
        "operator",
        "out",
        "override",
        "params",
        "private",
        "protected",
        "public",
        "readonly",
        "ref",
        "return",
        "sbyte",
        "sealed",
        "short",
        "sizeof",
        "stackalloc",
        "static",
        "string",
        "struct",
        "switch",
        "this",
        "throw",
        "true",
        "try",
        "typeof",
        "uint",
        "ulong",
        "unchecked",
        "unsafe",
        "ushort",
        "using",
        "virtual",
        "void",
        "volatile",
        "while",
        "add",
        "and",
        "alias",
        "ascending",
        "args",
        "async",
        "await",
        "by",
        "descending",
        "dynamic",
        "equals",
        "from",
        "get",
        "global",
        "group",
        "init",
        "into",
        "join",
        "let",
        "managed",
        "not",
        "notnull",
        "on",
        "or",
        "orderby",
        "partial",
        "record",
        "remove",
        "required",
        "select",
        "set",
        "unmanaged",
        "value",
        "var",
        "when",
        "where",
        "with",
        "yield",
    };

    extension(Type value)
    {
        /// <summary>
        /// Get the tool name.
        /// </summary>
        /// <returns>tool name.</returns>
        public string GetToolName()
        {
            var builder = new SourceBuilder();

            try
            {
                value.ToExpression().ToCode(ref builder);
                return builder.ToCode();
            }
            finally
            {
                builder.Dispose();
            }
        }

        /// <summary>
        ///  Get the version.
        /// </summary>
        /// <returns>version.</returns>
        public string GetVersion()
            => value.Assembly.GetName().Version.ToString();
    }
#pragma warning restore S2325, CA1034

    /// <summary>
    /// Get the alias of the type.
    /// </summary>
    /// <param name="typeSymbol">the type symbol.</param>
    /// <param name="compilation">compilation.</param>
    /// <returns>alias.</returns>
    public static string GetAlias(this ITypeSymbol typeSymbol, Compilation? compilation = null)
    {
        if (compilation is null || typeSymbol?.ContainingAssembly is not { } assembly)
        {
            return "global";
        }

        foreach (var reference in compilation.References)
        {
            var symbol = compilation.GetAssemblyOrModuleSymbol(reference);

            if (SymbolEqualityComparer.Default.Equals(symbol, assembly)
                && reference.Properties.Aliases.Length > 0)
            {
                return reference.Properties.Aliases[0];
            }
        }

        return "global";
    }
}