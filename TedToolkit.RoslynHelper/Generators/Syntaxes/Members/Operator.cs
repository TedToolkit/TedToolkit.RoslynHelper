// -----------------------------------------------------------------------
// <copyright file="Operator.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Add the operator
/// </summary>
/// <param name="returnType">return type</param>
/// <param name="operatorName">operator name</param>
public sealed class Operator(ReturnType returnType, string operatorName) :
    IMember,
    IParameters,
    IAttributes,
    IUnsafe,
    IPartial,
    IRootDescription,
    IStatementOwner
{
    /// <inheritdoc/>
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddDescriptions(ref builder);
        this.AddAttributes(ref builder);

        builder.Append("public static ");
        returnType.ToCode(ref builder);
        builder.Append(" operator ");

        builder.Append(operatorName);

        builder.Append('(');

        this.AddParametersNoSkip(ref builder);

        this.AddStatementsNoSkip(ref builder);
    }

    /// <inheritdoc/>
    public List<Parameter> Parameters
        => field ??= [];

    /// <inheritdoc/>
    public List<Attribute> Attributes
        => field ??= [];

    /// <inheritdoc/>
    public bool IsUnsafe { get; set; }

    /// <inheritdoc/>
    public bool IsPartial { get; set; }

    /// <inheritdoc/>
    public List<IRootDescriptionItem> RootDescriptions
        => field ??= [];

    /// <inheritdoc/>
    public List<IStatement> Statements
        => field ??= [];
}