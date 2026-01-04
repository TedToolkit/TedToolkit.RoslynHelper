// -----------------------------------------------------------------------
// <copyright file="MemberAccessItem.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

namespace TedToolkit.RoslynHelper.Generators.Types;

/// <summary>
/// The member access item
/// </summary>
/// <param name="Identifier">name.</param>
public record struct MemberAccessItem(string Identifier) :
    IToCode,
    IToDescription
{
    /// <summary>
    /// Is null item
    /// </summary>
    public bool IsNull { get; set; }

    /// <summary>
    /// Is array item
    /// </summary>
    public bool IsArray { get; set; }

    /// <summary>
    /// The Types
    /// </summary>
#pragma warning disable S2325
    public List<MemberAccess> Types
#pragma warning restore S2325
        => field ??= [];

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        builder.Append(Identifier);
        if (Types.Count > 0)
        {
            builder.Append('<');
            var isNotStart = false;
            foreach (var memberAccess in Types)
            {
                if (isNotStart)
                    builder.Append(", ");

                memberAccess.ToCode(ref builder);

                isNotStart = true;
            }

            builder.Append('>');
        }

        if (IsNull)
            builder.Append('?');

        if (IsArray)
            builder.Append("[]");
    }

    /// <inheritdoc />
    public string ToDescription()
    {
        using var builder = ZString.CreateStringBuilder();
        builder.Append(Identifier);
        if (Types.Count > 0)
        {
            builder.Append('{');
            var isNotStart = false;
            foreach (var memberAccess in Types)
            {
                if (isNotStart)
                    builder.Append(", ");

                builder.Append(memberAccess.ToDescription());

                isNotStart = true;
            }

            builder.Append('}');
        }

        if (IsNull)
            builder.Append('?');

        if (IsArray)
            builder.Append("[]");

        return builder.ToString();
    }

    /// <summary>
    /// To the Member access item.
    /// </summary>
    /// <param name="identifier">identifier</param>
    /// <returns>item</returns>
#pragma warning disable CA2225
    public static implicit operator MemberAccessItem(string identifier)
#pragma warning restore CA2225
        => new(identifier);
}