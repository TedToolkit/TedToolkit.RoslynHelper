// -----------------------------------------------------------------------
// <copyright file="Parameter.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

using Microsoft.CodeAnalysis;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The Parameter
/// </summary>
/// <param name="type">The Parameter Type</param>
/// <param name="identifier">The Variable</param>
public sealed class Parameter(DataType type, string identifier) :
    IToCode,
    IDescription,
    IVariable,
    IAttributes
{
    /// <summary>
    /// Create from a type
    /// </summary>
    /// <param name="type">type</param>
    /// <param name="identifier">identifier</param>
    public Parameter(Type type, string identifier)
        : this(DataType.FromType(type), identifier)
    {
    }

    /// <summary>
    /// Create from a type
    /// </summary>
    /// <param name="type">type</param>
    /// <param name="identifier">identifier</param>
    public Parameter(ITypeSymbol type, string identifier)
        : this(new DataType(type), identifier)
    {
    }

    /// <summary>
    /// Create from a symbol
    /// </summary>
    /// <param name="parameterSymbol">parameter symbol</param>
    /// <returns>parameter</returns>
    /// <exception cref="ArgumentNullException">parameterSymbol is null</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Parameter FromSymbol(IParameterSymbol parameterSymbol)
    {
        if (parameterSymbol is null)
            throw new ArgumentNullException(nameof(parameterSymbol));

        return FromSymbol(parameterSymbol, new(parameterSymbol.Type));
    }

    /// <summary>
    /// From a symbol with type
    /// </summary>
    /// <param name="parameterSymbol">parameter symbol</param>
    /// <param name="type">data type</param>
    /// <returns>parameter</returns>
    /// <exception cref="ArgumentNullException">parameterSymbol or type is null</exception>
    public static Parameter FromSymbol(IParameterSymbol parameterSymbol, DataType type)
    {
        if (parameterSymbol is null)
            throw new ArgumentNullException(nameof(parameterSymbol));

        if (type is null)
            throw new ArgumentNullException(nameof(type));

        if (parameterSymbol.ScopedKind is ScopedKind.None)
        {
            switch (parameterSymbol.RefKind)
            {
                case RefKind.Ref:
                    type = type.Ref;
                    break;

                case RefKind.Out:
                    type = type.Out;
                    break;

                case RefKind.In:
                    type = type.In;
                    break;

                case RefKind.RefReadOnlyParameter:
                    type = type.RefReadonly;
                    break;
            }
        }
        else
        {
            switch (parameterSymbol.RefKind)
            {
                case RefKind.Ref:
                    type = type.ScopedRef;
                    break;

                case RefKind.In:
                    type = type.ScopedIn;
                    break;

                case RefKind.RefReadOnlyParameter:
                    type = type.ScopedRefReadonly;
                    break;
            }
        }

        var parameter = new Parameter(type, parameterSymbol.Name);

        if (parameterSymbol.IsParams)
            parameter = parameter.Params;
        else if (parameterSymbol.IsThis)
            parameter = parameter.This;

        if (parameterSymbol.HasExplicitDefaultValue)
        {
            if (parameterSymbol.ExplicitDefaultValue is not { } defaultValue)
                parameter.AddDefault();
            else if (parameterSymbol.Type.SpecialType is SpecialType.System_String)
                parameter.AddDefault(defaultValue.ToString().ToLiteral());
            else
                parameter.AddDefault(defaultValue.ToString().ToSimpleName());
        }

        return parameter;
    }

    /// <inheritdoc />
    public List<IDescriptionItem> Descriptions
        => field ??= [];

    /// <inheritdoc />
    public IRootDescriptionItem ToRoot()
        => new DescriptionParam(Variable, Descriptions);

    /// <inheritdoc/>
    public string Variable
        => identifier.ToValidIdentifier();

    /// <summary>
    /// The default value.
    /// </summary>
    public IExpression? Default { get; internal set; }

    /// <summary>
    /// Add null
    /// </summary>
    /// <returns>self</returns>
    public Parameter AddNull()
    {
        Default = SimpleNameExpression.Null;
        return this;
    }

    /// <summary>
    /// Add default
    /// </summary>
    /// <returns>self</returns>
    public Parameter AddDefault()
    {
        Default = SimpleNameExpression.Default;
        return this;
    }

    /// <summary>
    /// Add default
    /// </summary>
    /// <param name="value">defaultValue</param>
    /// <returns>self</returns>
    public Parameter AddDefault(IExpression value)
    {
        Default = value;
        return this;
    }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddAttributes(ref builder);
        switch (ParameterKind)
        {
            case ParameterKind.THIS:
                builder.Append("this ");
                break;

            case ParameterKind.PARAMS:
                builder.Append("params ");
                break;
        }

        type.ToCode(ref builder);
        builder.Append(' ');
        builder.Append(identifier.ToValidIdentifier());
        if (Default is null)
            return;

        builder.Append(" = ");
        Default.ToCode(ref builder);
    }

    /// <inheritdoc />
    public List<Attribute> Attributes
        => field ??= [];

    /// <summary>
    /// Parameter Kind.
    /// </summary>
    public ParameterKind ParameterKind { get; set; }

    /// <summary>
    /// <see cref="ParameterKind.PARAMS"/>
    /// </summary>
    /// <returns>item</returns>
    public Parameter Params
    {
        get
        {
            ParameterKind = ParameterKind.PARAMS;
            return this;
        }
    }

    /// <summary>
    /// <see cref="ParameterKind.THIS"/>
    /// </summary>
    /// <returns>item</returns>
    public Parameter This
    {
        get
        {
            ParameterKind = ParameterKind.THIS;
            return this;
        }
    }
}