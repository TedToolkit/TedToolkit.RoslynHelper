using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Tests;

using static SourceComposer;
using static SourceComposer<ExpressionTests>;

internal class ExpressionTests
{
    [Test]
    public async Task ObjectCreationTest()
    {
        var code = File("File")
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddMember(Method("Method")
                        .AddStatement(new ObjectCreationExpression()))))
            .ToCode();

        await Assert.That(code).Contains("new();");
    }

    [Test]
    public async Task ObjectIntCreationTest()
    {
        var code = File("File")
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddMember(Method("Method")
                        .AddStatement(new ObjectCreationExpression(DataType.FromType<int>())
                            .AddArgument(Argument(10.ToLiteral()))))))
            .ToCode();

        await Assert.That(code).Contains("new int(10);");
    }

    [Test]
    public async Task CollectionsTest()
    {
        var code = File("File")
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("FirstClass").Public
                    .AddMember(Method("Method")
                        .AddStatement(new CollectionExpression()
                            .AddExpression(new ObjectCreationExpression(DataType.FromType<int>()))
                            .AddExpression(new ObjectCreationExpression(DataType.FromType<double>()))))))
            .ToCode();

        await Assert.That(code).Contains("[");
        await Assert.That(code).Contains("new int(),");
        await Assert.That(code).Contains("new double(),");
        await Assert.That(code).Contains("];");
    }
}