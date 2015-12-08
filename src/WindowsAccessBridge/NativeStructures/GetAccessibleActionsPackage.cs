using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using AccessibleContext = System.IntPtr;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  /// <summary>
  /// struct for requesting the actions associated with a component
  /// </summary>
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct GetAccessibleActionsPackage {
    public int vmID;
    public AccessibleContext accessibleContext;                                    // the component
    public AccessibleActions rAccessibleActions;           // the actions
  }
}
