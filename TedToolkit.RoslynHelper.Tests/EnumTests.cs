using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Tests;
using static SourceComposer;
using static SourceComposer<EnumTests>;
internal class EnumTests
{
    [Test]
    public async Task EnumTest()
    {
        var table = new DescriptionTable(new DescriptionText("Name"), new DescriptionText("Description"))
            .AddItem(new DescriptionText("Item1"), new DescriptionText("Description 1"));
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Enum("MyEnum").Public
                    .AddRootDescription(new DescriptionSummary(table))
                    .AddEnumMember(EnumMember("CCC"))))
            .ToCode();

        await Assert.That(code).Contains("public enum MyEnum");
        await Assert.That(code).Contains("CCC,");
    }
}