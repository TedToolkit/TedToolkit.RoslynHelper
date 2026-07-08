// -----------------------------------------------------------------------
// <copyright file="GeneralExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;

using Cysharp.Text;

using Microsoft.CodeAnalysis;

namespace TedToolkit.RoslynHelper;

/// <summary>
/// Some general things.
/// </summary>
#pragma warning disable CA1708
public static class GeneralExtensions
#pragma warning restore CA1708
{
    private ref struct Connector : IDisposable
    {
        private readonly Func<char, bool> _isConnector;

        private readonly char _defaultConnector;

        private readonly HintNameConnectorType _connectorType;

        private char? _char = null;

        private Utf16ValueStringBuilder _builder = default;

        public Connector(Func<char, bool> isConnector,
            char defaultConnector,
            HintNameConnectorType connectorType)
        {
            _isConnector = isConnector;
            _defaultConnector = defaultConnector;
            _connectorType = connectorType;
            if (connectorType is not HintNameConnectorType.KEEP_ALL)
            {
                return;
            }

            _builder = ZString.CreateStringBuilder();
        }

        public void Append(char c)
        {
            switch (_connectorType)
            {
                case HintNameConnectorType.KEEP_FIRST when _isConnector(c):
                    _char ??= c;
                    return;

                case HintNameConnectorType.KEEP_LAST when _isConnector(c):
                    _char = c;
                    return;

                case HintNameConnectorType.KEEP_ALL:
                    _builder.Append(_isConnector(c) ? c : _defaultConnector);
                    return;
            }
        }

        public void Write(ref Utf16ValueStringBuilder builder)
        {
            if (_connectorType is HintNameConnectorType.KEEP_ALL)
            {
                builder.Append(_builder.AsSpan());
            }
            else
            {
                builder.Append(_char ?? _defaultConnector);
            }
        }

        public void Clear()
        {
            if (_connectorType is HintNameConnectorType.KEEP_ALL)
            {
                _builder.Clear();
            }
            else
            {
                _char = null;
            }
        }

        public void Dispose()
        {
            if (_connectorType is not HintNameConnectorType.KEEP_ALL)
            {
                return;
            }

            _builder.Dispose();
        }
    }

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
        {
            return _keywords.Contains(value) ? ZString.Concat('@', value) : value;
        }

        /// <summary>
        /// Converts arbitrary text into a Roslyn hint-name-safe token while preserving dot separators.
        /// </summary>
        /// <param name="isValue">Determines which characters should be emitted as-is.</param>
        /// <param name="defaultConnector">The fallback connector used when a separator run does not include a dot.</param>
        /// <param name="connectorType">Controls how separator runs are collapsed before writing a connector.</param>
        /// <returns>Normalized hint name with dot separators preserved.</returns>
        public string ToHintNameKeepDot(
            Func<char, bool>? isValue = null,
            char defaultConnector = '_',
            HintNameConnectorType connectorType = HintNameConnectorType.KEEP_FIRST)
        {
            return value.ToHintName(
                isValue: isValue,
                isConnector: static c => c is '.',
                defaultConnector: defaultConnector,
                connectorType: connectorType);
        }

        /// <summary>
        /// Converts arbitrary text into a Roslyn hint-name-safe token.
        /// </summary>
        /// <param name="isValue">Determines which characters should be emitted as-is.</param>
        /// <param name="isConnector">Determines which separator characters should be preserved when collapsing separator runs.</param>
        /// <param name="defaultConnector">The fallback connector used when no preserved connector is selected.</param>
        /// <param name="connectorType">Controls how separator runs are collapsed before writing a connector.</param>
        /// <returns>Normalized hint name.</returns>
        public string ToHintName(
            Func<char, bool>? isValue = null,
            Func<char, bool>? isConnector = null,
            char defaultConnector = '_',
            HintNameConnectorType connectorType = HintNameConnectorType.KEEP_FIRST)
        {
            isValue ??= char.IsLetterOrDigit;
            isConnector ??= _ => false;

            var builder = ZString.CreateStringBuilder();
            try
            {
                var previousWasSeparator = false;
                using var connector = new Connector(isConnector, defaultConnector, connectorType);

                foreach (var c in value)
                {
                    if (isValue(c))
                    {
                        if (previousWasSeparator && builder.Length > 0)
                        {
                            connector.Write(ref builder);
                        }

                        connector.Clear();
                        builder.Append(c);
                        previousWasSeparator = false;
                    }
                    else
                    {
                        connector.Append(c);
                        previousWasSeparator = true;
                    }
                }

                return builder.ToString();
            }
            finally
            {
                builder.Dispose();
            }
        }

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
        { '\"', "\\\"" }, { '\n', @"\n" },
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
        {
            return value.Assembly.GetName().Version.ToString();
        }
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

    /// <summary>
    /// Generate to the codes.
    /// </summary>
    /// <param name="item">the code item.</param>
    /// <returns>code</returns>
    /// <exception cref="ArgumentNullException">The item is null.</exception>
    public static string ToCode(this IToCode item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        var builder = new SourceBuilder();
        item.ToCode(ref builder);
        return builder.ToCode();
    }
}