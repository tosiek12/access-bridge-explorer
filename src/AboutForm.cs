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
using System.Reflection;
using System.Windows.Forms;
using AccessBridgeExplorer.WindowsAccessBridge;

namespace AccessBridgeExplorer {
  public partial class AboutForm : Form {
    public AboutForm() {
      InitializeComponent();
    }

    public void FillForm(Assembly application, AccessBridge accessBridge) {
      applicationNameText.Text = GetAttribute<AssemblyTitleAttribute>(application).Title;
      applicationCopyrightText.Text = GetAttribute<AssemblyCopyrightAttribute>(application).Copyright;
      applicationVersionText.Text = string.Format("Version {0}", application.GetName().Version);

      accessBridgeNameText.Text = @"Access Bridge info:";
      try {
        var libraryVersion = accessBridge.Library.Version;
        accessBridgeProductText.Text = string.Format("Product: {0}", libraryVersion.ProductName);
        accessBridgeVersionText.Text = string.Format("Version: {0}", libraryVersion.FileVersion);
        accessBridgePathText.Text = string.Format("Path: {0}", libraryVersion.FileName);
      } catch (Exception e) {
        accessBridgeProductText.Text = @"<Error loading Access Bridge>";
        accessBridgeVersionText.Text = e.Message;
        accessBridgePathText.Text = "";
      }
    }

    private T GetAttribute<T>(Assembly assembly) where T : Attribute {
      return (T)Attribute.GetCustomAttribute(assembly, typeof(T), false);
    }

    protected override bool ProcessDialogKey(Keys keyData) {
      if (ModifierKeys == Keys.None && keyData == Keys.Escape) {
        Close();
        return true;
      }
      return base.ProcessDialogKey(keyData);
    }

    private void githubUrlLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
      System.Diagnostics.Process.Start(githubUrlLinkLabel.Text);
    }
  }
}
