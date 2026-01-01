using System.Diagnostics;

using TedToolkit.RoslynHelper.Generators;

namespace TedToolkit.RoslynHelper.Tests;

internal class ClassGeneratorTests
{
    [Test]
    public async Task ClassGenerateTest()
    {
        var instance = SourceComposer.Class("FirstClass").Public.Static.Unsafe.Partial;
        var code = instance.ToCode();

        if (!string.IsNullOrEmpty(code))
        {

        }
    }
}