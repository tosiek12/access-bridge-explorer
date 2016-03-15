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
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AccessBridgeExplorer {
  public partial class TooltipWindow : Form {
    public TooltipWindow() {
      InitializeComponent();
    }

    public enum GWL {
      ExStyle = -20
    }

    [Flags]
    public enum WS_EX {
      Transparent = 0x20,
      /// <summary>
      /// Do not show up in alt-tab list
      /// </summary>
      ToolWindow = 0x80,
      /// <summary>
      /// See https://msdn.microsoft.com/en-us/library/windows/desktop/ms632599(v=vs.85).aspx#layered
      /// Using a layered window can significantly improve performance and
      /// visual effects for a window that has a complex shape, animates its
      /// shape, or wishes to use alpha blending effects. The system
      /// automatically composes and repaints layered windows and the windows of
      /// underlying applications. As a result, layered windows are rendered
      /// smoothly, without the flickering typical of complex window regions. In
      /// addition, layered windows can be partially translucent, that is,
      /// alpha-blended.
      /// </summary>
      Layered = 0x80000,
    }

    public enum LWA {
      ColorKey = 0x1,
      Alpha = 0x2
    }

    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    public static extern int GetWindowLong(IntPtr hWnd, GWL nIndex);

    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    public static extern int SetWindowLong(IntPtr hWnd, GWL nIndex, int dwNewLong);

    [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
    public static extern bool SetLayeredWindowAttributes(IntPtr hWnd, int crKey, byte alpha, LWA dwFlags);

    protected override void OnShown(EventArgs e) {
      base.OnShown(e);
      User32Utils.SetTransparentLayeredWindowStyle(new HandleRef(this, this.Handle), 230);
    }

    protected override bool ShowWithoutActivation {
      get { return true; }
    }
  }
}
