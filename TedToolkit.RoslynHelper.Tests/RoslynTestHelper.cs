using System.Collections.Immutable;
using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TedToolkit.RoslynHelper.Syntaxes;

namespace TedToolkit.RoslynHelper.Tests;

internal static class RoslynTestHelper
{
    private static readonly CSharpParseOptions ParseOptions = CSharpParseOptions.Default
        .WithLanguageVersion(LanguageVersion.Preview);

    private static readonly ImmutableArray<MetadataReference> DefaultReferences = AppContext
        .GetData("TRUSTED_PLATFORM_ASSEMBLIES") is string trustedPlatformAssemblies
            ? trustedPlatformAssemblies
                .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries)
                .Select(path => MetadataReference.CreateFromFile(path))
                .Cast<MetadataReference>()
                .ToImmutableArray()
            : ImmutableArray<MetadataReference>.Empty;

    public static CSharpCompilation CreateCompilation(
        string source,
        string assemblyName = "RoslynHelper.Tests.Dynamic",
        IEnumerable<MetadataReference>? additionalReferences = null)
    {
        var references = DefaultReferences;
        if (additionalReferences is not null)
        {
            references = references.AddRange(additionalReferences);
        }

        var compilation = CSharpCompilation.Create(
            assemblyName,
            [CSharpSyntaxTree.ParseText(source, ParseOptions)],
            references,
            new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                nullableContextOptions: NullableContextOptions.Enable));

        var errors = compilation.GetDiagnostics()
            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .ToArray();

        if (errors.Length > 0)
        {
            throw new InvalidOperationException(string.Join(Environment.NewLine, errors));
        }

        return compilation;
    }

    public static MetadataReference CreateReferenceFromSource(
        string source,
        string assemblyName,
        params string[] aliases)
    {
        var compilation = CreateCompilation(source, assemblyName);
        using var stream = new MemoryStream();
        var result = compilation.Emit(stream);
        if (!result.Success)
        {
            throw new InvalidOperationException(string.Join(Environment.NewLine, result.Diagnostics));
        }

        return MetadataReference.CreateFromImage(
            stream.ToArray(),
            new MetadataReferenceProperties(aliases: aliases.ToImmutableArray()));
    }

    public static INamedTypeSymbol GetNamedType(Compilation compilation, string metadataName)
    {
        return compilation.GetTypeByMetadataName(metadataName)
               ?? throw new InvalidOperationException($"Type '{metadataName}' was not found.");
    }

    public static IMethodSymbol GetMethod(
        Compilation compilation,
        string metadataName,
        string methodName)
    {
        var method = GetNamedType(compilation, metadataName)
            .GetMembers(methodName)
            .OfType<IMethodSymbol>()
            .SingleOrDefault();

        return method ?? throw new InvalidOperationException($"Method '{metadataName}.{methodName}' was not found.");
    }

    public static IParameterSymbol GetParameter(
        Compilation compilation,
        string metadataName,
        string methodName,
        int index)
    {
        return GetMethod(compilation, metadataName, methodName).Parameters[index];
    }

    public static ITypeParameterSymbol GetTypeParameter(
        Compilation compilation,
        string metadataName,
        string typeParameterOwner,
        int index)
    {
        var owner = GetNamedType(compilation, metadataName)
            .GetMembers(typeParameterOwner)
            .OfType<IMethodSymbol>()
            .SingleOrDefault();

        return owner?.TypeParameters[index]
               ?? throw new InvalidOperationException(
                   $"Type parameter '{metadataName}.{typeParameterOwner}[{index}]' was not found.");
    }

    public static AttributeData GetAttribute(
        Compilation compilation,
        string metadataName,
        string attributeName)
    {
        return GetNamedType(compilation, metadataName)
            .GetAttributes()
            .Single(attribute => attribute.AttributeClass?.Name == attributeName);
    }

    public static MetadataReference CreateProjectReference()
    {
        return MetadataReference.CreateFromFile(typeof(SourceComposer).Assembly.Location);
    }
}