using System;
using System.Drawing;
using System.Linq.Expressions;
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