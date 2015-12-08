using System.Diagnostics.CodeAnalysis;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  /// <summary>
  /// The supported control code keys are:
  /// </summary>
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  public enum AccessibleKeyCode : ushort {
    ACCESSIBLE_VK_BACK_SPACE = 8,
    ACCESSIBLE_VK_DELETE = 127,
    ACCESSIBLE_VK_DOWN = 40,
    ACCESSIBLE_VK_END = 35,
    ACCESSIBLE_VK_HOME = 36,
    ACCESSIBLE_VK_INSERT = 155,
    ACCESSIBLE_VK_KP_DOWN = 225,
    ACCESSIBLE_VK_KP_LEFT = 226,
    ACCESSIBLE_VK_KP_RIGHT = 227,
    ACCESSIBLE_VK_KP_UP = 224,
    ACCESSIBLE_VK_LEFT = 37,
    ACCESSIBLE_VK_PAGE_DOWN = 34,
    ACCESSIBLE_VK_PAGE_UP = 33,
    ACCESSIBLE_VK_RIGHT = 39,
    ACCESSIBLE_VK_UP = 38,
  }
}