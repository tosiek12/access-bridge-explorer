using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using AccessBridgeExplorer.Model;
using AccessBridgeExplorer.WindowsAccessBridge.NativeStructures;

namespace AccessBridgeExplorer.WindowsAccessBridge {
  /// <summary>
  /// Wrapper for an accessible context returned by the Java Access Bridge.
  /// </summary>
  public class AccessibleContextNode : AccessibleNode {
    const int TextBufferSize = 256;
    private readonly JavaObjectHandle _ac;
    private Lazy<AccessibleContextInfo> _info;
    private readonly List<Lazy<AccessibleNode>> _childList = new List<Lazy<AccessibleNode>>();

    public AccessibleContextNode(AccessBridge accessBridge, JavaObjectHandle ac) : base(accessBridge) {
      _ac = ac;
      _info = new Lazy<AccessibleContextInfo>(FetchNodeInfo);
    }

    public override int JvmId {
      get { return _ac.JvmId; }
    }

    public override void Dispose() {
      _ac.Close();
    }

    public override AccessibleNode GetParent() {
      ThrowIfDisposed();
      var parentAc = new JavaObjectHandle(JvmId, AccessBridge.Functions.GetAccessibleParentFromContext(JvmId, _ac));
      if (parentAc.IsNull) {
        return null;
      }

      var hwnd = AccessBridge.Functions.GetHWNDFromAccessibleContext(JvmId, parentAc);
      if (hwnd != IntPtr.Zero) {
        return new AccessibleWindow(AccessBridge, hwnd, parentAc);
      }

      return new AccessibleContextNode(AccessBridge, parentAc);
    }

    private void ThrowIfDisposed() {
      if (_ac.IsClosed)
        throw new ObjectDisposedException("accessibility context");
    }

    public AccessibleContextInfo FetchNodeInfo() {
      ThrowIfDisposed();
      AccessibleContextInfo info;
      if (Failed(AccessBridge.Functions.GetAccessibleContextInfo(JvmId, _ac, out info))) {
        throw new ApplicationException("Error retrieving accessible context info");
      }
      _childList.Clear();
      _childList.AddRange(Enumerable.Range(0, info.childrenCount).Select(i => new Lazy<AccessibleNode>(() => FetchChildNode(i))));
      return info;
    }

    public AccessibleNode FetchChildNode(int i) {
      ThrowIfDisposed();
      var childContextPtr = AccessBridge.Functions.GetAccessibleChildFromContext(JvmId, _ac, i);
      if (childContextPtr == IntPtr.Zero) {
        throw new ApplicationException("Error retrieving accessible context for child component");
      }

      return new AccessibleContextNode(AccessBridge, new JavaObjectHandle(JvmId, childContextPtr));
    }

    public override void Refresh() {
      _info = new Lazy<AccessibleContextInfo>(FetchNodeInfo);
    }

    public AccessibleContextInfo GetInfo() {
      return _info.Value;
    }

    public override int GetChildrenCount() {
      return GetInfo().childrenCount;
    }

    public override AccessibleNode GetChildAt(int i) {
      ThrowIfDisposed();
      return _childList[i].Value;
    }

    public override Rectangle? GetScreenRectangle() {
      var info = GetInfo();
      if (info.width >= 0 && info.height >= 0) {
        return new Rectangle(info.x, info.y, info.width, info.height);
      }
      return null;
    }

    public override void FetchSubTree() {
      //GetInfo();
      //foreach (var child in GetChildren()) {
      //  child.FetchSubTree();
      //}
    }

    public override NodePath GetNodePathAt(Point screenPoint) {
      // Bail out early if the node is not visible
      var info = GetInfo();
      if (info.states != null && info.states.IndexOf("showing", StringComparison.InvariantCulture) < 0) {
        return null;
      }

      // Special case to avoid returning nodes from overlapping components:
      // If we are a viewport, we only return children contained inside our
      // containing rectangle.
      if (info.role != null && info.role.Equals("viewport", StringComparison.InvariantCulture)) {
        var rectangle = GetScreenRectangle();
        if (rectangle != null) {
          if (!rectangle.Value.Contains(screenPoint))
            return null;
        }
      }
      return base.GetNodePathAt(screenPoint);
    }

    /// <summary>
    /// Experimental implementation using <see cref="AccessBridgeFunctions.GetAccessibleContextAt"/>
    /// </summary>
    private NodePath GetNodePathAtUsingAccessBridge(Point screenPoint) {
      IntPtr childContextHandle;
      if (
        Failed(AccessBridge.Functions.GetAccessibleContextAt(JvmId, _ac, screenPoint.X, screenPoint.Y,
          out childContextHandle))) {
        return null;
      }

      if (childContextHandle == IntPtr.Zero) {
        return null;
      }

      var childHandle = new JavaObjectHandle(JvmId, childContextHandle);
      var childNode = new AccessibleContextNode(AccessBridge, childHandle);
      Debug.WriteLine("Child found: {0}-{1}-{2}-{3}", childNode.GetInfo().x, childNode.GetInfo().y,
        childNode.GetInfo().width, childNode.GetInfo().height);

      var path = new NodePath();
      for (AccessibleNode node = childNode; node != null; node = node.GetParent()) {
        /*DEBUG*/
        if (node is AccessibleContextNode) {
          ((AccessibleContextNode)node).GetInfo();
        }
        path.AddParent(node);
        if (node.Equals(this))
          break;
      }
      return path;
    }

    public override string GetTitle() {
      var info = GetInfo();
      string text;
      if (!string.IsNullOrEmpty(info.name))
        text = info.name;
      else if (!string.IsNullOrEmpty(info.description))
        text = info.description;
      else
        text = "";

      if (string.IsNullOrEmpty(text))
        text = string.Format("{0}", info.role ?? "");
      else
        text = string.Format("{0}: {1}", info.role ?? "", text);
      return text;
    }

    protected override void AddProperties(PropertyList list, PropertyOptions options) {
      AddContextProperties(list, options);
      AddTopLevelWindowProperties(list, options);
      AddParentContextProperties(list, options);
      AddSelectionProperties(list, options);
      AddKeyBindingsProperties(list, options);
      AddIconsProperties(list, options);
      AddActionProperties(list, options);
      AddVisibleDescendentsProperties(list, options);
      AddRelationSetProperties(list, options);
      AddValueProperties(list, options);
      AddTableProperties(list, options);
      AddTextProperties(list, options);
    }

    private void AddTopLevelWindowProperties(PropertyList list, PropertyOptions options) {
      if ((options & PropertyOptions.TopLevelWindowInfo) != 0) {
        var topLevel = new JavaObjectHandle(JvmId, AccessBridge.Functions.GetTopLevelObject(JvmId, _ac));
        if (!topLevel.IsNull) {
          var group = list.AddGroup("Top level window");
          group.Expanded = false;
          AddSubContextProperties(group.Children, topLevel);
        }
      }
    }

    private void AddParentContextProperties(PropertyList list, PropertyOptions options) {
      if ((options & PropertyOptions.ParentContext) != 0) {
        var parentContext = new JavaObjectHandle(JvmId, AccessBridge.Functions.GetAccessibleParentFromContext(JvmId, _ac));
        if (!parentContext.IsNull) {
          var group = list.AddGroup("Parent");
          group.Expanded = false;
          AddSubContextProperties(group.Children, parentContext);
        }
      }
    }

    private void AddSelectionProperties(PropertyList list, PropertyOptions options) {
      if ((options & PropertyOptions.AccessibleSelection) != 0) {
        var info = GetInfo();
        if (info.accessibleSelection != 0) {
          var selCount = AccessBridge.Functions.GetAccessibleSelectionCountFromContext(JvmId, _ac);
          var group = list.AddGroup("Selections", selCount);
          group.Expanded = false;
          if (selCount > 0) {
            for (var i = 0; i < selCount; i++) {
              var selGroup = group.AddGroup(string.Format("Selection {0} of {1}", i + 1, selCount));
              var selectedContext = new JavaObjectHandle(JvmId, AccessBridge.Functions.GetAccessibleSelectionFromContext(JvmId, _ac, i));
              if (selectedContext.IsNull) {
                selGroup.AddProperty(string.Format("Selection {0} of {1}", i + 1, selCount), "<Error>");
              } else {
                AddSubContextProperties(selGroup.Children, selectedContext);
              }
            }
          }
        }
      }
    }

    private void AddKeyBindingsProperties(PropertyList list, PropertyOptions options) {
      if ((options & PropertyOptions.AccessibleKeyBindings) != 0) {
        AccessibleKeyBindings keyBindings;
        if (Succeeded(AccessBridge.Functions.GetAccessibleKeyBindings(JvmId, _ac, out keyBindings))) {
          if (keyBindings.keyBindingsCount > 0) {
            var group = list.AddGroup("Key Bindings", keyBindings.keyBindingsCount);
            group.Expanded = false;
            for (var i = 0; i < keyBindings.keyBindingsCount; i++) {
              var keyGroup = group.AddGroup(string.Format("Key binding {0} of {1}", i + 1, keyBindings.keyBindingsCount));
              keyGroup.AddProperty("Character", keyBindings.keyBindingInfo[i].character);
              keyGroup.AddProperty("Modifiers", keyBindings.keyBindingInfo[i].modifiers);
            }
          }
        }
      }
    }

    private void AddIconsProperties(PropertyList list, PropertyOptions options) {
      if ((options & PropertyOptions.AccessibleIcons) != 0) {
        AccessibleIcons icons;
        if (Succeeded(AccessBridge.Functions.GetAccessibleIcons(JvmId, _ac, out icons))) {
          if (icons.iconsCount > 0) {
            var group = list.AddGroup("Icons", icons.iconsCount);
            group.Expanded = false;
            for (var i = 0; i < icons.iconsCount; i++) {
              var iconGroup = group.AddGroup(string.Format("Icon {0} of {1}", i + 1, icons.iconsCount));
              iconGroup.AddProperty("Height", icons.iconInfo[i].height);
              iconGroup.AddProperty("Width", icons.iconInfo[i].width);
            }
          }
        }
      }
    }

    private void AddActionProperties(PropertyList list, PropertyOptions options) {
      if ((options & PropertyOptions.AccessibleActions) != 0) {
        AccessibleActions actions = new AccessibleActions();
        if (Succeeded(AccessBridge.Functions.GetAccessibleActions(JvmId, _ac, actions))) {
          if (actions.actionsCount > 0) {
            var group = list.AddGroup("Actions", actions.actionsCount);
            group.Expanded = false;
            if (actions.actionsCount > 0) {
              for (var i = 0; i < actions.actionsCount; i++) {
                group.AddProperty(
                  string.Format("Action {0} of {1}", i + 1, actions.actionsCount),
                  actions.actionInfo[i].name);
              }
            }
          }
        }
      }
    }

    private void AddVisibleDescendentsProperties(PropertyList list, PropertyOptions options) {
      if ((options & PropertyOptions.VisibleDescendents) != 0) {
        var visibleCount = AccessBridge.Functions.GetVisibleChildrenCount(JvmId, _ac);
        if (visibleCount > 0) {
          var group = list.AddGroup("Visible Descendents", visibleCount);
          group.Expanded = false;
          VisibleChildrenInfo childrenInfo;
          if (AccessBridge.Functions.GetVisibleChildren(JvmId, _ac, 0, out childrenInfo) != 0) {
            var handles = Enumerable
              .Range(0, childrenInfo.returnedChildrenCount)
              .Select(i => new JavaObjectHandle(JvmId, childrenInfo.children[i]))
              .ToList();

            var childNodes = handles.Select(x => new AccessibleContextNode(AccessBridge, x)).ToList();
            var childIndex = 0;
            foreach (var childNode in childNodes) {
              var childGroup = group.AddGroup(string.Format("Descendent {0} of {1}", childIndex + 1, childNodes.Count));
              AddSubContextProperties(childGroup.Children, childNode);
              childIndex++;
            }
          }
        }
      }
    }

    private void AddRelationSetProperties(PropertyList list, PropertyOptions options) {
      if ((options & PropertyOptions.AccessibleRelationSet) != 0) {
        AccessibleRelationSetInfo relationSetInfo;
        if (Succeeded(AccessBridge.Functions.GetAccessibleRelationSet(JvmId, _ac, out relationSetInfo))) {
          if (relationSetInfo.relationCount > 0) {
            // Note: Create safe handles as soon as possible
            var handles = Enumerable
              .Range(0, relationSetInfo.relationCount)
              .SelectMany(i =>
                Enumerable
                  .Range(0, relationSetInfo.relations[i].targetCount)
                  .Select(j => new JavaObjectHandle(JvmId, relationSetInfo.relations[i].targets[j]))).ToList();

            var group = list.AddGroup("Relation Set", relationSetInfo.relationCount);
            group.Expanded = false;

            var handleIndex = 0;
            for (var i = 0; i < relationSetInfo.relationCount; i++) {
              var relationInfo = relationSetInfo.relations[i];
              var relGroup = group.AddGroup(string.Format("Relation {0} of {1}", i + 1, relationSetInfo.relationCount));
              relGroup.AddProperty("Key", relationInfo.key);

              var relSubGroup = relGroup.AddGroup("Targets", relationInfo.targetCount);
              for (var j = 0; j < relationInfo.targetCount; j++) {
                var targetGroup = relSubGroup.AddGroup(string.Format("Target {0} of {1}", j + 1, relationInfo.targetCount));
                AddSubContextProperties(targetGroup.Children, handles[handleIndex]);
                handleIndex++;
              }
            }
          }
        }
      }
    }

    private void AddValueProperties(PropertyList list, PropertyOptions options) {
      if ((options & PropertyOptions.AccessibleValue) != 0) {
        var info = GetInfo();
        if ((info.accessibleInterfaces & AccessibleInterfaces.cAccessibleValueInterface) != 0) {
          var group = list.AddGroup("Value");
          group.Expanded = false;

          var sb = new StringBuilder(TextBufferSize);
          if (Succeeded(AccessBridge.Functions.GetCurrentAccessibleValueFromContext(JvmId, _ac, sb, (short)sb.Capacity))) {
            group.AddProperty("Current", sb);
          }
          if (Succeeded(AccessBridge.Functions.GetMaximumAccessibleValueFromContext(JvmId, _ac, sb, (short)sb.Capacity))) {
            group.AddProperty("Maximum", sb);
          }
          if (Succeeded(AccessBridge.Functions.GetMinimumAccessibleValueFromContext(JvmId, _ac, sb, (short)sb.Capacity))) {
            group.AddProperty("Minimum", sb);
          }
        }
      }
    }

    public class AccessibleTableInfoWrapper {
      public JavaObjectHandle Caption; // AccessibleContext
      public JavaObjectHandle Summary; // AccessibleContext
      public int RowCount;
      public int ColumnCount;
      public JavaObjectHandle AccessibleContext;
      public JavaObjectHandle AccessibleTable;
    }

    public AccessibleTableInfoWrapper WrapTableInfo(AccessibleTableInfo info) {
      return new AccessibleTableInfoWrapper {
        Caption = new JavaObjectHandle(JvmId, info.caption),
        Summary = new JavaObjectHandle(JvmId, info.summary),
        RowCount = info.rowCount,
        ColumnCount = info.columnCount,
        AccessibleContext = new JavaObjectHandle(JvmId, info.accessibleContext),
        AccessibleTable = new JavaObjectHandle(JvmId, info.accessibleTable),
      };
    }

    private void AddTableProperties(PropertyList list, PropertyOptions options) {
      if ((options & PropertyOptions.AccessibleTable) != 0) {
        var nodeInfo = GetInfo();
        if ((nodeInfo.accessibleInterfaces & AccessibleInterfaces.cAccessibleTableInterface) != 0) {
          var group = list.AddGroup("Table");
          group.Expanded = false;
          AccessibleTableInfo tableInfoNative;
          if (Failed(AccessBridge.Functions.GetAccessibleTableInfo(JvmId, _ac, out tableInfoNative))) {
            group.AddProperty("Error", "Error retrieving table info");
          } else {
            var tableInfo = WrapTableInfo(tableInfoNative);
            AddTableInfo(group, tableInfo);

#if false
            int trow = AccessBridge.Functions.GetAccessibleTableRow(JvmId, tableInfo.AccessibleTable, 3);
            appendToBuffer(buffer, bufsize, "\r\n    getAccessibleTableRow:  %d", trow);

            int tcol = getAccessibleTableColumn(vmID, tableInfo.AccessibleTable, 2);
            appendToBuffer(buffer, bufsize, "\r\n    getAccessibleTableColumn:  %d", tcol);

            int tindex = getAccessibleTableIndex(vmID, tableInfo.AccessibleTable, 2, 3);
            appendToBuffer(buffer, bufsize, "\r\n    getAccessibleTableIndex:  %d", tindex);
#endif

            // Get the column headers
            var columnHeaderGroup = group.AddGroup("Column Header");
            columnHeaderGroup.Expanded = false;
            AccessibleTableInfo columnInfoNative;
            if (Failed(AccessBridge.Functions.GetAccessibleTableColumnHeader(JvmId, _ac, out columnInfoNative))) {
              columnHeaderGroup.AddProperty("Error", "Error retrieving column header info");
            } else {
              var columnInfo = WrapTableInfo(columnInfoNative);
              AddTableInfo(columnHeaderGroup, columnInfo);
              for (var rowIndex = 0; rowIndex < columnInfo.RowCount; rowIndex++) {
                for (var colunmnIndex = 0; colunmnIndex < columnInfo.ColumnCount; colunmnIndex++) {
                  var chGroup = columnHeaderGroup.AddGroup(string.Format("Column Header[Row={0},Column={1}]", rowIndex, colunmnIndex));

                }
              }
            }

            // Get the row headers
            var rowHeaderGroup = group.AddGroup("Row Header");
            rowHeaderGroup.Expanded = false;
            AccessibleTableInfo rowInfoNative;
            if (Failed(AccessBridge.Functions.GetAccessibleTableRowHeader(JvmId, _ac, out rowInfoNative))) {
              rowHeaderGroup.AddProperty("Error", "Error retrieving column header info");
            } else {
              var rowInfo = WrapTableInfo(rowInfoNative);
              AddTableInfo(rowHeaderGroup, rowInfo);
            }

            // Get the selected columns
            var numColSelections = AccessBridge.Functions.GetAccessibleTableColumnSelectionCount(JvmId, tableInfo.AccessibleTable);
            var selColGroup = group.AddGroup("Column selections", numColSelections);
            selColGroup.Expanded = false;
            if (numColSelections > 0) {
              var selections = new int[numColSelections];
              if (Failed(AccessBridge.Functions.GetAccessibleTableColumnSelections(JvmId, tableInfo.AccessibleTable, numColSelections, selections))) {
                selColGroup.AddProperty("Error", "Error getting column selections");
              } else {
                for (var j = 0; j < numColSelections; j++) {
                  selColGroup.AddProperty(string.Format("Column index {0} of {1}", j + 1, numColSelections), selections[j]);
                }
              }
            }

            // Get the selected rows
            var numRowSelections = AccessBridge.Functions.GetAccessibleTableRowSelectionCount(JvmId, tableInfo.AccessibleTable);
            var selRowGroup = group.AddGroup("Row selections", numRowSelections);
            selRowGroup.Expanded = false;
            if (numRowSelections > 0) {
              var selections = new int[numRowSelections];
              if (Failed(AccessBridge.Functions.GetAccessibleTableRowSelections(JvmId, tableInfo.AccessibleTable, numRowSelections, selections))) {
                selRowGroup.AddProperty("Error", "Error getting row selections");
              } else {
                for (var j = 0; j < numRowSelections; j++) {
                  selRowGroup.AddProperty(string.Format("Row index {0} of {1}", j + 1, numRowSelections), selections[j]);
                }
              }
            }

            // Get info of all cells
            if ((options & PropertyOptions.AccessibleTableCells) != 0) {
              var cellsGroup = group.AddGroup("Cells");
              cellsGroup.Expanded = false;
              for (var rowIndex = 0; rowIndex < tableInfo.RowCount; rowIndex++) {
                for (var colunmnIndex = 0; colunmnIndex < tableInfo.ColumnCount; colunmnIndex++) {
                  var cellGroup = cellsGroup.AddGroup(string.Format("Cell[Row={0},Column={1}]", rowIndex, colunmnIndex));

                  AccessibleTableCellInfo tableCellInfo;
                  if (
                    Failed(AccessBridge.Functions.GetAccessibleTableCellInfo(tableInfo.AccessibleTable.JvmId,
                      tableInfo.AccessibleTable, rowIndex, colunmnIndex, out tableCellInfo))) {
                    cellGroup.AddProperty("Error", "Error retrieving cell info");
                  } else {
                    var cellHandle = new JavaObjectHandle(tableInfo.AccessibleTable.JvmId,
                      tableCellInfo.accessibleContext);
                    cellGroup.AddProperty("Index", tableCellInfo.index);
                    cellGroup.AddProperty("Row extent", tableCellInfo.rowExtent);
                    cellGroup.AddProperty("Column extent", tableCellInfo.columnExtent);
                    cellGroup.AddProperty("Is selected", tableCellInfo.isSelected);

                    AddSubContextProperties(cellGroup.Children, cellHandle);
                  }
                }
              }
            }
          }
        }
      }
    }

    private void AddTableInfo(PropertyGroup group, AccessibleTableInfoWrapper tableInfo) {
      group.AddProperty("Row count", tableInfo.RowCount);
      group.AddProperty("Column count", tableInfo.ColumnCount);
    }

    private void AddTextProperties(PropertyList list, PropertyOptions options) {
      if ((options & PropertyOptions.AccessibleText) != 0) {
        var info = GetInfo();
        if (info.accessibleText != 0) {
          int x = 0;
          int y = 0;

          var group = list.AddGroup("Accessible Text");
          group.Expanded = false;

          AccessibleTextInfo textInfo;
          if (Succeeded(AccessBridge.Functions.GetAccessibleTextInfo(JvmId, _ac, out textInfo, x, y))) {
            group.AddProperty("Mouse point at text index", textInfo.indexAtPoint);
            group.AddProperty("Caret at text index", textInfo.caretIndex);
            group.AddProperty("Char count", textInfo.charCount);
          }

          AccessibleTextSelectionInfo textSelection;
          if (Succeeded(AccessBridge.Functions.GetAccessibleTextSelectionInfo(JvmId, _ac, out textSelection))) {
            group.AddProperty("Selection start index", textSelection.selectionStartIndex);
            group.AddProperty("Selection end index", textSelection.selectionEndIndex);
            group.AddProperty("Selected text", textSelection.selectedText);
          }

          /* ===== AccessibleText information at the mouse point ===== */

          var mouseGroup = group.AddGroup(string.Format("Mouse point at index {0} attributes", textInfo.indexAtPoint));
          AddTextAttributeAtIndex(mouseGroup.Children, textInfo.indexAtPoint);

          /* ===== AccessibleText information at the caret index ===== */

          var caretGroup = group.AddGroup(string.Format("Caret at index {0} attributes", textInfo.caretIndex));
          AddTextAttributeAtIndex(caretGroup.Children, textInfo.caretIndex);
        }
      }
    }

    private void AddTextAttributeAtIndex(PropertyList list, int index) {
      AccessibleTextRectInfo rectInfo;
      if (Succeeded(AccessBridge.Functions.GetAccessibleTextRect(JvmId, _ac, out rectInfo, index))) {
        list.AddProperty("Character bounding rectangle:", string.Format("[{0},{1},{2},{3}]", rectInfo.x, rectInfo.y, rectInfo.width, rectInfo.height));
      }

      int start;
      int end;
      if (Succeeded(AccessBridge.Functions.GetAccessibleTextLineBounds(JvmId, _ac, index, out start, out end))) {
        list.AddProperty("Line bounds", string.Format("[{0},{1}]", start, end));
        var sb = new StringBuilder(TextBufferSize);
        if (Succeeded(AccessBridge.Functions.GetAccessibleTextRange(JvmId, _ac, start, end, sb, (short)sb.Capacity))) {
          list.AddProperty("Line text", sb);
        }
      }

      AccessibleTextItemsInfo textItems;
      if (Succeeded(AccessBridge.Functions.GetAccessibleTextItems(JvmId, _ac, out textItems, index))) {
        list.AddProperty("Character", textItems.letter);
        list.AddProperty("Word", textItems.word);
        list.AddProperty("Sentence", textItems.sentence);
      }

      /* ===== AccessibleText attributes ===== */

      AccessibleTextAttributesInfo attributeInfo;
      if (Succeeded(AccessBridge.Functions.GetAccessibleTextAttributes(JvmId, _ac, index, out attributeInfo))) {
        list.AddProperty("Core attributes", (attributeInfo.bold != 0 ? "bold" : "not bold") + ", " +
                                            (attributeInfo.italic != 0 ? "italic" : "not italic") + ", " +
                                            (attributeInfo.underline != 0 ? "underline" : "not underline") + ", " +
                                            (attributeInfo.strikethrough != 0 ? "strikethrough" : "not strikethrough") + ", " +
                                            (attributeInfo.superscript != 0 ? "superscript" : "not superscript") + ", " +
                                            (attributeInfo.subscript != 0 ? "subscript" : "not subscript"));

        list.AddProperty("Background color", attributeInfo.backgroundColor);
        list.AddProperty("Foreground color", attributeInfo.foregroundColor);
        list.AddProperty("Font family", attributeInfo.fontFamily);
        list.AddProperty("Font size", attributeInfo.fontSize);
        list.AddProperty("First line indent", attributeInfo.firstLineIndent);
        list.AddProperty("Left indent", attributeInfo.leftIndent);
        list.AddProperty("Right indent", attributeInfo.rightIndent);
        list.AddProperty("Line spacing", attributeInfo.lineSpacing);
        list.AddProperty("Space above", attributeInfo.spaceAbove);
        list.AddProperty("Space below", attributeInfo.spaceBelow);
        list.AddProperty("Full attribute string", attributeInfo.fullAttributesString);

        // get the attribute run length
        short runLength;
        if (Succeeded(AccessBridge.Functions.GetTextAttributesInRange(JvmId, _ac, index, index + 100, out attributeInfo, out runLength))) {
          list.AddProperty("Attribute run", runLength);
        } else {
          list.AddProperty("Attribute run", "<Error>");
        }
      }
    }

    private void AddContextProperties(PropertyList list, PropertyOptions options) {
      if ((options & PropertyOptions.AccessibleContextInfo) != 0) {
        var info = GetInfo();
        list.AddProperty("Name", info.name ?? "-");
        list.AddProperty("Description", info.description ?? "-");
        list.AddProperty("Name (JAWS algorithm)", GetVirtualAccessibleName());
        list.AddProperty("Bounds", string.Format("[{0}, {1}, {2}, {3}]", info.x, info.y, info.width, info.height));
        list.AddProperty("Role", info.role ?? "-");
        list.AddProperty("Role_en_US", info.role_en_US ?? "-");
        list.AddProperty("States", info.states ?? "-");
        list.AddProperty("States_en_US", info.states_en_US ?? "-");
        list.AddProperty("childrenCount", info.childrenCount);
        list.AddProperty("indexInParent", info.indexInParent);
        list.AddProperty("accessibleComponent", info.accessibleComponent);
        list.AddProperty("accessibleAction", info.accessibleAction);
        list.AddProperty("accessibleSelection", info.accessibleSelection);
        list.AddProperty("accessibleText", info.accessibleText);
        list.AddProperty("accessibleInterfaces", info.accessibleInterfaces);
      }
    }

    private void AddSubContextProperties(PropertyList list, JavaObjectHandle contextHandle) {
      var cellNode = new AccessibleContextNode(AccessBridge, contextHandle);
      AddSubContextProperties(list, cellNode);
    }

    private void AddSubContextProperties(PropertyList list, AccessibleContextNode contextNode) {
      try {
        var info = contextNode.GetInfo();
        list.AddProperty("Name", info.name);
        list.AddProperty("Description", info.description);
        list.AddProperty("Name (JAWS algorithm)", contextNode.GetVirtualAccessibleName());
        list.AddProperty("Bounds", string.Format("[{0}, {1}, {2}, {3}]", info.x, info.y, info.width, info.height));
        list.AddProperty("Role", info.role);
        list.AddProperty("Index in parent", info.indexInParent);
        list.AddProperty("accessibleInterfaces", info.accessibleInterfaces);
      } catch (Exception e) {
        list.AddProperty("Error", e.Message);
      }
    }

    protected override void AddToolTipProperties(PropertyList list) {
      try {
        var info = GetInfo();
        list.AddProperty("Name", info.name);
        list.AddProperty("Description", info.description);
        list.AddProperty("Name (JAWS algorithm)", GetVirtualAccessibleName());
        list.AddProperty("Bounds", string.Format("[{0}, {1}, {2}, {3}]", info.x, info.y, info.width, info.height));
        list.AddProperty("Role", info.role);
        list.AddProperty("States", info.states);
        list.AddProperty("accessibleInterfaces", info.accessibleInterfaces);
      } catch (Exception e) {
        list.AddProperty("Error", e.Message);
      }
    }

    public override bool Equals(AccessibleNode other) {
      if (!base.Equals(other))
        return false;

      if (!(other is AccessibleContextNode))
        return false;

      return AccessBridge.Functions.IsSameObject(JvmId, _ac, ((AccessibleContextNode)other)._ac) != 0;
    }

    /// <summary>
    /// Call the custom function <see
    /// cref="AccessBridgeFunctions.GetVirtualAccessibleName"/> to retrieve the
    /// spoken name of an accessible component supposedly according to the
    /// algorithm used by the Jaws screen reader.
    /// </summary>
    private string GetVirtualAccessibleName() {
      var sb = new StringBuilder(TextBufferSize);
      if (Failed(AccessBridge.Functions.GetVirtualAccessibleName(JvmId, _ac, sb, sb.Capacity))) {
        return "<Error>";
      }
      return sb.ToString();
    }

    private static bool Failed(int accessBridgeReturnValue) {
      return accessBridgeReturnValue == 0;
    }

    private static bool Succeeded(int accessBridgeReturnValue) {
      return !Failed(accessBridgeReturnValue);
    }

    public override string ToString() {
      try {
        var info = GetInfo();
        return string.Format("AccessibleContextNode(name={0},role={1},x={2},y={3},w={4},h={5})", info.name ?? " - ", info.role ?? " - ", info.x, info.y, info.width, info.height);
      } catch (Exception e) {
        return string.Format("AccessibleContextNode(Error={0})", e.Message);
      }
    }
    public override int GetIndexInParent() {
      return GetInfo().indexInParent;
    }
  }
}