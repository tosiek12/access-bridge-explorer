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
using System.Diagnostics;
using System.Windows.Forms;

namespace AccessBridgeExplorer {
  public partial class ExceptionForm : Form {
    private readonly TreeListView _treeListView;

    public ExceptionForm() {
      InitializeComponent();
      _treeListView = new TreeListView(errorDetailListView, errorDetailListViewImageList);
    }

    public void DisplayError(Exception error, StackTrace stackTrace) {
      var topLevelNode = new TreeNode();
      CreateNodes(topLevelNode.Nodes, error, stackTrace);
      topLevelNode.ExpandAll();

      _treeListView.SetNodes(topLevelNode.Nodes);

      errorDetailListView.Select();
    }

    private void CreateNodes(TreeNodeCollection parentList, Exception error, StackTrace stackTrace) {
      var errorNode = parentList.Add("Exception");
      CreateErrorNodes(errorNode.Nodes, error);

      var stackNode = parentList.Add("Stacktrace");
      CreateStackStraceNodes(stackNode.Nodes, stackTrace);
    }

    private static void CreateErrorNodes(TreeNodeCollection parentList, Exception error) {
      parentList.Add(string.Format("Message: {0}", error.Message));
      parentList.Add(string.Format("Type: {0}", error.GetType().FullName));

      var stackTrace = new StackTrace(error, fNeedFileInfo: true);
      var node = parentList.Add("Stacktrace");
      CreateStackStraceNodes(node.Nodes, stackTrace);

      if (error.InnerException != null) {
        var errorNode = parentList.Add("Inner Exception");
        CreateErrorNodes(errorNode.Nodes, error.InnerException);
      }
    }

    private static void CreateStackStraceNodes(TreeNodeCollection parentList, StackTrace stackTrace) {
      var frames = stackTrace.GetFrames() ?? new StackFrame[0];
      foreach (var frame in frames) {
        var frameNode = new TreeNode();

        var method = frame.GetMethod() == null
          ? @"<Unknown method>"
          : string.Format("{0}.{1}()", frame.GetMethod().ReflectedType.FullName, frame.GetMethod().Name);

        var location = frame.GetFileName() == null
          ? ""
          : string.Format(" - {0}:{1}:{2}", frame.GetFileName(), frame.GetFileLineNumber(), frame.GetFileColumnNumber());

        frameNode.Text = string.Format("{0}{1}", method, location);
        parentList.Add(frameNode);
      }
    }
  }
}
