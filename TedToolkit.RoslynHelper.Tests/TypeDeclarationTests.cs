using System.Runtime.CompilerServices;

using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Tests;

using static SourceComposer;
using static SourceComposer<TypeDeclarationTests>;

internal class TypeDeclarationTests
{
    [Test]
    public async Task StaticClassTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public.Static.Unsafe.Partial))
            .ToCode();

        await Assert.That(code)
            .Contains(
                "[System.CodeDom.Compiler.GeneratedCodeAttribute(\"TedToolkit.RoslynHelper.Tests.TypeDeclarationTests\", \"");
        await Assert.That(code).Contains("public static unsafe partial class FirstClass");
    }

    [Test]
    public async Task BaseTypeTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public.AddBaseType<IDisposable>()))
            .ToCode();

        await Assert.That(code).Contains("System.IDisposable");
    }

    [Test]
    public async Task ParametersTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddParameter(Parameter(DataType.Int.ScopedIn, "item").This.AddDefault(10.ToLiteral()))))
            .ToCode();

        await Assert.That(code).Contains("this scoped in int item = 10");
    }

    [Test]
    public async Task ParametersNullTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddParameter(Parameter(DataType.Int.Null.ScopedIn,"item").AddDefault(10.ToLiteral()))))
            .ToCode();

        await Assert.That(code).Contains("scoped in int? item = 10");
    }


    [Test]
    public async Task ParametersNestTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddParameter(Parameter(DataType.FromType<NestType.NestClass>().ScopedIn, "item").AddDefault(10.ToLiteral()))))
            .ToCode();

        await Assert.That(code).Contains("scoped in TedToolkit.RoslynHelper.Tests.NestType.NestClass item = 10");
    }

    [Test]
    public async Task SummaryTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddRootDescription(new DescriptionSummary(
                        new DescriptionText("Good")
                    ))))
            .ToCode();

        await Assert.That(code).Contains("/// <summary>");
        await Assert.That(code).Contains("/// Good");
        await Assert.That(code).Contains("/// </summary>");
    }

    [Test]
    public async Task ParameterSummaryTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddParameter(Parameter<int>("item").AddDefault(10.ToLiteral()).AddDescription(
                        new DescriptionText("Good")
                    ))))
            .ToCode();

        await Assert.That(code).Contains("/// <param name=\"item\">");
        await Assert.That(code).Contains("/// Good");
        await Assert.That(code).Contains("/// </param>");
    }

    [Test]
    public async Task MethodTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddMember(Method("Method")
                        .AddParameter(Parameter<int>("item").AddDefault(10.ToLiteral())))))
            .ToCode();

        await Assert.That(code).Contains("void Method(");
    }


    [Test]
    public async Task MethodPartialTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddMember(Method("Method").Partial)))
            .ToCode();

        await Assert.That(code).Contains("void Method();");
    }

    [Test]
    public async Task MethodAttributeTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddMember(Method("Method")
                        .AddAttribute(Attribute<MethodImplAttribute>()
                            .AddArgument(Argument(MethodImplOptions.AggressiveInlining.ToExpression()))))))
            .ToCode();

        await Assert.That(code).Contains("[System.Runtime.CompilerServices.MethodImplAttribute(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]");
    }

    [Test]
    public async Task PropertyTest()
    {
        var code = File()
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
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddMember(Field<long>("Item").Internal.Readonly)))
            .ToCode();

        await Assert.That(code).Contains("internal readonly long Item;");
    }

    [Test]
    public async Task EventTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddMember(Event<Action<int>>("Item").Internal)))
            .ToCode();

        await Assert.That(code).Contains("internal event System.Action<int> Item;");
    }

    [Test]
    public async Task DelegateTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Delegate("ADelegate").Public))
            .ToCode();

        await Assert.That(code).Contains("public delegate void ADelegate();");
    }

    [Test]
    public async Task ForeachTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddMember(Method("Method")
                        .AddParameter(Parameter<int>("item").AddDefault(10.ToLiteral()))
                        .AddStatement(new ForEachStatement(DataType.Var, "item",
                            new SimpleNameExpression("source"))))))
            .ToCode();

        await Assert.That(code).Contains("foreach (var item in source)");
    }

    [Test]
    public async Task VariableTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddMember(Method("Method")
                        .AddParameter(Parameter<int>("item").AddDefault(10.ToLiteral()))
                        .AddStatement(new VariableExpression(DataType.Int, "item")
                            .AddDefault(10.ToLiteral())))))
            .ToCode();

        await Assert.That(code).Contains("int item = 10;");
    }

    [Test]
    public async Task GenericTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddTypeParameter(TypeParameter("Good").In
                        .AddNewConstraint()
                        .AddConstraint<int>())
                ))
            .ToCode();

        await Assert.That(code).Contains("in Good");
        await Assert.That(code).Contains("where Good: new(), int");
    }

    [Test]
    public async Task SwitchTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddMember(Method("Method")
                        .AddStatement(new SwitchStatement("a".ToSimpleName())
                            .AddSection(new SwitchSection()
                                .AddLabel(new SwitchLabel(1.ToLiteral(), "true".ToSimpleName()))
                                .AddLabel(new SwitchLabel())
                                .AddStatement("break".ToSimpleName()))))))
            .ToCode();

        await Assert.That(code).Contains("switch (a)");
        await Assert.That(code).Contains("case 1 when true:");
        await Assert.That(code).Contains("default:");
    }

    [Test]
    public async Task IndexerTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddMember(Indexer<int>().Internal
                        .AddParameter(Parameter<int>("index"))
                        .AddAccessor(Accessor(AccessorType.GET)
                            .AddStatement(10.ToLiteral().Return)))))
            .ToCode();

        await Assert.That(code).Contains("internal int this[");
        await Assert.That(code).Contains("get");
    }

    [Test]
    public async Task ConstructorTest()
    {
        var code = File()
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddMember(Constructor().Public
                        .AddInitializer(new ConstructorInitializer(false)))))
            .ToCode();

        await Assert.That(code).Contains("public FirstClass() : this()");
    }
}