namespace WinFormMarkup.Extensions;

public static class ToolStripDropDownItemExtensions
{
    public static TToolStripDropDownItem DropDownItems<TToolStripDropDownItem>(
        this TToolStripDropDownItem stripItem,
        params ToolStripItem[] items)
        where TToolStripDropDownItem : ToolStripDropDownItem
    {
        stripItem.DropDownItems.AddRange(items);
        return stripItem;
    }
}