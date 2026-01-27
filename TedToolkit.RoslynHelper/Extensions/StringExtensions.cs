// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Text.RegularExpressions;

namespace TedToolkit.RoslynHelper.Extensions;

/// <summary>
///     For the string extensions.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     Make a string to pascal case.
    /// </summary>
    /// <param name="input">input.</param>
    /// <returns>Pascal.</returns>
    /// <exception cref="ArgumentNullException">input is null.</exception>
    public static string ToPascalCase(this string input)
    {
        if (input is null)
            throw new ArgumentNullException(nameof(input));

        return string.Join(".", input.Split('.').Select(ConvertToPascalCase));

        static string ConvertToPascalCase(string input)
        {
            var invalidCharsRgx = new Regex("[^_a-zA-Z0-9]");
            var whiteSpace = new Regex(@"(?<=\s)");
            var startsWithLowerCaseChar = new Regex("^[a-z]");
            var firstCharFollowedByUpperCasesOnly = new Regex("(?<=[A-Z])[A-Z0-9]+$");
            var lowerCaseNextToNumber = new Regex("(?<=[0-9])[a-z]");
            var upperCaseInside = new Regex("(?<=[A-Z])[A-Z]+?((?=[A-Z][a-z])|(?=[0-9]))");

            var pascalCase = invalidCharsRgx.Replace(whiteSpace.Replace(input, "_"), "")
                .Split(['_',], StringSplitOptions.RemoveEmptyEntries)
                .Select(w => startsWithLowerCaseChar.Replace(w, m => m.Value.ToUpperInvariant()))
#pragma warning disable CA1308
                .Select(w => firstCharFollowedByUpperCasesOnly.Replace(w, m => m.Value.ToLowerInvariant()))
                .Select(w => lowerCaseNextToNumber.Replace(w, m => m.Value.ToUpperInvariant()))
                .Select(w => upperCaseInside.Replace(w, m => m.Value.ToLowerInvariant()));
#pragma warning restore CA1308

            return string.Concat(pascalCase);
        }
    }

    /// <summary>
    ///     Add leading string.
    /// </summary>
    /// <param name="input">Input.</param>
    /// <param name="leading">Leading.</param>
    /// <returns>result.</returns>
    /// <exception cref="ArgumentNullException">input or leading is null.</exception>
    public static string Leading(this string input, string leading)
    {
        if (input is null)
            throw new ArgumentNullException(nameof(input));

        if (leading is null)
            throw new ArgumentNullException(nameof(leading));

        return leading + input.Replace("\n", "\n" + leading);
    }
}