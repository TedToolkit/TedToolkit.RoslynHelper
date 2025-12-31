// -----------------------------------------------------------------------
// <copyright file="MethodName.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Text;

using Microsoft.CodeAnalysis;
using TedToolkit.RoslynHelper.Extensions;

namespace TedToolkit.RoslynHelper.Names;

/// <summary>
/// The name for the method.
/// </summary>
public class MethodName : TypeParametersName<IMethodSymbol>
{
    /// <summary>
    /// The method name.
    /// </summary>
    /// <param name="methodSymbol">symbol</param>
    internal MethodName(IMethodSymbol methodSymbol)
        : base(methodSymbol)
    {
        Parameters = methodSymbol.Parameters.GetNames().ToArray();
        ReturnType = methodSymbol.ReturnType.GetName();
        ContainingType = methodSymbol.ContainingType.GetName();
        Signature = new(methodSymbol);
    }

    /// <summary>
    ///     The signature of the method
    /// </summary>
    public MethodSignature Signature { get; }

    /// <summary>
    /// The parameters.
    /// </summary>
    public IReadOnlyList<ParameterName> Parameters { get; }

    /// <summary>
    ///     Return types.
    /// </summary>
    public TypeName ReturnType { get; }

    /// <summary>
    ///     ContainingType
    /// </summary>
    public TypeName ContainingType { get; }

    /// <inheritdoc/>
    private protected override IEnumerable<ITypeParameterSymbol> GetTypeParameters(IMethodSymbol symbol)
        => symbol.TypeParameters;

    /// <inheritdoc/>
    private protected override string GetSummaryName()
    {
        var builder = new StringBuilder(ContainingType.SummaryName)
            .Append('.')
            .Append(base.GetSummaryName());
        builder.Append('(').Append(string.Join(",", Parameters.Select(p =>
        {
            var stringBuilder = new StringBuilder();
            if (p.Symbol.ScopedKind is not ScopedKind.None)
                stringBuilder.Append("scoped ");

            switch (p.Symbol.RefKind)
            {
                case RefKind.Ref:
                    stringBuilder.Append("ref ");
                    break;

                case RefKind.Out:
                    stringBuilder.Append("in ");
                    break;

                case RefKind.In:
                    stringBuilder.Append("out ");
                    break;
            }

            return stringBuilder.Append(p.Type.SummaryName).ToString();
        }))).Append(')');
        return builder.ToString();
    }
}