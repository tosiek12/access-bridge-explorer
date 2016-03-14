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
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedMember.Local
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
using System;
using System.Runtime.InteropServices;
using System.Text;
using WindowHandle = System.IntPtr;
using BOOL = System.Int32;

namespace WindowsAccessBridgeInterop {
  /// <summary>
  /// Implementation of platform agnostic functions
  /// </summary>
  internal partial class AccessBridgeNativeFunctions : IAccessBridgeFunctions {

    #region Function implementations

    public void Windows_run() {
      EntryPoints.Windows_run();
    }

    public bool IsJavaWindow(WindowHandle window) {
      var result = EntryPoints.IsJavaWindow(window);
      return ToBool(result);
    }

    public bool IsSameObject(int vmid, JavaObjectHandle obj1, JavaObjectHandle obj2) {
      var result = EntryPoints.IsSameObject(vmid, Unwrap(vmid, obj1), Unwrap(vmid, obj2));
      GC.KeepAlive(obj1);
      GC.KeepAlive(obj2);
      return ToBool(result);
    }

    public bool GetAccessibleContextFromHWND(WindowHandle window, out int vmid, out JavaObjectHandle ac) {
      JOBJECT64 acTemp;
      var result = EntryPoints.GetAccessibleContextFromHWND(window, out vmid, out acTemp);
      if (Succeeded(result)) {
        ac = Wrap(vmid, acTemp);
      } else {
        acTemp = default(JOBJECT64);
        ac = Wrap(vmid, acTemp);
      }
      return Succeeded(result);
    }

    public WindowHandle GetHWNDFromAccessibleContext(int vmid, JavaObjectHandle ac) {
      var result = EntryPoints.GetHWNDFromAccessibleContext(vmid, Unwrap(vmid, ac));
      GC.KeepAlive(ac);
      return result;
    }

    public bool GetAccessibleContextAt(int vmid, JavaObjectHandle acParent, int x, int y, out JavaObjectHandle ac) {
      JOBJECT64 acTemp;
      var result = EntryPoints.GetAccessibleContextAt(vmid, Unwrap(vmid, acParent), x, y, out acTemp);
      GC.KeepAlive(acParent);
      if (Succeeded(result)) {
        ac = Wrap(vmid, acTemp);
      } else {
        acTemp = default(JOBJECT64);
        ac = Wrap(vmid, acTemp);
      }
      return Succeeded(result);
    }

    public bool GetAccessibleContextWithFocus(WindowHandle window, out int vmid, out JavaObjectHandle ac) {
      JOBJECT64 acTemp;
      var result = EntryPoints.GetAccessibleContextWithFocus(window, out vmid, out acTemp);
      if (Succeeded(result)) {
        ac = Wrap(vmid, acTemp);
      } else {
        acTemp = default(JOBJECT64);
        ac = Wrap(vmid, acTemp);
      }
      return Succeeded(result);
    }

    public bool GetAccessibleContextInfo(int vmid, JavaObjectHandle ac, out AccessibleContextInfo info) {
      info = new AccessibleContextInfo();
      var result = EntryPoints.GetAccessibleContextInfo(vmid, Unwrap(vmid, ac), info);
      GC.KeepAlive(ac);
      return Succeeded(result);
    }

    public JavaObjectHandle GetAccessibleChildFromContext(int vmid, JavaObjectHandle ac, int i) {
      var result = EntryPoints.GetAccessibleChildFromContext(vmid, Unwrap(vmid, ac), i);
      GC.KeepAlive(ac);
      return Wrap(vmid, result);
    }

    public JavaObjectHandle GetAccessibleParentFromContext(int vmid, JavaObjectHandle ac) {
      var result = EntryPoints.GetAccessibleParentFromContext(vmid, Unwrap(vmid, ac));
      GC.KeepAlive(ac);
      return Wrap(vmid, result);
    }

    public bool GetAccessibleRelationSet(int vmid, JavaObjectHandle accessibleContext, out AccessibleRelationSetInfo relationSetInfo) {
      AccessibleRelationSetInfoNative relationSetInfoTemp;
      var result = EntryPoints.GetAccessibleRelationSet(vmid, Unwrap(vmid, accessibleContext), out relationSetInfoTemp);
      GC.KeepAlive(accessibleContext);
      if (Succeeded(result))
        relationSetInfo = Wrap(vmid, relationSetInfoTemp);
      else
        relationSetInfo = default(AccessibleRelationSetInfo);
      return Succeeded(result);
    }

    public bool GetAccessibleHypertext(int vmid, JavaObjectHandle accessibleContext, out AccessibleHypertextInfo hypertextInfo) {
      AccessibleHypertextInfoNative hypertextInfoTemp;
      var result = EntryPoints.GetAccessibleHypertext(vmid, Unwrap(vmid, accessibleContext), out hypertextInfoTemp);
      GC.KeepAlive(accessibleContext);
      if (Succeeded(result))
        hypertextInfo = Wrap(vmid, hypertextInfoTemp);
      else
        hypertextInfo = default(AccessibleHypertextInfo);
      return Succeeded(result);
    }

    public bool ActivateAccessibleHyperlink(int vmid, JavaObjectHandle accessibleContext, JavaObjectHandle accessibleHyperlink) {
      var result = EntryPoints.ActivateAccessibleHyperlink(vmid, Unwrap(vmid, accessibleContext), Unwrap(vmid, accessibleHyperlink));
      GC.KeepAlive(accessibleContext);
      GC.KeepAlive(accessibleHyperlink);
      return ToBool(result);
    }

    public int GetAccessibleHyperlinkCount(int vmid, JavaObjectHandle accessibleContext) {
      var result = EntryPoints.GetAccessibleHyperlinkCount(vmid, Unwrap(vmid, accessibleContext));
      GC.KeepAlive(accessibleContext);
      return result;
    }

    public bool GetAccessibleHypertextExt(int vmid, JavaObjectHandle accessibleContext, int nStartIndex, out AccessibleHypertextInfo hypertextInfo) {
      AccessibleHypertextInfoNative hypertextInfoTemp;
      var result = EntryPoints.GetAccessibleHypertextExt(vmid, Unwrap(vmid, accessibleContext), nStartIndex, out hypertextInfoTemp);
      GC.KeepAlive(accessibleContext);
      if (Succeeded(result))
        hypertextInfo = Wrap(vmid, hypertextInfoTemp);
      else
        hypertextInfo = default(AccessibleHypertextInfo);
      return Succeeded(result);
    }

    public int GetAccessibleHypertextLinkIndex(int vmid, JavaObjectHandle hypertext, int nIndex) {
      var result = EntryPoints.GetAccessibleHypertextLinkIndex(vmid, Unwrap(vmid, hypertext), nIndex);
      GC.KeepAlive(hypertext);
      return result;
    }

    public bool GetAccessibleHyperlink(int vmid, JavaObjectHandle hypertext, int nIndex, out AccessibleHyperlinkInfo hyperlinkInfo) {
      AccessibleHyperlinkInfoNative hyperlinkInfoTemp;
      var result = EntryPoints.GetAccessibleHyperlink(vmid, Unwrap(vmid, hypertext), nIndex, out hyperlinkInfoTemp);
      GC.KeepAlive(hypertext);
      if (Succeeded(result))
        hyperlinkInfo = Wrap(vmid, hyperlinkInfoTemp);
      else
        hyperlinkInfo = default(AccessibleHyperlinkInfo);
      return Succeeded(result);
    }

    public bool GetAccessibleKeyBindings(int vmid, JavaObjectHandle accessibleContext, out AccessibleKeyBindings keyBindings) {
      var result = EntryPoints.GetAccessibleKeyBindings(vmid, Unwrap(vmid, accessibleContext), out keyBindings);
      GC.KeepAlive(accessibleContext);
      return Succeeded(result);
    }

    public bool GetAccessibleIcons(int vmid, JavaObjectHandle accessibleContext, out AccessibleIcons icons) {
      var result = EntryPoints.GetAccessibleIcons(vmid, Unwrap(vmid, accessibleContext), out icons);
      GC.KeepAlive(accessibleContext);
      return Succeeded(result);
    }

    public bool GetAccessibleActions(int vmid, JavaObjectHandle accessibleContext, out AccessibleActions actions) {
      actions = new AccessibleActions();
      var result = EntryPoints.GetAccessibleActions(vmid, Unwrap(vmid, accessibleContext), actions);
      GC.KeepAlive(accessibleContext);
      return Succeeded(result);
    }

    public bool DoAccessibleActions(int vmid, JavaObjectHandle accessibleContext, ref AccessibleActionsToDo actionsToDo, out int failure) {
      var result = EntryPoints.DoAccessibleActions(vmid, Unwrap(vmid, accessibleContext), ref actionsToDo, out failure);
      GC.KeepAlive(accessibleContext);
      return Succeeded(result);
    }

    public bool GetAccessibleTextInfo(int vmid, JavaObjectHandle at, out AccessibleTextInfo textInfo, int x, int y) {
      var result = EntryPoints.GetAccessibleTextInfo(vmid, Unwrap(vmid, at), out textInfo, x, y);
      GC.KeepAlive(at);
      return Succeeded(result);
    }

    public bool GetAccessibleTextItems(int vmid, JavaObjectHandle at, out AccessibleTextItemsInfo textItems, int index) {
      var result = EntryPoints.GetAccessibleTextItems(vmid, Unwrap(vmid, at), out textItems, index);
      GC.KeepAlive(at);
      return Succeeded(result);
    }

    public bool GetAccessibleTextSelectionInfo(int vmid, JavaObjectHandle at, out AccessibleTextSelectionInfo textSelection) {
      var result = EntryPoints.GetAccessibleTextSelectionInfo(vmid, Unwrap(vmid, at), out textSelection);
      GC.KeepAlive(at);
      return Succeeded(result);
    }

    public bool GetAccessibleTextAttributes(int vmid, JavaObjectHandle at, int index, out AccessibleTextAttributesInfo attributes) {
      attributes = new AccessibleTextAttributesInfo();
      var result = EntryPoints.GetAccessibleTextAttributes(vmid, Unwrap(vmid, at), index, attributes);
      GC.KeepAlive(at);
      return Succeeded(result);
    }

    public bool GetAccessibleTextRect(int vmid, JavaObjectHandle at, out AccessibleTextRectInfo rectInfo, int index) {
      var result = EntryPoints.GetAccessibleTextRect(vmid, Unwrap(vmid, at), out rectInfo, index);
      GC.KeepAlive(at);
      return Succeeded(result);
    }

    public bool GetAccessibleTextLineBounds(int vmid, JavaObjectHandle at, int index, out int startIndex, out int endIndex) {
      var result = EntryPoints.GetAccessibleTextLineBounds(vmid, Unwrap(vmid, at), index, out startIndex, out endIndex);
      GC.KeepAlive(at);
      return Succeeded(result);
    }

    public bool GetAccessibleTextRange(int vmid, JavaObjectHandle at, int start, int end, [Out]char[] text, short len) {
      var result = EntryPoints.GetAccessibleTextRange(vmid, Unwrap(vmid, at), start, end, text, len);
      GC.KeepAlive(at);
      return Succeeded(result);
    }

    public bool GetCurrentAccessibleValueFromContext(int vmid, JavaObjectHandle av, StringBuilder value, short len) {
      var result = EntryPoints.GetCurrentAccessibleValueFromContext(vmid, Unwrap(vmid, av), value, len);
      GC.KeepAlive(av);
      return Succeeded(result);
    }

    public bool GetMaximumAccessibleValueFromContext(int vmid, JavaObjectHandle av, StringBuilder value, short len) {
      var result = EntryPoints.GetMaximumAccessibleValueFromContext(vmid, Unwrap(vmid, av), value, len);
      GC.KeepAlive(av);
      return Succeeded(result);
    }

    public bool GetMinimumAccessibleValueFromContext(int vmid, JavaObjectHandle av, StringBuilder value, short len) {
      var result = EntryPoints.GetMinimumAccessibleValueFromContext(vmid, Unwrap(vmid, av), value, len);
      GC.KeepAlive(av);
      return Succeeded(result);
    }

    public void AddAccessibleSelectionFromContext(int vmid, JavaObjectHandle asel, int i) {
      EntryPoints.AddAccessibleSelectionFromContext(vmid, Unwrap(vmid, asel), i);
      GC.KeepAlive(asel);
    }

    public void ClearAccessibleSelectionFromContext(int vmid, JavaObjectHandle asel) {
      EntryPoints.ClearAccessibleSelectionFromContext(vmid, Unwrap(vmid, asel));
      GC.KeepAlive(asel);
    }

    public JavaObjectHandle GetAccessibleSelectionFromContext(int vmid, JavaObjectHandle asel, int i) {
      var result = EntryPoints.GetAccessibleSelectionFromContext(vmid, Unwrap(vmid, asel), i);
      GC.KeepAlive(asel);
      return Wrap(vmid, result);
    }

    public int GetAccessibleSelectionCountFromContext(int vmid, JavaObjectHandle asel) {
      var result = EntryPoints.GetAccessibleSelectionCountFromContext(vmid, Unwrap(vmid, asel));
      GC.KeepAlive(asel);
      return result;
    }

    public bool IsAccessibleChildSelectedFromContext(int vmid, JavaObjectHandle asel, int i) {
      var result = EntryPoints.IsAccessibleChildSelectedFromContext(vmid, Unwrap(vmid, asel), i);
      GC.KeepAlive(asel);
      return ToBool(result);
    }

    public void RemoveAccessibleSelectionFromContext(int vmid, JavaObjectHandle asel, int i) {
      EntryPoints.RemoveAccessibleSelectionFromContext(vmid, Unwrap(vmid, asel), i);
      GC.KeepAlive(asel);
    }

    public void SelectAllAccessibleSelectionFromContext(int vmid, JavaObjectHandle asel) {
      EntryPoints.SelectAllAccessibleSelectionFromContext(vmid, Unwrap(vmid, asel));
      GC.KeepAlive(asel);
    }

    public bool GetAccessibleTableInfo(int vmid, JavaObjectHandle ac, out AccessibleTableInfo tableInfo) {
      AccessibleTableInfoNative tableInfoTemp = new AccessibleTableInfoNative();
      var result = EntryPoints.GetAccessibleTableInfo(vmid, Unwrap(vmid, ac), tableInfoTemp);
      GC.KeepAlive(ac);
      tableInfo = new AccessibleTableInfo();
      if (Succeeded(result))
        CopyWrap(vmid, tableInfoTemp, tableInfo);
      return Succeeded(result);
    }

    public bool GetAccessibleTableCellInfo(int vmid, JavaObjectHandle at, int row, int column, out AccessibleTableCellInfo tableCellInfo) {
      AccessibleTableCellInfoNative tableCellInfoTemp = new AccessibleTableCellInfoNative();
      var result = EntryPoints.GetAccessibleTableCellInfo(vmid, Unwrap(vmid, at), row, column, tableCellInfoTemp);
      GC.KeepAlive(at);
      tableCellInfo = new AccessibleTableCellInfo();
      if (Succeeded(result))
        CopyWrap(vmid, tableCellInfoTemp, tableCellInfo);
      return Succeeded(result);
    }

    public bool GetAccessibleTableRowHeader(int vmid, JavaObjectHandle acParent, out AccessibleTableInfo tableInfo) {
      AccessibleTableInfoNative tableInfoTemp = new AccessibleTableInfoNative();
      var result = EntryPoints.GetAccessibleTableRowHeader(vmid, Unwrap(vmid, acParent), tableInfoTemp);
      GC.KeepAlive(acParent);
      tableInfo = new AccessibleTableInfo();
      if (Succeeded(result))
        CopyWrap(vmid, tableInfoTemp, tableInfo);
      return Succeeded(result);
    }

    public bool GetAccessibleTableColumnHeader(int vmid, JavaObjectHandle acParent, out AccessibleTableInfo tableInfo) {
      AccessibleTableInfoNative tableInfoTemp = new AccessibleTableInfoNative();
      var result = EntryPoints.GetAccessibleTableColumnHeader(vmid, Unwrap(vmid, acParent), tableInfoTemp);
      GC.KeepAlive(acParent);
      tableInfo = new AccessibleTableInfo();
      if (Succeeded(result))
        CopyWrap(vmid, tableInfoTemp, tableInfo);
      return Succeeded(result);
    }

    public JavaObjectHandle GetAccessibleTableRowDescription(int vmid, JavaObjectHandle acParent, int row) {
      var result = EntryPoints.GetAccessibleTableRowDescription(vmid, Unwrap(vmid, acParent), row);
      GC.KeepAlive(acParent);
      return Wrap(vmid, result);
    }

    public JavaObjectHandle GetAccessibleTableColumnDescription(int vmid, JavaObjectHandle acParent, int column) {
      var result = EntryPoints.GetAccessibleTableColumnDescription(vmid, Unwrap(vmid, acParent), column);
      GC.KeepAlive(acParent);
      return Wrap(vmid, result);
    }

    public int GetAccessibleTableRowSelectionCount(int vmid, JavaObjectHandle table) {
      var result = EntryPoints.GetAccessibleTableRowSelectionCount(vmid, Unwrap(vmid, table));
      GC.KeepAlive(table);
      return result;
    }

    public bool IsAccessibleTableRowSelected(int vmid, JavaObjectHandle table, int row) {
      var result = EntryPoints.IsAccessibleTableRowSelected(vmid, Unwrap(vmid, table), row);
      GC.KeepAlive(table);
      return ToBool(result);
    }

    public bool GetAccessibleTableRowSelections(int vmid, JavaObjectHandle table, int count, [Out]int[] selections) {
      var result = EntryPoints.GetAccessibleTableRowSelections(vmid, Unwrap(vmid, table), count, selections);
      GC.KeepAlive(table);
      return Succeeded(result);
    }

    public int GetAccessibleTableColumnSelectionCount(int vmid, JavaObjectHandle table) {
      var result = EntryPoints.GetAccessibleTableColumnSelectionCount(vmid, Unwrap(vmid, table));
      GC.KeepAlive(table);
      return result;
    }

    public bool IsAccessibleTableColumnSelected(int vmid, JavaObjectHandle table, int column) {
      var result = EntryPoints.IsAccessibleTableColumnSelected(vmid, Unwrap(vmid, table), column);
      GC.KeepAlive(table);
      return ToBool(result);
    }

    public bool GetAccessibleTableColumnSelections(int vmid, JavaObjectHandle table, int count, [Out]int[] selections) {
      var result = EntryPoints.GetAccessibleTableColumnSelections(vmid, Unwrap(vmid, table), count, selections);
      GC.KeepAlive(table);
      return Succeeded(result);
    }

    public int GetAccessibleTableRow(int vmid, JavaObjectHandle table, int index) {
      var result = EntryPoints.GetAccessibleTableRow(vmid, Unwrap(vmid, table), index);
      GC.KeepAlive(table);
      return result;
    }

    public int GetAccessibleTableColumn(int vmid, JavaObjectHandle table, int index) {
      var result = EntryPoints.GetAccessibleTableColumn(vmid, Unwrap(vmid, table), index);
      GC.KeepAlive(table);
      return result;
    }

    public int GetAccessibleTableIndex(int vmid, JavaObjectHandle table, int row, int column) {
      var result = EntryPoints.GetAccessibleTableIndex(vmid, Unwrap(vmid, table), row, column);
      GC.KeepAlive(table);
      return result;
    }

    public bool SetTextContents(int vmid, JavaObjectHandle ac, string text) {
      var result = EntryPoints.SetTextContents(vmid, Unwrap(vmid, ac), text);
      GC.KeepAlive(ac);
      return Succeeded(result);
    }

    public JavaObjectHandle GetParentWithRole(int vmid, JavaObjectHandle ac, string role) {
      var result = EntryPoints.GetParentWithRole(vmid, Unwrap(vmid, ac), role);
      GC.KeepAlive(ac);
      return Wrap(vmid, result);
    }

    public JavaObjectHandle GetParentWithRoleElseRoot(int vmid, JavaObjectHandle ac, string role) {
      var result = EntryPoints.GetParentWithRoleElseRoot(vmid, Unwrap(vmid, ac), role);
      GC.KeepAlive(ac);
      return Wrap(vmid, result);
    }

    public JavaObjectHandle GetTopLevelObject(int vmid, JavaObjectHandle ac) {
      var result = EntryPoints.GetTopLevelObject(vmid, Unwrap(vmid, ac));
      GC.KeepAlive(ac);
      return Wrap(vmid, result);
    }

    public int GetObjectDepth(int vmid, JavaObjectHandle ac) {
      var result = EntryPoints.GetObjectDepth(vmid, Unwrap(vmid, ac));
      GC.KeepAlive(ac);
      return result;
    }

    public JavaObjectHandle GetActiveDescendent(int vmid, JavaObjectHandle ac) {
      var result = EntryPoints.GetActiveDescendent(vmid, Unwrap(vmid, ac));
      GC.KeepAlive(ac);
      return Wrap(vmid, result);
    }

    public bool GetVirtualAccessibleName(int vmid, JavaObjectHandle ac, StringBuilder name, int len) {
      var result = EntryPoints.GetVirtualAccessibleName(vmid, Unwrap(vmid, ac), name, len);
      GC.KeepAlive(ac);
      return Succeeded(result);
    }

    public bool GetTextAttributesInRange(int vmid, JavaObjectHandle accessibleContext, int startIndex, int endIndex, out AccessibleTextAttributesInfo attributes, out short len) {
      attributes = new AccessibleTextAttributesInfo();
      var result = EntryPoints.GetTextAttributesInRange(vmid, Unwrap(vmid, accessibleContext), startIndex, endIndex, attributes, out len);
      GC.KeepAlive(accessibleContext);
      return Succeeded(result);
    }

    public bool GetCaretLocation(int vmid, JavaObjectHandle ac, out AccessibleTextRectInfo rectInfo, int index) {
      var result = EntryPoints.GetCaretLocation(vmid, Unwrap(vmid, ac), out rectInfo, index);
      GC.KeepAlive(ac);
      return Succeeded(result);
    }

    public int GetVisibleChildrenCount(int vmid, JavaObjectHandle accessibleContext) {
      var result = EntryPoints.GetVisibleChildrenCount(vmid, Unwrap(vmid, accessibleContext));
      GC.KeepAlive(accessibleContext);
      return result;
    }

    public bool GetVisibleChildren(int vmid, JavaObjectHandle accessibleContext, int startIndex, out VisibleChildrenInfo children) {
      VisibleChildrenInfoNative childrenTemp;
      var result = EntryPoints.GetVisibleChildren(vmid, Unwrap(vmid, accessibleContext), startIndex, out childrenTemp);
      GC.KeepAlive(accessibleContext);
      if (Succeeded(result))
        children = Wrap(vmid, childrenTemp);
      else
        children = default(VisibleChildrenInfo);
      return Succeeded(result);
    }

    public bool GetVersionInfo(int vmid, out AccessBridgeVersionInfo info) {
      var result = EntryPoints.GetVersionInfo(vmid, out info);
      return Succeeded(result);
    }

    #endregion

    #region Wrap/Unwrap structs

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

    private void CopyWrap(int vmid, AccessibleTableCellInfoNative infoSrc, AccessibleTableCellInfo infoDest) {
      infoDest.accessibleContext = Wrap(vmid, infoSrc.accessibleContext);
      infoDest.index = infoSrc.index;
      infoDest.row = infoSrc.row;
      infoDest.column = infoSrc.column;
      infoDest.rowExtent = infoSrc.rowExtent;
      infoDest.columnExtent = infoSrc.columnExtent;
      infoDest.isSelected = infoSrc.isSelected;
    }

    private void CopyUnwrap(int vmid, AccessibleTableCellInfo infoSrc, AccessibleTableCellInfoNative infoDest) {
      infoDest.accessibleContext = Unwrap(vmid, infoSrc.accessibleContext);
      infoDest.index = infoSrc.index;
      infoDest.row = infoSrc.row;
      infoDest.column = infoSrc.column;
      infoDest.rowExtent = infoSrc.rowExtent;
      infoDest.columnExtent = infoSrc.columnExtent;
      infoDest.isSelected = infoSrc.isSelected;
    }

    private void CopyWrap(int vmid, AccessibleTableInfoNative infoSrc, AccessibleTableInfo infoDest) {
      infoDest.caption = Wrap(vmid, infoSrc.caption);
      infoDest.summary = Wrap(vmid, infoSrc.summary);
      infoDest.rowCount = infoSrc.rowCount;
      infoDest.columnCount = infoSrc.columnCount;
      infoDest.accessibleContext = Wrap(vmid, infoSrc.accessibleContext);
      infoDest.accessibleTable = Wrap(vmid, infoSrc.accessibleTable);
    }

    private void CopyUnwrap(int vmid, AccessibleTableInfo infoSrc, AccessibleTableInfoNative infoDest) {
      infoDest.caption = Unwrap(vmid, infoSrc.caption);
      infoDest.summary = Unwrap(vmid, infoSrc.summary);
      infoDest.rowCount = infoSrc.rowCount;
      infoDest.columnCount = infoSrc.columnCount;
      infoDest.accessibleContext = Unwrap(vmid, infoSrc.accessibleContext);
      infoDest.accessibleTable = Unwrap(vmid, infoSrc.accessibleTable);
    }

    #endregion

  }

  /// <summary>
  /// Implementation of platform agnostic events
  /// </summary>
  internal partial class AccessBridgeNativeEvents : IAccessBridgeEvents {
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
          NativeEventsForwarder.PropertyChange += ForwardPropertyChange;
        _propertyChange += value;
      }
      remove{
        _propertyChange -= value;
        if (_propertyChange == null)
          NativeEventsForwarder.PropertyChange -= ForwardPropertyChange;
      }
    }
    public event JavaShutdownEventHandler JavaShutdown {
      add {
        if (_javaShutdown == null)
          NativeEventsForwarder.JavaShutdown += ForwardJavaShutdown;
        _javaShutdown += value;
      }
      remove{
        _javaShutdown -= value;
        if (_javaShutdown == null)
          NativeEventsForwarder.JavaShutdown -= ForwardJavaShutdown;
      }
    }
    public event FocusGainedEventHandler FocusGained {
      add {
        if (_focusGained == null)
          NativeEventsForwarder.FocusGained += ForwardFocusGained;
        _focusGained += value;
      }
      remove{
        _focusGained -= value;
        if (_focusGained == null)
          NativeEventsForwarder.FocusGained -= ForwardFocusGained;
      }
    }
    public event FocusLostEventHandler FocusLost {
      add {
        if (_focusLost == null)
          NativeEventsForwarder.FocusLost += ForwardFocusLost;
        _focusLost += value;
      }
      remove{
        _focusLost -= value;
        if (_focusLost == null)
          NativeEventsForwarder.FocusLost -= ForwardFocusLost;
      }
    }
    public event CaretUpdateEventHandler CaretUpdate {
      add {
        if (_caretUpdate == null)
          NativeEventsForwarder.CaretUpdate += ForwardCaretUpdate;
        _caretUpdate += value;
      }
      remove{
        _caretUpdate -= value;
        if (_caretUpdate == null)
          NativeEventsForwarder.CaretUpdate -= ForwardCaretUpdate;
      }
    }
    public event MouseClickedEventHandler MouseClicked {
      add {
        if (_mouseClicked == null)
          NativeEventsForwarder.MouseClicked += ForwardMouseClicked;
        _mouseClicked += value;
      }
      remove{
        _mouseClicked -= value;
        if (_mouseClicked == null)
          NativeEventsForwarder.MouseClicked -= ForwardMouseClicked;
      }
    }
    public event MouseEnteredEventHandler MouseEntered {
      add {
        if (_mouseEntered == null)
          NativeEventsForwarder.MouseEntered += ForwardMouseEntered;
        _mouseEntered += value;
      }
      remove{
        _mouseEntered -= value;
        if (_mouseEntered == null)
          NativeEventsForwarder.MouseEntered -= ForwardMouseEntered;
      }
    }
    public event MouseExitedEventHandler MouseExited {
      add {
        if (_mouseExited == null)
          NativeEventsForwarder.MouseExited += ForwardMouseExited;
        _mouseExited += value;
      }
      remove{
        _mouseExited -= value;
        if (_mouseExited == null)
          NativeEventsForwarder.MouseExited -= ForwardMouseExited;
      }
    }
    public event MousePressedEventHandler MousePressed {
      add {
        if (_mousePressed == null)
          NativeEventsForwarder.MousePressed += ForwardMousePressed;
        _mousePressed += value;
      }
      remove{
        _mousePressed -= value;
        if (_mousePressed == null)
          NativeEventsForwarder.MousePressed -= ForwardMousePressed;
      }
    }
    public event MouseReleasedEventHandler MouseReleased {
      add {
        if (_mouseReleased == null)
          NativeEventsForwarder.MouseReleased += ForwardMouseReleased;
        _mouseReleased += value;
      }
      remove{
        _mouseReleased -= value;
        if (_mouseReleased == null)
          NativeEventsForwarder.MouseReleased -= ForwardMouseReleased;
      }
    }
    public event MenuCanceledEventHandler MenuCanceled {
      add {
        if (_menuCanceled == null)
          NativeEventsForwarder.MenuCanceled += ForwardMenuCanceled;
        _menuCanceled += value;
      }
      remove{
        _menuCanceled -= value;
        if (_menuCanceled == null)
          NativeEventsForwarder.MenuCanceled -= ForwardMenuCanceled;
      }
    }
    public event MenuDeselectedEventHandler MenuDeselected {
      add {
        if (_menuDeselected == null)
          NativeEventsForwarder.MenuDeselected += ForwardMenuDeselected;
        _menuDeselected += value;
      }
      remove{
        _menuDeselected -= value;
        if (_menuDeselected == null)
          NativeEventsForwarder.MenuDeselected -= ForwardMenuDeselected;
      }
    }
    public event MenuSelectedEventHandler MenuSelected {
      add {
        if (_menuSelected == null)
          NativeEventsForwarder.MenuSelected += ForwardMenuSelected;
        _menuSelected += value;
      }
      remove{
        _menuSelected -= value;
        if (_menuSelected == null)
          NativeEventsForwarder.MenuSelected -= ForwardMenuSelected;
      }
    }
    public event PopupMenuCanceledEventHandler PopupMenuCanceled {
      add {
        if (_popupMenuCanceled == null)
          NativeEventsForwarder.PopupMenuCanceled += ForwardPopupMenuCanceled;
        _popupMenuCanceled += value;
      }
      remove{
        _popupMenuCanceled -= value;
        if (_popupMenuCanceled == null)
          NativeEventsForwarder.PopupMenuCanceled -= ForwardPopupMenuCanceled;
      }
    }
    public event PopupMenuWillBecomeInvisibleEventHandler PopupMenuWillBecomeInvisible {
      add {
        if (_popupMenuWillBecomeInvisible == null)
          NativeEventsForwarder.PopupMenuWillBecomeInvisible += ForwardPopupMenuWillBecomeInvisible;
        _popupMenuWillBecomeInvisible += value;
      }
      remove{
        _popupMenuWillBecomeInvisible -= value;
        if (_popupMenuWillBecomeInvisible == null)
          NativeEventsForwarder.PopupMenuWillBecomeInvisible -= ForwardPopupMenuWillBecomeInvisible;
      }
    }
    public event PopupMenuWillBecomeVisibleEventHandler PopupMenuWillBecomeVisible {
      add {
        if (_popupMenuWillBecomeVisible == null)
          NativeEventsForwarder.PopupMenuWillBecomeVisible += ForwardPopupMenuWillBecomeVisible;
        _popupMenuWillBecomeVisible += value;
      }
      remove{
        _popupMenuWillBecomeVisible -= value;
        if (_popupMenuWillBecomeVisible == null)
          NativeEventsForwarder.PopupMenuWillBecomeVisible -= ForwardPopupMenuWillBecomeVisible;
      }
    }
    public event PropertyNameChangeEventHandler PropertyNameChange {
      add {
        if (_propertyNameChange == null)
          NativeEventsForwarder.PropertyNameChange += ForwardPropertyNameChange;
        _propertyNameChange += value;
      }
      remove{
        _propertyNameChange -= value;
        if (_propertyNameChange == null)
          NativeEventsForwarder.PropertyNameChange -= ForwardPropertyNameChange;
      }
    }
    public event PropertyDescriptionChangeEventHandler PropertyDescriptionChange {
      add {
        if (_propertyDescriptionChange == null)
          NativeEventsForwarder.PropertyDescriptionChange += ForwardPropertyDescriptionChange;
        _propertyDescriptionChange += value;
      }
      remove{
        _propertyDescriptionChange -= value;
        if (_propertyDescriptionChange == null)
          NativeEventsForwarder.PropertyDescriptionChange -= ForwardPropertyDescriptionChange;
      }
    }
    public event PropertyStateChangeEventHandler PropertyStateChange {
      add {
        if (_propertyStateChange == null)
          NativeEventsForwarder.PropertyStateChange += ForwardPropertyStateChange;
        _propertyStateChange += value;
      }
      remove{
        _propertyStateChange -= value;
        if (_propertyStateChange == null)
          NativeEventsForwarder.PropertyStateChange -= ForwardPropertyStateChange;
      }
    }
    public event PropertyValueChangeEventHandler PropertyValueChange {
      add {
        if (_propertyValueChange == null)
          NativeEventsForwarder.PropertyValueChange += ForwardPropertyValueChange;
        _propertyValueChange += value;
      }
      remove{
        _propertyValueChange -= value;
        if (_propertyValueChange == null)
          NativeEventsForwarder.PropertyValueChange -= ForwardPropertyValueChange;
      }
    }
    public event PropertySelectionChangeEventHandler PropertySelectionChange {
      add {
        if (_propertySelectionChange == null)
          NativeEventsForwarder.PropertySelectionChange += ForwardPropertySelectionChange;
        _propertySelectionChange += value;
      }
      remove{
        _propertySelectionChange -= value;
        if (_propertySelectionChange == null)
          NativeEventsForwarder.PropertySelectionChange -= ForwardPropertySelectionChange;
      }
    }
    public event PropertyTextChangeEventHandler PropertyTextChange {
      add {
        if (_propertyTextChange == null)
          NativeEventsForwarder.PropertyTextChange += ForwardPropertyTextChange;
        _propertyTextChange += value;
      }
      remove{
        _propertyTextChange -= value;
        if (_propertyTextChange == null)
          NativeEventsForwarder.PropertyTextChange -= ForwardPropertyTextChange;
      }
    }
    public event PropertyCaretChangeEventHandler PropertyCaretChange {
      add {
        if (_propertyCaretChange == null)
          NativeEventsForwarder.PropertyCaretChange += ForwardPropertyCaretChange;
        _propertyCaretChange += value;
      }
      remove{
        _propertyCaretChange -= value;
        if (_propertyCaretChange == null)
          NativeEventsForwarder.PropertyCaretChange -= ForwardPropertyCaretChange;
      }
    }
    public event PropertyVisibleDataChangeEventHandler PropertyVisibleDataChange {
      add {
        if (_propertyVisibleDataChange == null)
          NativeEventsForwarder.PropertyVisibleDataChange += ForwardPropertyVisibleDataChange;
        _propertyVisibleDataChange += value;
      }
      remove{
        _propertyVisibleDataChange -= value;
        if (_propertyVisibleDataChange == null)
          NativeEventsForwarder.PropertyVisibleDataChange -= ForwardPropertyVisibleDataChange;
      }
    }
    public event PropertyChildChangeEventHandler PropertyChildChange {
      add {
        if (_propertyChildChange == null)
          NativeEventsForwarder.PropertyChildChange += ForwardPropertyChildChange;
        _propertyChildChange += value;
      }
      remove{
        _propertyChildChange -= value;
        if (_propertyChildChange == null)
          NativeEventsForwarder.PropertyChildChange -= ForwardPropertyChildChange;
      }
    }
    public event PropertyActiveDescendentChangeEventHandler PropertyActiveDescendentChange {
      add {
        if (_propertyActiveDescendentChange == null)
          NativeEventsForwarder.PropertyActiveDescendentChange += ForwardPropertyActiveDescendentChange;
        _propertyActiveDescendentChange += value;
      }
      remove{
        _propertyActiveDescendentChange -= value;
        if (_propertyActiveDescendentChange == null)
          NativeEventsForwarder.PropertyActiveDescendentChange -= ForwardPropertyActiveDescendentChange;
      }
    }
    public event PropertyTableModelChangeEventHandler PropertyTableModelChange {
      add {
        if (_propertyTableModelChange == null)
          NativeEventsForwarder.PropertyTableModelChange += ForwardPropertyTableModelChange;
        _propertyTableModelChange += value;
      }
      remove{
        _propertyTableModelChange -= value;
        if (_propertyTableModelChange == null)
          NativeEventsForwarder.PropertyTableModelChange -= ForwardPropertyTableModelChange;
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
      NativeEventsForwarder.PropertyChange -= ForwardPropertyChange;
      NativeEventsForwarder.JavaShutdown -= ForwardJavaShutdown;
      NativeEventsForwarder.FocusGained -= ForwardFocusGained;
      NativeEventsForwarder.FocusLost -= ForwardFocusLost;
      NativeEventsForwarder.CaretUpdate -= ForwardCaretUpdate;
      NativeEventsForwarder.MouseClicked -= ForwardMouseClicked;
      NativeEventsForwarder.MouseEntered -= ForwardMouseEntered;
      NativeEventsForwarder.MouseExited -= ForwardMouseExited;
      NativeEventsForwarder.MousePressed -= ForwardMousePressed;
      NativeEventsForwarder.MouseReleased -= ForwardMouseReleased;
      NativeEventsForwarder.MenuCanceled -= ForwardMenuCanceled;
      NativeEventsForwarder.MenuDeselected -= ForwardMenuDeselected;
      NativeEventsForwarder.MenuSelected -= ForwardMenuSelected;
      NativeEventsForwarder.PopupMenuCanceled -= ForwardPopupMenuCanceled;
      NativeEventsForwarder.PopupMenuWillBecomeInvisible -= ForwardPopupMenuWillBecomeInvisible;
      NativeEventsForwarder.PopupMenuWillBecomeVisible -= ForwardPopupMenuWillBecomeVisible;
      NativeEventsForwarder.PropertyNameChange -= ForwardPropertyNameChange;
      NativeEventsForwarder.PropertyDescriptionChange -= ForwardPropertyDescriptionChange;
      NativeEventsForwarder.PropertyStateChange -= ForwardPropertyStateChange;
      NativeEventsForwarder.PropertyValueChange -= ForwardPropertyValueChange;
      NativeEventsForwarder.PropertySelectionChange -= ForwardPropertySelectionChange;
      NativeEventsForwarder.PropertyTextChange -= ForwardPropertyTextChange;
      NativeEventsForwarder.PropertyCaretChange -= ForwardPropertyCaretChange;
      NativeEventsForwarder.PropertyVisibleDataChange -= ForwardPropertyVisibleDataChange;
      NativeEventsForwarder.PropertyChildChange -= ForwardPropertyChildChange;
      NativeEventsForwarder.PropertyActiveDescendentChange -= ForwardPropertyActiveDescendentChange;
      NativeEventsForwarder.PropertyTableModelChange -= ForwardPropertyTableModelChange;
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
  internal class AccessBridgeEntryPoints {
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
    public delegate BOOL GetAccessibleContextInfoFP(int vmid, JOBJECT64 ac, [Out]AccessibleContextInfo info);
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
    public delegate BOOL GetAccessibleKeyBindingsFP(int vmid, JOBJECT64 accessibleContext, out AccessibleKeyBindings keyBindings);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleIconsFP(int vmid, JOBJECT64 accessibleContext, out AccessibleIcons icons);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleActionsFP(int vmid, JOBJECT64 accessibleContext, [Out]AccessibleActions actions);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL DoAccessibleActionsFP(int vmid, JOBJECT64 accessibleContext, ref AccessibleActionsToDo actionsToDo, out int failure);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextInfoFP(int vmid, JOBJECT64 at, out AccessibleTextInfo textInfo, int x, int y);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextItemsFP(int vmid, JOBJECT64 at, out AccessibleTextItemsInfo textItems, int index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextSelectionInfoFP(int vmid, JOBJECT64 at, out AccessibleTextSelectionInfo textSelection);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextAttributesFP(int vmid, JOBJECT64 at, int index, [Out]AccessibleTextAttributesInfo attributes);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextRectFP(int vmid, JOBJECT64 at, out AccessibleTextRectInfo rectInfo, int index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextLineBoundsFP(int vmid, JOBJECT64 at, int index, out int startIndex, out int endIndex);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextRangeFP(int vmid, JOBJECT64 at, int start, int end, [Out]char[] text, short len);
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
    public delegate BOOL GetAccessibleTableInfoFP(int vmid, JOBJECT64 ac, [Out]AccessibleTableInfoNative tableInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableCellInfoFP(int vmid, JOBJECT64 at, int row, int column, [Out]AccessibleTableCellInfoNative tableCellInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableRowHeaderFP(int vmid, JOBJECT64 acParent, [Out]AccessibleTableInfoNative tableInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableColumnHeaderFP(int vmid, JOBJECT64 acParent, [Out]AccessibleTableInfoNative tableInfo);
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
    public delegate BOOL GetTextAttributesInRangeFP(int vmid, JOBJECT64 accessibleContext, int startIndex, int endIndex, [Out]AccessibleTextAttributesInfo attributes, out short len);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetCaretLocationFP(int vmid, JOBJECT64 ac, out AccessibleTextRectInfo rectInfo, int index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetVisibleChildrenCountFP(int vmid, JOBJECT64 accessibleContext);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetVisibleChildrenFP(int vmid, JOBJECT64 accessibleContext, int startIndex, out VisibleChildrenInfoNative children);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetVersionInfoFP(int vmid, out AccessBridgeVersionInfo info);
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
  /// Native library event handlers implementation
  /// </summary>
  internal partial class AccessBridgeNativeEventsForwarder {
    #region Event fields
    private AccessBridgeEntryPoints.PropertyChangeEventHandler _propertyChange;
    private AccessBridgeEntryPoints.JavaShutdownEventHandler _javaShutdown;
    private AccessBridgeEntryPoints.FocusGainedEventHandler _focusGained;
    private AccessBridgeEntryPoints.FocusLostEventHandler _focusLost;
    private AccessBridgeEntryPoints.CaretUpdateEventHandler _caretUpdate;
    private AccessBridgeEntryPoints.MouseClickedEventHandler _mouseClicked;
    private AccessBridgeEntryPoints.MouseEnteredEventHandler _mouseEntered;
    private AccessBridgeEntryPoints.MouseExitedEventHandler _mouseExited;
    private AccessBridgeEntryPoints.MousePressedEventHandler _mousePressed;
    private AccessBridgeEntryPoints.MouseReleasedEventHandler _mouseReleased;
    private AccessBridgeEntryPoints.MenuCanceledEventHandler _menuCanceled;
    private AccessBridgeEntryPoints.MenuDeselectedEventHandler _menuDeselected;
    private AccessBridgeEntryPoints.MenuSelectedEventHandler _menuSelected;
    private AccessBridgeEntryPoints.PopupMenuCanceledEventHandler _popupMenuCanceled;
    private AccessBridgeEntryPoints.PopupMenuWillBecomeInvisibleEventHandler _popupMenuWillBecomeInvisible;
    private AccessBridgeEntryPoints.PopupMenuWillBecomeVisibleEventHandler _popupMenuWillBecomeVisible;
    private AccessBridgeEntryPoints.PropertyNameChangeEventHandler _propertyNameChange;
    private AccessBridgeEntryPoints.PropertyDescriptionChangeEventHandler _propertyDescriptionChange;
    private AccessBridgeEntryPoints.PropertyStateChangeEventHandler _propertyStateChange;
    private AccessBridgeEntryPoints.PropertyValueChangeEventHandler _propertyValueChange;
    private AccessBridgeEntryPoints.PropertySelectionChangeEventHandler _propertySelectionChange;
    private AccessBridgeEntryPoints.PropertyTextChangeEventHandler _propertyTextChange;
    private AccessBridgeEntryPoints.PropertyCaretChangeEventHandler _propertyCaretChange;
    private AccessBridgeEntryPoints.PropertyVisibleDataChangeEventHandler _propertyVisibleDataChange;
    private AccessBridgeEntryPoints.PropertyChildChangeEventHandler _propertyChildChange;
    private AccessBridgeEntryPoints.PropertyActiveDescendentChangeEventHandler _propertyActiveDescendentChange;
    private AccessBridgeEntryPoints.PropertyTableModelChangeEventHandler _propertyTableModelChange;
    #endregion

    #region Event delegate fields
    private AccessBridgeEntryPoints.PropertyChangeEventHandler _onPropertyChangeKeepAliveDelegate;
    private AccessBridgeEntryPoints.JavaShutdownEventHandler _onJavaShutdownKeepAliveDelegate;
    private AccessBridgeEntryPoints.FocusGainedEventHandler _onFocusGainedKeepAliveDelegate;
    private AccessBridgeEntryPoints.FocusLostEventHandler _onFocusLostKeepAliveDelegate;
    private AccessBridgeEntryPoints.CaretUpdateEventHandler _onCaretUpdateKeepAliveDelegate;
    private AccessBridgeEntryPoints.MouseClickedEventHandler _onMouseClickedKeepAliveDelegate;
    private AccessBridgeEntryPoints.MouseEnteredEventHandler _onMouseEnteredKeepAliveDelegate;
    private AccessBridgeEntryPoints.MouseExitedEventHandler _onMouseExitedKeepAliveDelegate;
    private AccessBridgeEntryPoints.MousePressedEventHandler _onMousePressedKeepAliveDelegate;
    private AccessBridgeEntryPoints.MouseReleasedEventHandler _onMouseReleasedKeepAliveDelegate;
    private AccessBridgeEntryPoints.MenuCanceledEventHandler _onMenuCanceledKeepAliveDelegate;
    private AccessBridgeEntryPoints.MenuDeselectedEventHandler _onMenuDeselectedKeepAliveDelegate;
    private AccessBridgeEntryPoints.MenuSelectedEventHandler _onMenuSelectedKeepAliveDelegate;
    private AccessBridgeEntryPoints.PopupMenuCanceledEventHandler _onPopupMenuCanceledKeepAliveDelegate;
    private AccessBridgeEntryPoints.PopupMenuWillBecomeInvisibleEventHandler _onPopupMenuWillBecomeInvisibleKeepAliveDelegate;
    private AccessBridgeEntryPoints.PopupMenuWillBecomeVisibleEventHandler _onPopupMenuWillBecomeVisibleKeepAliveDelegate;
    private AccessBridgeEntryPoints.PropertyNameChangeEventHandler _onPropertyNameChangeKeepAliveDelegate;
    private AccessBridgeEntryPoints.PropertyDescriptionChangeEventHandler _onPropertyDescriptionChangeKeepAliveDelegate;
    private AccessBridgeEntryPoints.PropertyStateChangeEventHandler _onPropertyStateChangeKeepAliveDelegate;
    private AccessBridgeEntryPoints.PropertyValueChangeEventHandler _onPropertyValueChangeKeepAliveDelegate;
    private AccessBridgeEntryPoints.PropertySelectionChangeEventHandler _onPropertySelectionChangeKeepAliveDelegate;
    private AccessBridgeEntryPoints.PropertyTextChangeEventHandler _onPropertyTextChangeKeepAliveDelegate;
    private AccessBridgeEntryPoints.PropertyCaretChangeEventHandler _onPropertyCaretChangeKeepAliveDelegate;
    private AccessBridgeEntryPoints.PropertyVisibleDataChangeEventHandler _onPropertyVisibleDataChangeKeepAliveDelegate;
    private AccessBridgeEntryPoints.PropertyChildChangeEventHandler _onPropertyChildChangeKeepAliveDelegate;
    private AccessBridgeEntryPoints.PropertyActiveDescendentChangeEventHandler _onPropertyActiveDescendentChangeKeepAliveDelegate;
    private AccessBridgeEntryPoints.PropertyTableModelChangeEventHandler _onPropertyTableModelChangeKeepAliveDelegate;
    #endregion

    #region Event properties
    public event AccessBridgeEntryPoints.PropertyChangeEventHandler PropertyChange {
      add {
        if (_propertyChange == null) {
          _onPropertyChangeKeepAliveDelegate = OnPropertyChange;
          EntryPoints.SetPropertyChange(_onPropertyChangeKeepAliveDelegate);
        }
        _propertyChange += value;
      }
      remove{
        _propertyChange -= value;
        if (_propertyChange == null) {
          EntryPoints.SetPropertyChange(null);
          _onPropertyChangeKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.JavaShutdownEventHandler JavaShutdown {
      add {
        if (_javaShutdown == null) {
          _onJavaShutdownKeepAliveDelegate = OnJavaShutdown;
          EntryPoints.SetJavaShutdown(_onJavaShutdownKeepAliveDelegate);
        }
        _javaShutdown += value;
      }
      remove{
        _javaShutdown -= value;
        if (_javaShutdown == null) {
          EntryPoints.SetJavaShutdown(null);
          _onJavaShutdownKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.FocusGainedEventHandler FocusGained {
      add {
        if (_focusGained == null) {
          _onFocusGainedKeepAliveDelegate = OnFocusGained;
          EntryPoints.SetFocusGained(_onFocusGainedKeepAliveDelegate);
        }
        _focusGained += value;
      }
      remove{
        _focusGained -= value;
        if (_focusGained == null) {
          EntryPoints.SetFocusGained(null);
          _onFocusGainedKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.FocusLostEventHandler FocusLost {
      add {
        if (_focusLost == null) {
          _onFocusLostKeepAliveDelegate = OnFocusLost;
          EntryPoints.SetFocusLost(_onFocusLostKeepAliveDelegate);
        }
        _focusLost += value;
      }
      remove{
        _focusLost -= value;
        if (_focusLost == null) {
          EntryPoints.SetFocusLost(null);
          _onFocusLostKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.CaretUpdateEventHandler CaretUpdate {
      add {
        if (_caretUpdate == null) {
          _onCaretUpdateKeepAliveDelegate = OnCaretUpdate;
          EntryPoints.SetCaretUpdate(_onCaretUpdateKeepAliveDelegate);
        }
        _caretUpdate += value;
      }
      remove{
        _caretUpdate -= value;
        if (_caretUpdate == null) {
          EntryPoints.SetCaretUpdate(null);
          _onCaretUpdateKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.MouseClickedEventHandler MouseClicked {
      add {
        if (_mouseClicked == null) {
          _onMouseClickedKeepAliveDelegate = OnMouseClicked;
          EntryPoints.SetMouseClicked(_onMouseClickedKeepAliveDelegate);
        }
        _mouseClicked += value;
      }
      remove{
        _mouseClicked -= value;
        if (_mouseClicked == null) {
          EntryPoints.SetMouseClicked(null);
          _onMouseClickedKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.MouseEnteredEventHandler MouseEntered {
      add {
        if (_mouseEntered == null) {
          _onMouseEnteredKeepAliveDelegate = OnMouseEntered;
          EntryPoints.SetMouseEntered(_onMouseEnteredKeepAliveDelegate);
        }
        _mouseEntered += value;
      }
      remove{
        _mouseEntered -= value;
        if (_mouseEntered == null) {
          EntryPoints.SetMouseEntered(null);
          _onMouseEnteredKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.MouseExitedEventHandler MouseExited {
      add {
        if (_mouseExited == null) {
          _onMouseExitedKeepAliveDelegate = OnMouseExited;
          EntryPoints.SetMouseExited(_onMouseExitedKeepAliveDelegate);
        }
        _mouseExited += value;
      }
      remove{
        _mouseExited -= value;
        if (_mouseExited == null) {
          EntryPoints.SetMouseExited(null);
          _onMouseExitedKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.MousePressedEventHandler MousePressed {
      add {
        if (_mousePressed == null) {
          _onMousePressedKeepAliveDelegate = OnMousePressed;
          EntryPoints.SetMousePressed(_onMousePressedKeepAliveDelegate);
        }
        _mousePressed += value;
      }
      remove{
        _mousePressed -= value;
        if (_mousePressed == null) {
          EntryPoints.SetMousePressed(null);
          _onMousePressedKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.MouseReleasedEventHandler MouseReleased {
      add {
        if (_mouseReleased == null) {
          _onMouseReleasedKeepAliveDelegate = OnMouseReleased;
          EntryPoints.SetMouseReleased(_onMouseReleasedKeepAliveDelegate);
        }
        _mouseReleased += value;
      }
      remove{
        _mouseReleased -= value;
        if (_mouseReleased == null) {
          EntryPoints.SetMouseReleased(null);
          _onMouseReleasedKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.MenuCanceledEventHandler MenuCanceled {
      add {
        if (_menuCanceled == null) {
          _onMenuCanceledKeepAliveDelegate = OnMenuCanceled;
          EntryPoints.SetMenuCanceled(_onMenuCanceledKeepAliveDelegate);
        }
        _menuCanceled += value;
      }
      remove{
        _menuCanceled -= value;
        if (_menuCanceled == null) {
          EntryPoints.SetMenuCanceled(null);
          _onMenuCanceledKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.MenuDeselectedEventHandler MenuDeselected {
      add {
        if (_menuDeselected == null) {
          _onMenuDeselectedKeepAliveDelegate = OnMenuDeselected;
          EntryPoints.SetMenuDeselected(_onMenuDeselectedKeepAliveDelegate);
        }
        _menuDeselected += value;
      }
      remove{
        _menuDeselected -= value;
        if (_menuDeselected == null) {
          EntryPoints.SetMenuDeselected(null);
          _onMenuDeselectedKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.MenuSelectedEventHandler MenuSelected {
      add {
        if (_menuSelected == null) {
          _onMenuSelectedKeepAliveDelegate = OnMenuSelected;
          EntryPoints.SetMenuSelected(_onMenuSelectedKeepAliveDelegate);
        }
        _menuSelected += value;
      }
      remove{
        _menuSelected -= value;
        if (_menuSelected == null) {
          EntryPoints.SetMenuSelected(null);
          _onMenuSelectedKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.PopupMenuCanceledEventHandler PopupMenuCanceled {
      add {
        if (_popupMenuCanceled == null) {
          _onPopupMenuCanceledKeepAliveDelegate = OnPopupMenuCanceled;
          EntryPoints.SetPopupMenuCanceled(_onPopupMenuCanceledKeepAliveDelegate);
        }
        _popupMenuCanceled += value;
      }
      remove{
        _popupMenuCanceled -= value;
        if (_popupMenuCanceled == null) {
          EntryPoints.SetPopupMenuCanceled(null);
          _onPopupMenuCanceledKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.PopupMenuWillBecomeInvisibleEventHandler PopupMenuWillBecomeInvisible {
      add {
        if (_popupMenuWillBecomeInvisible == null) {
          _onPopupMenuWillBecomeInvisibleKeepAliveDelegate = OnPopupMenuWillBecomeInvisible;
          EntryPoints.SetPopupMenuWillBecomeInvisible(_onPopupMenuWillBecomeInvisibleKeepAliveDelegate);
        }
        _popupMenuWillBecomeInvisible += value;
      }
      remove{
        _popupMenuWillBecomeInvisible -= value;
        if (_popupMenuWillBecomeInvisible == null) {
          EntryPoints.SetPopupMenuWillBecomeInvisible(null);
          _onPopupMenuWillBecomeInvisibleKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.PopupMenuWillBecomeVisibleEventHandler PopupMenuWillBecomeVisible {
      add {
        if (_popupMenuWillBecomeVisible == null) {
          _onPopupMenuWillBecomeVisibleKeepAliveDelegate = OnPopupMenuWillBecomeVisible;
          EntryPoints.SetPopupMenuWillBecomeVisible(_onPopupMenuWillBecomeVisibleKeepAliveDelegate);
        }
        _popupMenuWillBecomeVisible += value;
      }
      remove{
        _popupMenuWillBecomeVisible -= value;
        if (_popupMenuWillBecomeVisible == null) {
          EntryPoints.SetPopupMenuWillBecomeVisible(null);
          _onPopupMenuWillBecomeVisibleKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.PropertyNameChangeEventHandler PropertyNameChange {
      add {
        if (_propertyNameChange == null) {
          _onPropertyNameChangeKeepAliveDelegate = OnPropertyNameChange;
          EntryPoints.SetPropertyNameChange(_onPropertyNameChangeKeepAliveDelegate);
        }
        _propertyNameChange += value;
      }
      remove{
        _propertyNameChange -= value;
        if (_propertyNameChange == null) {
          EntryPoints.SetPropertyNameChange(null);
          _onPropertyNameChangeKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.PropertyDescriptionChangeEventHandler PropertyDescriptionChange {
      add {
        if (_propertyDescriptionChange == null) {
          _onPropertyDescriptionChangeKeepAliveDelegate = OnPropertyDescriptionChange;
          EntryPoints.SetPropertyDescriptionChange(_onPropertyDescriptionChangeKeepAliveDelegate);
        }
        _propertyDescriptionChange += value;
      }
      remove{
        _propertyDescriptionChange -= value;
        if (_propertyDescriptionChange == null) {
          EntryPoints.SetPropertyDescriptionChange(null);
          _onPropertyDescriptionChangeKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.PropertyStateChangeEventHandler PropertyStateChange {
      add {
        if (_propertyStateChange == null) {
          _onPropertyStateChangeKeepAliveDelegate = OnPropertyStateChange;
          EntryPoints.SetPropertyStateChange(_onPropertyStateChangeKeepAliveDelegate);
        }
        _propertyStateChange += value;
      }
      remove{
        _propertyStateChange -= value;
        if (_propertyStateChange == null) {
          EntryPoints.SetPropertyStateChange(null);
          _onPropertyStateChangeKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.PropertyValueChangeEventHandler PropertyValueChange {
      add {
        if (_propertyValueChange == null) {
          _onPropertyValueChangeKeepAliveDelegate = OnPropertyValueChange;
          EntryPoints.SetPropertyValueChange(_onPropertyValueChangeKeepAliveDelegate);
        }
        _propertyValueChange += value;
      }
      remove{
        _propertyValueChange -= value;
        if (_propertyValueChange == null) {
          EntryPoints.SetPropertyValueChange(null);
          _onPropertyValueChangeKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.PropertySelectionChangeEventHandler PropertySelectionChange {
      add {
        if (_propertySelectionChange == null) {
          _onPropertySelectionChangeKeepAliveDelegate = OnPropertySelectionChange;
          EntryPoints.SetPropertySelectionChange(_onPropertySelectionChangeKeepAliveDelegate);
        }
        _propertySelectionChange += value;
      }
      remove{
        _propertySelectionChange -= value;
        if (_propertySelectionChange == null) {
          EntryPoints.SetPropertySelectionChange(null);
          _onPropertySelectionChangeKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.PropertyTextChangeEventHandler PropertyTextChange {
      add {
        if (_propertyTextChange == null) {
          _onPropertyTextChangeKeepAliveDelegate = OnPropertyTextChange;
          EntryPoints.SetPropertyTextChange(_onPropertyTextChangeKeepAliveDelegate);
        }
        _propertyTextChange += value;
      }
      remove{
        _propertyTextChange -= value;
        if (_propertyTextChange == null) {
          EntryPoints.SetPropertyTextChange(null);
          _onPropertyTextChangeKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.PropertyCaretChangeEventHandler PropertyCaretChange {
      add {
        if (_propertyCaretChange == null) {
          _onPropertyCaretChangeKeepAliveDelegate = OnPropertyCaretChange;
          EntryPoints.SetPropertyCaretChange(_onPropertyCaretChangeKeepAliveDelegate);
        }
        _propertyCaretChange += value;
      }
      remove{
        _propertyCaretChange -= value;
        if (_propertyCaretChange == null) {
          EntryPoints.SetPropertyCaretChange(null);
          _onPropertyCaretChangeKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.PropertyVisibleDataChangeEventHandler PropertyVisibleDataChange {
      add {
        if (_propertyVisibleDataChange == null) {
          _onPropertyVisibleDataChangeKeepAliveDelegate = OnPropertyVisibleDataChange;
          EntryPoints.SetPropertyVisibleDataChange(_onPropertyVisibleDataChangeKeepAliveDelegate);
        }
        _propertyVisibleDataChange += value;
      }
      remove{
        _propertyVisibleDataChange -= value;
        if (_propertyVisibleDataChange == null) {
          EntryPoints.SetPropertyVisibleDataChange(null);
          _onPropertyVisibleDataChangeKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.PropertyChildChangeEventHandler PropertyChildChange {
      add {
        if (_propertyChildChange == null) {
          _onPropertyChildChangeKeepAliveDelegate = OnPropertyChildChange;
          EntryPoints.SetPropertyChildChange(_onPropertyChildChangeKeepAliveDelegate);
        }
        _propertyChildChange += value;
      }
      remove{
        _propertyChildChange -= value;
        if (_propertyChildChange == null) {
          EntryPoints.SetPropertyChildChange(null);
          _onPropertyChildChangeKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.PropertyActiveDescendentChangeEventHandler PropertyActiveDescendentChange {
      add {
        if (_propertyActiveDescendentChange == null) {
          _onPropertyActiveDescendentChangeKeepAliveDelegate = OnPropertyActiveDescendentChange;
          EntryPoints.SetPropertyActiveDescendentChange(_onPropertyActiveDescendentChangeKeepAliveDelegate);
        }
        _propertyActiveDescendentChange += value;
      }
      remove{
        _propertyActiveDescendentChange -= value;
        if (_propertyActiveDescendentChange == null) {
          EntryPoints.SetPropertyActiveDescendentChange(null);
          _onPropertyActiveDescendentChangeKeepAliveDelegate = null;
        }
      }
    }
    public event AccessBridgeEntryPoints.PropertyTableModelChangeEventHandler PropertyTableModelChange {
      add {
        if (_propertyTableModelChange == null) {
          _onPropertyTableModelChangeKeepAliveDelegate = OnPropertyTableModelChange;
          EntryPoints.SetPropertyTableModelChange(_onPropertyTableModelChangeKeepAliveDelegate);
        }
        _propertyTableModelChange += value;
      }
      remove{
        _propertyTableModelChange -= value;
        if (_propertyTableModelChange == null) {
          EntryPoints.SetPropertyTableModelChange(null);
          _onPropertyTableModelChangeKeepAliveDelegate = null;
        }
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

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  internal struct AccessibleHyperlinkInfoNative {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string text;
    public int startIndex;
    public int endIndex;
    public JOBJECT64 accessibleHyperlink;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  internal struct AccessibleHypertextInfoNative {
    public int linkCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public AccessibleHyperlinkInfoNative[] links;
    public JOBJECT64 accessibleHypertext;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  internal struct AccessibleRelationInfoNative {
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string key;
    public int targetCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
    public JOBJECT64[] targets;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  internal struct AccessibleRelationSetInfoNative {
    public int relationCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public AccessibleRelationInfoNative[] relations;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  internal struct VisibleChildrenInfoNative {
    public int returnedChildrenCount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public JOBJECT64[] children;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  internal class AccessibleTableCellInfoNative {
    public JOBJECT64 accessibleContext;
    public int index;
    public int row;
    public int column;
    public int rowExtent;
    public int columnExtent;
    public byte isSelected;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  internal class AccessibleTableInfoNative {
    public JOBJECT64 caption;
    public JOBJECT64 summary;
    public int rowCount;
    public int columnCount;
    public JOBJECT64 accessibleContext;
    public JOBJECT64 accessibleTable;
  }

}
