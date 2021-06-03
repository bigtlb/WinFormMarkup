using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinFormMarkup.Extensions
{
    public static class TableLayoutPanelExtensions
    {
        public static TTableLayoutPanel TableLayout<TTableLayoutPanel>(
            this TTableLayoutPanel tableLayoutPanel,
            int columnCount,
            int rowCount,
            bool autoSize = true)
            where TTableLayoutPanel : TableLayoutPanel
        {
            tableLayoutPanel.ColumnCount = columnCount;
            tableLayoutPanel.RowCount = rowCount;
            tableLayoutPanel.AutoSize = autoSize;
            return tableLayoutPanel;
        }
        
        public static TTableLayoutPanel TableControls<TTableLayoutPanel>(
            this TTableLayoutPanel tableLayoutPanel,
            params TableLocation[] children)
            where TTableLayoutPanel : TableLayoutPanel
        {
            tableLayoutPanel.SuspendLayout();
            foreach (var c in children)
            {
                tableLayoutPanel.Controls.Add(c.control, c.column, c.row);
            }
            tableLayoutPanel.ResumeLayout();

            return tableLayoutPanel;
        }
    }
    public record TableLocation(Control control, int column, int row);
}