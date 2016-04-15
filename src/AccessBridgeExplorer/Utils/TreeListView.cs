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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AccessBridgeExplorer.Utils {
  /// <summary>
  /// Display a tree structure based on a <see cref="TreeListViewModel"/> in a
  /// <see cref="ListView"/>.
  /// </summary>
  public class TreeListView {
    private TreeListViewModel _currentModel;
    private readonly ListView _listView;
    private readonly ExpandedNodeState _nodeState = new ExpandedNodeState();

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

    public event EventHandler<ModelNodeArgs> ModelNodeShow;
    public event EventHandler<ModelNodeArgs> ModelNodeHide;
    public event EventHandler<ModelNodeArgs> ModelNodeExpand;
    public event EventHandler<ModelNodeArgs> ModelNodeCollapse;

    public IList<object> SelectedModelNodes {
      get {
        return _listView
          .SelectedItems
          .Cast<ListViewItem>()
          .Select(x => x.Tag)
          .OfType<ListViewItemTag>()
          .Select(x => x.ModelNode)
          .Where(x => x != null)
          .ToList();
      }
    }

    /// <summary>
    /// Set a new <paramref name="model"/> to be displayed in the <see
    /// cref="ListView"/>.
    /// </summary>
    public void SetModel(TreeListViewModel model) {
      _listView.BeginUpdate();
      try {
        _currentModel = model;
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
    /// expanded/collapsed.
    /// </summary>
    private void UpdateListView() {
      _listView.BeginUpdate();
      try {
        var oldItems = _listView.Items.AsList();
        var oldModelNodes = new HashSet<object>(oldItems.Select(x => ((ListViewItemTag)x.Tag).ModelNode));

        var newItems = CreateListViewItems(_currentModel, _nodeState);
        var newModelNodes = new HashSet<object>(newItems.Select(x => ((ListViewItemTag)x.Tag).ModelNode));

        ListHelpers.IncrementalUpdate(oldItems, newItems, new ListViewOperations());

        // Fire events related to adding/removing nodes from the display
        // inserted = newItems \ oldItems
        // removed = oldItems \ newItems
        foreach (var modelNode in newModelNodes) {
          if (!oldModelNodes.Contains(modelNode))
            OnModelNodeShow(new ModelNodeArgs(modelNode));
        }
        foreach (var modelNode in oldModelNodes) {
          if (!newModelNodes.Contains(modelNode))
            OnModelNodeHide(new ModelNodeArgs(modelNode));
        }
      } finally {
        _listView.EndUpdate();
      }
    }

    /// <summary>
    /// Create all list view items corresponding to all the properties and property groups
    /// that should be displayed in the list view. Children of property groups that are not
    /// expanded are skipped -- they will be created when the groups are expanded.
    /// </summary>
    private List<ListViewItem> CreateListViewItems(TreeListViewModel model, ExpandedNodeState expandedNodeState) {
      var itemList = new List<ListViewItem>();

      if (model.IsRootVisible()) {
        CreateListViewItem(itemList, expandedNodeState, "", 0, model, model.GetRootNode());
      } else {
        Enumerable.Range(0, model.GetChildrenCount(model.GetRootNode())).ForEach(index => {
          CreateListViewItem(itemList, expandedNodeState, "", 0, model, model.GetChildAt(model.GetRootNode(), index));
        });
      }
      return itemList;
    }

    private void CreateListViewItem(List<ListViewItem> itemList, ExpandedNodeState expandedNodeState, string parentPath, int indent, TreeListViewModel model, object modelNode) {
      var propertyNodePath = MakeNodePath(model, parentPath, modelNode);
      var item = new ListViewItem();
      itemList.Add(item); // Add the item before (optional) recursive call

      // Add sub-properties recursively (if group)
      if (model.IsNodeExpandable(modelNode)) {
        var isExpanded = expandedNodeState.IsExpanded(model, modelNode, propertyNodePath);
        item.ImageIndex = isExpanded ? 1 : 0;
        if (isExpanded) {
          Enumerable.Range(0, model.GetChildrenCount(modelNode)).ForEach(index => {
            CreateListViewItem(itemList, expandedNodeState, propertyNodePath, indent + 1, model, model.GetChildAt(modelNode, index));
          });
        }
      }

      // Setup final item properties after recursion, in case property node
      // value, for example, was changed as a side effect of the recursive call.
      item.Text = model.GetNodeText(modelNode);
      var subItemCount = model.GetNodeSubItemCount(modelNode);
      for (var subItem = 0; subItem < subItemCount; subItem++) {
        item.SubItems.Add(model.GetNodeSubItemAt(modelNode, subItem));
      }
      item.Tag = new ListViewItemTag(modelNode, propertyNodePath);
      item.IndentCount = indent;
    }

    private class ListViewOperations : IncrementalUpdateOperations<ListViewItem, ListViewItem> {
      public override int FindItem(IList<ListViewItem> items, int startIndex, ListViewItem newItem) {
        return FindIndexOfTag(items, startIndex, newItem.Tag);
      }

      public override void InsertItem(IList<ListViewItem> items, int index, ListViewItem newItem) {
        items.Insert(index, newItem);
      }

      public override void UpdateItem(IList<ListViewItem> items, int index, ListViewItem newItem) {
        ListViewItem oldItem = items[index];
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

    private static string MakeNodePath(TreeListViewModel model, string path, object modelNode) {
      var text = model.GetNodePath(modelNode).Replace('\\', '-');
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
      var modelNode = itemState.ModelNode;
      if (modelNode == null)
        return;

      switch (e.KeyCode) {
        case Keys.Add:
        case Keys.Right:
          if (!_nodeState.IsExpanded(_currentModel, modelNode, itemState.Path))
            ToggleExpanded(item);
          break;

        case Keys.Subtract:
        case Keys.Left:
          if (_nodeState.IsExpanded(_currentModel, modelNode, itemState.Path))
            ToggleExpanded(item);
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

      ToggleExpanded(info.Item);
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

      ToggleExpanded(info.Item);
    }

    private void ToggleExpanded(ListViewItem item) {
      var itemState = (ListViewItemTag)item.Tag;
      var modelNode = itemState.ModelNode;
      if (modelNode == null)
        return;

      var isExpanded = _nodeState.IsExpanded(_currentModel, modelNode, itemState.Path);
      _nodeState.SetExpanded(itemState.Path, !isExpanded);
      if (isExpanded)
        OnModelNodeCollapse(new ModelNodeArgs(modelNode));
      else
        OnModelNodeExpand(new ModelNodeArgs(modelNode));
      UpdateListView();
    }

    /// <summary>
    /// State associated with each item in the list view. The state is stored in
    /// the <see cref="ListViewItem.Tag"/> property.
    /// </summary>
    private class ListViewItemTag {
      private readonly object _modelNode;
      private readonly string _path;

      public ListViewItemTag(object modelNode, string path) {
        _modelNode = modelNode;
        _path = path ?? "";
      }

      public object ModelNode {
        get { return _modelNode; }
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

      public void SetExpanded(string path, bool expanded) {
        _expandedStates[path] = expanded;
      }

      public bool IsExpanded(TreeListViewModel model, object modelNode, string path) {
        bool savedState;
        if (_expandedStates.TryGetValue(path, out savedState)) {
          return savedState;
        }
        return model.IsNodeExpanded(modelNode);
      }
    }

    protected virtual void OnModelNodeShow(ModelNodeArgs e) {
      var handler = ModelNodeShow;
      if (handler != null) handler(this, e);
    }

    protected virtual void OnModelNodeHide(ModelNodeArgs e) {
      var handler = ModelNodeHide;
      if (handler != null) handler(this, e);
    }

    protected virtual void OnModelNodeExpand(ModelNodeArgs e) {
      var handler = ModelNodeExpand;
      if (handler != null) handler(this, e);
    }

    protected virtual void OnModelNodeCollapse(ModelNodeArgs e) {
      var handler = ModelNodeCollapse;
      if (handler != null) handler(this, e);
    }
  }
}