using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  /// <summary>
  /// all of the key bindings associated with a component
  /// </summary>
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleKeyBindings {
    public int keyBindingsCount;   // number of key bindings
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAX_KEY_BINDINGS)]
    public AccessibleKeyBindingInfo[] keyBindingInfo;
  }
}
