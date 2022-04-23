namespace WinFormMarkup.Extensions;

public static class FlowLayoutPanelExtensions
{
    public static TPanel FlowDirection<TPanel>(
        this TPanel panel,
        FlowDirection direction)
        where TPanel : FlowLayoutPanel
    {
        panel.FlowDirection = direction;
        return panel;
    }

    public static TPanel WrapContents<TPanel>(
        this TPanel panel,
        bool wrap)
        where TPanel : FlowLayoutPanel
    {
        panel.WrapContents = wrap;
        return panel;
    }
}