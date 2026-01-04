// -----------------------------------------------------------------------
// <copyright file="TypeDeclaration.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The builder for class
/// </summary>
/// <param name="Identifier">identifier</param>
/// <param name="Type">Type of the class</param>
public record struct TypeDeclaration(string Identifier, TypeDeclarationType Type) :
    IAccessibility,
    IUnsafe,
    IStatic,
    IPartial,
    IMember,
    IMemberOwner,
    IAttributes,
    IReadonly,
    IDescription,
    IParameters
{
    /// <inheritdoc />
    public string ToCode()
    {
        var builder = ZString.CreateStringBuilder();
        try
        {
            this.AddSummary(ref builder);
            this.AddParametersSummary(ref builder);
            this.AddAttributes(ref builder);
            this.AddAccessibility(ref builder);
            this.AddStatic(ref builder);
            this.AddUnsafe(ref builder);
            this.AddPartial(ref builder);
            builder.Append(Type switch
            {
                TypeDeclarationType.CLASS => "class ",
                TypeDeclarationType.STRUCT => "struct ",
                TypeDeclarationType.REF_STRUCT => "ref struct ",
                TypeDeclarationType.RECORD => "record ",
                TypeDeclarationType.RECORD_STRUCT => "record struct ",
                _ => throw new InvalidOperationException("The type is invalid."),
            });
            builder.Append(Identifier);
            this.AddParameters(ref builder);

            if (BaseTypes.Count > 0)
            {
                builder.Append(" :");
                var isNotStart = false;
                foreach (var memberAccess in BaseTypes)
                {
                    if (isNotStart)
                        builder.AppendLine(',');
                    else
                        builder.AppendLine();

                    builder.Append('\t');
                    builder.Append(memberAccess.ToCode());

                    isNotStart = true;
                }
            }

            if (Members.Count > 0)
            {
                builder.AppendLine();
                builder.AppendLine('{');

                foreach (var member in Members)
                {
                    builder.Append(member);
                    builder.AppendLine();
                    builder.AppendLine();
                }

                builder.Append('}');
            }

            return builder.ToString();
        }
        finally
        {
            builder.Dispose();
        }
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
    public List<Attribute> Attributes
        => field ??= [];

    /// <summary>
    /// The base Types
    /// </summary>
#pragma warning disable S2325
    public List<MemberAccess> BaseTypes
#pragma warning restore S2325
        => field ??= [];

    /// <inheritdoc />
    public List<string> Members
        => field ??= [];

    /// <inheritdoc />
    public bool IsReadonly { get; set; }

    /// <inheritdoc />
    public List<string> Description
        => field ??= [];

    /// <inheritdoc />
    public List<Parameter> Parameters
        => field ??= [];
}