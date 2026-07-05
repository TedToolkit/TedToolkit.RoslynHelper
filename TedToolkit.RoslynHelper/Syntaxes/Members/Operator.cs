// -----------------------------------------------------------------------
// <copyright file="Operator.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// Add the operator.
/// </summary>
/// <param name="returnType">return type.</param>
/// <param name="operatorName">operator name.</param>
public sealed class Operator(ReturnType returnType, string operatorName) :
    ConditionalCompilationSyntax,
    IMember,
    IParameters,
    IAttributes,
    IUnsafe,
    IPartial,
    IRootDescription,
    IStatementOwner
{
    /// <inheritdoc/>
    protected override void WriteSyntax(ref SourceBuilder builder)
    {
        this.AddDescriptions(ref builder);
        this.AddAttributes(ref builder);

        builder.Append("public static ");
        returnType.ToCode(ref builder);
        builder.Append(" operator ");

        builder.Append(operatorName);

        this.AddParametersNoSkip(ref builder);

        this.AddStatementsNoSkip(ref builder);
    }

    /// <inheritdoc/>
    public List<Parameter> Parameters
    {
        get
        {
            return field ??= [];
        }
    }

    /// <inheritdoc/>
    public List<ConditionalItem<Attribute>> Attributes
    {
        get
        {
            return field ??= [];
        }
    }

    /// <inheritdoc/>
    public bool IsUnsafe { get; set; }

    /// <inheritdoc/>
    public bool IsPartial { get; set; }

    /// <inheritdoc/>
    public List<IRootDescriptionItem> RootDescriptions
    {
        get
        {
            return field ??= [];
        }
    }

    /// <inheritdoc/>
    public List<IStatement> Statements
    {
        get
        {
            return field ??= [];
        }
    }
}