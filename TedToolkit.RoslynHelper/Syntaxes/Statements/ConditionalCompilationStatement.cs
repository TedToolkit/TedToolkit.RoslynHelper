// -----------------------------------------------------------------------
// <copyright file="ConditionalCompilationStatement.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// Base type for statements that support conditional compilation wrappers.
/// </summary>
public abstract class ConditionalCompilationStatement : IStatement, IConditionalCompilation
{
    /// <inheritdoc />
    public PreprocessorExpression? Condition { get; set; }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddConditionalCompilationStart(ref builder);
        this.WriteStatement(ref builder);
        this.AddConditionalCompilationEnd(ref builder);
    }

    /// <summary>
    /// Writes the underlying statement body without conditional compilation wrappers.
    /// </summary>
    /// <param name="builder">builder.</param>
    protected abstract void WriteStatement(ref SourceBuilder builder);
}
