using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace WinFormMarkup.Extensions
{
    /// <summary>
    ///     Fluent extensions for FileDialogs
    ///     <para>Inherits: <see cref="WinFormMarkup.Extensions.CommonDialogExtensions" /></para>
    /// </summary>
    public static class FileDialogExtensions
    {
        public static TFileDialog CheckFileExists<TFileDialog>(
            this TFileDialog dialog,
            bool checkExists)
            where TFileDialog : FileDialog
        {
            dialog.CheckFileExists = checkExists;
            return dialog;
        }

        public static TFileDialog CheckPathExists<TFileDialog>(
            this TFileDialog dialog,
            bool checkExists)
            where TFileDialog : FileDialog
        {
            dialog.CheckPathExists = checkExists;
            return dialog;
        }


        public static TFileDialog CustomPlaces<TFileDialog>(
            this TFileDialog dialog,
            params string[] places)
            where TFileDialog : FileDialog
        {
            foreach (var place in places) dialog.CustomPlaces.Add(place);

            return dialog;
        }

        public static TFileDialog CustomPlaces<TFileDialog>(
            this TFileDialog dialog,
            params Guid[] places)
            where TFileDialog : FileDialog
        {
            foreach (var place in places) dialog.CustomPlaces.Add(place);

            return dialog;
        }


        public static TFileDialog CustomPlaces<TFileDialog>(
            this TFileDialog dialog,
            params FileDialogCustomPlace[] places)
            where TFileDialog : FileDialog
        {
            foreach (var place in places) dialog.CustomPlaces.Add(place);

            return dialog;
        }

        public static TFileDialog DefaultExt<TFileDialog>(
            this TFileDialog dialog,
            string defaultExt)
            where TFileDialog : FileDialog
        {
            dialog.DefaultExt = defaultExt;
            return dialog;
        }

        public static TFileDialog FileName<TFileDialog>(
            this TFileDialog dialog,
            string fileName)
            where TFileDialog : FileDialog
        {
            dialog.FileName = fileName;
            return dialog;
        }

        public static TFileDialog FileOk<TFileDialog>(
            this TFileDialog dialog,
            Action<TFileDialog, CancelEventArgs> action)
            where TFileDialog : FileDialog
        {
            dialog.FileOk += (sender, args) => action.Invoke((sender as TFileDialog)!, args);
            return dialog;
        }

        public static TFileDialog Filter<TFileDialog>(
            this TFileDialog dialog,
            string filter)
            where TFileDialog : FileDialog
        {
            dialog.Filter = filter;
            return dialog;
        }

        public static TFileDialog InitialDirectory<TFileDialog>(
            this TFileDialog dialog,
            string initialPath)
            where TFileDialog : FileDialog
        {
            dialog.InitialDirectory = initialPath;
            return dialog;
        }

        public static TFileDialog RestoreDirectory<TFileDialog>(
            this TFileDialog dialog,
            bool restore)
            where TFileDialog : FileDialog
        {
            dialog.RestoreDirectory = restore;
            return dialog;
        }

        public static TFileDialog ShowHelp<TFileDialog>(
            this TFileDialog dialog,
            bool showHelp)
            where TFileDialog : FileDialog
        {
            dialog.ShowHelp = showHelp;
            return dialog;
        }


        public static TFileDialog SupportMultiDottedExtensions<TFileDialog>(
            this TFileDialog dialog,
            bool multiDot)
            where TFileDialog : FileDialog
        {
            dialog.SupportMultiDottedExtensions = multiDot;
            return dialog;
        }

        public static TFileDialog Title<TFileDialog>(
            this TFileDialog dialog,
            string title)
            where TFileDialog : FileDialog
        {
            dialog.Title = title;
            return dialog;
        }

        public static TFileDialog ValidateNames<TFileDialog>(
            this TFileDialog dialog,
            bool validate)
            where TFileDialog : FileDialog
        {
            dialog.ValidateNames = validate;
            return dialog;
        }
    }
}