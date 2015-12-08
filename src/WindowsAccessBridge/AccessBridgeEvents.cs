using System;
using System.Runtime.InteropServices;

namespace AccessBridgeExplorer.WindowsAccessBridge {
  #region Managed Event Handlers Delegate Definitions
  public delegate void PropertyChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source, [MarshalAs(UnmanagedType.LPWStr)]string property, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue);

  public delegate void JavaShutdownEventHandler(int vmid);

  public delegate void FocusGainedEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void FocusLostEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);

  public delegate void CaretUpdateEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);

  public delegate void MouseClickedEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void MouseEnteredEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void MouseExitedEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void MousePressedEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void MouseReleasedEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);

  public delegate void MenuCanceledEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void MenuDeselectedEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void MenuSelectedEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void PopupMenuCanceledEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void PopupMenuWillBecomeInvisibleEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void PopupMenuWillBecomeVisibleEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);

  public delegate void PropertyNameChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source, [MarshalAs(UnmanagedType.LPWStr)]string oldName, [MarshalAs(UnmanagedType.LPWStr)]string newName);
  public delegate void PropertyDescriptionChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source, [MarshalAs(UnmanagedType.LPWStr)]string oldDescription, [MarshalAs(UnmanagedType.LPWStr)]string newDescription);
  public delegate void PropertyStateChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source, [MarshalAs(UnmanagedType.LPWStr)]string oldState, [MarshalAs(UnmanagedType.LPWStr)]string newState);
  public delegate void PropertyValueChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue);
  public delegate void PropertySelectionChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void PropertyTextChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void PropertyCaretChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source, int oldPosition, int newPosition);
  public delegate void PropertyVisibleDataChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source);
  public delegate void PropertyChildChange(int vmid, JavaObjectHandle evt, JavaObjectHandle source, JavaObjectHandle oldChild, JavaObjectHandle newChild);
  public delegate void PropertyActiveDescendentChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle source, JavaObjectHandle oldActiveDescendent, JavaObjectHandle newActiveDescendent);

  public delegate void PropertyTableModelChangeEventHandler(int vmid, JavaObjectHandle evt, JavaObjectHandle src, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue);
  #endregion

  /// <summary>
  /// Single entry point to access all events exposed by the Java Access Bridge
  /// DLL (<see cref="AccessBridge.Events"/>).
  /// </summary>
  public class AccessBridgeEvents : IDisposable {
    private readonly AccessBridge _bridge;
    private readonly AccessBridgeEventsNative _nativeEvents;

    public AccessBridgeEvents(AccessBridge bridge) {
      _bridge = bridge;
      _nativeEvents = new AccessBridgeEventsNative(this);
    }

    public AccessBridge Bridge {
      get { return _bridge; }
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
    public event PropertyChildChange PropertyChildChange;
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
