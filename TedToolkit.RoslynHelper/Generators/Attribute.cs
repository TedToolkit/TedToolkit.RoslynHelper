// -----------------------------------------------------------------------
// <copyright file="Attribute.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// Attribute
/// </summary>
#pragma warning disable CA1711
public record struct Attribute :
#pragma warning restore CA1711
    IToCode
{
    /// <inheritdoc/>
    public void ToCode(ref SourceBuilder builder)
        => throw new NotImplementedException();
}