using System.Windows.Forms;

namespace WinFormMarkup.Extensions
{
    public static class FormExtensions
    {
        public static TForm AcceptButton<TForm>(
            this TForm form,
            Button acceptButton)
            where TForm : Form
        {
            form.AcceptButton = acceptButton;
            return form;
        }

        public static TForm AutoSize<TForm>(
            this TForm form,
            bool autoSize,
            AutoSizeMode mode
        )
            where TForm : Form
        {
            form.AutoSize = autoSize;
            if (autoSize) form.AutoSizeMode = mode;

            return form;
        }

        public static TForm CancelButton<TForm>(
            this TForm form,
            Button cancelButton)
            where TForm : Form
        {
            form.CancelButton = cancelButton;
            return form;
        }

        public static TForm StartPosition<TForm>(
            this TForm form,
            FormStartPosition startPosition)
            where TForm : Form
        {
            form.StartPosition = startPosition;
            return form;
        }
    }
}