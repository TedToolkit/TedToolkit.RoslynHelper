using Microsoft.CodeAnalysis;
using TedToolkit.RoslynHelper.Syntaxes;

namespace TedToolkit.RoslynHelper.Tests;

internal sealed class RoundTripTests
{
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
        var pointer = new FunctionPointerType(DataType.Void, convention)
            .AddParameter(DataType.Int.Ref)
            .AddParameter(DataType.Byte.Pointer);
        var field = new Field(new DataType(pointer), "Callback").Public;
        var source = TestRenderers.Render(field);

        await Assert.That(source.Trim()).IsEqualTo($"public delegate*{specifier}<ref int, byte*, void> Callback;");
        RoslynTestHelper.CreateCompilation($$"""
            public unsafe class Generated
            {
                {{source}}
            }
            """, allowUnsafe: true);
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
        var input = RoslynTestHelper.CreateCompilation($$"""
            public unsafe class Original
            {
                public {{signature}} Callback;
            }
            """, allowUnsafe: true);
        var symbol = RoslynTestHelper.GetNamedType(input, "Original")
            .GetMembers("Callback")
            .OfType<IFieldSymbol>()
            .Single()
            .Type;

        var generated = TestRenderers.Render(DataType.FromSymbol(symbol, input));

        await Assert.That(generated).IsEqualTo(signature);
        RoslynTestHelper.CreateCompilation($$"""
            public unsafe class Generated
            {
                public {{generated}} Callback;
            }
            """, allowUnsafe: true);
    }
}