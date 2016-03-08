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
using CodeGen.Interop.NativeStructures;

namespace CodeGen.Interop {
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  public interface IAccessBridgeFunctions {
    void Windows_run();

    bool IsJavaWindow(WindowHandle window);
    bool IsSameObject(int vmID, JavaObjectHandle obj1, JavaObjectHandle obj2);
    bool GetAccessibleContextFromHWND(WindowHandle window, out int vmID, out JavaObjectHandle ac);
    WindowHandle GetHWNDFromAccessibleContext(int vmID, JavaObjectHandle ac);

    /// <summary>
    /// Returns the deepest accessible context at screen location (x,y) using
    /// <paramref name="acParent"/> as the root of the search tree. The returned
    /// AccessibleContext <paramref name="ac"/> maybe be equal to <paramref
    /// name="acParent"/> in case there is no child at that location. Returns
    /// <code>false</code> in case of serious error.
    /// </summary>
    bool GetAccessibleContextAt(int vmID, JavaObjectHandle acParent, int x, int y, out JavaObjectHandle ac);

    bool GetAccessibleContextWithFocus(WindowHandle window, out int vmID, out JavaObjectHandle ac);
    bool GetAccessibleContextInfo(int vmID, JavaObjectHandle ac, out AccessibleContextInfo info);
    JavaObjectHandle GetAccessibleChildFromContext(int vmID, JavaObjectHandle ac, int i);
    JavaObjectHandle GetAccessibleParentFromContext(int vmID, JavaObjectHandle ac);

    #region AccessibleRelationSet

    bool GetAccessibleRelationSet(
      int vmID,
      JavaObjectHandle accessibleContext,
      out AccessibleRelationSetInfo relationSetInfo);

    #endregion

    #region AccessibleHypertext

    bool GetAccessibleHypertext(int vmID, JavaObjectHandle accessibleContext, out AccessibleHypertextInfo hypertextInfo);
    bool ActivateAccessibleHyperlink(int vmID, JavaObjectHandle accessibleContext, JavaObjectHandle accessibleHyperlink);
    int GetAccessibleHyperlinkCount(int vmID, JavaObjectHandle accessibleContext);

    bool GetAccessibleHypertextExt(
      int vmID,
      JavaObjectHandle accessibleContext,
      int nStartIndex,
      out AccessibleHypertextInfo hypertextInfo);

    int GetAccessibleHypertextLinkIndex(int vmID, JavaObjectHandle hypertext, int nIndex);

    bool GetAccessibleHyperlink(
      int vmID,
      JavaObjectHandle hypertext,
      int nIndex,
      out AccessibleHyperlinkInfo hyperlinkInfo);

    #endregion

    #region Accessible KeyBindings, Icons and Actions

    bool GetAccessibleKeyBindings(int vmID, JavaObjectHandle accessibleContext, out AccessibleKeyBindings keyBindings);
    bool GetAccessibleIcons(int vmID, JavaObjectHandle accessibleContext, out AccessibleIcons icons);
    bool GetAccessibleActions(int vmID, JavaObjectHandle accessibleContext, [Out] AccessibleActions actions);

    bool DoAccessibleActions(
      int vmID,
      JavaObjectHandle accessibleContext,
      ref AccessibleActionsToDo actionsToDo,
      out int failure);

    #endregion

    #region AccessibleText

    bool GetAccessibleTextInfo(int vmID, JavaObjectHandle at, out AccessibleTextInfo textInfo, int x, int y);
    bool GetAccessibleTextItems(int vmID, JavaObjectHandle at, out AccessibleTextItemsInfo textItems, int index);
    bool GetAccessibleTextSelectionInfo(int vmID, JavaObjectHandle at, out AccessibleTextSelectionInfo textSelection);

    bool GetAccessibleTextAttributes(
      int vmID,
      JavaObjectHandle at,
      int index,
      out AccessibleTextAttributesInfo attributes);

    bool GetAccessibleTextRect(int vmID, JavaObjectHandle at, out AccessibleTextRectInfo rectInfo, int index);
    bool GetAccessibleTextLineBounds(int vmID, JavaObjectHandle at, int index, out int startIndex, out int endIndex);
    bool GetAccessibleTextRange(int vmID, JavaObjectHandle at, int start, int end, StringBuilder text, short len);

    #endregion

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

    #region AccessibleTable

    bool GetAccessibleTableInfo(int vmID, JavaObjectHandle ac, out AccessibleTableInfo tableInfo);

    bool GetAccessibleTableCellInfo(
      int vmID,
      JavaObjectHandle accessibleTable,
      int row,
      int column,
      out AccessibleTableCellInfo tableCellInfo);

    bool GetAccessibleTableRowHeader(int vmID, JavaObjectHandle acParent, out AccessibleTableInfo tableInfo);
    bool GetAccessibleTableColumnHeader(int vmID, JavaObjectHandle acParent, out AccessibleTableInfo tableInfo);
    JavaObjectHandle GetAccessibleTableRowDescription(int vmID, JavaObjectHandle acParent, int row);
    JavaObjectHandle GetAccessibleTableColumnDescription(int vmID, JavaObjectHandle acParent, int column);
    int GetAccessibleTableRowSelectionCount(int vmID, JavaObjectHandle table);
    bool IsAccessibleTableRowSelected(int vmID, JavaObjectHandle table, int row);

    bool GetAccessibleTableRowSelections(
      int vmID,
      JavaObjectHandle table,
      int count,
      [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] int[] selections);

    int GetAccessibleTableColumnSelectionCount(int vmID, JavaObjectHandle table);
    bool IsAccessibleTableColumnSelected(int vmID, JavaObjectHandle table, int column);

    bool GetAccessibleTableColumnSelections(
      int vmID,
      JavaObjectHandle table,
      int count,
      [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] int[] selections);

    /// <summary>
    /// Return the row number for a cell at a given index
    /// </summary>
    int GetAccessibleTableRow(int vmID, JavaObjectHandle table, int index);

    /// <summary>
    /// Return the column number for a cell at a given index
    /// </summary>
    int GetAccessibleTableColumn(int vmID, JavaObjectHandle table, int index);

    /// <summary>
    /// Return the index of a cell at a given row and column
    /// </summary>
    int GetAccessibleTableIndex(int vmID, JavaObjectHandle table, int row, int column);

    #endregion

    #region Additional utility methods

    bool SetTextContents(int vmID, JavaObjectHandle ac, string text);
    JavaObjectHandle GetParentWithRole(int vmID, JavaObjectHandle ac, [MarshalAs(UnmanagedType.LPWStr)] string role);

    JavaObjectHandle GetParentWithRoleElseRoot(
      int vmID,
      JavaObjectHandle ac,
      [MarshalAs(UnmanagedType.LPWStr)] string role);

    JavaObjectHandle GetTopLevelObject(int vmID, JavaObjectHandle ac);
    int GetObjectDepth(int vmID, JavaObjectHandle ac);
    JavaObjectHandle GetActiveDescendent(int vmID, JavaObjectHandle ac);

    bool GetVirtualAccessibleName(int vmID, JavaObjectHandle ac, StringBuilder name, int len);

    bool GetTextAttributesInRange(
      int vmID,
      JavaObjectHandle accessibleContext,
      int startIndex,
      int endIndex,
      out AccessibleTextAttributesInfo attributes,
      out short len);

    bool GetCaretLocation(int vmID, JavaObjectHandle ac, out AccessibleTextRectInfo rectInfo, int index);

    int GetVisibleChildrenCount(int vmID, JavaObjectHandle accessibleContext);

    bool GetVisibleChildren(
      int vmID,
      JavaObjectHandle accessibleContext,
      int startIndex,
      out VisibleChildrenInfo children);

    #endregion

    bool GetVersionInfo(int vmID, out AccessBridgeVersionInfo info);

    #region Event handling routines

    event Action PropertyChange;

    event Action JavaShutdown;

    event Action FocusGained;
    event Action FocusLost;

    event Action CaretUpdate;

    event Action MouseClicked;
    event Action MouseEntered;
    event Action MouseExited;
    event Action MousePressed;
    event Action MouseReleased;

    event Action MenuCanceled;
    event Action MenuDeselected;
    event Action MenuSelected;
    event Action PopupMenuCanceled;
    event Action PopupMenuWillBecomeInvisible;
    event Action PopupMenuWillBecomeVisible;

    event Action PropertyNameChange;
    event Action PropertyDescriptionChange;
    event Action PropertyStateChange;
    event Action PropertyValueChange;
    event Action PropertySelectionChange;
    event Action PropertyTextChange;
    event Action PropertyCaretChange;
    event Action PropertyVisibleDataChange;
    event Action PropertyChildChange;
    event Action PropertyActiveDescendentChange;

    event Action PropertyTableModelChange;

    #endregion
  }
}