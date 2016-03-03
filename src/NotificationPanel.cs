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
          HideNotification();
        }
      };
      this.Resize += (sender, args) => {
        SetHeightFromText();
      };
    }

    public void AddNotification(string text, NotificationPanelIcon icon) {
      text = text.TrimEnd('\r', '\n');
      textBox.Text = text;
      switch (icon) {
        case NotificationPanelIcon.Error:
          pictureBox1.Image = AccessBridgeExplorer.Properties.Resources.ErrorIcon;
          break;
        case NotificationPanelIcon.Warning:
          pictureBox1.Image = AccessBridgeExplorer.Properties.Resources.WarningIcon;
          break;
        case NotificationPanelIcon.Info:
        default:
          pictureBox1.Image = AccessBridgeExplorer.Properties.Resources.InfoIcon;
          break;
      }
    }

    public void ShowNotification() {
      _shown = true;
      SetHeightFromText();
    }

    public void HideNotification() {
      _shown = false;
      Height = 0;
    }

    public string NotificationText {
      get { return textBox.Text; }
    }

    public bool NotificationShown {
      get { return _shown; }
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

    private void closeButton_Click(object sender, EventArgs e) {
      HideNotification();
    }

    private void textBox_LinkClicked(object sender, LinkClickedEventArgs e) {
      System.Diagnostics.Process.Start(e.LinkText);
    }
  }

  public enum NotificationPanelIcon {
    Info,
    Warning,
    Error,
  }
}
