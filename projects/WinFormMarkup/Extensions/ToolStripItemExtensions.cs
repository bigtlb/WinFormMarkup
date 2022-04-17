using System;
using System.Windows.Forms;

namespace WinFormMarkup.Extensions
{
    public static class ToolStripItemExtensions
    {
        public static TToolStripItem Clicked<TToolStripItem>(
            this TToolStripItem item,
            Action<TToolStripItem> action)
            where TToolStripItem : ToolStripItem
        {
            item.Click += (sender, _) => action.Invoke((sender as TToolStripItem)!);
            return item;
        }


        public static TToolStripItem Enabled<TToolStripItem>(
            this TToolStripItem item,
            bool enabled)
            where TToolStripItem : ToolStripItem
        {
            item.Enabled = enabled;
            return item;
        }

        public static TToolStripItem Text<TToolStripItem>(
            this TToolStripItem item,
            string text)
            where TToolStripItem : ToolStripItem?
        {
            item.Text = text;
            return item;
        }
        
        public static TToolStripItem TextAlign<TToolStripItem>(
            this TToolStripItem item,
            ContentAlignment textAlign)
            where TToolStripItem : ToolStripItem
        {
            item.TextAlign = textAlign;
            return item;
        }
        
                
        public static TToolStripItem Alignment<TToolStripItem>(
            this TToolStripItem item,
            ToolStripItemAlignment textAlign)
            where TToolStripItem : ToolStripItem
        {
            item.Alignment = textAlign;
            return item;
        }
        
        public static TToolStripItem Also<TToolStripItem>(
            this TToolStripItem item,
            Action<TToolStripItem> action)
            where TToolStripItem : ToolStripItem
        {
            action.Invoke(item);
            return item;
        }
    }
}