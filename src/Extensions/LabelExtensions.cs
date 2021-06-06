using System.Drawing;
using System.Windows.Forms;

namespace WinFormMarkup.Extensions
{
    public static class LabelExtensions
    {
        public static TLabel TextAlign<TLabel>(
            this TLabel label,
            ContentAlignment alignment)
            where TLabel : Label
        {
            label.TextAlign = alignment;
            return label;
        }
    }
}