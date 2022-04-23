using System.Windows.Forms;
using WinFormMarkup.Extensions;
using Xunit;

namespace UnitTests;

public class FlowLayoutExtensionTests
{
    [Fact]
    private void CanSet_FlowDirection()
    {
        var panel = new FlowLayoutPanel();

        Assert.Equal(panel, panel.FlowDirection(FlowDirection.TopDown));

        Assert.Equal(FlowDirection.TopDown, panel.FlowDirection);
    }

    [Fact]
    private void CanSet_WrapContents()
    {
        var panel = new FlowLayoutPanel();

        Assert.Equal(panel, panel.WrapContents(false));

        Assert.False(panel.WrapContents);
    }
}