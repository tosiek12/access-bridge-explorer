using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using AccessibleContext = System.IntPtr;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct GetAccessibleRelationSetPackage {
    public int vmID;
    public AccessibleContext accessibleContext;
    public AccessibleRelationSetInfo rAccessibleRelationSetInfo;
  }
}
