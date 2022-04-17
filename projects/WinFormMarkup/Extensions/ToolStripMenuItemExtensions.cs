using System.Windows.Forms;

namespace WinFormMarkup.Extensions
{
    public static class ToolStripMenuItemExtensions
    {
        public static TToolStripMenuItem Keys<TToolStripMenuItem>(
            this TToolStripMenuItem menuItem,
            Keys shortcutKeys)
            where TToolStripMenuItem : ToolStripMenuItem
        {
            menuItem.ShortcutKeys = shortcutKeys;
            return menuItem;
        }
    }
}