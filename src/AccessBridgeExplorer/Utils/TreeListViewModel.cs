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
  /// Model to use with a <see cref="TreeListView"/>.
  /// </summary>
  public abstract class TreeListViewModel {
    /// <summary>
    /// Return the root node of the model.
    /// </summary>
    public abstract object GetRootNode();

    /// <summary>
    /// Return <code>true</code> is the root node should be displayed at the top
    /// level entry. Return <code>false</code> if the children of the root node
    /// should be displayed as the top level entries.
    /// </summary>
    public abstract bool IsRootVisible();

    /// <summary>
    /// Return the number of children of a given <paramref name="modelNode"/>.
    /// </summary>
    public abstract int GetChildrenCount(object modelNode);

    /// <summary>
    /// Return the child at <paramref name="index"/> of a given <paramref name="modelNode"/>.
    /// </summary>
    public abstract object GetChildAt(object modelNode, int index);

    /// <summary>
    /// Return the expandable state of a given <paramref name="modelNode"/>.
    /// </summary>
    public abstract bool IsNodeExpandable(object modelNode);

    /// <summary>
    /// Return the initial expanded state of a given <paramref name="modelNode"/>.
    /// </summary>
    public virtual bool IsNodeExpanded(object modelNode) {
      return false;
    }

    /// <summary>
    /// Return the text of a given <paramref name="modelNode"/>.
    /// </summary>
    public abstract string GetNodeText(object modelNode);

    /// <summary>
    /// Return the path of a given <paramref name="modelNode"/>.
    /// </summary>
    public virtual string GetNodePath(object modelNode) {
      return GetNodeText(modelNode);
    }
  }
}