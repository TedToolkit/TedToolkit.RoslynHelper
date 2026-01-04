// -----------------------------------------------------------------------
// <copyright file="Parameter.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators.Delegates;

namespace TedToolkit.RoslynHelper.Generators.Types;

/// <summary>
/// The Parameter
/// </summary>
/// <param name="Type">The Parameter Type</param>
/// <param name="Identifier">The Variable</param>
public record struct Parameter(MemberAccess Type, string Identifier) :
    IToCode,
    IDescription,
    IVariables,
    IStorageKind
{
    /// <inheritdoc />
    public List<string> Description
        => field ??= [];

    /// <inheritdoc />
    public string Variable
    {
        readonly get => Identifier;

        set => Identifier = value;
    }

    /// <summary>
    /// The default value.
    /// </summary>
    public Argument? Default { get; internal set; }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddStorageKind(ref builder);
        Type.ToCode(ref builder);
        builder.Append(" @");
        builder.Append(Identifier);
        if (Default is null)
            return;

        builder.Append(" = ");
        Default.Value.ToCode(ref builder);
    }

    /// <inheritdoc />
    public StorageKind StorageKind { get; set; }
}