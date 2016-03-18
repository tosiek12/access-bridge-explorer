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

using System.Collections.Generic;
using System.Windows.Forms;
using WindowsAccessBridgeInterop;

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

      listView.MouseClick += PropertyListViewOnMouseClick;
      listView.MouseDoubleClick += PropertyListViewOnMouseDoubleClick;
      listView.KeyDown += PropertyListViewOnKeyDown;
    }

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

    /// <summary>
    /// Update the list view contents minimally after a group has been
    /// expanded/collapsed or any other change to <see
    /// cref="_currentPropertyList"/>.
    /// </summary>
    private void UpdateListView() {
      _listView.BeginUpdate();
      try {
        var newItems = CreateListViewItems(_currentPropertyList, _nodeState);
        var oldItems = _listView.Items;
        var oldInsertionIndex = 0;
        // We go through each item in the new list and decide what to do in the
        // existing list of items currently displayed. If there are additional
        // nodes in the new list, we insert them in the existing list. If there
        // are missing nodes in the new list we delete them from the existing
        // list.
        for (var newIndex = 0; newIndex < newItems.Count; newIndex++) {
          var newItem = newItems[newIndex];

          // Find item with same tag in old list.
          var oldItemIndex = FindIndexOfTag(oldItems, oldInsertionIndex, newItem.Tag);
          if (oldItemIndex < 0) {
            // If this is a new node (existing node not found), insert new list
            // view item at current insertion location (at end or in middle)

            oldItems.Insert(oldInsertionIndex, newItem);
            oldInsertionIndex++;
          } else {
            // If we found an equivalent node in the existing list, delete
            // existing items in between if needed, then update the existing
            // item with the updated values.

            // Delete items in range [oldIndex, oldItemIndex[
            for (var i = oldInsertionIndex; i < oldItemIndex; i++) {
              oldItems.RemoveAt(oldInsertionIndex);
            }
            oldItemIndex = oldInsertionIndex;

            // Update existing item with new property data
            UpdateListViewItem(oldItems[oldItemIndex], newItem);
            oldInsertionIndex++;
          }
        }

        // Delete all the existing items that don't exist anymore, since we
        // reached the end of the new list.
        while (oldInsertionIndex < oldItems.Count) {
          oldItems.RemoveAt(oldInsertionIndex);
        }
      } finally {
        _listView.EndUpdate();
      }
    }

    private static int FindIndexOfTag(ListView.ListViewItemCollection oldItems, int startIndex, object tag) {
      for (var index = startIndex; index < oldItems.Count; index++) {
        // This ends up calling PropertyListViewItemState.Equals()
        if (Equals(oldItems[index].Tag, tag))
          return index;
      }
      return -1;
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
      item.Tag = new PropertyListViewItemState {
        PropertyNode = propertyNode,
        Path = propertyNodePath,
      };
      item.IndentCount = indent;
      itemList.Add(item);

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
      item.Text = propertyNode.Name;
      item.SubItems.Add(ValueToString(propertyNode));
    }

    private static void UpdateListViewItem(ListViewItem oldItem, ListViewItem newItem) {
      oldItem.ImageIndex = newItem.ImageIndex;
      oldItem.StateImageIndex = newItem.StateImageIndex;
      oldItem.Text = newItem.Text;
      oldItem.IndentCount = newItem.IndentCount;
      //oldItem.SubItems.Clear();
      for (var i = 1; i <= newItem.SubItems.Count - 1; i++) {
        if (i >= oldItem.SubItems.Count) {
          oldItem.SubItems.Add(newItem.SubItems[i].Text);
        } else {
          oldItem.SubItems[i].Text = newItem.SubItems[i].Text;
        }
      }
      for (var i = oldItem.SubItems.Count - 1; i >= newItem.SubItems.Count; i--) {
        oldItem.SubItems.RemoveAt(i);
      }
      oldItem.Tag = newItem.Tag;
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
    private void PropertyListViewOnKeyDown(object sender, KeyEventArgs e) {
      var listView = (ListView)sender;
      if (listView.SelectedItems.Count == 0)
        return;

      var item = listView.SelectedItems[0];
      var itemState = (PropertyListViewItemState)item.Tag;
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
    private void PropertyListViewOnMouseDoubleClick(object sender, MouseEventArgs e) {
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
    private void PropertyListViewOnMouseClick(object sender, MouseEventArgs e) {
      var listView = (ListView)sender;
      var info = listView.HitTest(e.Location);
      if (info.Location != ListViewHitTestLocations.Image)
        return;

      TogglePropertyExpanded(info.Item);
    }

    private void TogglePropertyExpanded(ListViewItem item) {
      var itemState = (PropertyListViewItemState)item.Tag;
      var group = itemState.PropertyNode as PropertyGroup;
      if (group == null)
        return;

      group.Expanded = !group.Expanded;
      _nodeState.SetGroupState(itemState.Path, group.Expanded);
      UpdateListView();
    }

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    /// <summary>
    /// State associated with each item in the list view. The state is stored in
    /// the <see cref="ListViewItem.Tag"/> property.
    /// </summary>
    private class PropertyListViewItemState {
      public PropertyNode PropertyNode { get; set; }
      public string Path { get; set; }

      public override bool Equals(object obj) {
        return Equals(obj as PropertyListViewItemState);
      }

      public bool Equals(PropertyListViewItemState obj) {
        if (obj == null)
          return false;

        return Equals(PropertyNode, obj.PropertyNode) &&
               Equals(Path, obj.Path);
      }
    }
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()

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
  }
}