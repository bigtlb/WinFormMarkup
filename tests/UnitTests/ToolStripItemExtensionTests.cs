using System.Windows.Forms;
using WinFormMarkup.Extensions;
using Xunit;

namespace UnitTests;

public class ToolStripItemExtensionTests
{
    [Fact]
    private void CanSet_Text()
    {
        var menuItem = new ToolStripMenuItem();

        Assert.Equal(menuItem, menuItem.Text("One"));
        Assert.Equal("One", menuItem.Text);
    }
}