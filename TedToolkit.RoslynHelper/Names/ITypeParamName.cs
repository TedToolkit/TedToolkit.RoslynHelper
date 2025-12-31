// -----------------------------------------------------------------------
// <copyright file="ITypeParamName.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TedToolkit.RoslynHelper.Names;

/// <summary>
///     The type param name.
/// </summary>
public interface ITypeParamName
{
    /// <summary>
    ///     Syntax
    /// </summary>
    TypeParameterSyntax Syntax { get; }

    /// <summary>
    ///     Syntax name
    /// </summary>
    string SyntaxName { get; }

    /// <summary>
    ///     THe Constraint clause.
    /// </summary>
    TypeParameterConstraintClauseSyntax? ConstraintClause { get; }
}