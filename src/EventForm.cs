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
using System.Windows.Forms;
using AccessBridgeExplorer.Model;

namespace AccessBridgeExplorer {
  public partial class EventForm : Form {
    private readonly PropertyListViewWrapper _accessibleContextPropertyListWrapper;
    private readonly PropertyListViewWrapper _oldValuePropertyListWrapper;
    private readonly PropertyListViewWrapper _newValuePropertyListWrapper;

    public EventForm() {
      InitializeComponent();
      _accessibleContextPropertyListWrapper = new PropertyListViewWrapper(accessibleContextPropertyList, propertyImageList);
      _oldValuePropertyListWrapper = new PropertyListViewWrapper(oldValuePropertyList, propertyImageList);
      _newValuePropertyListWrapper = new PropertyListViewWrapper(newValuePropertyList, propertyImageList);

      //accessibleContextTabPage.Enter += (sender, args) => OnContextNodeSelect();
      //oldValuePage.Enter += (sender, args) => OnOldValueSelect();
      //newValuePage.Enter += (sender, args) => OnNewValueSelect();
    }

    public event EventHandler ContextNodeSelect;
    public event EventHandler OldValueSelect;
    public event EventHandler NewValueSelect;

    public void SetContextNodePropertyList(PropertyList propertyList) {
      _accessibleContextPropertyListWrapper.SetPropertyList(propertyList);
    }

    public void SetOldValuePropertyList(PropertyList propertyList) {
      _oldValuePropertyListWrapper.SetPropertyList(propertyList);
    }

    public void SetNewValuePropertyList(PropertyList propertyList) {
      _newValuePropertyListWrapper.SetPropertyList(propertyList);
    }

    protected virtual void OnContextNodeSelect() {
      var handler = ContextNodeSelect;
      if (handler != null) handler(this, EventArgs.Empty);
    }

    protected virtual void OnOldValueSelect() {
      var handler = OldValueSelect;
      if (handler != null) handler(this, EventArgs.Empty);
    }

    protected virtual void OnNewValueSelect() {
      var handler = NewValueSelect;
      if (handler != null) handler(this, EventArgs.Empty);
    }
  }
}
