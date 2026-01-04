using TedToolkit.RoslynHelper.Generators;

namespace TedToolkit.RoslynHelper.Tests;

using static SourceComposer;
using static SourceComposer<TypeDeclarationTests>;
internal class TypeDeclarationTests
{
    [Test]
    public async Task StaticClassTest()
    {
        var code = File("File", "Space")
            .AddMember(Class("FirstClass").Public.Static.Unsafe.Partial)
            .ToCode();

        await Assert.That(code).Contains("public static unsafe partial class FirstClass");
    }

    [Test]
    public async Task BaseTypeTest()
    {
        var code = File("File", "Space")
            .AddMember(Class("FirstClass").Public.AddBaseType<IDisposable>())
            .ToCode();

        await Assert.That(code).Contains("global::System.IDisposable");
    }

    [Test]
    public async Task ParametersTest()
    {
        var code = File("File", "Space")
            .AddMember(Class("FirstClass").Public
                .AddParameter(Parameter<int>("item").ScopedIn.AddDefault(Argument(10))))
            .ToCode();

        await Assert.That(code).Contains("scoped in int @item = 10");
    }

    [Test]
    public async Task SummaryTest()
    {
        var code = File("File", "Space")
            .AddMember(Class("FirstClass").Public
                .AddDescription("Good"))
            .ToCode();

        await Assert.That(code).Contains("/// <summary>");
        await Assert.That(code).Contains("/// Good");
        await Assert.That(code).Contains("/// </summary>");
    }

    [Test]
    public async Task ParameterSummaryTest()
    {
        var code = File("File", "Space")
            .AddMember(Class("FirstClass").Public
                .AddParameter(Parameter<int>("item").AddDefault(Argument(10)).AddDescription("Good")))
            .ToCode();

        await Assert.That(code).Contains("/// <param name=\"@item\">");
        await Assert.That(code).Contains("/// Good");
        await Assert.That(code).Contains("/// </param>");
    }

    [Test]
    public async Task MethodTest()
    {
        var code = File("File", "Space")
            .AddMember(Class("FirstClass").Public
                .AddMember(Method("Method")
                    .AddParameter(Parameter<int>("item").AddDefault(Argument(10)))))
            .ToCode();

        await Assert.That(code).Contains("void Method(");
    }

    [Test]
    public async Task PropertyTest()
    {
        var code = File("File", "Space")
            .AddMember(Class("FirstClass").Public
                .AddMember(Property("Item", SourceComposer.Type<long>()).Internal
                    .AddAccessor(Accessor(AccessorType.GET))))
            .ToCode();

        await Assert.That(code).Contains("internal long Item");
        await Assert.That(code).Contains("get");
    }
}