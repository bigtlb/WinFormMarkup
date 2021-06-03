using System.Windows.Forms;

namespace WinFormMarkup.Extensions
{
    public static class FormExtensions
    {
        public static TForm StartPosition<TForm>(
            this TForm form,
            FormStartPosition startPosition)
            where TForm : Form
        {
            form.StartPosition = startPosition;
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
            if (autoSize)
            {
                form.AutoSizeMode = mode;
            }

            return form;
        }
    }
}