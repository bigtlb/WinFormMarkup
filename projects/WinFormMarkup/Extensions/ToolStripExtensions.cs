namespace WinFormMarkup.Extensions;

public static class ToolStripExtensions
{
    public static TToolStrip Items<TToolStrip>(
        this TToolStrip strip,
        params ToolStripItem[] items)
        where TToolStrip : ToolStrip
    {
        strip.Items.AddRange(items);
        return strip;
    }
}