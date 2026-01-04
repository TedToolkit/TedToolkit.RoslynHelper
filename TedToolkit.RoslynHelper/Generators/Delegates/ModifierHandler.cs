// -----------------------------------------------------------------------
// <copyright file="ModifierHandler.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators.Delegates;

/// <summary>
/// Modify the item
/// </summary>
/// <typeparam name="T">argument</typeparam>
/// <param name="arg">the argument</param>
public delegate void ModifierHandler<T>(ref T arg);