using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  /// <summary>
  /// an action assocated with a component
  /// </summary>
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleActionInfo {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SHORT_STRING_SIZE)]
    public string name;        // action name
  }
}
