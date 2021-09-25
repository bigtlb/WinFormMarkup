using System;
using System.Linq.Expressions;
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
        
        public static TToolStripItem Clicked<TToolStripItem>(
            this TToolStripItem item,
            Action<TToolStripItem> action)
            where TToolStripItem : ToolStripItem
        {
            item.Click += (sender, _) => action.Invoke((sender as TToolStripItem)!);
            return item;
        }
    }
}