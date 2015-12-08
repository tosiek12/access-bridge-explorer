using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using AccessibleContext = System.IntPtr;
using jint = System.Int32;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTableInfo {
    public AccessibleContext caption; // AccessibleContext
    public AccessibleContext summary; // AccessibleContext
    public jint rowCount;
    public jint columnCount;
    public AccessibleContext accessibleContext;
    public AccessibleContext accessibleTable;
  }
}