// Copyright 2016 Google Inc. All Rights Reserved.
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

namespace AccessBridgeExplorer.Utils {
  /// <summary>
  /// Model to use with a <see cref="TreeListView{TNode}"/>.
  /// </summary>
  public abstract class TreeListViewModel<TNode> where TNode : class {
    /// <summary>
    /// Return the root node of the model.
    /// </summary>
    public abstract TNode GetRootNode();

    /// <summary>
    /// Return <code>true</code> is the root node should be displayed at the top
    /// level entry. Return <code>false</code> if the children of the root node
    /// should be displayed as the top level entries.
    /// </summary>
    public abstract bool IsRootVisible();

    /// <summary>
    /// Return the number of children of a given <paramref name="modelNode"/>.
    /// </summary>
    public abstract int GetChildrenCount(TNode modelNode);

    /// <summary>
    /// Return the child at <paramref name="index"/> of a given <paramref name="modelNode"/>.
    /// </summary>
    public abstract TNode GetChildAt(TNode modelNode, int index);

    /// <summary>
    /// Return the expandable state of a given <paramref name="modelNode"/>.
    /// </summary>
    public abstract bool IsNodeExpandable(TNode modelNode);

    /// <summary>
    /// Return the initial expanded state of a given <paramref name="modelNode"/>.
    /// </summary>
    public virtual bool IsNodeExpanded(TNode modelNode) {
      return false;
    }

    /// <summary>
    /// Return the text of a given <paramref name="modelNode"/>.
    /// </summary>
    public abstract string GetNodeText(TNode modelNode);

    public virtual int GetNodeSubItemCount(TNode modelNode) {
      return 0;
    }

    public virtual string GetNodeSubItemAt(TNode modelNode, int index) {
      return "";
    }

    /// <summary>
    /// Return the path of a given <paramref name="modelNode"/>.
    /// </summary>
    public virtual string GetNodePath(TNode modelNode) {
      return GetNodeText(modelNode);
    }
  }
}