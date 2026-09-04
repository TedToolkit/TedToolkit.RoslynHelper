// -----------------------------------------------------------------------
// <copyright file="Argument.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// The argument.
/// </summary>
/// <param name="variable">variable name.</param>
public sealed class Argument(IExpression variable) :
    IStorageKind,
    IToCode
{
    /// <summary>
    /// Gets or sets the parameter name.
    /// </summary>
    public string ParameterName { get; set; } = "";

    /// <inheritdoc />
    public StorageKind StorageKind { get; set; }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        if (!string.IsNullOrEmpty(ParameterName))
        {
            builder.Append(ParameterName.ToValidIdentifier());
            builder.Append(": ");
        }

        this.AddStorageKind(ref builder);
        variable.ToCode(ref builder);
    }

    /// <summary>
    /// Create from the info.
    /// </summary>
    /// <param name="info">info.</param>
    /// <returns>argument.</returns>
    /// <exception cref="ArgumentNullException">the info is null.</exception>
    public static Argument FromInfo(ParameterInfo info)
    {
        if (info is null)
        {
            throw new ArgumentNullException(nameof(info));
        }

        var basicExpression = new Argument(info.Name.ToSimpleName());

        if (!info.ParameterType.IsByRef)
        {
            return basicExpression;
        }
        else if (info.IsOut)
        {
            return basicExpression.Out;
        }
        else if (info.IsIn)
        {
            return basicExpression.In;
        }
        else
        {
            return basicExpression.Ref;
        }
    }
}