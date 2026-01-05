using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;
using TedToolkit.RoslynHelper.Generators.Syntaxes.Statements;

namespace TedToolkit.RoslynHelper.Tests;

using static SourceComposer;
using static SourceComposer<TypeDeclarationTests>;

internal class TypeDeclarationTests
{
    [Test]
    public async Task StaticClassTest()
    {
        var code = File("File")
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public.Static.Unsafe.Partial))
            .ToCode();

        await Assert.That(code).Contains("public static unsafe partial class FirstClass");
    }

    [Test]
    public async Task BaseTypeTest()
    {
        var code = File("File")
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public.AddBaseType<IDisposable>()))
            .ToCode();

        await Assert.That(code).Contains("System.IDisposable");
    }

    [Test]
    public async Task ParametersTest()
    {
        var code = File("File")
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddParameter(Parameter<int>("item").ScopedIn.AddDefault(Argument(10.ToLiteral())))))
            .ToCode();

        await Assert.That(code).Contains("scoped in int @item = 10");
    }

    [Test]
    public async Task SummaryTest()
    {
        var code = File("File")
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddDescription("Good")))
            .ToCode();

        await Assert.That(code).Contains("/// <summary>");
        await Assert.That(code).Contains("/// Good");
        await Assert.That(code).Contains("/// </summary>");
    }

    [Test]
    public async Task ParameterSummaryTest()
    {
        var code = File("File")
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddParameter(Parameter<int>("item").AddDefault(Argument(10.ToLiteral())).AddDescription("Good"))))
            .ToCode();

        await Assert.That(code).Contains("/// <param name=\"@item\">");
        await Assert.That(code).Contains("/// Good");
        await Assert.That(code).Contains("/// </param>");
    }

    [Test]
    public async Task MethodTest()
    {
        var code = File("File")
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddMember(Method("Method")
                        .AddParameter(Parameter<int>("item").AddDefault(Argument(10.ToLiteral()))))))
            .ToCode();

        await Assert.That(code).Contains("void Method(");
    }

    [Test]
    public async Task PropertyTest()
    {
        var code = File("File")
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddMember(Property<long>("Item").Internal
                        .AddAccessor(Accessor(AccessorType.GET)))))
            .ToCode();

        await Assert.That(code).Contains("internal long Item");
        await Assert.That(code).Contains("get");
    }

    [Test]
    public async Task FieldTest()
    {
        var code = File("File")
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddMember(Field<long>("Item").Internal.Readonly)))
            .ToCode();

        await Assert.That(code).Contains("internal readonly long Item;");
    }

    [Test]
    public async Task EventTest()
    {
        var code = File("File")
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddMember(Event<Action<int>>("Item").Internal)))
            .ToCode();

        await Assert.That(code).Contains("internal event System.Action<int> Item;");
    }

    [Test]
    public async Task DelegateTest()
    {
        var code = File("File")
            .AddNameSpace(NameSpace("Space")
                .AddMember(Delegate("ADelegate").Public))
            .ToCode();

        await Assert.That(code).Contains("public delegate void ADelegate();");
    }

    [Test]
    public async Task ForeachTest()
    {
        var code = File("File")
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddMember(Method("Method")
                        .AddParameter(Parameter<int>("item").AddDefault(Argument(10.ToLiteral())))
                        .AddStatement(new ForEachStatement(DataTypes.Var, "item", new SimpleNameExpression("source"))))))
            .ToCode();

        await Assert.That(code).Contains("for (var @item in source)");
    }
}