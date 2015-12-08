using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using jint = System.Int32;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  /// <summary>
  /// all of the icons associated with a component
  /// </summary>
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleIcons {
    public jint iconsCount;                // number of icons
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAX_ICON_INFO)]
    public AccessibleIconInfo[] iconInfo;     // the icons
  }
}
