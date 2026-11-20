using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using TedToolkit.RoslynHelper.Syntaxes;

namespace TedToolkit.RoslynHelper.Tests;

internal sealed class RoundTripTests
{
    private static readonly MetadataReference _coreLibrary = MetadataReference.CreateFromFile(typeof(object).Assembly.Location);

    /// <summary>
    /// Verifies that each calling convention generates a compilable function pointer field.
    /// </summary>
    [Test]
    [Arguments(FunctionPointerCallingConvention.MANAGED, "")]
    [Arguments(FunctionPointerCallingConvention.UNMANAGED, " unmanaged")]
    [Arguments(FunctionPointerCallingConvention.CDECL, " unmanaged[Cdecl]")]
    [Arguments(FunctionPointerCallingConvention.STDCALL, " unmanaged[Stdcall]")]
    [Arguments(FunctionPointerCallingConvention.THISCALL, " unmanaged[Thiscall]")]
    [Arguments(FunctionPointerCallingConvention.FASTCALL, " unmanaged[Fastcall]")]
    public async Task Should_compile_a_field_with_the_selected_calling_convention(
        FunctionPointerCallingConvention convention, string specifier)
    {
        var pointer = DataType.FunctionPointer(DataType.Void, convention, DataType.Int.Ref, DataType.Byte.Pointer);
        var field = new Field(pointer, "Callback").Public;
        var source = TestRenderers.Render(field);

        await Assert.That(source.Trim()).IsEqualTo($"public delegate*{specifier}<ref int, byte*, void> Callback;");
        CompileFunctionPointerSource($$"""
            public unsafe class Generated
            {
                {{source}}
            }
            """);
    }

    /// <summary>
    /// Verifies that the factory can create a managed function pointer with an empty parameter list.
    /// </summary>
    [Test]
    public async Task Should_default_to_a_managed_signature_without_parameters()
    {
        var pointer = DataType.FunctionPointer(DataType.Void);

        await Assert.That(TestRenderers.Render(pointer)).IsEqualTo("delegate*<void>");
    }

    /// <summary>
    /// Verifies that symbol conversion preserves nested pointers, calling-convention modifiers, and by-reference types.
    /// </summary>
    [Test]
    [Arguments("delegate*<void>")]
    [Arguments("delegate* unmanaged<void>")]
    [Arguments("delegate* unmanaged[Cdecl]<int, void>")]
    [Arguments("delegate* unmanaged[Stdcall]<int, void>")]
    [Arguments("delegate* unmanaged[Thiscall]<int, void>")]
    [Arguments("delegate* unmanaged[Fastcall]<int, void>")]
    [Arguments("delegate* unmanaged[Cdecl, SuppressGCTransition]<delegate*<void>, in int, out int, ref readonly int, ref readonly int>")]
    public async Task Should_preserve_the_signature_when_converting_a_symbol(string signature)
    {
        var input = CompileFunctionPointerSource($$"""
            public unsafe class Original
            {
                public {{signature}} Callback;
            }
            """);
        var symbol = RoslynTestHelper.GetNamedType(input, "Original")
            .GetMembers("Callback")
            .OfType<IFieldSymbol>()
            .Single()
            .Type;

        var generated = TestRenderers.Render(DataType.FromSymbol(symbol, input));

        await Assert.That(generated).IsEqualTo(signature);
        CompileFunctionPointerSource($$"""
            public unsafe class Generated
            {
                public {{generated}} Callback;
            }
            """);
    }

    private static CSharpCompilation CompileFunctionPointerSource(string source)
    {
        var compilation = CSharpCompilation.Create(
            "RoslynHelper.Tests.FunctionPointers",
            [CSharpSyntaxTree.ParseText(source)],
            [_coreLibrary],
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, allowUnsafe: true));

        var errors = compilation.GetDiagnostics()
            .Where(diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
            .ToArray();
        if (errors.Length > 0)
        {
            throw new InvalidOperationException(string.Join(Environment.NewLine, errors));
        }

        return compilation;
    }
}