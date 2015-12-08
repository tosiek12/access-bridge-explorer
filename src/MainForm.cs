using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace AccessBridgeExplorer {
  public partial class MainForm : Form, IUiThreadInvoker {
    private readonly WindowsHotKeyHandler _hotKeyHandler;
    private readonly AccessibilityController _controller;
    private bool _capturing;

    public MainForm() {
      InitializeComponent();

      var accessibleContextPropertyListWrapper = new PropertyListViewWrapper(accessibleContextPropertyList, propertyImageList);
      _controller = new AccessibilityController(this, accessibilityTree, accessibleContextPropertyListWrapper, statusLabel, eventList, messageList, eventsMenu, optionsToolStripMenuItem);
      _hotKeyHandler = new WindowsHotKeyHandler();
      _hotKeyHandler.KeyPressed += HotKeyHandlerOnKeyPressed;
      SetDoubleBuffered(accessibilityTree, true);
      SetDoubleBuffered(messageList, true);
      SetDoubleBuffered(accessibleContextPropertyList, true);
      SetDoubleBuffered(topLevelTabControl, true);
      SetDoubleBuffered(accessibleComponentTabControl, true);
      SetDoubleBuffered(bottomTabControl, true);
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
        LogIntroMessages();

        // OemPipe: Used for miscellaneous characters; it can vary by keyboard. For
        // the US standard keyboard, the '\|' key
        var keys = Keys.Control | Keys.OemPipe;
        _hotKeyHandler.Register(this, 1, keys);

        _controller.Initialize();
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
      _controller.LogMessage("Use the \"{0}\" menu to select event types to capture and display in the \"{1}\" window.", eventsMenu.Text.Replace("&", ""), eventsPage.Text);
      _controller.LogMessage("Use the \"{0}\" toolbar button to {1}.", findComponentButton.Text, findComponentButton.ToolTipText);
      _controller.LogMessage("Use the \"Ctrl+\\\" key in any Java application window to capture the accessible component located at the mouse cursor.");
    }

    private void overlayEnableButton_Click(object sender, EventArgs e) {
      var button = (ToolStripButton)sender;
      if (button.Checked) {
        button.Checked = false;
        button.ForeColor = SystemColors.ButtonFace;
      } else {
        button.Checked = true;
        button.ForeColor = Color.FromArgb(192, 255, 192);
      }
      _controller.EnableOverlayWindow(!button.Checked);
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
      eventList.Items.Clear();
    }

    private void clearMessagesButton_Click(object sender, EventArgs e) {
      messageList.Items.Clear();
    }

    private void showHelpButton_Click(object sender, EventArgs e) {
      ShowHelp();
    }

    private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e) {
      ShowHelp();
    }

    private void ShowHelp() {
      messageList.Items.Clear();
      LogIntroMessages();
      bottomTabControl.SelectedTab = messagesPage;
      foreach (ListViewItem item in messageList.Items) {
        item.Selected = true;
      }
      messageList.Focus();
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
  }
}
