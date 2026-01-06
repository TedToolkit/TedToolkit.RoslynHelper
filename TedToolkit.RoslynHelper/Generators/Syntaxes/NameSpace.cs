// -----------------------------------------------------------------------
// <copyright file="NameSpace.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators.Delegates;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// <see langword="namespace"/>
/// </summary>
/// <param name="Name">the name of the <see langword="namespace"/></param>
public record struct NameSpace(IExpression Name) :
    IMemberOwner,
    IToCode
{
    /// <inheritdoc />
    public List<SourceBuilderHandler> Members
        => field ??= [];

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append("namespace ");
        Name.ToCode(ref builder);

        this.AddMembers(ref builder);
    }
}