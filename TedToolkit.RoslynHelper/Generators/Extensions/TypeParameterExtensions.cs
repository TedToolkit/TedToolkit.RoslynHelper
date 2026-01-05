// -----------------------------------------------------------------------
// <copyright file="TypeParameterExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The type parameter extensions
/// </summary>
public static class TypeParameterExtensions
{
#pragma warning disable CA1034
    extension(ref TypeParameter instance)
#pragma warning restore CA1034
#pragma warning disable S2325
    {
        /// <summary>
        /// Add constraint
        /// </summary>
        /// <param name="constraint">the constraint</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref TypeParameter AddConstraint(IExpression constraint)
        {
            instance.Constraints.Add(constraint);
            return ref instance;
        }

        /// <summary>
        /// Add constraint
        /// </summary>
        /// <param name="constraint">the constraint</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref TypeParameter AddConstraint(scoped in DataType constraint)
        {
            instance.Constraints.Add(constraint.Type);
            return ref instance;
        }

        /// <summary>
        /// Add constraint
        /// </summary>
        /// <param name="constraint">the constraint</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref TypeParameter AddConstraint(Type constraint)
        {
            instance.Constraints.Add(DataType.FromType(constraint).Type);
            return ref instance;
        }

        /// <summary>
        /// Add constraint
        /// </summary>
        /// <typeparam name="T">constraint type</typeparam>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref TypeParameter AddConstraint<T>()
        {
            instance.Constraints.Add(DataType.FromType<T>().Type);
            return ref instance;
        }

        /// <summary>
        /// Add struct constraint
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref TypeParameter AddStructConstraint()
        {
            instance.Constraints.Add(new SimpleNameExpression("struct"));
            return ref instance;
        }

        /// <summary>
        /// Add class constraint
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref TypeParameter AddClassConstraint()
        {
            instance.Constraints.Add(new SimpleNameExpression("class"));
            return ref instance;
        }

        /// <summary>
        /// Add class null constraint
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref TypeParameter AddClassNullConstraint()
        {
            instance.Constraints.Add(new SimpleNameExpression("class?"));
            return ref instance;
        }

        /// <summary>
        /// Add not null constraint
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref TypeParameter AddNotNullConstraint()
        {
            instance.Constraints.Add(new SimpleNameExpression("notnull"));
            return ref instance;
        }

        /// <summary>
        /// Add new constraint
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref TypeParameter AddNewConstraint()
        {
            instance.Constraints.Add(new SimpleNameExpression("new()"));
            return ref instance;
        }

        /// <summary>
        /// Add unmanaged constraint
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref TypeParameter AddUnmanagedConstraint()
        {
            instance.Constraints.Add(new SimpleNameExpression("unmanaged"));
            return ref instance;
        }

        /// <summary>
        /// Add allows ref struct constraint
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref TypeParameter AddRefStructConstraint()
        {
            instance.Constraints.Add(new SimpleNameExpression("allows ref struct"));
            return ref instance;
        }
    }
#pragma warning restore S2325
}