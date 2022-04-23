using System.Windows.Forms;
using WinFormMarkup.Extensions;
using Xunit;

namespace UnitTests;

public class ToolStripExtensionTests
{
    [Fact]
    private void CanSet_Items()
    {
        var strip = new ToolStrip();

        Assert.Equal(strip, strip.Items(
            new ToolStripMenuItem("One"),
            new ToolStripMenuItem("Two")));

        Assert.True(strip.Items.Count == 2 &&
                    strip.Items[0].Text == "One" &&
                    strip.Items[1].Text == "Two");
    }
}