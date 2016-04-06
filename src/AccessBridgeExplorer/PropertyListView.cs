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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WindowsAccessBridgeInterop;
using AccessBridgeExplorer.Utils;

namespace AccessBridgeExplorer {
  /// <summary>
  /// Wraps a <see cref="ListView"/> used to display a <see
  /// cref="PropertyList"/> to handle node identation and expand/collapse. The
  /// <see cref="SetPropertyList"/> method must be called to switch to a new
  /// property list. The <see cref="Clear"/> method must be called to clear the
  /// contents of the listview.
  /// </summary>
  public class PropertyListView {
    private readonly ListView _listView;
    private readonly ExpandedNodeState _nodeState = new ExpandedNodeState();
    private PropertyList _currentPropertyList;

    /// <summary>
    /// Setup event handlers and image list. Can be called in the Forms
    /// constructor, just after <code>InitializeComponent</code>.
    /// </summary>
    public PropertyListView(ListView listView, ImageList stateImageList) {
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
      listView.SelectedIndexChanged += ListOnSelectedIndexChanged;
      listView.GotFocus += ListViewOnGotFocus;
    }

    public event EventHandler<AccessibleRectInfoSelectedEventArgs> AccessibleRectInfoSelected;
    public event EventHandler<PropertyGroupErrorEventArgs> Error;

    /// <summary>
    /// Set a new <paramref name="propertyList"/> to be displayed in the <see
    /// cref="ListView"/>.
    /// </summary>
    public void SetPropertyList(PropertyList propertyList) {
      _listView.BeginUpdate();
      try {
        _currentPropertyList = propertyList;
        UpdateListView();
      } finally {
        _listView.EndUpdate();
      }
    }

    /// <summary>
    /// Remove the current <see cref="PropertyList"/> and remove all entries
    /// displayed in the <see cref="ListView"/>.
    /// </summary>
    public void Clear() {
      _listView.Items.Clear();
    }

    private class ListViewOperations : IncrementalUpdateOperations<ListViewItem, ListViewItem> {
      private readonly PropertyListView _listView;

      public ListViewOperations(PropertyListView listView) {
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

    /// <summary>
    /// Update the list view contents minimally after a group has been
    /// expanded/collapsed or any other change to <see
    /// cref="_currentPropertyList"/>.
    /// </summary>
    private void UpdateListView() {
      _listView.BeginUpdate();
      try {
        var newItems = CreateListViewItems(_currentPropertyList, _nodeState);
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
    private static List<ListViewItem> CreateListViewItems(PropertyList propertyList, ExpandedNodeState expandedNodeState) {
      var itemList = new List<ListViewItem>();
      propertyList.ForEach(x => {
        AddListViewItem(x, 0, "", itemList, expandedNodeState);
      });
      return itemList;
    }

    private static void AddListViewItem(PropertyNode propertyNode, int indent, string parentPath, List<ListViewItem> itemList, ExpandedNodeState expandedNodeState) {
      var propertyNodePath = MakeNodePath(parentPath, propertyNode);
      var item = new ListViewItem();
      itemList.Add(item); // Add the item before (optional) recursive call

      // Add sub-properties recursively (if group)
      var propertyGroup = propertyNode as PropertyGroup;
      if (propertyGroup != null) {
        expandedNodeState.ApplyGroupState(propertyNodePath, propertyGroup);
        item.ImageIndex = (propertyGroup.Expanded ? 1 : 0);
        if (propertyGroup.Expanded) {
          foreach (var child in propertyGroup.Children) {
            AddListViewItem(child, indent + 1, propertyNodePath, itemList, expandedNodeState);
          }
        }
      }

      // Setup final item properties after recursion, in case property node
      // value, for example, was changed as a side effect of the recursive call.
      item.Text = propertyNode.Name;
      item.SubItems.Add(ValueToString(propertyNode));
      item.Tag = new ListViewItemTag(propertyNode, propertyNodePath);
      item.IndentCount = indent;
    }

    private void InsertListViewItem(IList<ListViewItem> oldItems, int oldInsertionIndex, ListViewItem newItem) {
      SetErrorHandler(newItem);
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
        SetErrorHandler(newItem);
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

    private void SetErrorHandler(ListViewItem newItem) {
      var group = newItem.Tag as PropertyGroup;
      if (group != null) {
        group.Error += (sender, args) => {
          OnError(new PropertyGroupErrorEventArgs((PropertyGroup)sender, args.GetException()));
        };
      }
    }

    private static string MakeNodePath(string path, PropertyNode node) {
      if (string.IsNullOrEmpty(path))
        return node.Name;
      return path + "\\" + node.Name;
    }

    private static string ValueToString(PropertyNode propertyNode) {
      var value = propertyNode.Value;
      string valueText;
      if (value == null) {
        if (propertyNode is PropertyGroup)
          valueText = "";
        else
          valueText = "-";
      } else if (value is bool) {
        valueText = ((bool)value) ? "Yes" : "No";
      } else if (value is string) {
        valueText = string.IsNullOrEmpty((string)value) ? "-" : (string)value;
      } else {
        valueText = value.ToString();
      }
      return valueText;
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
      var group = itemState.PropertyNode as PropertyGroup;
      if (group == null)
        return;

      switch (e.KeyCode) {
        case Keys.Add:
        case Keys.Right:
          if (!group.Expanded)
            TogglePropertyExpanded(item);
          break;

        case Keys.Subtract:
        case Keys.Left:
          if (group.Expanded)
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

    private void ListOnSelectedIndexChanged(object sender, EventArgs eventArgs) {
      var selection = _listView.SelectedItems.Cast<ListViewItem>().FirstOrDefault();
      if (selection == null)
        return;

      var rect = GetRectangleFromPropertyNode(selection);
      if (rect == null)
        return;

      if (!rect.IsVisible)
        return;

      OnAccessibleRectInfoSelected(new AccessibleRectInfoSelectedEventArgs {
        PropertyNode = ((ListViewItemTag)selection.Tag).PropertyNode,
        AccessibleRectInfo = rect,
      });
    }

    private static AccessibleRectInfo GetRectangleFromPropertyNode(ListViewItem selection) {
      var tag = selection.Tag as ListViewItemTag;
      if (tag == null)
        return null;

      var node = tag.PropertyNode;
      if (node == null)
        return null;

      // If group, look for property with rectangle
      {
        var group = node as PropertyGroup;
        if (group != null) {
          var rect = group.Children.Select(x => x.Value as AccessibleRectInfo).FirstOrDefault(x => x != null);
          if (rect != null)
            return rect;
        }
      }

      // If property with a rect, return the rect
      {
        var rect = node.Value as AccessibleRectInfo;
        if (rect != null)
          return rect;
      }

      // Look for containing group, then for property with a rect
      for (var parentGroupItem = FindParentGroupItem(selection); parentGroupItem != null; parentGroupItem = FindParentGroupItem(parentGroupItem)) {
        var groupNode = (PropertyGroup)((ListViewItemTag)parentGroupItem.Tag).PropertyNode;
        var rect = groupNode.Children.Select(x => x.Value as AccessibleRectInfo).FirstOrDefault(x => x != null);
        if (rect != null)
          return rect;
      }

      // If no parent group has a valid rectangle, last change is to look at all property node at indent = 0
      return selection.ListView.Items.Cast<ListViewItem>()
        .Where(x => x.IndentCount == 0)
        .Select(x => (ListViewItemTag)x.Tag)
        .Select(x => x.PropertyNode.Value as AccessibleRectInfo)
        .FirstOrDefault(x => x != null);
    }

    private static ListViewItem FindParentGroupItem(ListViewItem selection) {
      for (var i = selection.Index; i >= 0; i--) {
        var item = selection.ListView.Items[i];
        if (item.IndentCount == selection.IndentCount - 1) {
          var itemTag = (ListViewItemTag)item.Tag;
          var itemGroup = itemTag.PropertyNode as PropertyGroup;
          if (itemGroup != null) {
            return item;
          }
        }
      }
      return null;
    }

    private void ListViewOnGotFocus(object sender, EventArgs eventArgs) {
      ListOnSelectedIndexChanged(sender, eventArgs);
    }

    private void TogglePropertyExpanded(ListViewItem item) {
      var itemState = (ListViewItemTag)item.Tag;
      var group = itemState.PropertyNode as PropertyGroup;
      if (group == null)
        return;

      group.Expanded = !group.Expanded;
      _nodeState.SetGroupState(itemState.Path, group.Expanded);
      UpdateListView();
    }

    /// <summary>
    /// State associated with each item in the list view. The state is stored in
    /// the <see cref="ListViewItem.Tag"/> property.
    /// </summary>
    private class ListViewItemTag {
      private readonly PropertyNode _propertyNode;
      private readonly string _path;

      public ListViewItemTag(PropertyNode node, string path) {
        _propertyNode = node;
        _path = path ?? "";
      }

      public PropertyNode PropertyNode {
        get { return _propertyNode; }
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

      public void ApplyGroupState(string groupPath, PropertyGroup group) {
        bool savedState;
        if (_expandedStates.TryGetValue(groupPath, out savedState)) {
          group.Expanded = savedState;
        }
      }
    }

    protected virtual void OnAccessibleRectInfoSelected(AccessibleRectInfoSelectedEventArgs e) {
      var handler = AccessibleRectInfoSelected;
      if (handler != null) handler(this, e);
    }

    protected virtual void OnError(PropertyGroupErrorEventArgs e) {
      var handler = Error;
      if (handler != null) handler(this, e);
    }
  }
}