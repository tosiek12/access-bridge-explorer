using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessBridgeVersionInfo {
    /// <summary>
    /// output of "java -version"
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SHORT_STRING_SIZE)]
    public string VMversion;

    /// <summary>
    /// version of the AccessBridge.class
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SHORT_STRING_SIZE)]
    public string bridgeJavaClassVersion;

    /// <summary>
    /// version of JavaAccessBridge.dll
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SHORT_STRING_SIZE)]
    public string bridgeJavaDLLVersion;

    /// <summary>
    /// version of WindowsAccessBridge.dll
    /// </summary>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SHORT_STRING_SIZE)]
    public string bridgeWinDLLVersion;

  }
}