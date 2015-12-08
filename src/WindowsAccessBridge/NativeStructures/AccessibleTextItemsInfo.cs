using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTextItemsInfo {
    public char letter;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SHORT_STRING_SIZE)]
    public string word;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.MAX_STRING_SIZE)]
    public string sentence;
  }
}
