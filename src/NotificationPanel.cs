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
using System.Drawing;
using System.Windows.Forms;

namespace AccessBridgeExplorer {
  public partial class NotificationPanel : UserControl {
    private bool _shown;
    private bool _resizing;

    public NotificationPanel() {
      InitializeComponent();
      textBox.TextChanged += (sender, args) => {
        SetHeightFromText();
      };
      this.Load += (sender, args) => {
        if (!DesignMode) {
          Height = 0;
        }
      };
      this.Resize += (sender, args) => {
        SetHeightFromText();
      };
    }

    public override Color ForeColor {
      get { return base.ForeColor; }
      set {
        base.ForeColor = value;
        panel1.BackColor = value;
        textBox.BackColor = value;
      }
    }

    public override Color BackColor {
      get { return base.BackColor; }
      set {
        base.BackColor = value;
        panel1.BackColor = value;
        textBox.BackColor = value;
      }
    }

    private void SetHeightFromText() {
      if (!_shown)
        return;

      if (_resizing)
        return;
      _resizing = true;
      try {
        int numLines = this.textBox.GetLineFromCharIndex(this.textBox.TextLength) + 1;
        var height = textBox.Font.Height * numLines;
        Height = height + 4 + panel1.Padding.Top + panel1.Padding.Bottom;
      } finally {
        _resizing = false;
      }
    }

    public void ShowNotification(string text) {
      _shown = true;
      textBox.Text = text;
    }

    private void closeButton_Click(object sender, EventArgs e) {
      _shown = false;
      Height = 0;
    }
    private void textBox_LinkClicked(object sender, LinkClickedEventArgs e) {
      System.Diagnostics.Process.Start(e.LinkText);
    }
  }
}
