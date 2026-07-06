// -----------------------------------------------------------------------
// <copyright file="ConditionalCompilationSyntax.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

/// <summary>
/// Base type for syntax items that support conditional compilation wrappers.
/// </summary>
public abstract class ConditionalCompilationSyntax : IConditionalCompilation, IToCode
{
    /// <inheritdoc />
    public PreprocessorExpression? Condition { get; set; }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddConditionalCompilationStart(ref builder);
        this.WriteSyntax(ref builder);
        this.AddConditionalCompilationEnd(ref builder);
    }

    /// <summary>
    /// Writes the underlying syntax body without conditional compilation wrappers.
    /// </summary>
    /// <param name="builder">builder.</param>
    protected abstract void WriteSyntax(ref SourceBuilder builder);
}