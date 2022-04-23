namespace WinFormMarkup.Extensions;

public static class TreeNodeExtension
{
    public static TTreeNode Nodes<TTreeNode>(this TTreeNode treeNode, params TreeNode[] nodes)
        where TTreeNode : TreeNode
    {
        treeNode.Nodes.AddRange(nodes);
        return treeNode;
    }
}