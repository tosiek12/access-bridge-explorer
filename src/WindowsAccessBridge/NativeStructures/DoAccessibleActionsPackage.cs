using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using jint = System.Int32;
using BOOL = System.Int32;
using AccessibleContext = System.IntPtr;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  /// <summary>
  /// struct for sending an message to do one or more actions
  /// </summary>
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct DoAccessibleActionsPackage {
    public int vmID;                         // the virtual machine ID
    public AccessibleContext accessibleContext;       // component to do the action
    public AccessibleActionsToDo actionsToDo; // the accessible actions to do
    public BOOL rResult;                      // action return value
    public jint failure;                      // index of action that failed if rResult is FALSE
  }
}
