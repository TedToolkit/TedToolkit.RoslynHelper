// -----------------------------------------------------------------------
// <copyright file="TypeDeclaration.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// The builder for class.
/// </summary>
/// <param name="identifier">identifier.</param>
/// <param name="type">Type of the class.</param>
public sealed class TypeDeclaration(string identifier, TypeDeclarationType type) :
    ConditionalCompilationSyntax,
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

        this.AddAttributes(ref builder);
        this.AddAccessibility(ref builder);
        this.AddStatic(ref builder);
        this.AddReadonly(ref builder);
        this.AddPolymorphism(ref builder);
        this.AddUnsafe(ref builder);
        if (type is TypeDeclarationType.REF_STRUCT)
        {
            builder.Append("ref ");
        }

        this.AddPartial(ref builder);
        builder.Append(type switch
        {
            TypeDeclarationType.CLASS => "class ",
            TypeDeclarationType.STRUCT or TypeDeclarationType.REF_STRUCT => "struct ",
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
            for (var i = 0; i < BaseTypes.Count; i++)
            {
                var (baseType, baseTypeCondition) = BaseTypes[i];
                var hasFollowingBaseType = i < BaseTypes.Count - 1;
                builder.AppendLine();

                if (baseTypeCondition is not null)
                {
                    builder.Append("#if ");
                    baseTypeCondition.ToCode(ref builder);
                    builder.AppendLine();
                }

                builder.Append('\t');
                baseType.ToCode(ref builder);
                if (hasFollowingBaseType)
                {
                    builder.Append(',');
                }

                if (baseTypeCondition is not null)
                {
                    builder.AppendLine();
                    builder.Append("#endif");
                }
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
    public List<ConditionalItem<Attribute>> Attributes
    {
        get
        {
            return field ??= [];
        }
    }

    /// <summary>
    /// Gets the base DataTypes.
    /// </summary>
#pragma warning disable S2325
    public List<ConditionalItem<DataType>> BaseTypes
#pragma warning restore S2325
        => field ??= [];

    /// <summary>
    /// Add the baseType.
    /// </summary>
    /// <param name="baseType">the baseType.</param>
    /// <param name="condition">the optional conditional compilation expression.</param>
    /// <returns>the item.</returns>
    public TypeDeclaration AddBaseType(DataType baseType, PreprocessorExpression? condition = null)
    {
        BaseTypes.Add(new ConditionalItem<DataType>(baseType, condition));
        return this;
    }

    /// <summary>
    /// Add the baseType.
    /// </summary>
    /// <typeparam name="T">BaseType.</typeparam>
    /// <param name="alias">alias.</param>
    /// <param name="condition">the optional conditional compilation expression.</param>
    /// <returns>the item.</returns>
    public TypeDeclaration AddBaseType<T>(string alias = "global", PreprocessorExpression? condition = null)
    {
        BaseTypes.Add(new ConditionalItem<DataType>(DataType.FromType<T>(alias), condition));
        return this;
    }

    /// <summary>
    /// Add the baseType.
    /// </summary>
    /// <param name="baseType">type.</param>
    /// <param name="condition">the optional conditional compilation expression.</param>
    /// <returns>the item.</returns>
    /// <exception cref="ArgumentNullException">type is null.</exception>
    public TypeDeclaration AddBaseType(Type baseType, PreprocessorExpression? condition = null)
    {
        if (baseType is null)
        {
            throw new ArgumentNullException(nameof(baseType));
        }

        BaseTypes.Add(new ConditionalItem<DataType>(DataType.FromType(baseType), condition));
        return this;
    }

    /// <inheritdoc />
    public List<IMember> Members
    {
        get
        {
            return field ??= [];
        }
    }

    /// <inheritdoc />
    public bool IsReadonly { get; set; }

    /// <inheritdoc />
    public List<IRootDescriptionItem> RootDescriptions
    {
        get
        {
            return field ??= [];
        }
    }

    /// <inheritdoc />
    public List<Parameter> Parameters
    {
        get
        {
            return field ??= [];
        }
    }

    /// <inheritdoc />
    public Polymorphism Polymorphism { get; set; }

    /// <inheritdoc />
    public List<TypeParameter> TypeParameters
    {
        get
        {
            return field ??= [];
        }
    }
}