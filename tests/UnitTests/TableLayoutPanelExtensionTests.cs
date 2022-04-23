using System;
using System.Linq;
using System.Windows.Forms;
using UnitTests.Extensions;
using WinFormMarkup.Extensions;
using Xunit;

namespace UnitTests;

public class TableLayoutPanelExtensionTests
{
    [Fact]
    private void CanSet_ColumnStyles()
    {
        var panel = new TableLayoutPanel();

        Assert.Equal(panel, panel.ColumnStyles("*|25%|30"));

        Assert.Equal(3, panel.ColumnStyles.Count);
        Assert.True(panel.ColumnStyles[0].SizeType == SizeType.AutoSize);
        Assert.True(panel.ColumnStyles[1].SizeType == SizeType.Percent &&
                    panel.ColumnStyles[1].Width.AreClose(25f));
        Assert.True(
            panel.ColumnStyles[2].SizeType == SizeType.Absolute && panel.ColumnStyles[2].Width.AreClose(30f));
    }

    [Fact]
    private void CanSet_RowStyles()
    {
        var panel = new TableLayoutPanel();

        Assert.Equal(panel, panel.RowStyles("*|25%|30"));

        Assert.Equal(3, panel.RowStyles.Count);
        Assert.Equal(SizeType.AutoSize, panel.RowStyles[0].SizeType);
        Assert.True(panel.RowStyles[1].SizeType == SizeType.Percent && panel.RowStyles[1].Height.AreClose(25f));
        Assert.True(panel.RowStyles[2].SizeType == SizeType.Absolute && panel.RowStyles[2].Height.AreClose(30f));
    }

    [Fact]
    private void CanSet_TableControls()
    {
        var panel = new TableLayoutPanel();

        Assert.Equal(panel, panel.TableControls(
            new TableLocation(0, 0, new Control()),
            new TableLocation(1, 0, 2, 1, new Button()),
            new TableLocation(0, 1, 3, 2, new Button())));

        var info =
            (from ctl in panel.Controls.Cast<Control>()
                select new
                {
                    Col = panel.GetColumn(ctl),
                    Row = panel.GetRow(ctl),
                    ColSpan = panel.GetColumnSpan(ctl),
                    RowSpan = panel.GetRowSpan(ctl)
                }).ToArray();

        Assert.Equal(3, info.Length);
        Assert.StrictEqual(new { Col = 0, Row = 0, ColSpan = 1, RowSpan = 1 }, info[0]);
        Assert.StrictEqual(new { Col = 1, Row = 0, ColSpan = 2, RowSpan = 1 }, info[1]);
        Assert.StrictEqual(new { Col = 0, Row = 1, ColSpan = 3, RowSpan = 2 }, info[2]);
    }

    [Fact]
    private void CanSet_TableLayout()
    {
        var panel = new TableLayoutPanel();

        Assert.Equal(panel, panel.TableLayout(2, 3));
        Assert.True(panel.ColumnCount == 2 && panel.RowCount == 3 && panel.AutoSize);
    }

    [Fact]
    private void MustNotBeNull_ColumnStyles()
    {
        var panel = new TableLayoutPanel();

        Assert.Throws<ArgumentNullException>(() => panel.ColumnStyles(null!));
    }

    [Fact]
    private void MustNotBeNull_RowStyles()
    {
        var panel = new TableLayoutPanel();

        Assert.Throws<ArgumentNullException>(() => panel.RowStyles(null!));
    }
}