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
using AccessBridgeExplorer.WindowsAccessBridge.NativeStructures;
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
  public class AccessBridgeFunctionsNative : IAccessBridgeFunctions {
    #region Delegate definitions
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void Windows_runFP();

    // Note: We don't expose this function, as we use JavaObjectHandle to deal
    // with relasing Java objects references.
    //[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    //public delegate void ReleaseJavaObjectFP(int vmID, Java_Object @object);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL IsJavaWindowFP(HWND window);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL IsSameObjectFP(int vmID, JOBJECT64 obj1, JOBJECT64 obj2);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleContextFromHWNDFP(HWND window, out int vmID, out AccessibleContext ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate HWND GetHWNDFromAccessibleContextFP(int vmID, JOBJECT64 ac);

    /// <summary>
    /// Returns the deepest accessible context at screen location (x,y) using
    /// <paramref name="acParent"/> as the root of the search tree. The returned
    /// <see cref="AccessibleContext"/> maybe be equal to <paramref
    /// name="acParent"/> in case there is no child at that location. Returns
    /// <code>false</code> in case of serious error.
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleContextAtFP(int vmID, JOBJECT64 acParent, jint x, jint y, out AccessibleContext ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleContextWithFocusFP(HWND window, out int vmID, out AccessibleContext ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleContextInfoFP(int vmID, JOBJECT64 ac, out AccessibleContextInfo info);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate AccessibleContext GetAccessibleChildFromContextFP(int vmID, JOBJECT64 ac, jint i);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate AccessibleContext GetAccessibleParentFromContextFP(int vmID, JOBJECT64 ac);

    /* AccessibleRelationSet */
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleRelationSetFP(int vmID, JOBJECT64 accessibleContext, out AccessibleRelationSetInfo relationSetInfo);


    /* AccessibleHypertext */
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleHypertextFP(int vmID, JOBJECT64 accessibleContext, out AccessibleHypertextInfo hypertextInfo);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL ActivateAccessibleHyperlinkFP(int vmID, JOBJECT64 accessibleContext, JOBJECT64 accessibleHyperlink);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate jint GetAccessibleHyperlinkCountFP(int vmID, JOBJECT64 accessibleContext);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleHypertextExtFP(int vmID, JOBJECT64 accessibleContext, jint nStartIndex, out AccessibleHypertextInfo hypertextInfo);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate jint GetAccessibleHypertextLinkIndexFP(int vmID, JOBJECT64 hypertext, jint nIndex);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleHyperlinkFP(int vmID, JOBJECT64 hypertext, jint nIndex, out AccessibleHyperlinkInfo hyperlinkInfo);


    /* Accessible KeyBindings, Icons and Actions */
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleKeyBindingsFP(int vmID, JOBJECT64 accessibleContext, out AccessibleKeyBindings keyBindings);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleIconsFP(int vmID, JOBJECT64 accessibleContext, out AccessibleIcons icons);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleActionsFP(int vmID, JOBJECT64 accessibleContext, [Out]AccessibleActions actions);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL DoAccessibleActionsFP(int vmID, JOBJECT64 accessibleContext, ref AccessibleActionsToDo actionsToDo, out jint failure);

    /* AccessibleText */
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextInfoFP(int vmID, JOBJECT64 at, out AccessibleTextInfo textInfo, jint x, jint y);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextItemsFP(int vmID, JOBJECT64 at, out AccessibleTextItemsInfo textItems, jint index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextSelectionInfoFP(int vmID, JOBJECT64 at, out AccessibleTextSelectionInfo textSelection);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextAttributesFP(int vmID, JOBJECT64 at, jint index, out AccessibleTextAttributesInfo attributes);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextRectFP(int vmID, JOBJECT64 at, out AccessibleTextRectInfo rectInfo, jint index);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextLineBoundsFP(int vmID, JOBJECT64 at, jint index, out jint startIndex, out jint endIndex);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTextRangeFP(int vmID, JOBJECT64 at, jint start, jint end, StringBuilder text, short len);

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

    /* begin AccessibleTable */
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableInfoFP(int vmID, JOBJECT64 ac, out AccessibleTableInfo tableInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableCellInfoFP(int vmID, JOBJECT64 accessibleTable,
    jint row, jint column, out AccessibleTableCellInfo tableCellInfo);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableRowHeaderFP(int vmID, JOBJECT64 acParent, out AccessibleTableInfo tableInfo);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableColumnHeaderFP(int vmID, JOBJECT64 acParent, out AccessibleTableInfo tableInfo);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate AccessibleContext GetAccessibleTableRowDescriptionFP(int vmID, JOBJECT64 acParent, jint row);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate AccessibleContext GetAccessibleTableColumnDescriptionFP(int vmID, JOBJECT64 acParent, jint column);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate jint GetAccessibleTableRowSelectionCountFP(int vmID, JOBJECT64 table);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL IsAccessibleTableRowSelectedFP(int vmID, JOBJECT64 table, jint row);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableRowSelectionsFP(int vmID, JOBJECT64 table, jint count,
      [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] jint[] selections);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate jint GetAccessibleTableColumnSelectionCountFP(int vmID, JOBJECT64 table);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL IsAccessibleTableColumnSelectedFP(int vmID, JOBJECT64 table, jint column);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetAccessibleTableColumnSelectionsFP(int vmID, JOBJECT64 table, jint count,
      [Out, MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)]jint[] selections);

    /// <summary>
    /// Return the row number for a cell at a given index
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate jint GetAccessibleTableRowFP(int vmID, JOBJECT64 table, jint index);
    /// <summary>
    /// Return the column number for a cell at a given index
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate jint GetAccessibleTableColumnFP(int vmID, JOBJECT64 table, jint index);
    /// <summary>
    /// Return the index of a cell at a given row and column
    /// </summary>
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate jint GetAccessibleTableIndexFP(int vmID, JOBJECT64 table, jint row, jint column);
    /* end AccessibleTable */

    /* Additional utility methods */
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL SetTextContentsFP(int vmID, AccessibleContext ac, string text);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate AccessibleContext GetParentWithRoleFP(int vmID, JOBJECT64 ac, [MarshalAs(UnmanagedType.LPWStr)]string role);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate AccessibleContext GetParentWithRoleElseRootFP(int vmID, JOBJECT64 ac, [MarshalAs(UnmanagedType.LPWStr)]string role);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate AccessibleContext GetTopLevelObjectFP(int vmID, JOBJECT64 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetObjectDepthFP(int vmID, JOBJECT64 ac);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate AccessibleContext GetActiveDescendentFP(int vmID, JOBJECT64 ac);


    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetVirtualAccessibleNameFP(int vmID, JOBJECT64 ac, StringBuilder name, int len);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetTextAttributesInRangeFP(int vmID, JOBJECT64 accessibleContext, int startIndex, int endIndex, out AccessibleTextAttributesInfo attributes, out short len);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetCaretLocationFP(int vmID, JOBJECT64 ac, out AccessibleTextRectInfo rectInfo, jint index);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate int GetVisibleChildrenCountFP(int vmID, JOBJECT64 accessibleContext);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetVisibleChildrenFP(int vmID, JOBJECT64 accessibleContext, int startIndex, out VisibleChildrenInfo children);

    /* end additional utility methods */

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    public delegate BOOL GetVersionInfoFP(int vmID, out AccessBridgeVersionInfo info);
    #endregion

    public IsSameObjectFP IsSameObject { get; set; }

    #region Window routines
    public Windows_runFP Windows_run { get; set; }
    public IsJavaWindowFP IsJavaWindow { get; set; }
    public GetAccessibleContextFromHWNDFP GetAccessibleContextFromHWND { get; set; }
    public GetHWNDFromAccessibleContextFP GetHWNDFromAccessibleContext { get; set; }
    #endregion

    #region General routines
    /// <summary>
    /// Note: Don't use this method, use <see cref="JavaObjectHandle"/> instead.
    /// This method needs to be called for every accessible context
    /// retrieved via a "GetXxx" function.
    /// </summary>
    //public ReleaseJavaObjectFP ReleaseJavaObject { get; set; }
    public GetVersionInfoFP GetVersionInfo { get; set; }
    #endregion

    #region Accessible Context routines
    public GetAccessibleContextAtFP GetAccessibleContextAt { get; set; }
    public GetAccessibleContextWithFocusFP GetAccessibleContextWithFocus { get; set; }
    public GetAccessibleContextInfoFP GetAccessibleContextInfo { get; set; }
    public GetAccessibleChildFromContextFP GetAccessibleChildFromContext { get; set; }
    public GetAccessibleParentFromContextFP GetAccessibleParentFromContext { get; set; }
    #endregion

    #region Accessible Text routines
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

    #endregion

    #region Accessible Table routines
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
    #endregion

    #region AccessibleRelationSet
    public GetAccessibleRelationSetFP GetAccessibleRelationSet { get; set; }
    #endregion

    #region AccessibleHypertext
    /// <summary>
    /// Returns hypertext information associated with a component.
    /// </summary>
    public GetAccessibleHypertextFP GetAccessibleHypertext { get; set; }
    /// <summary>
    /// Requests that a hyperlink be activated.
    /// </summary>
    public ActivateAccessibleHyperlinkFP ActivateAccessibleHyperlink { get; set; }
    /// <summary>
    /// Returns the number of hyperlinks in a component.
    /// Maps to AccessibleHypertext.getLinkCount.
    /// Returns -1 on error.
    /// </summary>
    public GetAccessibleHyperlinkCountFP GetAccessibleHyperlinkCount { get; set; }
    /// <summary>
    /// This method is used to iterate through the hyperlinks in a component.  It
    /// returns hypertext information for a component starting at hyperlink index
    /// nStartIndex. No more than MAX_HYPERLINKS AccessibleHypertextInfo objects will
    /// be returned for each call to this method.
    /// Returns FALSE on error.
    /// </summary>
    public GetAccessibleHypertextExtFP GetAccessibleHypertextExt { get; set; }
    /// <summary>
    /// Returns the index into an array of hyperlinks that is associated with
    /// a character index in document.
    /// Maps to AccessibleHypertext.getLinkIndex
    /// Returns -1 on error.
    /// </summary>
    public GetAccessibleHypertextLinkIndexFP GetAccessibleHypertextLinkIndex { get; set; }
    /// <summary>
    /// Returns the nth hyperlink in a document.
    //  Maps to AccessibleHypertext.getLink.
    //  Returns FALSE on error
    /// </summary>
    public GetAccessibleHyperlinkFP GetAccessibleHyperlink { get; set; }
    #endregion

    #region Accessible KeyBindings, Icons and Actions */
    public GetAccessibleKeyBindingsFP GetAccessibleKeyBindings { get; set; }
    public GetAccessibleIconsFP GetAccessibleIcons { get; set; }
    public GetAccessibleActionsFP GetAccessibleActions { get; set; }
    public DoAccessibleActionsFP DoAccessibleActions { get; set; }
    #endregion

    #region Additional utility methods
    //public SetTextContentsFP SetTextContents { get; set; }
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
    #endregion

    #region Event handling routines
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate BOOL EventSetterDelegate(Delegate nativeEventHandler);
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

    #region IAccessBridgeFunctions implementation
    private bool ToBool(int value) {
      return value !=  0;
    }

    private JavaObjectHandle Wrap(int vmID, JOBJECT64 handle) {
      return new JavaObjectHandle(vmID, handle);
    }

    void IAccessBridgeFunctions.Windows_run() {
      this.Windows_run();
    }

    bool IAccessBridgeFunctions.IsJavaWindow(HWND window) {
      return ToBool(this.IsJavaWindow(window));
    }

    bool IAccessBridgeFunctions.IsSameObject(int vmID, JavaObjectHandle obj1, JavaObjectHandle obj2) {
      return ToBool(this.IsSameObject(vmID, obj1.Handle, obj2.Handle));
    }

    bool IAccessBridgeFunctions.GetAccessibleContextFromHWND(HWND window, out int vmID, out JavaObjectHandle ac) {
      JOBJECT64 acRef;
      var success = this.GetAccessibleContextFromHWND(window, out vmID, out acRef);
      ac = Wrap(vmID, acRef);
      return ToBool(success);
    }

    HWND IAccessBridgeFunctions.GetHWNDFromAccessibleContext(int vmID, JavaObjectHandle ac) {
      return this.GetHWNDFromAccessibleContext(vmID, ac.Handle);
    }

    bool IAccessBridgeFunctions.GetAccessibleContextAt(int vmID, JavaObjectHandle acParent, int x, int y, out JavaObjectHandle ac) {
      JOBJECT64 acRef;
      var success = this.GetAccessibleContextAt(vmID, acParent.Handle, x, y, out acRef);
      ac = Wrap(vmID, acRef);
      return ToBool(success);
    }

    bool IAccessBridgeFunctions.GetAccessibleContextWithFocus(HWND window, out int vmID, out JavaObjectHandle ac) {
      JOBJECT64 acRef;
      var success = this.GetAccessibleContextWithFocus(window, out vmID, out acRef);
      ac = Wrap(vmID, acRef);
      return ToBool(success);
    }

    bool IAccessBridgeFunctions.GetAccessibleContextInfo(int vmID, JavaObjectHandle ac, out AccessibleContextInfo info) {
      return ToBool(this.GetAccessibleContextInfo(vmID, ac.Handle, out info));
    }

    JavaObjectHandle IAccessBridgeFunctions.GetAccessibleChildFromContext(int vmID, JavaObjectHandle ac, int i) {
      return Wrap(vmID, this.GetAccessibleChildFromContext(vmID, ac.Handle, i));
    }

    JavaObjectHandle IAccessBridgeFunctions.GetAccessibleParentFromContext(int vmID, JavaObjectHandle ac) {
      return Wrap(vmID, this.GetAccessibleParentFromContext(vmID, ac.Handle));
    }

    bool IAccessBridgeFunctions.GetAccessibleRelationSet(int vmID, JavaObjectHandle ac, out AccessibleRelationSetInfo relationSetInfo) {
      return ToBool(this.GetAccessibleRelationSet(vmID, ac.Handle, out relationSetInfo));
    }

    bool IAccessBridgeFunctions.GetAccessibleHypertext(int vmID, JavaObjectHandle ac, out AccessibleHypertextInfo hypertextInfo) {
      return ToBool(this.GetAccessibleHypertext(vmID, ac.Handle, out hypertextInfo));
    }

    bool IAccessBridgeFunctions.ActivateAccessibleHyperlink(int vmID, JavaObjectHandle ac, JavaObjectHandle accessibleHyperlink) {
      return ToBool(this.ActivateAccessibleHyperlink(vmID, ac.Handle, accessibleHyperlink.Handle));
    }

    int IAccessBridgeFunctions.GetAccessibleHyperlinkCount(int vmID, JavaObjectHandle ac) {
      return this.GetAccessibleHyperlinkCount(vmID, ac.Handle);
    }

    bool IAccessBridgeFunctions.GetAccessibleHypertextExt(int vmID, JavaObjectHandle ac, int nStartIndex, out AccessibleHypertextInfo hypertextInfo) {
      return ToBool(this.GetAccessibleHypertextExt(vmID, ac.Handle, nStartIndex, out hypertextInfo));
    }

    int IAccessBridgeFunctions.GetAccessibleHypertextLinkIndex(int vmID, JavaObjectHandle hypertext, int nIndex) {
      return this.GetAccessibleHypertextLinkIndex(vmID, hypertext.Handle, nIndex);
    }

    bool IAccessBridgeFunctions.GetAccessibleHyperlink(int vmID, JavaObjectHandle hypertext, int nIndex, out AccessibleHyperlinkInfo hyperlinkInfo) {
      return ToBool(this.GetAccessibleHyperlink(vmID, hypertext.Handle, nIndex, out hyperlinkInfo));
    }

    bool IAccessBridgeFunctions.GetAccessibleKeyBindings(int vmID, JavaObjectHandle ac, out AccessibleKeyBindings keyBindings) {
      return ToBool(this.GetAccessibleKeyBindings(vmID, ac.Handle,  out keyBindings));
    }

    bool IAccessBridgeFunctions.GetAccessibleIcons(int vmID, JavaObjectHandle ac, out AccessibleIcons icons) {
      return ToBool(this.GetAccessibleIcons(vmID, ac.Handle, out icons));
    }

    bool IAccessBridgeFunctions.GetAccessibleActions(int vmID, JavaObjectHandle ac, AccessibleActions actions) {
      return ToBool(this.GetAccessibleActions(vmID, ac.Handle, actions));
    }

    bool IAccessBridgeFunctions.DoAccessibleActions(int vmID, JavaObjectHandle ac, ref AccessibleActionsToDo actionsToDo, out int failure) {
      return ToBool(this.DoAccessibleActions(vmID, ac.Handle, ref actionsToDo, out failure));
    }

    bool IAccessBridgeFunctions.GetAccessibleTextInfo(int vmID, JavaObjectHandle at, out AccessibleTextInfo textInfo, int x, int y) {
      return ToBool(this.GetAccessibleTextInfo(vmID, at.Handle, out textInfo, x, y));
    }

    bool IAccessBridgeFunctions.GetAccessibleTextItems(int vmID, JavaObjectHandle at, out AccessibleTextItemsInfo textItems, int index) {
      return ToBool(this.GetAccessibleTextItems(vmID, at.Handle, out textItems, index));
    }

    bool IAccessBridgeFunctions.GetAccessibleTextSelectionInfo(int vmID, JavaObjectHandle at, out AccessibleTextSelectionInfo textSelection) {
      return ToBool(this.GetAccessibleTextSelectionInfo(vmID, at.Handle, out textSelection));
    }

    bool IAccessBridgeFunctions.GetAccessibleTextAttributes(int vmID, JavaObjectHandle at, int index, out AccessibleTextAttributesInfo attributes) {
      return ToBool(this.GetAccessibleTextAttributes(vmID, at.Handle, index, out attributes));
    }

    bool IAccessBridgeFunctions.GetAccessibleTextRect(int vmID, JavaObjectHandle at, out AccessibleTextRectInfo rectInfo, int index) {
      return ToBool(this.GetAccessibleTextRect(vmID, at.Handle, out rectInfo, index));
    }

    bool IAccessBridgeFunctions.GetAccessibleTextLineBounds(int vmID, JavaObjectHandle at, int index, out int startIndex, out int endIndex) {
      return ToBool(this.GetAccessibleTextLineBounds(vmID, at.Handle, index, out startIndex, out endIndex));
    }

    bool IAccessBridgeFunctions.GetAccessibleTextRange(int vmID, JavaObjectHandle at, int start, int end, StringBuilder text, short len) {
      return ToBool(this.GetAccessibleTextRange(vmID, at.Handle, start, end, text, len));
    }

    bool IAccessBridgeFunctions.GetCurrentAccessibleValueFromContext(int vmID, JavaObjectHandle av, StringBuilder value, short len) {
      return ToBool(this.GetCurrentAccessibleValueFromContext(vmID, av.Handle, value, len));
    }

    bool IAccessBridgeFunctions.GetMaximumAccessibleValueFromContext(int vmID, JavaObjectHandle av, StringBuilder value, short len) {
      return ToBool(this.GetMaximumAccessibleValueFromContext(vmID, av.Handle, value, len));
    }

    bool IAccessBridgeFunctions.GetMinimumAccessibleValueFromContext(int vmID, JavaObjectHandle av, StringBuilder value, short len) {
      return ToBool(this.GetMinimumAccessibleValueFromContext(vmID, av.Handle, value, len));
    }

    void IAccessBridgeFunctions.AddAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel, int i) {
      this.AddAccessibleSelectionFromContext(vmID, asel.Handle, i);
    }

    void IAccessBridgeFunctions.ClearAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel) {
      this.ClearAccessibleSelectionFromContext(vmID, asel.Handle);
    }

    JavaObjectHandle IAccessBridgeFunctions.GetAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel, int i) {
      return Wrap(vmID, this.GetAccessibleSelectionFromContext(vmID, asel.Handle, i));
    }

    int IAccessBridgeFunctions.GetAccessibleSelectionCountFromContext(int vmID, JavaObjectHandle asel) {
      return this.GetAccessibleSelectionCountFromContext(vmID, asel.Handle);
    }

    bool IAccessBridgeFunctions.IsAccessibleChildSelectedFromContext(int vmID, JavaObjectHandle asel, int i) {
      return ToBool(this.IsAccessibleChildSelectedFromContext(vmID, asel.Handle, i));
    }

    void IAccessBridgeFunctions.RemoveAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel, int i) {
      this.RemoveAccessibleSelectionFromContext(vmID, asel.Handle, i);
    }

    void IAccessBridgeFunctions.SelectAllAccessibleSelectionFromContext(int vmID, JavaObjectHandle asel) {
      this.SelectAllAccessibleSelectionFromContext(vmID, asel.Handle);
    }

    bool IAccessBridgeFunctions.GetAccessibleTableInfo(int vmID, JavaObjectHandle table, out AccessibleTableInfo tableInfo) {
      return ToBool(this.GetAccessibleTableInfo(vmID, table.Handle, out tableInfo));
    }

    bool IAccessBridgeFunctions.GetAccessibleTableCellInfo(int vmID, JavaObjectHandle table, int row, int column, out AccessibleTableCellInfo tableCellInfo) {
      return ToBool(this.GetAccessibleTableCellInfo(vmID, table.Handle, row, column, out tableCellInfo));
    }

    bool IAccessBridgeFunctions.GetAccessibleTableRowHeader(int vmID, JavaObjectHandle table, out AccessibleTableInfo tableInfo) {
      return ToBool(this.GetAccessibleTableRowHeader(vmID, table.Handle, out tableInfo));
    }

    bool IAccessBridgeFunctions.GetAccessibleTableColumnHeader(int vmID, JavaObjectHandle table, out AccessibleTableInfo tableInfo) {
      return ToBool(this.GetAccessibleTableColumnHeader(vmID, table.Handle, out tableInfo));
    }

    JavaObjectHandle IAccessBridgeFunctions.GetAccessibleTableRowDescription(int vmID, JavaObjectHandle table, int row) {
      return Wrap(vmID, this.GetAccessibleTableRowDescription(vmID, table.Handle, row));
    }

    JavaObjectHandle IAccessBridgeFunctions.GetAccessibleTableColumnDescription(int vmID, JavaObjectHandle table, int column) {
      return Wrap(vmID, this.GetAccessibleTableColumnDescription(vmID, table.Handle, column));
    }

    int IAccessBridgeFunctions.GetAccessibleTableRowSelectionCount(int vmID, JavaObjectHandle table) {
      return this.GetAccessibleTableRowSelectionCount(vmID, table.Handle);
    }

    bool IAccessBridgeFunctions.IsAccessibleTableRowSelected(int vmID, JavaObjectHandle table, int row) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.GetAccessibleTableRowSelections(int vmID, JavaObjectHandle table, int count, int[] selections) {
      throw new NotImplementedException();
    }

    int IAccessBridgeFunctions.GetAccessibleTableColumnSelectionCount(int vmID, JavaObjectHandle table) {
      return this.GetAccessibleTableColumnSelectionCount(vmID, table.Handle);
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
      throw new NotImplementedException();
    }

    JavaObjectHandle IAccessBridgeFunctions.GetActiveDescendent(int vmID, JavaObjectHandle ac) {
      throw new NotImplementedException();
    }

    bool IAccessBridgeFunctions.GetVirtualAccessibleName(int vmID, JavaObjectHandle ac, StringBuilder name, int len) {
      throw new NotImplementedException();
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