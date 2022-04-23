namespace WinFormMarkup.Extensions;

public static class TreeViewExtension
{
    public static TTreeView Nodes<TTreeView>(this TTreeView treeView, params TreeNode[] nodes)
        where TTreeView : TreeView
    {
        treeView.Nodes.AddRange(nodes);
        return treeView;
    }

    public static TTreeView OnAfterSelect<TTreeView>(this TTreeView treeView, Action<TreeNode> action)
        where TTreeView : TreeView
    {
        treeView.AfterSelect += (sender, e) => action(e.Node);
        return treeView;
    }
}