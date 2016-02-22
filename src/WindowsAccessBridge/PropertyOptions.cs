// Copyright 2015 Google Inc. All Rights Reserved.
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

namespace AccessBridgeExplorer.WindowsAccessBridge {
  /// <summary>
  /// Flags specifiying what properties to include in <see cref="AccessibleNode.GetProperties"/>.
  /// </summary>
  [Flags]
  public enum PropertyOptions {
    AccessibleContextInfo = 0x0001,
    TopLevelWindowInfo = 0x0002,
    VisibleDescendents = 0x0004,
    AccessibleActions = 0x0008,
    AccessibleKeyBindings = 0x0010,
    AccessibleIcons = 0x0020,
    AccessibleRelationSet = 0x0040,
    ParentContext = 0x0080,
    AccessibleText = 0x0100,
    AccessibleValue = 0x0200,
    AccessibleSelection = 0x0400,
    AccessibleTable = 0x0800,
    AccessibleTableCells = 0x1000,
    AccessibleHyperText = 0x2000,
    ObjectDepth = 0x4000,
  }
}