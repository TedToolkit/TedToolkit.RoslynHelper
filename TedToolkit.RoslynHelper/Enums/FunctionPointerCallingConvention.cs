// -----------------------------------------------------------------------
// <copyright file="FunctionPointerCallingConvention.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper;

/// <summary>
/// The calling convention for a C# function pointer type.
/// </summary>
public enum FunctionPointerCallingConvention
{
    /// <summary>
    /// The default managed calling convention.
    /// </summary>
    MANAGED = 0,

    /// <summary>
    /// The platform's default unmanaged calling convention.
    /// </summary>
    UNMANAGED = 1,

    /// <summary>
    /// The unmanaged <c>Cdecl</c> calling convention.
    /// </summary>
    CDECL = 2,

    /// <summary>
    /// The unmanaged <c>Stdcall</c> calling convention.
    /// </summary>
    STDCALL = 3,

    /// <summary>
    /// The unmanaged <c>Thiscall</c> calling convention.
    /// </summary>
    THISCALL = 4,

    /// <summary>
    /// The unmanaged <c>Fastcall</c> calling convention.
    /// </summary>
    FASTCALL = 5,
}