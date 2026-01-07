// -----------------------------------------------------------------------
// <copyright file="Enum.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.RoslynHelper.Generators.Syntaxes;

/// <summary>
/// Enum
/// </summary>
/// <param name="identifer">identifier</param>
/// <param name="dataType">data type</param>
#pragma warning disable CA1711
public sealed class Enum(string identifer, DataType? dataType = null) :
#pragma warning restore CA1711
    IAccessibility,
    IMember,
    IAttributes,
    IRootDescription
{
    /// <inheritdoc/>
    public void ToCode(ref SourceBuilder builder)
    {
        this.AddDescriptions(ref builder);
        this.AddAttributes(ref builder);
        this.AddAccessibility(ref builder);
        builder.Append("enum ");
        builder.Append(identifer.ToValidIdentifier());

        if (dataType is not null)
        {
            builder.Append(" : ");
            dataType.ToCode(ref builder);
        }

        if (EnumMembers.Count is 0)
        {
            builder.Append(';');
            return;
        }

        builder.BeginBlock();

        var isNotStart = false;
        foreach (var member in EnumMembers)
        {
            builder.AppendLine();
            if (isNotStart)
                builder.AppendLine();

            member.ToCode(ref builder);
            isNotStart = true;
        }

        builder.EndBlock();
    }

    /// <inheritdoc/>
    public Accessibility Accessibility { get; set; }

    /// <inheritdoc/>
    public List<Attribute> Attributes
        => field ??= [];

    /// <summary>
    /// The enum members.
    /// </summary>
#pragma warning disable S2325
    public List<EnumMember> EnumMembers
#pragma warning restore S2325
        => field ??= [];

    /// <summary>
    /// Add enum member
    /// </summary>
    /// <param name="member">member</param>
    /// <returns>self</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Enum AddEnumMember(EnumMember member)
    {
        EnumMembers.Add(member);
        return this;
    }

    /// <inheritdoc/>
    public List<IRootDescriptionItem> RootDescriptions
        => field ??= [];
}