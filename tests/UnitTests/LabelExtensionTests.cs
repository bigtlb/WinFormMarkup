using System.Drawing;
using System.Windows.Forms;
using WinFormMarkup.Extensions;
using Xunit;

namespace UnitTests
{
    public class LabelExtensionTests
    {
        [Fact]
        private void CanSet_TextAlign()
        {
            var label = new Label();

            Assert.Equal(label, label.TextAlign(ContentAlignment.MiddleRight));

            Assert.Equal(ContentAlignment.MiddleRight, label.TextAlign);
        }
    }
}