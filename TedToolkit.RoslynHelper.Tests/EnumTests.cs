using TedToolkit.RoslynHelper.Generators;

namespace TedToolkit.RoslynHelper.Tests;
using static SourceComposer;
using static SourceComposer<EnumTests>;
internal class EnumTests
{
    [Test]
    public async Task EnumTest()
    {
        var code = File("File")
            .AddNameSpace(NameSpace("Space")
                .AddMember(Enum("MyEnum").Public
                    .AddEnumMember(EnumMember("CCC"))))
            .ToCode();

        await Assert.That(code).Contains("public enum MyEnum");
        await Assert.That(code).Contains("CCC,");
    }
}