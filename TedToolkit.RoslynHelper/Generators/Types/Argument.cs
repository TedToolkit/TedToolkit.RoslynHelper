// -----------------------------------------------------------------------
// <copyright file="Argument.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Types;

/// <summary>
/// The argument
/// </summary>
/// <param name="Variable">variable name</param>
public record struct Argument(MemberAccess Variable) :
    IStorageKind,
    IToCode
{
    /// <summary>
    /// The parameter name
    /// </summary>
    public string ParameterName { get; set; } = "";

    /// <inheritdoc />
    public StorageKind StorageKind { get; set; }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        if (!string.IsNullOrEmpty(ParameterName))
        {
            builder.Append(ParameterName);
            builder.Append(": ");
        }

        this.AddStorageKind(ref builder);
        Variable.ToCode(ref builder);
    }
}