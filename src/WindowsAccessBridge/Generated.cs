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
using System;
using System.Runtime.InteropServices;
using System.Text;
using WindowHandle = System.IntPtr;
using BOOL = System.Int32;

namespace AccessBridgeExplorer.WindowsAccessBridge {
  /// <summary>
  /// Platform agnostic abstraction over WindowAccessBridge DLL entry points
  /// </summary>
  public interface IAccessBridgeFunctions {
    void Windows_run();
    bool IsJavaWindow(WindowHandle window);
    bool IsSameObject(int vmID, JavaObjectHandle obj1, JavaObjectHandle obj2);
    bool GetAccessibleContextFromHWND(WindowHandle window, out int vmID, out JavaObjectHandle ac);
    WindowHandle GetHWNDFromAccessibleContext(int vmID, JavaObjectHandle ac);
    bool GetAccessibleContextAt(int vmID, JavaObjectHandle acParent, int x, int y, out JavaObjectHandle ac);
    bool GetAccessibleContextWithFocus(WindowHandle window, out int vmID, out JavaObjectHandle ac);
    bool GetAccessibleContextInfo(int vmID, JavaObjectHandle ac, out AccessibleContextInfo info);
    JavaObjectHandle GetAccessibleChildFromContext(int vmID, JavaObjectHandle ac, int i);
    JavaObjectHandle GetAccessibleParentFromContext(int vmID, JavaObjectHandle ac);
    bool GetAccessibleRelationSet(int vmID, JavaObjectHandle accessibleContext, out AccessibleRelationSetInfo relationSetInfo);
    bool GetAccessibleHypertext(int vmID, JavaObjectHandle accessibleContext, out AccessibleHypertextInfo hypertextInfo);
    bool ActivateAccessibleHyperlink(int vmID, JavaObjectHandle accessibleContext, JavaObjectHandle accessibleHyperlink);
    int GetAccessibleHyperlinkCount(int vmID, JavaObjectHandle accessibleContext);
    bool GetAccessibleHypertextExt(int vmID, JavaObjectHandle accessibleContext, int nStartIndex, out AccessibleHypertextInfo hypertextInfo);
    int GetAccessibleHypertextLinkIndex(int vmID, JavaObjectHandle hypertext, int nIndex);
    bool GetAccessibleHyperlink(int vmID, JavaObjectHandle hypertext, int nIndex, out AccessibleHyperlinkInfo hyperlinkInfo);
    bool GetAccessibleKeyBindings(int vmID, JavaObjectHandle accessibleContext, out AccessibleKeyBindings keyBindings);
    bool GetAccessibleIcons(int vmID, JavaObjectHandle accessibleContext, out AccessibleIcons icons);
    bool GetAccessibleActions(int vmID, JavaObjectHandle accessibleContext, [Out]AccessibleActions actions);
    bool DoAccessibleActions(int vmID, JavaObjectHandle accessibleContext, ref AccessibleActionsToDo actionsToDo, out int failure);
    bool GetAccessibleTextInfo(int vmID, JavaObjectHandle at, out AccessibleTextInfo textInfo, int x, int y);
    bool GetAccessibleTextItems(int vmID, JavaObjectHandle at, out AccessibleTextItemsInfo textItems, int index);
    bool GetAccessibleTextSelectionInfo(int vmID, JavaObjectHandle at, out AccessibleTextSelectionInfo textSelection);
    bool GetAccessibleTextAttributes(int vmID, JavaObjectHandle at, int index, out AccessibleTextAttributesInfo attributes);
    bool GetAccessibleTextRect(int vmID, JavaObjectHandle at, out AccessibleTextRectInfo rectInfo, int index);
    bool GetAccessibleTextLineBounds(int vmID, JavaObjectHandle at, int index, out int startIndex, out int endIndex);
    bool GetAccessibleTextRange(int vmID, JavaObjectHandle at, int start, int end, StringBuilder text, short len);
    bool GetCurrentAccessibleValueFromContext(int vmID, JavaObjectHandle av, StringBuilder value, short len);
    bool GetMaximumAccessibleValueFromContext(int vmID, JavaObjectHandle av, StringBuilder value, short len);
    bool GetMinimumAccessibleValueFromContext(int vmID, JavaObjectHandle av, StringBuilder value, short len);
    void AddAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel, int i);
    void ClearAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel);
    JavaObjectHandle GetAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel, int i);
    int GetAccessibleSelectionCountFromContext(int vmID, JavaObjectHandle asel);
    bool IsAccessibleChildSelectedFromContext(int vmID, JavaObjectHandle asel, int i);
    void RemoveAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel, int i);
    void SelectAllAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel);
    bool GetAccessibleTableInfo(int vmID, JavaObjectHandle ac, out AccessibleTableInfo tableInfo);
    bool GetAccessibleTableCellInfo(int vmID, JavaObjectHandle accessibleTable, int row, int column, out AccessibleTableCellInfo tableCellInfo);
    bool GetAccessibleTableRowHeader(int vmID, JavaObjectHandle acParent, out AccessibleTableInfo tableInfo);
    bool GetAccessibleTableColumnHeader(int vmID, JavaObjectHandle acParent, out AccessibleTableInfo tableInfo);
    JavaObjectHandle GetAccessibleTableRowDescription(int vmID, JavaObjectHandle acParent, int row);
    JavaObjectHandle GetAccessibleTableColumnDescription(int vmID, JavaObjectHandle acParent, int column);
    int GetAccessibleTableRowSelectionCount(int vmID, JavaObjectHandle table);
    bool IsAccessibleTableRowSelected(int vmID, JavaObjectHandle table, int row);
    bool GetAccessibleTableRowSelections(int vmID, JavaObjectHandle table, int count, [Out]int[] selections);
    int GetAccessibleTableColumnSelectionCount(int vmID, JavaObjectHandle table);
    bool IsAccessibleTableColumnSelected(int vmID, JavaObjectHandle table, int column);
    bool GetAccessibleTableColumnSelections(int vmID, JavaObjectHandle table, int count, [Out]int[] selections);
    int GetAccessibleTableRow(int vmID, JavaObjectHandle table, int index);
    int GetAccessibleTableColumn(int vmID, JavaObjectHandle table, int index);
    int GetAccessibleTableIndex(int vmID, JavaObjectHandle table, int row, int column);
    bool SetTextContents(int vmID, JavaObjectHandle ac, string text);
    JavaObjectHandle GetParentWithRole(int vmID, JavaObjectHandle ac, string role);
    JavaObjectHandle GetParentWithRoleElseRoot(int vmID, JavaObjectHandle ac, string role);
    JavaObjectHandle GetTopLevelObject(int vmID, JavaObjectHandle ac);
    int GetObjectDepth(int vmID, JavaObjectHandle ac);
    JavaObjectHandle GetActiveDescendent(int vmID, JavaObjectHandle ac);
    bool GetVirtualAccessibleName(int vmID, JavaObjectHandle ac, StringBuilder name, int len);
    bool GetTextAttributesInRange(int vmID, JavaObjectHandle accessibleContext, int startIndex, int endIndex, out AccessibleTextAttributesInfo attributes, out short len);
    bool GetCaretLocation(int vmID, JavaObjectHandle ac, out AccessibleTextRectInfo rectInfo, int index);
    int GetVisibleChildrenCount(int vmID, JavaObjectHandle accessibleContext);
    bool GetVisibleChildren(int vmID, JavaObjectHandle accessibleContext, int startIndex, out VisibleChildrenInfo children);
    bool GetVersionInfo(int vmID, out AccessBridgeVersionInfo info);
  }

  /// <summary>
  /// Platform agnostic abstraction over WindowAccessBridge DLL entry points
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

  /// <summary>
  /// Platform agnostic abstraction over WindowAccessBridge DLL entry points
  /// </summary>
  public partial class AccessBridgeFunctions: IAccessBridgeFunctions {
    public void Windows_run() {
      LibraryFunctions.Windows_run();
    }

    public bool IsJavaWindow(WindowHandle window) {
      var result = LibraryFunctions.IsJavaWindow(window);
      return ToBool(result);
    }

    public bool IsSameObject(int vmID, JavaObjectHandle obj1, JavaObjectHandle obj2) {
      var result = LibraryFunctions.IsSameObject(vmID, Unwrap(obj1), Unwrap(obj2));
      return ToBool(result);
    }

    public bool GetAccessibleContextFromHWND(WindowHandle window, out int vmID, out JavaObjectHandle ac) {
      JOBJECT64 acTemp;
      var result = LibraryFunctions.GetAccessibleContextFromHWND(window, out vmID, out acTemp);
      ac = Wrap(vmID, acTemp);
      return ToBool(result);
    }

    public WindowHandle GetHWNDFromAccessibleContext(int vmID, JavaObjectHandle ac) {
      var result = LibraryFunctions.GetHWNDFromAccessibleContext(vmID, Unwrap(ac));
      return result;
    }

    public bool GetAccessibleContextAt(int vmID, JavaObjectHandle acParent, int x, int y, out JavaObjectHandle ac) {
      JOBJECT64 acTemp;
      var result = LibraryFunctions.GetAccessibleContextAt(vmID, Unwrap(acParent), x, y, out acTemp);
      ac = Wrap(vmID, acTemp);
      return ToBool(result);
    }

    public bool GetAccessibleContextWithFocus(WindowHandle window, out int vmID, out JavaObjectHandle ac) {
      JOBJECT64 acTemp;
      var result = LibraryFunctions.GetAccessibleContextWithFocus(window, out vmID, out acTemp);
      ac = Wrap(vmID, acTemp);
      return ToBool(result);
    }

    public bool GetAccessibleContextInfo(int vmID, JavaObjectHandle ac, out AccessibleContextInfo info) {
      var result = LibraryFunctions.GetAccessibleContextInfo(vmID, Unwrap(ac), out info);
      return ToBool(result);
    }

    public JavaObjectHandle GetAccessibleChildFromContext(int vmID, JavaObjectHandle ac, int i) {
      var result = LibraryFunctions.GetAccessibleChildFromContext(vmID, Unwrap(ac), i);
      return Wrap(vmID, result);
    }

    public JavaObjectHandle GetAccessibleParentFromContext(int vmID, JavaObjectHandle ac) {
      var result = LibraryFunctions.GetAccessibleParentFromContext(vmID, Unwrap(ac));
      return Wrap(vmID, result);
    }

    public bool GetAccessibleRelationSet(int vmID, JavaObjectHandle accessibleContext, out AccessibleRelationSetInfo relationSetInfo) {
      var result = LibraryFunctions.GetAccessibleRelationSet(vmID, Unwrap(accessibleContext), out relationSetInfo);
      return ToBool(result);
    }

    public bool GetAccessibleHypertext(int vmID, JavaObjectHandle accessibleContext, out AccessibleHypertextInfo hypertextInfo) {
      var result = LibraryFunctions.GetAccessibleHypertext(vmID, Unwrap(accessibleContext), out hypertextInfo);
      return ToBool(result);
    }

    public bool ActivateAccessibleHyperlink(int vmID, JavaObjectHandle accessibleContext, JavaObjectHandle accessibleHyperlink) {
      var result = LibraryFunctions.ActivateAccessibleHyperlink(vmID, Unwrap(accessibleContext), Unwrap(accessibleHyperlink));
      return ToBool(result);
    }

    public int GetAccessibleHyperlinkCount(int vmID, JavaObjectHandle accessibleContext) {
      var result = LibraryFunctions.GetAccessibleHyperlinkCount(vmID, Unwrap(accessibleContext));
      return result;
    }

    public bool GetAccessibleHypertextExt(int vmID, JavaObjectHandle accessibleContext, int nStartIndex, out AccessibleHypertextInfo hypertextInfo) {
      var result = LibraryFunctions.GetAccessibleHypertextExt(vmID, Unwrap(accessibleContext), nStartIndex, out hypertextInfo);
      return ToBool(result);
    }

    public int GetAccessibleHypertextLinkIndex(int vmID, JavaObjectHandle hypertext, int nIndex) {
      var result = LibraryFunctions.GetAccessibleHypertextLinkIndex(vmID, Unwrap(hypertext), nIndex);
      return result;
    }

    public bool GetAccessibleHyperlink(int vmID, JavaObjectHandle hypertext, int nIndex, out AccessibleHyperlinkInfo hyperlinkInfo) {
      var result = LibraryFunctions.GetAccessibleHyperlink(vmID, Unwrap(hypertext), nIndex, out hyperlinkInfo);
      return ToBool(result);
    }

    public bool GetAccessibleKeyBindings(int vmID, JavaObjectHandle accessibleContext, out AccessibleKeyBindings keyBindings) {
      var result = LibraryFunctions.GetAccessibleKeyBindings(vmID, Unwrap(accessibleContext), out keyBindings);
      return ToBool(result);
    }

    public bool GetAccessibleIcons(int vmID, JavaObjectHandle accessibleContext, out AccessibleIcons icons) {
      var result = LibraryFunctions.GetAccessibleIcons(vmID, Unwrap(accessibleContext), out icons);
      return ToBool(result);
    }

    public bool GetAccessibleActions(int vmID, JavaObjectHandle accessibleContext, [Out]AccessibleActions actions) {
      var result = LibraryFunctions.GetAccessibleActions(vmID, Unwrap(accessibleContext), actions);
      return ToBool(result);
    }

    public bool DoAccessibleActions(int vmID, JavaObjectHandle accessibleContext, ref AccessibleActionsToDo actionsToDo, out int failure) {
      var result = LibraryFunctions.DoAccessibleActions(vmID, Unwrap(accessibleContext), ref actionsToDo, out failure);
      return ToBool(result);
    }

    public bool GetAccessibleTextInfo(int vmID, JavaObjectHandle at, out AccessibleTextInfo textInfo, int x, int y) {
      var result = LibraryFunctions.GetAccessibleTextInfo(vmID, Unwrap(at), out textInfo, x, y);
      return ToBool(result);
    }

    public bool GetAccessibleTextItems(int vmID, JavaObjectHandle at, out AccessibleTextItemsInfo textItems, int index) {
      var result = LibraryFunctions.GetAccessibleTextItems(vmID, Unwrap(at), out textItems, index);
      return ToBool(result);
    }

    public bool GetAccessibleTextSelectionInfo(int vmID, JavaObjectHandle at, out AccessibleTextSelectionInfo textSelection) {
      var result = LibraryFunctions.GetAccessibleTextSelectionInfo(vmID, Unwrap(at), out textSelection);
      return ToBool(result);
    }

    public bool GetAccessibleTextAttributes(int vmID, JavaObjectHandle at, int index, out AccessibleTextAttributesInfo attributes) {
      var result = LibraryFunctions.GetAccessibleTextAttributes(vmID, Unwrap(at), index, out attributes);
      return ToBool(result);
    }

    public bool GetAccessibleTextRect(int vmID, JavaObjectHandle at, out AccessibleTextRectInfo rectInfo, int index) {
      var result = LibraryFunctions.GetAccessibleTextRect(vmID, Unwrap(at), out rectInfo, index);
      return ToBool(result);
    }

    public bool GetAccessibleTextLineBounds(int vmID, JavaObjectHandle at, int index, out int startIndex, out int endIndex) {
      var result = LibraryFunctions.GetAccessibleTextLineBounds(vmID, Unwrap(at), index, out startIndex, out endIndex);
      return ToBool(result);
    }

    public bool GetAccessibleTextRange(int vmID, JavaObjectHandle at, int start, int end, StringBuilder text, short len) {
      var result = LibraryFunctions.GetAccessibleTextRange(vmID, Unwrap(at), start, end, text, len);
      return ToBool(result);
    }

    public bool GetCurrentAccessibleValueFromContext(int vmID, JavaObjectHandle av, StringBuilder value, short len) {
      var result = LibraryFunctions.GetCurrentAccessibleValueFromContext(vmID, Unwrap(av), value, len);
      return ToBool(result);
    }

    public bool GetMaximumAccessibleValueFromContext(int vmID, JavaObjectHandle av, StringBuilder value, short len) {
      var result = LibraryFunctions.GetMaximumAccessibleValueFromContext(vmID, Unwrap(av), value, len);
      return ToBool(result);
    }

    public bool GetMinimumAccessibleValueFromContext(int vmID, JavaObjectHandle av, StringBuilder value, short len) {
      var result = LibraryFunctions.GetMinimumAccessibleValueFromContext(vmID, Unwrap(av), value, len);
      return ToBool(result);
    }

    public void AddAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel, int i) {
      LibraryFunctions.AddAccessibleSelectionFromContext(vmID, Unwrap(asel), i);
    }

    public void ClearAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel) {
      LibraryFunctions.ClearAccessibleSelectionFromContext(vmID, Unwrap(asel));
    }

    public JavaObjectHandle GetAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel, int i) {
      var result = LibraryFunctions.GetAccessibleSelectionFromContext(vmID, Unwrap(asel), i);
      return Wrap(vmID, result);
    }

    public int GetAccessibleSelectionCountFromContext(int vmID, JavaObjectHandle asel) {
      var result = LibraryFunctions.GetAccessibleSelectionCountFromContext(vmID, Unwrap(asel));
      return result;
    }

    public bool IsAccessibleChildSelectedFromContext(int vmID, JavaObjectHandle asel, int i) {
      var result = LibraryFunctions.IsAccessibleChildSelectedFromContext(vmID, Unwrap(asel), i);
      return ToBool(result);
    }

    public void RemoveAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel, int i) {
      LibraryFunctions.RemoveAccessibleSelectionFromContext(vmID, Unwrap(asel), i);
    }

    public void SelectAllAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel) {
      LibraryFunctions.SelectAllAccessibleSelectionFromContext(vmID, Unwrap(asel));
    }

    public bool GetAccessibleTableInfo(int vmID, JavaObjectHandle ac, out AccessibleTableInfo tableInfo) {
      var result = LibraryFunctions.GetAccessibleTableInfo(vmID, Unwrap(ac), out tableInfo);
      return ToBool(result);
    }

    public bool GetAccessibleTableCellInfo(int vmID, JavaObjectHandle accessibleTable, int row, int column, out AccessibleTableCellInfo tableCellInfo) {
      var result = LibraryFunctions.GetAccessibleTableCellInfo(vmID, Unwrap(accessibleTable), row, column, out tableCellInfo);
      return ToBool(result);
    }

    public bool GetAccessibleTableRowHeader(int vmID, JavaObjectHandle acParent, out AccessibleTableInfo tableInfo) {
      var result = LibraryFunctions.GetAccessibleTableRowHeader(vmID, Unwrap(acParent), out tableInfo);
      return ToBool(result);
    }

    public bool GetAccessibleTableColumnHeader(int vmID, JavaObjectHandle acParent, out AccessibleTableInfo tableInfo) {
      var result = LibraryFunctions.GetAccessibleTableColumnHeader(vmID, Unwrap(acParent), out tableInfo);
      return ToBool(result);
    }

    public JavaObjectHandle GetAccessibleTableRowDescription(int vmID, JavaObjectHandle acParent, int row) {
      var result = LibraryFunctions.GetAccessibleTableRowDescription(vmID, Unwrap(acParent), row);
      return Wrap(vmID, result);
    }

    public JavaObjectHandle GetAccessibleTableColumnDescription(int vmID, JavaObjectHandle acParent, int column) {
      var result = LibraryFunctions.GetAccessibleTableColumnDescription(vmID, Unwrap(acParent), column);
      return Wrap(vmID, result);
    }

    public int GetAccessibleTableRowSelectionCount(int vmID, JavaObjectHandle table) {
      var result = LibraryFunctions.GetAccessibleTableRowSelectionCount(vmID, Unwrap(table));
      return result;
    }

    public bool IsAccessibleTableRowSelected(int vmID, JavaObjectHandle table, int row) {
      var result = LibraryFunctions.IsAccessibleTableRowSelected(vmID, Unwrap(table), row);
      return ToBool(result);
    }

    public bool GetAccessibleTableRowSelections(int vmID, JavaObjectHandle table, int count, [Out]int[] selections) {
      var result = LibraryFunctions.GetAccessibleTableRowSelections(vmID, Unwrap(table), count, selections);
      return ToBool(result);
    }

    public int GetAccessibleTableColumnSelectionCount(int vmID, JavaObjectHandle table) {
      var result = LibraryFunctions.GetAccessibleTableColumnSelectionCount(vmID, Unwrap(table));
      return result;
    }

    public bool IsAccessibleTableColumnSelected(int vmID, JavaObjectHandle table, int column) {
      var result = LibraryFunctions.IsAccessibleTableColumnSelected(vmID, Unwrap(table), column);
      return ToBool(result);
    }

    public bool GetAccessibleTableColumnSelections(int vmID, JavaObjectHandle table, int count, [Out]int[] selections) {
      var result = LibraryFunctions.GetAccessibleTableColumnSelections(vmID, Unwrap(table), count, selections);
      return ToBool(result);
    }

    public int GetAccessibleTableRow(int vmID, JavaObjectHandle table, int index) {
      var result = LibraryFunctions.GetAccessibleTableRow(vmID, Unwrap(table), index);
      return result;
    }

    public int GetAccessibleTableColumn(int vmID, JavaObjectHandle table, int index) {
      var result = LibraryFunctions.GetAccessibleTableColumn(vmID, Unwrap(table), index);
      return result;
    }

    public int GetAccessibleTableIndex(int vmID, JavaObjectHandle table, int row, int column) {
      var result = LibraryFunctions.GetAccessibleTableIndex(vmID, Unwrap(table), row, column);
      return result;
    }

    public bool SetTextContents(int vmID, JavaObjectHandle ac, string text) {
      var result = LibraryFunctions.SetTextContents(vmID, Unwrap(ac), text);
      return ToBool(result);
    }

    public JavaObjectHandle GetParentWithRole(int vmID, JavaObjectHandle ac, string role) {
      var result = LibraryFunctions.GetParentWithRole(vmID, Unwrap(ac), role);
      return Wrap(vmID, result);
    }

    public JavaObjectHandle GetParentWithRoleElseRoot(int vmID, JavaObjectHandle ac, string role) {
      var result = LibraryFunctions.GetParentWithRoleElseRoot(vmID, Unwrap(ac), role);
      return Wrap(vmID, result);
    }

    public JavaObjectHandle GetTopLevelObject(int vmID, JavaObjectHandle ac) {
      var result = LibraryFunctions.GetTopLevelObject(vmID, Unwrap(ac));
      return Wrap(vmID, result);
    }

    public int GetObjectDepth(int vmID, JavaObjectHandle ac) {
      var result = LibraryFunctions.GetObjectDepth(vmID, Unwrap(ac));
      return result;
    }

    public JavaObjectHandle GetActiveDescendent(int vmID, JavaObjectHandle ac) {
      var result = LibraryFunctions.GetActiveDescendent(vmID, Unwrap(ac));
      return Wrap(vmID, result);
    }

    public bool GetVirtualAccessibleName(int vmID, JavaObjectHandle ac, StringBuilder name, int len) {
      var result = LibraryFunctions.GetVirtualAccessibleName(vmID, Unwrap(ac), name, len);
      return ToBool(result);
    }

    public bool GetTextAttributesInRange(int vmID, JavaObjectHandle accessibleContext, int startIndex, int endIndex, out AccessibleTextAttributesInfo attributes, out short len) {
      var result = LibraryFunctions.GetTextAttributesInRange(vmID, Unwrap(accessibleContext), startIndex, endIndex, out attributes, out len);
      return ToBool(result);
    }

    public bool GetCaretLocation(int vmID, JavaObjectHandle ac, out AccessibleTextRectInfo rectInfo, int index) {
      var result = LibraryFunctions.GetCaretLocation(vmID, Unwrap(ac), out rectInfo, index);
      return ToBool(result);
    }

    public int GetVisibleChildrenCount(int vmID, JavaObjectHandle accessibleContext) {
      var result = LibraryFunctions.GetVisibleChildrenCount(vmID, Unwrap(accessibleContext));
      return result;
    }

    public bool GetVisibleChildren(int vmID, JavaObjectHandle accessibleContext, int startIndex, out VisibleChildrenInfo children) {
      var result = LibraryFunctions.GetVisibleChildren(vmID, Unwrap(accessibleContext), startIndex, out children);
      return ToBool(result);
    }

    public bool GetVersionInfo(int vmID, out AccessBridgeVersionInfo info) {
      var result = LibraryFunctions.GetVersionInfo(vmID, out info);
      return ToBool(result);
    }

  }

  /// <summary>
  /// Acess Bridge event handlers implementation
  /// </summary>
  public partial class AccessBridgeEvents : IAccessBridgeEvents {
    #region Event functions
    public event PropertyChangeEventHandler PropertyChange;
    public event JavaShutdownEventHandler JavaShutdown;
    public event FocusGainedEventHandler FocusGained;
    public event FocusLostEventHandler FocusLost;
    public event CaretUpdateEventHandler CaretUpdate;
    public event MouseClickedEventHandler MouseClicked;
    public event MouseEnteredEventHandler MouseEntered;
    public event MouseExitedEventHandler MouseExited;
    public event MousePressedEventHandler MousePressed;
    public event MouseReleasedEventHandler MouseReleased;
    public event MenuCanceledEventHandler MenuCanceled;
    public event MenuDeselectedEventHandler MenuDeselected;
    public event MenuSelectedEventHandler MenuSelected;
    public event PopupMenuCanceledEventHandler PopupMenuCanceled;
    public event PopupMenuWillBecomeInvisibleEventHandler PopupMenuWillBecomeInvisible;
    public event PopupMenuWillBecomeVisibleEventHandler PopupMenuWillBecomeVisible;
    public event PropertyNameChangeEventHandler PropertyNameChange;
    public event PropertyDescriptionChangeEventHandler PropertyDescriptionChange;
    public event PropertyStateChangeEventHandler PropertyStateChange;
    public event PropertyValueChangeEventHandler PropertyValueChange;
    public event PropertySelectionChangeEventHandler PropertySelectionChange;
    public event PropertyTextChangeEventHandler PropertyTextChange;
    public event PropertyCaretChangeEventHandler PropertyCaretChange;
    public event PropertyVisibleDataChangeEventHandler PropertyVisibleDataChange;
    public event PropertyChildChangeEventHandler PropertyChildChange;
    public event PropertyActiveDescendentChangeEventHandler PropertyActiveDescendentChange;
    public event PropertyTableModelChangeEventHandler PropertyTableModelChange;
    #endregion

    #region Event handlers
    protected virtual void OnPropertyChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string property, string oldValue, string newValue) {
      var handler = PropertyChange;
      if (handler != null)
        handler(vmid, evt, source, property, oldValue, newValue);
    }
    protected virtual void OnJavaShutdown(int vmid) {
      var handler = JavaShutdown;
      if (handler != null)
        handler(vmid);
    }
    protected virtual void OnFocusGained(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = FocusGained;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnFocusLost(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = FocusLost;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnCaretUpdate(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = CaretUpdate;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseClicked(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = MouseClicked;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseEntered(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = MouseEntered;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseExited(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = MouseExited;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMousePressed(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = MousePressed;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseReleased(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = MouseReleased;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMenuCanceled(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = MenuCanceled;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMenuDeselected(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = MenuDeselected;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMenuSelected(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = MenuSelected;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPopupMenuCanceled(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = PopupMenuCanceled;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPopupMenuWillBecomeInvisible(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = PopupMenuWillBecomeInvisible;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPopupMenuWillBecomeVisible(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = PopupMenuWillBecomeVisible;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyNameChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string oldName, string newName) {
      var handler = PropertyNameChange;
      if (handler != null)
        handler(vmid, evt, source, oldName, newName);
    }
    protected virtual void OnPropertyDescriptionChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string oldDescription, string newDescription) {
      var handler = PropertyDescriptionChange;
      if (handler != null)
        handler(vmid, evt, source, oldDescription, newDescription);
    }
    protected virtual void OnPropertyStateChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string oldState, string newState) {
      var handler = PropertyStateChange;
      if (handler != null)
        handler(vmid, evt, source, oldState, newState);
    }
    protected virtual void OnPropertyValueChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string oldValue, string newValue) {
      var handler = PropertyValueChange;
      if (handler != null)
        handler(vmid, evt, source, oldValue, newValue);
    }
    protected virtual void OnPropertySelectionChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = PropertySelectionChange;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyTextChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = PropertyTextChange;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyCaretChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, int oldPosition, int newPosition) {
      var handler = PropertyCaretChange;
      if (handler != null)
        handler(vmid, evt, source, oldPosition, newPosition);
    }
    protected virtual void OnPropertyVisibleDataChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = PropertyVisibleDataChange;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyChildChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, JavaObjectHandle oldChild, JavaObjectHandle newChild) {
      var handler = PropertyChildChange;
      if (handler != null)
        handler(vmid, evt, source, oldChild, newChild);
    }
    protected virtual void OnPropertyActiveDescendentChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, JavaObjectHandle oldActiveDescendent, JavaObjectHandle newActiveDescendent) {
      var handler = PropertyActiveDescendentChange;
      if (handler != null)
        handler(vmid, evt, source, oldActiveDescendent, newActiveDescendent);
    }
    protected virtual void OnPropertyTableModelChange(int vmid, JavaObjectHandle evt, JavaObjectHandle src, string oldValue, string newValue) {
      var handler = PropertyTableModelChange;
      if (handler != null)
        handler(vmid, evt, src, oldValue, newValue);
    }
    #endregion

    private void AttachForwarders(AccessBridgeEventsNative nativeEvents) {
      nativeEvents.PropertyChange += ForwardPropertyChange;
      nativeEvents.JavaShutdown += ForwardJavaShutdown;
      nativeEvents.FocusGained += ForwardFocusGained;
      nativeEvents.FocusLost += ForwardFocusLost;
      nativeEvents.CaretUpdate += ForwardCaretUpdate;
      nativeEvents.MouseClicked += ForwardMouseClicked;
      nativeEvents.MouseEntered += ForwardMouseEntered;
      nativeEvents.MouseExited += ForwardMouseExited;
      nativeEvents.MousePressed += ForwardMousePressed;
      nativeEvents.MouseReleased += ForwardMouseReleased;
      nativeEvents.MenuCanceled += ForwardMenuCanceled;
      nativeEvents.MenuDeselected += ForwardMenuDeselected;
      nativeEvents.MenuSelected += ForwardMenuSelected;
      nativeEvents.PopupMenuCanceled += ForwardPopupMenuCanceled;
      nativeEvents.PopupMenuWillBecomeInvisible += ForwardPopupMenuWillBecomeInvisible;
      nativeEvents.PopupMenuWillBecomeVisible += ForwardPopupMenuWillBecomeVisible;
      nativeEvents.PropertyNameChange += ForwardPropertyNameChange;
      nativeEvents.PropertyDescriptionChange += ForwardPropertyDescriptionChange;
      nativeEvents.PropertyStateChange += ForwardPropertyStateChange;
      nativeEvents.PropertyValueChange += ForwardPropertyValueChange;
      nativeEvents.PropertySelectionChange += ForwardPropertySelectionChange;
      nativeEvents.PropertyTextChange += ForwardPropertyTextChange;
      nativeEvents.PropertyCaretChange += ForwardPropertyCaretChange;
      nativeEvents.PropertyVisibleDataChange += ForwardPropertyVisibleDataChange;
      nativeEvents.PropertyChildChange += ForwardPropertyChildChange;
      nativeEvents.PropertyActiveDescendentChange += ForwardPropertyActiveDescendentChange;
      nativeEvents.PropertyTableModelChange += ForwardPropertyTableModelChange;
    }

    private void DetachForwarders(AccessBridgeEventsNative nativeEvents) {
      nativeEvents.PropertyChange -= ForwardPropertyChange;
      nativeEvents.JavaShutdown -= ForwardJavaShutdown;
      nativeEvents.FocusGained -= ForwardFocusGained;
      nativeEvents.FocusLost -= ForwardFocusLost;
      nativeEvents.CaretUpdate -= ForwardCaretUpdate;
      nativeEvents.MouseClicked -= ForwardMouseClicked;
      nativeEvents.MouseEntered -= ForwardMouseEntered;
      nativeEvents.MouseExited -= ForwardMouseExited;
      nativeEvents.MousePressed -= ForwardMousePressed;
      nativeEvents.MouseReleased -= ForwardMouseReleased;
      nativeEvents.MenuCanceled -= ForwardMenuCanceled;
      nativeEvents.MenuDeselected -= ForwardMenuDeselected;
      nativeEvents.MenuSelected -= ForwardMenuSelected;
      nativeEvents.PopupMenuCanceled -= ForwardPopupMenuCanceled;
      nativeEvents.PopupMenuWillBecomeInvisible -= ForwardPopupMenuWillBecomeInvisible;
      nativeEvents.PopupMenuWillBecomeVisible -= ForwardPopupMenuWillBecomeVisible;
      nativeEvents.PropertyNameChange -= ForwardPropertyNameChange;
      nativeEvents.PropertyDescriptionChange -= ForwardPropertyDescriptionChange;
      nativeEvents.PropertyStateChange -= ForwardPropertyStateChange;
      nativeEvents.PropertyValueChange -= ForwardPropertyValueChange;
      nativeEvents.PropertySelectionChange -= ForwardPropertySelectionChange;
      nativeEvents.PropertyTextChange -= ForwardPropertyTextChange;
      nativeEvents.PropertyCaretChange -= ForwardPropertyCaretChange;
      nativeEvents.PropertyVisibleDataChange -= ForwardPropertyVisibleDataChange;
      nativeEvents.PropertyChildChange -= ForwardPropertyChildChange;
      nativeEvents.PropertyActiveDescendentChange -= ForwardPropertyActiveDescendentChange;
      nativeEvents.PropertyTableModelChange -= ForwardPropertyTableModelChange;
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
    #region Function delegate types
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void Windows_runFP();
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL IsJavaWindowFP(WindowHandle window);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL IsSameObjectFP(int vmID, JOBJECT64 obj1, JOBJECT64 obj2);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleContextFromHWNDFP(WindowHandle window, out int vmID, out JOBJECT64 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate WindowHandle GetHWNDFromAccessibleContextFP(int vmID, JOBJECT64 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleContextAtFP(int vmID, JOBJECT64 acParent, int x, int y, out JOBJECT64 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleContextWithFocusFP(WindowHandle window, out int vmID, out JOBJECT64 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleContextInfoFP(int vmID, JOBJECT64 ac, out AccessibleContextInfo info);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT64 GetAccessibleChildFromContextFP(int vmID, JOBJECT64 ac, int i);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT64 GetAccessibleParentFromContextFP(int vmID, JOBJECT64 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleRelationSetFP(int vmID, JOBJECT64 accessibleContext, out AccessibleRelationSetInfo relationSetInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleHypertextFP(int vmID, JOBJECT64 accessibleContext, out AccessibleHypertextInfo hypertextInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL ActivateAccessibleHyperlinkFP(int vmID, JOBJECT64 accessibleContext, JOBJECT64 accessibleHyperlink);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleHyperlinkCountFP(int vmID, JOBJECT64 accessibleContext);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleHypertextExtFP(int vmID, JOBJECT64 accessibleContext, int nStartIndex, out AccessibleHypertextInfo hypertextInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleHypertextLinkIndexFP(int vmID, JOBJECT64 hypertext, int nIndex);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleHyperlinkFP(int vmID, JOBJECT64 hypertext, int nIndex, out AccessibleHyperlinkInfo hyperlinkInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleKeyBindingsFP(int vmID, JOBJECT64 accessibleContext, out AccessibleKeyBindings keyBindings);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleIconsFP(int vmID, JOBJECT64 accessibleContext, out AccessibleIcons icons);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleActionsFP(int vmID, JOBJECT64 accessibleContext, [Out]AccessibleActions actions);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL DoAccessibleActionsFP(int vmID, JOBJECT64 accessibleContext, ref AccessibleActionsToDo actionsToDo, out int failure);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextInfoFP(int vmID, JOBJECT64 at, out AccessibleTextInfo textInfo, int x, int y);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextItemsFP(int vmID, JOBJECT64 at, out AccessibleTextItemsInfo textItems, int index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextSelectionInfoFP(int vmID, JOBJECT64 at, out AccessibleTextSelectionInfo textSelection);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextAttributesFP(int vmID, JOBJECT64 at, int index, out AccessibleTextAttributesInfo attributes);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextRectFP(int vmID, JOBJECT64 at, out AccessibleTextRectInfo rectInfo, int index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextLineBoundsFP(int vmID, JOBJECT64 at, int index, out int startIndex, out int endIndex);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextRangeFP(int vmID, JOBJECT64 at, int start, int end, StringBuilder text, short len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetCurrentAccessibleValueFromContextFP(int vmID, JOBJECT64 av, StringBuilder value, short len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetMaximumAccessibleValueFromContextFP(int vmID, JOBJECT64 av, StringBuilder value, short len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetMinimumAccessibleValueFromContextFP(int vmID, JOBJECT64 av, StringBuilder value, short len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void AddAccessibleSelectionFromContextFP(int vmID, JOBJECT64 asel, int i);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void ClearAccessibleSelectionFromContextFP(int vmID, JOBJECT64 asel);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT64 GetAccessibleSelectionFromContextFP(int vmID, JOBJECT64 asel, int i);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleSelectionCountFromContextFP(int vmID, JOBJECT64 asel);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL IsAccessibleChildSelectedFromContextFP(int vmID, JOBJECT64 asel, int i);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void RemoveAccessibleSelectionFromContextFP(int vmID, JOBJECT64 asel, int i);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate void SelectAllAccessibleSelectionFromContextFP(int vmID, JOBJECT64 asel);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableInfoFP(int vmID, JOBJECT64 ac, out AccessibleTableInfo tableInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableCellInfoFP(int vmID, JOBJECT64 accessibleTable, int row, int column, out AccessibleTableCellInfo tableCellInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableRowHeaderFP(int vmID, JOBJECT64 acParent, out AccessibleTableInfo tableInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableColumnHeaderFP(int vmID, JOBJECT64 acParent, out AccessibleTableInfo tableInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT64 GetAccessibleTableRowDescriptionFP(int vmID, JOBJECT64 acParent, int row);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT64 GetAccessibleTableColumnDescriptionFP(int vmID, JOBJECT64 acParent, int column);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleTableRowSelectionCountFP(int vmID, JOBJECT64 table);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL IsAccessibleTableRowSelectedFP(int vmID, JOBJECT64 table, int row);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableRowSelectionsFP(int vmID, JOBJECT64 table, int count, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]int[] selections);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleTableColumnSelectionCountFP(int vmID, JOBJECT64 table);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL IsAccessibleTableColumnSelectedFP(int vmID, JOBJECT64 table, int column);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableColumnSelectionsFP(int vmID, JOBJECT64 table, int count, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]int[] selections);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleTableRowFP(int vmID, JOBJECT64 table, int index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleTableColumnFP(int vmID, JOBJECT64 table, int index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetAccessibleTableIndexFP(int vmID, JOBJECT64 table, int row, int column);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetTextContentsFP(int vmID, JOBJECT64 ac, string text);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT64 GetParentWithRoleFP(int vmID, JOBJECT64 ac, [MarshalAs(UnmanagedType.LPWStr)]string role);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT64 GetParentWithRoleElseRootFP(int vmID, JOBJECT64 ac, [MarshalAs(UnmanagedType.LPWStr)]string role);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT64 GetTopLevelObjectFP(int vmID, JOBJECT64 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetObjectDepthFP(int vmID, JOBJECT64 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate JOBJECT64 GetActiveDescendentFP(int vmID, JOBJECT64 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetVirtualAccessibleNameFP(int vmID, JOBJECT64 ac, StringBuilder name, int len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetTextAttributesInRangeFP(int vmID, JOBJECT64 accessibleContext, int startIndex, int endIndex, out AccessibleTextAttributesInfo attributes, out short len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetCaretLocationFP(int vmID, JOBJECT64 ac, out AccessibleTextRectInfo rectInfo, int index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetVisibleChildrenCountFP(int vmID, JOBJECT64 accessibleContext);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetVisibleChildrenFP(int vmID, JOBJECT64 accessibleContext, int startIndex, out VisibleChildrenInfo children);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetVersionInfoFP(int vmID, out AccessBridgeVersionInfo info);
    #endregion

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

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL PropertyChangeFP(PropertyChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL JavaShutdownFP(JavaShutdownEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL FocusGainedFP(FocusGainedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL FocusLostFP(FocusLostEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL CaretUpdateFP(CaretUpdateEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL MouseClickedFP(MouseClickedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL MouseEnteredFP(MouseEnteredEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL MouseExitedFP(MouseExitedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL MousePressedFP(MousePressedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL MouseReleasedFP(MouseReleasedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL MenuCanceledFP(MenuCanceledEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL MenuDeselectedFP(MenuDeselectedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL MenuSelectedFP(MenuSelectedEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL PopupMenuCanceledFP(PopupMenuCanceledEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL PopupMenuWillBecomeInvisibleFP(PopupMenuWillBecomeInvisibleEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL PopupMenuWillBecomeVisibleFP(PopupMenuWillBecomeVisibleEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL PropertyNameChangeFP(PropertyNameChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL PropertyDescriptionChangeFP(PropertyDescriptionChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL PropertyStateChangeFP(PropertyStateChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL PropertyValueChangeFP(PropertyValueChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL PropertySelectionChangeFP(PropertySelectionChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL PropertyTextChangeFP(PropertyTextChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL PropertyCaretChangeFP(PropertyCaretChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL PropertyVisibleDataChangeFP(PropertyVisibleDataChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL PropertyChildChangeFP(PropertyChildChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL PropertyActiveDescendentChangeFP(PropertyActiveDescendentChangeEventHandler handler);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL PropertyTableModelChangeFP(PropertyTableModelChangeEventHandler handler);
    #endregion

    #region Event functions
    public PropertyChangeFP SetPropertyChange { get; set; }
    public JavaShutdownFP SetJavaShutdown { get; set; }
    public FocusGainedFP SetFocusGained { get; set; }
    public FocusLostFP SetFocusLost { get; set; }
    public CaretUpdateFP SetCaretUpdate { get; set; }
    public MouseClickedFP SetMouseClicked { get; set; }
    public MouseEnteredFP SetMouseEntered { get; set; }
    public MouseExitedFP SetMouseExited { get; set; }
    public MousePressedFP SetMousePressed { get; set; }
    public MouseReleasedFP SetMouseReleased { get; set; }
    public MenuCanceledFP SetMenuCanceled { get; set; }
    public MenuDeselectedFP SetMenuDeselected { get; set; }
    public MenuSelectedFP SetMenuSelected { get; set; }
    public PopupMenuCanceledFP SetPopupMenuCanceled { get; set; }
    public PopupMenuWillBecomeInvisibleFP SetPopupMenuWillBecomeInvisible { get; set; }
    public PopupMenuWillBecomeVisibleFP SetPopupMenuWillBecomeVisible { get; set; }
    public PropertyNameChangeFP SetPropertyNameChange { get; set; }
    public PropertyDescriptionChangeFP SetPropertyDescriptionChange { get; set; }
    public PropertyStateChangeFP SetPropertyStateChange { get; set; }
    public PropertyValueChangeFP SetPropertyValueChange { get; set; }
    public PropertySelectionChangeFP SetPropertySelectionChange { get; set; }
    public PropertyTextChangeFP SetPropertyTextChange { get; set; }
    public PropertyCaretChangeFP SetPropertyCaretChange { get; set; }
    public PropertyVisibleDataChangeFP SetPropertyVisibleDataChange { get; set; }
    public PropertyChildChangeFP SetPropertyChildChange { get; set; }
    public PropertyActiveDescendentChangeFP SetPropertyActiveDescendentChange { get; set; }
    public PropertyTableModelChangeFP SetPropertyTableModelChange { get; set; }
    #endregion
  }
  /// <summary>
  /// Native library event handlers implementation
  /// </summary>
  public partial class AccessBridgeEventsNative {
    #region Event functions
    public event AccessBridgeLibraryFunctions.PropertyChangeEventHandler PropertyChange;
    public event AccessBridgeLibraryFunctions.JavaShutdownEventHandler JavaShutdown;
    public event AccessBridgeLibraryFunctions.FocusGainedEventHandler FocusGained;
    public event AccessBridgeLibraryFunctions.FocusLostEventHandler FocusLost;
    public event AccessBridgeLibraryFunctions.CaretUpdateEventHandler CaretUpdate;
    public event AccessBridgeLibraryFunctions.MouseClickedEventHandler MouseClicked;
    public event AccessBridgeLibraryFunctions.MouseEnteredEventHandler MouseEntered;
    public event AccessBridgeLibraryFunctions.MouseExitedEventHandler MouseExited;
    public event AccessBridgeLibraryFunctions.MousePressedEventHandler MousePressed;
    public event AccessBridgeLibraryFunctions.MouseReleasedEventHandler MouseReleased;
    public event AccessBridgeLibraryFunctions.MenuCanceledEventHandler MenuCanceled;
    public event AccessBridgeLibraryFunctions.MenuDeselectedEventHandler MenuDeselected;
    public event AccessBridgeLibraryFunctions.MenuSelectedEventHandler MenuSelected;
    public event AccessBridgeLibraryFunctions.PopupMenuCanceledEventHandler PopupMenuCanceled;
    public event AccessBridgeLibraryFunctions.PopupMenuWillBecomeInvisibleEventHandler PopupMenuWillBecomeInvisible;
    public event AccessBridgeLibraryFunctions.PopupMenuWillBecomeVisibleEventHandler PopupMenuWillBecomeVisible;
    public event AccessBridgeLibraryFunctions.PropertyNameChangeEventHandler PropertyNameChange;
    public event AccessBridgeLibraryFunctions.PropertyDescriptionChangeEventHandler PropertyDescriptionChange;
    public event AccessBridgeLibraryFunctions.PropertyStateChangeEventHandler PropertyStateChange;
    public event AccessBridgeLibraryFunctions.PropertyValueChangeEventHandler PropertyValueChange;
    public event AccessBridgeLibraryFunctions.PropertySelectionChangeEventHandler PropertySelectionChange;
    public event AccessBridgeLibraryFunctions.PropertyTextChangeEventHandler PropertyTextChange;
    public event AccessBridgeLibraryFunctions.PropertyCaretChangeEventHandler PropertyCaretChange;
    public event AccessBridgeLibraryFunctions.PropertyVisibleDataChangeEventHandler PropertyVisibleDataChange;
    public event AccessBridgeLibraryFunctions.PropertyChildChangeEventHandler PropertyChildChange;
    public event AccessBridgeLibraryFunctions.PropertyActiveDescendentChangeEventHandler PropertyActiveDescendentChange;
    public event AccessBridgeLibraryFunctions.PropertyTableModelChangeEventHandler PropertyTableModelChange;
    #endregion

    #region Event handlers
    protected virtual void OnPropertyChange(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string property, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue) {
      var handler = PropertyChange;
      if (handler != null)
        handler(vmid, evt, source, property, oldValue, newValue);
    }
    protected virtual void OnJavaShutdown(int vmid) {
      var handler = JavaShutdown;
      if (handler != null)
        handler(vmid);
    }
    protected virtual void OnFocusGained(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = FocusGained;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnFocusLost(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = FocusLost;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnCaretUpdate(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = CaretUpdate;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseClicked(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = MouseClicked;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseEntered(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = MouseEntered;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseExited(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = MouseExited;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMousePressed(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = MousePressed;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMouseReleased(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = MouseReleased;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMenuCanceled(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = MenuCanceled;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMenuDeselected(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = MenuDeselected;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnMenuSelected(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = MenuSelected;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPopupMenuCanceled(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = PopupMenuCanceled;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPopupMenuWillBecomeInvisible(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = PopupMenuWillBecomeInvisible;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPopupMenuWillBecomeVisible(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = PopupMenuWillBecomeVisible;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyNameChange(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string oldName, [MarshalAs(UnmanagedType.LPWStr)]string newName) {
      var handler = PropertyNameChange;
      if (handler != null)
        handler(vmid, evt, source, oldName, newName);
    }
    protected virtual void OnPropertyDescriptionChange(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string oldDescription, [MarshalAs(UnmanagedType.LPWStr)]string newDescription) {
      var handler = PropertyDescriptionChange;
      if (handler != null)
        handler(vmid, evt, source, oldDescription, newDescription);
    }
    protected virtual void OnPropertyStateChange(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string oldState, [MarshalAs(UnmanagedType.LPWStr)]string newState) {
      var handler = PropertyStateChange;
      if (handler != null)
        handler(vmid, evt, source, oldState, newState);
    }
    protected virtual void OnPropertyValueChange(int vmid, JOBJECT64 evt, JOBJECT64 source, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue) {
      var handler = PropertyValueChange;
      if (handler != null)
        handler(vmid, evt, source, oldValue, newValue);
    }
    protected virtual void OnPropertySelectionChange(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = PropertySelectionChange;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyTextChange(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = PropertyTextChange;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyCaretChange(int vmid, JOBJECT64 evt, JOBJECT64 source, int oldPosition, int newPosition) {
      var handler = PropertyCaretChange;
      if (handler != null)
        handler(vmid, evt, source, oldPosition, newPosition);
    }
    protected virtual void OnPropertyVisibleDataChange(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      var handler = PropertyVisibleDataChange;
      if (handler != null)
        handler(vmid, evt, source);
    }
    protected virtual void OnPropertyChildChange(int vmid, JOBJECT64 evt, JOBJECT64 source, JOBJECT64 oldChild, JOBJECT64 newChild) {
      var handler = PropertyChildChange;
      if (handler != null)
        handler(vmid, evt, source, oldChild, newChild);
    }
    protected virtual void OnPropertyActiveDescendentChange(int vmid, JOBJECT64 evt, JOBJECT64 source, JOBJECT64 oldActiveDescendent, JOBJECT64 newActiveDescendent) {
      var handler = PropertyActiveDescendentChange;
      if (handler != null)
        handler(vmid, evt, source, oldActiveDescendent, newActiveDescendent);
    }
    protected virtual void OnPropertyTableModelChange(int vmid, JOBJECT64 evt, JOBJECT64 src, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue) {
      var handler = PropertyTableModelChange;
      if (handler != null)
        handler(vmid, evt, src, oldValue, newValue);
    }
    #endregion
  }

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

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessBridgeVersionInfo {
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
  public struct AccessibleActionInfo {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string name;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleActionsToDo {
    public int actionsCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public AccessibleActionInfo[] actions;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleContextInfo {
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
  public struct AccessibleHyperlinkInfo {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string text;
    public int startIndex;
    public int endIndex;
    public JOBJECT64 accessibleHyperlink;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleHypertextInfo {
    public int linkCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public AccessibleHyperlinkInfo[] links;
    public JOBJECT64 accessibleHypertext;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleIconInfo {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string description;
    public int height;
    public int width;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleIcons {
    public int iconsCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public AccessibleIconInfo[] iconInfo;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleKeyBindingInfo {
    public AccessibleKeyCode character;
    public AccessibleModifiers modifiers;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleKeyBindings {
    public int keyBindingsCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
    public AccessibleKeyBindingInfo[] keyBindingInfo;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleRelationInfo {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string key;
    public int targetCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
    public JOBJECT64[] targets;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleRelationSetInfo {
    public int relationCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public AccessibleRelationInfo[] relations;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTableCellInfo {
    public JOBJECT64 accessibleContext;
    public int index;
    public int row;
    public int column;
    public int rowExtent;
    public int columnExtent;
    public Byte isSelected;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTableInfo {
    public JOBJECT64 caption;
    public JOBJECT64 summary;
    public int rowCount;
    public int columnCount;
    public JOBJECT64 accessibleContext;
    public JOBJECT64 accessibleTable;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTextAttributesInfo {
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
  public struct AccessibleTextInfo {
    public int charCount;
    public int caretIndex;
    public int indexAtPoint;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTextItemsInfo {
    public Char letter;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string word;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
    public string sentence;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTextRectInfo {
    public int x;
    public int y;
    public int width;
    public int height;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleTextSelectionInfo {
    public int selectionStartIndex;
    public int selectionEndIndex;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
    public string selectedText;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct DoAccessibleActionsPackage {
    public int vmID;
    public JOBJECT64 accessibleContext;
    public AccessibleActionsToDo actionsToDo;
    public int rResult;
    public int failure;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct GetAccessibleActionsPackage {
    public int vmID;
    public JOBJECT64 accessibleContext;
    public AccessibleActions rAccessibleActions;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct GetAccessibleIconsPackage {
    public int vmID;
    public JOBJECT64 accessibleContext;
    public AccessibleIcons rAccessibleIcons;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct GetAccessibleKeyBindingsPackage {
    public int vmID;
    public JOBJECT64 accessibleContext;
    public AccessibleKeyBindings rAccessibleKeyBindings;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct GetAccessibleRelationSetPackage {
    public int vmID;
    public JOBJECT64 accessibleContext;
    public AccessibleRelationSetInfo rAccessibleRelationSetInfo;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct VisibleChildrenInfo {
    public int returnedChildrenCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public JOBJECT64[] children;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public class AccessibleActions {
    public int actionsCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public AccessibleActionInfo[] actionInfo;
  }
}
