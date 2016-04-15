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

using WindowsAccessBridgeInterop;
using AccessBridgeExplorer.Utils;

namespace AccessBridgeExplorer {
  public class PropertyListTreeViewModel : TreeListViewModel {
    private readonly PropertyGroup _rootNode;

    public PropertyListTreeViewModel(PropertyList propertyList) {
      _rootNode = new PropertyGroup("");
      _rootNode.Children.AddRange(propertyList);
    }

    public override object GetRootNode() {
      return _rootNode;
    }

    public override bool IsRootVisible() {
      return false;
    }

    public override int GetChildrenCount(object modelNode) {
      var node = (PropertyNode)modelNode;
      var group = modelNode as PropertyGroup;
      if (group == null)
        return 0;
      return group.Children.Count;
    }

    public override object GetChildAt(object modelNode, int index) {
      var group = (PropertyGroup)modelNode;
      return group.Children[index];
    }

    public override bool IsNodeExpandable(object modelNode) {
      var node = (PropertyNode)modelNode;
      var group = modelNode as PropertyGroup;
      return group != null;
    }

    public override bool IsNodeExpanded(object modelNode) {
      var node = (PropertyNode)modelNode;
      var group = modelNode as PropertyGroup;
      if (group != null)
        return group.Expanded;
      return false;
    }

    public override string GetNodeText(object modelNode) {
      var node = (PropertyNode)modelNode;
      return node.Name;
    }

    public override int GetNodeSubItemCount(object modelNode) {
      return 1;
    }

    public override string GetNodeSubItemAt(object modelNode, int index) {
      var node = (PropertyNode)modelNode;
      return ValueToString(node);
    }

    private static string ValueToString(PropertyNode propertyNode) {
      var value = propertyNode.Value;
      string valueText;
      if (value == null) {
        if (propertyNode is PropertyGroup)
          valueText = "";
        else
          valueText = "-";
      } else if (value is bool) {
        valueText = ((bool)value) ? "Yes" : "No";
      } else if (value is string) {
        valueText = string.IsNullOrEmpty((string)value) ? "-" : (string)value;
      } else {
        valueText = value.ToString();
      }
      return valueText;
    }
  }
}