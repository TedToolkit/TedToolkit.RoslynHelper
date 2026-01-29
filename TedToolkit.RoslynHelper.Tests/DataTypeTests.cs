using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Tests;

using static SourceComposer;
using static SourceComposer<DataTypeTests>;

internal class DataTypeTests
{
    [Test]
    public async Task GenericNonParameterTypeTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public.AddBaseType(typeof(List<>))))
            .ToCode();

        await Assert.That(code).Contains("System.Collections.Generic.List");
    }

    [Test]
    public async Task GenericTypeTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public.AddBaseType(DataType.FromType(typeof(List<>))
                    .Generic(DataType.FromType<int>()))))
            .ToCode();

        await Assert.That(code).Contains("System.Collections.Generic.List<int>");
    }

    [Test]
    public async Task GenericTypeGlobalTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public.AddBaseType(DataType.FromType(typeof(List<>), "global")
                    .Generic(DataType.FromType<int>()))))
            .ToCode();

        await Assert.That(code).Contains("global::System.Collections.Generic.List<int>");
    }

    [Test]
    public async Task GenericBaseTypeTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public.AddBaseType<List<int>>()))
            .ToCode();

        await Assert.That(code).Contains("System.Collections.Generic.List<int>");
    }
}