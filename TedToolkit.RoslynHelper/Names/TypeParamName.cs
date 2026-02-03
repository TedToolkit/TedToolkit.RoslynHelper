// -----------------------------------------------------------------------
// <copyright file="TypeParamName.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TedToolkit.RoslynHelper.Extensions;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace TedToolkit.RoslynHelper.Names;

/// <summary>
/// Type param.
/// </summary>
[Obsolete("Do not use this method, try to use the generators instead!")]
public class TypeParamName : BaseName<ITypeParameterSymbol>, ITypeParamName
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TypeParamName"/> class.
    /// Create by the symbol.
    /// </summary>
    /// <param name="symbol">symbol.</param>
    internal TypeParamName(ITypeParameterSymbol symbol)
        : base(symbol)
    {
    }

    /// <summary>
    ///     Gets or sets modifier.
    /// </summary>
    public string Prefix { get; set; } = "";

    /// <summary>
    /// Gets syntax.
    /// </summary>
    public TypeParameterSyntax Syntax
    {
        get
        {
            var typeParameter = TypeParameter(Identifier(SyntaxName));
            return Symbol.Variance switch
            {
                VarianceKind.In => typeParameter.WithVarianceKeyword(Token(SyntaxKind.InKeyword)),
                VarianceKind.Out => typeParameter.WithVarianceKeyword(Token(SyntaxKind.OutKeyword)),
                _ => typeParameter,
            };
        }
    }

    /// <summary>
    ///     Gets the Syntax name.
    /// </summary>
    public string SyntaxName
        => Prefix + Symbol.Name;

    /// <summary>
    /// Gets constraint.
    /// </summary>
    public TypeParameterConstraintClauseSyntax? ConstraintClause
    {
        get
        {
            var constraints = new List<TypeParameterConstraintSyntax>();

            if (Symbol.HasReferenceTypeConstraint)
                constraints.Add(ClassOrStructConstraint(SyntaxKind.ClassConstraint));

            if (Symbol.HasUnmanagedTypeConstraint)
                constraints.Add(TypeConstraint(IdentifierName("unmanaged")));
            else if (Symbol.HasValueTypeConstraint)
                constraints.Add(ClassOrStructConstraint(SyntaxKind.StructConstraint));

            if (Symbol.HasNotNullConstraint)
                constraints.Add(TypeConstraint(IdentifierName("notnull")));

            foreach (var constraintType in Symbol.ConstraintTypes)
                constraints.Add(TypeConstraint(ParseTypeName(constraintType.GetName().FullName)));

            if (Symbol.HasConstructorConstraint)
                constraints.Add(ConstructorConstraint());

            if (constraints.Count is 0)
                return null;

            return TypeParameterConstraintClause(
                IdentifierName(Prefix + Symbol.Name),
                SeparatedList(constraints));
        }
    }
}