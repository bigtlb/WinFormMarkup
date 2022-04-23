namespace WinFormMarkup.Extensions;

public static class OpenFileDialogExtensions
{
    public static OpenFileDialog Multiselect(
        this OpenFileDialog dialog,
        bool multiselect)
    {
        dialog.Multiselect = multiselect;
        return dialog;
    }

    public static OpenFileDialog ReadOnlyChecked(
        this OpenFileDialog dialog,
        bool readOnlyChecked)
    {
        dialog.ReadOnlyChecked = readOnlyChecked;
        return dialog;
    }

    public static OpenFileDialog ShowReadOnly(
        this OpenFileDialog dialog,
        bool showReadOnly)
    {
        dialog.ShowReadOnly = showReadOnly;
        return dialog;
    }
}