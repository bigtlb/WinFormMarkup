namespace WinFormMarkup.Extensions;

public static class FolderBrowserDialogExtension
{
    public static FolderBrowserDialog Description(
        this FolderBrowserDialog dialog,
        string description)
    {
        dialog.Description = description;
        return dialog;
    }

    public static FolderBrowserDialog RootFolder(
        this FolderBrowserDialog dialog,
        Environment.SpecialFolder rootFolder)
    {
        dialog.RootFolder = rootFolder;
        return dialog;
    }

    public static FolderBrowserDialog SelectedPath(
        this FolderBrowserDialog dialog,
        string selectedPath)
    {
        dialog.SelectedPath = selectedPath;
        return dialog;
    }

    public static FolderBrowserDialog ShowNewFolderButton(
        this FolderBrowserDialog dialog,
        bool newFolderButton)
    {
        dialog.ShowNewFolderButton = newFolderButton;
        return dialog;
    }

    public static FolderBrowserDialog UseDescriptionForTitle(
        this FolderBrowserDialog dialog,
        bool descriptionForTitle)
    {
        dialog.UseDescriptionForTitle = descriptionForTitle;
        return dialog;
    }
}