// -----------------------------------------------------------------------
// <copyright file="NameSpace.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

namespace TedToolkit.RoslynHelper.Syntaxes;

/// <summary>
/// <see langword="namespace"/>.
/// </summary>
/// <param name="name">the name of the <see langword="namespace"/>.</param>
public sealed class NameSpace(IExpression name) :
    IMemberOwner,
    IToCode
{
    /// <inheritdoc />
    public List<IMember> Members
    {
        get
        {
            return field ??= [];
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NameSpace"/> class.
    /// Create the namespace based on strings.
    /// </summary>
    /// <param name="nameSpace">strings.</param>
    public NameSpace(in ReadOnlySpan<string> nameSpace)
        : this(ZString.Join('.', nameSpace))
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NameSpace"/> class.
    /// Create the namespace based on string.
    /// </summary>
    /// <param name="nameSpace">string.</param>
    public NameSpace(string nameSpace)
        : this(nameSpace?.ToSimpleName()
               ?? throw new ArgumentNullException(nameof(nameSpace)))
    {
    }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        ToCode(ref builder, forceBlock: false);
    }

    /// <summary>
    /// Writes a namespace, requiring a block when other file-level declarations coexist.
    /// </summary>
    /// <param name="builder">The source builder.</param>
    /// <param name="forceBlock">Whether an empty namespace must retain a block.</param>
    internal void ToCode(ref SourceBuilder builder, bool forceBlock)
    {
        builder.Append("namespace ");
        name.ToCode(ref builder);

        if (forceBlock)
        {
            this.AddMembersNoSkip(ref builder);
        }
        else
        {
            this.AddMembers(ref builder);
        }
    }
}