using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Tests;

internal sealed class GeneratorDescriptionTests
{
    /// <summary>
    /// Verifies that text descriptions escape XML-sensitive characters.
    /// </summary>
    [Test]
    public async Task Should_escape_xml_content_in_description_text()
    {
        var code = TestRenderers.RenderDescription(new DescriptionText("<demo> & \"quote\""));

        await Assert.That(code).IsEqualTo("/// &lt;demo&gt; &amp; &quot;quote&quot;\n");
    }

    /// <summary>
    /// Verifies that inline and block formatting descriptions wrap nested text with the expected tags.
    /// </summary>
    [Test]
    public async Task Should_render_formatting_description_wrappers()
    {
        await Assert.That(TestRenderers.RenderDescription(new DescriptionBold(new DescriptionText("Bold"))))
            .IsEqualTo("/// <b>\n/// Bold\n/// </b>\n");
        await Assert.That(TestRenderers.RenderDescription(new DescriptionItalic(new DescriptionText("Italic"))))
            .IsEqualTo("/// <i>\n/// Italic\n/// </i>\n");
        await Assert.That(TestRenderers.RenderDescription(new DescriptionPara(new DescriptionText("Paragraph"))))
            .IsEqualTo("/// <para>\n/// Paragraph\n/// </para>\n");
        await Assert.That(TestRenderers.RenderDescription(new DescriptionCode(false, new DescriptionText("x"))))
            .IsEqualTo("/// <c>\n/// x\n/// </c>\n");
        await Assert.That(TestRenderers.RenderDescription(new DescriptionCode(true, new DescriptionText("x"))))
            .IsEqualTo("/// <code>\n/// x\n/// </code>\n");
    }

    /// <summary>
    /// Verifies that root descriptions render the correct XML containers.
    /// </summary>
    [Test]
    public async Task Should_render_root_description_containers()
    {
        await Assert.That(TestRenderers.RenderRootDescription(new DescriptionSummary(new DescriptionText("Summary"))))
            .IsEqualTo("/// <summary>\n/// Summary\n/// </summary>\n");
        await Assert.That(TestRenderers.RenderRootDescription(new DescriptionExample(new DescriptionText("Example"))))
            .IsEqualTo("/// <example>\n/// Example\n/// </example>\n");
        await Assert.That(TestRenderers.RenderRootDescription(new DescriptionRemarks(new DescriptionText("Remarks"))))
            .IsEqualTo("/// <remarks>\n/// Remarks\n/// </remarks>\n");
        await Assert.That(TestRenderers.RenderRootDescription(new DescriptionReturns(new DescriptionText("Returns"))))
            .IsEqualTo("/// <returns>\n/// Returns\n/// </returns>\n");
        await Assert.That(TestRenderers.RenderRootDescription(new DescriptionValue(new DescriptionText("Value"))))
            .IsEqualTo("/// <value>\n/// Value\n/// </value>\n");
    }

    /// <summary>
    /// Verifies that cref-based descriptions choose the expected XML element and cref formatting.
    /// </summary>
    [Test]
    public async Task Should_render_cref_based_descriptions()
    {
        var cref = new TypeParameterExpression("List".ToSimpleName(), DataType.Int);

        await Assert.That(TestRenderers.RenderDescription(new DescriptionSee(cref)))
            .IsEqualTo("/// <see cref=\"List{int}\"/>\n");
        await Assert.That(TestRenderers.RenderRootDescription(new DescriptionSeeAlso(cref)))
            .IsEqualTo("/// <seealso cref=\"List{int}\"/>\n");
        await Assert.That(TestRenderers.RenderRootDescription(new DescriptionException(cref, new DescriptionText("Thrown"))))
            .IsEqualTo("/// <exception cref=\"List{int}\">\n/// Thrown\n/// </exception>\n");
        await Assert.That(TestRenderers.RenderRootDescription(new DescriptionInheritDoc()))
            .IsEqualTo("/// <inheritdoc/>\n");
        await Assert.That(TestRenderers.RenderRootDescription(new DescriptionInheritDoc(cref)))
            .IsEqualTo("/// <inheritdoc cref=\"List{int}\"/>\n");
    }

    /// <summary>
    /// Verifies that named and custom descriptions preserve the expected content.
    /// </summary>
    [Test]
    public async Task Should_render_named_table_and_custom_descriptions()
    {
        var table = new DescriptionTable(new DescriptionText("Name"), new DescriptionText("Meaning"))
            .AddItem(new DescriptionText("A"), new DescriptionText("Alpha"));

        await Assert.That(TestRenderers.RenderRootDescription(new DescriptionParam("@item", new DescriptionText("Param"))))
            .IsEqualTo("/// <param name=\"item\">\n/// Param\n/// </param>\n");
        await Assert.That(TestRenderers.RenderRootDescription(new DescriptionTypeParam("T", new DescriptionText("Type"))))
            .IsEqualTo("/// <typeparam name=\"T\">\n/// Type\n/// </typeparam>\n");
        await Assert.That(TestRenderers.RenderDescription(table)).IsEqualTo(
            "/// <list type=\"table\">\n/// <listheader>\n/// <term>\n/// Name\n/// </term>\n/// <description>\n/// Meaning\n/// </description>\n/// </listheader>\n/// <item>\n/// <term>\n/// A\n/// </term>\n/// <description>\n/// Alpha\n/// </description>\n/// </item>\n/// </list>\n");
        await Assert.That(TestRenderers.RenderDescription(new DescriptionCustom("/// raw")))
            .IsEqualTo("/// raw");
    }
}
