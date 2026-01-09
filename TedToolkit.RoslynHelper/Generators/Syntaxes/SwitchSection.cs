// -----------------------------------------------------------------------
// <copyright file="SwitchSection.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The switch Sections
/// </summary>
public sealed class SwitchSection :
    IToCode,
    IStatementOwner
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        foreach (var switchLabel in Labels)
        {
            builder.AppendLine();
            switchLabel.ToCode(ref builder);
        }

        builder.Indent();
        foreach (var statement in Statements)
        {
            builder.AppendLine();
            statement.ToCode(ref builder);
        }

        builder.Dedent();
    }

    /// <inheritdoc />
    public List<IStatement> Statements
        => field ??= [];

    /// <summary>
    /// The labels
    /// </summary>
#pragma warning disable S2325
    public List<SwitchLabel> Labels
#pragma warning restore S2325
        => field ??= [];

    /// <summary>
    /// Add a label
    /// </summary>
    /// <param name="label">label</param>
    /// <returns>value</returns>
    public SwitchSection AddLabel(SwitchLabel label)
    {
        Labels.Add(label);
        return this;
    }
}