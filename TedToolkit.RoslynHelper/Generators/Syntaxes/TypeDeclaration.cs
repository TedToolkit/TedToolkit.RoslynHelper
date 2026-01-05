// -----------------------------------------------------------------------
// <copyright file="TypeDeclaration.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators.Delegates;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The builder for class
/// </summary>
/// <param name="Identifier">identifier</param>
/// <param name="Type">Type of the class</param>
public record struct TypeDeclaration(string Identifier, TypeDeclarationType Type) :
    IAccessibility,
    IUnsafe,
    IStatic,
    IPartial,
    IMember,
    IMemberOwner,
    IAttributes,
    IReadonly,
    IDescription,
    IParameters,
    IPolymorphism
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddSummary(ref builder);
        this.AddParametersSummary(ref builder);
        this.AddAttributes(ref builder);
        this.AddAccessibility(ref builder);
        this.AddStatic(ref builder);
        this.AddReadonly(ref builder);
        this.AddPolymorphism(ref builder);
        this.AddUnsafe(ref builder);
        this.AddPartial(ref builder);
        builder.Append(Type switch
        {
            TypeDeclarationType.CLASS => "class ",
            TypeDeclarationType.STRUCT => "struct ",
            TypeDeclarationType.REF_STRUCT => "ref struct ",
            TypeDeclarationType.RECORD => "record ",
            TypeDeclarationType.RECORD_STRUCT => "record struct ",
            TypeDeclarationType.INTERFACE => "interface ",
            _ => throw new InvalidOperationException("The type is invalid."),
        });
        builder.Append(Identifier);
        this.AddParameters(ref builder);

        if (BaseTypes.Count > 0)
        {
            builder.Append(" :");
            var isNotStart = false;
            foreach (var memberAccess in BaseTypes)
            {
                if (isNotStart)
                    builder.AppendLine(',');
                else
                    builder.AppendLine();

                builder.Append('\t');
                memberAccess.ToCode(ref builder);

                isNotStart = true;
            }
        }

        this.AddMembers(ref builder);
    }

    /// <inheritdoc />
    public Accessibility Accessibility { get; set; }

    /// <inheritdoc />
    public bool IsUnsafe { get; set; }

    /// <inheritdoc />
    public bool IsStatic { get; set; }

    /// <inheritdoc />
    public bool IsPartial { get; set; }

    /// <inheritdoc />
    public List<Attribute> Attributes
        => field ??= [];

    /// <summary>
    /// The base Types
    /// </summary>
#pragma warning disable S2325
    public List<IExpression> BaseTypes
#pragma warning restore S2325
        => field ??= [];

    /// <inheritdoc />
    public List<ToCodeHandler> Members
        => field ??= [];

    /// <inheritdoc />
    public bool IsReadonly { get; set; }

    /// <inheritdoc />
    public List<string> Description
        => field ??= [];

    /// <inheritdoc />
    public List<Parameter> Parameters
        => field ??= [];

    /// <inheritdoc />
    public Polymorphism Polymorphism { get; set; }
}