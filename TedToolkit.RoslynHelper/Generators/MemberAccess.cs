// -----------------------------------------------------------------------
// <copyright file="MemberAccess.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The member access.
/// </summary>
public record struct MemberAccess() :
    IToCode,
    IToDescription
{
    /// <summary>
    /// The items
    /// </summary>
#pragma warning disable S2325
    public List<MemberAccessItem> Items
#pragma warning restore S2325
        => field ??= [];

    /// <summary>
    /// The alias
    /// </summary>
    public string Alias { get; set; } = "";

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        if (!string.IsNullOrEmpty(Alias))
        {
            builder.Append(Alias);
            builder.Append("::");
        }

        var isNotStart = false;
        foreach (var memberAccessItem in Items)
        {
            if (isNotStart)
                builder.Append('.');

            memberAccessItem.ToCode(ref builder);

            isNotStart = true;
        }
    }

    /// <inheritdoc />
    public string ToDescription()
    {
        using var builder = ZString.CreateStringBuilder();
        if (!string.IsNullOrEmpty(Alias))
        {
            builder.Append(Alias);
            builder.Append("::");
        }

        var isNotStart = false;
        foreach (var memberAccessItem in Items)
        {
            if (isNotStart)
                builder.Append('.');

            builder.Append(memberAccessItem.ToDescription());

            isNotStart = true;
        }

        return builder.ToString();
    }

    /// <summary>
    /// To the Member access item.
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <returns>item</returns>
#pragma warning disable CA2225
    public static implicit operator MemberAccess(string identifier)
#pragma warning restore CA2225
    {
        var result = new MemberAccess();
        result.Items.Add(identifier);
        return result;
    }
}