// -----------------------------------------------------------------------
// <copyright file="SwitchStatement.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// Switch Statement.
/// </summary>
/// <param name="expression">expression.</param>
public sealed class SwitchStatement(IExpression expression) :
    ConditionalCompilationSyntax,
    IStatement
{
    /// <inheritdoc />
    protected override void WriteSyntax(ref SourceBuilder builder)
    {
        builder.Append("switch (");
        expression.ToCode(ref builder);
        builder.Append(')');
        builder.BeginBlock();

        var isNotStart = false;
        foreach (var switchSection in Sections)
        {
            if (isNotStart)
            {
                builder.AppendLine();
            }

            switchSection.ToCode(ref builder);
            isNotStart = true;
        }

        builder.EndBlock();
    }

    /// <summary>
    /// Gets sections.
    /// </summary>
#pragma warning disable S2325
    public List<SwitchSection> Sections
#pragma warning restore S2325
        => field ??= [];

    /// <summary>
    /// Add the section.
    /// </summary>
    /// <param name="section">section.</param>
    /// <returns>result.</returns>
    public SwitchStatement AddSection(SwitchSection section)
    {
        Sections.Add(section);
        return this;
    }
}