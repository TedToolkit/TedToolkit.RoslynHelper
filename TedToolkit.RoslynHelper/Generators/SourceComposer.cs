// -----------------------------------------------------------------------
// <copyright file="SourceComposer.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The Source Composer
/// </summary>
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type

public static class SourceComposer
{
    /// <summary>
    /// Create a class
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <param name="result">result</param>
    /// <returns>class</returns>
    public static ref Class Class(string identifier, in Class result = default)
    {
        ref var instance = ref Unsafe.AsRef(in result);
        instance.Identifier = identifier;
        return ref instance;
    }
}
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type