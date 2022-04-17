namespace WinFormMarkup.Extensions;

public static class TextBoxExtension
{
    public static TTextBox Multiline<TTextBox>(this TTextBox textBox, bool multiLine) where TTextBox : TextBox
    {
        textBox.Multiline = multiLine;
        return textBox;
    }
}