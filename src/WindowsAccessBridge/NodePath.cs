using System.Collections.Generic;
using System.Linq;

namespace AccessBridgeExplorer.WindowsAccessBridge {
  public class NodePath {
    private readonly Stack<AccessibleNode> _nodes = new Stack<AccessibleNode>();

    public int Count {
      get { return _nodes.Count; }
    }

    public AccessibleNode LeafNode {
      get { return _nodes.Last(); }
    }

    public void AddParent(AccessibleNode accessibleNode) {
      _nodes.Push(accessibleNode);
    }

    public AccessibleNode Pop() {
      return _nodes.Pop();
    }
  }
}