// -----------------------------------------------------------------------
// <copyright file="PreprocessorOrExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

/// <summary>
/// Represents a logical OR expression for preprocessor symbols.
/// </summary>
/// <param name="left">The left operand.</param>
/// <param name="right">The right operand.</param>
internal sealed class PreprocessorOrExpression(
    PreprocessorExpression left,
    PreprocessorExpression right) : PreprocessorExpression
{
    /// <inheritdoc />
    internal override void WriteTo(ref SourceBuilder builder)
    {
        left.WriteTo(ref builder);
        builder.Append(" || ");
        right.WriteTo(ref builder);
    }
}
