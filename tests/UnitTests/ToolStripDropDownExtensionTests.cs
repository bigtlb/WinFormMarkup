using System.Windows.Forms;
using WinFormMarkup.Extensions;
using Xunit;

namespace UnitTests
{
    public class ToolStripDropDownExtensionTests
    {
        [Fact]
        private void CanSet_DropDownItems()
        {
            var item = new ToolStripMenuItem("Test");

            ToolStripMenuItem one;
            ToolStripMenuItem two;
            ToolStripMenuItem three;
            Assert.Equal(item, item.DropDownItems(
                one = new ToolStripMenuItem("One"),
                two = new ToolStripMenuItem("Two"),
                three = new ToolStripMenuItem("Three")));

            Assert.True(item.DropDownItems[0] == one && item.DropDownItems[1] == two && item.DropDownItems[2] == three);
        }
    }
}