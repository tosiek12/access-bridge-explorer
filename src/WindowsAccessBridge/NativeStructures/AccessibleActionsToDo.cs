using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using jint = System.Int32;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  /// <summary>
  /// list of AccessibleActions to do
  /// </summary>
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleActionsToDo {
    public jint actionsCount;                              // number of actions to do
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAX_ACTIONS_TO_DO)]
    public AccessibleActionInfo[] actions;// the accessible actions to do
  }
}
