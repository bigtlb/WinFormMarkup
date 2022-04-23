namespace WinFormMarkup.Extensions;

public static class SaveFileDialogExtensions
{
    public static SaveFileDialog CreatePrompt(
        this SaveFileDialog dialog,
        bool createPrompt)
    {
        dialog.CreatePrompt = createPrompt;
        return dialog;
    }

    public static SaveFileDialog OverwritePrompt(
        this SaveFileDialog dialog,
        bool overwritePrompt)
    {
        dialog.OverwritePrompt = overwritePrompt;
        return dialog;
    }
}