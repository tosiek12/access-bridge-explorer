// Copyright 2016 Google Inc. All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

// ReSharper disable InconsistentNaming
// ReSharper disable DelegateSubtraction
// ReSharper disable UseObjectOrCollectionInitializer
using System;
using System.Runtime.InteropServices;
using System.Text;
using WindowHandle = System.IntPtr;
using BOOL = System.Int32;

namespace AccessBridgeExplorer.WindowsAccessBridge {
  /// <summary>
  /// Platform agnostic abstraction over WindowAccessBridge DLL functions
  /// </summary>
  public interface IAccessBridgeFunctions {
    void Windows_run();
    bool IsJavaWindow(WindowHandle window);
    bool IsSameObject(int vmid, JavaObjectHandle obj1, JavaObjectHandle obj2);
    bool GetAccessibleContextFromHWND(WindowHandle window, out int vmid, out JavaObjectHandle ac);
    WindowHandle GetHWNDFromAccessibleContext(int vmid, JavaObjectHandle ac);
    bool GetAccessibleContextAt(int vmid, JavaObjectHandle acParent, int x, int y, out JavaObjectHandle ac);
    bool GetAccessibleContextWithFocus(WindowHandle window, out int vmid, out JavaObjectHandle ac);
    bool GetAccessibleContextInfo(int vmid, JavaObjectHandle ac, out AccessibleContextInfo info);
    JavaObjectHandle GetAccessibleChildFromContext(int vmid, JavaObjectHandle ac, int i);
    JavaObjectHandle GetAccessibleParentFromContext(int vmid, JavaObjectHandle ac);
    bool GetAccessibleRelationSet(int vmid, JavaObjectHandle accessibleContext, out AccessibleRelationSetInfo relationSetInfo);
    bool GetAccessibleHypertext(int vmid, JavaObjectHandle accessibleContext, out AccessibleHypertextInfo hypertextInfo);
    bool ActivateAccessibleHyperlink(int vmid, JavaObjectHandle accessibleContext, JavaObjectHandle accessibleHyperlink);
    int GetAccessibleHyperlinkCount(int vmid, JavaObjectHandle accessibleContext);
    bool GetAccessibleHypertextExt(int vmid, JavaObjectHandle accessibleContext, int nStartIndex, out AccessibleHypertextInfo hypertextInfo);
    int GetAccessibleHypertextLinkIndex(int vmid, JavaObjectHandle hypertext, int nIndex);
    bool GetAccessibleHyperlink(int vmid, JavaObjectHandle hypertext, int nIndex, out AccessibleHyperlinkInfo hyperlinkInfo);
    bool GetAccessibleKeyBindings(int vmid, JavaObjectHandle accessibleContext, out AccessibleKeyBindings keyBindings);
    bool GetAccessibleIcons(int vmid, JavaObjectHandle accessibleContext, out AccessibleIcons icons);
    bool GetAccessibleActions(int vmid, JavaObjectHandle accessibleContext, [Out]AccessibleActions actions);
    bool DoAccessibleActions(int vmid, JavaObjectHandle accessibleContext, ref AccessibleActionsToDo actionsToDo, out int failure);
    bool GetAccessibleTextInfo(int vmid, JavaObjectHandle at, out AccessibleTextInfo textInfo, int x, int y);
    bool GetAccessibleTextItems(int vmid, JavaObjectHandle at, out AccessibleTextItemsInfo textItems, int index);
    bool GetAccessibleTextSelectionInfo(int vmid, JavaObjectHandle at, out AccessibleTextSelectionInfo textSelection);
    bool GetAccessibleTextAttributes(int vmid, JavaObjectHandle at, int index, out AccessibleTextAttributesInfo attributes);
    bool GetAccessibleTextRect(int vmid, JavaObjectHandle at, out AccessibleTextRectInfo rectInfo, int index);
    bool GetAccessibleTextLineBounds(int vmid, JavaObjectHandle at, int index, out int startIndex, out int endIndex);
    bool GetAccessibleTextRange(int vmid, JavaObjectHandle at, int start, int end, StringBuilder text, short len);
    bool GetCurrentAccessibleValueFromContext(int vmid, JavaObjectHandle av, StringBuilder value, short len);
    bool GetMaximumAccessibleValueFromContext(int vmid, JavaObjectHandle av, StringBuilder value, short len);
    bool GetMinimumAccessibleValueFromContext(int vmid, JavaObjectHandle av, StringBuilder value, short len);
    void AddAccessibleSelectionFromContext(int vmid, JavaObjectHandle asel, int i);
    void ClearAccessibleSelectionFromContext(int vmid, JavaObjectHandle asel);
    JavaObjectHandle GetAccessibleSelectionFromContext(int vmid, JavaObjectHandle asel, int i);
    int GetAccessibleSelectionCountFromContext(int vmid, JavaObjectHandle asel);
    bool IsAccessibleChildSelectedFromContext(int vmid, JavaObjectHandle asel, int i);
    void RemoveAccessibleSelectionFromContext(int vmid, JavaObjectHandle asel, int i);
    void SelectAllAccessibleSelectionFromContext(int vmid, JavaObjectHandle asel);
    bool GetAccessibleTableInfo(int vmid, JavaObjectHandle ac, out AccessibleTableInfo tableInfo);
    bool GetAccessibleTableCellInfo(int vmid, JavaObjectHandle accessibleTable, int row, int column, out AccessibleTableCellInfo tableCellInfo);
    bool GetAccessibleTableRowHeader(int vmid, JavaObjectHandle acParent, out AccessibleTableInfo tableInfo);
    bool GetAccessibleTableColumnHeader(int vmid, JavaObjectHandle acParent, out AccessibleTableInfo tableInfo);
    JavaObjectHandle GetAccessibleTableRowDescription(int vmid, JavaObjectHandle acParent, int row);
    JavaObjectHandle GetAccessibleTableColumnDescription(int vmid, JavaObjectHandle acParent, int column);
    int GetAccessibleTableRowSelectionCount(int vmid, JavaObjectHandle table);
    bool IsAccessibleTableRowSelected(int vmid, JavaObjectHandle table, int row);
    bool GetAccessibleTableRowSelections(int vmid, JavaObjectHandle table, int count, [Out]int[] selections);
    int GetAccessibleTableColumnSelectionCount(int vmid, JavaObjectHandle table);
    bool IsAccessibleTableColumnSelected(int vmid, JavaObjectHandle table, int column);
    bool GetAccessibleTableColumnSelections(int vmid, JavaObjectHandle table, int count, [Out]int[] selections);
    int GetAccessibleTableRow(int vmid, JavaObjectHandle table, int index);
    int GetAccessibleTableColumn(int vmid, JavaObjectHandle table, int index);
    int GetAccessibleTableIndex(int vmid, JavaObjectHandle table, int row, int column);
    bool SetTextContents(int vmid, JavaObjectHandle ac, string text);
    JavaObjectHandle GetParentWithRole(int vmid, JavaObjectHandle ac, string role);
    JavaObjectHandle GetParentWithRoleElseRoot(int vmid, JavaObjectHandle ac, string role);
    JavaObjectHandle GetTopLevelObject(int vmid, JavaObjectHandle ac);
    int GetObjectDepth(int vmid, JavaObjectHandle ac);
    JavaObjectHandle GetActiveDescendent(int vmid, JavaObjectHandle ac);
    bool GetVirtualAccessibleName(int vmid, JavaObjectHandle ac, StringBuilder name, int len);
    bool GetTextAttributesInRange(int vmid, JavaObjectHandle accessibleContext, int startIndex, int endIndex, out AccessibleTextAttributesInfo attributes, out short len);
    bool GetCaretLocation(int vmid, JavaObjectHandle ac, out AccessibleTextRectInfo rectInfo, int index);
    int GetVisibleChildrenCount(int vmid, JavaObjectHandle accessibleContext);
    bool GetVisibleChildren(int vmid, JavaObjectHandle accessibleContext, int startIndex, out VisibleChildrenInfo children);
    bool GetVersionInfo(int vmid, out AccessBridgeVersionInfo info);
  }

  /// <summary>
  /// Platform agnostic abstraction over WindowAccessBridge DLL events
  /// </summary>
  public interface IAccessBridgeEvents {
    event PropertyChangeEventHandler PropertyChange;
    event JavaShutdownEventHandler JavaShutdown;
    event FocusGainedEventHandler FocusGained;
    event FocusLostEventHandler FocusLost;
    event CaretUpdateEventHandler CaretUpdate;
    event MouseClickedEventHandler MouseClicked;
    event MouseEnteredEventHandler MouseEntered;
    event MouseExitedEventHandler MouseExited;
    event MousePressedEventHandler MousePressed;
    event MouseReleasedEventHandler MouseReleased;
    event MenuCanceledEventHandler MenuCanceled;
    event MenuDeselectedEventHandler MenuDeselected;
    event MenuSelectedEventHandler MenuSelected;
    event PopupMenuCanceledEventHandler PopupMenuCanceled;
    event PopupMenuWillBecomeInvisibleEventHandler PopupMenuWillBecomeInvisible;
    event PopupMenuWillBecomeVisibleEventHandler PopupMenuWillBecomeVisible;
    event PropertyNameChangeEventHandler PropertyNameChange;
    event PropertyDescriptionChangeEventHandler PropertyDescriptionChange;
    event PropertyStateChangeEventHandler PropertyStateChange;
    event PropertyValueChangeEventHandler PropertyValueChange;
    event PropertySelectionChangeEventHandler PropertySelectionChange;
    event PropertyTextChangeEventHandler PropertyTextChange;
    event PropertyCaretChangeEventHandler PropertyCaretChange;
    event PropertyVisibleDataChangeEventHandler PropertyVisibleDataChange;
    event PropertyChildChangeEventHandler PropertyChildChange;
    event PropertyActiveDescendentChangeEventHandler PropertyActiveDescendentChange;
    event PropertyTableModelChangeEventHandler PropertyTableModelChange;
  }

  #region Platform agnostic event handler delegate types
  public delegate void PropertyChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string property, string oldValue, string newValue);
  public delegate void JavaShutdownEventHandler(int vmid);
  public delegate void FocusGainedEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void FocusLostEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void CaretUpdateEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void MouseClickedEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void MouseEnteredEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void MouseExitedEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void MousePressedEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void MouseReleasedEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void MenuCanceledEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void MenuDeselectedEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void MenuSelectedEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void PopupMenuCanceledEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void PopupMenuWillBecomeInvisibleEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void PopupMenuWillBecomeVisibleEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void PropertyNameChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string oldName, string newName);
  public delegate void PropertyDescriptionChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string oldDescription, string newDescription);
  public delegate void PropertyStateChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string oldState, string newState);
  public delegate void PropertyValueChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string oldValue, string newValue);
  public delegate void PropertySelectionChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void PropertyTextChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void PropertyCaretChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source, int oldPosition, int newPosition);
  public delegate void PropertyVisibleDataChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void PropertyChildChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source, JavaObjectHandle oldChild, JavaObjectHandle newChild);
  public delegate void PropertyActiveDescendentChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source, JavaObjectHandle oldActiveDescendent, JavaObjectHandle newActiveDescendent);
  public delegate void PropertyTableModelChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle src, string oldValue, string newValue);
  #endregion

  [Flags]
  public enum AccessibleInterfaces {
    cAccessibleValueInterface = 1,
    cAccessibleActionInterface = 2,
    cAccessibleComponentInterface = 4,
    cAccessibleSelectionInterface = 8,
    cAccessibleTableInterface = 16,
    cAccessibleTextInterface = 32,
    cAccessibleHypertextInterface = 64,
  }

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

  [Flags]
  public enum AccessibleModifiers {
    ACCESSIBLE_SHIFT_KEYSTROKE = 1,
    ACCESSIBLE_CONTROL_KEYSTROKE = 2,
    ACCESSIBLE_META_KEYSTROKE = 4,
    ACCESSIBLE_ALT_KEYSTROKE = 8,
    ACCESSIBLE_ALT_GRAPH_KEYSTROKE = 16,
    ACCESSIBLE_BUTTON1_KEYSTROKE = 32,
    ACCESSIBLE_BUTTON2_KEYSTROKE = 64,
    ACCESSIBLE_BUTTON3_KEYSTROKE = 128,
    ACCESSIBLE_FKEY_KEYSTROKE = 256,
    ACCESSIBLE_CONTROLCODE_KEYSTROKE = 512,
  }

  public struct AccessBridgeVersionInfo {
    public string VMversion;
    public string bridgeJavaClassVersion;
    public string bridgeJavaDLLVersion;
    public string bridgeWinDLLVersion;
  }

  public struct AccessibleActionInfo {
    public string name;
  }

  public struct AccessibleActionsToDo {
    public int actionsCount;
    public AccessibleActionInfo[] actions;
  }

  public struct AccessibleContextInfo {
    public string name;
    public string description;
    public string role;
    public string role_en_US;
    public string states;
    public string states_en_US;
    public int indexInParent;
    public int childrenCount;
    public int x;
    public int y;
    public int width;
    public int height;
    public int accessibleComponent;
    public int accessibleAction;
    public int accessibleSelection;
    public int accessibleText;
    public AccessibleInterfaces accessibleInterfaces;
  }

  public struct AccessibleHyperlinkInfo {
    public string text;
    public int startIndex;
    public int endIndex;
    public JavaObjectHandle accessibleHyperlink;
  }

  public struct AccessibleHypertextInfo {
    public int linkCount;
    public AccessibleHyperlinkInfo[] links;
    public JavaObjectHandle accessibleHypertext;
  }

  public struct AccessibleIconInfo {
    public string description;
    public int height;
    public int width;
  }

  public struct AccessibleIcons {
    public int iconsCount;
    public AccessibleIconInfo[] iconInfo;
  }

  public struct AccessibleKeyBindingInfo {
    public AccessibleKeyCode character;
    public AccessibleModifiers modifiers;
  }

  public struct AccessibleKeyBindings {
    public int keyBindingsCount;
    public AccessibleKeyBindingInfo[] keyBindingInfo;
  }

  public struct AccessibleRelationInfo {
    public string key;
    public int targetCount;
    public JavaObjectHandle[] targets;
  }

  public struct AccessibleRelationSetInfo {
    public int relationCount;
    public AccessibleRelationInfo[] relations;
  }

  public struct AccessibleTableCellInfo {
    public JavaObjectHandle accessibleContext;
    public int index;
    public int row;
    public int column;
    public int rowExtent;
    public int columnExtent;
    public byte isSelected;
  }

  public struct AccessibleTableInfo {
    public JavaObjectHandle caption;
    public JavaObjectHandle summary;
    public int rowCount;
    public int columnCount;
    public JavaObjectHandle accessibleContext;
    public JavaObjectHandle accessibleTable;
  }

  public struct AccessibleTextAttributesInfo {
    public int bold;
    public int italic;
    public int underline;
    public int strikethrough;
    public int superscript;
    public int subscript;
    public string backgroundColor;
    public string foregroundColor;
    public string fontFamily;
    public int fontSize;
    public int alignment;
    public int bidiLevel;
    public float firstLineIndent;
    public float leftIndent;
    public float rightIndent;
    public float lineSpacing;
    public float spaceAbove;
    public float spaceBelow;
    public string fullAttributesString;
  }

  public struct AccessibleTextInfo {
    public int charCount;
    public int caretIndex;
    public int indexAtPoint;
  }

  public struct AccessibleTextItemsInfo {
    public char letter;
    public string word;
    public string sentence;
  }

  public struct AccessibleTextRectInfo {
    public int x;
    public int y;
    public int width;
    public int height;
  }

  public struct AccessibleTextSelectionInfo {
    public int selectionStartIndex;
    public int selectionEndIndex;
    public string selectedText;
  }

  public struct VisibleChildrenInfo {
    public int returnedChildrenCount;
    public JavaObjectHandle[] children;
  }

  public class AccessibleActions {
    public int actionsCount;
    public AccessibleActionInfo[] actionInfo;
  }

  /// <summary>
  /// Implementation of platform agnostic functions
  /// </summary>
  public partial class AccessBridgeFunctions : IAccessBridgeFunctions {

    #region Function implementations

    public void Windows_run() {
      LibraryFunctions.Windows_run();
    }

    public bool IsJavaWindow(WindowHandle window) {
      var result = LibraryFunctions.IsJavaWindow(window);
      return ToBool(result);
    }

    public bool IsSameObject(int vmid, JavaObjectHandle obj1, JavaObjectHandle obj2) {
      var result = LibraryFunctions.IsSameObject(vmid, Unwrap(vmid, obj1), Unwrap(vmid, obj2));
      GC.KeepAlive(obj1);
      GC.KeepAlive(obj2);
      return ToBool(result);
    }

    public bool GetAccessibleContextFromHWND(WindowHandle window, out int vmid, out JavaObjectHandle ac) {
      JOBJECT64 acTemp;
      var result = LibraryFunctions.GetAccessibleContextFromHWND(window, out vmid, out acTemp);
      ac = Wrap(vmid, acTemp);
      return ToBool(result);
    }

    public WindowHandle GetHWNDFromAccessibleContext(int vmid, JavaObjectHandle ac) {
      var result = LibraryFunctions.GetHWNDFromAccessibleContext(vmid, Unwrap(vmid, ac));
      GC.KeepAlive(ac);
      return result;
    }

    public bool GetAccessibleContextAt(int vmid, JavaObjectHandle acParent, int x, int y, out JavaObjectHandle ac) {
      JOBJECT64 acTemp;
      var result = LibraryFunctions.GetAccessibleContextAt(vmid, Unwrap(vmid, acParent), x, y, out acTemp);
      GC.KeepAlive(acParent);
      ac = Wrap(vmid, acTemp);
      return ToBool(result);
    }

    public bool GetAccessibleContextWithFocus(WindowHandle window, out int vmid, out JavaObjectHandle ac) {
      JOBJECT64 acTemp;
      var result = LibraryFunctions.GetAccessibleContextWithFocus(window, out vmid, out acTemp);
      ac = Wrap(vmid, acTemp);
      return ToBool(result);
    }

    public bool GetAccessibleContextInfo(int vmid, JavaObjectHandle ac, out AccessibleContextInfo info) {
      AccessibleContextInfoNative infoTemp;
      var result = LibraryFunctions.GetAccessibleContextInfo(vmid, Unwrap(vmid, ac), out infoTemp);
      GC.KeepAlive(ac);
      info = Wrap(vmid, infoTemp);
      return ToBool(result);
    }

    public JavaObjectHandle GetAccessibleChildFromContext(int vmid, JavaObjectHandle ac, int i) {
      var result = LibraryFunctions.GetAccessibleChildFromContext(vmid, Unwrap(vmid, ac), i);
      GC.KeepAlive(ac);
      return Wrap(vmid, result);
    }

    public JavaObjectHandle GetAccessibleParentFromContext(int vmid, JavaObjectHandle ac) {
      var result = LibraryFunctions.GetAccessibleParentFromContext(vmid, Unwrap(vmid, ac));
      GC.KeepAlive(ac);
      return Wrap(vmid, result);
    }

    public bool GetAccessibleRelationSet(int vmid, JavaObjectHandle accessibleContext, out AccessibleRelationSetInfo relationSetInfo) {
      AccessibleRelationSetInfoNative relationSetInfoTemp;
      var result = LibraryFunctions.GetAccessibleRelationSet(vmid, Unwrap(vmid, accessibleContext), out relationSetInfoTemp);
      GC.KeepAlive(accessibleContext);
      relationSetInfo = Wrap(vmid, relationSetInfoTemp);
      return ToBool(result);
    }

    public bool GetAccessibleHypertext(int vmid, JavaObjectHandle accessibleContext, out AccessibleHypertextInfo hypertextInfo) {
      AccessibleHypertextInfoNative hypertextInfoTemp;
      var result = LibraryFunctions.GetAccessibleHypertext(vmid, Unwrap(vmid, accessibleContext), out hypertextInfoTemp);
      GC.KeepAlive(accessibleContext);
      hypertextInfo = Wrap(vmid, hypertextInfoTemp);
      return ToBool(result);
    }

    public bool ActivateAccessibleHyperlink(int vmid, JavaObjectHandle accessibleContext, JavaObjectHandle accessibleHyperlink) {
      var result = LibraryFunctions.ActivateAccessibleHyperlink(vmid, Unwrap(vmid, accessibleContext), Unwrap(vmid, accessibleHyperlink));
      GC.KeepAlive(accessibleContext);
      GC.KeepAlive(accessibleHyperlink);
      return ToBool(result);
    }

    public int GetAccessibleHyperlinkCount(int vmid, JavaObjectHandle accessibleContext) {
      var result = LibraryFunctions.GetAccessibleHyperlinkCount(vmid, Unwrap(vmid, accessibleContext));
      GC.KeepAlive(accessibleContext);
      return result;
    }

    public bool GetAccessibleHypertextExt(int vmid, JavaObjectHandle accessibleContext, int nStartIndex, out AccessibleHypertextInfo hypertextInfo) {
      AccessibleHypertextInfoNative hypertextInfoTemp;
      var result = LibraryFunctions.GetAccessibleHypertextExt(vmid, Unwrap(vmid, accessibleContext), nStartIndex, out hypertextInfoTemp);
      GC.KeepAlive(accessibleContext);
      hypertextInfo = Wrap(vmid, hypertextInfoTemp);
      return ToBool(result);
    }

    public int GetAccessibleHypertextLinkIndex(int vmid, JavaObjectHandle hypertext, int nIndex) {
      var result = LibraryFunctions.GetAccessibleHypertextLinkIndex(vmid, Unwrap(vmid, hypertext), nIndex);
      GC.KeepAlive(hypertext);
      return result;
    }

    public bool GetAccessibleHyperlink(int vmid, JavaObjectHandle hypertext, int nIndex, out AccessibleHyperlinkInfo hyperlinkInfo) {
      AccessibleHyperlinkInfoNative hyperlinkInfoTemp;
      var result = LibraryFunctions.GetAccessibleHyperlink(vmid, Unwrap(vmid, hypertext), nIndex, out hyperlinkInfoTemp);
      GC.KeepAlive(hypertext);
      hyperlinkInfo = Wrap(vmid, hyperlinkInfoTemp);
      return ToBool(result);
    }

    public bool GetAccessibleKeyBindings(int vmid, JavaObjectHandle accessibleContext, out AccessibleKeyBindings keyBindings) {
      AccessibleKeyBindingsNative keyBindingsTemp;
      var result = LibraryFunctions.GetAccessibleKeyBindings(vmid, Unwrap(vmid, accessibleContext), out keyBindingsTemp);
      GC.KeepAlive(accessibleContext);
      keyBindings = Wrap(vmid, keyBindingsTemp);
      return ToBool(result);
    }

    public bool GetAccessibleIcons(int vmid, JavaObjectHandle accessibleContext, out AccessibleIcons icons) {
      AccessibleIconsNative iconsTemp;
      var result = LibraryFunctions.GetAccessibleIcons(vmid, Unwrap(vmid, accessibleContext), out iconsTemp);
      GC.KeepAlive(accessibleContext);
      icons = Wrap(vmid, iconsTemp);
      return ToBool(result);
    }

    public bool GetAccessibleActions(int vmid, JavaObjectHandle accessibleContext, [Out]AccessibleActions actions) {
      AccessibleActionsNative actionsTemp = new AccessibleActionsNative();
      var result = LibraryFunctions.GetAccessibleActions(vmid, Unwrap(vmid, accessibleContext), actionsTemp);
      GC.KeepAlive(accessibleContext);
      CopyWrap(vmid, actionsTemp, actions);
      return ToBool(result);
    }

    public bool DoAccessibleActions(int vmid, JavaObjectHandle accessibleContext, ref AccessibleActionsToDo actionsToDo, out int failure) {
      AccessibleActionsToDoNative actionsToDoTemp = Unwrap(vmid, actionsToDo);
      var result = LibraryFunctions.DoAccessibleActions(vmid, Unwrap(vmid, accessibleContext), ref actionsToDoTemp, out failure);
      GC.KeepAlive(accessibleContext);
      actionsToDo = Wrap(vmid, actionsToDoTemp);
      return ToBool(result);
    }

    public bool GetAccessibleTextInfo(int vmid, JavaObjectHandle at, out AccessibleTextInfo textInfo, int x, int y) {
      AccessibleTextInfoNative textInfoTemp;
      var result = LibraryFunctions.GetAccessibleTextInfo(vmid, Unwrap(vmid, at), out textInfoTemp, x, y);
      GC.KeepAlive(at);
      textInfo = Wrap(vmid, textInfoTemp);
      return ToBool(result);
    }

    public bool GetAccessibleTextItems(int vmid, JavaObjectHandle at, out AccessibleTextItemsInfo textItems, int index) {
      AccessibleTextItemsInfoNative textItemsTemp;
      var result = LibraryFunctions.GetAccessibleTextItems(vmid, Unwrap(vmid, at), out textItemsTemp, index);
      GC.KeepAlive(at);
      textItems = Wrap(vmid, textItemsTemp);
      return ToBool(result);
    }

    public bool GetAccessibleTextSelectionInfo(int vmid, JavaObjectHandle at, out AccessibleTextSelectionInfo textSelection) {
      AccessibleTextSelectionInfoNative textSelectionTemp;
      var result = LibraryFunctions.GetAccessibleTextSelectionInfo(vmid, Unwrap(vmid, at), out textSelectionTemp);
      GC.KeepAlive(at);
      textSelection = Wrap(vmid, textSelectionTemp);
      return ToBool(result);
    }

    public bool GetAccessibleTextAttributes(int vmid, JavaObjectHandle at, int index, out AccessibleTextAttributesInfo attributes) {
      AccessibleTextAttributesInfoNative attributesTemp;
      var result = LibraryFunctions.GetAccessibleTextAttributes(vmid, Unwrap(vmid, at), index, out attributesTemp);
      GC.KeepAlive(at);
      attributes = Wrap(vmid, attributesTemp);
      return ToBool(result);
    }

    public bool GetAccessibleTextRect(int vmid, JavaObjectHandle at, out AccessibleTextRectInfo rectInfo, int index) {
      AccessibleTextRectInfoNative rectInfoTemp;
      var result = LibraryFunctions.GetAccessibleTextRect(vmid, Unwrap(vmid, at), out rectInfoTemp, index);
      GC.KeepAlive(at);
      rectInfo = Wrap(vmid, rectInfoTemp);
      return ToBool(result);
    }

    public bool GetAccessibleTextLineBounds(int vmid, JavaObjectHandle at, int index, out int startIndex, out int endIndex) {
      var result = LibraryFunctions.GetAccessibleTextLineBounds(vmid, Unwrap(vmid, at), index, out startIndex, out endIndex);
      GC.KeepAlive(at);
      return ToBool(result);
    }

    public bool GetAccessibleTextRange(int vmid, JavaObjectHandle at, int start, int end, StringBuilder text, short len) {
      var result = LibraryFunctions.GetAccessibleTextRange(vmid, Unwrap(vmid, at), start, end, text, len);
      GC.KeepAlive(at);
      return ToBool(result);
    }

    public bool GetCurrentAccessibleValueFromContext(int vmid, JavaObjectHandle av, StringBuilder value, short len) {
      var result = LibraryFunctions.GetCurrentAccessibleValueFromContext(vmid, Unwrap(vmid, av), value, len);
      GC.KeepAlive(av);
      return ToBool(result);
    }

    public bool GetMaximumAccessibleValueFromContext(int vmid, JavaObjectHandle av, StringBuilder value, short len) {
      var result = LibraryFunctions.GetMaximumAccessibleValueFromContext(vmid, Unwrap(vmid, av), value, len);
      GC.KeepAlive(av);
      return ToBool(result);
    }

    public bool GetMinimumAccessibleValueFromContext(int vmid, JavaObjectHandle av, StringBuilder value, short len) {
      var result = LibraryFunctions.GetMinimumAccessibleValueFromContext(vmid, Unwrap(vmid, av), value, len);
      GC.KeepAlive(av);
      return ToBool(result);
    }

    public void AddAccessibleSelectionFromContext(int vmid, JavaObjectHandle asel, int i) {
      LibraryFunctions.AddAccessibleSelectionFromContext(vmid, Unwrap(vmid, asel), i);
      GC.KeepAlive(asel);
    }

    public void ClearAccessibleSelectionFromContext(int vmid, JavaObjectHandle asel) {
      LibraryFunctions.ClearAccessibleSelectionFromContext(vmid, Unwrap(vmid, asel));
      GC.KeepAlive(asel);
    }

    public JavaObjectHandle GetAccessibleSelectionFromContext(int vmid, JavaObjectHandle asel, int i) {
      var result = LibraryFunctions.GetAccessibleSelectionFromContext(vmid, Unwrap(vmid, asel), i);
      GC.KeepAlive(asel);
      return Wrap(vmid, result);
    }

    public int GetAccessibleSelectionCountFromContext(int vmid, JavaObjectHandle asel) {
      var result = LibraryFunctions.GetAccessibleSelectionCountFromContext(vmid, Unwrap(vmid, asel));
      GC.KeepAlive(asel);
      return result;
    }

    public bool IsAccessibleChildSelectedFromContext(int vmid, JavaObjectHandle asel, int i) {
      var result = LibraryFunctions.IsAccessibleChildSelectedFromContext(vmid, Unwrap(vmid, asel), i);
      GC.KeepAlive(asel);
      return ToBool(result);
    }

    public void RemoveAccessibleSelectionFromContext(int vmid, JavaObjectHandle asel, int i) {
      LibraryFunctions.RemoveAccessibleSelectionFromContext(vmid, Unwrap(vmid, asel), i);
      GC.KeepAlive(asel);
    }

    public void SelectAllAccessibleSelectionFromContext(int vmid, JavaObjectHandle asel) {
      LibraryFunctions.SelectAllAccessibleSelectionFromContext(vmid, Unwrap(vmid, asel));
      GC.KeepAlive(asel);
    }

    public bool GetAccessibleTableInfo(int vmid, JavaObjectHandle ac, out AccessibleTableInfo tableInfo) {
      AccessibleTableInfoNative tableInfoTemp;
      var result = LibraryFunctions.GetAccessibleTableInfo(vmid, Unwrap(vmid, ac), out tableInfoTemp);
      GC.KeepAlive(ac);
      tableInfo = Wrap(vmid, tableInfoTemp);
      return ToBool(result);
    }

    public bool GetAccessibleTableCellInfo(int vmid, JavaObjectHandle accessibleTable, int row, int column, out AccessibleTableCellInfo tableCellInfo) {
      AccessibleTableCellInfoNative tableCellInfoTemp;
      var result = LibraryFunctions.GetAccessibleTableCellInfo(vmid, Unwrap(vmid, accessibleTable), row, column, out tableCellInfoTemp);
      GC.KeepAlive(accessibleTable);
      tableCellInfo = Wrap(vmid, tableCellInfoTemp);
      return ToBool(result);
    }

    public bool GetAccessibleTableRowHeader(int vmid, JavaObjectHandle acParent, out AccessibleTableInfo tableInfo) {
      AccessibleTableInfoNative tableInfoTemp;
      var result = LibraryFunctions.GetAccessibleTableRowHeader(vmid, Unwrap(vmid, acParent), out tableInfoTemp);
      GC.KeepAlive(acParent);
      tableInfo = Wrap(vmid, tableInfoTemp);
      return ToBool(result);
    }

    public bool GetAccessibleTableColumnHeader(int vmid, JavaObjectHandle acParent, out AccessibleTableInfo tableInfo) {
      AccessibleTableInfoNative tableInfoTemp;
      var result = LibraryFunctions.GetAccessibleTableColumnHeader(vmid, Unwrap(vmid, acParent), out tableInfoTemp);
      GC.KeepAlive(acParent);
      tableInfo = Wrap(vmid, tableInfoTemp);
      return ToBool(result);
    }

    public JavaObjectHandle GetAccessibleTableRowDescription(int vmid, JavaObjectHandle acParent, int row) {
      var result = LibraryFunctions.GetAccessibleTableRowDescription(vmid, Unwrap(vmid, acParent), row);
      GC.KeepAlive(acParent);
      return Wrap(vmid, result);
    }

    public JavaObjectHandle GetAccessibleTableColumnDescription(int vmid, JavaObjectHandle acParent, int column) {
      var result = LibraryFunctions.GetAccessibleTableColumnDescription(vmid, Unwrap(vmid, acParent), column);
      GC.KeepAlive(acParent);
      return Wrap(vmid, result);
    }

    public int GetAccessibleTableRowSelectionCount(int vmid, JavaObjectHandle table) {
      var result = LibraryFunctions.GetAccessibleTableRowSelectionCount(vmid, Unwrap(vmid, table));
      GC.KeepAlive(table);
      return result;
    }

    public bool IsAccessibleTableRowSelected(int vmid, JavaObjectHandle table, int row) {
      var result = LibraryFunctions.IsAccessibleTableRowSelected(vmid, Unwrap(vmid, table), row);
      GC.KeepAlive(table);
      return ToBool(result);
    }

    public bool GetAccessibleTableRowSelections(int vmid, JavaObjectHandle table, int count, [Out]int[] selections) {
      var result = LibraryFunctions.GetAccessibleTableRowSelections(vmid, Unwrap(vmid, table), count, selections);
      GC.KeepAlive(table);
      return ToBool(result);
    }

    public int GetAccessibleTableColumnSelectionCount(int vmid, JavaObjectHandle table) {
      var result = LibraryFunctions.GetAccessibleTableColumnSelectionCount(vmid, Unwrap(vmid, table));
      GC.KeepAlive(table);
      return result;
    }

    public bool IsAccessibleTableColumnSelected(int vmid, JavaObjectHandle table, int column) {
      var result = LibraryFunctions.IsAccessibleTableColumnSelected(vmid, Unwrap(vmid, table), column);
      GC.KeepAlive(table);
      return ToBool(result);
    }

    public bool GetAccessibleTableColumnSelections(int vmid, JavaObjectHandle table, int count, [Out]int[] selections) {
      var result = LibraryFunctions.GetAccessibleTableColumnSelections(vmid, Unwrap(vmid, table), count, selections);
      GC.KeepAlive(table);
      return ToBool(result);
    }

    public int GetAccessibleTableRow(int vmid, JavaObjectHandle table, int index) {
      var result = LibraryFunctions.GetAccessibleTableRow(vmid, Unwrap(vmid, table), index);
      GC.KeepAlive(table);
      return result;
    }

    public int GetAccessibleTableColumn(int vmid, JavaObjectHandle table, int index) {
      var result = LibraryFunctions.GetAccessibleTableColumn(vmid, Unwrap(vmid, table), index);
      GC.KeepAlive(table);
      return result;
    }

    public int GetAccessibleTableIndex(int vmid, JavaObjectHandle table, int row, int column) {
      var result = LibraryFunctions.GetAccessibleTableIndex(vmid, Unwrap(vmid, table), row, column);
      GC.KeepAlive(table);
      return result;
    }

    public bool SetTextContents(int vmid, JavaObjectHandle ac, string text) {
      var result = LibraryFunctions.SetTextContents(vmid, Unwrap(vmid, ac), text);
      GC.KeepAlive(ac);
      return ToBool(result);
    }

    public JavaObjectHandle GetParentWithRole(int vmid, JavaObjectHandle ac, string role) {
      var result = LibraryFunctions.GetParentWithRole(vmid, Unwrap(vmid, ac), role);
      GC.KeepAlive(ac);
      return Wrap(vmid, result);
    }

    public JavaObjectHandle GetParentWithRoleElseRoot(int vmid, JavaObjectHandle ac, string role) {
      var result = LibraryFunctions.GetParentWithRoleElseRoot(vmid, Unwrap(vmid, ac), role);
      GC.KeepAlive(ac);
      return Wrap(vmid, result);
    }

    public JavaObjectHandle GetTopLevelObject(int vmid, JavaObjectHandle ac) {
      var result = LibraryFunctions.GetTopLevelObject(vmid, Unwrap(vmid, ac));
      GC.KeepAlive(ac);
      return Wrap(vmid, result);
    }

    public int GetObjectDepth(int vmid, JavaObjectHandle ac) {
      var result = LibraryFunctions.GetObjectDepth(vmid, Unwrap(vmid, ac));
      GC.KeepAlive(ac);
      return result;
    }

    public JavaObjectHandle GetActiveDescendent(int vmid, JavaObjectHandle ac) {
      var result = LibraryFunctions.GetActiveDescendent(vmid, Unwrap(vmid, ac));
      GC.KeepAlive(ac);
      return Wrap(vmid, result);
    }

    public bool GetVirtualAccessibleName(int vmid, JavaObjectHandle ac, StringBuilder name, int len) {
      var result = LibraryFunctions.GetVirtualAccessibleName(vmid, Unwrap(vmid, ac), name, len);
      GC.KeepAlive(ac);
      return ToBool(result);
    }

    public bool GetTextAttributesInRange(int vmid, JavaObjectHandle accessibleContext, int startIndex, int endIndex, out AccessibleTextAttributesInfo attributes, out short len) {
      AccessibleTextAttributesInfoNative attributesTemp;
      var result = LibraryFunctions.GetTextAttributesInRange(vmid, Unwrap(vmid, accessibleContext), startIndex, endIndex, out attributesTemp, out len);
      GC.KeepAlive(accessibleContext);
      attributes = Wrap(vmid, attributesTemp);
      return ToBool(result);
    }

    public bool GetCaretLocation(int vmid, JavaObjectHandle ac, out AccessibleTextRectInfo rectInfo, int index) {
      AccessibleTextRectInfoNative rectInfoTemp;
      var result = LibraryFunctions.GetCaretLocation(vmid, Unwrap(vmid, ac), out rectInfoTemp, index);
      GC.KeepAlive(ac);
      rectInfo = Wrap(vmid, rectInfoTemp);
      return ToBool(result);
    }

    public int GetVisibleChildrenCount(int vmid, JavaObjectHandle accessibleContext) {
      var result = LibraryFunctions.GetVisibleChildrenCount(vmid, Unwrap(vmid, accessibleContext));
      GC.KeepAlive(accessibleContext);
      return result;
    }

    public bool GetVisibleChildren(int vmid, JavaObjectHandle accessibleContext, int startIndex, out VisibleChildrenInfo children) {
      VisibleChildrenInfoNative childrenTemp;
      var result = LibraryFunctions.GetVisibleChildren(vmid, Unwrap(vmid, accessibleContext), startIndex, out childrenTemp);
      GC.KeepAlive(accessibleContext);
      children = Wrap(vmid, childrenTemp);
      return ToBool(result);
    }

    public bool GetVersionInfo(int vmid, out AccessBridgeVersionInfo info) {
      AccessBridgeVersionInfoNative infoTemp;
      var result = LibraryFunctions.GetVersionInfo(vmid, out infoTemp);
      info = Wrap(vmid, infoTemp);
      return ToBool(result);
    }

    #endregion

    #region Wrap/Unwrap structs

    private AccessBridgeVersionInfo Wrap(int vmid, AccessBridgeVersionInfoNative info) {
      var result = new AccessBridgeVersionInfo();
      result.VMversion = info.VMversion;
      result.bridgeJavaClassVersion = info.bridgeJavaClassVersion;
      result.bridgeJavaDLLVersion = info.bridgeJavaDLLVersion;
      result.bridgeWinDLLVersion = info.bridgeWinDLLVersion;
      return result;
    }

    private AccessBridgeVersionInfoNative Unwrap(int vmid, AccessBridgeVersionInfo info) {
      var result = new AccessBridgeVersionInfoNative();
      result.VMversion = info.VMversion;
      result.bridgeJavaClassVersion = info.bridgeJavaClassVersion;
      result.bridgeJavaDLLVersion = info.bridgeJavaDLLVersion;
      result.bridgeWinDLLVersion = info.bridgeWinDLLVersion;
      return result;
    }

    private AccessibleActionInfo Wrap(int vmid, AccessibleActionInfoNative info) {
      var result = new AccessibleActionInfo();
      result.name = info.name;
      return result;
    }

    private AccessibleActionInfoNative Unwrap(int vmid, AccessibleActionInfo info) {
      var result = new AccessibleActionInfoNative();
      result.name = info.name;
      return result;
    }

    private AccessibleActionsToDo Wrap(int vmid, AccessibleActionsToDoNative info) {
      var result = new AccessibleActionsToDo();
      result.actionsCount = info.actionsCount;
      if (info.actions != null) {
        var count = info.actionsCount;
        result.actions = new AccessibleActionInfo[count];
        for(var i = 0; i < count; i++) {
          result.actions[i] = Wrap(vmid, info.actions[i]);
        }
      }
      return result;
    }

    private AccessibleActionsToDoNative Unwrap(int vmid, AccessibleActionsToDo info) {
      var result = new AccessibleActionsToDoNative();
      result.actionsCount = info.actionsCount;
      if (info.actions != null) {
        var count = info.actionsCount;
        result.actions = new AccessibleActionInfoNative[count];
        for(var i = 0; i < count; i++) {
          result.actions[i] = Unwrap(vmid, info.actions[i]);
        }
      }
      return result;
    }

    private AccessibleContextInfo Wrap(int vmid, AccessibleContextInfoNative info) {
      var result = new AccessibleContextInfo();
      result.name = info.name;
      result.description = info.description;
      result.role = info.role;
      result.role_en_US = info.role_en_US;
      result.states = info.states;
      result.states_en_US = info.states_en_US;
      result.indexInParent = info.indexInParent;
      result.childrenCount = info.childrenCount;
      result.x = info.x;
      result.y = info.y;
      result.width = info.width;
      result.height = info.height;
      result.accessibleComponent = info.accessibleComponent;
      result.accessibleAction = info.accessibleAction;
      result.accessibleSelection = info.accessibleSelection;
      result.accessibleText = info.accessibleText;
      result.accessibleInterfaces = info.accessibleInterfaces;
      return result;
    }

    private AccessibleContextInfoNative Unwrap(int vmid, AccessibleContextInfo info) {
      var result = new AccessibleContextInfoNative();
      result.name = info.name;
      result.description = info.description;
      result.role = info.role;
      result.role_en_US = info.role_en_US;
      result.states = info.states;
      result.states_en_US = info.states_en_US;
      result.indexInParent = info.indexInParent;
      result.childrenCount = info.childrenCount;
      result.x = info.x;
      result.y = info.y;
      result.width = info.width;
      result.height = info.height;
      result.accessibleComponent = info.accessibleComponent;
      result.accessibleAction = info.accessibleAction;
      result.accessibleSelection = info.accessibleSelection;
      result.accessibleText = info.accessibleText;
      result.accessibleInterfaces = info.accessibleInterfaces;
      return result;
    }

    private AccessibleHyperlinkInfo Wrap(int vmid, AccessibleHyperlinkInfoNative info) {
      var result = new AccessibleHyperlinkInfo();
      result.text = info.text;
      result.startIndex = info.startIndex;
      result.endIndex = info.endIndex;
      result.accessibleHyperlink = Wrap(vmid, info.accessibleHyperlink);
      return result;
    }

    private AccessibleHyperlinkInfoNative Unwrap(int vmid, AccessibleHyperlinkInfo info) {
      var result = new AccessibleHyperlinkInfoNative();
      result.text = info.text;
      result.startIndex = info.startIndex;
      result.endIndex = info.endIndex;
      result.accessibleHyperlink = Unwrap(vmid, info.accessibleHyperlink);
      return result;
    }

    private AccessibleHypertextInfo Wrap(int vmid, AccessibleHypertextInfoNative info) {
      var result = new AccessibleHypertextInfo();
      result.linkCount = info.linkCount;
      if (info.links != null) {
        var count = info.linkCount;
        result.links = new AccessibleHyperlinkInfo[count];
        for(var i = 0; i < count; i++) {
          result.links[i] = Wrap(vmid, info.links[i]);
        }
      }
      result.accessibleHypertext = Wrap(vmid, info.accessibleHypertext);
      return result;
    }

    private AccessibleHypertextInfoNative Unwrap(int vmid, AccessibleHypertextInfo info) {
      var result = new AccessibleHypertextInfoNative();
      result.linkCount = info.linkCount;
      if (info.links != null) {
        var count = info.linkCount;
        result.links = new AccessibleHyperlinkInfoNative[count];
        for(var i = 0; i < count; i++) {
          result.links[i] = Unwrap(vmid, info.links[i]);
        }
      }
      result.accessibleHypertext = Unwrap(vmid, info.accessibleHypertext);
      return result;
    }

    private AccessibleIconInfo Wrap(int vmid, AccessibleIconInfoNative info) {
      var result = new AccessibleIconInfo();
      result.description = info.description;
      result.height = info.height;
      result.width = info.width;
      return result;
    }

    private AccessibleIconInfoNative Unwrap(int vmid, AccessibleIconInfo info) {
      var result = new AccessibleIconInfoNative();
      result.description = info.description;
      result.height = info.height;
      result.width = info.width;
      return result;
    }

    private AccessibleIcons Wrap(int vmid, AccessibleIconsNative info) {
      var result = new AccessibleIcons();
      result.iconsCount = info.iconsCount;
      if (info.iconInfo != null) {
        var count = info.iconsCount;
        result.iconInfo = new AccessibleIconInfo[count];
        for(var i = 0; i < count; i++) {
          result.iconInfo[i] = Wrap(vmid, info.iconInfo[i]);
        }
      }
      return result;
    }

    private AccessibleIconsNative Unwrap(int vmid, AccessibleIcons info) {
      var result = new AccessibleIconsNative();
      result.iconsCount = info.iconsCount;
      if (info.iconInfo != null) {
        var count = info.iconsCount;
        result.iconInfo = new AccessibleIconInfoNative[count];
        for(var i = 0; i < count; i++) {
          result.iconInfo[i] = Unwrap(vmid, info.iconInfo[i]);
        }
      }
      return result;
    }

    private AccessibleKeyBindingInfo Wrap(int vmid, AccessibleKeyBindingInfoNative info) {
      var result = new AccessibleKeyBindingInfo();
      result.character = info.character;
      result.modifiers = info.modifiers;
      return result;
    }

    private AccessibleKeyBindingInfoNative Unwrap(int vmid, AccessibleKeyBindingInfo info) {
      var result = new AccessibleKeyBindingInfoNative();
      result.character = info.character;
      result.modifiers = info.modifiers;
      return result;
    }

    private AccessibleKeyBindings Wrap(int vmid, AccessibleKeyBindingsNative info) {
      var result = new AccessibleKeyBindings();
      result.keyBindingsCount = info.keyBindingsCount;
      if (info.keyBindingInfo != null) {
        var count = info.keyBindingsCount;
        result.keyBindingInfo = new AccessibleKeyBindingInfo[count];
        for(var i = 0; i < count; i++) {
          result.keyBindingInfo[i] = Wrap(vmid, info.keyBindingInfo[i]);
        }
      }
      return result;
    }

    private AccessibleKeyBindingsNative Unwrap(int vmid, AccessibleKeyBindings info) {
      var result = new AccessibleKeyBindingsNative();
      result.keyBindingsCount = info.keyBindingsCount;
      if (info.keyBindingInfo != null) {
        var count = info.keyBindingsCount;
        result.keyBindingInfo = new AccessibleKeyBindingInfoNative[count];
        for(var i = 0; i < count; i++) {
          result.keyBindingInfo[i] = Unwrap(vmid, info.keyBindingInfo[i]);
        }
      }
      return result;
    }

    private AccessibleRelationInfo Wrap(int vmid, AccessibleRelationInfoNative info) {
      var result = new AccessibleRelationInfo();
      result.key = info.key;
      result.targetCount = info.targetCount;
      if (info.targets != null) {
        var count = info.targetCount;
        result.targets = new JavaObjectHandle[count];
        for(var i = 0; i < count; i++) {
          result.targets[i] = Wrap(vmid, info.targets[i]);
        }
      }
      return result;
    }

    private AccessibleRelationInfoNative Unwrap(int vmid, AccessibleRelationInfo info) {
      var result = new AccessibleRelationInfoNative();
      result.key = info.key;
      result.targetCount = info.targetCount;
      if (info.targets != null) {
        var count = info.targetCount;
        result.targets = new JOBJECT64[count];
        for(var i = 0; i < count; i++) {
          result.targets[i] = Unwrap(vmid, info.targets[i]);
        }
      }
      return result;
    }

    private AccessibleRelationSetInfo Wrap(int vmid, AccessibleRelationSetInfoNative info) {
      var result = new AccessibleRelationSetInfo();
      result.relationCount = info.relationCount;
      if (info.relations != null) {
        var count = info.relationCount;
        result.relations = new AccessibleRelationInfo[count];
        for(var i = 0; i < count; i++) {
          result.relations[i] = Wrap(vmid, info.relations[i]);
        }
      }
      return result;
    }

    private AccessibleRelationSetInfoNative Unwrap(int vmid, AccessibleRelationSetInfo info) {
      var result = new AccessibleRelationSetInfoNative();
      result.relationCount = info.relationCount;
      if (info.relations != null) {
        var count = info.relationCount;
        result.relations = new AccessibleRelationInfoNative[count];
        for(var i = 0; i < count; i++) {
          result.relations[i] = Unwrap(vmid, info.relations[i]);
        }
      }
      return result;
    }

    private AccessibleTableCellInfo Wrap(int vmid, AccessibleTableCellInfoNative info) {
      var result = new AccessibleTableCellInfo();
      result.accessibleContext = Wrap(vmid, info.accessibleContext);
      result.index = info.index;
      result.row = info.row;
      result.column = info.column;
      result.rowExtent = info.rowExtent;
      result.columnExtent = info.columnExtent;
      result.isSelected = info.isSelected;
      return result;
    }

    private AccessibleTableCellInfoNative Unwrap(int vmid, AccessibleTableCellInfo info) {
      var result = new AccessibleTableCellInfoNative();
      result.accessibleContext = Unwrap(vmid, info.accessibleContext);
      result.index = info.index;
      result.row = info.row;
      result.column = info.column;
      result.rowExtent = info.rowExtent;
      result.columnExtent = info.columnExtent;
      result.isSelected = info.isSelected;
      return result;
    }

    private AccessibleTableInfo Wrap(int vmid, AccessibleTableInfoNative info) {
      var result = new AccessibleTableInfo();
      result.caption = Wrap(vmid, info.caption);
      result.summary = Wrap(vmid, info.summary);
      result.rowCount = info.rowCount;
      result.columnCount = info.columnCount;
      result.accessibleContext = Wrap(vmid, info.accessibleContext);
      result.accessibleTable = Wrap(vmid, info.accessibleTable);
      return result;
    }

    private AccessibleTableInfoNative Unwrap(int vmid, AccessibleTableInfo info) {
      var result = new AccessibleTableInfoNative();
      result.caption = Unwrap(vmid, info.caption);
      result.summary = Unwrap(vmid, info.summary);
      result.rowCount = info.rowCount;
      result.columnCount = info.columnCount;
      result.accessibleContext = Unwrap(vmid, info.accessibleContext);
      result.accessibleTable = Unwrap(vmid, info.accessibleTable);
      return result;
    }

    private AccessibleTextAttributesInfo Wrap(int vmid, AccessibleTextAttributesInfoNative info) {
      var result = new AccessibleTextAttributesInfo();
      result.bold = info.bold;
      result.italic = info.italic;
      result.underline = info.underline;
      result.strikethrough = info.strikethrough;
      result.superscript = info.superscript;
      result.subscript = info.subscript;
      result.backgroundColor = info.backgroundColor;
      result.foregroundColor = info.foregroundColor;
      result.fontFamily = info.fontFamily;
      result.fontSize = info.fontSize;
      result.alignment = info.alignment;
      result.bidiLevel = info.bidiLevel;
      result.firstLineIndent = info.firstLineIndent;
      result.leftIndent = info.leftIndent;
      result.rightIndent = info.rightIndent;
      result.lineSpacing = info.lineSpacing;
      result.spaceAbove = info.spaceAbove;
      result.spaceBelow = info.spaceBelow;
      result.fullAttributesString = info.fullAttributesString;
      return result;
    }

    private AccessibleTextAttributesInfoNative Unwrap(int vmid, AccessibleTextAttributesInfo info) {
      var result = new AccessibleTextAttributesInfoNative();
      result.bold = info.bold;
      result.italic = info.italic;
      result.underline = info.underline;
      result.strikethrough = info.strikethrough;
      result.superscript = info.superscript;
      result.subscript = info.subscript;
      result.backgroundColor = info.backgroundColor;
      result.foregroundColor = info.foregroundColor;
      result.fontFamily = info.fontFamily;
      result.fontSize = info.fontSize;
      result.alignment = info.alignment;
      result.bidiLevel = info.bidiLevel;
      result.firstLineIndent = info.firstLineIndent;
      result.leftIndent = info.leftIndent;
      result.rightIndent = info.rightIndent;
      result.lineSpacing = info.lineSpacing;
      result.spaceAbove = info.spaceAbove;
      result.spaceBelow = info.spaceBelow;
      result.fullAttributesString = info.fullAttributesString;
      return result;
    }

    private AccessibleTextInfo Wrap(int vmid, AccessibleTextInfoNative info) {
      var result = new AccessibleTextInfo();
      result.charCount = info.charCount;
      result.caretIndex = info.caretIndex;
      result.indexAtPoint = info.indexAtPoint;
      return result;
    }

    private AccessibleTextInfoNative Unwrap(int vmid, AccessibleTextInfo info) {
      var result = new AccessibleTextInfoNative();
      result.charCount = info.charCount;
      result.caretIndex = info.caretIndex;
      result.indexAtPoint = info.indexAtPoint;
      return result;
    }

    private AccessibleTextItemsInfo Wrap(int vmid, AccessibleTextItemsInfoNative info) {
      var result = new AccessibleTextItemsInfo();
      result.letter = info.letter;
      result.word = info.word;
      result.sentence = info.sentence;
      return result;
    }

    private AccessibleTextItemsInfoNative Unwrap(int vmid, AccessibleTextItemsInfo info) {
      var result = new AccessibleTextItemsInfoNative();
      result.letter = info.letter;
      result.word = info.word;
      result.sentence = info.sentence;
      return result;
    }

    private AccessibleTextRectInfo Wrap(int vmid, AccessibleTextRectInfoNative info) {
      var result = new AccessibleTextRectInfo();
      result.x = info.x;
      result.y = info.y;
      result.width = info.width;
      result.height = info.height;
      return result;
    }

    private AccessibleTextRectInfoNative Unwrap(int vmid, AccessibleTextRectInfo info) {
      var result = new AccessibleTextRectInfoNative();
      result.x = info.x;
      result.y = info.y;
      result.width = info.width;
      result.height = info.height;
      return result;
    }

    private AccessibleTextSelectionInfo Wrap(int vmid, AccessibleTextSelectionInfoNative info) {
      var result = new AccessibleTextSelectionInfo();
      result.selectionStartIndex = info.selectionStartIndex;
      result.selectionEndIndex = info.selectionEndIndex;
      result.selectedText = info.selectedText;
      return result;
    }

    private AccessibleTextSelectionInfoNative Unwrap(int vmid, AccessibleTextSelectionInfo info) {
      var result = new AccessibleTextSelectionInfoNative();
      result.selectionStartIndex = info.selectionStartIndex;
      result.selectionEndIndex = info.selectionEndIndex;
      result.selectedText = info.selectedText;
      return result;
    }

    private VisibleChildrenInfo Wrap(int vmid, VisibleChildrenInfoNative info) {
      var result = new VisibleChildrenInfo();
      result.returnedChildrenCount = info.returnedChildrenCount;
      if (info.children != null) {
        var count = info.returnedChildrenCount;
        result.children = new JavaObjectHandle[count];
        for(var i = 0; i < count; i++) {
          result.children[i] = Wrap(vmid, info.children[i]);
        }
      }
      return result;
    }

    private VisibleChildrenInfoNative Unwrap(int vmid, VisibleChildrenInfo info) {
      var result = new VisibleChildrenInfoNative();
      result.returnedChildrenCount = info.returnedChildrenCount;
      if (info.children != null) {
        var count = info.returnedChildrenCount;
        result.children = new JOBJECT64[count];
        for(var i = 0; i < count; i++) {
          result.children[i] = Unwrap(vmid, info.children[i]);
        }
      }
      return result;
    }

    #endregion

    #region CopyWrap/CopyUnwrap classes

    private void CopyWrap(int vmid, AccessibleActionsNative infoSrc, AccessibleActions infoDest) {
      infoDest.actionsCount = infoSrc.actionsCount;
      if (infoSrc.actionInfo != null) {
        var count = infoSrc.actionsCount;
        infoDest.actionInfo = new AccessibleActionInfo[count];
        for(var i = 0; i < count; i++) {
          infoDest.actionInfo[i] = Wrap(vmid, infoSrc.actionInfo[i]);
        }
      }
    }

    private void CopyUnwrap(int vmid, AccessibleActions infoSrc, AccessibleActionsNative infoDest) {
      infoDest.actionsCount = infoSrc.actionsCount;
      if (infoSrc.actionInfo != null) {
        var count = infoSrc.actionsCount;
        infoDest.actionInfo = new AccessibleActionInfoNative[count];
        for(var i = 0; i < count; i++) {
          infoDest.actionInfo[i] = Unwrap(vmid, infoSrc.actionInfo[i]);
        }
      }
    }

    #endregion

  }

  /// <summary>
  /// Implementation of platform agnostic events
  /// </summary>
  public partial class AccessBridgeEvents : IAccessBridgeEvents {
    #region Event fields
    private PropertyChangeEventHandler _propertyChange;
    private JavaShutdownEventHandler _javaShutdown;
    private FocusGainedEventHandler _focusGained;
    private FocusLostEventHandler _focusLost;
    private CaretUpdateEventHandler _caretUpdate;
    private MouseClickedEventHandler _mouseClicked;
    private MouseEnteredEventHandler _mouseEntered;
    private MouseExitedEventHandler _mouseExited;
    private MousePressedEventHandler _mousePressed;
    private MouseReleasedEventHandler _mouseReleased;
    private MenuCanceledEventHandler _menuCanceled;
    private MenuDeselectedEventHandler _menuDeselected;
    private MenuSelectedEventHandler _menuSelected;
    private PopupMenuCanceledEventHandler _popupMenuCanceled;
    private PopupMenuWillBecomeInvisibleEventHandler _popupMenuWillBecomeInvisible;
    private PopupMenuWillBecomeVisibleEventHandler _popupMenuWillBecomeVisible;
    private PropertyNameChangeEventHandler _propertyNameChange;
    private PropertyDescriptionChangeEventHandler _propertyDescriptionChange;
    private PropertyStateChangeEventHandler _propertyStateChange;
    private PropertyValueChangeEventHandler _propertyValueChange;
    private PropertySelectionChangeEventHandler _propertySelectionChange;
    private PropertyTextChangeEventHandler _propertyTextChange;
    private PropertyCaretChangeEventHandler _propertyCaretChange;
    private PropertyVisibleDataChangeEventHandler _propertyVisibleDataChange;
    private PropertyChildChangeEventHandler _propertyChildChange;
    private PropertyActiveDescendentChangeEventHandler _propertyActiveDescendentChange;
    private PropertyTableModelChangeEventHandler _propertyTableModelChange;
    #endregion

    #region Event properties
    public event PropertyChangeEventHandler PropertyChange {
      add {
        if (_propertyChange == null)
          NativeEvents.PropertyChange += ForwardPropertyChange;
        _propertyChange += value;
      }
      remove{
        _propertyChange -= value;
        if (_propertyChange == null)
          NativeEvents.PropertyChange -= ForwardPropertyChange;
      }
    }
    public event JavaShutdownEventHandler JavaShutdown {
      add {
        if (_javaShutdown == null)
          NativeEvents.JavaShutdown += ForwardJavaShutdown;
        _javaShutdown += value;
      }
      remove{
        _javaShutdown -= value;
        if (_javaShutdown == null)
          NativeEvents.JavaShutdown -= ForwardJavaShutdown;
      }
    }
    public event FocusGainedEventHandler FocusGained {
      add {
        if (_focusGained == null)
          NativeEvents.FocusGained += ForwardFocusGained;
        _focusGained += value;
      }
      remove{
        _focusGained -= value;
        if (_focusGained == null)
          NativeEvents.FocusGained -= ForwardFocusGained;
      }
    }
    public event FocusLostEventHandler FocusLost {
      add {
        if (_focusLost == null)
          NativeEvents.FocusLost += ForwardFocusLost;
        _focusLost += value;
      }
      remove{
        _focusLost -= value;
        if (_focusLost == null)
          NativeEvents.FocusLost -= ForwardFocusLost;
      }
    }
    public event CaretUpdateEventHandler CaretUpdate {
      add {
        if (_caretUpdate == null)
          NativeEvents.CaretUpdate += ForwardCaretUpdate;
        _caretUpdate += value;
      }
      remove{
        _caretUpdate -= value;
        if (_caretUpdate == null)
          NativeEvents.CaretUpdate -= ForwardCaretUpdate;
      }
    }
    public event MouseClickedEventHandler MouseClicked {
      add {
        if (_mouseClicked == null)
          NativeEvents.MouseClicked += ForwardMouseClicked;
        _mouseClicked += value;
      }
      remove{
        _mouseClicked -= value;
        if (_mouseClicked == null)
          NativeEvents.MouseClicked -= ForwardMouseClicked;
      }
    }
    public event MouseEnteredEventHandler MouseEntered {
      add {
        if (_mouseEntered == null)
          NativeEvents.MouseEntered += ForwardMouseEntered;
        _mouseEntered += value;
      }
      remove{
        _mouseEntered -= value;
        if (_mouseEntered == null)
          NativeEvents.MouseEntered -= ForwardMouseEntered;
      }
    }
    public event MouseExitedEventHandler MouseExited {
      add {
        if (_mouseExited == null)
          NativeEvents.MouseExited += ForwardMouseExited;
        _mouseExited += value;
      }
      remove{
        _mouseExited -= value;
        if (_mouseExited == null)
          NativeEvents.MouseExited -= ForwardMouseExited;
      }
    }
    public event MousePressedEventHandler MousePressed {
      add {
        if (_mousePressed == null)
          NativeEvents.MousePressed += ForwardMousePressed;
        _mousePressed += value;
      }
      remove{
        _mousePressed -= value;
        if (_mousePressed == null)
          NativeEvents.MousePressed -= ForwardMousePressed;
      }
    }
    public event MouseReleasedEventHandler MouseReleased {
      add {
        if (_mouseReleased == null)
          NativeEvents.MouseReleased += ForwardMouseReleased;
        _mouseReleased += value;
      }
      remove{
        _mouseReleased -= value;
        if (_mouseReleased == null)
          NativeEvents.MouseReleased -= ForwardMouseReleased;
      }
    }
    public event MenuCanceledEventHandler MenuCanceled {
      add {
        if (_menuCanceled == null)
          NativeEvents.MenuCanceled += ForwardMenuCanceled;
        _menuCanceled += value;
      }
      remove{
        _menuCanceled -= value;
        if (_menuCanceled == null)
          NativeEvents.MenuCanceled -= ForwardMenuCanceled;
      }
    }
    public event MenuDeselectedEventHandler MenuDeselected {
      add {
        if (_menuDeselected == null)
          NativeEvents.MenuDeselected += ForwardMenuDeselected;
        _menuDeselected += value;
      }
      remove{
        _menuDeselected -= value;
        if (_menuDeselected == null)
          NativeEvents.MenuDeselected -= ForwardMenuDeselected;
      }
    }
    public event MenuSelectedEventHandler MenuSelected {
      add {
        if (_menuSelected == null)
          NativeEvents.MenuSelected += ForwardMenuSelected;
        _menuSelected += value;
      }
      remove{
        _menuSelected -= value;
        if (_menuSelected == null)
          NativeEvents.MenuSelected -= ForwardMenuSelected;
      }
    }
    public event PopupMenuCanceledEventHandler PopupMenuCanceled {
      add {
        if (_popupMenuCanceled == null)
          NativeEvents.PopupMenuCanceled += ForwardPopupMenuCanceled;
        _popupMenuCanceled += value;
      }
      remove{
        _popupMenuCanceled -= value;
        if (_popupMenuCanceled == null)
          NativeEvents.PopupMenuCanceled -= ForwardPopupMenuCanceled;
      }
    }
    public event PopupMenuWillBecomeInvisibleEventHandler PopupMenuWillBecomeInvisible {
      add {
        if (_popupMenuWillBecomeInvisible == null)
          NativeEvents.PopupMenuWillBecomeInvisible += ForwardPopupMenuWillBecomeInvisible;
        _popupMenuWillBecomeInvisible += value;
      }
      remove{
        _popupMenuWillBecomeInvisible -= value;
        if (_popupMenuWillBecomeInvisible == null)
          NativeEvents.PopupMenuWillBecomeInvisible -= ForwardPopupMenuWillBecomeInvisible;
      }
    }
    public event PopupMenuWillBecomeVisibleEventHandler PopupMenuWillBecomeVisible {
      add {
        if (_popupMenuWillBecomeVisible == null)
          NativeEvents.PopupMenuWillBecomeVisible += ForwardPopupMenuWillBecomeVisible;
        _popupMenuWillBecomeVisible += value;
      }
      remove{
        _popupMenuWillBecomeVisible -= value;
        if (_popupMenuWillBecomeVisible == null)
          NativeEvents.PopupMenuWillBecomeVisible -= ForwardPopupMenuWillBecomeVisible;
      }
    }
    public event PropertyNameChangeEventHandler PropertyNameChange {
      add {
        if (_propertyNameChange == null)
          NativeEvents.PropertyNameChange += ForwardPropertyNameChange;
        _propertyNameChange += value;
      }
      remove{
        _propertyNameChange -= value;
        if (_propertyNameChange == null)
          NativeEvents.PropertyNameChange -= ForwardPropertyNameChange;
      }
    }
    public event PropertyDescriptionChangeEventHandler PropertyDescriptionChange {
      add {
        if (_propertyDescriptionChange == null)
          NativeEvents.PropertyDescriptionChange += ForwardPropertyDescriptionChange;
        _propertyDescriptionChange += value;
      }
      remove{
        _propertyDescriptionChange -= value;
        if (_propertyDescriptionChange == null)
          NativeEvents.PropertyDescriptionChange -= ForwardPropertyDescriptionChange;
      }
    }
    public event PropertyStateChangeEventHandler PropertyStateChange {
      add {
        if (_propertyStateChange == null)
          NativeEvents.PropertyStateChange += ForwardPropertyStateChange;
        _propertyStateChange += value;
      }
      remove{
        _propertyStateChange -= value;
        if (_propertyStateChange == null)
          NativeEvents.PropertyStateChange -= ForwardPropertyStateChange;
      }
    }
    public event PropertyValueChangeEventHandler PropertyValueChange {
      add {
        if (_propertyValueChange == null)
          NativeEvents.PropertyValueChange += ForwardPropertyValueChange;
        _propertyValueChange += value;
      }
      remove{
        _propertyValueChange -= value;
        if (_propertyValueChange == null)
          NativeEvents.PropertyValueChange -= ForwardPropertyValueChange;
      }
    }
    public event PropertySelectionChangeEventHandler PropertySelectionChange {
      add {
        if (_propertySelectionChange == null)
          NativeEvents.PropertySelectionChange += ForwardPropertySelectionChange;
        _propertySelectionChange += value;
      }
      remove{
        _propertySelectionChange -= value;
        if (_propertySelectionChange == null)
          NativeEvents.PropertySelectionChange -= ForwardPropertySelectionChange;
      }
    }
    public event PropertyTextChangeEventHandler PropertyTextChange {
      add {
        if (_propertyTextChange == null)
          NativeEvents.PropertyTextChange += ForwardPropertyTextChange;
        _propertyTextChange += value;
      }
      remove{
        _propertyTextChange -= value;
        if (_propertyTextChange == null)
          NativeEvents.PropertyTextChange -= ForwardPropertyTextChange;
      }
    }
    public event PropertyCaretChangeEventHandler PropertyCaretChange {
      add {
        if (_propertyCaretChange == null)
          NativeEvents.PropertyCaretChange += ForwardPropertyCaretChange;
        _propertyCaretChange += value;
      }
      remove{
        _propertyCaretChange -= value;
        if (_propertyCaretChange == null)
          NativeEvents.PropertyCaretChange -= ForwardPropertyCaretChange;
      }
    }
    public event PropertyVisibleDataChangeEventHandler PropertyVisibleDataChange {
      add {
        if (_propertyVisibleDataChange == null)
          NativeEvents.PropertyVisibleDataChange += ForwardPropertyVisibleDataChange;
        _propertyVisibleDataChange += value;
      }
      remove{
        _propertyVisibleDataChange -= value;
        if (_propertyVisibleDataChange == null)
          NativeEvents.PropertyVisibleDataChange -= ForwardPropertyVisibleDataChange;
      }
    }
    public event PropertyChildChangeEventHandler PropertyChildChange {
      add {
        if (_propertyChildChange == null)
          NativeEvents.PropertyChildChange += ForwardPropertyChildChange;
        _propertyChildChange += value;
      }
      remove{
        _propertyChildChange -= value;
        if (_propertyChildChange == null)
          NativeEvents.PropertyChildChange -= ForwardPropertyChildChange;
      }
    }
    public event PropertyActiveDescendentChangeEventHandler PropertyActiveDescendentChange {
      add {
        if (_propertyActiveDescendentChange == null)
          NativeEvents.PropertyActiveDescendentChange += ForwardPropertyActiveDescendentChange;
        _propertyActiveDescendentChange += value;
      }
      remove{
        _propertyActiveDescendentChange -= value;
        if (_propertyActiveDescendentChange == null)
          NativeEvents.PropertyActiveDescendentChange -= ForwardPropertyActiveDescendentChange;
      }
    }
    public event PropertyTableModelChangeEventHandler PropertyTableModelChange {
      add {
        if (_propertyTableModelChange == null)
          NativeEvents.PropertyTableModelChange += ForwardPropertyTableModelChange;
        _propertyTableModelChange += value;
      }
      remove{
        _propertyTableModelChange -= value;
        if (_propertyTableModelChange == null)
          NativeEvents.PropertyTableModelChange -= ForwardPropertyTableModelChange;
      }
    }
    #endregion

    #region Event handlers
    protected virtual void OnPropertyChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string property, string oldValue, string newValue) {
      var handler = _propertyChange;
      if (handler != null)
        handler(vmid, evt, source, property, oldValue, newValue);
    }
    protected virtual void OnJavaShutdown(int vmid) {
      var handler = _javaShutdown;
      if (handler != null)
        handler(vmid);
    }
    protected virtual void OnFocusGained(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = _focusGained;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnFocusLost(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = _focusLost;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnCaretUpdate(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = _caretUpdate;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseClicked(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = _mouseClicked;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseEntered(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = _mouseEntered;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseExited(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = _mouseExited;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMousePressed(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = _mousePressed;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseReleased(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = _mouseReleased;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMenuCanceled(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = _menuCanceled;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMenuDeselected(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = _menuDeselected;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMenuSelected(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = _menuSelected;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPopupMenuCanceled(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = _popupMenuCanceled;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPopupMenuWillBecomeInvisible(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = _popupMenuWillBecomeInvisible;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPopupMenuWillBecomeVisible(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = _popupMenuWillBecomeVisible;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyNameChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string oldName, string newName) {
      var handler = _propertyNameChange;
      if (handler != null)
        handler(vmid, evt, source, oldName, newName);
    }
    protected virtual void OnPropertyDescriptionChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string oldDescription, string newDescription) {
      var handler = _propertyDescriptionChange;
      if (handler != null)
        handler(vmid, evt, source, oldDescription, newDescription);
    }
    protected virtual void OnPropertyStateChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string oldState, string newState) {
      var handler = _propertyStateChange;
      if (handler != null)
        handler(vmid, evt, source, oldState, newState);
    }
    protected virtual void OnPropertyValueChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string oldValue, string newValue) {
      var handler = _propertyValueChange;
      if (handler != null)
        handler(vmid, evt, source, oldValue, newValue);
    }
    protected virtual void OnPropertySelectionChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = _propertySelectionChange;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyTextChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = _propertyTextChange;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyCaretChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, int oldPosition, int newPosition) {
      var handler = _propertyCaretChange;
      if (handler != null)
        handler(vmid, evt, source, oldPosition, newPosition);
    }
    protected virtual void OnPropertyVisibleDataChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = _propertyVisibleDataChange;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyChildChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, JavaObjectHandle oldChild, JavaObjectHandle newChild) {
      var handler = _propertyChildChange;
      if (handler != null)
        handler(vmid, evt, source, oldChild, newChild);
    }
    protected virtual void OnPropertyActiveDescendentChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, JavaObjectHandle oldActiveDescendent, JavaObjectHandle newActiveDescendent) {
      var handler = _propertyActiveDescendentChange;
      if (handler != null)
        handler(vmid, evt, source, oldActiveDescendent, newActiveDescendent);
    }
    protected virtual void OnPropertyTableModelChange(int vmid, JavaObjectHandle evt, JavaObjectHandle src, string oldValue, string newValue) {
      var handler = _propertyTableModelChange;
      if (handler != null)
        handler(vmid, evt, src, oldValue, newValue);
    }
    #endregion

    private void DetachForwarders() {
      NativeEvents.PropertyChange -= ForwardPropertyChange;
      NativeEvents.JavaShutdown -= ForwardJavaShutdown;
      NativeEvents.FocusGained -= ForwardFocusGained;
      NativeEvents.FocusLost -= ForwardFocusLost;
      NativeEvents.CaretUpdate -= ForwardCaretUpdate;
      NativeEvents.MouseClicked -= ForwardMouseClicked;
      NativeEvents.MouseEntered -= ForwardMouseEntered;
      NativeEvents.MouseExited -= ForwardMouseExited;
      NativeEvents.MousePressed -= ForwardMousePressed;
      NativeEvents.MouseReleased -= ForwardMouseReleased;
      NativeEvents.MenuCanceled -= ForwardMenuCanceled;
      NativeEvents.MenuDeselected -= ForwardMenuDeselected;
      NativeEvents.MenuSelected -= ForwardMenuSelected;
      NativeEvents.PopupMenuCanceled -= ForwardPopupMenuCanceled;
      NativeEvents.PopupMenuWillBecomeInvisible -= ForwardPopupMenuWillBecomeInvisible;
      NativeEvents.PopupMenuWillBecomeVisible -= ForwardPopupMenuWillBecomeVisible;
      NativeEvents.PropertyNameChange -= ForwardPropertyNameChange;
      NativeEvents.PropertyDescriptionChange -= ForwardPropertyDescriptionChange;
      NativeEvents.PropertyStateChange -= ForwardPropertyStateChange;
      NativeEvents.PropertyValueChange -= ForwardPropertyValueChange;
      NativeEvents.PropertySelectionChange -= ForwardPropertySelectionChange;
      NativeEvents.PropertyTextChange -= ForwardPropertyTextChange;
      NativeEvents.PropertyCaretChange -= ForwardPropertyCaretChange;
      NativeEvents.PropertyVisibleDataChange -= ForwardPropertyVisibleDataChange;
      NativeEvents.PropertyChildChange -= ForwardPropertyChildChange;
      NativeEvents.PropertyActiveDescendentChange -= ForwardPropertyActiveDescendentChange;
      NativeEvents.PropertyTableModelChange -= ForwardPropertyTableModelChange;
    }

    #region Event forwarders
    private void ForwardPropertyChange(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string property, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue) {
      OnPropertyChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), property, oldValue, newValue);
    }
    private void ForwardJavaShutdown(int vmid) {
      OnJavaShutdown(vmid);
    }
    private void ForwardFocusGained(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      OnFocusGained(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }
    private void ForwardFocusLost(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      OnFocusLost(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }
    private void ForwardCaretUpdate(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      OnCaretUpdate(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }
    private void ForwardMouseClicked(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      OnMouseClicked(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }
    private void ForwardMouseEntered(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      OnMouseEntered(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }
    private void ForwardMouseExited(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      OnMouseExited(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }
    private void ForwardMousePressed(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      OnMousePressed(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }
    private void ForwardMouseReleased(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      OnMouseReleased(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }
    private void ForwardMenuCanceled(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      OnMenuCanceled(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }
    private void ForwardMenuDeselected(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      OnMenuDeselected(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }
    private void ForwardMenuSelected(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      OnMenuSelected(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }
    private void ForwardPopupMenuCanceled(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      OnPopupMenuCanceled(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }
    private void ForwardPopupMenuWillBecomeInvisible(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      OnPopupMenuWillBecomeInvisible(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }
    private void ForwardPopupMenuWillBecomeVisible(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      OnPopupMenuWillBecomeVisible(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }
    private void ForwardPropertyNameChange(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string oldName, [MarshalAs(UnmanagedType.LPWStr)]string newName) {
      OnPropertyNameChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), oldName, newName);
    }
    private void ForwardPropertyDescriptionChange(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string oldDescription, [MarshalAs(UnmanagedType.LPWStr)]string newDescription) {
      OnPropertyDescriptionChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), oldDescription, newDescription);
    }
    private void ForwardPropertyStateChange(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string oldState, [MarshalAs(UnmanagedType.LPWStr)]string newState) {
      OnPropertyStateChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), oldState, newState);
    }
    private void ForwardPropertyValueChange(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue) {
      OnPropertyValueChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), oldValue, newValue);
    }
    private void ForwardPropertySelectionChange(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      OnPropertySelectionChange(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }
    private void ForwardPropertyTextChange(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      OnPropertyTextChange(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }
    private void ForwardPropertyCaretChange(int vmid, JOBJECT64 evt, JOBJECT64 source, int oldPosition, int newPosition) {
      OnPropertyCaretChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), oldPosition, newPosition);
    }
    private void ForwardPropertyVisibleDataChange(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      OnPropertyVisibleDataChange(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }
    private void ForwardPropertyChildChange(int vmid, JOBJECT64 evt, JOBJECT64 source, JOBJECT64 oldChild, JOBJECT64 newChild) {
      OnPropertyChildChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), Wrap(vmid, oldChild), Wrap(vmid, newChild));
    }
    private void ForwardPropertyActiveDescendentChange(int vmid, JOBJECT64 evt, JOBJECT64 source, JOBJECT64 oldActiveDescendent, JOBJECT64 newActiveDescendent) {
      OnPropertyActiveDescendentChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), Wrap(vmid, oldActiveDescendent), Wrap(vmid, newActiveDescendent));
    }
    private void ForwardPropertyTableModelChange(int vmid, JOBJECT64 evt, JOBJECT64 src, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue) {
      OnPropertyTableModelChange(vmid, Wrap(vmid, evt), Wrap(vmid, src), oldValue, newValue);
    }
    #endregion
  }

  /// <summary>
  /// Container of WindowAccessBridge DLL entry points
  /// </summary>
  public class AccessBridgeLibraryFunctions {
    #region Functions
    public Windows_runFP Windows_run { get; set; }
    public IsJavaWindowFP IsJavaWindow { get; set; }
    public IsSameObjectFP IsSameObject { get; set; }
    public GetAccessibleContextFromHWNDFP GetAccessibleContextFromHWND { get; set; }
    public GetHWNDFromAccessibleContextFP GetHWNDFromAccessibleContext { get; set; }
    public GetAccessibleContextAtFP GetAccessibleContextAt { get; set; }
    public GetAccessibleContextWithFocusFP GetAccessibleContextWithFocus { get; set; }
    public GetAccessibleContextInfoFP GetAccessibleContextInfo { get; set; }
    public GetAccessibleChildFromContextFP GetAccessibleChildFromContext { get; set; }
    public GetAccessibleParentFromContextFP GetAccessibleParentFromContext { get; set; }
    public GetAccessibleRelationSetFP GetAccessibleRelationSet { get; set; }
    public GetAccessibleHypertextFP GetAccessibleHypertext { get; set; }
    public ActivateAccessibleHyperlinkFP ActivateAccessibleHyperlink { get; set; }
    public GetAccessibleHyperlinkCountFP GetAccessibleHyperlinkCount { get; set; }
    public GetAccessibleHypertextExtFP GetAccessibleHypertextExt { get; set; }
    public GetAccessibleHypertextLinkIndexFP GetAccessibleHypertextLinkIndex { get; set; }
    public GetAccessibleHyperlinkFP GetAccessibleHyperlink { get; set; }
    public GetAccessibleKeyBindingsFP GetAccessibleKeyBindings { get; set; }
    public GetAccessibleIconsFP GetAccessibleIcons { get; set; }
    public GetAccessibleActionsFP GetAccessibleActions { get; set; }
    public DoAccessibleActionsFP DoAccessibleActions { get; set; }
    public GetAccessibleTextInfoFP GetAccessibleTextInfo { get; set; }
    public GetAccessibleTextItemsFP GetAccessibleTextItems { get; set; }
    public GetAccessibleTextSelectionInfoFP GetAccessibleTextSelectionInfo { get; set; }
    public GetAccessibleTextAttributesFP GetAccessibleTextAttributes { get; set; }
    public GetAccessibleTextRectFP GetAccessibleTextRect { get; set; }
    public GetAccessibleTextLineBoundsFP GetAccessibleTextLineBounds { get; set; }
    public GetAccessibleTextRangeFP GetAccessibleTextRange { get; set; }
    public GetCurrentAccessibleValueFromContextFP GetCurrentAccessibleValueFromContext { get; set; }
    public GetMaximumAccessibleValueFromContextFP GetMaximumAccessibleValueFromContext { get; set; }
    public GetMinimumAccessibleValueFromContextFP GetMinimumAccessibleValueFromContext { get; set; }
    public AddAccessibleSelectionFromContextFP AddAccessibleSelectionFromContext { get; set; }
    public ClearAccessibleSelectionFromContextFP ClearAccessibleSelectionFromContext { get; set; }
    public GetAccessibleSelectionFromContextFP GetAccessibleSelectionFromContext { get; set; }
    public GetAccessibleSelectionCountFromContextFP GetAccessibleSelectionCountFromContext { get; set; }
    public IsAccessibleChildSelectedFromContextFP IsAccessibleChildSelectedFromContext { get; set; }
    public RemoveAccessibleSelectionFromContextFP RemoveAccessibleSelectionFromContext { get; set; }
    public SelectAllAccessibleSelectionFromContextFP SelectAllAccessibleSelectionFromContext { get; set; }
    public GetAccessibleTableInfoFP GetAccessibleTableInfo { get; set; }
    public GetAccessibleTableCellInfoFP GetAccessibleTableCellInfo { get; set; }
    public GetAccessibleTableRowHeaderFP GetAccessibleTableRowHeader { get; set; }
    public GetAccessibleTableColumnHeaderFP GetAccessibleTableColumnHeader { get; set; }
    public GetAccessibleTableRowDescriptionFP GetAccessibleTableRowDescription { get; set; }
    public GetAccessibleTableColumnDescriptionFP GetAccessibleTableColumnDescription { get; set; }
    public GetAccessibleTableRowSelectionCountFP GetAccessibleTableRowSelectionCount { get; set; }
    public IsAccessibleTableRowSelectedFP IsAccessibleTableRowSelected { get; set; }
    public GetAccessibleTableRowSelectionsFP GetAccessibleTableRowSelections { get; set; }
    public GetAccessibleTableColumnSelectionCountFP GetAccessibleTableColumnSelectionCount { get; set; }
    public IsAccessibleTableColumnSelectedFP IsAccessibleTableColumnSelected { get; set; }
    public GetAccessibleTableColumnSelectionsFP GetAccessibleTableColumnSelections { get; set; }
    public GetAccessibleTableRowFP GetAccessibleTableRow { get; set; }
    public GetAccessibleTableColumnFP GetAccessibleTableColumn { get; set; }
    public GetAccessibleTableIndexFP GetAccessibleTableIndex { get; set; }
    public SetTextContentsFP SetTextContents { get; set; }
    public GetParentWithRoleFP GetParentWithRole { get; set; }
    public GetParentWithRoleElseRootFP GetParentWithRoleElseRoot { get; set; }
    public GetTopLevelObjectFP GetTopLevelObject { get; set; }
    public GetObjectDepthFP GetObjectDepth { get; set; }
    public GetActiveDescendentFP GetActiveDescendent { get; set; }
    public GetVirtualAccessibleNameFP GetVirtualAccessibleName { get; set; }
    public GetTextAttributesInRangeFP GetTextAttributesInRange { get; set; }
    public GetCaretLocationFP GetCaretLocation { get; set; }
    public GetVisibleChildrenCountFP GetVisibleChildrenCount { get; set; }
    public GetVisibleChildrenFP GetVisibleChildren { get; set; }
    public GetVersionInfoFP GetVersionInfo { get; set; }
    #endregion

    #region Event functions
    public SetPropertyChangeFP SetPropertyChange { get; set; }
    public SetJavaShutdownFP SetJavaShutdown { get; set; }
    public SetFocusGainedFP SetFocusGained { get; set; }
    public SetFocusLostFP SetFocusLost { get; set; }
    public SetCaretUpdateFP SetCaretUpdate { get; set; }
    public SetMouseClickedFP SetMouseClicked { get; set; }
    public SetMouseEnteredFP SetMouseEntered { get; set; }
    public SetMouseExitedFP SetMouseExited { get; set; }
    public SetMousePressedFP SetMousePressed { get; set; }
    public SetMouseReleasedFP SetMouseReleased { get; set; }
    public SetMenuCanceledFP SetMenuCanceled { get; set; }
    public SetMenuDeselectedFP SetMenuDeselected { get; set; }
    public SetMenuSelectedFP SetMenuSelected { get; set; }
    public SetPopupMenuCanceledFP SetPopupMenuCanceled { get; set; }
    public SetPopupMenuWillBecomeInvisibleFP SetPopupMenuWillBecomeInvisible { get; set; }
    public SetPopupMenuWillBecomeVisibleFP SetPopupMenuWillBecomeVisible { get; set; }
    public SetPropertyNameChangeFP SetPropertyNameChange { get; set; }
    public SetPropertyDescriptionChangeFP SetPropertyDescriptionChange { get; set; }
    public SetPropertyStateChangeFP SetPropertyStateChange { get; set; }
    public SetPropertyValueChangeFP SetPropertyValueChange { get; set; }
    public SetPropertySelectionChangeFP SetPropertySelectionChange { get; set; }
    public SetPropertyTextChangeFP SetPropertyTextChange { get; set; }
    public SetPropertyCaretChangeFP SetPropertyCaretChange { get; set; }
    public SetPropertyVisibleDataChangeFP SetPropertyVisibleDataChange { get; set; }
    public SetPropertyChildChangeFP SetPropertyChildChange { get; set; }
    public SetPropertyActiveDescendentChangeFP SetPropertyActiveDescendentChange { get; set; }
    public SetPropertyTableModelChangeFP SetPropertyTableModelChange { get; set; }
    #endregion

    #region Function delegate types
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void Windows_runFP();
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL IsJavaWindowFP(WindowHandle window);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL IsSameObjectFP(int vmid, JOBJECT64 obj1, JOBJECT64 obj2);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleContextFromHWNDFP(WindowHandle window, out int vmid, out JOBJECT64 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate WindowHandle GetHWNDFromAccessibleContextFP(int vmid, JOBJECT64 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleContextAtFP(int vmid, JOBJECT64 acParent, int x, int y, out JOBJECT64 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleContextWithFocusFP(WindowHandle window, out int vmid, out JOBJECT64 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleContextInfoFP(int vmid, JOBJECT64 ac, out AccessibleContextInfoNative info);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT64 GetAccessibleChildFromContextFP(int vmid, JOBJECT64 ac, int i);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT64 GetAccessibleParentFromContextFP(int vmid, JOBJECT64 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleRelationSetFP(int vmid, JOBJECT64 accessibleContext, out AccessibleRelationSetInfoNative relationSetInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleHypertextFP(int vmid, JOBJECT64 accessibleContext, out AccessibleHypertextInfoNative hypertextInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL ActivateAccessibleHyperlinkFP(int vmid, JOBJECT64 accessibleContext, JOBJECT64 accessibleHyperlink);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleHyperlinkCountFP(int vmid, JOBJECT64 accessibleContext);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleHypertextExtFP(int vmid, JOBJECT64 accessibleContext, int nStartIndex, out AccessibleHypertextInfoNative hypertextInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleHypertextLinkIndexFP(int vmid, JOBJECT64 hypertext, int nIndex);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleHyperlinkFP(int vmid, JOBJECT64 hypertext, int nIndex, out AccessibleHyperlinkInfoNative hyperlinkInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleKeyBindingsFP(int vmid, JOBJECT64 accessibleContext, out AccessibleKeyBindingsNative keyBindings);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleIconsFP(int vmid, JOBJECT64 accessibleContext, out AccessibleIconsNative icons);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleActionsFP(int vmid, JOBJECT64 accessibleContext, [Out]AccessibleActionsNative actions);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL DoAccessibleActionsFP(int vmid, JOBJECT64 accessibleContext, ref AccessibleActionsToDoNative actionsToDo, out int failure);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextInfoFP(int vmid, JOBJECT64 at, out AccessibleTextInfoNative textInfo, int x, int y);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextItemsFP(int vmid, JOBJECT64 at, out AccessibleTextItemsInfoNative textItems, int index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextSelectionInfoFP(int vmid, JOBJECT64 at, out AccessibleTextSelectionInfoNative textSelection);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextAttributesFP(int vmid, JOBJECT64 at, int index, out AccessibleTextAttributesInfoNative attributes);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextRectFP(int vmid, JOBJECT64 at, out AccessibleTextRectInfoNative rectInfo, int index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextLineBoundsFP(int vmid, JOBJECT64 at, int index, out int startIndex, out int endIndex);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextRangeFP(int vmid, JOBJECT64 at, int start, int end, StringBuilder text, short len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetCurrentAccessibleValueFromContextFP(int vmid, JOBJECT64 av, StringBuilder value, short len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetMaximumAccessibleValueFromContextFP(int vmid, JOBJECT64 av, StringBuilder value, short len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetMinimumAccessibleValueFromContextFP(int vmid, JOBJECT64 av, StringBuilder value, short len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void AddAccessibleSelectionFromContextFP(int vmid, JOBJECT64 asel, int i);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void ClearAccessibleSelectionFromContextFP(int vmid, JOBJECT64 asel);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT64 GetAccessibleSelectionFromContextFP(int vmid, JOBJECT64 asel, int i);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleSelectionCountFromContextFP(int vmid, JOBJECT64 asel);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL IsAccessibleChildSelectedFromContextFP(int vmid, JOBJECT64 asel, int i);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void RemoveAccessibleSelectionFromContextFP(int vmid, JOBJECT64 asel, int i);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void SelectAllAccessibleSelectionFromContextFP(int vmid, JOBJECT64 asel);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableInfoFP(int vmid, JOBJECT64 ac, out AccessibleTableInfoNative tableInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableCellInfoFP(int vmid, JOBJECT64 accessibleTable, int row, int column, out AccessibleTableCellInfoNative tableCellInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableRowHeaderFP(int vmid, JOBJECT64 acParent, out AccessibleTableInfoNative tableInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableColumnHeaderFP(int vmid, JOBJECT64 acParent, out AccessibleTableInfoNative tableInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT64 GetAccessibleTableRowDescriptionFP(int vmid, JOBJECT64 acParent, int row);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT64 GetAccessibleTableColumnDescriptionFP(int vmid, JOBJECT64 acParent, int column);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleTableRowSelectionCountFP(int vmid, JOBJECT64 table);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL IsAccessibleTableRowSelectedFP(int vmid, JOBJECT64 table, int row);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableRowSelectionsFP(int vmid, JOBJECT64 table, int count, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]int[] selections);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleTableColumnSelectionCountFP(int vmid, JOBJECT64 table);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL IsAccessibleTableColumnSelectedFP(int vmid, JOBJECT64 table, int column);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableColumnSelectionsFP(int vmid, JOBJECT64 table, int count, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]int[] selections);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleTableRowFP(int vmid, JOBJECT64 table, int index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleTableColumnFP(int vmid, JOBJECT64 table, int index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleTableIndexFP(int vmid, JOBJECT64 table, int row, int column);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetTextContentsFP(int vmid, JOBJECT64 ac, string text);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT64 GetParentWithRoleFP(int vmid, JOBJECT64 ac, [MarshalAs(UnmanagedType.LPWStr)]string role);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT64 GetParentWithRoleElseRootFP(int vmid, JOBJECT64 ac, [MarshalAs(UnmanagedType.LPWStr)]string role);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT64 GetTopLevelObjectFP(int vmid, JOBJECT64 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetObjectDepthFP(int vmid, JOBJECT64 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT64 GetActiveDescendentFP(int vmid, JOBJECT64 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetVirtualAccessibleNameFP(int vmid, JOBJECT64 ac, StringBuilder name, int len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetTextAttributesInRangeFP(int vmid, JOBJECT64 accessibleContext, int startIndex, int endIndex, out AccessibleTextAttributesInfoNative attributes, out short len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetCaretLocationFP(int vmid, JOBJECT64 ac, out AccessibleTextRectInfoNative rectInfo, int index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetVisibleChildrenCountFP(int vmid, JOBJECT64 accessibleContext);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetVisibleChildrenFP(int vmid, JOBJECT64 accessibleContext, int startIndex, out VisibleChildrenInfoNative children);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetVersionInfoFP(int vmid, out AccessBridgeVersionInfoNative info);
    #endregion

    #region Event delegate types
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyChangeEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string property, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void JavaShutdownEventHandler(int vmid);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void FocusGainedEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void FocusLostEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void CaretUpdateEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void MouseClickedEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void MouseEnteredEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void MouseExitedEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void MousePressedEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void MouseReleasedEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void MenuCanceledEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void MenuDeselectedEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void MenuSelectedEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PopupMenuCanceledEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PopupMenuWillBecomeInvisibleEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PopupMenuWillBecomeVisibleEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyNameChangeEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string oldName, [MarshalAs(UnmanagedType.LPWStr)]string newName);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyDescriptionChangeEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string oldDescription, [MarshalAs(UnmanagedType.LPWStr)]string newDescription);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyStateChangeEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string oldState, [MarshalAs(UnmanagedType.LPWStr)]string newState);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyValueChangeEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertySelectionChangeEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyTextChangeEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyCaretChangeEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source, int oldPosition, int newPosition);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyVisibleDataChangeEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyChildChangeEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source, JOBJECT64 oldChild, JOBJECT64 newChild);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyActiveDescendentChangeEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 source, JOBJECT64 oldActiveDescendent, JOBJECT64 newActiveDescendent);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyTableModelChangeEventHandler(int vmid, JOBJECT64 evt, JOBJECT64 src, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue);
    #endregion

    #region Event function delegate types
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyChangeFP(PropertyChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetJavaShutdownFP(JavaShutdownEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetFocusGainedFP(FocusGainedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetFocusLostFP(FocusLostEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetCaretUpdateFP(CaretUpdateEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetMouseClickedFP(MouseClickedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetMouseEnteredFP(MouseEnteredEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetMouseExitedFP(MouseExitedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetMousePressedFP(MousePressedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetMouseReleasedFP(MouseReleasedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetMenuCanceledFP(MenuCanceledEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetMenuDeselectedFP(MenuDeselectedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetMenuSelectedFP(MenuSelectedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPopupMenuCanceledFP(PopupMenuCanceledEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPopupMenuWillBecomeInvisibleFP(PopupMenuWillBecomeInvisibleEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPopupMenuWillBecomeVisibleFP(PopupMenuWillBecomeVisibleEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyNameChangeFP(PropertyNameChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyDescriptionChangeFP(PropertyDescriptionChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyStateChangeFP(PropertyStateChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyValueChangeFP(PropertyValueChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertySelectionChangeFP(PropertySelectionChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyTextChangeFP(PropertyTextChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyCaretChangeFP(PropertyCaretChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyVisibleDataChangeFP(PropertyVisibleDataChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyChildChangeFP(PropertyChildChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyActiveDescendentChangeFP(PropertyActiveDescendentChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyTableModelChangeFP(PropertyTableModelChangeEventHandler handler);
    #endregion
  }

  /// <summary>
  /// Container of WindowAccessBridge DLL entry points
  /// </summary>
  public class AccessBridgeLibraryFunctionsLegacy {
    #region Functions
    public Windows_runFP Windows_run { get; set; }
    public IsJavaWindowFP IsJavaWindow { get; set; }
    public IsSameObjectFP IsSameObject { get; set; }
    public GetAccessibleContextFromHWNDFP GetAccessibleContextFromHWND { get; set; }
    public GetHWNDFromAccessibleContextFP GetHWNDFromAccessibleContext { get; set; }
    public GetAccessibleContextAtFP GetAccessibleContextAt { get; set; }
    public GetAccessibleContextWithFocusFP GetAccessibleContextWithFocus { get; set; }
    public GetAccessibleContextInfoFP GetAccessibleContextInfo { get; set; }
    public GetAccessibleChildFromContextFP GetAccessibleChildFromContext { get; set; }
    public GetAccessibleParentFromContextFP GetAccessibleParentFromContext { get; set; }
    public GetAccessibleRelationSetFP GetAccessibleRelationSet { get; set; }
    public GetAccessibleHypertextFP GetAccessibleHypertext { get; set; }
    public ActivateAccessibleHyperlinkFP ActivateAccessibleHyperlink { get; set; }
    public GetAccessibleHyperlinkCountFP GetAccessibleHyperlinkCount { get; set; }
    public GetAccessibleHypertextExtFP GetAccessibleHypertextExt { get; set; }
    public GetAccessibleHypertextLinkIndexFP GetAccessibleHypertextLinkIndex { get; set; }
    public GetAccessibleHyperlinkFP GetAccessibleHyperlink { get; set; }
    public GetAccessibleKeyBindingsFP GetAccessibleKeyBindings { get; set; }
    public GetAccessibleIconsFP GetAccessibleIcons { get; set; }
    public GetAccessibleActionsFP GetAccessibleActions { get; set; }
    public DoAccessibleActionsFP DoAccessibleActions { get; set; }
    public GetAccessibleTextInfoFP GetAccessibleTextInfo { get; set; }
    public GetAccessibleTextItemsFP GetAccessibleTextItems { get; set; }
    public GetAccessibleTextSelectionInfoFP GetAccessibleTextSelectionInfo { get; set; }
    public GetAccessibleTextAttributesFP GetAccessibleTextAttributes { get; set; }
    public GetAccessibleTextRectFP GetAccessibleTextRect { get; set; }
    public GetAccessibleTextLineBoundsFP GetAccessibleTextLineBounds { get; set; }
    public GetAccessibleTextRangeFP GetAccessibleTextRange { get; set; }
    public GetCurrentAccessibleValueFromContextFP GetCurrentAccessibleValueFromContext { get; set; }
    public GetMaximumAccessibleValueFromContextFP GetMaximumAccessibleValueFromContext { get; set; }
    public GetMinimumAccessibleValueFromContextFP GetMinimumAccessibleValueFromContext { get; set; }
    public AddAccessibleSelectionFromContextFP AddAccessibleSelectionFromContext { get; set; }
    public ClearAccessibleSelectionFromContextFP ClearAccessibleSelectionFromContext { get; set; }
    public GetAccessibleSelectionFromContextFP GetAccessibleSelectionFromContext { get; set; }
    public GetAccessibleSelectionCountFromContextFP GetAccessibleSelectionCountFromContext { get; set; }
    public IsAccessibleChildSelectedFromContextFP IsAccessibleChildSelectedFromContext { get; set; }
    public RemoveAccessibleSelectionFromContextFP RemoveAccessibleSelectionFromContext { get; set; }
    public SelectAllAccessibleSelectionFromContextFP SelectAllAccessibleSelectionFromContext { get; set; }
    public GetAccessibleTableInfoFP GetAccessibleTableInfo { get; set; }
    public GetAccessibleTableCellInfoFP GetAccessibleTableCellInfo { get; set; }
    public GetAccessibleTableRowHeaderFP GetAccessibleTableRowHeader { get; set; }
    public GetAccessibleTableColumnHeaderFP GetAccessibleTableColumnHeader { get; set; }
    public GetAccessibleTableRowDescriptionFP GetAccessibleTableRowDescription { get; set; }
    public GetAccessibleTableColumnDescriptionFP GetAccessibleTableColumnDescription { get; set; }
    public GetAccessibleTableRowSelectionCountFP GetAccessibleTableRowSelectionCount { get; set; }
    public IsAccessibleTableRowSelectedFP IsAccessibleTableRowSelected { get; set; }
    public GetAccessibleTableRowSelectionsFP GetAccessibleTableRowSelections { get; set; }
    public GetAccessibleTableColumnSelectionCountFP GetAccessibleTableColumnSelectionCount { get; set; }
    public IsAccessibleTableColumnSelectedFP IsAccessibleTableColumnSelected { get; set; }
    public GetAccessibleTableColumnSelectionsFP GetAccessibleTableColumnSelections { get; set; }
    public GetAccessibleTableRowFP GetAccessibleTableRow { get; set; }
    public GetAccessibleTableColumnFP GetAccessibleTableColumn { get; set; }
    public GetAccessibleTableIndexFP GetAccessibleTableIndex { get; set; }
    public SetTextContentsFP SetTextContents { get; set; }
    public GetParentWithRoleFP GetParentWithRole { get; set; }
    public GetParentWithRoleElseRootFP GetParentWithRoleElseRoot { get; set; }
    public GetTopLevelObjectFP GetTopLevelObject { get; set; }
    public GetObjectDepthFP GetObjectDepth { get; set; }
    public GetActiveDescendentFP GetActiveDescendent { get; set; }
    public GetVirtualAccessibleNameFP GetVirtualAccessibleName { get; set; }
    public GetTextAttributesInRangeFP GetTextAttributesInRange { get; set; }
    public GetCaretLocationFP GetCaretLocation { get; set; }
    public GetVisibleChildrenCountFP GetVisibleChildrenCount { get; set; }
    public GetVisibleChildrenFP GetVisibleChildren { get; set; }
    public GetVersionInfoFP GetVersionInfo { get; set; }
    #endregion

    #region Event functions
    public SetPropertyChangeFP SetPropertyChange { get; set; }
    public SetJavaShutdownFP SetJavaShutdown { get; set; }
    public SetFocusGainedFP SetFocusGained { get; set; }
    public SetFocusLostFP SetFocusLost { get; set; }
    public SetCaretUpdateFP SetCaretUpdate { get; set; }
    public SetMouseClickedFP SetMouseClicked { get; set; }
    public SetMouseEnteredFP SetMouseEntered { get; set; }
    public SetMouseExitedFP SetMouseExited { get; set; }
    public SetMousePressedFP SetMousePressed { get; set; }
    public SetMouseReleasedFP SetMouseReleased { get; set; }
    public SetMenuCanceledFP SetMenuCanceled { get; set; }
    public SetMenuDeselectedFP SetMenuDeselected { get; set; }
    public SetMenuSelectedFP SetMenuSelected { get; set; }
    public SetPopupMenuCanceledFP SetPopupMenuCanceled { get; set; }
    public SetPopupMenuWillBecomeInvisibleFP SetPopupMenuWillBecomeInvisible { get; set; }
    public SetPopupMenuWillBecomeVisibleFP SetPopupMenuWillBecomeVisible { get; set; }
    public SetPropertyNameChangeFP SetPropertyNameChange { get; set; }
    public SetPropertyDescriptionChangeFP SetPropertyDescriptionChange { get; set; }
    public SetPropertyStateChangeFP SetPropertyStateChange { get; set; }
    public SetPropertyValueChangeFP SetPropertyValueChange { get; set; }
    public SetPropertySelectionChangeFP SetPropertySelectionChange { get; set; }
    public SetPropertyTextChangeFP SetPropertyTextChange { get; set; }
    public SetPropertyCaretChangeFP SetPropertyCaretChange { get; set; }
    public SetPropertyVisibleDataChangeFP SetPropertyVisibleDataChange { get; set; }
    public SetPropertyChildChangeFP SetPropertyChildChange { get; set; }
    public SetPropertyActiveDescendentChangeFP SetPropertyActiveDescendentChange { get; set; }
    public SetPropertyTableModelChangeFP SetPropertyTableModelChange { get; set; }
    #endregion

    #region Function delegate types
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void Windows_runFP();
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL IsJavaWindowFP(WindowHandle window);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL IsSameObjectFP(int vmid, JOBJECT32 obj1, JOBJECT32 obj2);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleContextFromHWNDFP(WindowHandle window, out int vmid, out JOBJECT32 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate WindowHandle GetHWNDFromAccessibleContextFP(int vmid, JOBJECT32 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleContextAtFP(int vmid, JOBJECT32 acParent, int x, int y, out JOBJECT32 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleContextWithFocusFP(WindowHandle window, out int vmid, out JOBJECT32 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleContextInfoFP(int vmid, JOBJECT32 ac, out AccessibleContextInfoNativeLegacy info);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT32 GetAccessibleChildFromContextFP(int vmid, JOBJECT32 ac, int i);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT32 GetAccessibleParentFromContextFP(int vmid, JOBJECT32 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleRelationSetFP(int vmid, JOBJECT32 accessibleContext, out AccessibleRelationSetInfoNativeLegacy relationSetInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleHypertextFP(int vmid, JOBJECT32 accessibleContext, out AccessibleHypertextInfoNativeLegacy hypertextInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL ActivateAccessibleHyperlinkFP(int vmid, JOBJECT32 accessibleContext, JOBJECT32 accessibleHyperlink);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleHyperlinkCountFP(int vmid, JOBJECT32 accessibleContext);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleHypertextExtFP(int vmid, JOBJECT32 accessibleContext, int nStartIndex, out AccessibleHypertextInfoNativeLegacy hypertextInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleHypertextLinkIndexFP(int vmid, JOBJECT32 hypertext, int nIndex);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleHyperlinkFP(int vmid, JOBJECT32 hypertext, int nIndex, out AccessibleHyperlinkInfoNativeLegacy hyperlinkInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleKeyBindingsFP(int vmid, JOBJECT32 accessibleContext, out AccessibleKeyBindingsNativeLegacy keyBindings);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleIconsFP(int vmid, JOBJECT32 accessibleContext, out AccessibleIconsNativeLegacy icons);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleActionsFP(int vmid, JOBJECT32 accessibleContext, [Out]AccessibleActionsNativeLegacy actions);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL DoAccessibleActionsFP(int vmid, JOBJECT32 accessibleContext, ref AccessibleActionsToDoNativeLegacy actionsToDo, out int failure);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextInfoFP(int vmid, JOBJECT32 at, out AccessibleTextInfoNativeLegacy textInfo, int x, int y);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextItemsFP(int vmid, JOBJECT32 at, out AccessibleTextItemsInfoNativeLegacy textItems, int index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextSelectionInfoFP(int vmid, JOBJECT32 at, out AccessibleTextSelectionInfoNativeLegacy textSelection);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextAttributesFP(int vmid, JOBJECT32 at, int index, out AccessibleTextAttributesInfoNativeLegacy attributes);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextRectFP(int vmid, JOBJECT32 at, out AccessibleTextRectInfoNativeLegacy rectInfo, int index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextLineBoundsFP(int vmid, JOBJECT32 at, int index, out int startIndex, out int endIndex);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextRangeFP(int vmid, JOBJECT32 at, int start, int end, StringBuilder text, short len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetCurrentAccessibleValueFromContextFP(int vmid, JOBJECT32 av, StringBuilder value, short len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetMaximumAccessibleValueFromContextFP(int vmid, JOBJECT32 av, StringBuilder value, short len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetMinimumAccessibleValueFromContextFP(int vmid, JOBJECT32 av, StringBuilder value, short len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void AddAccessibleSelectionFromContextFP(int vmid, JOBJECT32 asel, int i);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void ClearAccessibleSelectionFromContextFP(int vmid, JOBJECT32 asel);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT32 GetAccessibleSelectionFromContextFP(int vmid, JOBJECT32 asel, int i);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleSelectionCountFromContextFP(int vmid, JOBJECT32 asel);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL IsAccessibleChildSelectedFromContextFP(int vmid, JOBJECT32 asel, int i);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void RemoveAccessibleSelectionFromContextFP(int vmid, JOBJECT32 asel, int i);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void SelectAllAccessibleSelectionFromContextFP(int vmid, JOBJECT32 asel);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableInfoFP(int vmid, JOBJECT32 ac, out AccessibleTableInfoNativeLegacy tableInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableCellInfoFP(int vmid, JOBJECT32 accessibleTable, int row, int column, out AccessibleTableCellInfoNativeLegacy tableCellInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableRowHeaderFP(int vmid, JOBJECT32 acParent, out AccessibleTableInfoNativeLegacy tableInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableColumnHeaderFP(int vmid, JOBJECT32 acParent, out AccessibleTableInfoNativeLegacy tableInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT32 GetAccessibleTableRowDescriptionFP(int vmid, JOBJECT32 acParent, int row);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT32 GetAccessibleTableColumnDescriptionFP(int vmid, JOBJECT32 acParent, int column);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleTableRowSelectionCountFP(int vmid, JOBJECT32 table);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL IsAccessibleTableRowSelectedFP(int vmid, JOBJECT32 table, int row);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableRowSelectionsFP(int vmid, JOBJECT32 table, int count, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]int[] selections);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleTableColumnSelectionCountFP(int vmid, JOBJECT32 table);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL IsAccessibleTableColumnSelectedFP(int vmid, JOBJECT32 table, int column);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableColumnSelectionsFP(int vmid, JOBJECT32 table, int count, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]int[] selections);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleTableRowFP(int vmid, JOBJECT32 table, int index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleTableColumnFP(int vmid, JOBJECT32 table, int index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleTableIndexFP(int vmid, JOBJECT32 table, int row, int column);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetTextContentsFP(int vmid, JOBJECT32 ac, string text);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT32 GetParentWithRoleFP(int vmid, JOBJECT32 ac, [MarshalAs(UnmanagedType.LPWStr)]string role);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT32 GetParentWithRoleElseRootFP(int vmid, JOBJECT32 ac, [MarshalAs(UnmanagedType.LPWStr)]string role);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT32 GetTopLevelObjectFP(int vmid, JOBJECT32 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetObjectDepthFP(int vmid, JOBJECT32 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT32 GetActiveDescendentFP(int vmid, JOBJECT32 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetVirtualAccessibleNameFP(int vmid, JOBJECT32 ac, StringBuilder name, int len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetTextAttributesInRangeFP(int vmid, JOBJECT32 accessibleContext, int startIndex, int endIndex, out AccessibleTextAttributesInfoNativeLegacy attributes, out short len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetCaretLocationFP(int vmid, JOBJECT32 ac, out AccessibleTextRectInfoNativeLegacy rectInfo, int index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetVisibleChildrenCountFP(int vmid, JOBJECT32 accessibleContext);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetVisibleChildrenFP(int vmid, JOBJECT32 accessibleContext, int startIndex, out VisibleChildrenInfoNativeLegacy children);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetVersionInfoFP(int vmid, out AccessBridgeVersionInfoNativeLegacy info);
    #endregion

    #region Event delegate types
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyChangeEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source, [MarshalAs(UnmanagedType.LPWStr)]string property, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void JavaShutdownEventHandler(int vmid);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void FocusGainedEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void FocusLostEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void CaretUpdateEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void MouseClickedEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void MouseEnteredEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void MouseExitedEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void MousePressedEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void MouseReleasedEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void MenuCanceledEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void MenuDeselectedEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void MenuSelectedEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PopupMenuCanceledEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PopupMenuWillBecomeInvisibleEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PopupMenuWillBecomeVisibleEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyNameChangeEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source, [MarshalAs(UnmanagedType.LPWStr)]string oldName, [MarshalAs(UnmanagedType.LPWStr)]string newName);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyDescriptionChangeEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source, [MarshalAs(UnmanagedType.LPWStr)]string oldDescription, [MarshalAs(UnmanagedType.LPWStr)]string newDescription);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyStateChangeEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source, [MarshalAs(UnmanagedType.LPWStr)]string oldState, [MarshalAs(UnmanagedType.LPWStr)]string newState);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyValueChangeEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertySelectionChangeEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyTextChangeEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyCaretChangeEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source, int oldPosition, int newPosition);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyVisibleDataChangeEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyChildChangeEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source, JOBJECT32 oldChild, JOBJECT32 newChild);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyActiveDescendentChangeEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 source, JOBJECT32 oldActiveDescendent, JOBJECT32 newActiveDescendent);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void PropertyTableModelChangeEventHandler(int vmid, JOBJECT32 evt, JOBJECT32 src, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue);
    #endregion

    #region Event function delegate types
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyChangeFP(PropertyChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetJavaShutdownFP(JavaShutdownEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetFocusGainedFP(FocusGainedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetFocusLostFP(FocusLostEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetCaretUpdateFP(CaretUpdateEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetMouseClickedFP(MouseClickedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetMouseEnteredFP(MouseEnteredEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetMouseExitedFP(MouseExitedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetMousePressedFP(MousePressedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetMouseReleasedFP(MouseReleasedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetMenuCanceledFP(MenuCanceledEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetMenuDeselectedFP(MenuDeselectedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetMenuSelectedFP(MenuSelectedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPopupMenuCanceledFP(PopupMenuCanceledEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPopupMenuWillBecomeInvisibleFP(PopupMenuWillBecomeInvisibleEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPopupMenuWillBecomeVisibleFP(PopupMenuWillBecomeVisibleEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyNameChangeFP(PropertyNameChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyDescriptionChangeFP(PropertyDescriptionChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyStateChangeFP(PropertyStateChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyValueChangeFP(PropertyValueChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertySelectionChangeFP(PropertySelectionChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyTextChangeFP(PropertyTextChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyCaretChangeFP(PropertyCaretChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyVisibleDataChangeFP(PropertyVisibleDataChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyChildChangeFP(PropertyChildChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyActiveDescendentChangeFP(PropertyActiveDescendentChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetPropertyTableModelChangeFP(PropertyTableModelChangeEventHandler handler);
    #endregion
  }

  /// <summary>
  /// Native library event handlers implementation
  /// </summary>
  public partial class AccessBridgeEventsNative {
    #region Event fields
    private AccessBridgeLibraryFunctions.PropertyChangeEventHandler _propertyChange;
    private AccessBridgeLibraryFunctions.JavaShutdownEventHandler _javaShutdown;
    private AccessBridgeLibraryFunctions.FocusGainedEventHandler _focusGained;
    private AccessBridgeLibraryFunctions.FocusLostEventHandler _focusLost;
    private AccessBridgeLibraryFunctions.CaretUpdateEventHandler _caretUpdate;
    private AccessBridgeLibraryFunctions.MouseClickedEventHandler _mouseClicked;
    private AccessBridgeLibraryFunctions.MouseEnteredEventHandler _mouseEntered;
    private AccessBridgeLibraryFunctions.MouseExitedEventHandler _mouseExited;
    private AccessBridgeLibraryFunctions.MousePressedEventHandler _mousePressed;
    private AccessBridgeLibraryFunctions.MouseReleasedEventHandler _mouseReleased;
    private AccessBridgeLibraryFunctions.MenuCanceledEventHandler _menuCanceled;
    private AccessBridgeLibraryFunctions.MenuDeselectedEventHandler _menuDeselected;
    private AccessBridgeLibraryFunctions.MenuSelectedEventHandler _menuSelected;
    private AccessBridgeLibraryFunctions.PopupMenuCanceledEventHandler _popupMenuCanceled;
    private AccessBridgeLibraryFunctions.PopupMenuWillBecomeInvisibleEventHandler _popupMenuWillBecomeInvisible;
    private AccessBridgeLibraryFunctions.PopupMenuWillBecomeVisibleEventHandler _popupMenuWillBecomeVisible;
    private AccessBridgeLibraryFunctions.PropertyNameChangeEventHandler _propertyNameChange;
    private AccessBridgeLibraryFunctions.PropertyDescriptionChangeEventHandler _propertyDescriptionChange;
    private AccessBridgeLibraryFunctions.PropertyStateChangeEventHandler _propertyStateChange;
    private AccessBridgeLibraryFunctions.PropertyValueChangeEventHandler _propertyValueChange;
    private AccessBridgeLibraryFunctions.PropertySelectionChangeEventHandler _propertySelectionChange;
    private AccessBridgeLibraryFunctions.PropertyTextChangeEventHandler _propertyTextChange;
    private AccessBridgeLibraryFunctions.PropertyCaretChangeEventHandler _propertyCaretChange;
    private AccessBridgeLibraryFunctions.PropertyVisibleDataChangeEventHandler _propertyVisibleDataChange;
    private AccessBridgeLibraryFunctions.PropertyChildChangeEventHandler _propertyChildChange;
    private AccessBridgeLibraryFunctions.PropertyActiveDescendentChangeEventHandler _propertyActiveDescendentChange;
    private AccessBridgeLibraryFunctions.PropertyTableModelChangeEventHandler _propertyTableModelChange;
    #endregion

    #region Event properties
    public event AccessBridgeLibraryFunctions.PropertyChangeEventHandler PropertyChange {
      add {
        if (_propertyChange == null)
          LibraryFunctions.SetPropertyChange(OnPropertyChange);
        _propertyChange += value;
      }
      remove{
        _propertyChange -= value;
        if (_propertyChange == null)
          LibraryFunctions.SetPropertyChange(null);
      }
    }
    public event AccessBridgeLibraryFunctions.JavaShutdownEventHandler JavaShutdown {
      add {
        if (_javaShutdown == null)
          LibraryFunctions.SetJavaShutdown(OnJavaShutdown);
        _javaShutdown += value;
      }
      remove{
        _javaShutdown -= value;
        if (_javaShutdown == null)
          LibraryFunctions.SetJavaShutdown(null);
      }
    }
    public event AccessBridgeLibraryFunctions.FocusGainedEventHandler FocusGained {
      add {
        if (_focusGained == null)
          LibraryFunctions.SetFocusGained(OnFocusGained);
        _focusGained += value;
      }
      remove{
        _focusGained -= value;
        if (_focusGained == null)
          LibraryFunctions.SetFocusGained(null);
      }
    }
    public event AccessBridgeLibraryFunctions.FocusLostEventHandler FocusLost {
      add {
        if (_focusLost == null)
          LibraryFunctions.SetFocusLost(OnFocusLost);
        _focusLost += value;
      }
      remove{
        _focusLost -= value;
        if (_focusLost == null)
          LibraryFunctions.SetFocusLost(null);
      }
    }
    public event AccessBridgeLibraryFunctions.CaretUpdateEventHandler CaretUpdate {
      add {
        if (_caretUpdate == null)
          LibraryFunctions.SetCaretUpdate(OnCaretUpdate);
        _caretUpdate += value;
      }
      remove{
        _caretUpdate -= value;
        if (_caretUpdate == null)
          LibraryFunctions.SetCaretUpdate(null);
      }
    }
    public event AccessBridgeLibraryFunctions.MouseClickedEventHandler MouseClicked {
      add {
        if (_mouseClicked == null)
          LibraryFunctions.SetMouseClicked(OnMouseClicked);
        _mouseClicked += value;
      }
      remove{
        _mouseClicked -= value;
        if (_mouseClicked == null)
          LibraryFunctions.SetMouseClicked(null);
      }
    }
    public event AccessBridgeLibraryFunctions.MouseEnteredEventHandler MouseEntered {
      add {
        if (_mouseEntered == null)
          LibraryFunctions.SetMouseEntered(OnMouseEntered);
        _mouseEntered += value;
      }
      remove{
        _mouseEntered -= value;
        if (_mouseEntered == null)
          LibraryFunctions.SetMouseEntered(null);
      }
    }
    public event AccessBridgeLibraryFunctions.MouseExitedEventHandler MouseExited {
      add {
        if (_mouseExited == null)
          LibraryFunctions.SetMouseExited(OnMouseExited);
        _mouseExited += value;
      }
      remove{
        _mouseExited -= value;
        if (_mouseExited == null)
          LibraryFunctions.SetMouseExited(null);
      }
    }
    public event AccessBridgeLibraryFunctions.MousePressedEventHandler MousePressed {
      add {
        if (_mousePressed == null)
          LibraryFunctions.SetMousePressed(OnMousePressed);
        _mousePressed += value;
      }
      remove{
        _mousePressed -= value;
        if (_mousePressed == null)
          LibraryFunctions.SetMousePressed(null);
      }
    }
    public event AccessBridgeLibraryFunctions.MouseReleasedEventHandler MouseReleased {
      add {
        if (_mouseReleased == null)
          LibraryFunctions.SetMouseReleased(OnMouseReleased);
        _mouseReleased += value;
      }
      remove{
        _mouseReleased -= value;
        if (_mouseReleased == null)
          LibraryFunctions.SetMouseReleased(null);
      }
    }
    public event AccessBridgeLibraryFunctions.MenuCanceledEventHandler MenuCanceled {
      add {
        if (_menuCanceled == null)
          LibraryFunctions.SetMenuCanceled(OnMenuCanceled);
        _menuCanceled += value;
      }
      remove{
        _menuCanceled -= value;
        if (_menuCanceled == null)
          LibraryFunctions.SetMenuCanceled(null);
      }
    }
    public event AccessBridgeLibraryFunctions.MenuDeselectedEventHandler MenuDeselected {
      add {
        if (_menuDeselected == null)
          LibraryFunctions.SetMenuDeselected(OnMenuDeselected);
        _menuDeselected += value;
      }
      remove{
        _menuDeselected -= value;
        if (_menuDeselected == null)
          LibraryFunctions.SetMenuDeselected(null);
      }
    }
    public event AccessBridgeLibraryFunctions.MenuSelectedEventHandler MenuSelected {
      add {
        if (_menuSelected == null)
          LibraryFunctions.SetMenuSelected(OnMenuSelected);
        _menuSelected += value;
      }
      remove{
        _menuSelected -= value;
        if (_menuSelected == null)
          LibraryFunctions.SetMenuSelected(null);
      }
    }
    public event AccessBridgeLibraryFunctions.PopupMenuCanceledEventHandler PopupMenuCanceled {
      add {
        if (_popupMenuCanceled == null)
          LibraryFunctions.SetPopupMenuCanceled(OnPopupMenuCanceled);
        _popupMenuCanceled += value;
      }
      remove{
        _popupMenuCanceled -= value;
        if (_popupMenuCanceled == null)
          LibraryFunctions.SetPopupMenuCanceled(null);
      }
    }
    public event AccessBridgeLibraryFunctions.PopupMenuWillBecomeInvisibleEventHandler PopupMenuWillBecomeInvisible {
      add {
        if (_popupMenuWillBecomeInvisible == null)
          LibraryFunctions.SetPopupMenuWillBecomeInvisible(OnPopupMenuWillBecomeInvisible);
        _popupMenuWillBecomeInvisible += value;
      }
      remove{
        _popupMenuWillBecomeInvisible -= value;
        if (_popupMenuWillBecomeInvisible == null)
          LibraryFunctions.SetPopupMenuWillBecomeInvisible(null);
      }
    }
    public event AccessBridgeLibraryFunctions.PopupMenuWillBecomeVisibleEventHandler PopupMenuWillBecomeVisible {
      add {
        if (_popupMenuWillBecomeVisible == null)
          LibraryFunctions.SetPopupMenuWillBecomeVisible(OnPopupMenuWillBecomeVisible);
        _popupMenuWillBecomeVisible += value;
      }
      remove{
        _popupMenuWillBecomeVisible -= value;
        if (_popupMenuWillBecomeVisible == null)
          LibraryFunctions.SetPopupMenuWillBecomeVisible(null);
      }
    }
    public event AccessBridgeLibraryFunctions.PropertyNameChangeEventHandler PropertyNameChange {
      add {
        if (_propertyNameChange == null)
          LibraryFunctions.SetPropertyNameChange(OnPropertyNameChange);
        _propertyNameChange += value;
      }
      remove{
        _propertyNameChange -= value;
        if (_propertyNameChange == null)
          LibraryFunctions.SetPropertyNameChange(null);
      }
    }
    public event AccessBridgeLibraryFunctions.PropertyDescriptionChangeEventHandler PropertyDescriptionChange {
      add {
        if (_propertyDescriptionChange == null)
          LibraryFunctions.SetPropertyDescriptionChange(OnPropertyDescriptionChange);
        _propertyDescriptionChange += value;
      }
      remove{
        _propertyDescriptionChange -= value;
        if (_propertyDescriptionChange == null)
          LibraryFunctions.SetPropertyDescriptionChange(null);
      }
    }
    public event AccessBridgeLibraryFunctions.PropertyStateChangeEventHandler PropertyStateChange {
      add {
        if (_propertyStateChange == null)
          LibraryFunctions.SetPropertyStateChange(OnPropertyStateChange);
        _propertyStateChange += value;
      }
      remove{
        _propertyStateChange -= value;
        if (_propertyStateChange == null)
          LibraryFunctions.SetPropertyStateChange(null);
      }
    }
    public event AccessBridgeLibraryFunctions.PropertyValueChangeEventHandler PropertyValueChange {
      add {
        if (_propertyValueChange == null)
          LibraryFunctions.SetPropertyValueChange(OnPropertyValueChange);
        _propertyValueChange += value;
      }
      remove{
        _propertyValueChange -= value;
        if (_propertyValueChange == null)
          LibraryFunctions.SetPropertyValueChange(null);
      }
    }
    public event AccessBridgeLibraryFunctions.PropertySelectionChangeEventHandler PropertySelectionChange {
      add {
        if (_propertySelectionChange == null)
          LibraryFunctions.SetPropertySelectionChange(OnPropertySelectionChange);
        _propertySelectionChange += value;
      }
      remove{
        _propertySelectionChange -= value;
        if (_propertySelectionChange == null)
          LibraryFunctions.SetPropertySelectionChange(null);
      }
    }
    public event AccessBridgeLibraryFunctions.PropertyTextChangeEventHandler PropertyTextChange {
      add {
        if (_propertyTextChange == null)
          LibraryFunctions.SetPropertyTextChange(OnPropertyTextChange);
        _propertyTextChange += value;
      }
      remove{
        _propertyTextChange -= value;
        if (_propertyTextChange == null)
          LibraryFunctions.SetPropertyTextChange(null);
      }
    }
    public event AccessBridgeLibraryFunctions.PropertyCaretChangeEventHandler PropertyCaretChange {
      add {
        if (_propertyCaretChange == null)
          LibraryFunctions.SetPropertyCaretChange(OnPropertyCaretChange);
        _propertyCaretChange += value;
      }
      remove{
        _propertyCaretChange -= value;
        if (_propertyCaretChange == null)
          LibraryFunctions.SetPropertyCaretChange(null);
      }
    }
    public event AccessBridgeLibraryFunctions.PropertyVisibleDataChangeEventHandler PropertyVisibleDataChange {
      add {
        if (_propertyVisibleDataChange == null)
          LibraryFunctions.SetPropertyVisibleDataChange(OnPropertyVisibleDataChange);
        _propertyVisibleDataChange += value;
      }
      remove{
        _propertyVisibleDataChange -= value;
        if (_propertyVisibleDataChange == null)
          LibraryFunctions.SetPropertyVisibleDataChange(null);
      }
    }
    public event AccessBridgeLibraryFunctions.PropertyChildChangeEventHandler PropertyChildChange {
      add {
        if (_propertyChildChange == null)
          LibraryFunctions.SetPropertyChildChange(OnPropertyChildChange);
        _propertyChildChange += value;
      }
      remove{
        _propertyChildChange -= value;
        if (_propertyChildChange == null)
          LibraryFunctions.SetPropertyChildChange(null);
      }
    }
    public event AccessBridgeLibraryFunctions.PropertyActiveDescendentChangeEventHandler PropertyActiveDescendentChange {
      add {
        if (_propertyActiveDescendentChange == null)
          LibraryFunctions.SetPropertyActiveDescendentChange(OnPropertyActiveDescendentChange);
        _propertyActiveDescendentChange += value;
      }
      remove{
        _propertyActiveDescendentChange -= value;
        if (_propertyActiveDescendentChange == null)
          LibraryFunctions.SetPropertyActiveDescendentChange(null);
      }
    }
    public event AccessBridgeLibraryFunctions.PropertyTableModelChangeEventHandler PropertyTableModelChange {
      add {
        if (_propertyTableModelChange == null)
          LibraryFunctions.SetPropertyTableModelChange(OnPropertyTableModelChange);
        _propertyTableModelChange += value;
      }
      remove{
        _propertyTableModelChange -= value;
        if (_propertyTableModelChange == null)
          LibraryFunctions.SetPropertyTableModelChange(null);
      }
    }
    #endregion

    #region Event handlers
    protected virtual void OnPropertyChange(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string property, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue) {
      var handler = _propertyChange;
      if (handler != null)
        handler(vmid, evt, source, property, oldValue, newValue);
    }
    protected virtual void OnJavaShutdown(int vmid) {
      var handler = _javaShutdown;
      if (handler != null)
        handler(vmid);
    }
    protected virtual void OnFocusGained(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = _focusGained;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnFocusLost(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = _focusLost;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnCaretUpdate(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = _caretUpdate;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseClicked(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = _mouseClicked;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseEntered(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = _mouseEntered;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseExited(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = _mouseExited;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMousePressed(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = _mousePressed;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseReleased(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = _mouseReleased;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMenuCanceled(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = _menuCanceled;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMenuDeselected(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = _menuDeselected;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMenuSelected(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = _menuSelected;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPopupMenuCanceled(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = _popupMenuCanceled;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPopupMenuWillBecomeInvisible(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = _popupMenuWillBecomeInvisible;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPopupMenuWillBecomeVisible(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = _popupMenuWillBecomeVisible;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyNameChange(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string oldName, [MarshalAs(UnmanagedType.LPWStr)]string newName) {
      var handler = _propertyNameChange;
      if (handler != null)
        handler(vmid, evt, source, oldName, newName);
    }
    protected virtual void OnPropertyDescriptionChange(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string oldDescription, [MarshalAs(UnmanagedType.LPWStr)]string newDescription) {
      var handler = _propertyDescriptionChange;
      if (handler != null)
        handler(vmid, evt, source, oldDescription, newDescription);
    }
    protected virtual void OnPropertyStateChange(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string oldState, [MarshalAs(UnmanagedType.LPWStr)]string newState) {
      var handler = _propertyStateChange;
      if (handler != null)
        handler(vmid, evt, source, oldState, newState);
    }
    protected virtual void OnPropertyValueChange(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue) {
      var handler = _propertyValueChange;
      if (handler != null)
        handler(vmid, evt, source, oldValue, newValue);
    }
    protected virtual void OnPropertySelectionChange(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = _propertySelectionChange;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyTextChange(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = _propertyTextChange;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyCaretChange(int vmid, JOBJECT64 evt, JOBJECT64 source, int oldPosition, int newPosition) {
      var handler = _propertyCaretChange;
      if (handler != null)
        handler(vmid, evt, source, oldPosition, newPosition);
    }
    protected virtual void OnPropertyVisibleDataChange(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = _propertyVisibleDataChange;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyChildChange(int vmid, JOBJECT64 evt, JOBJECT64 source, JOBJECT64 oldChild, JOBJECT64 newChild) {
      var handler = _propertyChildChange;
      if (handler != null)
        handler(vmid, evt, source, oldChild, newChild);
    }
    protected virtual void OnPropertyActiveDescendentChange(int vmid, JOBJECT64 evt, JOBJECT64 source, JOBJECT64 oldActiveDescendent, JOBJECT64 newActiveDescendent) {
      var handler = _propertyActiveDescendentChange;
      if (handler != null)
        handler(vmid, evt, source, oldActiveDescendent, newActiveDescendent);
    }
    protected virtual void OnPropertyTableModelChange(int vmid, JOBJECT64 evt, JOBJECT64 src, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue) {
      var handler = _propertyTableModelChange;
      if (handler != null)
        handler(vmid, evt, src, oldValue, newValue);
    }
    #endregion
  }

  /// <summary>
  /// Native library event handlers implementation
  /// </summary>
  public partial class AccessBridgeEventsNativeLegacy {
    #region Event fields
    private AccessBridgeLibraryFunctionsLegacy.PropertyChangeEventHandler _propertyChange;
    private AccessBridgeLibraryFunctionsLegacy.JavaShutdownEventHandler _javaShutdown;
    private AccessBridgeLibraryFunctionsLegacy.FocusGainedEventHandler _focusGained;
    private AccessBridgeLibraryFunctionsLegacy.FocusLostEventHandler _focusLost;
    private AccessBridgeLibraryFunctionsLegacy.CaretUpdateEventHandler _caretUpdate;
    private AccessBridgeLibraryFunctionsLegacy.MouseClickedEventHandler _mouseClicked;
    private AccessBridgeLibraryFunctionsLegacy.MouseEnteredEventHandler _mouseEntered;
    private AccessBridgeLibraryFunctionsLegacy.MouseExitedEventHandler _mouseExited;
    private AccessBridgeLibraryFunctionsLegacy.MousePressedEventHandler _mousePressed;
    private AccessBridgeLibraryFunctionsLegacy.MouseReleasedEventHandler _mouseReleased;
    private AccessBridgeLibraryFunctionsLegacy.MenuCanceledEventHandler _menuCanceled;
    private AccessBridgeLibraryFunctionsLegacy.MenuDeselectedEventHandler _menuDeselected;
    private AccessBridgeLibraryFunctionsLegacy.MenuSelectedEventHandler _menuSelected;
    private AccessBridgeLibraryFunctionsLegacy.PopupMenuCanceledEventHandler _popupMenuCanceled;
    private AccessBridgeLibraryFunctionsLegacy.PopupMenuWillBecomeInvisibleEventHandler _popupMenuWillBecomeInvisible;
    private AccessBridgeLibraryFunctionsLegacy.PopupMenuWillBecomeVisibleEventHandler _popupMenuWillBecomeVisible;
    private AccessBridgeLibraryFunctionsLegacy.PropertyNameChangeEventHandler _propertyNameChange;
    private AccessBridgeLibraryFunctionsLegacy.PropertyDescriptionChangeEventHandler _propertyDescriptionChange;
    private AccessBridgeLibraryFunctionsLegacy.PropertyStateChangeEventHandler _propertyStateChange;
    private AccessBridgeLibraryFunctionsLegacy.PropertyValueChangeEventHandler _propertyValueChange;
    private AccessBridgeLibraryFunctionsLegacy.PropertySelectionChangeEventHandler _propertySelectionChange;
    private AccessBridgeLibraryFunctionsLegacy.PropertyTextChangeEventHandler _propertyTextChange;
    private AccessBridgeLibraryFunctionsLegacy.PropertyCaretChangeEventHandler _propertyCaretChange;
    private AccessBridgeLibraryFunctionsLegacy.PropertyVisibleDataChangeEventHandler _propertyVisibleDataChange;
    private AccessBridgeLibraryFunctionsLegacy.PropertyChildChangeEventHandler _propertyChildChange;
    private AccessBridgeLibraryFunctionsLegacy.PropertyActiveDescendentChangeEventHandler _propertyActiveDescendentChange;
    private AccessBridgeLibraryFunctionsLegacy.PropertyTableModelChangeEventHandler _propertyTableModelChange;
    #endregion

    #region Event properties
    public event AccessBridgeLibraryFunctionsLegacy.PropertyChangeEventHandler PropertyChange {
      add {
        if (_propertyChange == null)
          LibraryFunctions.SetPropertyChange(OnPropertyChange);
        _propertyChange += value;
      }
      remove{
        _propertyChange -= value;
        if (_propertyChange == null)
          LibraryFunctions.SetPropertyChange(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.JavaShutdownEventHandler JavaShutdown {
      add {
        if (_javaShutdown == null)
          LibraryFunctions.SetJavaShutdown(OnJavaShutdown);
        _javaShutdown += value;
      }
      remove{
        _javaShutdown -= value;
        if (_javaShutdown == null)
          LibraryFunctions.SetJavaShutdown(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.FocusGainedEventHandler FocusGained {
      add {
        if (_focusGained == null)
          LibraryFunctions.SetFocusGained(OnFocusGained);
        _focusGained += value;
      }
      remove{
        _focusGained -= value;
        if (_focusGained == null)
          LibraryFunctions.SetFocusGained(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.FocusLostEventHandler FocusLost {
      add {
        if (_focusLost == null)
          LibraryFunctions.SetFocusLost(OnFocusLost);
        _focusLost += value;
      }
      remove{
        _focusLost -= value;
        if (_focusLost == null)
          LibraryFunctions.SetFocusLost(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.CaretUpdateEventHandler CaretUpdate {
      add {
        if (_caretUpdate == null)
          LibraryFunctions.SetCaretUpdate(OnCaretUpdate);
        _caretUpdate += value;
      }
      remove{
        _caretUpdate -= value;
        if (_caretUpdate == null)
          LibraryFunctions.SetCaretUpdate(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.MouseClickedEventHandler MouseClicked {
      add {
        if (_mouseClicked == null)
          LibraryFunctions.SetMouseClicked(OnMouseClicked);
        _mouseClicked += value;
      }
      remove{
        _mouseClicked -= value;
        if (_mouseClicked == null)
          LibraryFunctions.SetMouseClicked(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.MouseEnteredEventHandler MouseEntered {
      add {
        if (_mouseEntered == null)
          LibraryFunctions.SetMouseEntered(OnMouseEntered);
        _mouseEntered += value;
      }
      remove{
        _mouseEntered -= value;
        if (_mouseEntered == null)
          LibraryFunctions.SetMouseEntered(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.MouseExitedEventHandler MouseExited {
      add {
        if (_mouseExited == null)
          LibraryFunctions.SetMouseExited(OnMouseExited);
        _mouseExited += value;
      }
      remove{
        _mouseExited -= value;
        if (_mouseExited == null)
          LibraryFunctions.SetMouseExited(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.MousePressedEventHandler MousePressed {
      add {
        if (_mousePressed == null)
          LibraryFunctions.SetMousePressed(OnMousePressed);
        _mousePressed += value;
      }
      remove{
        _mousePressed -= value;
        if (_mousePressed == null)
          LibraryFunctions.SetMousePressed(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.MouseReleasedEventHandler MouseReleased {
      add {
        if (_mouseReleased == null)
          LibraryFunctions.SetMouseReleased(OnMouseReleased);
        _mouseReleased += value;
      }
      remove{
        _mouseReleased -= value;
        if (_mouseReleased == null)
          LibraryFunctions.SetMouseReleased(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.MenuCanceledEventHandler MenuCanceled {
      add {
        if (_menuCanceled == null)
          LibraryFunctions.SetMenuCanceled(OnMenuCanceled);
        _menuCanceled += value;
      }
      remove{
        _menuCanceled -= value;
        if (_menuCanceled == null)
          LibraryFunctions.SetMenuCanceled(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.MenuDeselectedEventHandler MenuDeselected {
      add {
        if (_menuDeselected == null)
          LibraryFunctions.SetMenuDeselected(OnMenuDeselected);
        _menuDeselected += value;
      }
      remove{
        _menuDeselected -= value;
        if (_menuDeselected == null)
          LibraryFunctions.SetMenuDeselected(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.MenuSelectedEventHandler MenuSelected {
      add {
        if (_menuSelected == null)
          LibraryFunctions.SetMenuSelected(OnMenuSelected);
        _menuSelected += value;
      }
      remove{
        _menuSelected -= value;
        if (_menuSelected == null)
          LibraryFunctions.SetMenuSelected(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.PopupMenuCanceledEventHandler PopupMenuCanceled {
      add {
        if (_popupMenuCanceled == null)
          LibraryFunctions.SetPopupMenuCanceled(OnPopupMenuCanceled);
        _popupMenuCanceled += value;
      }
      remove{
        _popupMenuCanceled -= value;
        if (_popupMenuCanceled == null)
          LibraryFunctions.SetPopupMenuCanceled(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.PopupMenuWillBecomeInvisibleEventHandler PopupMenuWillBecomeInvisible {
      add {
        if (_popupMenuWillBecomeInvisible == null)
          LibraryFunctions.SetPopupMenuWillBecomeInvisible(OnPopupMenuWillBecomeInvisible);
        _popupMenuWillBecomeInvisible += value;
      }
      remove{
        _popupMenuWillBecomeInvisible -= value;
        if (_popupMenuWillBecomeInvisible == null)
          LibraryFunctions.SetPopupMenuWillBecomeInvisible(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.PopupMenuWillBecomeVisibleEventHandler PopupMenuWillBecomeVisible {
      add {
        if (_popupMenuWillBecomeVisible == null)
          LibraryFunctions.SetPopupMenuWillBecomeVisible(OnPopupMenuWillBecomeVisible);
        _popupMenuWillBecomeVisible += value;
      }
      remove{
        _popupMenuWillBecomeVisible -= value;
        if (_popupMenuWillBecomeVisible == null)
          LibraryFunctions.SetPopupMenuWillBecomeVisible(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.PropertyNameChangeEventHandler PropertyNameChange {
      add {
        if (_propertyNameChange == null)
          LibraryFunctions.SetPropertyNameChange(OnPropertyNameChange);
        _propertyNameChange += value;
      }
      remove{
        _propertyNameChange -= value;
        if (_propertyNameChange == null)
          LibraryFunctions.SetPropertyNameChange(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.PropertyDescriptionChangeEventHandler PropertyDescriptionChange {
      add {
        if (_propertyDescriptionChange == null)
          LibraryFunctions.SetPropertyDescriptionChange(OnPropertyDescriptionChange);
        _propertyDescriptionChange += value;
      }
      remove{
        _propertyDescriptionChange -= value;
        if (_propertyDescriptionChange == null)
          LibraryFunctions.SetPropertyDescriptionChange(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.PropertyStateChangeEventHandler PropertyStateChange {
      add {
        if (_propertyStateChange == null)
          LibraryFunctions.SetPropertyStateChange(OnPropertyStateChange);
        _propertyStateChange += value;
      }
      remove{
        _propertyStateChange -= value;
        if (_propertyStateChange == null)
          LibraryFunctions.SetPropertyStateChange(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.PropertyValueChangeEventHandler PropertyValueChange {
      add {
        if (_propertyValueChange == null)
          LibraryFunctions.SetPropertyValueChange(OnPropertyValueChange);
        _propertyValueChange += value;
      }
      remove{
        _propertyValueChange -= value;
        if (_propertyValueChange == null)
          LibraryFunctions.SetPropertyValueChange(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.PropertySelectionChangeEventHandler PropertySelectionChange {
      add {
        if (_propertySelectionChange == null)
          LibraryFunctions.SetPropertySelectionChange(OnPropertySelectionChange);
        _propertySelectionChange += value;
      }
      remove{
        _propertySelectionChange -= value;
        if (_propertySelectionChange == null)
          LibraryFunctions.SetPropertySelectionChange(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.PropertyTextChangeEventHandler PropertyTextChange {
      add {
        if (_propertyTextChange == null)
          LibraryFunctions.SetPropertyTextChange(OnPropertyTextChange);
        _propertyTextChange += value;
      }
      remove{
        _propertyTextChange -= value;
        if (_propertyTextChange == null)
          LibraryFunctions.SetPropertyTextChange(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.PropertyCaretChangeEventHandler PropertyCaretChange {
      add {
        if (_propertyCaretChange == null)
          LibraryFunctions.SetPropertyCaretChange(OnPropertyCaretChange);
        _propertyCaretChange += value;
      }
      remove{
        _propertyCaretChange -= value;
        if (_propertyCaretChange == null)
          LibraryFunctions.SetPropertyCaretChange(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.PropertyVisibleDataChangeEventHandler PropertyVisibleDataChange {
      add {
        if (_propertyVisibleDataChange == null)
          LibraryFunctions.SetPropertyVisibleDataChange(OnPropertyVisibleDataChange);
        _propertyVisibleDataChange += value;
      }
      remove{
        _propertyVisibleDataChange -= value;
        if (_propertyVisibleDataChange == null)
          LibraryFunctions.SetPropertyVisibleDataChange(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.PropertyChildChangeEventHandler PropertyChildChange {
      add {
        if (_propertyChildChange == null)
          LibraryFunctions.SetPropertyChildChange(OnPropertyChildChange);
        _propertyChildChange += value;
      }
      remove{
        _propertyChildChange -= value;
        if (_propertyChildChange == null)
          LibraryFunctions.SetPropertyChildChange(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.PropertyActiveDescendentChangeEventHandler PropertyActiveDescendentChange {
      add {
        if (_propertyActiveDescendentChange == null)
          LibraryFunctions.SetPropertyActiveDescendentChange(OnPropertyActiveDescendentChange);
        _propertyActiveDescendentChange += value;
      }
      remove{
        _propertyActiveDescendentChange -= value;
        if (_propertyActiveDescendentChange == null)
          LibraryFunctions.SetPropertyActiveDescendentChange(null);
      }
    }
    public event AccessBridgeLibraryFunctionsLegacy.PropertyTableModelChangeEventHandler PropertyTableModelChange {
      add {
        if (_propertyTableModelChange == null)
          LibraryFunctions.SetPropertyTableModelChange(OnPropertyTableModelChange);
        _propertyTableModelChange += value;
      }
      remove{
        _propertyTableModelChange -= value;
        if (_propertyTableModelChange == null)
          LibraryFunctions.SetPropertyTableModelChange(null);
      }
    }
    #endregion

    #region Event handlers
    protected virtual void OnPropertyChange(int vmid, JOBJECT32 evt, JOBJECT32 source, [MarshalAs(UnmanagedType.LPWStr)]string property, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue) {
      var handler = _propertyChange;
      if (handler != null)
        handler(vmid, evt, source, property, oldValue, newValue);
    }
    protected virtual void OnJavaShutdown(int vmid) {
      var handler = _javaShutdown;
      if (handler != null)
        handler(vmid);
    }
    protected virtual void OnFocusGained(int vmid, JOBJECT32 evt, JOBJECT32 source) {
      var handler = _focusGained;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnFocusLost(int vmid, JOBJECT32 evt, JOBJECT32 source) {
      var handler = _focusLost;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnCaretUpdate(int vmid, JOBJECT32 evt, JOBJECT32 source) {
      var handler = _caretUpdate;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseClicked(int vmid, JOBJECT32 evt, JOBJECT32 source) {
      var handler = _mouseClicked;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseEntered(int vmid, JOBJECT32 evt, JOBJECT32 source) {
      var handler = _mouseEntered;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseExited(int vmid, JOBJECT32 evt, JOBJECT32 source) {
      var handler = _mouseExited;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMousePressed(int vmid, JOBJECT32 evt, JOBJECT32 source) {
      var handler = _mousePressed;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseReleased(int vmid, JOBJECT32 evt, JOBJECT32 source) {
      var handler = _mouseReleased;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMenuCanceled(int vmid, JOBJECT32 evt, JOBJECT32 source) {
      var handler = _menuCanceled;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMenuDeselected(int vmid, JOBJECT32 evt, JOBJECT32 source) {
      var handler = _menuDeselected;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMenuSelected(int vmid, JOBJECT32 evt, JOBJECT32 source) {
      var handler = _menuSelected;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPopupMenuCanceled(int vmid, JOBJECT32 evt, JOBJECT32 source) {
      var handler = _popupMenuCanceled;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPopupMenuWillBecomeInvisible(int vmid, JOBJECT32 evt, JOBJECT32 source) {
      var handler = _popupMenuWillBecomeInvisible;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPopupMenuWillBecomeVisible(int vmid, JOBJECT32 evt, JOBJECT32 source) {
      var handler = _popupMenuWillBecomeVisible;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyNameChange(int vmid, JOBJECT32 evt, JOBJECT32 source, [MarshalAs(UnmanagedType.LPWStr)]string oldName, [MarshalAs(UnmanagedType.LPWStr)]string newName) {
      var handler = _propertyNameChange;
      if (handler != null)
        handler(vmid, evt, source, oldName, newName);
    }
    protected virtual void OnPropertyDescriptionChange(int vmid, JOBJECT32 evt, JOBJECT32 source, [MarshalAs(UnmanagedType.LPWStr)]string oldDescription, [MarshalAs(UnmanagedType.LPWStr)]string newDescription) {
      var handler = _propertyDescriptionChange;
      if (handler != null)
        handler(vmid, evt, source, oldDescription, newDescription);
    }
    protected virtual void OnPropertyStateChange(int vmid, JOBJECT32 evt, JOBJECT32 source, [MarshalAs(UnmanagedType.LPWStr)]string oldState, [MarshalAs(UnmanagedType.LPWStr)]string newState) {
      var handler = _propertyStateChange;
      if (handler != null)
        handler(vmid, evt, source, oldState, newState);
    }
    protected virtual void OnPropertyValueChange(int vmid, JOBJECT32 evt, JOBJECT32 source, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue) {
      var handler = _propertyValueChange;
      if (handler != null)
        handler(vmid, evt, source, oldValue, newValue);
    }
    protected virtual void OnPropertySelectionChange(int vmid, JOBJECT32 evt, JOBJECT32 source) {
      var handler = _propertySelectionChange;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyTextChange(int vmid, JOBJECT32 evt, JOBJECT32 source) {
      var handler = _propertyTextChange;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyCaretChange(int vmid, JOBJECT32 evt, JOBJECT32 source, int oldPosition, int newPosition) {
      var handler = _propertyCaretChange;
      if (handler != null)
        handler(vmid, evt, source, oldPosition, newPosition);
    }
    protected virtual void OnPropertyVisibleDataChange(int vmid, JOBJECT32 evt, JOBJECT32 source) {
      var handler = _propertyVisibleDataChange;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyChildChange(int vmid, JOBJECT32 evt, JOBJECT32 source, JOBJECT32 oldChild, JOBJECT32 newChild) {
      var handler = _propertyChildChange;
      if (handler != null)
        handler(vmid, evt, source, oldChild, newChild);
    }
    protected virtual void OnPropertyActiveDescendentChange(int vmid, JOBJECT32 evt, JOBJECT32 source, JOBJECT32 oldActiveDescendent, JOBJECT32 newActiveDescendent) {
      var handler = _propertyActiveDescendentChange;
      if (handler != null)
        handler(vmid, evt, source, oldActiveDescendent, newActiveDescendent);
    }
    protected virtual void OnPropertyTableModelChange(int vmid, JOBJECT32 evt, JOBJECT32 src, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue) {
      var handler = _propertyTableModelChange;
      if (handler != null)
        handler(vmid, evt, src, oldValue, newValue);
    }
    #endregion
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessBridgeVersionInfoNative {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string VMversion;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string bridgeJavaClassVersion;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string bridgeJavaDLLVersion;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string bridgeWinDLLVersion;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleActionInfoNative {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string name;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleActionsToDoNative {
    public int actionsCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public AccessibleActionInfoNative[] actions;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleContextInfoNative {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
    public string name;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
    public string description;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string role;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string role_en_US;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string states;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string states_en_US;
    public int indexInParent;
    public int childrenCount;
    public int x;
    public int y;
    public int width;
    public int height;
    public int accessibleComponent;
    public int accessibleAction;
    public int accessibleSelection;
    public int accessibleText;
    public AccessibleInterfaces accessibleInterfaces;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleHyperlinkInfoNative {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string text;
    public int startIndex;
    public int endIndex;
    public JOBJECT64 accessibleHyperlink;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleHypertextInfoNative {
    public int linkCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public AccessibleHyperlinkInfoNative[] links;
    public JOBJECT64 accessibleHypertext;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleIconInfoNative {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string description;
    public int height;
    public int width;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleIconsNative {
    public int iconsCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public AccessibleIconInfoNative[] iconInfo;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleKeyBindingInfoNative {
    public AccessibleKeyCode character;
    public AccessibleModifiers modifiers;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleKeyBindingsNative {
    public int keyBindingsCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
    public AccessibleKeyBindingInfoNative[] keyBindingInfo;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleRelationInfoNative {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string key;
    public int targetCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
    public JOBJECT64[] targets;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleRelationSetInfoNative {
    public int relationCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public AccessibleRelationInfoNative[] relations;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTableCellInfoNative {
    public JOBJECT64 accessibleContext;
    public int index;
    public int row;
    public int column;
    public int rowExtent;
    public int columnExtent;
    public byte isSelected;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTableInfoNative {
    public JOBJECT64 caption;
    public JOBJECT64 summary;
    public int rowCount;
    public int columnCount;
    public JOBJECT64 accessibleContext;
    public JOBJECT64 accessibleTable;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTextAttributesInfoNative {
    public int bold;
    public int italic;
    public int underline;
    public int strikethrough;
    public int superscript;
    public int subscript;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string backgroundColor;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string foregroundColor;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string fontFamily;
    public int fontSize;
    public int alignment;
    public int bidiLevel;
    public float firstLineIndent;
    public float leftIndent;
    public float rightIndent;
    public float lineSpacing;
    public float spaceAbove;
    public float spaceBelow;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
    public string fullAttributesString;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTextInfoNative {
    public int charCount;
    public int caretIndex;
    public int indexAtPoint;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTextItemsInfoNative {
    public char letter;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string word;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
    public string sentence;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTextRectInfoNative {
    public int x;
    public int y;
    public int width;
    public int height;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTextSelectionInfoNative {
    public int selectionStartIndex;
    public int selectionEndIndex;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
    public string selectedText;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct VisibleChildrenInfoNative {
    public int returnedChildrenCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public JOBJECT64[] children;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public class AccessibleActionsNative {
    public int actionsCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public AccessibleActionInfoNative[] actionInfo;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessBridgeVersionInfoNativeLegacy {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string VMversion;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string bridgeJavaClassVersion;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string bridgeJavaDLLVersion;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string bridgeWinDLLVersion;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleActionInfoNativeLegacy {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string name;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleActionsToDoNativeLegacy {
    public int actionsCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public AccessibleActionInfoNativeLegacy[] actions;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleContextInfoNativeLegacy {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
    public string name;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
    public string description;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string role;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string role_en_US;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string states;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string states_en_US;
    public int indexInParent;
    public int childrenCount;
    public int x;
    public int y;
    public int width;
    public int height;
    public int accessibleComponent;
    public int accessibleAction;
    public int accessibleSelection;
    public int accessibleText;
    public AccessibleInterfaces accessibleInterfaces;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleHyperlinkInfoNativeLegacy {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string text;
    public int startIndex;
    public int endIndex;
    public JOBJECT32 accessibleHyperlink;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleHypertextInfoNativeLegacy {
    public int linkCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public AccessibleHyperlinkInfoNativeLegacy[] links;
    public JOBJECT32 accessibleHypertext;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleIconInfoNativeLegacy {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string description;
    public int height;
    public int width;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleIconsNativeLegacy {
    public int iconsCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public AccessibleIconInfoNativeLegacy[] iconInfo;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleKeyBindingInfoNativeLegacy {
    public AccessibleKeyCode character;
    public AccessibleModifiers modifiers;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleKeyBindingsNativeLegacy {
    public int keyBindingsCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
    public AccessibleKeyBindingInfoNativeLegacy[] keyBindingInfo;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleRelationInfoNativeLegacy {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string key;
    public int targetCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
    public JOBJECT32[] targets;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleRelationSetInfoNativeLegacy {
    public int relationCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public AccessibleRelationInfoNativeLegacy[] relations;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTableCellInfoNativeLegacy {
    public JOBJECT32 accessibleContext;
    public int index;
    public int row;
    public int column;
    public int rowExtent;
    public int columnExtent;
    public byte isSelected;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTableInfoNativeLegacy {
    public JOBJECT32 caption;
    public JOBJECT32 summary;
    public int rowCount;
    public int columnCount;
    public JOBJECT32 accessibleContext;
    public JOBJECT32 accessibleTable;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTextAttributesInfoNativeLegacy {
    public int bold;
    public int italic;
    public int underline;
    public int strikethrough;
    public int superscript;
    public int subscript;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string backgroundColor;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string foregroundColor;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string fontFamily;
    public int fontSize;
    public int alignment;
    public int bidiLevel;
    public float firstLineIndent;
    public float leftIndent;
    public float rightIndent;
    public float lineSpacing;
    public float spaceAbove;
    public float spaceBelow;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
    public string fullAttributesString;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTextInfoNativeLegacy {
    public int charCount;
    public int caretIndex;
    public int indexAtPoint;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTextItemsInfoNativeLegacy {
    public char letter;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string word;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
    public string sentence;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTextRectInfoNativeLegacy {
    public int x;
    public int y;
    public int width;
    public int height;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTextSelectionInfoNativeLegacy {
    public int selectionStartIndex;
    public int selectionEndIndex;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
    public string selectedText;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct VisibleChildrenInfoNativeLegacy {
    public int returnedChildrenCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public JOBJECT32[] children;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public class AccessibleActionsNativeLegacy {
    public int actionsCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public AccessibleActionInfoNativeLegacy[] actionInfo;
  }

}
