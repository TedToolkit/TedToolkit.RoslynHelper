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
    private readonly TypeParameterExpression _signature;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionPointerType"/> class.
    /// </summary>
    /// <param name="returnType">The return type. Use <see cref="DataType.Void"/> for no return value.</param>
    /// <param name="callingConvention">The calling convention to emit.</param>
    /// <param name="parameterTypes">The unnamed parameter types in signature order.</param>
    /// <exception cref="ArgumentNullException"><paramref name="parameterTypes"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="callingConvention"/> is undefined.</exception>
    public FunctionPointerType(
        DataType returnType,
        FunctionPointerCallingConvention callingConvention = FunctionPointerCallingConvention.MANAGED,
        params DataType[] parameterTypes)
        : this(returnType, callingConvention switch
        {
            FunctionPointerCallingConvention.MANAGED => "",
            FunctionPointerCallingConvention.UNMANAGED => " unmanaged",
            FunctionPointerCallingConvention.CDECL => " unmanaged[Cdecl]",
            FunctionPointerCallingConvention.STDCALL => " unmanaged[Stdcall]",
            FunctionPointerCallingConvention.THISCALL => " unmanaged[Thiscall]",
            FunctionPointerCallingConvention.FASTCALL => " unmanaged[Fastcall]",
            _ => throw new ArgumentOutOfRangeException(nameof(callingConvention), callingConvention, null),
        }, parameterTypes ?? throw new ArgumentNullException(nameof(parameterTypes)))
    {
    }

    private FunctionPointerType(DataType returnType, string callingConvention, DataType[] parameterTypes)
    {
        _signature = new SimpleNameExpression($"delegate*{callingConvention}")
            .Generic([
                .. parameterTypes,
                returnType,
            ]);
    }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        _signature.ToCode(ref builder);
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

        var parameterTypes = new DataType[signature.Parameters.Length];
        for (var index = 0; index < parameterTypes.Length; index++)
        {
            var parameter = signature.Parameters[index];
            var type = DataType.FromSymbol(parameter.Type, compilation);
            type.StorageKind = parameter.RefKind switch
            {
                RefKind.Ref => StorageKind.REF,
                RefKind.In => StorageKind.IN,
                RefKind.Out => StorageKind.OUT,
                RefKind.RefReadOnlyParameter => StorageKind.REF_READONLY,
                _ => StorageKind.NONE,
            };
            parameterTypes[index] = type;
        }

        return signature.CallingConvention switch
        {
            SignatureCallingConvention.Default => new(returnType, parameterTypes: parameterTypes),
            SignatureCallingConvention.CDecl => new(returnType, FunctionPointerCallingConvention.CDECL, parameterTypes),
            SignatureCallingConvention.StdCall => new(returnType, FunctionPointerCallingConvention.STDCALL, parameterTypes),
            SignatureCallingConvention.ThisCall => new(returnType, FunctionPointerCallingConvention.THISCALL, parameterTypes),
            SignatureCallingConvention.FastCall => new(returnType, FunctionPointerCallingConvention.FASTCALL, parameterTypes),
            SignatureCallingConvention.Unmanaged => new(returnType, GetUnmanagedConvention(signature), parameterTypes),
            _ => throw new NotSupportedException(
                $"Function pointer calling convention '{signature.CallingConvention.ToString()}' has no C# syntax."),
        };
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