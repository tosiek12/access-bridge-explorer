using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  /// <summary>
  /// a key binding associates with a component
  /// </summary>
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleKeyBindingInfo {
    public AccessibleKeyCode character;                // the key character
    public AccessibleModifiers modifiers;                 // the key modifiers
  }
}
