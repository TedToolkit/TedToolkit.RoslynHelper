using BenchmarkDotNet.Attributes;

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using TedToolkit.RoslynHelper.Extensions;
using TedToolkit.RoslynHelper.Generators;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

using static TedToolkit.RoslynHelper.Extensions.SyntaxExtensions;
using static TedToolkit.RoslynHelper.Generators.SourceComposer;
using static TedToolkit.RoslynHelper.Generators.SourceComposer<TedToolkit.RoslynHelper.Benchmarks.RoslynRunner>;

namespace TedToolkit.RoslynHelper.Benchmarks;

/// <summary>
/// Roslyn Runner.
/// </summary>
[MemoryDiagnoser]
public class RoslynRunner
{
    /// <summary>
    /// Old one
    /// </summary>
    /// <returns>code</returns>
    [Benchmark(Baseline = true)]
    public string Roslyn()
    {
        var node = NamespaceDeclaration(
                IdentifierName("Space"))
            .WithMembers(
                SingletonList<MemberDeclarationSyntax>(
                    ClassDeclaration("Class")
                        .AddAttributeLists(GeneratedCodeAttribute(typeof(RoslynRunner)))
                        .WithModifiers(
                            TokenList(
                                new[]
                                {
                                    Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword),
                                    Token(SyntaxKind.UnsafeKeyword)
                                }))
                        .WithTypeParameterList(
                            TypeParameterList(
                                SingletonSeparatedList<TypeParameterSyntax>(
                                    TypeParameter(
                                        Identifier("T")))))));
        return node.NodeToString();
    }

    /// <summary>
    /// New one.
    /// </summary>
    /// <returns>code</returns>
    [Benchmark]
    public string Helper()
    {
        var file = File("File")
            .AddNameSpace(NameSpace("Space")
                .AddMember(Class("Class").Public.Static.Unsafe
                    .AddTypeParameter(SourceComposer.TypeParameter("T"))));

        return file.ToCode();
    }
}