using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using jint = System.Int32;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleRelationSetInfo {
    public jint relationCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAX_RELATIONS)]
    public AccessibleRelationInfo[] relations;
  }
}
