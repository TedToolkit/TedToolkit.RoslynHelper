// -----------------------------------------------------------------------
// <copyright file="PreprocessorUnaryExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

/// <summary>
/// Represents a logical NOT expression for a preprocessor symbol or expression.
/// </summary>
/// <param name="operand">The operand to negate.</param>
internal sealed class PreprocessorUnaryExpression(PreprocessorExpression operand) : PreprocessorExpression
{
    /// <inheritdoc />
    internal override void WriteTo(ref SourceBuilder builder)
    {
        builder.Append('!');
        if (operand is PreprocessorAndExpression or PreprocessorOrExpression)
        {
            builder.Append('(');
            operand.WriteTo(ref builder);
            builder.Append(')');
            return;
        }

        operand.WriteTo(ref builder);
    }
}