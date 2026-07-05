// -----------------------------------------------------------------------
// <copyright file="ConditionalCompilationBlock.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

/// <summary>
/// Shared renderer for conditional compilation blocks.
/// </summary>
/// <typeparam name="TItem">item type.</typeparam>
public abstract class ConditionalCompilationBlock<TItem> : IToCode
{
    private readonly ConditionalCompilationBranch _ifBranch;

    private List<ConditionalCompilationBranch>? _elseIfBranches;

    private ConditionalCompilationBranch? _elseBranch;

    private ConditionalCompilationBranch _currentBranch;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConditionalCompilationBlock{TItem}"/> class.
    /// </summary>
    /// <param name="condition">condition.</param>
    protected ConditionalCompilationBlock(PreprocessorExpression condition)
    {
        if (condition is null)
        {
            throw new ArgumentNullException(nameof(condition));
        }

        _ifBranch = new(condition);
        _currentBranch = _ifBranch;
    }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        WriteConditionalDirective(ref builder, "if", _ifBranch);

        if (_elseIfBranches is not null)
        {
            foreach (var branch in _elseIfBranches)
            {
                builder.AppendLine();
                WriteConditionalDirective(ref builder, "elif", branch);
            }
        }

        if (_elseBranch is not null)
        {
            builder.AppendLine();
            builder.Append("#else");
            builder.AppendLine();
            WriteItems(ref builder, _elseBranch.Items);
        }

        builder.AppendLine();
        builder.Append("#endif");
    }

    /// <summary>
    /// Gets the items in the current branch.
    /// </summary>
    protected List<TItem> CurrentItems
    {
        get
        {
            return _currentBranch.Items;
        }
    }

    /// <summary>
    /// Starts an <c>#elif</c> branch.
    /// </summary>
    /// <param name="condition">condition.</param>
    /// <exception cref="ArgumentNullException">condition is null.</exception>
    /// <exception cref="InvalidOperationException">else branch already exists.</exception>
    protected void StartElseIf(PreprocessorExpression condition)
    {
        if (condition is null)
        {
            throw new ArgumentNullException(nameof(condition));
        }

        if (_elseBranch is not null)
        {
            throw new InvalidOperationException("Cannot add an elif branch after an else branch.");
        }

        var branch = new ConditionalCompilationBranch(condition);
        _elseIfBranches ??= [];
        _elseIfBranches.Add(branch);
        _currentBranch = branch;
    }

    /// <summary>
    /// Starts an <c>#else</c> branch.
    /// </summary>
    /// <exception cref="InvalidOperationException">else branch already exists.</exception>
    protected void StartElse()
    {
        if (_elseBranch is not null)
        {
            throw new InvalidOperationException("Cannot add more than one else branch.");
        }

        _elseBranch = new();
        _currentBranch = _elseBranch;
    }

    private void WriteConditionalDirective(ref SourceBuilder builder, string directive, ConditionalCompilationBranch branch)
    {
        builder.Append('#');
        builder.Append(directive);
        builder.AppendSpace();
        branch.Condition!.ToCode(ref builder);
        builder.AppendLine();
        WriteItems(ref builder, branch.Items);
    }

    /// <summary>
    /// Writes items in a single branch.
    /// </summary>
    /// <param name="builder">builder.</param>
    /// <param name="items">items.</param>
    protected abstract void WriteItems(ref SourceBuilder builder, List<TItem> items);

    /// <summary>
    /// Gets all branches.
    /// </summary>
    protected IEnumerable<List<TItem>> AllBranches
    {
        get
        {
            yield return _ifBranch.Items;

            if (_elseIfBranches is not null)
            {
                foreach (var branch in _elseIfBranches)
                {
                    yield return branch.Items;
                }
            }

            if (_elseBranch is null)
            {
                yield break;
            }

            yield return _elseBranch.Items;
        }
    }

    private sealed class ConditionalCompilationBranch(PreprocessorExpression? condition = null)
    {
        public PreprocessorExpression? Condition { get; } = condition;

        public List<TItem> Items
        {
            get
            {
                return field ??= [];
            }
        }
    }
}