using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using jint = System.Int32;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTextSelectionInfo {
    public jint selectionStartIndex;
    public jint selectionEndIndex;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.MAX_STRING_SIZE)]
    public string selectedText;
  }
}
