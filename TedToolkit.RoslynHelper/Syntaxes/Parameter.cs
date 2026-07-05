// -----------------------------------------------------------------------
// <copyright file="Parameter.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Reflection;
using System.Runtime.CompilerServices;

using Microsoft.CodeAnalysis;

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// The Parameter.
/// </summary>
/// <param name="type">The Parameter Type.</param>
/// <param name="identifier">The Variable.</param>
public sealed class Parameter(DataType type, string identifier) :
    IToCode,
    IDescription,
    IVariable,
    IAttributes,
    IDefault
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Parameter"/> class.
    /// Create from a type.
    /// </summary>
    /// <param name="type">type.</param>
    /// <param name="identifier">identifier.</param>
    public Parameter(Type type, string identifier)
        : this(DataType.FromType(type), identifier)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Parameter"/> class.
    /// Create from a type.
    /// </summary>
    /// <param name="type">type.</param>
    /// <param name="identifier">identifier.</param>
    /// <param name="compilation">compilation.</param>
    public Parameter(ITypeSymbol type, string identifier, Compilation? compilation = null)
        : this(DataType.FromSymbol(type, compilation), identifier)
    {
    }

    /// <summary>
    /// Create from a symbol.
    /// </summary>
    /// <param name="parameterSymbol">parameter symbol.</param>
    /// <param name="compilation">compilation.</param>
    /// <returns>parameter.</returns>
    /// <exception cref="ArgumentNullException">parameterSymbol is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Parameter FromSymbol(IParameterSymbol parameterSymbol, Compilation? compilation = null)
    {
        if (parameterSymbol is null)
        {
            throw new ArgumentNullException(nameof(parameterSymbol));
        }

        return FromSymbol(parameterSymbol, DataType.FromSymbol(parameterSymbol.Type, compilation), compilation);
    }

    /// <summary>
    /// From a symbol with type.
    /// </summary>
    /// <param name="parameterSymbol">parameter symbol.</param>
    /// <param name="type">data type.</param>
    /// <param name="compilation">compilation.</param>
    /// <returns>parameter.</returns>
    /// <exception cref="ArgumentNullException">parameterSymbol or type is null.</exception>
    public static Parameter FromSymbol(IParameterSymbol parameterSymbol, DataType type, Compilation? compilation = null)
    {
        if (parameterSymbol is null)
        {
            throw new ArgumentNullException(nameof(parameterSymbol));
        }

        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }

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

                case RefKind.Out:
                    type = type.Out;
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

        foreach (var attributeData in parameterSymbol.GetAttributes())
        {
            parameter.AddAttribute(Attribute.FromSymbol(attributeData, compilation));
        }

        if (parameterSymbol.IsParams)
        {
            parameter = parameter.Params;
        }
        else if (parameterSymbol.IsThis
                 || (parameterSymbol.Ordinal is 0
                     && parameterSymbol.ContainingSymbol is IMethodSymbol { IsExtensionMethod: true, }))
        {
            parameter = parameter.This;
        }

        if (parameterSymbol.HasExplicitDefaultValue)
        {
            switch (parameterSymbol.ExplicitDefaultValue)
            {
                case string str:
                    parameter.AddDefault(str.ToLiteral());
                    break;

                case bool b:
                    parameter.AddDefault(b.ToLiteral());
                    break;

                case { } obj:
                    parameter.AddDefault(obj.ToString().ToSimpleName());
                    break;

                default:
                    parameter.AddDefault();
                    break;
            }
        }

        return parameter;
    }

    /// <summary>
    /// Create from info.
    /// </summary>
    /// <param name="parameterInfo">parameter info.</param>
    /// <param name="alias">alias.</param>
    /// <returns>parameter.</returns>
    /// <exception cref="ArgumentNullException">parameter info or type is null.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Parameter FromInfo(ParameterInfo parameterInfo, string alias = "global")
    {
        if (parameterInfo is null)
        {
            throw new ArgumentNullException(nameof(parameterInfo));
        }

        return FromInfo(parameterInfo, DataType.FromType(parameterInfo.ParameterType, alias));
    }

    /// <summary>
    /// Create from info.
    /// </summary>
    /// <param name="parameterInfo">parameter info.</param>
    /// <param name="type">data type.</param>
    /// <returns>parameter.</returns>
    /// <exception cref="ArgumentNullException">parameter info or type is null.</exception>
    public static Parameter FromInfo(ParameterInfo parameterInfo, DataType type)
    {
        if (parameterInfo is null)
        {
            throw new ArgumentNullException(nameof(parameterInfo));
        }

        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        if (parameterInfo.ParameterType.IsByRef)
        {
            if (parameterInfo.CustomAttributes.Any(a =>
                    a.AttributeType.FullName == "System.Runtime.CompilerServices.ScopedRefAttribute"))
            {
                if (parameterInfo.IsIn)
                {
                    if (parameterInfo.CustomAttributes.Any(a =>
                            a.AttributeType.FullName == "System.Runtime.CompilerServices.RequiresLocationAttribute"))
                    {
                        type = type.ScopedRefReadonly;
                    }
                    else
                    {
                        type = type.ScopedIn;
                    }
                }
                else
                {
                    type = type.ScopedRef;
                }
            }
            else if (parameterInfo.IsOut)
            {
                type = type.Out;
            }
            else if (parameterInfo.IsIn)
            {
                if (parameterInfo.CustomAttributes.Any(a =>
                        a.AttributeType.FullName == "System.Runtime.CompilerServices.RequiresLocationAttribute"))
                {
                    type = type.RefReadonly;
                }
                else
                {
                    type = type.In;
                }
            }
            else
            {
                type = type.Ref;
            }
        }

        var parameter = new Parameter(type, parameterInfo.Name);

        if (parameterInfo.IsDefined(typeof(ParamArrayAttribute), false))
        {
            parameter = parameter.Params;
        }
        else if (parameterInfo.Position is 0 && parameterInfo.Member.IsDefined(typeof(ExtensionAttribute), false))
        {
            parameter = parameter.This;
        }

        if (parameterInfo.HasDefaultValue)
        {
            if (parameterInfo.DefaultValue is not { } defaultValue)
            {
                parameter.AddDefault();
            }
            else if (defaultValue is string str)
            {
                parameter.AddDefault(str.ToLiteral());
            }
            else
            {
                parameter.AddDefault(defaultValue.ToString().ToSimpleName());
            }
        }

        return parameter;
    }

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
        return new DescriptionParam(Variable, Descriptions);
    }

    /// <inheritdoc/>
    public string Variable
    {
        get
        {
            return identifier.ToValidIdentifier();
        }
    }

    /// <inheritdoc/>
    public IExpression? Default { get; set; }

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
        this.AddDefault(ref builder);
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
    /// Gets or sets parameter Kind.
    /// </summary>
    public ParameterKind ParameterKind { get; set; }

    /// <summary>
    /// Gets <see cref="ParameterKind.PARAMS"/>.
    /// </summary>
    /// <returns>item.</returns>
    public Parameter Params
    {
        get
        {
            ParameterKind = ParameterKind.PARAMS;
            return this;
        }
    }

    /// <summary>
    /// Gets <see cref="ParameterKind.THIS"/>.
    /// </summary>
    /// <returns>item.</returns>
    public Parameter This
    {
        get
        {
            ParameterKind = ParameterKind.THIS;
            return this;
        }
    }
}