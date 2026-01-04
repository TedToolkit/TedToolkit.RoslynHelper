using TedToolkit.RoslynHelper.Generators;

namespace TedToolkit.RoslynHelper.Tests;

internal class TypeDeclarationTests
{
    [Test]
    public async Task StaticClassTest()
    {
        var instance = SourceComposer.Class("FirstClass").Public.Static.Unsafe.Partial;
        var code = instance.ToCode();

        await Assert.That(code).EqualTo("public static unsafe partial class FirstClass");
    }

    [Test]
    public async Task BaseTypeTest()
    {
        var instance = SourceComposer.Class("FirstClass").Public
            .AddBaseType<IDisposable>();
        var code = instance.ToCode();

        await Assert.That(code).EndsWith("global::System.IDisposable");
    }

    [Test]
    public async Task ParametersTest()
    {
        var instance = SourceComposer.Class("FirstClass").Public
            .AddParameter(SourceComposer.Parameter<int>("item").AddDefault(10));
        var code = instance.ToCode();

        await Assert.That(code).Contains("int @item = 10");
    }

    [Test]
    public async Task SummaryTest()
    {
        var instance = SourceComposer.Class("FirstClass").Public
            .AddDescription("Good");
        var code = instance.ToCode();

        await Assert.That(code).Contains("int @item = 10");
    }
}