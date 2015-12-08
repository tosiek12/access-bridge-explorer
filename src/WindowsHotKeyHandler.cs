using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AccessBridgeExplorer {
  public class WindowsHotKeyHandler : IMessageFilter, IDisposable {
    private Control _control;
    private int _id;
    // ReSharper disable once InconsistentNaming
    private const uint WM_HOTKEY = 0x312;

    [Flags]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum Modifiers : uint {
      MOD_ALT = 0x1,
      MOD_CONTROL = 0x2,
      MOD_SHIFT = 0x4,
      MOD_WIN = 0x8,
    }

    public event EventHandler KeyPressed;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="control"></param>
    /// <param name="id">The identifier of the hot key. If a hot key already
    /// exists with the same control and id parameters, see Remarks for the
    /// action taken. An application must specify an id value in the range
    /// 0x0000 through 0xBFFF. A shared DLL must specify a value in the range
    /// 0xC000 through 0xFFFF (the range returned by the GlobalAddAtom
    /// function). To avoid conflicts with hot-key identifiers defined by other
    /// shared DLLs, a DLL should use the GlobalAddAtom function to obtain the
    /// hot-key identifier.</param>
    /// <param name="key"></param>
    public void Register(Control control, int id, Keys key) {
      if (this._control != null) {
        throw new InvalidOperationException("Hotkey already registered");
      }

      Application.AddMessageFilter(this);
      try {
        var modifiers = default(Modifiers);
        if ((key & Keys.Shift) != 0) modifiers |= Modifiers.MOD_SHIFT;
        if ((key & Keys.Control) != 0) modifiers |= Modifiers.MOD_CONTROL;
        if ((key & Keys.Alt) != 0) modifiers |= Modifiers.MOD_ALT;
        //if ((key & Keys.Win) != 0) modifiers |= Modifiers.MOD_WIN;

        // Register the hotkey
        if (RegisterHotKey(control.Handle, id, (uint) modifiers, key & Keys.KeyCode) == 0) {
          throw new Win32Exception(Marshal.GetLastWin32Error(), "Error registering global hotkey");
        }
      } catch {
        Application.RemoveMessageFilter(this);
        throw;
      }
      _control = control;
      _id = id;
    }

    public void Dispose() {
      Unregister();
    }

    public void Unregister() {
      if (_control == null)
        return;

      if (UnregisterHotKey(_control.Handle, _id) == 0) {
        throw new Win32Exception(Marshal.GetLastWin32Error(), "Error un-registering global hotkey");
      }

      Application.RemoveMessageFilter(this);
      _control = null;
      _id = 0;
    }

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, Keys vk);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int UnregisterHotKey(IntPtr hWnd, int id);

    protected virtual void OnKeyPressed() {
      var handler = KeyPressed;
      if (handler != null) handler(this, EventArgs.Empty);
    }

    public bool PreFilterMessage(ref Message m) {
      // Only process WM_HOTKEY
      if (m.Msg != WM_HOTKEY)
        return false;

      // Only process our "id".
      var id = m.WParam.ToInt32();
      if (_control == null || _id != id)
        return false;

      OnKeyPressed();
      // TODO: Never eat the keystroke?
      return false;
    }
  }
}
