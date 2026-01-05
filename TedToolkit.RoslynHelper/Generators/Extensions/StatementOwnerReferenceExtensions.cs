// -----------------------------------------------------------------------
// <copyright file="StatementOwnerReferenceExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="IStatementOwner"/>
/// </summary>
public static class StatementOwnerReferenceExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, IStatementOwner
#pragma warning restore CA1034
    {
        /// <summary>
        /// Add the statement
        /// </summary>
        /// <param name="statement">the statement</param>
        /// <typeparam name="TStatement">statement type</typeparam>
        /// <returns>the item</returns>
        public TItem AddStatement<TStatement>(TStatement statement)
            where TStatement : class, IStatement
        {
            instance.Statements.Add(statement);
            return instance;
        }

        /// <summary>
        /// Add the statement
        /// </summary>
        /// <param name="expression">the statement</param>
        /// <returns>the item</returns>
        public TItem AddStatement(IExpression expression)
        {
            instance.Statements.Add(new Statement(expression));
            return instance;
        }

        internal void AddStatements(ref SourceBuilder builder)
        {
            builder.BeginBlock();

            foreach (var statement in instance.Statements)
            {
                builder.AppendLine();
                statement.ToCode(ref builder);
            }

            builder.EndBlock();
        }
    }
}