namespace WinFormMarkup.Extensions;

public static class TableLayoutPanelExtensions
{
    /// <summary>
    ///     Sets the ColumnStyles collection, and returns a reference to the panel.
    /// </summary>
    /// <remarks>
    ///     **NOTE:** columnStyles is a string delimited by '|' with values for each ColumnStyle. Values are '*' or autosize,
    ///     'nn.n%' for percent width, and 'nn.n' for absolute. width
    /// </remarks>
    /// <example>
    ///     var panel = new TableLayoutPanel();
    ///     panel.ColumnStyles("25|40%|40%|*");
    /// </example>
    /// <param name="tableLayoutPanel"></param>
    /// <param name="columnStyles"></param>
    /// <typeparam name="TTableLayoutPanel"></typeparam>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static TTableLayoutPanel ColumnStyles<TTableLayoutPanel>(
        this TTableLayoutPanel tableLayoutPanel,
        string columnStyles)
        where TTableLayoutPanel : TableLayoutPanel
    {
        if (columnStyles == null) throw new ArgumentNullException(nameof(columnStyles));
        foreach (var style in columnStyles.Split('|'))
            tableLayoutPanel.ColumnStyles.Add(
                style[^1] == '*'
                    ? new ColumnStyle { SizeType = SizeType.AutoSize, Width = 1 }
                    : style[^1] == '%'
                        ? new ColumnStyle { SizeType = SizeType.Percent, Width = float.Parse(style[..^1]) }
                        : new ColumnStyle { SizeType = SizeType.Absolute, Width = float.Parse(style) });

        return tableLayoutPanel;
    }

    public static TTableLayoutPanel RowStyles<TTableLayoutPanel>(
        this TTableLayoutPanel tableLayoutPanel,
        string rowStyles)
        where TTableLayoutPanel : TableLayoutPanel
    {
        if (rowStyles == null) throw new ArgumentNullException(nameof(rowStyles));
        foreach (var style in rowStyles.Split('|'))
            tableLayoutPanel.RowStyles.Add(
                style[^1] == '*'
                    ? new RowStyle { SizeType = SizeType.AutoSize, Height = 1 }
                    : style[^1] == '%'
                        ? new RowStyle { SizeType = SizeType.Percent, Height = float.Parse(style[..^1]) }
                        : new RowStyle { SizeType = SizeType.Absolute, Height = float.Parse(style) });

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
            tableLayoutPanel.Controls.Add(c.Control, c.Column, c.Row);
            if (c.HasSpans)
            {
                if (c.ColumnSpan > 1) tableLayoutPanel.SetColumnSpan(c.Control, c.ColumnSpan);
                if (c.RowSpan > 1) tableLayoutPanel.SetRowSpan(c.Control, c.RowSpan);
            }
        }

        tableLayoutPanel.ResumeLayout();

        return tableLayoutPanel;
    }

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
}

public class TableLocation
{
    public TableLocation(int column, int row, Control control) : this(column, row, 1, 1, control)
    {
        HasSpans = false;
    }

    public TableLocation(int column, int row, int columnSpan, int rowSpan, Control control)
    {
        Column = column;
        Row = row;
        ColumnSpan = columnSpan;
        RowSpan = rowSpan;
        Control = control;
        HasSpans = RowSpan > 1 || ColumnSpan > 1;
    }

    public int Column { get; }
    public int Row { get; }
    public int ColumnSpan { get; }
    public int RowSpan { get; }
    public Control Control { get; }
    public bool HasSpans { get; }
}