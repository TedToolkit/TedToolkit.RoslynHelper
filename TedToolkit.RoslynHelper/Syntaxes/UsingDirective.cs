// -----------------------------------------------------------------------
// <copyright file="UsingDirective.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// A file-level using directive.
/// </summary>
/// <param name="name">The imported namespace or type name.</param>
public sealed class UsingDirective(IExpression name) : IToCode, IStatic
{
    /// <inheritdoc />
    public bool IsStatic { get; set; }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append("using ");
        this.AddStatic(ref builder);
        name.ToCode(ref builder);
        builder.Append(';');
    }
}