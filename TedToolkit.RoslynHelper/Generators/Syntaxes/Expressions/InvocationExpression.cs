// -----------------------------------------------------------------------
// <copyright file="InvocationExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The Invocation expression
/// </summary>
/// <param name="member">the member</param>
public sealed class InvocationExpression(IExpression member) :
    IParameters,
    IStatement
{
    /// <inheritdoc />
    public List<Parameter> Parameters
        => field ??= [];

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        member.ToCode(ref builder);
        this.AddParametersNoSkip(ref builder);
    }
}