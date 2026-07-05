// -----------------------------------------------------------------------
// <copyright file="Method.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// The method.
/// </summary>
/// <param name="identifier">name.</param>
/// <param name="returnType">ReturnType.</param>
public sealed class Method(string identifier, ReturnType? returnType = null) :
    ConditionalCompilationSyntax,
    IMember,
    IParameters,
    IAttributes,
    IAccessibility,
    IUnsafe,
    IPartial,
    IStatic,
    IReadonly,
    IPolymorphism,
    IRootDescription,
    IStatementOwner,
    IStatement,
    ITypeParameters
{
    /// <inheritdoc/>
    protected override void WriteSyntax(ref SourceBuilder builder)
    {
        this.AddDescriptions(ref builder);
        foreach (var parameter in Parameters)
        {
            parameter.ToRoot().ToDescription(ref builder);
        }

        foreach (var typeParameter in TypeParameters)
        {
            typeParameter.ToRoot().ToDescription(ref builder);
        }

        returnType?.ToRoot().ToDescription(ref builder);

        this.AddAttributes(ref builder);
        this.AddAccessibility(ref builder);
        this.AddStatic(ref builder);
        this.AddReadonly(ref builder);
        this.AddPolymorphism(ref builder);
        this.AddUnsafe(ref builder);
        this.AddPartial(ref builder);
        if (IsExtern)
        {
            builder.Append("extern ");
        }

        if (returnType is not null)
        {
            returnType.ToCode(ref builder);
        }
        else
        {
            builder.Append("void");
        }

        builder.Append(' ');

        builder.Append(identifier.ToValidIdentifier());
        this.AddTypeParameters(ref builder);
        this.AddParametersNoSkip(ref builder);
        this.AddTypeParameterConstraints(ref builder);

        if (IsPartial || IsExtern)
        {
            this.AddStatements(ref builder);
        }
        else
        {
            this.AddStatementsNoSkip(ref builder);
        }
    }

    /// <inheritdoc/>
    public List<Parameter> Parameters
    {
        get
        {
            return field ??= [];
        }
    }

    /// <inheritdoc/>
    public List<ConditionalItem<Attribute>> Attributes
    {
        get
        {
            return field ??= [];
        }
    }

    /// <inheritdoc/>
    public Accessibility Accessibility { get; set; }

    /// <inheritdoc/>
    public bool IsUnsafe { get; set; }

    /// <inheritdoc/>
    public bool IsPartial { get; set; }

    /// <inheritdoc />
    public bool IsStatic { get; set; }

    /// <inheritdoc />
    public Polymorphism Polymorphism { get; set; }

    /// <inheritdoc />
    public List<IRootDescriptionItem> RootDescriptions
    {
        get
        {
            return field ??= [];
        }
    }

    /// <inheritdoc />
    public List<IStatement> Statements
    {
        get
        {
            return field ??= [];
        }
    }

    /// <inheritdoc />
    public bool IsReadonly { get; set; }

    /// <summary>
    /// If it is marked as extern
    /// </summary>
    public bool IsExtern { get; set; }

    /// <summary>
    /// Gets <see langword="extern"/>.
    /// </summary>
    public Method Extern
    {
        get
        {
            this.IsExtern = true;
            return this;
        }
    }

    /// <inheritdoc />
    public List<TypeParameter> TypeParameters
    {
        get
        {
            return field ??= [];
        }
    }
}
