// -----------------------------------------------------------------------
// <copyright file="FunctionPointerType.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection.Metadata;
using System.Security;

using Microsoft.CodeAnalysis;

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// A C# <c>delegate*</c> type with an ordered parameter list and a return type.
/// </summary>
public sealed class FunctionPointerType : IExpression
{
    private readonly DataType _returnType;

    private readonly string _callingConvention;

    private readonly List<DataType> _parameterTypes = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionPointerType"/> class with no parameters.
    /// </summary>
    /// <param name="returnType">The return type. Use <see cref="DataType.Void"/> for no return value.</param>
    /// <param name="callingConvention">The calling convention to emit.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="callingConvention"/> is undefined.</exception>
    public FunctionPointerType(
        DataType returnType,
        FunctionPointerCallingConvention callingConvention = FunctionPointerCallingConvention.MANAGED)
        : this(returnType, callingConvention switch
        {
            FunctionPointerCallingConvention.MANAGED => "",
            FunctionPointerCallingConvention.UNMANAGED => " unmanaged",
            FunctionPointerCallingConvention.CDECL => " unmanaged[Cdecl]",
            FunctionPointerCallingConvention.STDCALL => " unmanaged[Stdcall]",
            FunctionPointerCallingConvention.THISCALL => " unmanaged[Thiscall]",
            FunctionPointerCallingConvention.FASTCALL => " unmanaged[Fastcall]",
            _ => throw new ArgumentOutOfRangeException(nameof(callingConvention), callingConvention, null),
        })
    {
    }

    private FunctionPointerType(DataType returnType, string callingConvention)
    {
        _returnType = returnType;
        _callingConvention = callingConvention;
    }

    /// <summary>
    /// Appends an unnamed parameter to the signature.
    /// </summary>
    /// <param name="type">The parameter type, including any by-reference modifier.</param>
    /// <returns>The current function pointer type.</returns>
    public FunctionPointerType AddParameter(DataType type)
    {
        _parameterTypes.Add(type);
        return this;
    }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append("delegate*");
        builder.Append(_callingConvention);
        builder.Append('<');
        foreach (var type in _parameterTypes.AsSpan())
        {
            type.ToCode(ref builder);
            builder.Append(", ");
        }

        _returnType.ToCode(ref builder);
        builder.Append('>');
    }

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
    {
        builder.Append(SecurityElement.Escape(this.ToCode())!);
    }

    /// <summary>
    /// Converts a Roslyn function pointer signature to a syntax object.
    /// </summary>
    /// <param name="symbol">The function pointer symbol.</param>
    /// <param name="compilation">The compilation used to resolve type aliases.</param>
    /// <returns>The function pointer type.</returns>
    /// <exception cref="NotSupportedException">The calling convention has no C# syntax.</exception>
    internal static FunctionPointerType FromSymbol(IFunctionPointerTypeSymbol symbol, Compilation? compilation)
    {
        var signature = symbol.Signature;
        var returnType = DataType.FromSymbol(signature.ReturnType, compilation);
        returnType.StorageKind = signature.RefKind switch
        {
            RefKind.Ref => StorageKind.REF,
            RefKind.RefReadOnly => StorageKind.REF_READONLY,
            _ => StorageKind.NONE,
        };

        var pointer = signature.CallingConvention switch
        {
            SignatureCallingConvention.Default => new FunctionPointerType(returnType),
            SignatureCallingConvention.CDecl => new FunctionPointerType(returnType, FunctionPointerCallingConvention.CDECL),
            SignatureCallingConvention.StdCall => new FunctionPointerType(returnType, FunctionPointerCallingConvention.STDCALL),
            SignatureCallingConvention.ThisCall => new FunctionPointerType(returnType, FunctionPointerCallingConvention.THISCALL),
            SignatureCallingConvention.FastCall => new FunctionPointerType(returnType, FunctionPointerCallingConvention.FASTCALL),
            SignatureCallingConvention.Unmanaged => new FunctionPointerType(returnType, GetUnmanagedConvention(signature)),
            _ => throw new NotSupportedException(
                $"Function pointer calling convention '{signature.CallingConvention.ToString()}' has no C# syntax."),
        };

        foreach (var parameter in signature.Parameters)
        {
            var type = DataType.FromSymbol(parameter.Type, compilation);
            type.StorageKind = parameter.RefKind switch
            {
                RefKind.Ref => StorageKind.REF,
                RefKind.In => StorageKind.IN,
                RefKind.Out => StorageKind.OUT,
                RefKind.RefReadOnlyParameter => StorageKind.REF_READONLY,
                _ => StorageKind.NONE,
            };
            pointer.AddParameter(type);
        }

        return pointer;
    }

    private static string GetUnmanagedConvention(IMethodSymbol signature)
    {
        if (signature.UnmanagedCallingConventionTypes.IsDefaultOrEmpty)
        {
            return " unmanaged";
        }

        const string prefix = "CallConv";
        var names = signature.UnmanagedCallingConventionTypes.Select(type => type.Name.Substring(prefix.Length).ToValidIdentifier());
        return $" unmanaged[{string.Join(", ", names)}]";
    }
}