// -----------------------------------------------------------------------
// <copyright file="ParameterExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The parameter extensions
/// </summary>
public static class ParameterExtensions
{
#pragma warning disable CA1034
    extension(ref Parameter instance)
#pragma warning restore CA1034
#pragma warning disable S2325
    {
        /// <summary>
        /// Add default
        /// </summary>
        public ref Parameter AddDefault()
        {
            instance.Default = (ref builder) => builder.Append("default");
            return ref instance;
        }

        /// <summary>
        /// Add default
        /// </summary>
        /// <param name="value">defaultValue</param>
        public ref Parameter AddDefault(MemberAccess value)
        {
            instance.Default = value.ToCode;
            return ref instance;
        }

        /// <summary>
        /// Add default
        /// </summary>
        /// <param name="value">defaultValue</param>
        public ref Parameter AddDefault(string value)
        {
            instance.Default = (ref builder) =>
            {
                builder.Append('"');
                builder.Append(value);
                builder.Append('"');
            };
            return ref instance;
        }

        /// <summary>
        /// Add default
        /// </summary>
        /// <param name="value">defaultValue</param>
        public ref Parameter AddDefault(char value)
        {
            instance.Default = (ref builder) =>
            {
                builder.Append('\'');
                builder.Append(value);
                builder.Append('\'');
            };
            return ref instance;
        }

        /// <summary>
        /// Add default
        /// </summary>
        /// <param name="value">defaultValue</param>
        public ref Parameter AddDefault(int value)
        {
            instance.Default = (ref builder) => builder.Append(value.ToString(CultureInfo.InvariantCulture));
            return ref instance;
        }

        /// <summary>
        /// Add default
        /// </summary>
        /// <param name="value">defaultValue</param>
        public ref Parameter AddDefault(long value)
        {
            instance.Default = (ref builder) => builder.Append(value.ToString(CultureInfo.InvariantCulture));
            return ref instance;
        }

        /// <summary>
        /// Add default
        /// </summary>
        /// <param name="value">defaultValue</param>
        public ref Parameter AddDefault(uint value)
        {
            instance.Default = (ref builder) => builder.Append(value.ToString(CultureInfo.InvariantCulture));
            return ref instance;
        }

        /// <summary>
        /// Add default
        /// </summary>
        /// <param name="value">defaultValue</param>
        public ref Parameter AddDefault(ulong value)
        {
            instance.Default = (ref builder) => builder.Append(value.ToString(CultureInfo.InvariantCulture));
            return ref instance;
        }

        /// <summary>
        /// Add default
        /// </summary>
        /// <param name="value">defaultValue</param>
        public ref Parameter AddDefault(byte value)
        {
            instance.Default = (ref builder) => builder.Append(value.ToString(CultureInfo.InvariantCulture));
            return ref instance;
        }

        /// <summary>
        /// Add default
        /// </summary>
        /// <param name="value">defaultValue</param>
        public ref Parameter AddDefault(sbyte value)
        {
            instance.Default = (ref builder) => builder.Append(value.ToString(CultureInfo.InvariantCulture));
            return ref instance;
        }

        /// <summary>
        /// Add default
        /// </summary>
        /// <param name="value">defaultValue</param>
        public ref Parameter AddDefault(short value)
        {
            instance.Default = (ref builder) => builder.Append(value.ToString(CultureInfo.InvariantCulture));
            return ref instance;
        }

        /// <summary>
        /// Add default
        /// </summary>
        /// <param name="value">defaultValue</param>
        public ref Parameter AddDefault(ushort value)
        {
            instance.Default = (ref builder) => builder.Append(value.ToString(CultureInfo.InvariantCulture));
            return ref instance;
        }

        /// <summary>
        /// Add default
        /// </summary>
        /// <param name="value">defaultValue</param>
        public ref Parameter AddDefault(double value)
        {
            instance.Default = (ref builder) => builder.Append(value.ToString(CultureInfo.InvariantCulture));
            return ref instance;
        }

        /// <summary>
        /// Add default
        /// </summary>
        /// <param name="value">defaultValue</param>
        public ref Parameter AddDefault(float value)
        {
            instance.Default = (ref builder) => builder.Append(value.ToString(CultureInfo.InvariantCulture));
            return ref instance;
        }

        /// <summary>
        /// Add default
        /// </summary>
        /// <param name="value">defaultValue</param>
        public ref Parameter AddDefault(decimal value)
        {
            instance.Default = (ref builder) => builder.Append(value.ToString(CultureInfo.InvariantCulture));
            return ref instance;
        }
    }
#pragma warning restore S2325
}