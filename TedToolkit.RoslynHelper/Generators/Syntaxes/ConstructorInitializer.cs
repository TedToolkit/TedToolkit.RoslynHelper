// -----------------------------------------------------------------------
// <copyright file="ConstructorInitializer.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Initializer.
/// </summary>
/// <param name="isBase">is base.</param>
public sealed class ConstructorInitializer(bool isBase) :
    IArguments,
    IToCode
{
    /// <inheritdoc />
    public List<Argument> Arguments
    {
        get
        {
            return field ??= [];
        }
    }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append(isBase ? " : base" : " : this");
        this.AddArgumentsNoSkip(ref builder);
    }
}