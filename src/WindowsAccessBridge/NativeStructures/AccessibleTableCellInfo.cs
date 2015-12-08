using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using AccessibleContext = System.IntPtr;
using jint = System.Int32;
using jboolean = System.Byte;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {

  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTableCellInfo {
    public AccessibleContext accessibleContext;
    public jint index;
    public jint row;
    public jint column;
    public jint rowExtent;
    public jint columnExtent;
    public jboolean isSelected;
  }
}