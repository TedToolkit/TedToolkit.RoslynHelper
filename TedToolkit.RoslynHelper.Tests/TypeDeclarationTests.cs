using TedToolkit.RoslynHelper.Generators;

namespace TedToolkit.RoslynHelper.Tests;

internal class TypeDeclarationTests
{
    [Test]
    public async Task StaticClassTest()
    {
        var code = SourceComposer.File("File")
            .AddMember(SourceComposer.Class("FirstClass").Public.Static.Unsafe.Partial)
            .ToCode();

        await Assert.That(code).Contains("public static unsafe partial class FirstClass");
    }

    [Test]
    public async Task BaseTypeTest()
    {
        var code = SourceComposer.File("File")
            .AddMember(SourceComposer.Class("FirstClass").Public.AddBaseType<IDisposable>())
            .ToCode();

        await Assert.That(code).Contains("global::System.IDisposable");
    }

    [Test]
    public async Task ParametersTest()
    {
        var code = SourceComposer.File("File")
            .AddMember(SourceComposer.Class("FirstClass").Public
                .AddParameter(SourceComposer.Parameter<int>("item").AddDefault(10)))
            .ToCode();

        await Assert.That(code).Contains("int @item = 10");
    }

    [Test]
    public async Task SummaryTest()
    {
        var code = SourceComposer.File("File")
            .AddMember(SourceComposer.Class("FirstClass").Public
                .AddDescription("Good"))
            .ToCode();

        await Assert.That(code).Contains("/// <summary>");
        await Assert.That(code).Contains("/// Good");
        await Assert.That(code).Contains("/// </summary>");
    }

    [Test]
    public async Task ParameterSummaryTest()
    {
        var code = SourceComposer.File("File")
            .AddMember(SourceComposer.Class("FirstClass").Public
                .AddParameter(SourceComposer.Parameter<int>("item").AddDefault(10).AddDescription("Good")))
            .ToCode();

        await Assert.That(code).Contains("/// <param name=\"@item\">");
        await Assert.That(code).Contains("/// Good");
        await Assert.That(code).Contains("/// </param>");
    }

    [Test]
    public async Task MethodTest()
    {
        var code = SourceComposer.File("File")
            .AddMember(SourceComposer.Class("FirstClass").Public
                .AddMember(SourceComposer.Method("Method")));
    }
}