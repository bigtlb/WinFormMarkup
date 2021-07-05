using System;
using System.Windows.Forms;

namespace WinFormMarkup.Extensions
{
    /// <summary>
    /// Fluent extensions for CommonDialogs
    /// </summary>
    public static class CommonDialogExtensions
    {
        /// <summary>
        /// Assign property in a fluent manner
        /// </summary>
        /// <param name="dialog"></param>
        /// <param name="action"></param>
        /// <typeparam name="TCommonDialog"></typeparam>
        /// <returns></returns>
        public static TCommonDialog HelpRequested<TCommonDialog>(
            this TCommonDialog dialog,
            Action<TCommonDialog> action)
            where TCommonDialog : CommonDialog
        {
            dialog.HelpRequest += (sender, _) => action.Invoke((sender as TCommonDialog)!);
            return dialog;
        }

        public static DialogInfo<TCommonDialog> Show<TCommonDialog>(
            this TCommonDialog dialog)
            where TCommonDialog : CommonDialog
        {
            return new(dialog, dialog.ShowDialog());
        }
    }

    public record DialogInfo<TDialog>(TDialog Dialog, DialogResult Result);
}