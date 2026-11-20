// -----------------------------------------------------------------------
// <copyright file="FunctionPointerType.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;
using System.Reflection.Metadata;
using System.Security;

using Microsoft.CodeAnalysis;

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// A C# <c>delegate*</c> type with an ordered parameter list and a return type.
/// </summary>
public sealed class FunctionPointerType : IExpression
{
    private readonly DataType[] _signatureTypes;

    private readonly TypeParameterExpression _signature;

    /// <summary>
    /// Initializes a new instance of the <see cref="FunctionPointerType"/> class.
    /// </summary>
    /// <param name="returnType">The return type. Use <see cref="DataType.Void"/> for no return value.</param>
    /// <param name="callingConvention">The calling convention to emit.</param>
    /// <param name="parameterTypes">The unnamed parameter types in signature order.</param>
    /// <exception cref="ArgumentNullException"><paramref name="returnType"/> or <paramref name="parameterTypes"/> is null.</exception>
    /// <exception cref="ArgumentException">A parameter type or the return type is invalid for a function pointer signature.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="callingConvention"/> has no C# function pointer syntax.</exception>
    public FunctionPointerType(
        DataType returnType,
        SignatureCallingConvention callingConvention = SignatureCallingConvention.Default,
        params DataType[] parameterTypes)
        : this(
            GetCallingConventionSpecifier(callingConvention)
                ?? throw new ArgumentOutOfRangeException(nameof(callingConvention), callingConvention, null),
            [.. parameterTypes ?? throw new ArgumentNullException(nameof(parameterTypes)), returnType,])
    {
    }

    private FunctionPointerType(string conventionSpecifier, DataType[] signatureTypes)
    {
        _signatureTypes = signatureTypes;
        ValidateSignature();
        _signature = new SimpleNameExpression($"delegate*{conventionSpecifier}").Generic(signatureTypes);
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException">A parameter type or the return type is invalid for a function pointer signature.</exception>
    public void ToCode(ref SourceBuilder builder)
    {
        ValidateSignature();
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
    /// <exception cref="ArgumentException">The signature contains an invalid parameter or return type.</exception>
    /// <exception cref="NotSupportedException">The calling convention or reference modifier has no C# syntax.</exception>
    internal static FunctionPointerType FromSymbol(IFunctionPointerTypeSymbol symbol, Compilation? compilation)
    {
        var signature = symbol.Signature;
        var convention = signature.CallingConvention;
        var conventionSpecifier = GetCallingConventionSpecifier(convention)
            ?? throw new NotSupportedException($"Function pointer calling convention '{convention.ToString()}' has no C# syntax.");
        if (convention is SignatureCallingConvention.Unmanaged && !signature.UnmanagedCallingConventionTypes.IsDefaultOrEmpty)
        {
            const string prefix = "CallConv";
            var names = signature.UnmanagedCallingConventionTypes.Select(type => type.Name.Substring(prefix.Length).ToValidIdentifier());
            conventionSpecifier += $"[{string.Join(", ", names)}]";
        }

        var parameterCount = signature.Parameters.Length;
        var signatureTypes = new DataType[parameterCount + 1];
        for (var index = 0; index < parameterCount; index++)
        {
            var parameter = signature.Parameters[index];
            var type = DataType.FromSymbol(parameter.Type, compilation);
            type.StorageKind = parameter.RefKind switch
            {
                RefKind.None => StorageKind.NONE,
                RefKind.Ref => StorageKind.REF,
                RefKind.In => StorageKind.IN,
                RefKind.Out => StorageKind.OUT,
                RefKind.RefReadOnlyParameter => StorageKind.REF_READONLY,
                _ => throw new NotSupportedException(
                    $"Unexpected reference modifier on function pointer parameter {(index + 1).ToString(CultureInfo.InvariantCulture)}."),
            };
            signatureTypes[index] = type;
        }

        var returnType = DataType.FromSymbol(signature.ReturnType, compilation);
        returnType.StorageKind = signature.RefKind switch
        {
            RefKind.None => StorageKind.NONE,
            RefKind.Ref => StorageKind.REF,
            RefKind.RefReadOnly => StorageKind.REF_READONLY,
            _ => throw new NotSupportedException("Unexpected reference modifier on a function pointer return type."),
        };
        signatureTypes[parameterCount] = returnType;
        return new(conventionSpecifier, signatureTypes);
    }

    private static string? GetCallingConventionSpecifier(SignatureCallingConvention convention)
    {
        return convention switch
        {
            SignatureCallingConvention.Default => "",
            SignatureCallingConvention.Unmanaged => " unmanaged",
            SignatureCallingConvention.CDecl => " unmanaged[Cdecl]",
            SignatureCallingConvention.StdCall => " unmanaged[Stdcall]",
            SignatureCallingConvention.ThisCall => " unmanaged[Thiscall]",
            SignatureCallingConvention.FastCall => " unmanaged[Fastcall]",
            _ => null,
        };
    }

    private void ValidateSignature()
    {
        var parameterCount = _signatureTypes.Length - 1;
        ValidateReturnType(_signatureTypes[parameterCount]);
        ValidateParameterTypes(_signatureTypes.AsSpan(0, parameterCount));
    }

    private static void ValidateReturnType(DataType returnType)
    {
        if (returnType is null)
        {
            throw new ArgumentNullException(nameof(returnType));
        }

        if (returnType.StorageKind is not (StorageKind.NONE or StorageKind.REF or StorageKind.REF_READONLY))
        {
            throw new ArgumentException("A function pointer return type allows only ref or ref readonly.", nameof(returnType));
        }

        if (!IsNonPointerVoid(returnType) || (!returnType.IsArray && returnType.StorageKind is StorageKind.NONE))
        {
            return;
        }

        throw new ArgumentException("A void return type cannot be an array or returned by reference.", nameof(returnType));
    }

    private static void ValidateParameterTypes(scoped in ReadOnlySpan<DataType> parameterTypes)
    {
        for (var index = 0; index < parameterTypes.Length; index++)
        {
            var type = parameterTypes[index];
            if (type is null || IsNonPointerVoid(type))
            {
                var position = (index + 1).ToString(CultureInfo.InvariantCulture);
                throw new ArgumentException($"Function pointer parameter {position} must have a non-void type.", nameof(parameterTypes));
            }

            if (type.StorageKind is not
                (StorageKind.NONE or StorageKind.REF or StorageKind.REF_READONLY or StorageKind.IN or StorageKind.OUT))
            {
                var position = (index + 1).ToString(CultureInfo.InvariantCulture);
                throw new ArgumentException(
                    $"Function pointer parameter {position} allows only ref, ref readonly, in, or out.", nameof(parameterTypes));
            }
        }
    }

    private static bool IsNonPointerVoid(DataType type)
    {
        return type.PointCounter <= 0 && type.Type is SimpleNameExpression name && name.Name == "void";
    }
}