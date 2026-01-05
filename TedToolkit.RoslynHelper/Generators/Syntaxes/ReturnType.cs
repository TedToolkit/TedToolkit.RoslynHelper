// -----------------------------------------------------------------------
// <copyright file="ReturnType.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The Return Type
/// </summary>
/// <param name="Type">return Type</param>
public record struct ReturnType(MemberAccessExpression Type) :
    IDescription,
    IStorageKind,
    IToCode
{
    /// <inheritdoc />
    public List<string> Description
        => field ??= [];

    /// <inheritdoc />
    public StorageKind StorageKind { get; set; }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddStorageKind(ref builder);
        Type.ToCode(ref builder);
    }
}