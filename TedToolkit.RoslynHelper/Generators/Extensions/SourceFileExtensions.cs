using System.Runtime.CompilerServices;

using TedToolkit.RoslynHelper.Generators.Types;

namespace TedToolkit.RoslynHelper.Generators;

/// <summary>
/// The extensions for the <see cref="SourceFile"/>
/// </summary>
public static class SourceFileExtensions
{
#pragma warning disable CA1034
    extension(ref SourceFile instance)
#pragma warning restore CA1034
    {
        /// <summary>
        /// Add a name space.
        /// </summary>
        /// <param name="nameSpace">the namespace</param>
        /// <returns>result</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref SourceFile AddNameSpace(NameSpace nameSpace)
        {
            instance.NameSpaces.Add(nameSpace);
            return ref instance;
        }
    }
}