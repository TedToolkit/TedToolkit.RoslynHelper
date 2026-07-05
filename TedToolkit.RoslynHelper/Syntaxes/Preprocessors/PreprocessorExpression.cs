// -----------------------------------------------------------------------
// <copyright file="PreprocessorExpression.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Cysharp.Text;

using Microsoft.CodeAnalysis.CSharp;

namespace TedToolkit.RoslynHelper.Syntaxes.Preprocessors;

/// <summary>
/// Base type for conditional compilation expressions.
/// </summary>
public abstract class PreprocessorExpression : IToCode
{
    /// <summary>
    /// Gets the predefined <c>DEBUG</c> symbol.
    /// </summary>
    public static PreprocessorExpression Debug { get; } = Symbol("DEBUG");

    /// <summary>
    /// Gets the predefined <c>TRACE</c> symbol.
    /// </summary>
    public static PreprocessorExpression Trace { get; } = Symbol("TRACE");

    /// <summary>
    /// Gets a literal <see langword="true"/> expression.
    /// </summary>
    public static PreprocessorExpression True { get; } = Symbol("true");

    /// <summary>
    /// Gets a literal <see langword="false"/> expression.
    /// </summary>
    public static PreprocessorExpression False { get; } = Symbol("false");

    /// <summary>
    /// Gets the predefined <c>NETFRAMEWORK</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetFramework { get; } = Symbol("NETFRAMEWORK");

    /// <summary>
    /// Gets the predefined <c>NETSTANDARD</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetStandard { get; } = Symbol("NETSTANDARD");

    /// <summary>
    /// Gets the predefined <c>NETCOREAPP</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetCoreApp { get; } = Symbol("NETCOREAPP");

    /// <summary>
    /// Gets the predefined <c>NET</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net { get; } = Symbol("NET");

    /// <summary>
    /// Gets the predefined <c>NET481</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net481 { get; } = Symbol("NET481");

    /// <summary>
    /// Gets the predefined <c>NET48</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net48 { get; } = Symbol("NET48");

    /// <summary>
    /// Gets the predefined <c>NET472</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net472 { get; } = Symbol("NET472");

    /// <summary>
    /// Gets the predefined <c>NET471</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net471 { get; } = Symbol("NET471");

    /// <summary>
    /// Gets the predefined <c>NET47</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net47 { get; } = Symbol("NET47");

    /// <summary>
    /// Gets the predefined <c>NET462</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net462 { get; } = Symbol("NET462");

    /// <summary>
    /// Gets the predefined <c>NET461</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net461 { get; } = Symbol("NET461");

    /// <summary>
    /// Gets the predefined <c>NET46</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net46 { get; } = Symbol("NET46");

    /// <summary>
    /// Gets the predefined <c>NET452</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net452 { get; } = Symbol("NET452");

    /// <summary>
    /// Gets the predefined <c>NET451</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net451 { get; } = Symbol("NET451");

    /// <summary>
    /// Gets the predefined <c>NET45</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net45 { get; } = Symbol("NET45");

    /// <summary>
    /// Gets the predefined <c>NET40</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net40 { get; } = Symbol("NET40");

    /// <summary>
    /// Gets the predefined <c>NET35</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net35 { get; } = Symbol("NET35");

    /// <summary>
    /// Gets the predefined <c>NET20</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net20 { get; } = Symbol("NET20");

    /// <summary>
    /// Gets the predefined <c>NET48_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net48OrGreater { get; } = Symbol("NET48_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NET472_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net472OrGreater { get; } = Symbol("NET472_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NET471_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net471OrGreater { get; } = Symbol("NET471_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NET47_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net47OrGreater { get; } = Symbol("NET47_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NET462_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net462OrGreater { get; } = Symbol("NET462_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NET461_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net461OrGreater { get; } = Symbol("NET461_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NET46_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net46OrGreater { get; } = Symbol("NET46_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NET452_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net452OrGreater { get; } = Symbol("NET452_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NET451_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net451OrGreater { get; } = Symbol("NET451_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NET45_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net45OrGreater { get; } = Symbol("NET45_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NET40_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net40OrGreater { get; } = Symbol("NET40_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NET35_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net35OrGreater { get; } = Symbol("NET35_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NET20_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net20OrGreater { get; } = Symbol("NET20_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NETSTANDARD2_1</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetStandard21 { get; } = Symbol("NETSTANDARD2_1");

    /// <summary>
    /// Gets the predefined <c>NETSTANDARD2_0</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetStandard20 { get; } = Symbol("NETSTANDARD2_0");

    /// <summary>
    /// Gets the predefined <c>NETSTANDARD1_6</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetStandard16 { get; } = Symbol("NETSTANDARD1_6");

    /// <summary>
    /// Gets the predefined <c>NETSTANDARD1_5</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetStandard15 { get; } = Symbol("NETSTANDARD1_5");

    /// <summary>
    /// Gets the predefined <c>NETSTANDARD1_4</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetStandard14 { get; } = Symbol("NETSTANDARD1_4");

    /// <summary>
    /// Gets the predefined <c>NETSTANDARD1_3</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetStandard13 { get; } = Symbol("NETSTANDARD1_3");

    /// <summary>
    /// Gets the predefined <c>NETSTANDARD1_2</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetStandard12 { get; } = Symbol("NETSTANDARD1_2");

    /// <summary>
    /// Gets the predefined <c>NETSTANDARD1_1</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetStandard11 { get; } = Symbol("NETSTANDARD1_1");

    /// <summary>
    /// Gets the predefined <c>NETSTANDARD1_0</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetStandard10 { get; } = Symbol("NETSTANDARD1_0");

    /// <summary>
    /// Gets the predefined <c>NETSTANDARD2_1_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetStandard21OrGreater { get; } = Symbol("NETSTANDARD2_1_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NETSTANDARD2_0_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetStandard20OrGreater { get; } = Symbol("NETSTANDARD2_0_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NETSTANDARD1_6_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetStandard16OrGreater { get; } = Symbol("NETSTANDARD1_6_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NETSTANDARD1_5_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetStandard15OrGreater { get; } = Symbol("NETSTANDARD1_5_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NETSTANDARD1_4_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetStandard14OrGreater { get; } = Symbol("NETSTANDARD1_4_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NETSTANDARD1_3_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetStandard13OrGreater { get; } = Symbol("NETSTANDARD1_3_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NETSTANDARD1_2_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetStandard12OrGreater { get; } = Symbol("NETSTANDARD1_2_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NETSTANDARD1_1_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetStandard11OrGreater { get; } = Symbol("NETSTANDARD1_1_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NETSTANDARD1_0_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetStandard10OrGreater { get; } = Symbol("NETSTANDARD1_0_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NET10_0</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net100 { get; } = Symbol("NET10_0");

    /// <summary>
    /// Gets the predefined <c>NET9_0</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net90 { get; } = Symbol("NET9_0");

    /// <summary>
    /// Gets the predefined <c>NET8_0</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net80 { get; } = Symbol("NET8_0");

    /// <summary>
    /// Gets the predefined <c>NET7_0</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net70 { get; } = Symbol("NET7_0");

    /// <summary>
    /// Gets the predefined <c>NET6_0</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net60 { get; } = Symbol("NET6_0");

    /// <summary>
    /// Gets the predefined <c>NET5_0</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net50 { get; } = Symbol("NET5_0");

    /// <summary>
    /// Gets the predefined <c>NETCOREAPP3_1</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetCoreApp31 { get; } = Symbol("NETCOREAPP3_1");

    /// <summary>
    /// Gets the predefined <c>NETCOREAPP3_0</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetCoreApp30 { get; } = Symbol("NETCOREAPP3_0");

    /// <summary>
    /// Gets the predefined <c>NETCOREAPP2_2</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetCoreApp22 { get; } = Symbol("NETCOREAPP2_2");

    /// <summary>
    /// Gets the predefined <c>NETCOREAPP2_1</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetCoreApp21 { get; } = Symbol("NETCOREAPP2_1");

    /// <summary>
    /// Gets the predefined <c>NETCOREAPP2_0</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetCoreApp20 { get; } = Symbol("NETCOREAPP2_0");

    /// <summary>
    /// Gets the predefined <c>NETCOREAPP1_1</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetCoreApp11 { get; } = Symbol("NETCOREAPP1_1");

    /// <summary>
    /// Gets the predefined <c>NETCOREAPP1_0</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetCoreApp10 { get; } = Symbol("NETCOREAPP1_0");

    /// <summary>
    /// Gets the predefined <c>NET10_0_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net100OrGreater { get; } = Symbol("NET10_0_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NET9_0_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net90OrGreater { get; } = Symbol("NET9_0_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NET8_0_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net80OrGreater { get; } = Symbol("NET8_0_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NET7_0_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net70OrGreater { get; } = Symbol("NET7_0_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NET6_0_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net60OrGreater { get; } = Symbol("NET6_0_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NET5_0_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Net50OrGreater { get; } = Symbol("NET5_0_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NETCOREAPP3_1_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetCoreApp31OrGreater { get; } = Symbol("NETCOREAPP3_1_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NETCOREAPP3_0_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetCoreApp30OrGreater { get; } = Symbol("NETCOREAPP3_0_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NETCOREAPP2_2_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetCoreApp22OrGreater { get; } = Symbol("NETCOREAPP2_2_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NETCOREAPP2_1_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetCoreApp21OrGreater { get; } = Symbol("NETCOREAPP2_1_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NETCOREAPP2_0_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetCoreApp20OrGreater { get; } = Symbol("NETCOREAPP2_0_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NETCOREAPP1_1_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetCoreApp11OrGreater { get; } = Symbol("NETCOREAPP1_1_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>NETCOREAPP1_0_OR_GREATER</c> symbol.
    /// </summary>
    public static PreprocessorExpression NetCoreApp10OrGreater { get; } = Symbol("NETCOREAPP1_0_OR_GREATER");

    /// <summary>
    /// Gets the predefined <c>ANDROID</c> symbol.
    /// </summary>
    public static PreprocessorExpression Android { get; } = Symbol("ANDROID");

    /// <summary>
    /// Gets the predefined <c>BROWSER</c> symbol.
    /// </summary>
    public static PreprocessorExpression Browser { get; } = Symbol("BROWSER");

    /// <summary>
    /// Gets the predefined <c>IOS</c> symbol.
    /// </summary>
    public static PreprocessorExpression Ios { get; } = Symbol("IOS");

    /// <summary>
    /// Gets the predefined <c>MACCATALYST</c> symbol.
    /// </summary>
    public static PreprocessorExpression MacCatalyst { get; } = Symbol("MACCATALYST");

    /// <summary>
    /// Gets the predefined <c>MACOS</c> symbol.
    /// </summary>
    public static PreprocessorExpression MacOs { get; } = Symbol("MACOS");

    /// <summary>
    /// Gets the predefined <c>TVOS</c> symbol.
    /// </summary>
    public static PreprocessorExpression TvOs { get; } = Symbol("TVOS");

    /// <summary>
    /// Gets the predefined <c>WINDOWS</c> symbol.
    /// </summary>
    public static PreprocessorExpression Windows { get; } = Symbol("WINDOWS");

    /// <summary>
    /// Creates a preprocessor symbol expression.
    /// </summary>
    /// <param name="symbol">symbol name.</param>
    /// <returns>result.</returns>
    /// <exception cref="ArgumentException">The symbol is not a valid preprocessor identifier.</exception>
    public static PreprocessorExpression Symbol(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            throw new ArgumentException("Preprocessor symbol cannot be null, empty, or whitespace.", nameof(symbol));
        }

        if (!SyntaxFacts.IsValidIdentifier(symbol))
        {
            throw new ArgumentException($"'{symbol}' is not a valid preprocessor symbol.", nameof(symbol));
        }

        return new PreprocessorSymbolExpression(symbol);
    }

    /// <summary>
    /// Creates an OS version symbol such as <c>IOS15_1</c>.
    /// </summary>
    /// <param name="platform">platform token, for example <c>IOS</c>.</param>
    /// <param name="version">version token, for example <c>15.1</c>.</param>
    /// <returns>result.</returns>
    public static PreprocessorExpression PlatformVersion(string platform, string version)
    {
        return Symbol(BuildPlatformVersionSymbol(platform, version, false));
    }

    /// <summary>
    /// Creates an OS version symbol such as <c>IOS15_1_OR_GREATER</c>.
    /// </summary>
    /// <param name="platform">platform token, for example <c>IOS</c>.</param>
    /// <param name="version">version token, for example <c>15.1</c>.</param>
    /// <returns>result.</returns>
    public static PreprocessorExpression PlatformVersionOrGreater(string platform, string version)
    {
        return Symbol(BuildPlatformVersionSymbol(platform, version, true));
    }

    /// <inheritdoc />
    public void ToCode(ref SourceBuilder builder)
    {
        WriteTo(ref builder);
    }

#pragma warning disable CA2225
    /// <summary>
    /// Logical not.
    /// </summary>
    /// <param name="value">operand.</param>
    /// <returns>result.</returns>
    public static PreprocessorExpression operator !(PreprocessorExpression value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return new PreprocessorUnaryExpression(value);
    }

    /// <summary>
    /// Logical and. Used by C# to support <c>&amp;&amp;</c>.
    /// </summary>
    /// <param name="left">left operand.</param>
    /// <param name="right">right operand.</param>
    /// <returns>result.</returns>
    public static PreprocessorExpression operator &(PreprocessorExpression left, PreprocessorExpression right)
    {
        if (left is null)
        {
            throw new ArgumentNullException(nameof(left));
        }

        if (right is null)
        {
            throw new ArgumentNullException(nameof(right));
        }

        return new PreprocessorAndExpression(left, right);
    }

    /// <summary>
    /// Logical or. Used by C# to support <c>||</c>.
    /// </summary>
    /// <param name="left">left operand.</param>
    /// <param name="right">right operand.</param>
    /// <returns>result.</returns>
    public static PreprocessorExpression operator |(PreprocessorExpression left, PreprocessorExpression right)
    {
        if (left is null)
        {
            throw new ArgumentNullException(nameof(left));
        }

        if (right is null)
        {
            throw new ArgumentNullException(nameof(right));
        }

        return new PreprocessorOrExpression(left, right);
    }
#pragma warning restore CA2225

    /// <summary>
    /// Renders a source-code string for the expression.
    /// </summary>
    /// <returns>result.</returns>
    public override string ToString()
    {
        return this.ToCode();
    }

    /// <summary>
    /// Writes this expression to source code.
    /// </summary>
    /// <param name="builder">builder.</param>
    internal abstract void WriteTo(ref SourceBuilder builder);

    private static string BuildPlatformVersionSymbol(string platform, string version, bool orGreater)
    {
        if (string.IsNullOrWhiteSpace(platform))
        {
            throw new ArgumentException("Platform cannot be null, empty, or whitespace.", nameof(platform));
        }

        if (string.IsNullOrWhiteSpace(version))
        {
            throw new ArgumentException("Version cannot be null, empty, or whitespace.", nameof(version));
        }

        var normalizedVersion = version.Replace('.', '_');
        var symbol = ZString.Concat(platform.ToUpperInvariant(), normalizedVersion,
            orGreater ? "_OR_GREATER" : "");

        if (!SyntaxFacts.IsValidIdentifier(symbol))
        {
            throw new ArgumentException($"'{symbol}' is not a valid preprocessor symbol.", nameof(version));
        }

        return symbol;
    }
}