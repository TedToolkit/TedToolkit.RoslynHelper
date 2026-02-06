// -----------------------------------------------------------------------
// <copyright file="Extension.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// The extension for the member.
/// </summary>
/// <param name="parameter">parameter.</param>
public sealed class Extension(Parameter parameter) :
    IMember,
    IMemberOwner,
    ITypeParameters
{
    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        parameter.ToRoot().ToDescription(ref builder);
        builder.Append("extension");
        this.AddTypeParameters(ref builder);
        builder.Append('(');
        parameter.ToCode(ref builder);
        builder.Append(')');
        this.AddTypeParameterConstraints(ref builder);
        this.AddMembers(ref builder);
    }

    /// <inheritdoc />
    public List<IMember> Members
        => field ??= [];

    /// <inheritdoc />
    public List<TypeParameter> TypeParameters
        => field ??= [];
}