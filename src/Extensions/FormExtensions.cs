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
    }
}