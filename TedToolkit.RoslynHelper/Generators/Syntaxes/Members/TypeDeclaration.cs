// -----------------------------------------------------------------------
// <copyright file="TypeDeclaration.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The builder for class.
/// </summary>
/// <param name="identifier">identifier.</param>
/// <param name="type">Type of the class.</param>
public sealed class TypeDeclaration(string identifier, TypeDeclarationType type) :
    IAccessibility,
    IUnsafe,
    IStatic,
    IPartial,
    IMember,
    IMemberOwner,
    IAttributes,
    IReadonly,
    IRootDescription,
    IParameters,
    IPolymorphism,
    ITypeParameters
{
    /// <summary>
    ///  Gets the identifier.
    /// </summary>
    public string Identifier { get; } = identifier.ToValidIdentifier();

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddDescriptions(ref builder);
        foreach (var parameter in Parameters)
            parameter.ToRoot().ToDescription(ref builder);

        foreach (var typeParameter in TypeParameters)
            typeParameter.ToRoot().ToDescription(ref builder);

        this.AddAttributes(ref builder);
        this.AddAccessibility(ref builder);
        this.AddStatic(ref builder);
        this.AddReadonly(ref builder);
        this.AddPolymorphism(ref builder);
        this.AddUnsafe(ref builder);
        this.AddPartial(ref builder);
        builder.Append(type switch
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
        this.AddTypeParameters(ref builder);
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

        this.AddTypeParameterConstraints(ref builder);
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
    /// Gets the base DataTypes.
    /// </summary>
#pragma warning disable S2325
    public List<DataType> BaseTypes
#pragma warning restore S2325
        => field ??= [];

    /// <summary>
    /// Add the baseType.
    /// </summary>
    /// <param name="baseType">the baseType.</param>
    /// <returns>the item.</returns>
    public TypeDeclaration AddBaseType(DataType baseType)
    {
        BaseTypes.Add(baseType);
        return this;
    }

    /// <summary>
    /// Add the baseType.
    /// </summary>
    /// <typeparam name="T">BaseType.</typeparam>
    /// <param name="alias">alias.</param>
    /// <returns>the item.</returns>
    public TypeDeclaration AddBaseType<T>(string alias = "")
    {
        BaseTypes.Add(DataType.FromType<T>(alias));
        return this;
    }

    /// <summary>
    /// Add the baseType.
    /// </summary>
    /// <param name="baseType">type.</param>
    /// <returns>the item.</returns>
    /// <exception cref="ArgumentNullException">type is null.</exception>
    public TypeDeclaration AddBaseType(Type baseType)
    {
        if (baseType is null)
            throw new ArgumentNullException(nameof(baseType));

        BaseTypes.Add(DataType.FromType(baseType));
        return this;
    }

    /// <inheritdoc />
    public List<IMember> Members
        => field ??= [];

    /// <inheritdoc />
    public bool IsReadonly { get; set; }

    /// <inheritdoc />
    public List<IRootDescriptionItem> RootDescriptions
        => field ??= [];

    /// <inheritdoc />
    public List<Parameter> Parameters
        => field ??= [];

    /// <inheritdoc />
    public Polymorphism Polymorphism { get; set; }

    /// <inheritdoc />
    public List<TypeParameter> TypeParameters
        => field ??= [];
}