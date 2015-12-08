using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using jint = System.Int32;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTextInfo {
    public jint charCount;                 // # of characters in this text object
    public jint caretIndex;                // index of caret
    public jint indexAtPoint;              // index at the passsed in point
  }
}
