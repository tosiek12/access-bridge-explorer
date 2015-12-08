using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AccessBridgeExplorer.Model;

namespace AccessBridgeExplorer {
  /// <summary>
  /// Wraps a <see cref="ListView"/> used to display a <see
  /// cref="PropertyList"/> to handle node identation and expand/collapse. The
  /// <see cref="SetPropertyList"/> method must be called to switch to a new
  /// property list. The <see cref="Clear"/> method must be called to clear the
  /// contents of the listview.
  /// </summary>
  public class PropertyListViewWrapper {
    private readonly ListView _listView;
    private readonly ExpandedNodeState _nodeState = new ExpandedNodeState();
    private PropertyList _currentPropertyList;

    /// <summary>
    /// Setup event handlers and image list. Can be called in the Forms
    /// constructor, just after <code>InitializeComponent</code>.
    /// </summary>
    public PropertyListViewWrapper(ListView listView, ImageList stateImageList) {
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
        _listView.Items.Clear();
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

    private void UpdateListView() {
      _listView.BeginUpdate();
      try {
        var newItems = CreateListViewItems(_currentPropertyList, _nodeState);
        var oldItems = _listView.Items;
        var oldInsertionIndex = 0;
        for (var newIndex = 0; newIndex < newItems.Count; newIndex++) {
          var newItem = newItems[newIndex];

          // Find item with same tag in old list.
          var oldItemIndex = FindIndexOfTag(oldItems, oldInsertionIndex, newItem.Tag);
          if (oldItemIndex < 0) {
            // Insert new item at current insertion location (at end or in middle)
            oldItems.Insert(oldInsertionIndex, newItem);
            oldInsertionIndex++;
          } else {
            // Delete items in range [oldIndex, oldItemIndex[
            for (var i = oldInsertionIndex; i < oldItemIndex; i++) {
              oldItems.RemoveAt(oldInsertionIndex);
            }
            oldItemIndex = oldInsertionIndex;
            var oldItem = oldItems[oldItemIndex];
            oldItem.ImageIndex = newItem.ImageIndex;
            oldItem.StateImageIndex = newItem.StateImageIndex;
            //oldItem.Text = newItem.Text;
            //oldItem.IndentCount = newItem.IndentCount;
            //oldItem.SubItems.Clear();
            //oldItem.SubItems.AddRange(newItem.SubItems.Cast<ListViewItem.ListViewSubItem>().ToArray());
            //oldItem.Tag = newItem.Tag;
            oldInsertionIndex++;
          }
        }

        while (oldInsertionIndex < oldItems.Count) {
          oldItems.RemoveAt(oldInsertionIndex);
        }
      } finally {
        _listView.EndUpdate();
      }
    }

    private static List<ListViewItem> CreateListViewItems(PropertyList propertyList, ExpandedNodeState expandedNodeState) {
      List<ListViewItem> itemList = new List<ListViewItem>();
      propertyList.ForEach(x => {
        AddListViewItem(x, 0, "", itemList, expandedNodeState);
      });
      return itemList;
    }

    private static void AddListViewItem(PropertyNode propertyNode, int indent, string parentPath, List<ListViewItem> itemList, ExpandedNodeState expandedNodeState) {
      var propertyNodePath = MakeNodePath(parentPath, propertyNode);
      var item = new ListViewItem();
      item.Text = propertyNode.Name;
      item.SubItems.Add(ValueToString(propertyNode));
      item.Tag = new PropertyListViewItemState {
        PropertyNode = propertyNode,
        Path = propertyNodePath,
      };
      item.IndentCount = indent;
      itemList.Add(item);

      var propertyGroup = propertyNode as PropertyGroup;
      if (propertyGroup != null) {
        if (propertyGroup.Children.Count > 0) {
          expandedNodeState.ApplyGroupState(propertyNodePath, propertyGroup);
          item.ImageIndex = (propertyGroup.Expanded ? 1 : 0);
          if (propertyGroup.Expanded) {
            foreach (var child in propertyGroup.Children) {
              AddListViewItem(child, indent + 1, propertyNodePath, itemList, expandedNodeState);
            }
          }
        }
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

    private static int FindIndexOfTag(ListView.ListViewItemCollection oldItems, int startIndex, object tag) {
      for (var index = startIndex; index < oldItems.Count; index++) {
        if (Equals(oldItems[index].Tag, tag))
          return index;
      }
      return -1;
    }

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

    private void PropertyListViewOnMouseDoubleClick(object sender, MouseEventArgs e) {
      var listView = (ListView)sender;
      var info = listView.HitTest(e.Location);
      if (info.Location == ListViewHitTestLocations.None)
        return;

      TogglePropertyExpanded(info.Item);
    }

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