using System;
using System.Runtime.InteropServices;

namespace AccessBridgeExplorer.Win32 {
  public static class NativeMethods {
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
  }
}
