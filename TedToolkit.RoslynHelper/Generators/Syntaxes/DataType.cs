// -----------------------------------------------------------------------
// <copyright file="DataType.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The data Type
/// </summary>
/// <param name="Type">expression to the type</param>
public record struct DataType(IExpression Type) :
    IStorageKind,
    IToCode
{
    /// <inheritdoc />
    public StorageKind StorageKind { get; set; }

    /// <summary>
    /// Is Array
    /// </summary>
    public bool IsArray { get; set; }

    /// <summary>
    /// The pointer counter
    /// </summary>
    public int PointCounter { get; set; }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddStorageKind(ref builder);
        Type.ToCode(ref builder);
        if (IsArray)
            builder.Append("[]");

        if (PointCounter > 0)
            builder.Append('*', PointCounter);
    }
}