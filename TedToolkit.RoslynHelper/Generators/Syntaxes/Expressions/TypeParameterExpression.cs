// -----------------------------------------------------------------------
// <copyright file="TypeParameterExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The type param Expression
/// </summary>
/// <param name="expression">expression</param>
/// <param name="types">the types</param>
public sealed class TypeParameterExpression(IExpression expression, params DataType[] types) :
    IExpression
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        expression.ToCode(ref builder);
        if (types.Length is 0)
            return;

        builder.Append('<');
        var isNotStart = false;
        foreach (var type in types)
        {
            if (isNotStart)
                builder.Append(", ");

            type.ToCode(ref builder);
            isNotStart = true;
        }

        builder.Append('>');
    }
}