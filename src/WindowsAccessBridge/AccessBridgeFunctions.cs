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

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;
using BOOL = System.Int32;
using HWND = System.IntPtr;
using AccessibleContext = AccessBridgeExplorer.WindowsAccessBridge.JOBJECT64;
using jint = System.Int32;

namespace AccessBridgeExplorer.WindowsAccessBridge {
  /// <summary>
  /// Single entry point to access all functions exposed by the Java Access Bridge
  /// DLL (<see cref="AccessBridge.Functions"/>).
  /// </summary>
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  public class AccessBridgeFunctions : IAccessBridgeFunctions {
    private AccessBridgeLibraryFunctions _functions;

    public AccessBridgeFunctions(AccessBridgeLibraryFunctions functions) {
      this._functions = functions;
    }

    #region IAccessBridgeFunctions implementation
    private bool ToBool(int value) {
      return value != 0;
    }

    private JavaObjectHandle Wrap(int vmID, JOBJECT64 handle) {
      return new JavaObjectHandle(vmID, handle);
    }

    void IAccessBridgeFunctions.Windows_run() {
      _functions.Windows_run();
    }

    bool IAccessBridgeFunctions.IsJavaWindow(HWND window) {
      return ToBool(_functions.IsJavaWindow(window));
    }

    bool IAccessBridgeFunctions.IsSameObject(int vmID, JavaObjectHandle obj1, JavaObjectHandle obj2) {
      return ToBool(_functions.IsSameObject(vmID, obj1.Handle, obj2.Handle));
    }

    bool IAccessBridgeFunctions.GetAccessibleContextFromHWND(HWND window, out int vmID, out JavaObjectHandle ac) {
      JOBJECT64 acRef;
      var success = _functions.GetAccessibleContextFromHWND(window, out vmID, out acRef);
      ac = Wrap(vmID, acRef);
      return ToBool(success);
    }

    HWND IAccessBridgeFunctions.GetHWNDFromAccessibleContext(int vmID, JavaObjectHandle ac) {
      return _functions.GetHWNDFromAccessibleContext(vmID, ac.Handle);
    }

    bool IAccessBridgeFunctions.GetAccessibleContextAt(int vmID, JavaObjectHandle acParent, int x, int y, out JavaObjectHandle ac) {
      JOBJECT64 acRef;
      var success = _functions.GetAccessibleContextAt(vmID, acParent.Handle, x, y, out acRef);
      ac = Wrap(vmID, acRef);
      return ToBool(success);
    }

    bool IAccessBridgeFunctions.GetAccessibleContextWithFocus(HWND window, out int vmID, out JavaObjectHandle ac) {
      JOBJECT64 acRef;
      var success = _functions.GetAccessibleContextWithFocus(window, out vmID, out acRef);
      ac = Wrap(vmID, acRef);
      return ToBool(success);
    }

    bool IAccessBridgeFunctions.GetAccessibleContextInfo(int vmID, JavaObjectHandle ac, out AccessibleContextInfo info) {
      return ToBool(_functions.GetAccessibleContextInfo(vmID, ac.Handle, out info));
    }

    JavaObjectHandle IAccessBridgeFunctions.GetAccessibleChildFromContext(int vmID, JavaObjectHandle ac, int i) {
      return Wrap(vmID, _functions.GetAccessibleChildFromContext(vmID, ac.Handle, i));
    }

    JavaObjectHandle IAccessBridgeFunctions.GetAccessibleParentFromContext(int vmID, JavaObjectHandle ac) {
      return Wrap(vmID, _functions.GetAccessibleParentFromContext(vmID, ac.Handle));
    }

    bool IAccessBridgeFunctions.GetAccessibleRelationSet(int vmID, JavaObjectHandle ac, out AccessibleRelationSetInfo relationSetInfo) {
      return ToBool(_functions.GetAccessibleRelationSet(vmID, ac.Handle, out relationSetInfo));
    }

    bool IAccessBridgeFunctions.GetAccessibleHypertext(int vmID, JavaObjectHandle ac, out AccessibleHypertextInfo hypertextInfo) {
      return ToBool(_functions.GetAccessibleHypertext(vmID, ac.Handle, out hypertextInfo));
    }

    bool IAccessBridgeFunctions.ActivateAccessibleHyperlink(int vmID, JavaObjectHandle ac, JavaObjectHandle accessibleHyperlink) {
      return ToBool(_functions.ActivateAccessibleHyperlink(vmID, ac.Handle, accessibleHyperlink.Handle));
    }

    int IAccessBridgeFunctions.GetAccessibleHyperlinkCount(int vmID, JavaObjectHandle ac) {
      return _functions.GetAccessibleHyperlinkCount(vmID, ac.Handle);
    }

    bool IAccessBridgeFunctions.GetAccessibleHypertextExt(int vmID, JavaObjectHandle ac, int nStartIndex, out AccessibleHypertextInfo hypertextInfo) {
      return ToBool(_functions.GetAccessibleHypertextExt(vmID, ac.Handle, nStartIndex, out hypertextInfo));
    }

    int IAccessBridgeFunctions.GetAccessibleHypertextLinkIndex(int vmID, JavaObjectHandle hypertext, int nIndex) {
      return _functions.GetAccessibleHypertextLinkIndex(vmID, hypertext.Handle, nIndex);
    }

    bool IAccessBridgeFunctions.GetAccessibleHyperlink(int vmID, JavaObjectHandle hypertext, int nIndex, out AccessibleHyperlinkInfo hyperlinkInfo) {
      return ToBool(_functions.GetAccessibleHyperlink(vmID, hypertext.Handle, nIndex, out hyperlinkInfo));
    }

    bool IAccessBridgeFunctions.GetAccessibleKeyBindings(int vmID, JavaObjectHandle ac, out AccessibleKeyBindings keyBindings) {
      return ToBool(_functions.GetAccessibleKeyBindings(vmID, ac.Handle, out keyBindings));
    }

    bool IAccessBridgeFunctions.GetAccessibleIcons(int vmID, JavaObjectHandle ac, out AccessibleIcons icons) {
      return ToBool(_functions.GetAccessibleIcons(vmID, ac.Handle, out icons));
    }

    bool IAccessBridgeFunctions.GetAccessibleActions(int vmID, JavaObjectHandle ac, AccessibleActions actions) {
      return ToBool(_functions.GetAccessibleActions(vmID, ac.Handle, actions));
    }

    bool IAccessBridgeFunctions.DoAccessibleActions(int vmID, JavaObjectHandle ac, ref AccessibleActionsToDo actionsToDo, out int failure) {
      return ToBool(_functions.DoAccessibleActions(vmID, ac.Handle, ref actionsToDo, out failure));
    }

    bool IAccessBridgeFunctions.GetAccessibleTextInfo(int vmID, JavaObjectHandle at, out AccessibleTextInfo textInfo, int x, int y) {
      return ToBool(_functions.GetAccessibleTextInfo(vmID, at.Handle, out textInfo, x, y));
    }

    bool IAccessBridgeFunctions.GetAccessibleTextItems(int vmID, JavaObjectHandle at, out AccessibleTextItemsInfo textItems, int index) {
      return ToBool(_functions.GetAccessibleTextItems(vmID, at.Handle, out textItems, index));
    }

    bool IAccessBridgeFunctions.GetAccessibleTextSelectionInfo(int vmID, JavaObjectHandle at, out AccessibleTextSelectionInfo textSelection) {
      return ToBool(_functions.GetAccessibleTextSelectionInfo(vmID, at.Handle, out textSelection));
    }

    bool IAccessBridgeFunctions.GetAccessibleTextAttributes(int vmID, JavaObjectHandle at, int index, out AccessibleTextAttributesInfo attributes) {
      return ToBool(_functions.GetAccessibleTextAttributes(vmID, at.Handle, index, out attributes));
    }

    bool IAccessBridgeFunctions.GetAccessibleTextRect(int vmID, JavaObjectHandle at, out AccessibleTextRectInfo rectInfo, int index) {
      return ToBool(_functions.GetAccessibleTextRect(vmID, at.Handle, out rectInfo, index));
    }

    bool IAccessBridgeFunctions.GetAccessibleTextLineBounds(int vmID, JavaObjectHandle at, int index, out int startIndex, out int endIndex) {
      return ToBool(_functions.GetAccessibleTextLineBounds(vmID, at.Handle, index, out startIndex, out endIndex));
    }

    bool IAccessBridgeFunctions.GetAccessibleTextRange(int vmID, JavaObjectHandle at, int start, int end, StringBuilder text, short len) {
      return ToBool(_functions.GetAccessibleTextRange(vmID, at.Handle, start, end, text, len));
    }

    bool IAccessBridgeFunctions.GetCurrentAccessibleValueFromContext(int vmID, JavaObjectHandle av, StringBuilder value, short len) {
      return ToBool(_functions.GetCurrentAccessibleValueFromContext(vmID, av.Handle, value, len));
    }

    bool IAccessBridgeFunctions.GetMaximumAccessibleValueFromContext(int vmID, JavaObjectHandle av, StringBuilder value, short len) {
      return ToBool(_functions.GetMaximumAccessibleValueFromContext(vmID, av.Handle, value, len));
    }

    bool IAccessBridgeFunctions.GetMinimumAccessibleValueFromContext(int vmID, JavaObjectHandle av, StringBuilder value, short len) {
      return ToBool(_functions.GetMinimumAccessibleValueFromContext(vmID, av.Handle, value, len));
    }

    void IAccessBridgeFunctions.AddAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel, int i) {
      _functions.AddAccessibleSelectionFromContext(vmID, asel.Handle, i);
    }

    void IAccessBridgeFunctions.ClearAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel) {
      _functions.ClearAccessibleSelectionFromContext(vmID, asel.Handle);
    }

    JavaObjectHandle IAccessBridgeFunctions.GetAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel, int i) {
      return Wrap(vmID, _functions.GetAccessibleSelectionFromContext(vmID, asel.Handle, i));
    }

    int IAccessBridgeFunctions.GetAccessibleSelectionCountFromContext(int vmID, JavaObjectHandle asel) {
      return _functions.GetAccessibleSelectionCountFromContext(vmID, asel.Handle);
    }

    bool IAccessBridgeFunctions.IsAccessibleChildSelectedFromContext(int vmID, JavaObjectHandle asel, int i) {
      return ToBool(_functions.IsAccessibleChildSelectedFromContext(vmID, asel.Handle, i));
    }

    void IAccessBridgeFunctions.RemoveAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel, int i) {
      _functions.RemoveAccessibleSelectionFromContext(vmID, asel.Handle, i);
    }

    void IAccessBridgeFunctions.SelectAllAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel) {
      _functions.SelectAllAccessibleSelectionFromContext(vmID, asel.Handle);
    }

    bool IAccessBridgeFunctions.GetAccessibleTableInfo(int vmID, JavaObjectHandle table, out AccessibleTableInfo tableInfo) {
      return ToBool(_functions.GetAccessibleTableInfo(vmID, table.Handle, out tableInfo));
    }

    bool IAccessBridgeFunctions.GetAccessibleTableCellInfo(int vmID, JavaObjectHandle table, int row, int column, out AccessibleTableCellInfo tableCellInfo) {
      return ToBool(_functions.GetAccessibleTableCellInfo(vmID, table.Handle, row, column, out tableCellInfo));
    }

    bool IAccessBridgeFunctions.GetAccessibleTableRowHeader(int vmID, JavaObjectHandle table, out AccessibleTableInfo tableInfo) {
      return ToBool(_functions.GetAccessibleTableRowHeader(vmID, table.Handle, out tableInfo));
    }

    bool IAccessBridgeFunctions.GetAccessibleTableColumnHeader(int vmID, JavaObjectHandle table, out AccessibleTableInfo tableInfo) {
      return ToBool(_functions.GetAccessibleTableColumnHeader(vmID, table.Handle, out tableInfo));
    }

    JavaObjectHandle IAccessBridgeFunctions.GetAccessibleTableRowDescription(int vmID, JavaObjectHandle table, int row) {
      return Wrap(vmID, _functions.GetAccessibleTableRowDescription(vmID, table.Handle, row));
    }

    JavaObjectHandle IAccessBridgeFunctions.GetAccessibleTableColumnDescription(int vmID, JavaObjectHandle table, int column) {
      return Wrap(vmID, _functions.GetAccessibleTableColumnDescription(vmID, table.Handle, column));
    }

    int IAccessBridgeFunctions.GetAccessibleTableRowSelectionCount(int vmID, JavaObjectHandle table) {
      return _functions.GetAccessibleTableRowSelectionCount(vmID, table.Handle);
    }

    bool IAccessBridgeFunctions.IsAccessibleTableRowSelected(int vmID, JavaObjectHandle table, int row) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.GetAccessibleTableRowSelections(int vmID, JavaObjectHandle table, int count, int[] selections) {
      throw new NotImplementedException();
    }

    int IAccessBridgeFunctions.GetAccessibleTableColumnSelectionCount(int vmID, JavaObjectHandle table) {
      return _functions.GetAccessibleTableColumnSelectionCount(vmID, table.Handle);
    }

    bool IAccessBridgeFunctions.IsAccessibleTableColumnSelected(int vmID, JavaObjectHandle table, int column) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.GetAccessibleTableColumnSelections(int vmID, JavaObjectHandle table, int count, int[] selections) {
      throw new NotImplementedException();
    }

    int IAccessBridgeFunctions.GetAccessibleTableRow(int vmID, JavaObjectHandle table, int index) {
      throw new NotImplementedException();
    }

    int IAccessBridgeFunctions.GetAccessibleTableColumn(int vmID, JavaObjectHandle table, int index) {
      throw new NotImplementedException();
    }

    int IAccessBridgeFunctions.GetAccessibleTableIndex(int vmID, JavaObjectHandle table, int row, int column) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetTextContents(int vmID, JavaObjectHandle ac, string text) {
      throw new NotImplementedException();
    }

    JavaObjectHandle IAccessBridgeFunctions.GetParentWithRole(int vmID, JavaObjectHandle ac, string role) {
      throw new NotImplementedException();
    }

    JavaObjectHandle IAccessBridgeFunctions.GetParentWithRoleElseRoot(int vmID, JavaObjectHandle ac, string role) {
      throw new NotImplementedException();
    }

    JavaObjectHandle IAccessBridgeFunctions.GetTopLevelObject(int vmID, JavaObjectHandle ac) {
      throw new NotImplementedException();
    }

    int IAccessBridgeFunctions.GetObjectDepth(int vmID, JavaObjectHandle ac) {
      return _functions.GetObjectDepth(vmID, ac.Handle);
    }

    JavaObjectHandle IAccessBridgeFunctions.GetActiveDescendent(int vmID, JavaObjectHandle ac) {
      return Wrap(vmID, _functions.GetActiveDescendent(vmID, ac.Handle));
    }

    bool IAccessBridgeFunctions.GetVirtualAccessibleName(int vmID, JavaObjectHandle ac, StringBuilder name, int len) {
      return ToBool(_functions.GetVirtualAccessibleName(vmID, ac.Handle, name, len));
    }

    bool IAccessBridgeFunctions.GetTextAttributesInRange(
      int vmID,
      JavaObjectHandle accessibleContext,
      int startIndex,
      int endIndex,
      out AccessibleTextAttributesInfo attributes,
      out short len) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.GetCaretLocation(int vmID, JavaObjectHandle ac, out AccessibleTextRectInfo rectInfo, int index) {
      throw new NotImplementedException();
    }

    int IAccessBridgeFunctions.GetVisibleChildrenCount(int vmID, JavaObjectHandle accessibleContext) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.GetVisibleChildren(int vmID, JavaObjectHandle accessibleContext, int startIndex, out VisibleChildrenInfo children) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.GetVersionInfo(int vmID, out AccessBridgeVersionInfo info) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetPropertyChange(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetJavaShutdown(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetFocusGained(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetFocusLost(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetCaretUpdate(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetMouseClicked(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetMouseEntered(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetMouseExited(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetMousePressed(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetMouseReleased(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetMenuCanceled(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetMenuDeselected(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetMenuSelected(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetPopupMenuCanceled(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetPopupMenuWillBecomeInvisible(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetPopupMenuWillBecomeVisible(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetPropertyNameChange(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetPropertyDescriptionChange(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetPropertyStateChange(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetPropertyValueChange(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetPropertySelectionChange(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetPropertyTextChange(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetPropertyCaretChange(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetPropertyVisibleDataChange(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetPropertyChildChange(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetPropertyActiveDescendentChange(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.SetPropertyTableModelChange(Delegate nativeEventHandler) {
      throw new NotImplementedException();
    }
    #endregion
  }
}