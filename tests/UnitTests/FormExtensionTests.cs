using System.Windows.Forms;
using WinFormMarkup.Extensions;
using Xunit;

namespace UnitTests;

public class FormExtensionTests
{
    [Fact]
    private void CanSet_AcceptButton()
    {
        Button acceptButton;
        var form = new Form().Controls(acceptButton = new Button());

        Assert.Equal(form, form.AcceptButton(acceptButton));
        Assert.Equal(acceptButton, form.AcceptButton);
    }

    [Fact]
    private void CanSet_AutoSize_WithAutoGrow()
    {
        var form = new Form();

        Assert.Equal(form, form.AutoSize(true, AutoSizeMode.GrowAndShrink));
        Assert.True(form.AutoSize);
        Assert.Equal(AutoSizeMode.GrowAndShrink, form.AutoSizeMode);
    }

    [Fact]
    private void CanSet_CancelButton()
    {
        Button cancelButton;
        var form = new Form().Controls(cancelButton = new Button());

        Assert.Equal(form, form.CancelButton(cancelButton));
        Assert.Equal(cancelButton, form.CancelButton);
    }


    [Fact]
    private void CanSet_Position()
    {
        var form = new Form();

        Assert.Equal(form, form.StartPosition(FormStartPosition.CenterScreen));
        Assert.Equal(FormStartPosition.CenterScreen, form.StartPosition);
    }
}