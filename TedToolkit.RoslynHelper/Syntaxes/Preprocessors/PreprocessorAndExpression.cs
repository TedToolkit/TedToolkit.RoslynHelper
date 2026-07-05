// -----------------------------------------------------------------------
// <copyright file="PreprocessorAndExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

/// <summary>
/// Represents a logical AND expression for preprocessor symbols.
/// </summary>
/// <param name="left">The left operand.</param>
/// <param name="right">The right operand.</param>
internal sealed class PreprocessorAndExpression(
    PreprocessorExpression left,
    PreprocessorExpression right) : PreprocessorExpression
{
    /// <inheritdoc />
    internal override void WriteTo(ref SourceBuilder builder)
    {
        WriteOperand(ref builder, left);
        builder.Append(" && ");
        WriteOperand(ref builder, right);
    }

    /// <summary>
    /// Writes an operand and adds parentheses when needed to preserve precedence.
    /// </summary>
    /// <param name="builder">The target builder.</param>
    /// <param name="expression">The operand to write.</param>
    private static void WriteOperand(ref SourceBuilder builder, PreprocessorExpression expression)
    {
        if (expression is PreprocessorOrExpression)
        {
            builder.Append('(');
            expression.WriteTo(ref builder);
            builder.Append(')');
            return;
        }

        expression.WriteTo(ref builder);
    }
}