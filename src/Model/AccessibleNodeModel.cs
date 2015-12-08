using System.Linq;
using System.Windows.Forms;
using AccessBridgeExplorer.WindowsAccessBridge;

namespace AccessBridgeExplorer.Model {
  public class AccessibleNodeModel : NodeModel {
    private readonly AccessibleNode _accessibleNode;

    public AccessibleNodeModel(AccessibleNode accessibleNode) {
      _accessibleNode = accessibleNode;
    }

    public AccessibleNode AccessibleNode {
      get { return _accessibleNode; }
    }

    public override void AddChildren(TreeNode node) {
      _accessibleNode.GetChildren()
        .Select(x => new AccessibleNodeModel(x))
        .ForEach(x => {
          node.Nodes.Add(x.CreateTreeNode());
        });
    }

    public override void SetupTreeNode(TreeNode node) {
      var childrenCount = _accessibleNode.GetChildrenCount();
      if (childrenCount > 0) {
        AddFakeChild(node);
      }

      node.Text = _accessibleNode.GetTitle();
    }
  }
}