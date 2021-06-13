using System.Windows.Forms;
using WinFormMarkup.Extensions;
using Xunit;

namespace UnitTests
{
    public class ScrollableControlExtensionTests
    {
        [Fact]
        private void CanSet_AuoScroll()
        {
            var ctl = new ScrollableControl();

            Assert.Equal(ctl, ctl.AutoScroll(true));

            Assert.True(ctl.AutoScroll);
        }
    }
}