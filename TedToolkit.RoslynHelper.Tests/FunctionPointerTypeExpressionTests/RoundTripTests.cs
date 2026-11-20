using System.Reflection.Metadata;

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
    [Arguments(SignatureCallingConvention.Default, "")]
    [Arguments(SignatureCallingConvention.Unmanaged, " unmanaged")]
    [Arguments(SignatureCallingConvention.CDecl, " unmanaged[Cdecl]")]
    [Arguments(SignatureCallingConvention.StdCall, " unmanaged[Stdcall]")]
    [Arguments(SignatureCallingConvention.ThisCall, " unmanaged[Thiscall]")]
    [Arguments(SignatureCallingConvention.FastCall, " unmanaged[Fastcall]")]
    public async Task Should_compile_a_field_with_the_selected_calling_convention(
        SignatureCallingConvention convention, string specifier)
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
        var expression = new FunctionPointerTypeExpression(DataType.Void);

        await Assert.That(TestRenderers.Render(pointer)).IsEqualTo("delegate*<void>");
        await Assert.That(TestRenderers.Render(expression)).IsEqualTo("delegate*<void>");
    }

    /// <summary>
    /// Verifies that symbol conversion preserves nested pointers, calling-convention modifiers, and by-reference types.
    /// </summary>
    [Test]
    [Arguments("delegate*<void>", false)]
    [Arguments("delegate* unmanaged<void>", false)]
    [Arguments("delegate* unmanaged[Cdecl]<int, void>", false)]
    [Arguments("delegate* unmanaged[Stdcall]<int, void>", false)]
    [Arguments("delegate* unmanaged[Thiscall]<int, void>", false)]
    [Arguments("delegate* unmanaged[Fastcall]<int, void>", false)]
    [Arguments("delegate*<int*[], void*, void>", false)]
    [Arguments("delegate*<int*[], void*, void>", true)]
    [Arguments("delegate*<int*[][], int*[,], void>", false)]
    [Arguments("delegate*<string?[]?, void>", true)]
    [Arguments("delegate* unmanaged[Cdecl, SuppressGCTransition]<delegate*<void>, in int, out int, ref readonly int, ref readonly int>", false)]
    [Arguments("delegate* unmanaged[Cdecl, SuppressGCTransition]<delegate*<void>, in int, out int, ref readonly int, ref readonly int>", true)]
    public async Task Should_preserve_the_signature_when_converting_a_symbol(string signature, bool fromMetadata)
    {
        var input = CompileFunctionPointerSource($$"""
            public unsafe class Original
            {
                public {{signature}} Callback;
            }
            """);
        if (fromMetadata)
        {
            using var stream = new MemoryStream();
            var emission = input.Emit(stream);
            if (!emission.Success)
            {
                throw new InvalidOperationException(string.Join(Environment.NewLine, emission.Diagnostics));
            }

            input = input.RemoveAllSyntaxTrees()
                .WithAssemblyName("RoslynHelper.Tests.FunctionPointerConsumer")
                .AddReferences(MetadataReference.CreateFromImage(stream.ToArray()));
        }

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

    /// <summary>
    /// Verifies that a function pointer return type rejects parameter-only modifiers.
    /// </summary>
    [Test]
    [Arguments(StorageKind.IN)]
    [Arguments(StorageKind.OUT)]
    [Arguments(StorageKind.SCOPED_REF)]
    public async Task Should_reject_invalid_return_modifiers(StorageKind modifier)
    {
        var returnType = DataType.Int;
        returnType.StorageKind = modifier;

        await Assert.That(() => DataType.FunctionPointer(returnType)).Throws<ArgumentException>();
    }

    /// <summary>
    /// Verifies that function pointer parameters reject scoped modifiers.
    /// </summary>
    [Test]
    [Arguments(StorageKind.SCOPED_REF)]
    [Arguments(StorageKind.SCOPED_IN)]
    [Arguments(StorageKind.SCOPED_REF_READONLY)]
    public async Task Should_reject_scoped_parameter_modifiers(StorageKind modifier)
    {
        var parameterType = DataType.Int;
        parameterType.StorageKind = modifier;

        await Assert.That(() => DataType.FunctionPointer(DataType.Void, parameterTypes: [parameterType]))
            .Throws<ArgumentException>();
    }

    /// <summary>
    /// Verifies that void requires a pointer in parameter positions and by-reference returns.
    /// </summary>
    [Test]
    public async Task Should_reject_invalid_void_types()
    {
        await Assert.That(() => DataType.FunctionPointer(DataType.Void, parameterTypes: [DataType.Void]))
            .Throws<ArgumentException>();
        await Assert.That(() => DataType.FunctionPointer(DataType.Void.Ref)).Throws<ArgumentException>();
    }

    /// <summary>
    /// Verifies that modifying a signature type after construction cannot bypass validation.
    /// </summary>
    [Test]
    public async Task Should_reject_a_parameter_that_becomes_invalid_before_emission()
    {
        var parameterType = DataType.Int;
        var pointer = DataType.FunctionPointer(DataType.Void, parameterTypes: [parameterType]);
        parameterType.StorageKind = StorageKind.SCOPED_REF;

        await Assert.That(() => TestRenderers.Render(pointer)).Throws<ArgumentException>();
    }

    /// <summary>
    /// Verifies that the factory and the Roslyn conversion reject the unsupported VarArgs convention.
    /// </summary>
    [Test]
    public async Task Should_reject_the_varargs_calling_convention()
    {
        var input = CompileFunctionPointerSource("");
        var symbol = input.CreateFunctionPointerTypeSymbol(
            input.GetSpecialType(SpecialType.System_Void), RefKind.None, [], [], SignatureCallingConvention.VarArgs);

        await Assert.That(() => DataType.FunctionPointer(DataType.Void, SignatureCallingConvention.VarArgs))
            .Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => DataType.FromSymbol(symbol, input)).Throws<NotSupportedException>();
    }

    private static CSharpCompilation CompileFunctionPointerSource(string source)
    {
        var compilation = CSharpCompilation.Create(
            "RoslynHelper.Tests.FunctionPointers",
            [CSharpSyntaxTree.ParseText(source)],
            [_coreLibrary],
            new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                allowUnsafe: true,
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
}