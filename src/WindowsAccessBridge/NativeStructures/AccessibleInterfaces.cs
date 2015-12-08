using System;
using System.Diagnostics.CodeAnalysis;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  [Flags]
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  public enum AccessibleInterfaces {
    cAccessibleValueInterface = 1, // 1 << 1 (TRUE)
    cAccessibleActionInterface = 2, // 1 << 2
    cAccessibleComponentInterface = 4, // 1 << 3
    cAccessibleSelectionInterface = 8, // 1 << 4
    cAccessibleTableInterface = 16, // 1 << 5
    cAccessibleTextInterface = 32, // 1 << 6
    cAccessibleHypertextInterface = 64 // 1 << 7
  }
}