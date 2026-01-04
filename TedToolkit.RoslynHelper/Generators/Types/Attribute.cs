// -----------------------------------------------------------------------
// <copyright file="Attribute.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Types;

/// <summary>
/// Attribute
/// </summary>
/// <param name="Type">The Type</param>
#pragma warning disable CA1711
public record struct Attribute(MemberAccess Type) :
#pragma warning restore CA1711
    IToCode,
    IArguments
{
    /// <inheritdoc/>
    public void ToCode(ref SourceBuilder builder)
    {
        Type.ToCode(ref builder);
        this.AddArguments(ref builder);
    }

    /// <inheritdoc />
    public List<Argument> Arguments
        => field ??= [];
}