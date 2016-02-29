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
using System.Reflection;
using System.Windows.Forms;

namespace AccessBridgeExplorer {
  public partial class ExplorerForm : Form, IExplorerFormView, IMessageQueue {
    private readonly WindowsHotKeyHandler _hotKeyHandler;
    private readonly ExplorerFormController _controller;
    private PropertyListView _accessibleContextPropertyList;
    private bool _capturing;
    private int _navigationVersion = int.MinValue;

    public ExplorerForm() {
      InitializeComponent();
      mainToolStrip.Renderer = new OverlayButtonRenderer(this);

      _accessibleContextPropertyList = new PropertyListView(accessibleContextPropertyList, propertyImageList);
      _controller = new ExplorerFormController(this);
      _hotKeyHandler = new WindowsHotKeyHandler();
      _hotKeyHandler.KeyPressed += HotKeyHandlerOnKeyPressed;
      SetDoubleBuffered(_accessibilityTree, true);
      SetDoubleBuffered(_messageList, true);
      SetDoubleBuffered(accessibleContextPropertyList, true);
      SetDoubleBuffered(topLevelTabControl, true);
      SetDoubleBuffered(accessibleComponentTabControl, true);
      SetDoubleBuffered(bottomTabControl, true);

      overlayEnableButton_Click(overlayEnableButton, new EventArgs());
    }

    private void MainForm_Load(object sender, EventArgs e) {
      //accessibleContextPropertyList.SizeChanged += (o, args) => SizeLastColumn(accessibleContextPropertyList);
      //SizeLastColumn(accessibleContextPropertyList);
      //eventList.SizeChanged += (o, args) => SizeLastColumn(eventList);
      //SizeLastColumn(eventList);
      //messageList.SizeChanged += (o, args) => SizeLastColumn(messageList);
      //SizeLastColumn(messageList);
    }

    private void MainForm_Shown(object sender, EventArgs e) {
      InvokeLater(() => {
        _controller.Initialize();

        LogIntroMessages();

        try {
          // OemPipe: Used for miscellaneous characters; it can vary by keyboard. For
          // the US standard keyboard, the '\|' key
          var keys = Keys.Control | Keys.OemPipe;
          _hotKeyHandler.Register(this, 1, keys);
        } catch (Exception ex) {
          _controller.LogErrorMessage(ex);
        }

        _controller.LogMessage("Ready!");
        Application.Idle += ApplicationOnIdle;
      });
    }

    private void ApplicationOnIdle(object sender, EventArgs eventArgs) {
      UpdateNavigationState();
    }

    private void UpdateNavigationState() {
      if (_navigationVersion == _controller.Navigation.Version)
        return;
      _navigationVersion = _controller.Navigation.Version;

      navigateBackwardButton.Enabled = _controller.Navigation.BackwardAvailable;
      navigateBackwardMenuItem.Enabled = _controller.Navigation.BackwardAvailable;
      AddDropDownEntries(navigateBackwardButton.DropDownItems, _controller.Navigation.BackwardEntries);

      navigateForwardButton.Enabled = _controller.Navigation.ForwardAvailable;
      navigateForwardMenuItem.Enabled = _controller.Navigation.ForwardAvailable;
      AddDropDownEntries(navigateForwardButton.DropDownItems, _controller.Navigation.ForwardEntries);
    }

    private void AddDropDownEntries(ToolStripItemCollection items, IEnumerable<NavigationEntry> entries) {
      items.Clear();
      var index = 0;
      entries.ForEach(entry => {
        if (index++ >= 20)
          return;

        var item = items.Add(entry.Description);
        item.Click += (sender, args) => {
          _controller.Navigation.NavigateTo(entry);
          UpdateNavigationState();
        };
      });
    }

    private void HotKeyHandlerOnKeyPressed(object sender, EventArgs eventArgs) {
      _controller.RefreshTree();

      var screenPoint = MousePosition;
      _controller.SelectNodeAtPoint(screenPoint);
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
      _controller.Dispose();
      Close();
    }

    private void refreshToolStripMenuItem_Click(object sender, EventArgs e) {
      _controller.RefreshTree();
    }

    private void refreshButton_Click(object sender, EventArgs e) {
      _controller.RefreshTree();
    }

    private void LogIntroMessages() {
      _controller.LogMessage("{0} allows exploring and interacting with accessibility features of Java applications.", Text);
      _controller.LogMessage("Use the \"{0}\" window to explore accessible components of active Java application windows.", accessibilityTreePage.Text);
      _controller.LogMessage("Use the \"{0}\" toolbar button to refresh the content of the \"{1}\" window.", refreshButton.Text, accessibilityTreePage.Text);
      _controller.LogMessage("Use the \"{0}\" menu to select event types to capture and display in the \"{1}\" window.", _eventsMenu.Text.Replace("&", ""), eventsPage.Text);
      _controller.LogMessage("Use the \"{0}\" toolbar button to {1}.", findComponentButton.Text, findComponentButton.ToolTipText);
      _controller.LogMessage("Use the \"Ctrl+\\\" key in any Java application window to capture the accessible component located at the mouse cursor.");
    }

    private void overlayEnableButton_Click(object sender, EventArgs e) {
      var button = (ToolStripButton)sender;
      var enable = !button.Checked;
      button.Checked = enable;
      showOverlayMenuItem.Checked = enable;
      if (enable) {
        button.ForeColor = Color.FromArgb(128, 255, 128);
      } else {
        button.ForeColor = SystemColors.InactiveCaption;
      }
      _controller.EnableOverlayWindow(enable);
    }

    private class OverlayButtonRenderer : ToolStripProfessionalRenderer {
      private readonly ExplorerForm _explorerForm;

      public OverlayButtonRenderer(ExplorerForm explorerForm) {
        _explorerForm = explorerForm;
      }

      protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e) {
        base.OnRenderButtonBackground(e);

        var btn = e.Item as ToolStripButton;
        if (object.Equals(btn, _explorerForm.overlayEnableButton)) {
          var bounds = new Rectangle(Point.Empty, e.Item.Size);
          bounds.Inflate(-1, -1);
          e.Graphics.FillRectangle(new SolidBrush(btn.ForeColor), bounds);
        }
      }
    }

    private void showOverlayMenuItem_Click(object sender, EventArgs e) {
      overlayEnableButton_Click(overlayEnableButton, new EventArgs());
    }

    private void catpureButton_MouseDown(object sender, MouseEventArgs e) {
      _controller.ClearSelectedNode();
      _controller.RefreshTree();
      _capturing = true;
      Cursor = Cursors.Cross;
      Capture = true;
    }

    private void MainForm_MouseCaptureChanged(object sender, EventArgs e) {
      Cursor = Cursors.Default;
      _capturing = false;
      _controller.HideToolTip();
    }

    private void MainForm_MouseMove(object sender, MouseEventArgs e) {
      var screenPoint = PointToScreen(e.Location);
      var nodePath = _controller.GetNodePathAt(screenPoint);
      if (nodePath != null) {
        _controller.ShowToolTip(screenPoint, nodePath);
        _controller.ShowOverlayForNodePath(nodePath);
      } else {
        _controller.HideOverlayWindow();
        _controller.HideToolTip();
      }
    }

    private void MainForm_MouseUp(object sender, MouseEventArgs e) {
      if (!_capturing)
        return;

      var screenPoint = PointToScreen(e.Location);
      _controller.SelectNodeAtPoint(screenPoint);
      Capture = false;
    }

    private void refreshTimer_Tick(object sender, EventArgs e) {
      _controller.RefreshTick();
    }

    private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
      Application.Idle -= ApplicationOnIdle;
      _controller.Dispose();
      _hotKeyHandler.Dispose();
    }

    private void MainForm_Activated(object sender, EventArgs e) {
      _controller.OnFocusGained();
    }

    private void MainForm_Deactivate(object sender, EventArgs e) {
      _controller.OnFocusLost();
    }

    private void SizeLastColumn(ListView lv) {
      lv.Columns[lv.Columns.Count - 1].Width = -2;
    }

    private void clearEventsButton_Click(object sender, EventArgs e) {
      _eventList.Items.Clear();
    }

    private void clearMessagesButton_Click(object sender, EventArgs e) {
      _messageList.Items.Clear();
    }

    private void showHelpButton_Click(object sender, EventArgs e) {
      ShowHelp();
    }

    private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e) {
      ShowHelp();
    }

    private void ShowHelp() {
      _messageList.Items.Clear();
      LogIntroMessages();
      bottomTabControl.SelectedTab = messagesPage;
      foreach (ListViewItem item in _messageList.Items) {
        item.Selected = true;
      }
      _messageList.Focus();
    }

    public static void SetDoubleBuffered(Control control, bool enable) {
      var doubleBufferPropertyInfo = control.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
      doubleBufferPropertyInfo.SetValue(control, enable, null);
    }

    public void Invoke(Action action) {
      base.Invoke(action);
    }

    public void InvokeLater(Action action) {
      base.BeginInvoke(action);
    }

    public T Compute<T>(Func<T> function) {
      return (T)base.Invoke(function);
    }

    private void navigateForwardButton_Click(object sender, EventArgs e) {
      _controller.Navigation.NavigateForward();
      UpdateNavigationState();
    }

    private void navigateForwardToolStripMenuItem_Click(object sender, EventArgs e) {
      _controller.Navigation.NavigateForward();
      UpdateNavigationState();
    }

    private void navigateBackwardButton_Click(object sender, EventArgs e) {
      _controller.Navigation.NavigateBackward();
      UpdateNavigationState();
    }

    private void navigateBackwardToolStripMenuItem_Click(object sender, EventArgs e) {
      _controller.Navigation.NavigateBackward();
      UpdateNavigationState();
    }

    #region IExplorerFormView
    IMessageQueue IExplorerFormView.MessageQueue {
      get { return this; }
    }

    TreeView IExplorerFormView.AccessibilityTree {
      get { return _accessibilityTree; }
    }

    PropertyListView IExplorerFormView.AccessibleComponentPropertyList {
      get { return _accessibleContextPropertyList; }
    }

    ListView IExplorerFormView.MessageList {
      get { return _messageList; }
    }

    ListView IExplorerFormView.EventList {
      get { return _eventList; }
    }

    ToolStripMenuItem IExplorerFormView.PropertiesMenu {
      get { return _propertiesMenu; }
    }

    ToolStripMenuItem IExplorerFormView.EventsMenu {
      get { return _eventsMenu; }
    }

    ToolStripStatusLabel IExplorerFormView.StatusLabel {
      get { return _statusLabel; }
    }

    void IExplorerFormView.ShowMessageBox(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon) {
      MessageBox.Show(this, message, title, buttons, icon);
    }

    void IExplorerFormView.ShowDialog(Form form) {
      form.ShowDialog(this);
    }
    #endregion
  }
}
