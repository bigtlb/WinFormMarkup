using System.Windows.Forms;
using WinFormMarkup.Extensions;
using Xunit;

namespace UnitTests
{
    public class ToolStripMenuItemExtensionTests
    {
        [Fact]
        private void CanSet_Keys()
        {
            var menuItem = new ToolStripMenuItem();

            Assert.Equal(menuItem, menuItem.Keys(Keys.ControlKey | Keys.F4));
            Assert.Equal(Keys.ControlKey | Keys.F4, menuItem.ShortcutKeys);
        }
    }
}