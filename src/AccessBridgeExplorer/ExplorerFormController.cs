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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using WindowsAccessBridgeInterop;
using AccessBridgeExplorer.Model;
using AccessBridgeExplorer.Utils;
using AccessBridgeExplorer.Utils.Settings;

namespace AccessBridgeExplorer {
  public class ExplorerFormController : IDisposable {
    private readonly IUserSettings _userSettings;
    private readonly IExplorerFormView _view;
    private readonly IExplorerFormNavigation _navigation;
    private readonly AccessibleNodeModelResources _accessibleNodeModelResources;
    private readonly AccessBridge _accessBridge = new AccessBridge();
    private readonly OverlayWindow _overlayWindow = new OverlayWindow();
    private readonly TooltipWindow _tooltipWindow = new TooltipWindow();
    private readonly SingleDelayedTask _delayedRefreshTree = new SingleDelayedTask();
    private readonly SingleDelayedTask _hideOverlayOnFocusLost = new SingleDelayedTask();
    private readonly HwndCache _windowCache = new HwndCache();

    private readonly UserSetting<bool> _autoDetectApplicationsEnabledSetting;
    private readonly UserSetting<OverlayActivation> _overlayActivationSetting;
    private readonly UserSetting<OverlayDisplayType> _overlayDisplayTypeSetting;
    private readonly UserSetting<PropertyOptions> _propertyOptionsSetting;
    private readonly UserSetting<bool> _overlayEnabledSetting;
    private readonly UserSetting<int> _collectionSizeLimitSetting;
    private readonly UserSetting<int> _textLineCountLimitSetting;
    private readonly UserSetting<int> _textLineLengthLimitSetting;

    private Rectangle? _overlayWindowRectangle;
    private bool _disposed;
    private int _eventId;
    private int _messageId;
    private int _refreshCallId;

    public ExplorerFormController(IExplorerFormView explorerFormView, IUserSettings userSetting) {
      _view = explorerFormView;
      _userSettings = userSetting;
      _navigation = new ExplorerFormNavigation();
      _accessibleNodeModelResources = new AccessibleNodeModelResources(_view.AccessibilityTree);

      _userSettings.Error += UserSettings_OnError;

      _overlayEnabledSetting = new BoolUserSetting(userSetting, "overlay.enabled", true);
      _overlayEnabledSetting.Changed += (sender, args) => {
        if (_disposed)
          return;
        _view.EnableOverlayMenuItem.Checked = args.NewValue;
        UpdateOverlayMenuItems();
      };

      _overlayActivationSetting = new OverlayActivationSetting(this);
      _overlayDisplayTypeSetting = new OverlayDisplayTypeSetting(this);
      _propertyOptionsSetting = new PropertyOptionsSetting(this);
      _autoDetectApplicationsEnabledSetting = new AutoDetectApplicationsSetting(this);

      _collectionSizeLimitSetting = new IntUserSetting(_userSettings, "accessBridge.collections.size.limit", _accessBridge.CollectionSizeLimit);
      _collectionSizeLimitSetting.Sync += (sender, args) => {
        _accessBridge.CollectionSizeLimit = args.Value;
      };
      _textLineCountLimitSetting = new IntUserSetting(_userSettings, "accessBridge.text.lineCount.limit", _accessBridge.TextLineCountLimit);
      _textLineCountLimitSetting.Sync += (sender, args) => {
        _accessBridge.TextLineCountLimit = args.Value;
      };
      _textLineLengthLimitSetting = new IntUserSetting(_userSettings, "accessBridge.text.lineLength.limit", _accessBridge.TextLineLengthLimit);
      _textLineLengthLimitSetting.Sync += (sender, args) => {
        _accessBridge.TextLineLengthLimit = args.Value;
      };
    }

    private class OverlayDisplayTypeSetting : EnumUserSetting<OverlayDisplayType> {
      public OverlayDisplayTypeSetting(ExplorerFormController controller) :
        base(controller._userSettings, "overlay.displayType", OverlayDisplayType.OverlayOnly) {
        Sync += (sender, args) => {
          if (controller._disposed)
            return;
          controller.UpdateOverlayMenuItems();
        };
      }
    }

    private class OverlayActivationSetting : EnumFlagsUserSetting<OverlayActivation> {
      public OverlayActivationSetting(ExplorerFormController controller) :
        base(controller._userSettings, "overlay.activation", OverlayActivation.OnTreeSelection | OverlayActivation.OnComponentSelection) {
        Changed += (sender, args) => {
          if (controller._disposed)
            return;
          controller.UpdateOverlayActivation(args.PreviousValue);
        };

        controller._accessBridge.Initilized += (sender, args) => {
          if (controller._disposed)
            return;

          OnChanged(new ChangedEventArgs<OverlayActivation>(this, OverlayActivation.None, Value));
        };
      }
    }

    private class PropertyOptionsSetting : EnumFlagsUserSetting<PropertyOptions> {
      private const PropertyOptions DefaultPropertyOptions = PropertyOptions.AccessibleContextInfo |
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

      public PropertyOptionsSetting(ExplorerFormController controller) :
        base(controller._userSettings, "accessibleComponent.displayProperties", DefaultPropertyOptions) {
        Changed += (sender, args) => {
          // TODO: Refresh accessible component property pane?
          //if (controller._disposed)
          //  return;
        };
      }
    }

    private class AutoDetectApplicationsSetting : BoolUserSetting {
      private readonly ExplorerFormController _controller;

      public AutoDetectApplicationsSetting(ExplorerFormController controller) :
        base(controller._userSettings, "runningApplications.autoDetect", true) {
        _controller = controller;
        controller._accessBridge.Initilized += (sender, args) => {
          if (controller._disposed)
            return;

          OnSync(new SyncEventArgs<bool>(this, Value));
        };

        Sync += (sender, args) => {
          if (controller._disposed)
            return;

          if (controller._accessBridge.IsLoaded) {
            if (args.Value) {
              controller._accessBridge.Events.FocusGained += AccessBridgeEvents_OnFocusGained;
              controller._accessBridge.Events.JavaShutdown += AccessBridgeEvents_JavaShutdown;
            } else {
              controller._accessBridge.Events.FocusGained -= AccessBridgeEvents_OnFocusGained;
              controller._accessBridge.Events.JavaShutdown -= AccessBridgeEvents_JavaShutdown;
            }
          }
        };
      }

      private void AccessBridgeEvents_OnFocusGained(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
        if (_controller._disposed)
          return;

        _controller.UiAction(() => {
          // Access the top level object as fast as possible to
          // ensure the application knows there is an active screen reader.
          if (_controller._windowCache.Windows.All(x => x.JvmId != vmid)) {
            var topLevel = _controller._accessBridge.Functions.GetTopLevelObject(source.JvmId, source);
            topLevel.Dispose();
          }
          _controller.PostRefreshTree();
        });
      }

      private void AccessBridgeEvents_JavaShutdown(int vmid) {
        if (_controller._disposed)
          return;

        _controller.PostRefreshTree();
      }
    }

    public PropertyOptions PropertyOptions {
      get { return _propertyOptionsSetting.Value; }
    }

    public IExplorerFormNavigation Navigation {
      get { return _navigation; }
    }

    public OverlayDisplayType OverlayDisplayType {
      set { _overlayDisplayTypeSetting.Value = value; }
    }

    public void Initialize() {
      _view.EventsMenu.Enabled = false;
      _view.PropertiesMenu.Enabled = false;
      _view.LimitCollectionSizesMenu.Enabled = false;

      _view.AccessibilityTree.AfterSelect += AccessibilityTree_AfterSelect;
      _view.AccessibilityTree.BeforeExpand += AccessibilityTree_BeforeExpand;
      _view.AccessibilityTree.KeyDown += AccessibilityTree_KeyDown;
      _view.AccessibilityTree.GotFocus += AccessibilityTree_GotFocus;

      _view.MessageList.MouseDoubleClick += AccessibilityMessageList_MouseDoubleClick;
      _view.MessageList.KeyDown += AccessibilityMessageList_KeyDown;

      _view.EventList.MouseDoubleClick += AccessibilityEventList_MouseDoubleClick;
      _view.EventList.KeyDown += AccessibilityEventList_KeyDown;

      _view.EnableOverlayMenuItem.CheckedChanged += EnableOverlayMenuItem_OnCheckedChanged;
      _view.EnableOverlayButton.Click += EnableOverlayButton_OnClick;
      _view.AccessibleComponentPropertyListView.AccessibleRectInfoSelected += ComponentPropertyListView_AccessibleRectInfoSelected;
      _view.AccessibleComponentPropertyListView.Error += ComponentPropertyListView_Error;

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
        EnableAutoDetect(_autoDetectApplicationsEnabledSetting.Value);
        CreateEventMenuItems();
        CreatePropertyOptionsMenuItems();
        CreateLimitCollectionSizesMenuItems();
        CreateLimitTextLineCountMenuItems();
        CreateLimitTextLineLengthsMenuItems();
        _view.EventsMenu.Enabled = true;
        _view.PropertiesMenu.Enabled = true;
        _view.LimitCollectionSizesMenu.Enabled = true;
        UpdateOverlayMenuItems();

        LogMessage("Ready!");
      };

      UiAction(() => {
        //TODO: We initialize now so that the access bridge DLL has time to
        // discover the list of JVMs by the time we enumerate all windows.
        _accessBridge.Initialize();
      });
    }

    private void EnableOverlayMenuItem_OnCheckedChanged(object sender, EventArgs eventArgs) {
      _overlayEnabledSetting.Value = _view.EnableOverlayMenuItem.Checked;
    }

    private void EnableOverlayButton_OnClick(object sender, EventArgs eventArgs) {
      _overlayEnabledSetting.Value = !_view.EnableOverlayButton.Checked;
    }

    private void UserSettings_OnError(object sender, ErrorEventArgs errorEventArgs) {
      if (_disposed)
        return;

      LogErrorMessage(errorEventArgs.GetException());
    }

    private void UpdateOverlayMenuItems() {
      bool enabled = _view.EnableOverlayMenuItem.Checked;

      _view.ActivateOverlayOnTreeSelectionMenu.Checked = (_overlayActivationSetting.Value & OverlayActivation.OnTreeSelection) != 0;
      _view.ActivateOverlayOnComponentSelectionMenu.Checked = (_overlayActivationSetting.Value & OverlayActivation.OnComponentSelection) != 0;
      _view.ActivateOverlayOnFocusMenu.Checked = (_overlayActivationSetting.Value & OverlayActivation.OnFocusGained) != 0;
      _view.ActivateOverlayOnActiveDescendantMenu.Checked = (_overlayActivationSetting.Value & OverlayActivation.OnActiveDescendantChanged) != 0;

      _view.ShowTooltipAndOverlayMenuItem.Checked = (_overlayDisplayTypeSetting.Value == OverlayDisplayType.OverlayAndTooltip);
      _view.ShowTooltipOnlyMenuItem.Checked = (_overlayDisplayTypeSetting.Value == OverlayDisplayType.TooltipOnly);
      _view.ShowOverlayOnlyMenuItem.Checked = (_overlayDisplayTypeSetting.Value == OverlayDisplayType.OverlayOnly);

      _view.ActivateOverlayOnTreeSelectionMenu.Enabled = enabled;
      _view.ActivateOverlayOnComponentSelectionMenu.Enabled = enabled;
      _view.ActivateOverlayOnFocusMenu.Enabled = enabled;
      _view.ActivateOverlayOnActiveDescendantMenu.Enabled = enabled;

      _view.ShowTooltipAndOverlayMenuItem.Enabled = enabled;
      _view.ShowTooltipOnlyMenuItem.Enabled = enabled;
      _view.ShowOverlayOnlyMenuItem.Enabled = enabled;

      // Update overlay button (which applies only to tree activation).
      var button = _view.EnableOverlayButton;
      button.Checked = _overlayEnabledSetting.Value;
      if (button.Checked) {
        button.ForeColor = Color.FromArgb(128, 255, 128);
      } else {
        button.ForeColor = SystemColors.InactiveCaption;
      }
    }

    private void AccessibilityEventList_KeyDown(object sender, KeyEventArgs e) {
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

    private void AccessibilityEventList_MouseDoubleClick(object sender, MouseEventArgs e) {
      ListViewHitTestInfo info = _view.EventList.HitTest(e.X, e.Y);
      if (info.Location == ListViewHitTestLocations.None)
        return;
      ShowEvent(info.Item);
    }

    private void AccessibilityMessageList_KeyDown(object sender, KeyEventArgs e) {
      if (_view.MessageList.SelectedItems.Count == 0)
        return;

      switch (e.KeyCode & Keys.KeyCode) {
        case Keys.Return:
          foreach (ListViewItem item in _view.MessageList.SelectedItems) {
            ShowMessage(item);
          }
          break;
      }
    }

    private void AccessibilityMessageList_MouseDoubleClick(object sender, MouseEventArgs e) {
      ListViewHitTestInfo info = _view.MessageList.HitTest(e.X, e.Y);
      if (info.Location == ListViewHitTestLocations.None)
        return;
      ShowMessage(info.Item);
    }

    private void ShowMessage(ListViewItem item) {
      var messageInfo = item.Tag as MessageInfo;
      if (messageInfo == null)
        return;

      if (messageInfo.OnDisplay == null)
        return;

      try {
        messageInfo.OnDisplay();
      } catch (Exception e) {
        LogErrorMessage(e);
        _view.ShowMessageBox(e.Message, @"Error displaying message data", MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }
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

      // Find event handler
      var handlerMethod = GetType().GetMethod("EventsOn" + evt.Name, privateMembers);
      if (handlerMethod == null) {
        throw new ApplicationException(string.Format("Type \"{0}\" should contain a method named \"{1}\"",
          GetType().Name, "EventsOn" + evt.Name));
      }
      var handlerDelegate = Delegate.CreateDelegate(evt.EventHandlerType, this, handlerMethod);

      // Create persistent user setting
      var userSetting = new BoolUserSetting(_userSettings, "accessBridge.events." + name, false);
      Action<bool> apply = value => {
        if (value) {
          // Add handler
          evt.AddEventHandler(_accessBridge.Events, handlerDelegate);
        } else {
          // Remove handler
          evt.RemoveEventHandler(_accessBridge.Events, handlerDelegate);
        }
      };
      userSetting.Changed += (sender, args) => {
        apply(args.NewValue);
      };
      // Settings are already loaded, so we won't get a "Loaded" event, so
      // we ensure the event handler is setup if needed.
      if (userSetting.Value != userSetting.DefaultValue) {
        apply(userSetting.Value);
      }

      // Create menu item (fixed font for alignment)
      var item = new ToolStripMenuItem();
      item.Font = new Font("Lucida Console", _view.EventsMenu.Font.SizeInPoints, _view.EventsMenu.Font.Style, GraphicsUnit.Point);
      char mnemonicCharacter = (char)(index < 10 ? '0' + index : 'A' + index - 10);
      item.Text = string.Format("&{0} - {1}", mnemonicCharacter, name);
      item.CheckOnClick = true;
      item.Checked = userSetting.Value;
      item.CheckedChanged += (sender, args) => {
        userSetting.Value = item.Checked;
      };
      _view.EventsMenu.DropDownItems.Add(item);
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
          _propertyOptionsSetting.Value |= value;
        } else {
          _propertyOptionsSetting.Value &= ~value;
        }
      };
    }

    private void CreateLimitCollectionSizesMenuItems() {
      int index = 0;
      foreach (var size in new[] { 10, 20, 50, 100, 250, 500, 1000, 2000 }) {
        char mnemonicCharacter = (char)('A' + index);
        var text = string.Format("&{0} - {1} elements", mnemonicCharacter, size);
        CreateLimitSizeItem(_view.LimitCollectionSizesMenu, text, size,
          _collectionSizeLimitSetting.Value,
          x => {
            _collectionSizeLimitSetting.Value = x;
          });
        index++;
      }
    }

    private void CreateLimitTextLineCountMenuItems() {
      int index = 0;
      foreach (var size in new[] { 100, 200, 300, 500, 1000, 2000, 5000 }) {
        char mnemonicCharacter = (char)('A' + index);
        var text = string.Format("&{0} - {1} lines", mnemonicCharacter, size);
        CreateLimitSizeItem(_view.LimitTextLineCountMenu, text, size,
          _textLineCountLimitSetting.Value,
          x => {
            _textLineCountLimitSetting.Value = x;
          });
        index++;
      }
    }

    private void CreateLimitTextLineLengthsMenuItems() {
      int index = 0;
      foreach (var size in new[] { 40, 80, 120, 160, 200, 300, 400, 500, 1000 }) {
        char mnemonicCharacter = (char)('A' + index);
        var text = string.Format("&{0} - {1} characters", mnemonicCharacter, size);
        CreateLimitSizeItem(_view.LimitTextLineLengthsMenu, text, size,
          _textLineLengthLimitSetting.Value,
          x => {
            _textLineLengthLimitSetting.Value = x;
          });
        index++;
      }
    }

    private static void CreateLimitSizeItem(ToolStripMenuItem menu, string text, int size, int defaultSize, Action<int> setter) {
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

      _userSettings.Error -= UserSettings_OnError;

      _hideOverlayOnFocusLost.Cancel();
      _delayedRefreshTree.Cancel();

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

    public class MessageInfo {
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

    public ListViewItem LogMessage(string format, params object[] args) {
      _messageId++;
      var time = DateTime.Now;
      ListViewItem item = new ListViewItem();
      item.Text = _messageId.ToString();
      item.SubItems.Add(time.ToLongTimeString());
      item.SubItems.Add(string.Format(format, args));
      AddListViewItem(_view.MessageList, item);
      return item;
    }

    public void LogErrorMessage(Exception error) {
      // Note: Exceptions don't capture the full stack trace, so we capture the full stack
      // trace here to display in the error dialog.
      // See http://stackoverflow.com/questions/5301535/exception-call-stack-truncated-without-any-re-throwing
      var stackTrace = new StackTrace(fNeedFileInfo: true);

      for (var current = error; current != null; current = current.InnerException) {
        var exception = current;

        var item = LogMessage("{0}{1}", (exception == error ? "ERROR: " : "      "), exception.Message);
        item.Tag = new MessageInfo {
          OnDisplay = () => {
            var form = new ExceptionForm();
            form.DisplayError(error, stackTrace);
            _view.ShowDialog(form);
          }
        };
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

    public List<AccessibleJvm> EnumJvms() {
      return _accessBridge.EnumJvms(hwnd => _windowCache.Get(_accessBridge, hwnd));
    }

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
          _windowCache.Clear();
          var jvms = EnumJvms();
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
              IsExpired = () => GetAccessibleJvmsFromTree().Any()
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

    private void RefreshTree(IList<AccessibleJvm> jvms) {
      _view.AccessibilityTree.BeginUpdate();
      try {
        _view.AccessibilityTree.Nodes.Clear();
        UpdateTree(jvms);
      } finally {
        _view.AccessibilityTree.EndUpdate();
      }
      _view.StatusLabel.Text = @"Ready.";
      HideOverlayWindow();
      HideToolTip();
      _navigation.Clear();
    }

    private void UpdateTree(IList<AccessibleJvm> jvms) {
      ListHelpers.IncrementalUpdate(_view.AccessibilityTree.Nodes.AsList(), jvms, new JvmNodesOperations(this));
      // Display help message if no jvm nodes
      if (_view.AccessibilityTree.Nodes.Count == 0) {
        _view.AccessibilityTree.Nodes.Add("No application detected. Try presssing Refresh again.");
      }

#if false
      TestExceptionForm();
#endif
    }

    private void TestExceptionForm() {
      try {
        ThrowException<int>();
      } catch (Exception e1) {
        try {
          Test<string>.ThrowInvalidException(e1);
        } catch (Exception e2) {
          LogErrorMessage(e2);
        }
      }
    }

    private static void ThrowException<T>() {
      throw new ApplicationException("Test");
    }

    private class Test<T> {
      public static void ThrowInvalidException(Exception e1) {
        throw new InvalidOperationException("Invalid Op", e1);
      }
    }

    private class JvmNodesOperations : IncrementalUpdateOperations<TreeNode, AccessibleJvm> {
      private readonly ExplorerFormController _controller;
      private readonly WindowNodesOperations _windowNodesOperations;

      public JvmNodesOperations(ExplorerFormController controller) {
        _controller = controller;
        _windowNodesOperations = new WindowNodesOperations(_controller);
      }

      public override int FindItem(IList<TreeNode> items, int startIndex, AccessibleJvm newItem) {
        for (var i = 0; i < items.Count; i++) {
          var node = items[i];
          if ((node.Tag != null) && _controller.GetAccessibleJvmFromNode(node).JvmId == newItem.JvmId) {
            return i;
          }
        }
        return -1;
      }

      public override void InsertItem(IList<TreeNode> items, int index, AccessibleJvm newItem) {
        var model = new AccessibleNodeModel(_controller._accessibleNodeModelResources, newItem);
        var node = model.CreateTreeNode();

        // Insert and expand node at insertion position
        _controller._view.AccessibilityTree.Nodes.Insert(index, node);
        node.Expand();
      }

      public override void UpdateItem(IList<TreeNode> items, int index, AccessibleJvm newItem) {
        // Update the tree node to point to the new AccessibleJvm
        var jvmTreeNode = items[index];
        //_controller.SetNodeAccessibleJvm(jvmTreeNode, newItem);

        // Update the tree nodes representing the list of windows
        ListHelpers.IncrementalUpdate(items[index].Nodes.AsList(), newItem.Windows, _windowNodesOperations);

        // Update tree node text, as AccessibleJvm text may be different as windows
        // are added/removed
        var title = newItem.GetTitle();
        if (jvmTreeNode.Text != title) {
          jvmTreeNode.Text = title;
        }
      }

      public override void RemoveItem(IList<TreeNode> items, int index) {
        _controller.DisposeTreeNode(items[index]);
        base.RemoveItem(items, index);
      }
    }

    private class WindowNodesOperations : IncrementalUpdateOperations<TreeNode, AccessibleWindow> {
      private readonly ExplorerFormController _controller;

      public WindowNodesOperations(ExplorerFormController controller) {
        _controller = controller;
      }

      public override int FindItem(IList<TreeNode> items, int startIndex, AccessibleWindow newItem) {
        for (var i = 0; i < items.Count; i++) {
          var node = items[i];
          if ((node.Tag != null) && _controller.GetAccessibleWindowFromNode(node).Hwnd == newItem.Hwnd) {
            return i;
          }
        }
        return -1;
      }

      public override void InsertItem(IList<TreeNode> items, int index, AccessibleWindow newItem) {
        var model = new AccessibleNodeModel(_controller._accessibleNodeModelResources, newItem);
        var node = model.CreateTreeNode();
        items.Insert(index, node);
      }

      public override void UpdateItem(IList<TreeNode> items, int index, AccessibleWindow newItem) {
        // Update the tree node to point to the new AccessibleWindow node
        var treeNode = items[index];
        //_controller.SetNodeAccessibleWindow(treeNode, newItem);

        // Update tree node text, as AccessibleWindow titles may change over time.
        var title = newItem.GetTitle();
        if (treeNode.Text != title) {
          treeNode.Text = title;
        }
      }

      public override void RemoveItem(IList<TreeNode> items, int index) {
        _controller.DisposeTreeNode(items[index]);
        base.RemoveItem(items, index);
      }
    }

    private void AccessBridgeEvents_OverlayActivation_OnActiveDescendantChanged(int vmid, JavaObjectHandle evt, JavaObjectHandle source, JavaObjectHandle oldactivedescendent, JavaObjectHandle newactivedescendent) {
      UiAction(() => {
        UpdateOverlayDisplay(newactivedescendent, OverlayActivation.OnActiveDescendantChanged);
      });
    }

    private void AccessBridgeEvents_OverlayActivation_OnFocusGained(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      UiAction(() => {
        _hideOverlayOnFocusLost.Cancel();
        UpdateOverlayDisplay(source, OverlayActivation.OnFocusGained);
      });
    }

    private void AccessBridgeEvents_OverlayActivation_OnFocusLost(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      UiAction(() => {
        _hideOverlayOnFocusLost.Post(TimeSpan.FromMilliseconds(100), () => {
          UiAction(() => {
            HideOverlayWindow();
            HideToolTip();
          });
        });
      });
    }

    private void AccessBridgeEvents_OverlayActivation_OnJavaShutdown(int vmid) {
      //TODO
    }

    private void UpdateOverlayDisplay(JavaObjectHandle source, OverlayActivation activation) {
      if (source.IsNull) {
        return;
      }

      UpdateOverlayDisplay(new AccessibleContextNode(_accessBridge, source), activation);
    }

    private void UpdateOverlayDisplay(AccessibleNode node, OverlayActivation activation) {
      if (!_overlayEnabledSetting.Value) {
        return;
      }

      if ((_overlayActivationSetting.Value & activation) == 0) {
        return;
      }

      _overlayWindowRectangle = node.GetScreenRectangle();
      if (_overlayDisplayTypeSetting.Value == OverlayDisplayType.OverlayAndTooltip || _overlayDisplayTypeSetting.Value == OverlayDisplayType.OverlayOnly) {
        ShowOverlayWindow();
      }

      if (_overlayDisplayTypeSetting.Value == OverlayDisplayType.OverlayAndTooltip || _overlayDisplayTypeSetting.Value == OverlayDisplayType.TooltipOnly) {
        ShowTooltipWindow(node);
      }
    }

    private void PostRefreshTree() {
      _delayedRefreshTree.Post(TimeSpan.FromMilliseconds(200), () => {
        try {
          // Enumerate on thread pool thread, refresh on UI thread.
          UiAction(() => {
            var jvms = EnumJvms();
            UpdateTree(jvms);
          });
        } catch (Exception e) {
          UiAction(() => { LogErrorMessage(e); });
        }
      });
    }

    private AccessibleJvm GetAccessibleJvmFromNode(TreeNode node) {
      var model = (AccessibleNodeModel)node.Tag;
      return (AccessibleJvm)model.AccessibleNode;
    }

    private AccessibleWindow GetAccessibleWindowFromNode(TreeNode node) {
      var model = (AccessibleNodeModel)node.Tag;
      return (AccessibleWindow)model.AccessibleNode;
    }

    private IEnumerable<AccessibleJvm> GetAccessibleJvmsFromTree() {
      return _view.AccessibilityTree.Nodes
        .AsList()
        .Select(x => x.Tag)
        .Where(x => x != null)
        .OfType<AccessibleNodeModel>()
        .Select(x => x.AccessibleNode)
        .OfType<AccessibleJvm>();
    }

    private void DisposeTreeNode(TreeNode node) {
      DisposeTreeNodeList(node.Nodes);
      var model = node.Tag as AccessibleNodeModel;
      if (model != null) {
        model.AccessibleNode.Dispose();
      }
    }

    private void DisposeTreeNodeList(TreeNodeCollection list) {
      foreach (TreeNode node in list) {
        DisposeTreeNode(node);
      }
    }

    public void ClearSelectedNode() {
      var node = _view.AccessibilityTree.SelectedNode;
      if (node != null) {
        _view.AccessibilityTree.SelectedNode = null;
        _overlayWindowRectangle = null;
        ShowOverlayWindow();
        _view.AccessibleComponentPropertyListView.Clear();
      }
    }

    private void AccessibilityTree_BeforeExpand(object sender, TreeViewCancelEventArgs e) {
      var node = e.Node.Tag as NodeModel;
      if (node == null)
        return;
      UiAction(() => {
        node.BeforeExpand(sender, e);
      });
    }

    private void AccessibilityTree_AfterSelect(object sender, TreeViewEventArgs e) {
      var treeNode = e.Node;
      if (treeNode == null)
        return;

      var nodeModel = treeNode.Tag as AccessibleNodeModel;
      if (nodeModel == null) {
        _overlayWindowRectangle = null;
        ShowOverlayWindow();
        _view.AccessibleComponentPropertyListView.Clear();
        return;
      }

      _overlayWindowRectangle = null;
      UiAction(() => {
        _overlayWindowRectangle = nodeModel.AccessibleNode.GetScreenRectangle();
        var propertyList = nodeModel.AccessibleNode.GetProperties(PropertyOptions);
        _view.AccessibleComponentPropertyListView.SetPropertyList(propertyList);
      });

      EnsureTreeNodeVisible(treeNode);
      UpdateOverlayDisplay(nodeModel.AccessibleNode, OverlayActivation.OnTreeSelection);

      UiAction(() => {
        var navigationEntry = new NavigationEntry {
          Description = string.Format("Navigate to \"{0}\"", nodeModel.AccessibleNode.GetTitle()),
          Action = () => SelectTreeNode(nodeModel.AccessibleNode),
        };
        _navigation.AddNavigationAction(navigationEntry);
      });
    }

    private void AccessibilityTree_GotFocus(object sender, EventArgs eventArgs) {
      var treeNode = _view.AccessibilityTree.SelectedNode;
      if (treeNode == null)
        return;

      var nodeModel = treeNode.Tag as AccessibleNodeModel;
      if (nodeModel == null)
        return;

      UiAction(() => {
        UpdateOverlayDisplay(nodeModel.AccessibleNode, OverlayActivation.OnTreeSelection);
      });
    }

    private void AccessibilityTree_KeyDown(object sender, KeyEventArgs keyEventArgs) {
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
        _view.AccessibleComponentPropertyListView.SetPropertyList(propertyList);

        // Update the overlay window
        UpdateOverlayDisplay(nodeModel.AccessibleNode, OverlayActivation.OnTreeSelection);
      });
    }

    private void ComponentPropertyListView_AccessibleRectInfoSelected(object sender, AccessibleRectInfoSelectedEventArgs e) {
      _overlayWindowRectangle = e.AccessibleRectInfo.Rectangle;
      UpdateOverlayDisplay(e.AccessibleRectInfo.AccessibleNode, OverlayActivation.OnComponentSelection);
    }

    private void ComponentPropertyListView_Error(object sender, PropertyGroupErrorEventArgs e) {
      LogErrorMessage(e.GetException());
    }

    private void EnsureTreeNodeVisible(TreeNode node) {
      if (node.Level >= 2) {
        node.EnsureVisible();
      }
    }

    private void ShowTooltipWindow(AccessibleNode node) {
      if (_overlayWindowRectangle == null) {
        HideToolTip();
        return;
      }

      ShowToolTip(_overlayWindowRectangle.Value.Location, node);
    }

    private void ShowOverlayWindow() {
      if (_overlayWindowRectangle == null) {
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

    public void EnableOverlayActivationFlag(OverlayActivation value, bool enabled) {
      _overlayActivationSetting.Value = CombineActivationFlags(_overlayActivationSetting.Value, value, enabled);
    }

    private void UpdateOverlayActivation(OverlayActivation previous) {
      _hideOverlayOnFocusLost.Cancel();

      // Update UI
      if (_overlayActivationSetting.Value == OverlayActivation.None) {
        HideOverlayWindow();
        HideToolTip();
      }

      // Update event handlers
      if (_accessBridge.IsLoaded) {
        if (FlagActivated(previous, _overlayActivationSetting.Value, OverlayActivation.OnFocusGained | OverlayActivation.OnActiveDescendantChanged)) {
          _accessBridge.Events.FocusGained += AccessBridgeEvents_OverlayActivation_OnFocusGained;
          _accessBridge.Events.FocusLost += AccessBridgeEvents_OverlayActivation_OnFocusLost;
          _accessBridge.Events.JavaShutdown += AccessBridgeEvents_OverlayActivation_OnJavaShutdown;
          _accessBridge.Events.PropertyActiveDescendentChange += AccessBridgeEvents_OverlayActivation_OnActiveDescendantChanged;
        } else if (FlagDeactivated(previous, _overlayActivationSetting.Value, OverlayActivation.OnFocusGained | OverlayActivation.OnActiveDescendantChanged)) {
          _accessBridge.Events.FocusGained -= AccessBridgeEvents_OverlayActivation_OnFocusGained;
          _accessBridge.Events.FocusLost -= AccessBridgeEvents_OverlayActivation_OnFocusLost;
          _accessBridge.Events.JavaShutdown -= AccessBridgeEvents_OverlayActivation_OnJavaShutdown;
          _accessBridge.Events.PropertyActiveDescendentChange -= AccessBridgeEvents_OverlayActivation_OnActiveDescendantChanged;
        }
      }

      UpdateOverlayMenuItems();
    }

    private bool FlagActivated(OverlayActivation before, OverlayActivation after, OverlayActivation flag) {
      var before1 = before & flag;
      var after1 = after & flag;
      return before1 == 0 && after1 != 0;
    }

    private bool FlagDeactivated(OverlayActivation before, OverlayActivation after, OverlayActivation flag) {
      var before1 = before & flag;
      var after1 = after & flag;
      return before1 != 0 && after1 == 0;
    }

    private OverlayActivation CombineActivationFlags(OverlayActivation flags, OverlayActivation flag, bool enabled) {
      if (enabled) {
        return flags | flag;
      } else {
        return flags & ~flag;
      }
    }

    public void EnableAutoDetect(bool enabled) {
      _autoDetectApplicationsEnabledSetting.Value = enabled;
    }

    /// <summary>
    /// Return the <see cref="NodePath"/> of a node given a location on screen.
    /// Return <code>null</code> if there is no node at that location.
    /// </summary>
    public NodePath GetNodePathAt(Point screenPoint) {
      return UiCompute(() => {
        // Note: We should never have more than one window because
        // AccessibleWindow uses WindowFromPoint to filter out themselves if
        // needed.
        return GetAccessibleJvmsFromTree()
          .Select(node => node.GetNodePathAt(screenPoint))
          .Where(x => x != null)
          .FirstOrDefault();
      });
    }

    public void ShowOverlayForNodePath(NodePath path) {
      _overlayWindowRectangle = UiCompute(() => path.LeafNode.GetScreenRectangle());
      ShowOverlayWindow();
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
      UiAction(() => {
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
      });
    }

    private TreeNode FindTreeNodeInList(TreeNodeCollection list, AccessibleNode node) {
      // Sequential search (for Jvm, Window nodes).
      foreach (TreeNode treeNode in list) {
        var childNode = treeNode.Tag as AccessibleNodeModel;
        if (childNode == null)
          continue;

        if (childNode.AccessibleNode.Equals(node)) {
          return treeNode;
        }
      }

      // Search by child index (for transient nodes)
      var childIndex = node.GetIndexInParent();
      if (childIndex >= 0 && childIndex < list.Count) {
        return list[childIndex];
      }

      return null;
    }

    public void ShowToolTip(Point screenPoint, NodePath nodePath) {
      UiAction(() => {
        ShowToolTip(screenPoint, nodePath.LeafNode);
      });
    }

    public void ShowToolTip(Point screenPoint, AccessibleNode node) {
      UiAction(() => {
        var sb = new StringBuilder();
        foreach (var x in node.GetToolTipProperties(PropertyOptions)) {
          if (sb.Length > 0)
            sb.Append("\r\n");
          sb.AppendFormat("{0}: {1}", x.Name, PropertyListTreeViewModel.PropertyNodeValueToString(x));
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
      HideToolTip();
    }

    public void OnFocusGained() {
      ShowOverlayWindow();
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

    public class HwndCache {
      private readonly ConcurrentDictionary<IntPtr, AccessibleWindow> _cache = new ConcurrentDictionary<IntPtr, AccessibleWindow>();

      public AccessibleWindow Get(AccessBridge accessBridge, IntPtr hwnd) {
        return _cache.GetOrAdd(hwnd, key => accessBridge.CreateAccessibleWindow(key));
      }

      public void Clear() {
        _cache.Clear();
      }

      public IEnumerable<AccessibleWindow> Windows {
        get { return _cache.Values.Where(x => x != null); }
      }
    }
  }

  [Flags]
  public enum OverlayActivation {
    None = 0x0,
    OnTreeSelection = 0x01,
    OnComponentSelection = 0x02,
    OnFocusGained = 0x04,
    OnActiveDescendantChanged = 0x08,
  }

  public enum OverlayDisplayType {
    OverlayOnly,
    TooltipOnly,
    OverlayAndTooltip,
  }
}
