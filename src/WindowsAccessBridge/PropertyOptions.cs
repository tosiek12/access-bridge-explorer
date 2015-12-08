using System;

namespace AccessBridgeExplorer.WindowsAccessBridge {
  /// <summary>
  /// Flags specifiying what properties to include in <see cref="AccessibleNode.GetProperties"/>.
  /// </summary>
  [Flags]
  public enum PropertyOptions {
    AccessibleContextInfo = 0x0001,
    TopLevelWindowInfo = 0x0002,
    VisibleDescendents = 0x0004,
    AccessibleActions = 0x0008,
    AccessibleKeyBindings = 0x0010,
    AccessibleIcons = 0x0020,
    AccessibleRelationSet = 0x0040,
    ParentContext = 0x0080,
    AccessibleText = 0x0100,
    AccessibleValue = 0x0200,
    AccessibleSelection = 0x0400,
    AccessibleTable = 0x0800,
    AccessibleTableCells = 0x1000,
  }
}