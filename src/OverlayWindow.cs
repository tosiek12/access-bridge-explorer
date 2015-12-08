using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AccessBridgeExplorer {
  public partial class OverlayWindow : Form {
    public OverlayWindow() {
      InitializeComponent();
    }

    public enum GWL {
      ExStyle = -20
    }

    [Flags]
    public enum WS_EX {
      Transparent = 0x20,
      /// <summary>
      /// Do not show up in alt-tab list
      /// </summary>
      ToolWindow = 0x80,
      /// <summary>
      /// See https://msdn.microsoft.com/en-us/library/windows/desktop/ms632599(v=vs.85).aspx#layered
      /// Using a layered window can significantly improve performance and
      /// visual effects for a window that has a complex shape, animates its
      /// shape, or wishes to use alpha blending effects. The system
      /// automatically composes and repaints layered windows and the windows of
      /// underlying applications. As a result, layered windows are rendered
      /// smoothly, without the flickering typical of complex window regions. In
      /// addition, layered windows can be partially translucent, that is,
      /// alpha-blended.
      /// </summary>
      Layered = 0x80000,
    }

    public enum LWA {
      ColorKey = 0x1,
      Alpha = 0x2
    }

    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    public static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);

    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    public static extern int SetWindowLong(IntPtr hWnd, GWL nIndex, int dwNewLong);

    [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
    public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, int crKey, byte alpha, LWA dwFlags);

    protected override void OnShown(EventArgs e) {
      base.OnShown(e);
      int wl = GetWindowLong(this.Handle, GWL.ExStyle);

      // See https://msdn.microsoft.com/en-us/library/windows/desktop/ms632599(v=vs.85).aspx#layered
      // However, if the layered window has the WS_EX_TRANSPARENT extended
      // window style, the shape of the layered window will be ignored and the
      // mouse events will be passed to other windows underneath the layered
      // window.
      wl = wl | (int)WS_EX.Layered | (int)WS_EX.Transparent | (int)WS_EX.ToolWindow;
      SetWindowLong(this.Handle, GWL.ExStyle, wl);
      SetLayeredWindowAttributes(this.Handle, 0, 128, LWA.Alpha);
    }

    protected override bool ShowWithoutActivation {
      get { return true; }
    }
  }
}
