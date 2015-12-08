using System.Diagnostics;
using System.Windows.Forms;

namespace AccessBridgeExplorer.Model {
  public abstract class NodeModel {
    private static readonly FakeChildNodeModel FakeChildModel = new FakeChildNodeModel();
    private TreeNode _treeNode;

    private class FakeChildNodeModel : NodeModel {
    }

    public TreeNode CreateTreeNode() {
      var treeNode = new TreeNode();
      treeNode.Tag = this;
      SetupTreeNode(treeNode);
      _treeNode = treeNode;
      return treeNode;
    }

    protected void AddFakeChild(TreeNode node) {
      var fakeChild = new TreeNode();
      fakeChild.Tag = FakeChildModel;
      node.Nodes.Add(fakeChild);
    }

    public virtual void SetupTreeNode(TreeNode node) { }

    public void BeforeExpand(object sender, TreeViewCancelEventArgs e) {
      Debug.Assert(ReferenceEquals(_treeNode, e.Node));
      if (_treeNode.Nodes.Count == 1 && _treeNode.Nodes[0].Tag == FakeChildModel) {
        _treeNode.Nodes.Clear();
        AddChildren(e.Node);
      }
    }

    public virtual void AddChildren(TreeNode node) {
    }

    public void ResetChildren(TreeNode treeNode) {
      _treeNode.Nodes.Clear();
      SetupTreeNode(treeNode);
    }
  }
}
