using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using AccessibleContext = System.IntPtr;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  /// <summary>
  /// struct to get the key bindings associated with a component
  /// </summary>
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct GetAccessibleKeyBindingsPackage {
    public int vmID;                                      // the virtual machine id
    public AccessibleContext accessibleContext;           // the component
    public AccessibleKeyBindings rAccessibleKeyBindings;  // the key bindings
  }
}
