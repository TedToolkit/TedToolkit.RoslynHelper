// -----------------------------------------------------------------------
// <copyright file="Attribute.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// Attribute
/// </summary>
#pragma warning disable CA1711
public record struct Attribute :
#pragma warning restore CA1711
    ICode
{
    /// <inheritdoc/>
    public string ToCode()
        => throw new NotImplementedException();
}