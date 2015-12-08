using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using jint = System.Int32;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  /// <summary>
  /// all of the actions associated with a component
  /// </summary>
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public class AccessibleActions {
    public jint actionsCount;              // number of actions
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAX_ACTION_INFO)]
    public AccessibleActionInfo[] actionInfo;       // the action information
  }
}
