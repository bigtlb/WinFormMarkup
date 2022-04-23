using System.Windows.Forms;
using WinFormMarkup.Extensions;
using Xunit;

namespace UnitTests;

public class DateTimePickerExtensionTests
{
    [Fact]
    private void CanSet_CustomFormat()
    {
        var picker = new DateTimePicker();

        Assert.Equal(picker, picker.CustomFormat("dd/MM/yyyy"));

        Assert.Equal("dd/MM/yyyy", picker.CustomFormat);
        Assert.Equal(DateTimePickerFormat.Custom, picker.Format);
    }

    [Fact]
    private void CanSet_Format()
    {
        var picker = new DateTimePicker();

        Assert.Equal(picker, picker.Format(DateTimePickerFormat.Long));

        Assert.Equal(DateTimePickerFormat.Long, picker.Format);
    }
}