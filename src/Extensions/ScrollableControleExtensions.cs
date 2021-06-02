using System.Windows.Forms;

namespace WinFormMarkup.Extensions
{
    public static class ScrollableControleExtensions
    {
        public static TControl AutoScroll<TControl>(
            this TControl control,
            bool autoScroll)
            where TControl : ScrollableControl
        {
            control.AutoScroll = autoScroll;
            return control;
        }
    }
}