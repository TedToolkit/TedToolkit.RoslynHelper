using Microsoft.CodeAnalysis;
using TedToolkit.RoslynHelper.Syntaxes;

namespace TedToolkit.RoslynHelper.Tests;

using static SourceComposer;

internal sealed class RoslynSymbolConversionTests
{
    /// <summary>
    /// Verifies that DataType.FromSymbol preserves aliases for imported metadata references.
    /// </summary>
    [Test]
    public async Task Should_render_reference_alias_when_creating_data_type_from_symbol()
    {
        const string dependencySource = """
namespace External.Library;

public class Box<T>;
""";
        const string source = """
extern alias demo;

namespace Consumer;

public class Sample
{
    public demo::External.Library.Box<int> Property { get; set; } = null!;
}
""";

        var reference = RoslynTestHelper.CreateReferenceFromSource(dependencySource, "External.Library", "demo");
        var compilation = RoslynTestHelper.CreateCompilation(source, additionalReferences: [reference]);
        var symbol = RoslynTestHelper.GetNamedType(compilation, "Consumer.Sample")
            .GetMembers("Property")
            .OfType<IPropertySymbol>()
            .Single()
            .Type;

        var result = TestRenderers.Render(DataType.FromSymbol(symbol, compilation));

        await Assert.That(result).IsEqualTo("demo::External.Library.Box<int>");
    }

    /// <summary>
    /// Verifies that DataType.FromSymbol supports tuples, nullable reference types, and generic symbols.
    /// </summary>
    [Test]
    public async Task Should_render_tuple_and_nullable_shapes_when_creating_data_type_from_symbol()
    {
        const string source = """
using System.Collections.Generic;

namespace Consumer;

public class Sample
{
    public (string? Name, List<int> Values) Property { get; set; } = default!;
}
""";

        var compilation = RoslynTestHelper.CreateCompilation(source);
        var symbol = RoslynTestHelper.GetNamedType(compilation, "Consumer.Sample")
            .GetMembers("Property")
            .OfType<IPropertySymbol>()
            .Single()
            .Type;

        var result = TestRenderers.Render(DataType.FromSymbol(symbol, compilation));

        await Assert.That(result).IsEqualTo("(string? Name, global::System.Collections.Generic.List<int> Values)");
    }

    /// <summary>
    /// Verifies that Parameter.FromSymbol preserves parameter modifiers, defaults, and attributes.
    /// </summary>
    [Test]
    public async Task Should_render_parameter_metadata_when_creating_parameter_from_symbol()
    {
        const string source = """
using System;

namespace Consumer;

[AttributeUsage(AttributeTargets.Parameter)]
public sealed class DemoAttribute : Attribute
{
}

public static class Extensions
{
    public static void Configure(this string text, [Demo] bool? enabled = null)
    {
    }
}
""";

        var compilation = RoslynTestHelper.CreateCompilation(source);
        var extensionParameter = RoslynTestHelper.GetParameter(compilation, "Consumer.Extensions", "Configure", 0);
        var optionalParameter = RoslynTestHelper.GetParameter(compilation, "Consumer.Extensions", "Configure", 1);

        await Assert.That(TestRenderers.Render(Parameter(extensionParameter, compilation))).IsEqualTo("this string text");
        await Assert.That(TestRenderers.Render(Parameter(optionalParameter, compilation)))
            .IsEqualTo("[global::Consumer.DemoAttribute]\nbool? enabled = default");
    }

    /// <summary>
    /// 验证复制未显式提供可选构造参数的特性时不会补出默认参数。
    /// </summary>
    [Test]
    public async Task Should_omit_implicit_optional_attribute_arguments_when_copying_parameter_or_method_attribute()
    {
        const string source = """
using System;

namespace Consumer;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method)]
public abstract class DocumentationAttribute : Attribute;

public enum ConstDepth
{
    ALL,
}

public sealed class ConstAttribute(ConstDepth depths = ConstDepth.ALL) : DocumentationAttribute;

public class Sample
{
    [Const]
    public void Execute([Const] int value)
    {
    }
}
""";

        var compilation = RoslynTestHelper.CreateCompilation(source);
        var method = RoslynTestHelper.GetMethod(compilation, "Consumer.Sample", "Execute");
        var parameter = method.Parameters.Single();

        await Assert.That(TestRenderers.Render(Attribute(method.GetAttributes().Single(), compilation)))
            .IsEqualTo("global::Consumer.ConstAttribute");
        await Assert.That(TestRenderers.Render(Parameter(parameter, compilation)))
            .IsEqualTo("[global::Consumer.ConstAttribute]\nint @value");
    }

    /// <summary>
    /// Verifies that Parameter.FromSymbol handles ref-like storage modifiers from Roslyn symbols.
    /// </summary>
    [Test]
    public async Task Should_render_storage_kinds_when_creating_parameter_from_symbol()
    {
        const string source = """
namespace Consumer;

public class Sample
{
    public void Update(ref int left, out int right, scoped in int value)
    {
        right = left + value;
    }
}
""";

        var compilation = RoslynTestHelper.CreateCompilation(source);

        await Assert.That(TestRenderers.Render(Parameter(RoslynTestHelper.GetParameter(compilation, "Consumer.Sample", "Update", 0), compilation)))
            .IsEqualTo("ref int left");
        await Assert.That(TestRenderers.Render(Parameter(RoslynTestHelper.GetParameter(compilation, "Consumer.Sample", "Update", 1), compilation)))
            .IsEqualTo("out int right");
        await Assert.That(TestRenderers.Render(Parameter(RoslynTestHelper.GetParameter(compilation, "Consumer.Sample", "Update", 2), compilation)))
            .IsEqualTo("scoped in int @value");
    }

    /// <summary>
    /// Verifies that TypeParameter.FromSymbol keeps constraint information from Roslyn symbols.
    /// </summary>
    [Test]
    public async Task Should_render_constraints_when_creating_type_parameter_from_symbol()
    {
        const string source = """
using System;

namespace Consumer;

public class Sample
{
    public void Process<T>()
        where T : class?, IDisposable, new()
    {
    }
}
""";

        var compilation = RoslynTestHelper.CreateCompilation(source);
        var typeParameter = TypeParameter(RoslynTestHelper.GetTypeParameter(compilation, "Consumer.Sample", "Process", 0), compilation);
        var method = new Method("Process")
            .AddTypeParameter(typeParameter);

        await Assert.That(TestRenderers.Render(typeParameter)).IsEqualTo("T");
        await Assert.That(TestRenderers.Render(method)).Contains("where T: class?, global::System.IDisposable, new()");
    }

    /// <summary>
    /// Verifies that Attribute.FromSymbol renders primitive, enum, type, array, and named arguments.
    /// </summary>
    [Test]
    public async Task Should_render_attribute_arguments_when_creating_attribute_from_symbol()
    {
        const string source = """
using System;

namespace Consumer;

[AttributeUsage(AttributeTargets.Class)]
public sealed class DemoAttribute : Attribute
{
    public DemoAttribute(string text, DayOfWeek day, Type kind, Type[] kinds, float ratio, double factor)
    {
    }

    public bool Enabled { get; set; }
}

[Demo("hello", DayOfWeek.Friday, typeof(string), new[] { typeof(int), typeof(DateTime) }, 1.5f, 2.5d, Enabled = true)]
public class Sample;
""";

        var compilation = RoslynTestHelper.CreateCompilation(source);
        var attributeData = RoslynTestHelper.GetAttribute(compilation, "Consumer.Sample", "DemoAttribute");

        var result = TestRenderers.Render(SourceComposer.Attribute(attributeData, compilation));

        await Assert.That(result).IsEqualTo(
            "global::Consumer.DemoAttribute(\"hello\", global::System.DayOfWeek.Friday, typeof(string), [\n\ttypeof(int),\n\ttypeof(global::System.DateTime),\n], 1.5F, 2.5D, Enabled = true)");
    }
}