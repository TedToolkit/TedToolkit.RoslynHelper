// -----------------------------------------------------------------------
// <copyright file="ExpressionOwnerExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper;

/// <summary>
/// The extensions for the <see cref="IExpressionOwner"/>.
/// </summary>
public static class ExpressionOwnerExtensions
{
#pragma warning disable CA1034
    extension<TItem>(TItem instance)
        where TItem : class, IExpressionOwner
#pragma warning restore CA1034
    {
        /// <summary>
        /// Add the expression.
        /// </summary>
        /// <param name="expression">the expression.</param>
        /// <returns>the item.</returns>
        public TItem AddExpression(IExpression expression)
        {
            instance.Expressions.Add(expression);
            return instance;
        }
    }
}