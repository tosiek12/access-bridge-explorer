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
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace AccessBridgeExplorer.WindowsAccessBridge {
  /// <summary>
  /// Helper class used to forward native events from the Java Access Bridge to
  /// an instance of <see cref="AccessBridgeEvents"/>.
  /// </summary>
  public class AccessBridgeEventsNative {
    private readonly AccessBridgeLibraryFunctions _libraryFunctions;
    private readonly List<Delegate> _handlers = new List<Delegate>();

    public AccessBridgeEventsNative(AccessBridgeLibraryFunctions libraryFunctions) {
      _libraryFunctions = libraryFunctions;
    }

    public void SetHandlers() {
      // Call "_bridge.Functions.SetXxxFP(OnXxx)" for all events.
      var publicMembers = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;
      var privateMembers = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic;
      foreach (var evt in GetType().GetEvents(publicMembers)) {
        var name = evt.Name;

        // Create delegate to the "OnXxx" instance method
        var methodName = "On" + evt.Name;
        var method = GetType().GetMethod(methodName, privateMembers);
        var handler = Delegate.CreateDelegate(evt.EventHandlerType, this, method);
        _handlers.Add(handler); // Hold delegate instance in list so it does not get GC'ed

        // Find the native access bridge event setter method to call
        var setName = "Set" + name;
        var propertyMethod = _libraryFunctions.GetType().GetProperty(setName, publicMembers);
        if (propertyMethod == null) {
          throw new ApplicationException(string.Format("Type \"{0}\" should contain a method named \"{1}\"",
            _libraryFunctions.GetType().Name, setName));
        }
        var getter = propertyMethod.GetGetMethod();
        var action = getter.Invoke(_libraryFunctions, new object[] { });

        // Call it with the handler instance
        ((Delegate)action).DynamicInvoke(new object[] { handler });
      }
    }

    public void ReleaseHandlers() {
      // Call "_bridge.Functions.SetXxxFP(null)" for all events.
      var publicMembers = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;
      foreach (var evt in GetType().GetEvents(publicMembers)) {
        var name = evt.Name;

        // Find the native access bridge event setter method to call
        var setName = "Set" + name;
        var getter = _libraryFunctions.GetType().GetProperty(setName, publicMembers).GetGetMethod();
        var action = getter.Invoke(_libraryFunctions, new object[] { });

        // Call it with "null"
        ((Delegate)action).DynamicInvoke(new object[] { null });
      }

      // We don't need to hold ref. to delegate instances anymore
      _handlers.Clear();
    }

    private JavaObjectHandle Wrap(int vmid, JOBJECT64 javaObject) {
      return new JavaObjectHandle(vmid, javaObject);
    }

    #region List of events
    /// <summary>
    /// These events are used at run-time to determine the callbacks to send to
    /// the WindowsAccessBridge DLL.
    /// </summary>
#pragma warning disable 67
    public event AccessBridgeLibraryFunctions.PropertyChangeEventHandler PropertyChange;

    public event AccessBridgeLibraryFunctions.JavaShutdownEventHandler JavaShutdown;

    public event AccessBridgeLibraryFunctions.FocusGainedEventHandler FocusGained;
    public event AccessBridgeLibraryFunctions.FocusLostEventHandler FocusLost;

    public event AccessBridgeLibraryFunctions.CaretUpdateEventHandler CaretUpdate;

    //TODO: Hooking up any of these event break the JBTabsImpl of IntelliJ: Clicking on
    // a TabLabel does not activate the tab anymore.
#if false
    public event AccessBridgeLibraryFunctions.MouseClickedEventHandler MouseClicked;
    public event AccessBridgeLibraryFunctions.MouseEnteredEventHandler MouseEntered;
    public event AccessBridgeLibraryFunctions.MouseExitedEventHandler MouseExited;
    public event AccessBridgeLibraryFunctions.MousePressedEventHandler MousePressed;
    public event AccessBridgeLibraryFunctions.MouseReleasedEventHandler MouseReleased;
#endif

    public event AccessBridgeLibraryFunctions.MenuCanceledEventHandler MenuCanceled;
    public event AccessBridgeLibraryFunctions.MenuDeselectedEventHandler MenuDeselected;
    public event AccessBridgeLibraryFunctions.MenuSelectedEventHandler MenuSelected;
    public event AccessBridgeLibraryFunctions.PopupMenuCanceledEventHandler PopupMenuCanceled;
    public event AccessBridgeLibraryFunctions.PopupMenuWillBecomeInvisibleEventHandler PopupMenuWillBecomeInvisible;
    public event AccessBridgeLibraryFunctions.PopupMenuWillBecomeVisibleEventHandler PopupMenuWillBecomeVisible;

    public event AccessBridgeLibraryFunctions.PropertyNameChangeEventHandler PropertyNameChange;
    public event AccessBridgeLibraryFunctions.PropertyDescriptionChangeEventHandler PropertyDescriptionChange;
    public event AccessBridgeLibraryFunctions.PropertyStateChangeEventHandler PropertyStateChange;
    public event AccessBridgeLibraryFunctions.PropertyValueChangeEventHandler PropertyValueChange;
    public event AccessBridgeLibraryFunctions.PropertySelectionChangeEventHandler PropertySelectionChange;
    public event AccessBridgeLibraryFunctions.PropertyTextChangeEventHandler PropertyTextChange;
    public event AccessBridgeLibraryFunctions.PropertyCaretChangeEventHandler PropertyCaretChange;
    public event AccessBridgeLibraryFunctions.PropertyVisibleDataChangeEventHandler PropertyVisibleDataChange;
    public event AccessBridgeLibraryFunctions.PropertyChildChangeEventHandler PropertyChildChange;
    public event AccessBridgeLibraryFunctions.PropertyActiveDescendentChangeEventHandler PropertyActiveDescendentChange;

    public event AccessBridgeLibraryFunctions.PropertyTableModelChangeEventHandler PropertyTableModelChange;

#pragma warning restore 67
    #endregion

    #region Event Handlers
    protected virtual void OnJavaShutdown(int vmid) {
      //_libraryFunctions.OnJavaShutdown(vmid);
    }

    protected virtual void OnPropertyChange(int vmid, JOBJECT64 evt, JOBJECT64 source, string property, string oldvalue, string newvalue) {
      //_libraryFunctions.OnPropertyChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), property, oldvalue, newvalue);
    }

    protected virtual void OnPropertyChildChange(int vmid, JOBJECT64 evt, JOBJECT64 source, JOBJECT64 oldchild, JOBJECT64 newchild) {
      //_libraryFunctions.OnPropertyChildChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), Wrap(vmid, oldchild), Wrap(vmid, newchild));
    }

    protected virtual void OnFocusGained(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      //_libraryFunctions.OnFocusGained(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnFocusLost(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      //_libraryFunctions.OnFocusLost(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnCaretUpdate(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      //_libraryFunctions.OnCaretUpdate(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnMouseClicked(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      //_libraryFunctions.OnMouseClicked(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnMouseEntered(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      //_libraryFunctions.OnMouseEntered(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnMouseExited(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      //_libraryFunctions.OnMouseExited(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnMousePressed(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      //_libraryFunctions.OnMousePressed(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnMouseReleased(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      //_libraryFunctions.OnMouseReleased(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnMenuCanceled(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      //_libraryFunctions.OnMenuCanceled(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnMenuDeselected(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      //_libraryFunctions.OnMenuDeselected(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnMenuSelected(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      //_libraryFunctions.OnMenuSelected(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnPopupMenuCanceled(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      //_libraryFunctions.OnPopupMenuCanceled(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnPopupMenuWillBecomeInvisible(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      //_libraryFunctions.OnPopupMenuWillBecomeInvisible(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnPopupMenuWillBecomeVisible(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      //_libraryFunctions.OnPopupMenuWillBecomeVisible(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnPropertyNameChange(int vmid, JOBJECT64 evt, JOBJECT64 source, string oldname, string newname) {
      //_libraryFunctions.OnPropertyNameChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), oldname, newname);
    }

    protected virtual void OnPropertyDescriptionChange(int vmid, JOBJECT64 evt, JOBJECT64 source, string olddescription, string newdescription) {
      //_libraryFunctions.OnPropertyDescriptionChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), olddescription, newdescription);
    }

    protected virtual void OnPropertyStateChange(int vmid, JOBJECT64 evt, JOBJECT64 source, string oldstate, string newstate) {
      //_libraryFunctions.OnPropertyStateChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), oldstate, newstate);
    }

    protected virtual void OnPropertyValueChange(int vmid, JOBJECT64 evt, JOBJECT64 source, string oldvalue, string newvalue) {
      //_libraryFunctions.OnPropertyValueChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), oldvalue, newvalue);
    }

    protected virtual void OnPropertySelectionChange(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      //_libraryFunctions.OnPropertySelectionChange(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnPropertyTextChange(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      //_libraryFunctions.OnPropertyTextChange(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnPropertyCaretChange(int vmid, JOBJECT64 evt, JOBJECT64 source, int oldposition, int newposition) {
      //_libraryFunctions.OnPropertyCaretChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), oldposition, newposition);
    }

    protected virtual void OnPropertyVisibleDataChange(int vmid, JOBJECT64 evt, JOBJECT64 source) {
      //_libraryFunctions.OnPropertyVisibleDataChange(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnPropertyActiveDescendentChange(int vmid, JOBJECT64 evt, JOBJECT64 source, JOBJECT64 oldactivedescendent, JOBJECT64 newactivedescendent) {
      //_libraryFunctions.OnPropertyActiveDescendentChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), Wrap(vmid, oldactivedescendent), Wrap(vmid, newactivedescendent));
    }

    protected virtual void OnPropertyTableModelChange(int vmid, JOBJECT64 evt, JOBJECT64 source, string oldvalue, string newvalue) {
      //_libraryFunctions.OnPropertyTableModelChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), oldvalue, newvalue);
    }

    #endregion
  }
}