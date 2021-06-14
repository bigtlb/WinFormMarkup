using System;
using System.Windows.Forms;

namespace WinFormMarkup.Extensions
{
    public static class CommonDialogExtensions
    {
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