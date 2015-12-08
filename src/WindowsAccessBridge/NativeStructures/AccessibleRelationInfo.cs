using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using jint = System.Int32;
using AccessibleContext = System.IntPtr;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleRelationInfo {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SHORT_STRING_SIZE)]
    public string key;
    public jint targetCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAX_RELATION_TARGETS)]
    public AccessibleContext[] targets;  // AccessibleContexts
  }
}
