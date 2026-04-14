// -----------------------------------------------------------------------
// <copyright file="TypeParameter.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

using Microsoft.CodeAnalysis;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The TypeParameters.
/// </summary>
/// <param name="identifier">identifier.</param>
public sealed class TypeParameter(string identifier) :
    IToCode,
    IDescription,
    IVariable,
    IAttributes,
    IStorageKind
{
    /// <inheritdoc />
    public List<IDescriptionItem> Descriptions
    {
        get
        {
            return field ??= [];
        }
    }

    /// <inheritdoc />
    public IRootDescriptionItem ToRoot()
    {
        return new DescriptionTypeParam(Variable, Descriptions);
    }

    /// <inheritdoc/>
    public string Variable
    {
        get
        {
            return identifier;
        }
    }

    /// <inheritdoc />
    public List<Attribute> Attributes
    {
        get
        {
            return field ??= [];
        }
    }

    /// <summary>
    /// Gets constraints.
    /// </summary>
#pragma warning disable S2325
    public List<IExpression> Constraints
#pragma warning restore S2325
        => field ??= [];

    /// <inheritdoc />
    public StorageKind StorageKind { get; set; }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddAttributes(ref builder);
        this.AddStorageKind(ref builder);
        builder.Append(identifier);
    }

    /// <summary>
    /// To the constraint.
    /// </summary>
    /// <param name="builder">the builder.</param>
    internal void ToConstraint(ref SourceBuilder builder)
    {
        if (Constraints.Count is 0)
        {
            return;
        }

        builder.AppendLine();
        builder.Append("\twhere ");
        builder.Append(identifier);
        builder.Append(": ");

        var isNotStart = false;
        foreach (var constraint in Constraints)
        {
            if (isNotStart)
            {
                builder.Append(", ");
            }

            constraint.ToCode(ref builder);

            isNotStart = true;
        }
    }

    /// <summary>
    /// Create a type parameter from symbol.
    /// </summary>
    /// <param name="symbol">symbol.</param>
    /// <param name="compilation">compilation.</param>
    /// <returns>result.</returns>
    /// <exception cref="ArgumentNullException">throw if symbol is null.</exception>
    public static TypeParameter FromSymbol(ITypeParameterSymbol symbol, Compilation? compilation = null)
    {
        if (symbol is null)
        {
            throw new ArgumentNullException(nameof(symbol));
        }

        var result = new TypeParameter(symbol.Name);

        if (symbol.HasReferenceTypeConstraint)
        {
            result.AddClassConstraint();
        }

        if (symbol.HasUnmanagedTypeConstraint)
        {
            result.AddUnmanagedConstraint();
        }

        if (symbol.HasValueTypeConstraint)
        {
            result.AddStructConstraint();
        }

        if (symbol.HasNotNullConstraint)
        {
            result.AddNotNullConstraint();
        }

        foreach (var symbolConstraintType in symbol.ConstraintTypes)
        {
            result.AddConstraint(DataType.FromSymbol(symbolConstraintType, compilation));
        }

        if (symbol.HasConstructorConstraint)
        {
            result.AddNewConstraint();
        }

        if (symbol.AllowsRefLikeType)
        {
            result.AddRefStructConstraint();
        }

        return result;
    }

    /// <summary>
    /// Add constraint.
    /// </summary>
    /// <param name="constraint">the constraint.</param>
    /// <returns>self.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TypeParameter AddConstraint(IExpression constraint)
    {
        Constraints.Add(constraint);
        return this;
    }

    /// <summary>
    /// Add constraint.
    /// </summary>
    /// <param name="constraint">the constraint.</param>
    /// <returns>self.</returns>
    /// <exception cref="ArgumentNullException">constraint is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TypeParameter AddConstraint(DataType constraint)
    {
        if (constraint is null)
        {
            throw new ArgumentNullException(nameof(constraint));
        }

        Constraints.Add(constraint.Type);
        return this;
    }

    /// <summary>
    /// Add constraint.
    /// </summary>
    /// <param name="constraint">the constraint.</param>
    /// <returns>self.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TypeParameter AddConstraint(Type constraint)
    {
        Constraints.Add(DataType.FromType(constraint).Type);
        return this;
    }

    /// <summary>
    /// Add constraint.
    /// </summary>
    /// <typeparam name="T">constraint type.</typeparam>
    /// <returns>self.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TypeParameter AddConstraint<T>()
    {
        Constraints.Add(DataType.FromType<T>().Type);
        return this;
    }

    /// <summary>
    /// Add struct constraint.
    /// </summary>
    /// <returns>self.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TypeParameter AddStructConstraint()
    {
        Constraints.Add(new SimpleNameExpression("struct"));
        return this;
    }

    /// <summary>
    /// Add class constraint.
    /// </summary>
    /// <returns>self.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TypeParameter AddClassConstraint()
    {
        Constraints.Add(new SimpleNameExpression("class"));
        return this;
    }

    /// <summary>
    /// Add class null constraint.
    /// </summary>
    /// <returns>self.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TypeParameter AddClassNullConstraint()
    {
        Constraints.Add(new SimpleNameExpression("class?"));
        return this;
    }

    /// <summary>
    /// Add not null constraint.
    /// </summary>
    /// <returns>self.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TypeParameter AddNotNullConstraint()
    {
        Constraints.Add(new SimpleNameExpression("notnull"));
        return this;
    }

    /// <summary>
    /// Add new constraint.
    /// </summary>
    /// <returns>self.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TypeParameter AddNewConstraint()
    {
        Constraints.Add(new SimpleNameExpression("new()"));
        return this;
    }

    /// <summary>
    /// Add unmanaged constraint.
    /// </summary>
    /// <returns>self.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TypeParameter AddUnmanagedConstraint()
    {
        Constraints.Add(new SimpleNameExpression("unmanaged"));
        return this;
    }

    /// <summary>
    /// Add allows ref struct constraint.
    /// </summary>
    /// <returns>self.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TypeParameter AddRefStructConstraint()
    {
        Constraints.Add(new SimpleNameExpression("allows ref struct"));
        return this;
    }
}