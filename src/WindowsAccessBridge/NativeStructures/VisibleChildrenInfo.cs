using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using AccessibleContext = System.IntPtr;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct VisibleChildrenInfo {
    public int returnedChildrenCount; // number of children returned

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAX_VISIBLE_CHILDREN)]
    public AccessibleContext[] children; // the visible children
  }
}
