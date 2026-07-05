// -----------------------------------------------------------------------
// <copyright file="ConditionalCompilationMember.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

/// <summary>
/// A conditional compilation block that renders members.
/// </summary>
/// <param name="condition">condition.</param>
public sealed class ConditionalCompilationMember(PreprocessorExpression condition) :
    ConditionalCompilationBlock<IMember>(condition),
    IMember,
    IOwner
{
    private string _owner = "";

    /// <inheritdoc />
    public string Owner
    {
        get
        {
            return _owner;
        }

        set
        {
            _owner = value;
            this.ApplyOwner();
        }
    }

    /// <summary>
    /// Adds a member to the current branch.
    /// </summary>
    /// <param name="member">member.</param>
    /// <typeparam name="TMember">member type.</typeparam>
    /// <returns>this.</returns>
    public ConditionalCompilationMember AddMember<TMember>(TMember member)
        where TMember : class, IMember
    {
        this.ApplyOwner(member);
        CurrentItems.Add(member);
        return this;
    }

    /// <summary>
    /// Starts an <c>#elif</c> branch.
    /// </summary>
    /// <param name="condition">condition.</param>
    /// <returns>this.</returns>
    public ConditionalCompilationMember ElseIf(PreprocessorExpression condition)
    {
        StartElseIf(condition);
        return this;
    }

    /// <summary>
    /// Starts an <c>#else</c> branch.
    /// </summary>
    /// <returns>this.</returns>
    public ConditionalCompilationMember Else()
    {
        StartElse();
        return this;
    }

    /// <summary>
    /// Gets the members in the current branch.
    /// </summary>
    public List<IMember> Members
    {
        get
        {
            return CurrentItems;
        }
    }

    /// <inheritdoc />
    protected override void WriteItems(ref SourceBuilder builder, List<IMember> items)
    {
        var isNotStart = false;
        foreach (var item in items)
        {
            if (isNotStart)
            {
                builder.AppendLine();
                builder.AppendLine();
            }

            item.ToCode(ref builder);
            isNotStart = true;
        }
    }

    private void ApplyOwner(IMember member)
    {
        if (member is not IOwner memberOwner || string.IsNullOrWhiteSpace(Owner))
        {
            return;
        }

        memberOwner.Owner = Owner;
    }

    private void ApplyOwner()
    {
        foreach (var branch in AllBranches)
        {
            foreach (var member in branch)
            {
                this.ApplyOwner(member);
            }
        }
    }
}