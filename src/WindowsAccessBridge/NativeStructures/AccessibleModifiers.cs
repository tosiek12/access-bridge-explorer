using System;
using System.Diagnostics.CodeAnalysis;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  /// <summary>
  /// keyboard character modifiers
  /// </summary>
  [Flags]
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  public enum AccessibleModifiers {
    ACCESSIBLE_SHIFT_KEYSTROKE = 1,
    ACCESSIBLE_CONTROL_KEYSTROKE = 2,
    ACCESSIBLE_META_KEYSTROKE = 4,
    ACCESSIBLE_ALT_KEYSTROKE = 8,
    ACCESSIBLE_ALT_GRAPH_KEYSTROKE = 16,
    ACCESSIBLE_BUTTON1_KEYSTROKE = 32,
    ACCESSIBLE_BUTTON2_KEYSTROKE = 64,
    ACCESSIBLE_BUTTON3_KEYSTROKE = 128,
    ACCESSIBLE_FKEY_KEYSTROKE = 256, // F key pressed, character contains 1-24
    ACCESSIBLE_CONTROLCODE_KEYSTROKE = 512,  // Control code key pressed, character contains control code.
  }
}