namespace WinFormMarkup.Extensions;

public static class SplitterControlExtension
{
    public static TSplitContainer Panel1<TSplitContainer>(this TSplitContainer splitContainer, params Control[] controls) where TSplitContainer : SplitContainer
    {
        splitContainer.Panel1.Controls(controls);
        return splitContainer;
    }
    public static TSplitContainer Panel2<TSplitContainer>(this TSplitContainer splitContainer, params Control[] controls) where TSplitContainer : SplitContainer
    {
        splitContainer.Panel2.Controls(controls);
        return splitContainer;
    }
    public static TSplitContainer SplitterDistance<TSplitContainer>(this TSplitContainer splitContainer, int distance) where TSplitContainer : SplitContainer
    {
        splitContainer.SplitterDistance = distance;
        return splitContainer;
    }
}