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

using System.Collections.Generic;
using System.Windows.Forms;
using AccessBridgeExplorer.Utils;

namespace AccessBridgeExplorer {
  public class TreeListView {
    private readonly ListView _listView;
    private readonly ExpandedNodeState _nodeState = new ExpandedNodeState();
    private TreeNodeCollection _currentNodes;

    /// <summary>
    /// Setup event handlers and image list. Can be called in the Forms
    /// constructor, just after <code>InitializeComponent</code>.
    /// </summary>
    public TreeListView(ListView listView, ImageList stateImageList) {
      _listView = listView;

      listView.FullRowSelect = true;
      listView.GridLines = true;
      listView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      listView.MultiSelect = false;
      listView.SmallImageList = stateImageList;
      listView.UseCompatibleStateImageBehavior = false;
      listView.View = View.Details;
      listView.HideSelection = false;

      listView.MouseClick += ListViewOnMouseClick;
      listView.MouseDoubleClick += ListViewOnMouseDoubleClick;
      listView.KeyDown += ListViewOnKeyDown;
    }

    /// <summary>
    /// Set a new <paramref name="nodeCollection"/> to be displayed in the <see
    /// cref="ListView"/>.
    /// </summary>
    public void SetNodes(TreeNodeCollection nodeCollection) {
      _listView.BeginUpdate();
      try {
        _currentNodes = nodeCollection;
        UpdateListView();
      } finally {
        _listView.EndUpdate();
      }
    }

    /// <summary>
    /// Remove the current <see cref="TreeNodeCollection"/> and remove all entries
    /// displayed in the <see cref="ListView"/>.
    /// </summary>
    public void Clear() {
      _listView.Items.Clear();
    }

    /// <summary>
    /// Update the list view contents minimally after a group has been
    /// expanded/collapsed or any other change to <see cref="_currentNodes"/>.
    /// </summary>
    private void UpdateListView() {
      _listView.BeginUpdate();
      try {
        var newItems = CreateListViewItems(_currentNodes, _nodeState);
        var oldItems = _listView.Items.AsList();
        ListHelpers.IncrementalUpdate(oldItems, newItems, new ListViewOperations(this));
      } finally {
        _listView.EndUpdate();
      }
    }

    /// <summary>
    /// Create all list view items corresponding to all the properties and property groups
    /// that should be displayed in the list view. Children of property groups that are not
    /// expanded are skipped -- they will be created when the groups are expanded.
    /// </summary>
    private List<ListViewItem> CreateListViewItems(TreeNodeCollection nodes, ExpandedNodeState expandedNodeState) {
      var itemList = new List<ListViewItem>();
      nodes.AsList().ForEach(x => {
        AddListViewItem(x, 0, "", itemList, expandedNodeState);
      });
      return itemList;
    }

    private void AddListViewItem(TreeNode treeNode, int indent, string parentPath, List<ListViewItem> itemList, ExpandedNodeState expandedNodeState) {
      var propertyNodePath = MakeNodePath(parentPath, treeNode);
      var item = new ListViewItem();
      itemList.Add(item); // Add the item before (optional) recursive call

      // Add sub-properties recursively (if group)
      if (treeNode.Nodes.Count > 0) {
        expandedNodeState.ApplyGroupState(propertyNodePath, treeNode);
        item.ImageIndex = treeNode.IsExpanded ? 1 : 0;
        if (treeNode.IsExpanded) {
          foreach (var child in treeNode.Nodes.AsList()) {
            AddListViewItem(child, indent + 1, propertyNodePath, itemList, expandedNodeState);
          }
        }
      }

      // Setup final item properties after recursion, in case property node
      // value, for example, was changed as a side effect of the recursive call.
      item.Text = treeNode.Text;
      item.Tag = new ListViewItemTag(treeNode, propertyNodePath);
      item.IndentCount = indent;
    }

    private class ListViewOperations : IncrementalUpdateOperations<ListViewItem, ListViewItem> {
      private readonly TreeListView _listView;

      public ListViewOperations(TreeListView listView) {
        _listView = listView;
      }

      public override int FindItem(IList<ListViewItem> items, int startIndex, ListViewItem newItem) {
        return FindIndexOfTag(items, startIndex, newItem.Tag);
      }

      public override void InsertItem(IList<ListViewItem> items, int index, ListViewItem newItem) {
        _listView.InsertListViewItem(items, index, newItem);
      }

      public override void UpdateItem(IList<ListViewItem> items, int index, ListViewItem newItem) {
        _listView.UpdateListViewItem(items[index], newItem);
      }

      private int FindIndexOfTag(IList<ListViewItem> oldItems, int startIndex, object tag) {
        var itemTag = (ListViewItemTag)tag;
        for (var index = startIndex; index < oldItems.Count; index++) {
          var oldItemTag = (ListViewItemTag)oldItems[index].Tag;
          if (Equals(itemTag.Path, oldItemTag.Path))
            return index;
        }
        return -1;
      }
    }

    private void InsertListViewItem(IList<ListViewItem> oldItems, int oldInsertionIndex, ListViewItem newItem) {
      oldItems.Insert(oldInsertionIndex, newItem);
    }

    private void UpdateListViewItem(ListViewItem oldItem, ListViewItem newItem) {
      // Note: For performance reason, we only assign values if they have changed
      if (oldItem.ImageIndex != newItem.ImageIndex)
        oldItem.ImageIndex = newItem.ImageIndex;

      if (oldItem.StateImageIndex != newItem.StateImageIndex)
        oldItem.StateImageIndex = newItem.StateImageIndex;

      if (oldItem.IndentCount != newItem.IndentCount)
        oldItem.IndentCount = newItem.IndentCount;

      if (!ReferenceEquals(oldItem.Tag, newItem.Tag)) {
        oldItem.Tag = newItem.Tag;
      }

      // SubItem[0] is the same as the Text property. So we just need to
      // synchronize the SubItems collections.
      for (var i = 0; i < newItem.SubItems.Count; i++) {
        var newText = newItem.SubItems[i].Text;
        if (i < oldItem.SubItems.Count) {
          if (oldItem.SubItems[i].Text != newText)
            oldItem.SubItems[i].Text = newText;
        } else {
          oldItem.SubItems.Add(newText);
        }
      }
      // If there were more subitems in the existing item, remove them
      for (var i = oldItem.SubItems.Count - 1; i >= newItem.SubItems.Count; i--) {
        oldItem.SubItems.RemoveAt(i);
      }
    }

    private static string MakeNodePath(string path, TreeNode node) {
      var text = node.Text.Replace('\\', '-');
      if (string.IsNullOrEmpty(path))
        return text;
      return path + "\\" + text;
    }

    /// <summary>
    /// Toggle expanded/collapsed state of the selecte list view item if
    /// left/right keys are pressed and the item is a group.
    /// </summary>
    private void ListViewOnKeyDown(object sender, KeyEventArgs e) {
      var listView = (ListView)sender;
      if (listView.SelectedItems.Count == 0)
        return;

      var item = listView.SelectedItems[0];
      var itemState = (ListViewItemTag)item.Tag;
      var group = itemState.TreeNode;
      if (group == null)
        return;

      switch (e.KeyCode) {
        case Keys.Add:
        case Keys.Right:
          if (!group.IsExpanded)
            TogglePropertyExpanded(item);
          break;

        case Keys.Subtract:
        case Keys.Left:
          if (group.IsExpanded)
            TogglePropertyExpanded(item);
          break;
      }
    }

    /// <summary>
    /// Toggle the expanded/collapsed state of the list view item target of the
    /// double click event.
    /// </summary>
    private void ListViewOnMouseDoubleClick(object sender, MouseEventArgs e) {
      var listView = (ListView)sender;
      var info = listView.HitTest(e.Location);
      if (info.Location == ListViewHitTestLocations.None)
        return;

      TogglePropertyExpanded(info.Item);
    }

    /// <summary>
    /// Toggle expanded/collapsed state if mouse click on the image associated
    /// with the list view item target of the click event.
    /// </summary>
    private void ListViewOnMouseClick(object sender, MouseEventArgs e) {
      var listView = (ListView)sender;
      var info = listView.HitTest(e.Location);
      if (info.Location != ListViewHitTestLocations.Image)
        return;

      TogglePropertyExpanded(info.Item);
    }

    private void TogglePropertyExpanded(ListViewItem item) {
      var itemState = (ListViewItemTag)item.Tag;
      var group = itemState.TreeNode;
      if (group == null)
        return;

      if (group.IsExpanded)
        group.Collapse();
      else
        group.Expand();
      _nodeState.SetGroupState(itemState.Path, group.IsExpanded);
      UpdateListView();
    }

    /// <summary>
    /// State associated with each item in the list view. The state is stored in
    /// the <see cref="ListViewItem.Tag"/> property.
    /// </summary>
    private class ListViewItemTag {
      private readonly TreeNode _treeNode;
      private readonly string _path;

      public ListViewItemTag(TreeNode treeNode, string path) {
        _treeNode = treeNode;
        _path = path ?? "";
      }

      public TreeNode TreeNode {
        get { return _treeNode; }
      }

      public string Path {
        get { return _path; }
      }
    }

    /// <summary>
    /// Store the expanded/collapsed state of each group of the property list so
    /// that when switching to a new property list, we can re-apply the
    /// expanded/collapsed state to corresponding group in the new list.
    /// </summary>
    public class ExpandedNodeState {
      private readonly Dictionary<string, bool> _expandedStates = new Dictionary<string, bool>();

      public void SetGroupState(string groupPath, bool expanded) {
        _expandedStates[groupPath] = expanded;
      }

      public void ApplyGroupState(string groupPath, TreeNode group) {
        bool savedState;
        if (_expandedStates.TryGetValue(groupPath, out savedState)) {
          if (savedState) {
            group.Expand();
          } else {
            group.Collapse();
          }
        }
      }
    }
  }
}