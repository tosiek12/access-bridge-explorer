using System.Diagnostics.CodeAnalysis;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  public static class Constants {
    public const int MAX_STRING_SIZE = 1024;
    public const int SHORT_STRING_SIZE = 256;
    public const int MAX_RELATIONS = 5;
    public const int MAX_VISIBLE_CHILDREN = 256;
    public const int MAX_ACTION_INFO = 256;
    public const int MAX_ACTIONS_TO_DO = 32;
    public const int MAX_KEY_BINDINGS = 10;
    public const int MAX_ICON_INFO = 8;
    public const int MAX_RELATION_TARGETS = 25;
  }
}