// -----------------------------------------------------------------------
// <copyright file="ObjectCreationExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Creation expression
/// </summary>
/// <param name="dataType">data type</param>
public sealed class ObjectCreationExpression(DataType? dataType = null) :
    IArguments,
    IExpression
{
    /// <inheritdoc />
    public List<Argument> Arguments
        => field ??= [];

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append("new ");
        dataType?.ToCode(ref builder);
        this.AddArgumentsNoSkip(ref builder);
    }

    /// <inheritdoc />
    public void ToCref(ref SourceBuilder builder)
        => ToCode(ref builder);
}