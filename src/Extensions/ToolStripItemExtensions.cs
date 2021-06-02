using System.Windows.Forms;

namespace WinFormMarkup.Extensions
{
    public static class ToolStripItemExtensions
    {
        public static TToolStripItem Text<TToolStripItem>(
            this TToolStripItem item,
            string text)
            where TToolStripItem : ToolStripItem
        {
            item.Text = text;
            return item;
        }
    }
}