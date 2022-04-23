namespace WinFormMarkup.Extensions;

public static class ScrollableControlExtensions
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