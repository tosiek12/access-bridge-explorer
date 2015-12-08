using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using AccessBridgeExplorer.Model;

namespace AccessBridgeExplorer.WindowsAccessBridge {
  /// <summary>
  /// Root node of a tree of <see cref="AccessibleContextNode"/> instances of a
  /// specific Java Window.
  /// </summary>
  public class AccessibleWindow : AccessibleContextNode {
    private readonly IntPtr _hWnd;

    public AccessibleWindow(AccessBridge accessBridge, IntPtr hWnd, JavaObjectHandle ac) : base(accessBridge, ac) {
      _hWnd = hWnd;
    }

    public override AccessibleNode GetParent() {
      return new AccessibleJvm(AccessBridge, JvmId);
    }

    protected override void AddToolTipProperties(PropertyList list) {
      list.AddProperty("Window", _hWnd);
      base.AddToolTipProperties(list);
    }

    public string GetDisplaySortString() {
      var info = GetInfo();

      var sb = new StringBuilder();
      if (string.IsNullOrEmpty(info.role))
        sb.Append("  ");
      else if (info.role == "frame")
        sb.Append("a ");
      else
        sb.Append("z" + info.role[0]);

      sb.Append('-');
      if (string.IsNullOrEmpty(info.name))
        sb.Append('z');
      else
        sb.Append("a" + info.name);
      return sb.ToString();
    }

    public override NodePath GetNodePathAt(Point screenPoint) {
      // Bail out early if Windows says this window does not contain "screenPoint"
      // See http://blogs.msdn.com/b/oldnewthing/archive/2010/12/30/10110077.aspx
      // Multi monitor notes:
      // https://msdn.microsoft.com/en-us/library/windows/desktop/dd162827(v=vs.85).aspx
      var hwnd = WindowFromPoint(screenPoint);
      if (hwnd != _hWnd)
        return null;

      return base.GetNodePathAt(screenPoint);
    }

    public override bool Equals(AccessibleNode other) {
      if (!base.Equals(other))
        return false;

      if (!(other is AccessibleWindow))
        return false;

      return _hWnd == ((AccessibleWindow) other)._hWnd;
    }

    public override string ToString() {
      return string.Format("AccessibleWindowNode(hwnd={0})", _hWnd);
    }

    [DllImport("user32.dll")]
    static extern IntPtr WindowFromPoint(Point p);
  }

}