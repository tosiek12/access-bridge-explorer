// Copyright 2015 Google Inc. All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WindowsAccessBridgeInterop {
  /// <summary>
  /// Represents a path from a root node to a leaf node.
  /// </summary>
  public class AccessibleNodePath : IEnumerable<AccessibleNode> {
    private readonly List<AccessibleNode> _nodes = new List<AccessibleNode>();

    public AccessibleNode LeafNode {
      get {
        return _nodes.LastOrDefault();
      }
    }

    public void AddParent(AccessibleNode accessibleNode) {
      _nodes.Insert(0, accessibleNode);
    }

    public AccessibleNodePathCursor CreateCursor() {
      return new AccessibleNodePathCursor(_nodes, 0, _nodes.Count);
    }

    public IEnumerator<AccessibleNode> GetEnumerator() {
      return _nodes.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }
  }
}