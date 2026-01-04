// -----------------------------------------------------------------------
// <copyright file="Parameter.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

using TedToolkit.RoslynHelper.Generators.Delegates;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The Parameter
/// </summary>
/// <param name="Type">The Parameter Type</param>
/// <param name="Identifier">The Identifier</param>
public record struct Parameter(MemberAccess Type, string Identifier) :
    IToCode,
    IDescription
{
    /// <inheritdoc />
    public List<string> Description
        => field ??= [];

    /// <summary>
    /// The default value.
    /// </summary>
    public MemberHandler? Default { get; internal set; }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        Type.ToCode(ref builder);
        builder.Append(" @");
        builder.Append(Identifier);
        if (Default is null)
            return;

        builder.Append(" = ");
        Default(ref builder);
    }
}