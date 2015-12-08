using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
// ReSharper disable InconsistentNaming

namespace AccessBridgeExplorer.WindowsAccessBridge {
  #region Native Event Handlers Delegate Definitions
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_PropertyChangeFP(int vmid, IntPtr evt, IntPtr source, [MarshalAs(UnmanagedType.LPWStr)]string property, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue);

  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_JavaShutdownFP(int vmid);

  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_FocusGainedFP(int vmid, IntPtr evt, IntPtr source);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_FocusLostFP(int vmid, IntPtr evt, IntPtr source);

  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_CaretUpdateFP(int vmid, IntPtr evt, IntPtr source);

  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_MouseClickedFP(int vmid, IntPtr evt, IntPtr source);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_MouseEnteredFP(int vmid, IntPtr evt, IntPtr source);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_MouseExitedFP(int vmid, IntPtr evt, IntPtr source);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_MousePressedFP(int vmid, IntPtr evt, IntPtr source);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_MouseReleasedFP(int vmid, IntPtr evt, IntPtr source);

  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_MenuCanceledFP(int vmid, IntPtr evt, IntPtr source);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_MenuDeselectedFP(int vmid, IntPtr evt, IntPtr source);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_MenuSelectedFP(int vmid, IntPtr evt, IntPtr source);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_PopupMenuCanceledFP(int vmid, IntPtr evt, IntPtr source);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_PopupMenuWillBecomeInvisibleFP(int vmid, IntPtr evt, IntPtr source);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_PopupMenuWillBecomeVisibleFP(int vmid, IntPtr evt, IntPtr source);

  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_PropertyNameChangeFP(int vmid, IntPtr evt, IntPtr source, [MarshalAs(UnmanagedType.LPWStr)]string oldName, [MarshalAs(UnmanagedType.LPWStr)]string newName);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_PropertyDescriptionChangeFP(int vmid, IntPtr evt, IntPtr source, [MarshalAs(UnmanagedType.LPWStr)]string oldDescription, [MarshalAs(UnmanagedType.LPWStr)]string newDescription);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_PropertyStateChangeFP(int vmid, IntPtr evt, IntPtr source, [MarshalAs(UnmanagedType.LPWStr)]string oldState, [MarshalAs(UnmanagedType.LPWStr)]string newState);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_PropertyValueChangeFP(int vmid, IntPtr evt, IntPtr source, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_PropertySelectionChangeFP(int vmid, IntPtr evt, IntPtr source);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_PropertyTextChangeFP(int vmid, IntPtr evt, IntPtr source);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_PropertyCaretChangeFP(int vmid, IntPtr evt, IntPtr source, int oldPosition, int newPosition);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_PropertyVisibleDataChangeFP(int vmid, IntPtr evt, IntPtr source);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_PropertyChildChangeFP(int vmid, IntPtr evt, IntPtr source, IntPtr oldchild, IntPtr newchild);
  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_PropertyActiveDescendentChangeFP(int vmid, IntPtr evt, IntPtr source, IntPtr oldActiveDescendent, IntPtr newActiveDescendent);

  [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
  public delegate void AccessBridge_PropertyTableModelChangeFP(int vmID, IntPtr evt, IntPtr source, [MarshalAs(UnmanagedType.LPWStr)]string oldValue, [MarshalAs(UnmanagedType.LPWStr)]string newValue);
  #endregion

  /// <summary>
  /// Helper class used to forward native events from the Java Access Bridge to
  /// an instance of <see cref="AccessBridgeEvents"/>.
  /// </summary>
  public class AccessBridgeEventsNative {
    private readonly AccessBridgeEvents _accessBridgeEvents;
    private readonly List<Delegate> _handlers = new List<Delegate>();

    public AccessBridgeEventsNative(AccessBridgeEvents accessBridgeEvents) {
      _accessBridgeEvents = accessBridgeEvents;
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
        var propertyMethod = _accessBridgeEvents.Bridge.Functions.GetType().GetProperty(setName, publicMembers);
        if (propertyMethod == null) {
          throw new ApplicationException(string.Format("Type \"{0}\" should contain a method named \"{1}\"",
            _accessBridgeEvents.Bridge.Functions.GetType().Name, setName));
        }
        var getter = propertyMethod.GetGetMethod();
        var action = getter.Invoke(_accessBridgeEvents.Bridge.Functions, new object[] { });

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
        var getter = _accessBridgeEvents.Bridge.Functions.GetType().GetProperty(setName, publicMembers).GetGetMethod();
        var action = getter.Invoke(_accessBridgeEvents.Bridge.Functions, new object[] { });

        // Call it with "null"
        ((Delegate)action).DynamicInvoke(new object[] { null });
      }

      // We don't need to hold ref. to delegate instances anymore
      _handlers.Clear();
    }

    private JavaObjectHandle Wrap(int vmid, IntPtr javaObject) {
      return new JavaObjectHandle(vmid, javaObject);
    }

    #region List of events
    /// <summary>
    /// These events are used at run-time to determine the callbacks to send to
    /// the WindowsAccessBridge DLL.
    /// </summary>
#pragma warning disable 67
    public event AccessBridge_PropertyChangeFP PropertyChange;

    public event AccessBridge_JavaShutdownFP JavaShutdown;

    public event AccessBridge_FocusGainedFP FocusGained;
    public event AccessBridge_FocusLostFP FocusLost;

    public event AccessBridge_CaretUpdateFP CaretUpdate;

    //TODO: Hooking up any of these event break the JBTabsImpl of IntelliJ: Clicking on
    // a TabLabel does not activate the tab anymore.
#if false
    public event AccessBridge_MouseClickedFP MouseClicked;
    public event AccessBridge_MouseEnteredFP MouseEntered;
    public event AccessBridge_MouseExitedFP MouseExited;
    public event AccessBridge_MousePressedFP MousePressed;
    public event AccessBridge_MouseReleasedFP MouseReleased;
#endif

    public event AccessBridge_MenuCanceledFP MenuCanceled;
    public event AccessBridge_MenuDeselectedFP MenuDeselected;
    public event AccessBridge_MenuSelectedFP MenuSelected;
    public event AccessBridge_PopupMenuCanceledFP PopupMenuCanceled;
    public event AccessBridge_PopupMenuWillBecomeInvisibleFP PopupMenuWillBecomeInvisible;
    public event AccessBridge_PopupMenuWillBecomeVisibleFP PopupMenuWillBecomeVisible;

    public event AccessBridge_PropertyNameChangeFP PropertyNameChange;
    public event AccessBridge_PropertyDescriptionChangeFP PropertyDescriptionChange;
    public event AccessBridge_PropertyStateChangeFP PropertyStateChange;
    public event AccessBridge_PropertyValueChangeFP PropertyValueChange;
    public event AccessBridge_PropertySelectionChangeFP PropertySelectionChange;
    public event AccessBridge_PropertyTextChangeFP PropertyTextChange;
    public event AccessBridge_PropertyCaretChangeFP PropertyCaretChange;
    public event AccessBridge_PropertyVisibleDataChangeFP PropertyVisibleDataChange;
    public event AccessBridge_PropertyChildChangeFP PropertyChildChange;
    public event AccessBridge_PropertyActiveDescendentChangeFP PropertyActiveDescendentChange;

    public event AccessBridge_PropertyTableModelChangeFP PropertyTableModelChange;

#pragma warning restore 67
    #endregion

    #region Event Handlers
    protected virtual void OnJavaShutdown(int vmid) {
      _accessBridgeEvents.OnJavaShutdown(vmid);
    }

    protected virtual void OnPropertyChange(int vmid, IntPtr evt, IntPtr source, string property, string oldvalue, string newvalue) {
      _accessBridgeEvents.OnPropertyChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), property, oldvalue, newvalue);
    }

    protected virtual void OnPropertyChildChange(int vmid, IntPtr evt, IntPtr source, IntPtr oldchild, IntPtr newchild) {
      _accessBridgeEvents.OnPropertyChildChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), Wrap(vmid, oldchild), Wrap(vmid, newchild));
    }

    protected virtual void OnFocusGained(int vmid, IntPtr evt, IntPtr source) {
      _accessBridgeEvents.OnFocusGained(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnFocusLost(int vmid, IntPtr evt, IntPtr source) {
      _accessBridgeEvents.OnFocusLost(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnCaretUpdate(int vmid, IntPtr evt, IntPtr source) {
      _accessBridgeEvents.OnCaretUpdate(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnMouseClicked(int vmid, IntPtr evt, IntPtr source) {
      _accessBridgeEvents.OnMouseClicked(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnMouseEntered(int vmid, IntPtr evt, IntPtr source) {
      _accessBridgeEvents.OnMouseEntered(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnMouseExited(int vmid, IntPtr evt, IntPtr source) {
      _accessBridgeEvents.OnMouseExited(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnMousePressed(int vmid, IntPtr evt, IntPtr source) {
      _accessBridgeEvents.OnMousePressed(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnMouseReleased(int vmid, IntPtr evt, IntPtr source) {
      _accessBridgeEvents.OnMouseReleased(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnMenuCanceled(int vmid, IntPtr evt, IntPtr source) {
      _accessBridgeEvents.OnMenuCanceled(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnMenuDeselected(int vmid, IntPtr evt, IntPtr source) {
      _accessBridgeEvents.OnMenuDeselected(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnMenuSelected(int vmid, IntPtr evt, IntPtr source) {
      _accessBridgeEvents.OnMenuSelected(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnPopupMenuCanceled(int vmid, IntPtr evt, IntPtr source) {
      _accessBridgeEvents.OnPopupMenuCanceled(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnPopupMenuWillBecomeInvisible(int vmid, IntPtr evt, IntPtr source) {
      _accessBridgeEvents.OnPopupMenuWillBecomeInvisible(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnPopupMenuWillBecomeVisible(int vmid, IntPtr evt, IntPtr source) {
      _accessBridgeEvents.OnPopupMenuWillBecomeVisible(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnPropertyNameChange(int vmid, IntPtr evt, IntPtr source, string oldname, string newname) {
      _accessBridgeEvents.OnPropertyNameChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), oldname, newname);
    }

    protected virtual void OnPropertyDescriptionChange(int vmid, IntPtr evt, IntPtr source, string olddescription, string newdescription) {
      _accessBridgeEvents.OnPropertyDescriptionChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), olddescription, newdescription);
    }

    protected virtual void OnPropertyStateChange(int vmid, IntPtr evt, IntPtr source, string oldstate, string newstate) {
      _accessBridgeEvents.OnPropertyStateChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), oldstate, newstate);
    }

    protected virtual void OnPropertyValueChange(int vmid, IntPtr evt, IntPtr source, string oldvalue, string newvalue) {
      _accessBridgeEvents.OnPropertyValueChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), oldvalue, newvalue);
    }

    protected virtual void OnPropertySelectionChange(int vmid, IntPtr evt, IntPtr source) {
      _accessBridgeEvents.OnPropertySelectionChange(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnPropertyTextChange(int vmid, IntPtr evt, IntPtr source) {
      _accessBridgeEvents.OnPropertyTextChange(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnPropertyCaretChange(int vmid, IntPtr evt, IntPtr source, int oldposition, int newposition) {
      _accessBridgeEvents.OnPropertyCaretChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), oldposition, newposition);
    }

    protected virtual void OnPropertyVisibleDataChange(int vmid, IntPtr evt, IntPtr source) {
      _accessBridgeEvents.OnPropertyVisibleDataChange(vmid, Wrap(vmid, evt), Wrap(vmid, source));
    }

    protected virtual void OnPropertyActiveDescendentChange(int vmid, IntPtr evt, IntPtr source, IntPtr oldactivedescendent, IntPtr newactivedescendent) {
      _accessBridgeEvents.OnPropertyActiveDescendentChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), Wrap(vmid, oldactivedescendent), Wrap(vmid, newactivedescendent));
    }

    protected virtual void OnPropertyTableModelChange(int vmid, IntPtr evt, IntPtr source, string oldvalue, string newvalue) {
      _accessBridgeEvents.OnPropertyTableModelChange(vmid, Wrap(vmid, evt), Wrap(vmid, source), oldvalue, newvalue);
    }

    #endregion
  }
}