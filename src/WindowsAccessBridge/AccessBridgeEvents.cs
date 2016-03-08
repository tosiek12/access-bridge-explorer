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

namespace AccessBridgeExplorer.WindowsAccessBridge {
  /// <summary>
  /// Single entry point to access all events exposed by the Java Access Bridge
  /// DLL (<see cref="AccessBridge.Events"/>).
  /// </summary>
  public class AccessBridgeEvents : IDisposable {
    private readonly AccessBridgeEventsNative _nativeEvents;

    public AccessBridgeEvents(AccessBridgeLibraryFunctions libraryFunctions) {
      _nativeEvents = new AccessBridgeEventsNative(libraryFunctions);
    }

    public void Dispose() {
      _nativeEvents.ReleaseHandlers();
    }

    public void SetHandlers() {
      _nativeEvents.SetHandlers();
    }

    public event PropertyChangeEventHandler PropertyChange;

    public event JavaShutdownEventHandler JavaShutdown;

    public event FocusGainedEventHandler FocusGained;
    public event FocusLostEventHandler FocusLost;

    public event CaretUpdateEventHandler CaretUpdate;

    public event MouseClickedEventHandler MouseClicked;
    public event MouseEnteredEventHandler MouseEntered;
    public event MouseExitedEventHandler MouseExited;
    public event MousePressedEventHandler MousePressed;
    public event MouseReleasedEventHandler MouseReleased;

    public event MenuCanceledEventHandler MenuCanceled;
    public event MenuDeselectedEventHandler MenuDeselected;
    public event MenuSelectedEventHandler MenuSelected;
    public event PopupMenuCanceledEventHandler PopupMenuCanceled;
    public event PopupMenuWillBecomeInvisibleEventHandler PopupMenuWillBecomeInvisible;
    public event PopupMenuWillBecomeVisibleEventHandler PopupMenuWillBecomeVisible;

    public event PropertyNameChangeEventHandler PropertyNameChange;
    public event PropertyDescriptionChangeEventHandler PropertyDescriptionChange;
    public event PropertyStateChangeEventHandler PropertyStateChange;
    public event PropertyValueChangeEventHandler PropertyValueChange;
    public event PropertySelectionChangeEventHandler PropertySelectionChange;
    public event PropertyTextChangeEventHandler PropertyTextChange;
    public event PropertyCaretChangeEventHandler PropertyCaretChange;
    public event PropertyVisibleDataChangeEventHandler PropertyVisibleDataChange;
    public event PropertyChildChangeEventHandler PropertyChildChange;
    public event PropertyActiveDescendentChangeEventHandler PropertyActiveDescendentChange;

    public event PropertyTableModelChangeEventHandler PropertyTableModelChange;

    public virtual void OnJavaShutdown(int vmid) {
      var handler = JavaShutdown;
      if (handler != null) handler(vmid);
    }

    public virtual void OnPropertyChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string property, string oldvalue, string newvalue) {
      var handler = PropertyChange;
      if (handler != null) handler(vmid, evt, source, property, oldvalue, newvalue);
    }

    public virtual void OnPropertyNameChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string oldname, string newname) {
      var handler = PropertyNameChange;
      if (handler != null) handler(vmid, evt, source, oldname, newname);
    }

    public virtual void OnPropertyDescriptionChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string olddescription, string newdescription) {
      var handler = PropertyDescriptionChange;
      if (handler != null) handler(vmid, evt, source, olddescription, newdescription);
    }

    public virtual void OnPropertyStateChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string oldstate, string newstate) {
      var handler = PropertyStateChange;
      if (handler != null) handler(vmid, evt, source, oldstate, newstate);
    }

    public virtual void OnPropertyValueChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, string oldvalue, string newvalue) {
      var handler = PropertyValueChange;
      if (handler != null) handler(vmid, evt, source, oldvalue, newvalue);
    }

    public virtual void OnPropertySelectionChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = PropertySelectionChange;
      if (handler != null) handler(vmid, evt, source);
    }

    public virtual void OnPropertyTextChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = PropertyTextChange;
      if (handler != null) handler(vmid, evt, source);
    }

    public virtual void OnPropertyCaretChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, int oldposition, int newposition) {
      var handler = PropertyCaretChange;
      if (handler != null) handler(vmid, evt, source, oldposition, newposition);
    }

    public virtual void OnPropertyVisibleDataChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = PropertyVisibleDataChange;
      if (handler != null) handler(vmid, evt, source);
    }

    public virtual void OnPropertyChildChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, JavaObjectHandle oldchild, JavaObjectHandle newchild) {
      var handler = PropertyChildChange;
      if (handler != null) handler(vmid, evt, source, oldchild, newchild);
    }

    public virtual void OnPropertyActiveDescendentChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, JavaObjectHandle oldactivedescendent, JavaObjectHandle newactivedescendent) {
      var handler = PropertyActiveDescendentChange;
      if (handler != null) handler(vmid, evt, source, oldactivedescendent, newactivedescendent);
    }

    public virtual void OnFocusGained(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = FocusGained;
      if (handler != null) handler(vmid, evt, source);
    }

    public virtual void OnFocusLost(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = FocusLost;
      if (handler != null) handler(vmid, evt, source);
    }

    public virtual void OnCaretUpdate(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = CaretUpdate;
      if (handler != null) handler(vmid, evt, source);
    }

    public virtual void OnMouseClicked(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = MouseClicked;
      if (handler != null) handler(vmid, evt, source);
    }

    public virtual void OnMouseEntered(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = MouseEntered;
      if (handler != null) handler(vmid, evt, source);
    }

    public virtual void OnMouseExited(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = MouseExited;
      if (handler != null) handler(vmid, evt, source);
    }

    public virtual void OnMousePressed(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = MousePressed;
      if (handler != null) handler(vmid, evt, source);
    }

    public virtual void OnMouseReleased(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = MouseReleased;
      if (handler != null) handler(vmid, evt, source);
    }

    public virtual void OnMenuCanceled(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = MenuCanceled;
      if (handler != null) handler(vmid, evt, source);
    }

    public virtual void OnMenuDeselected(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = MenuDeselected;
      if (handler != null) handler(vmid, evt, source);
    }

    public virtual void OnMenuSelected(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = MenuSelected;
      if (handler != null) handler(vmid, evt, source);
    }

    public virtual void OnPopupMenuCanceled(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = PopupMenuCanceled;
      if (handler != null) handler(vmid, evt, source);
    }

    public virtual void OnPopupMenuWillBecomeInvisible(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = PopupMenuWillBecomeInvisible;
      if (handler != null) handler(vmid, evt, source);
    }

    public virtual void OnPopupMenuWillBecomeVisible(int vmid, JavaObjectHandle evt, JavaObjectHandle source) {
      var handler = PopupMenuWillBecomeVisible;
      if (handler != null) handler(vmid, evt, source);
    }

    public virtual void OnPropertyTableModelChange(int vmid, JavaObjectHandle evt, JavaObjectHandle src, string oldvalue, string newvalue) {
      var handler = PropertyTableModelChange;
      if (handler != null) handler(vmid, evt, src, oldvalue, newvalue);
    }
  }
}
