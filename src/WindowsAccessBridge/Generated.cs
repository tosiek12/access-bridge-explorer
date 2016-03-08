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
  public interface IAccessBridgeFunctionsTODO /*: IAccessBridgeFunctions*/ {
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
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate BOOL EventSetterDelegate(Delegate nativeEventHandler);
    #endregion

    #region Event functions
    public EventSetterDelegate SetPropertyChange { get; set; }
    public EventSetterDelegate SetJavaShutdown { get; set; }
    public EventSetterDelegate SetFocusGained { get; set; }
    public EventSetterDelegate SetFocusLost { get; set; }
    public EventSetterDelegate SetCaretUpdate { get; set; }
    public EventSetterDelegate SetMouseClicked { get; set; }
    public EventSetterDelegate SetMouseEntered { get; set; }
    public EventSetterDelegate SetMouseExited { get; set; }
    public EventSetterDelegate SetMousePressed { get; set; }
    public EventSetterDelegate SetMouseReleased { get; set; }
    public EventSetterDelegate SetMenuCanceled { get; set; }
    public EventSetterDelegate SetMenuDeselected { get; set; }
    public EventSetterDelegate SetMenuSelected { get; set; }
    public EventSetterDelegate SetPopupMenuCanceled { get; set; }
    public EventSetterDelegate SetPopupMenuWillBecomeInvisible { get; set; }
    public EventSetterDelegate SetPopupMenuWillBecomeVisible { get; set; }
    public EventSetterDelegate SetPropertyNameChange { get; set; }
    public EventSetterDelegate SetPropertyDescriptionChange { get; set; }
    public EventSetterDelegate SetPropertyStateChange { get; set; }
    public EventSetterDelegate SetPropertyValueChange { get; set; }
    public EventSetterDelegate SetPropertySelectionChange { get; set; }
    public EventSetterDelegate SetPropertyTextChange { get; set; }
    public EventSetterDelegate SetPropertyCaretChange { get; set; }
    public EventSetterDelegate SetPropertyVisibleDataChange { get; set; }
    public EventSetterDelegate SetPropertyChildChange { get; set; }
    public EventSetterDelegate SetPropertyActiveDescendentChange { get; set; }
    public EventSetterDelegate SetPropertyTableModelChange { get; set; }
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
