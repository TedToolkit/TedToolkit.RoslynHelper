using TedToolkit.RoslynHelper.Generators;
using TedToolkit.RoslynHelper.Generators.Syntaxes;

namespace TedToolkit.RoslynHelper.Tests;

internal static class TestRenderers
{
    public static string Render(IToCode item)
    {
        return Normalize(item.ToCode());
    }

    public static string Render(SourceFile item)
    {
        return Normalize(item.ToCode());
    }

    public static string RenderDescription(IDescriptionItem item)
    {
        var builder = new SourceBuilder();

        try
        {
            item.ToDescription(ref builder);
            return Normalize(builder.ToCode());
        }
        finally
        {
            builder.Dispose();
        }
    }

    public static string RenderRootDescription(IRootDescriptionItem item)
    {
        var builder = new SourceBuilder();

        try
        {
            item.ToDescription(ref builder);
            return Normalize(builder.ToCode());
        }
        finally
        {
            builder.Dispose();
        }
    }

    public static string Normalize(string value)
    {
        return value.Replace("\r\n", "\n");
    }
}
