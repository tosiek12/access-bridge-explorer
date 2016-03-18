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
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using WindowsAccessBridgeInterop;
using AccessBridgeExplorer.Model;

namespace AccessBridgeExplorer {
  public class ExplorerFormController : IDisposable {
    private readonly IExplorerFormView _view;
    private readonly IExplorerFormNavigation _navigation;
    private readonly AccessibleNodeModelResources _accessibleNodeModelResources;
    private readonly AccessBridge _accessBridge = new AccessBridge();
    private readonly OverlayWindow _overlayWindow = new OverlayWindow();
    private readonly TooltipWindow _tooltipWindow = new TooltipWindow();
    private bool _overlayWindowEnabled;
    private Rectangle? _overlayWindowRectangle;
    private bool _disposed;
    private int _eventId;
    private int _messageId;

    public ExplorerFormController(IExplorerFormView explorerFormView) {
      _navigation = new ExplorerFormNavigation();
      _view = explorerFormView;
      _accessibleNodeModelResources = new AccessibleNodeModelResources(_view.AccessibilityTree);
      _overlayWindowEnabled = true;

      _view.EventsMenu.Enabled = false;
      _view.PropertiesMenu.Enabled = false;
      _view.LimitCollectionSizesMenu.Enabled = false;

      _view.AccessibilityTree.AfterSelect += AccessibilityTreeAfterSelect;
      _view.AccessibilityTree.BeforeExpand += AccessibilityTreeBeforeExpand;
      _view.AccessibilityTree.KeyDown += AccessibilityTreeOnKeyDown;
      _view.AccessibilityTree.GotFocus += AccessibilityTreeOnGotFocus;

      _view.EventList.MouseDoubleClick += AccessibilityEventListOnMouseDoubleClick;
      _view.EventList.KeyDown += AccessibilityEventListOnKeyDown;

      _view.AccessibleComponentPropertyList.AccessibleRectInfoSelected += OnAccessibleRectInfoSelected;

      PropertyOptions = PropertyOptions.AccessibleContextInfo |
        PropertyOptions.AccessibleIcons |
        PropertyOptions.AccessibleKeyBindings |
        PropertyOptions.AccessibleRelationSet |
        PropertyOptions.ParentContext |
        PropertyOptions.Children |
        PropertyOptions.ObjectDepth |
        PropertyOptions.TopLevelWindowInfo |
        PropertyOptions.ActiveDescendent |
        PropertyOptions.AccessibleText |
        PropertyOptions.AccessibleHyperText |
        PropertyOptions.AccessibleValue |
        PropertyOptions.AccessibleSelection |
        PropertyOptions.AccessibleTable |
        PropertyOptions.AccessibleTableCells |
        PropertyOptions.AccessibleTableCellsSelect |
        PropertyOptions.AccessibleActions;
    }

    public PropertyOptions PropertyOptions { get; set; }

    public IExplorerFormNavigation Navigation {
      get { return _navigation; }
    }

    public void Initialize() {
      _overlayWindow.TopMost = true;
      _overlayWindow.Visible = true;
      _overlayWindow.Size = new Size(0, 0);
      _overlayWindow.Location = new Point(-10, -10);
      _overlayWindow.Shown += (sender, args) => _view.AccessibilityTree.Focus();

      _tooltipWindow.TopMost = true;
      _tooltipWindow.Visible = true;
      _tooltipWindow.Size = new Size(0, 0);
      _tooltipWindow.Location = new Point(-10, -10);
      _tooltipWindow.Shown += (sender, args) => _view.AccessibilityTree.Focus();

      LogMessage("Initializing Java Access Bridge and enumerating active Java application windows.");
      _accessBridge.Initilized += (sender, args) => {
        CreateEventMenuItems();
        CreatePropertyOptionsMenuItems();
        CreateLimitCollectionSizesMenuItems();
        CreateLimitTextLineCountMenuItems();
        CreateLimitTextLineLengthsMenuItems();
        _view.EventsMenu.Enabled = true;
        _view.PropertiesMenu.Enabled = true;
        _view.LimitCollectionSizesMenu.Enabled = true;
        LogMessage("Ready!");
      };

      UiAction(() => {
        //TODO: We initialize now so that the access bridge DLL has time to
        // discover the list of JVMs by the time we enumerate all windows.
        _accessBridge.Initialize();
      });
    }

    private void AccessibilityEventListOnKeyDown(object sender, KeyEventArgs e) {
      if (_view.EventList.SelectedItems.Count == 0)
        return;

      switch (e.KeyCode & Keys.KeyCode) {
        case Keys.Return:
          foreach (ListViewItem item in _view.EventList.SelectedItems) {
            ShowEvent(item);
          }
          break;
      }
    }

    private void AccessibilityEventListOnMouseDoubleClick(object sender, MouseEventArgs e) {
      ListViewHitTestInfo info = _view.EventList.HitTest(e.X, e.Y);
      if (info.Location == ListViewHitTestLocations.None)
        return;
      ShowEvent(info.Item);
    }

    private void ShowEvent(ListViewItem item) {
      var eventInfo = item.Tag as EventInfo;
      if (eventInfo == null)
        return;

      if (eventInfo.OnDisplay == null)
        return;

      try {
        eventInfo.OnDisplay();
      } catch (Exception e) {
        LogErrorMessage(e);
        _view.ShowMessageBox(e.Message, @"Error displaying event data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
    }

    private void ShowEventDialog(AccessibleContextNode accessibleContextNode) {
      var form = new EventForm();
      form.SetContextNodePropertyList(accessibleContextNode.GetProperties(PropertyOptions));
      form.ContextNodeSelect += (sender, args) => SelectTreeNode(accessibleContextNode);
      _view.ShowDialog(form);
    }

    private void ShowEventDialog(AccessibleContextNode accessibleContextNode, AccessibleContextNode oldNode, AccessibleContextNode newNode) {
      var form = new EventForm();
      form.SetContextNodePropertyList(accessibleContextNode.GetProperties(PropertyOptions));
      form.SetOldValuePropertyList(oldNode.GetProperties(PropertyOptions));
      form.SetNewValuePropertyList(newNode.GetProperties(PropertyOptions));
      form.ContextNodeSelect += (sender, args) => SelectTreeNode(accessibleContextNode);
      form.OldValueSelect += (sender, args) => SelectTreeNode(oldNode);
      form.NewValueSelect += (sender, args) => SelectTreeNode(newNode);
      _view.ShowDialog(form);
    }

    private void ShowEventDialog(AccessibleContextNode accessibleContextNode, string oldValueName, string oldValue, string newValueName, string newValue) {
      var form = new EventForm();
      form.SetContextNodePropertyList(accessibleContextNode.GetProperties(PropertyOptions));
      form.SetOldValuePropertyList(new PropertyList { new PropertyNode(oldValueName, oldValue) });
      form.SetNewValuePropertyList(new PropertyList { new PropertyNode(newValueName, newValue) });
      form.ContextNodeSelect += (sender, args) => SelectTreeNode(accessibleContextNode);
      _view.ShowDialog(form);
    }

    private void CreateEventMenuItems() {
      var publicMembers = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;
      int index = 0;
      foreach (var evt in _accessBridge.Events.GetType().GetEvents(publicMembers)) {
        CreateEventMenuItem(evt, index);
        index++;
      }
    }

    private void CreateEventMenuItem(System.Reflection.EventInfo evt, int index) {
      var privateMembers = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic;
      var name = evt.Name;

      // Create menu item (fixed font for alignment)
      var item = new ToolStripMenuItem();
      item.Font = new Font("Lucida Console", _view.EventsMenu.Font.SizeInPoints, _view.EventsMenu.Font.Style, GraphicsUnit.Point);
      char mnemonicCharacter = (char)(index < 10 ? '0' + index : 'A' + index - 10);
      item.Text = string.Format("&{0} - {1}", mnemonicCharacter, name);
      item.CheckOnClick = true;
      item.CheckState = CheckState.Unchecked;
      _view.EventsMenu.DropDownItems.Add(item);

      // Find event handler
      var handlerMethod = GetType().GetMethod("EventsOn" + evt.Name, privateMembers);
      if (handlerMethod == null) {
        throw new ApplicationException(string.Format("Type \"{0}\" should contain a method named \"{1}\"",
          GetType().Name, "EventsOn" + evt.Name));
      }
      var handlerDelegate = Delegate.CreateDelegate(evt.EventHandlerType, this, handlerMethod);

      // Create click handler
      item.CheckedChanged += (sender, args) => {
        if (item.Checked) {
          // Add handler
          evt.AddEventHandler(_accessBridge.Events, handlerDelegate);
        } else {
          // Remove handler
          evt.RemoveEventHandler(_accessBridge.Events, handlerDelegate);
        }
      };
    }

    private void CreatePropertyOptionsMenuItems() {
      int index = 0;
      foreach (var field in typeof(PropertyOptions).GetFields(BindingFlags.Static | BindingFlags.Public)) {
        CreatePropertyOptionsMenuItem(field, index);
        index++;
      }
    }

    private void CreatePropertyOptionsMenuItem(FieldInfo field, int index) {
      var name = field.Name;
      var value = (PropertyOptions)field.GetValue(null);

      // Create menu item (fixed font for alignment)
      var item = new ToolStripMenuItem();
      item.Font = new Font("Lucida Console", _view.PropertiesMenu.Font.SizeInPoints, _view.PropertiesMenu.Font.Style, GraphicsUnit.Point);
      char mnemonicCharacter = (char)(index < 10 ? '0' + index : 'A' + index - 10);
      item.Text = string.Format("&{0} - {1}", mnemonicCharacter, name);
      item.CheckOnClick = true;
      item.CheckState = ((PropertyOptions & value) == 0 ? CheckState.Unchecked : CheckState.Checked);
      _view.PropertiesMenu.DropDownItems.Add(item);

      // Create click handler
      item.CheckedChanged += (sender, args) => {
        if (item.Checked) {
          PropertyOptions |= value;
        } else {
          PropertyOptions &= ~value;
        }
      };
    }

    private void CreateLimitCollectionSizesMenuItems() {
      int index = 0;
      foreach (var size in new int[] { 10, 20, 50, 100, 250, 500, 1000, 2000 }) {
        char mnemonicCharacter = (char)('A' + index);
        var text = string.Format("&{0} - {1} elements", mnemonicCharacter, size);
        CreateLimitSizeItem(_view.LimitCollectionSizesMenu, text, index, size,
          _accessBridge.CollectionSizeLimit,
          x => {
            _accessBridge.CollectionSizeLimit = x;
          });
        index++;
      }
    }

    private void CreateLimitTextLineCountMenuItems() {
      int index = 0;
      foreach (var size in new int[] {100, 200, 300, 500, 1000, 2000, 5000}) {
        char mnemonicCharacter = (char)('A' + index);
        var text = string.Format("&{0} - {1} lines", mnemonicCharacter, size);
        CreateLimitSizeItem(_view.LimitTextLineCountMenu, text, index, size,
          _accessBridge.TextLineCountLimit,
          x => {
            _accessBridge.TextLineCountLimit = x;
          });
        index++;
      }
    }

    private void CreateLimitTextLineLengthsMenuItems() {
      int index = 0;
      foreach (var size in new int[] { 40, 80, 120, 160, 200, 300, 400, 500, 1000 }) {
        char mnemonicCharacter = (char)('A' + index);
        var text = string.Format("&{0} - {1} characters", mnemonicCharacter, size);
        CreateLimitSizeItem(_view.LimitTextLineLengthsMenu, text, index, size,
          _accessBridge.TextLineLengthLimit,
          x => {
            _accessBridge.TextLineLengthLimit = x;
          });
        index++;
      }
    }

    private static void CreateLimitSizeItem(ToolStripMenuItem menu, string text, int index, int size, int defaultSize, Action<int> setter) {
      var item = new ToolStripMenuItem();
      item.Text = text;
      item.CheckOnClick = false;
      item.CheckState = size == defaultSize ? CheckState.Checked : CheckState.Unchecked;
      menu.DropDownItems.Add(item);

      // Create click handler
      item.Click += (sender, args) => {
        if (item.Checked)
          return;

        for (var i = 0; i < menu.DropDownItems.Count; i++) {
          var subItem = (ToolStripMenuItem)menu.DropDownItems[i];
          if (subItem.Checked)
            subItem.Checked = false;
        }
        item.Checked = true;
        setter(size);
      };
    }

    public void Dispose() {
      if (_disposed)
        return;

      DisposeTreeNodeList(_view.AccessibilityTree.Nodes);
      _accessBridge.Dispose();
      _disposed = true;
    }

    public void UiAction(Action action) {
      _view.MessageQueue.Invoke(() => {
        try {
          action();
        } catch (Exception e) {
          LogErrorMessage(e);
        }
      });
    }

    public T UiCompute<T>(Func<T> function) {
      return _view.MessageQueue.Compute(() => {
        try {
          return function();
        } catch (Exception e) {
          LogErrorMessage(e);
          return default(T);
        }
      });
    }


    public void ShowHelp() {
      _view.MessageList.Items.Clear();
      LogIntroMessages();
      foreach (ListViewItem item in _view.MessageList.Items) {
        item.Selected = true;
      }
      _view.FocusMessageList();
    }

    public void ShowAbout() {
      var form = new AboutForm();
      form.FillForm(Assembly.GetExecutingAssembly(), _accessBridge);
      _view.ShowDialog(form);
    }

    public void LogIntroMessages() {
      LogMessage("{0} allows exploring and interacting with accessibility features of Java applications.", _view.Caption);
      LogMessage("Use the \"{0}\" window to explore accessible components of active Java application windows.", _view.AccessibilityTreePage.Text);
      LogMessage("Use the \"{0}\" toolbar button to refresh the content of the \"{1}\" window.", _view.RefreshButton.Text, _view.AccessibilityTreePage.Text);
      LogMessage("Use the \"{0}\" menu to select event types to capture and display in the \"{1}\" window.", _view.EventsMenu.Text.Replace("&", ""), _view.EventListPage.Text);
      LogMessage("Use the \"{0}\" toolbar button to {1}.", _view.FindComponentButton.Text, _view.FindComponentButton.ToolTipText);
      LogMessage("Use the \"Ctrl+\\\" key in any Java application window to capture the accessible component located at the mouse cursor.");
    }

    public class EventInfo {
      public EventInfo() {
      }

      public EventInfo(string eventName, AccessibleNode source, string oldValue = "", string newValue = "") {
        EventName = eventName;
        Source = source;
        OldValue = oldValue;
        NewValue = newValue;
      }

      public string EventName { get; set; }
      public AccessibleNode Source { get; set; }
      public string OldValue { get; set; }
      public string NewValue { get; set; }
      public Action OnDisplay { get; set; }
    }

    public void LogEvent(EventInfo eventInfo) {
      _eventId++;
      var time = DateTime.Now;
      ListViewItem item = new ListViewItem();
      item.Text = _eventId.ToString();
      item.SubItems.Add(time.ToLongTimeString());
      item.SubItems.Add(eventInfo.Source.JvmId.ToString());
      item.SubItems.Add(eventInfo.EventName);
      item.SubItems.Add(eventInfo.Source.GetTitle());
      item.SubItems.Add(eventInfo.OldValue);
      item.SubItems.Add(eventInfo.NewValue);
      item.Tag = eventInfo;
      AddEvent(item);
    }

    public void LogErrorEvent(string eventName, Exception error) {
      _eventId++;
      var time = DateTime.Now;
      ListViewItem item = new ListViewItem();
      item.Text = _eventId.ToString();
      item.SubItems.Add(time.ToLongTimeString());
      item.SubItems.Add("-");
      item.SubItems.Add(eventName);
      item.SubItems.Add(error.Message);

      AddEvent(item);
    }

    public void LogNodeEvent(string eventName, Func<AccessibleContextNode> factory, Action onDisplayAction = null) {
      try {
        LogEvent(new EventInfo {
          EventName = eventName,
          Source = factory(),
          OnDisplay = onDisplayAction
        });
      } catch (Exception e) {
        LogErrorEvent(eventName, e);
      }
    }

    public void LogNodeEvent(string eventName, Func<Tuple<AccessibleContextNode, string, string>> factory, Action onDisplayAction = null) {
      try {
        var result = factory();
        LogEvent(new EventInfo {
          EventName = eventName,
          Source = result.Item1,
          OldValue = result.Item2,
          NewValue = result.Item3,
          OnDisplay = onDisplayAction
        });
      } catch (Exception e) {
        LogErrorEvent(eventName, e);
      }
    }

    private void AddEvent(ListViewItem item) {
      AddListViewItem(_view.EventList, item);
    }

    public void LogMessage(string format, params object[] args) {
      _messageId++;
      var time = DateTime.Now;
      ListViewItem item = new ListViewItem();
      item.Text = _messageId.ToString();
      item.SubItems.Add(time.ToLongTimeString());
      item.SubItems.Add(string.Format(format, args));
      AddListViewItem(_view.MessageList, item);
    }

    public void LogErrorMessage(Exception error) {
      for (var current = error; current != null; current = current.InnerException) {
        LogMessage("{0}{1}", (current == error ? "ERROR: " : "      "), current.Message);
      }
    }

    private static void AddListViewItem(ListView listview, ListViewItem item) {
      listview.BeginUpdate();
      try {
        // Manage list overflow
        if (listview.Items.Count >= 1000) {
          for (var i = 0; i < 100; i++) {
            listview.Items.RemoveAt(0);
          }
        }
        // Add item and make it visible (scrolling).
        listview.Items.Add(item);
        item.EnsureVisible();
      } finally {
        listview.EndUpdate();
      }
    }

    private int _refreshCallId = 0;
    public void RefreshTree() {
      if (_disposed)
        return;

      _refreshCallId++;
      UiAction(() => {
        // Try initializing Access Bridge.
        try {
          _accessBridge.Initialize();
        } catch (Exception e) {
          RefreshTree(new List<AccessibleJvm>());
          _view.ShowNotification(new NotificationPanelEntry {
            Text = ExceptionUtils.FormatExceptionMessage(e),
            Icon = NotificationPanelIcon.Error,
            IsExpired = () => _accessBridge.IsLoaded
          });
          return;
        }

        // Enumerate JVMs/Windows and update the accessibility tree.
        var currentRefreshCallId = _refreshCallId;
        try {
          var jvms = _accessBridge.EnumJvms();
          RefreshTree(jvms);
          if (jvms.Count == 0) {
            var sb = new StringBuilder();
            sb.Append("No Java application using the Java Access Bridge has been detected.  ");
            sb.Append("This can happen if no application is currently running, or if the " +
                      "Java Access Bridge has not been enabled.  ");
            sb.Append("See http://docs.oracle.com/javase/8/docs/technotes/guides/access/enable_and_test.html.");
            _view.ShowNotification(new NotificationPanelEntry {
              Text = sb.ToString(),
              Icon = NotificationPanelIcon.Info,
              IsExpired = () => _refreshCallId != currentRefreshCallId
            });
          }
        } catch (Exception e) {
          RefreshTree(new List<AccessibleJvm>());
          _view.ShowNotification(new NotificationPanelEntry {
            Text = ExceptionUtils.FormatExceptionMessage(e),
            Icon = NotificationPanelIcon.Error,
            IsExpired = () => _refreshCallId != currentRefreshCallId
          });
        }
      });
    }

    private void RefreshTree(List<AccessibleJvm> jvms) {
      _view.AccessibilityTree.BeginUpdate();
      try {
        // Cleanup previous tree
        DisposeTreeNodeList(_view.AccessibilityTree.Nodes);
        _view.AccessibilityTree.Nodes.Clear();

        // Add nodes for new tree, one node per JVM ID, expanded to the second level (windows).
        var topLevelNodes = jvms.Select(x => new AccessibleNodeModel(_accessibleNodeModelResources, x));
        topLevelNodes.ForEach(x => {
          var node = x.CreateTreeNode();
          _view.AccessibilityTree.Nodes.Add(node);
          node.Expand();
        });

        if (_view.AccessibilityTree.Nodes.Count == 0) {
          _view.AccessibilityTree.Nodes.Add("No application detected. Try presssing Refresh again.");
        }
      } finally {
        _view.AccessibilityTree.EndUpdate();
      }
      _view.StatusLabel.Text = @"Ready.";
      HideOverlayWindow();
      HideToolTip();
      _navigation.Clear();
    }

    private static void DisposeTreeNodeList(TreeNodeCollection list) {
      foreach (TreeNode node in list) {
        DisposeTreeNode(node);
      }
    }

    private static void DisposeTreeNode(TreeNode node) {
      DisposeTreeNodeList(node.Nodes);
      var model = node.Tag as AccessibleNodeModel;
      if (model != null) {
        model.AccessibleNode.Dispose();
      }
    }

    public void ClearSelectedNode() {
      var node = _view.AccessibilityTree.SelectedNode;
      if (node != null) {
        _view.AccessibilityTree.SelectedNode = null;
        _overlayWindowRectangle = null;
        UpdateOverlayWindow();
        _view.AccessibleComponentPropertyList.Clear();
      }
    }

    private void AccessibilityTreeBeforeExpand(object sender, TreeViewCancelEventArgs e) {
      var node = e.Node.Tag as NodeModel;
      if (node == null)
        return;
      UiAction(() => {
        node.BeforeExpand(sender, e);
      });
    }

    private void AccessibilityTreeAfterSelect(object sender, TreeViewEventArgs e) {
      var treeNode = e.Node;
      if (treeNode == null)
        return;

      var nodeModel = treeNode.Tag as AccessibleNodeModel;
      if (nodeModel == null) {
        _overlayWindowRectangle = null;
        UpdateOverlayWindow();
        _view.AccessibleComponentPropertyList.Clear();
        return;
      }

      _overlayWindowRectangle = null;
      UiAction(() => {
        _overlayWindowRectangle = nodeModel.AccessibleNode.GetScreenRectangle();
        var propertyList = nodeModel.AccessibleNode.GetProperties(PropertyOptions);
        _view.AccessibleComponentPropertyList.SetPropertyList(propertyList);
      });

      EnsureTreeNodeVisible(treeNode);
      UpdateOverlayWindow();

      var navigationEntry = new NavigationEntry {
        Description = string.Format("Navigate to \"{0}\"", nodeModel.AccessibleNode.GetTitle()),
        Action = () => SelectTreeNode(nodeModel.AccessibleNode),
      };
      _navigation.AddNavigationAction(navigationEntry);
    }

    private void AccessibilityTreeOnGotFocus(object sender, EventArgs eventArgs) {
      var treeNode = _view.AccessibilityTree.SelectedNode;
      if (treeNode == null)
        return;

      var nodeModel = treeNode.Tag as AccessibleNodeModel;
      if (nodeModel == null)
        return;

      _overlayWindowRectangle = nodeModel.AccessibleNode.GetScreenRectangle();
      UpdateOverlayWindow();
    }

    private void AccessibilityTreeOnKeyDown(object sender, KeyEventArgs keyEventArgs) {
      if (keyEventArgs.KeyCode != Keys.Return)
        return;
      var treeNode = _view.AccessibilityTree.SelectedNode;
      if (treeNode == null)
        return;

      var nodeModel = _view.AccessibilityTree.SelectedNode.Tag as AccessibleNodeModel;
      if (nodeModel == null)
        return;

      UiAction(() => {
        // First thing first; Tell the node to forget about what it knows
        nodeModel.AccessibleNode.Refresh();

        // Update the treeview children so they get refreshed
        var expanded = treeNode.IsExpanded;
        if (expanded) {
          treeNode.Collapse();
        }
        nodeModel.ResetChildren(treeNode);
        if (expanded) {
          treeNode.Expand();
        }

        // Update the property list
        var propertyList = nodeModel.AccessibleNode.GetProperties(PropertyOptions);
        _view.AccessibleComponentPropertyList.SetPropertyList(propertyList);

        // Update the overlay window
        _overlayWindowRectangle = nodeModel.AccessibleNode.GetScreenRectangle();
        UpdateOverlayWindow();
      });
    }

    private void OnAccessibleRectInfoSelected(object sender, AccessibleRectInfoSelectedEventArgs e) {
      _overlayWindowRectangle = e.AccessibleRectInfo.Rectangle;
      UpdateOverlayWindow();
    }

    private void EnsureTreeNodeVisible(TreeNode node) {
      if (node.Level >= 2) {
        node.EnsureVisible();
      }
    }

    private void UpdateOverlayWindow() {
      if (_overlayWindowRectangle == null || !_overlayWindowEnabled) {
        HideOverlayWindow();
        return;
      }

      if (!_overlayWindow.Visible) {
        _overlayWindow.TopMost = true;
        _overlayWindow.Visible = true;
      }

      // Note: The _overlayWindowRectangle value comes from the Java Access
      // Bridge for a given accessible component. Sometimes, the component has
      // bounds that extend outside of the visible screen (for example, an
      // editor embedded in a viewport will report negative values for y index
      // when scrolled down to the end of the text). On the other side, Windows
      // (or WinForms) limits the size of windows to the height/width of the
      // desktop. So, an a 1600x1200 screen, a rectangle of [x=10, y=-2000,
      // w=600, h=3000] is cropped to [x=10, y=-2000, w=600, h=1200]. This
      // results in a window that is not visible (y + height < 0).
      // to workaround that issue, we do some math. to make the rectable visible.
      var rect = _overlayWindowRectangle.Value;
      var invisibleX = rect.X < 0 ? -rect.X : 0;
      if (invisibleX > 0) {
        rect.X = 0;
        rect.Width -= invisibleX;
      }
      var invisibleY = rect.Y < 0 ? -rect.Y : 0;
      if (invisibleY > 0) {
        rect.Y = 0;
        rect.Height -= invisibleY;
      }
      _overlayWindow.Location = rect.Location;
      _overlayWindow.Size = rect.Size;
    }

    public void HideOverlayWindow() {
      _overlayWindow.Location = new Point(-10, -10);
      _overlayWindow.Size = new Size(0, 0);
    }

    public void EnableOverlayWindow(bool enabled) {
      _overlayWindowEnabled = enabled;
      UpdateOverlayWindow();
    }

    /// <summary>
    /// Return the <see cref="NodePath"/> of a node given a location on screen.
    /// Return <code>null</code> if there is no node at that location.
    /// </summary>
    public NodePath GetNodePathAt(Point screenPoint) {
      return UiCompute(() => {
        foreach (TreeNode treeNode in _view.AccessibilityTree.Nodes) {
          var node = treeNode.Tag as AccessibleNodeModel;
          if (node == null)
            continue;

          var result = node.AccessibleNode.GetNodePathAt(screenPoint);
          if (result != null)
            return result;
        }

        return null;
      });
    }

    public void ShowOverlayForNodePath(NodePath path) {
      _overlayWindowRectangle = UiCompute(() => path.LeafNode.GetScreenRectangle());
      UpdateOverlayWindow();
    }

    public void SelectNodeAtPoint(Point screenPoint) {
      var nodePath = GetNodePathAt(screenPoint);
      if (nodePath == null) {
        LogMessage("No Accessible component found at mouse location {0}", screenPoint);
        return;
      }
      SelectTreeNode(nodePath);
      _view.AccessibilityTree.Focus();
    }

    public void SelectTreeNode(AccessibleNode childNode) {
      UiAction(() => {
        var path = new NodePath();
        while (childNode != null) {
          path.AddParent(childNode);
          childNode = childNode.GetParent();
        }
        SelectTreeNode(path);
      });
    }

    public void SelectTreeNode(NodePath nodePath) {
      TreeNode lastFoundTreeNode = null;
      // Pop each node and find it in the corresponding collection
      var parentNodeList = _view.AccessibilityTree.Nodes;
      while (nodePath.Count > 0) {
        var childNode = nodePath.Pop();

        var childTreeNode = FindTreeNodeInList(parentNodeList, childNode);
        if (childTreeNode == null) {
          LogMessage("Error finding child node in node list: {0}", childNode);
          break;
        }
        lastFoundTreeNode = childTreeNode;

        if (nodePath.Count == 0) {
          // Expand the whole subtree to force each node to refresh their value
          // in case the sub-tree disappears from the accessible application
          // (e.g. in the case of an ephemeral window).
          childNode.FetchSubTree();
          break;
        }

        childTreeNode.Expand();
        parentNodeList = childTreeNode.Nodes;
      }

      if (lastFoundTreeNode != null) {
        _view.AccessibilityTree.SelectedNode = lastFoundTreeNode;
        EnsureTreeNodeVisible(lastFoundTreeNode);
      }
    }

    private TreeNode FindTreeNodeInList(TreeNodeCollection list, AccessibleNode node) {
      return UiCompute(() => {
        // Search by child index (for transient nodes)
        var childIndex = node.GetIndexInParent();
        if (childIndex >= 0 && childIndex < list.Count) {
          return list[childIndex];
        }

        // Sequential search (for Jvm, Window nodes).
        foreach (TreeNode treeNode in list) {
          var childNode = treeNode.Tag as AccessibleNodeModel;
          if (childNode == null)
            continue;

          if (childNode.AccessibleNode.Equals(node)) {
            return treeNode;
          }
        }
        return null;
      });
    }

    public void ShowToolTip(Point screenPoint, NodePath nodePath) {
      UiAction(() => {
        var node = nodePath.LeafNode;

        var sb = new StringBuilder();
        foreach (var x in node.GetToolTipProperties(PropertyOptions)) {
          if (sb.Length > 0)
            sb.Append("\r\n");
          sb.AppendFormat("{0}: {1}", x.Name, x.Value);
        }
        _tooltipWindow.AutoSize = true;
        _tooltipWindow.Label.Text = string.Format("{0}", sb);
        _tooltipWindow.Location = new Point(
          Math.Max(0, screenPoint.X - _tooltipWindow.Size.Width - 16),
          Math.Max(0, screenPoint.Y - _tooltipWindow.Size.Height / 2));
        //_tooltipWindow.Location = new Point(
        //  Math.Max(0, screenPoint.X + 20),
        //  Math.Max(0, screenPoint.Y - _tooltipWindow.Size.Height / 2));
      });
    }

    public void HideToolTip() {
      _tooltipWindow.Label.Text = "";
      _tooltipWindow.Location = new Point(-10, -10);
      _tooltipWindow.Size = new Size(0, 0);
    }

    public void OnFocusLost() {
      HideOverlayWindow();
    }

    public void OnFocusGained() {
      UpdateOverlayWindow();
    }

    #region Event Handlers
    // ReSharper disable UnusedMember.Local
    // ReSharper disable UnusedParameter.Local
    private void EventsOnPropertyChange(int vmId, JavaObjectHandle evt, JavaObjectHandle source, string property, string oldValue, string newValue) {
      // Note: It seems this event is never fired. Maybe this is from older JDKs?
      LogNodeEvent(string.Format("PropertyChange: {0}", property),
        () => Tuple.Create(new AccessibleContextNode(_accessBridge, source), oldValue, newValue),
        () => ShowEventDialog(
          new AccessibleContextNode(_accessBridge, source),
          "Old value", oldValue,
          "New value", newValue));
    }

    private void EventsOnPropertyVisibleDataChange(int vmId, JavaObjectHandle evt, JavaObjectHandle source) {
      LogNodeEvent("PropertyVisibleDataChange",
        () => new AccessibleContextNode(_accessBridge, source),
        () => ShowEventDialog(new AccessibleContextNode(_accessBridge, source)));
    }

    private void EventsOnPropertyValueChange(int vmId, JavaObjectHandle evt, JavaObjectHandle source, string oldValue, string newValue) {
      LogNodeEvent("PropertyVisibleDataChange",
        () => Tuple.Create(new AccessibleContextNode(_accessBridge, source), oldValue, newValue),
        () => ShowEventDialog(
          new AccessibleContextNode(_accessBridge, source),
          "Old value", oldValue,
          "New value", newValue));
    }

    private void EventsOnPropertyTextChange(int vmId, JavaObjectHandle evt, JavaObjectHandle source) {
      LogNodeEvent("PropertyTextChange",
        () => new AccessibleContextNode(_accessBridge, source),
        () => ShowEventDialog(new AccessibleContextNode(_accessBridge, source)));
    }

    private void EventsOnPropertyStateChange(int vmId, JavaObjectHandle evt, JavaObjectHandle source, string oldState, string newState) {
      LogNodeEvent("PropertyStateChange",
        () => Tuple.Create(new AccessibleContextNode(_accessBridge, source), oldState, newState),
        () => ShowEventDialog(
          new AccessibleContextNode(_accessBridge, source),
          "Old state", oldState,
          "New state", newState));
    }

    private void EventsOnPropertySelectionChange(int vmId, JavaObjectHandle evt, JavaObjectHandle source) {
      LogNodeEvent("PropertySelectionChange",
        () => new AccessibleContextNode(_accessBridge, source),
        () => ShowEventDialog(new AccessibleContextNode(_accessBridge, source)));
    }

    private void EventsOnPropertyNameChange(int vmId, JavaObjectHandle evt, JavaObjectHandle source, string oldName, string newName) {
      LogNodeEvent("PropertyNameChange",
        () => Tuple.Create(new AccessibleContextNode(_accessBridge, source), oldName, newName),
        () => ShowEventDialog(
          new AccessibleContextNode(_accessBridge, source),
          "Old name", oldName,
          "New name", newName));
    }

    private void EventsOnPropertyDescriptionChange(int vmId, JavaObjectHandle evt, JavaObjectHandle source, string oldDescription, string newDescription) {
      LogNodeEvent("PropertyDescriptionChange",
        () => Tuple.Create(new AccessibleContextNode(_accessBridge, source), oldDescription, newDescription),
        () => ShowEventDialog(
          new AccessibleContextNode(_accessBridge, source),
          "Old description", oldDescription,
          "New description", newDescription));
    }

    private void EventsOnPropertyCaretChange(int vmId, JavaObjectHandle evt, JavaObjectHandle source, int oldPosition, int newPosition) {
      LogNodeEvent("PropertyCaretChange",
        () => Tuple.Create(new AccessibleContextNode(_accessBridge, source), oldPosition.ToString(), newPosition.ToString()),
        () => ShowEventDialog(
          new AccessibleContextNode(_accessBridge, source),
          "Old position", oldPosition.ToString(),
          "New position", newPosition.ToString()));
    }

    private void EventsOnPropertyActiveDescendentChange(int vmId, JavaObjectHandle evt, JavaObjectHandle source, JavaObjectHandle oldActiveDescendent, JavaObjectHandle newActiveDescendent) {
      LogNodeEvent("PropertyActiveDescendentChange",
        () => Tuple.Create(
          new AccessibleContextNode(_accessBridge, source),
          new AccessibleContextNode(_accessBridge, oldActiveDescendent).GetTitle(),
          new AccessibleContextNode(_accessBridge, newActiveDescendent).GetTitle()),
        () => {
          ShowEventDialog(
            new AccessibleContextNode(_accessBridge, source),
            new AccessibleContextNode(_accessBridge, oldActiveDescendent),
            new AccessibleContextNode(_accessBridge, newActiveDescendent));
        });
    }

    private void EventsOnPropertyChildChange(int vmId, JavaObjectHandle evt, JavaObjectHandle source, JavaObjectHandle oldChild, JavaObjectHandle newChild) {
      LogNodeEvent("PropertyChildChange",
        () => Tuple.Create(
          new AccessibleContextNode(_accessBridge, source),
          new AccessibleContextNode(_accessBridge, oldChild).GetTitle(),
          new AccessibleContextNode(_accessBridge, newChild).GetTitle()),
        () => ShowEventDialog(
          new AccessibleContextNode(_accessBridge, source),
          new AccessibleContextNode(_accessBridge, oldChild),
          new AccessibleContextNode(_accessBridge, newChild)));
    }

    private void EventsOnMouseReleased(int vmId, JavaObjectHandle evt, JavaObjectHandle source) {
      LogNodeEvent("MouseReleased",
        () => new AccessibleContextNode(_accessBridge, source),
        () => ShowEventDialog(new AccessibleContextNode(_accessBridge, source)));
    }

    private void EventsOnMousePressed(int vmId, JavaObjectHandle evt, JavaObjectHandle source) {
      LogNodeEvent("MousePressed",
        () => new AccessibleContextNode(_accessBridge, source),
        () => ShowEventDialog(new AccessibleContextNode(_accessBridge, source)));
    }

    private void EventsOnMouseExited(int vmId, JavaObjectHandle evt, JavaObjectHandle source) {
      LogNodeEvent("MouseExited",
        () => new AccessibleContextNode(_accessBridge, source),
        () => ShowEventDialog(new AccessibleContextNode(_accessBridge, source)));
    }

    private void EventsOnMouseEntered(int vmId, JavaObjectHandle evt, JavaObjectHandle source) {
      LogNodeEvent("MouseEntered",
        () => new AccessibleContextNode(_accessBridge, source),
        () => ShowEventDialog(new AccessibleContextNode(_accessBridge, source)));
    }

    private void EventsOnMouseClicked(int vmId, JavaObjectHandle evt, JavaObjectHandle source) {
      LogNodeEvent("MouseClicked",
        () => new AccessibleContextNode(_accessBridge, source),
        () => ShowEventDialog(new AccessibleContextNode(_accessBridge, source)));
    }

    private void EventsOnCaretUpdate(int vmId, JavaObjectHandle evt, JavaObjectHandle source) {
      LogNodeEvent("CaretUpdate",
        () => new AccessibleContextNode(_accessBridge, source),
        () => ShowEventDialog(new AccessibleContextNode(_accessBridge, source)));
    }

    private void EventsOnFocusGained(int vmId, JavaObjectHandle evt, JavaObjectHandle source) {
      LogNodeEvent("FocusGained",
        () => new AccessibleContextNode(_accessBridge, source),
        () => ShowEventDialog(new AccessibleContextNode(_accessBridge, source)));
    }

    private void EventsOnFocusLost(int vmId, JavaObjectHandle evt, JavaObjectHandle source) {
      LogNodeEvent("FocusLost",
        () => new AccessibleContextNode(_accessBridge, source),
        () => ShowEventDialog(new AccessibleContextNode(_accessBridge, source)));
    }

    private void EventsOnJavaShutdown(int jvmId) {
      LogMessage("JVM {0} has shutdown. Refresh tree.", jvmId);
    }

    private void EventsOnMenuCanceled(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      LogNodeEvent("MenuCanceled",
        () => new AccessibleContextNode(_accessBridge, source),
        () => ShowEventDialog(new AccessibleContextNode(_accessBridge, source)));
    }

    private void EventsOnMenuDeselected(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      LogNodeEvent("MenuDeselected",
        () => new AccessibleContextNode(_accessBridge, source),
        () => ShowEventDialog(new AccessibleContextNode(_accessBridge, source)));
    }

    private void EventsOnMenuSelected(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      LogNodeEvent("MenuSelected",
        () => new AccessibleContextNode(_accessBridge, source),
        () => ShowEventDialog(new AccessibleContextNode(_accessBridge, source)));
    }

    private void EventsOnPopupMenuCanceled(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      LogNodeEvent("PopupMenuCanceled",
        () => new AccessibleContextNode(_accessBridge, source),
        () => ShowEventDialog(new AccessibleContextNode(_accessBridge, source)));
    }

    private void EventsOnPopupMenuWillBecomeInvisible(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      LogNodeEvent("PopupMenuWillBecomeInvisible",
        () => new AccessibleContextNode(_accessBridge, source),
        () => ShowEventDialog(new AccessibleContextNode(_accessBridge, source)));
    }

    private void EventsOnPopupMenuWillBecomeVisible(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      LogNodeEvent("PopupMenuWillBecomeVisible",
        () => new AccessibleContextNode(_accessBridge, source),
        () => ShowEventDialog(new AccessibleContextNode(_accessBridge, source)));
    }

    private void EventsOnPropertyTableModelChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string oldValue, string newValue) {
      LogNodeEvent("PropertyTableModelChange",
        () => Tuple.Create(new AccessibleContextNode(_accessBridge, source), oldValue, newValue),
        () => ShowEventDialog(
          new AccessibleContextNode(_accessBridge, source),
          "Old value", oldValue,
          "New value", newValue));
    }
    // ReSharper restore UnusedParameter.Local
    // ReSharper restore UnusedMember.Local
    #endregion
  }
}
