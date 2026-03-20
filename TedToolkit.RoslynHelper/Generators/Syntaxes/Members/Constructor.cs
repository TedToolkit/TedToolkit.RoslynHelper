// -----------------------------------------------------------------------
// <copyright file="Constructor.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The constructor.
/// </summary>
public sealed class Constructor :
    IMember,
    IAccessibility,
    IStatementOwner,
    IRootDescription,
    IAttributes,
    IStatic,
    IUnsafe,
    IPartial,
    IParameters,
    IOwner
{
    /// <inheritdoc/>
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddDescriptions(ref builder);
        foreach (var parameter in Parameters)
        {
            parameter.ToRoot().ToDescription(ref builder);
        }

        this.AddAttributes(ref builder);
        this.AddAccessibility(ref builder);
        this.AddStatic(ref builder);
        this.AddUnsafe(ref builder);
        this.AddPartial(ref builder);

        builder.Append(Owner);

        this.AddParametersNoSkip(ref builder);

        Initializer?.ToCode(ref builder);

        this.AddStatementsNoSkip(ref builder);
    }

    /// <inheritdoc/>
    public List<IStatement> Statements
        => field ??= [];

    /// <inheritdoc/>
    public List<IRootDescriptionItem> RootDescriptions
        => field ??= [];

    /// <inheritdoc/>
    public List<Attribute> Attributes
        => field ??= [];

    /// <inheritdoc/>
    public List<Parameter> Parameters
        => field ??= [];

    /// <inheritdoc/>
    public string Owner { get; set; } = "";

    /// <inheritdoc />
    public Accessibility Accessibility { get; set; }

    /// <inheritdoc/>
    public bool IsUnsafe { get; set; }

    /// <inheritdoc/>
    public bool IsPartial { get; set; }

    /// <inheritdoc/>
    public bool IsStatic { get; set; }

    /// <summary>
    ///  Gets or sets initializer.
    /// </summary>
    public ConstructorInitializer? Initializer { get; set; }

    /// <summary>
    /// Add initializer.
    /// </summary>
    /// <param name="initializer">initializer.</param>
    /// <returns>self.</returns>
    public Constructor AddInitializer(ConstructorInitializer initializer)
    {
        Initializer = initializer;
        return this;
    }
}