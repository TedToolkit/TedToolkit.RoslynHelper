// -----------------------------------------------------------------------
// <copyright file="ReturnType.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The Return Type
/// </summary>
/// <param name="Type">return Type</param>
public record struct ReturnType(MemberAccess Type)
    : IDescription
{
    /// <inheritdoc />
    public List<string> Description
        => field ??= [];
}