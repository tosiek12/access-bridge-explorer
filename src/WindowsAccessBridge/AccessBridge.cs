using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using AccessBridgeExplorer.Win32;

namespace AccessBridgeExplorer.WindowsAccessBridge {
  /// <summary>
  /// Class used to dynamically load and access the Java Access Bridge DLL.
  /// </summary>
  public class AccessBridge : IDisposable {
    private UnmanagedLibrary _library;
    private AccessBridgeFunctions _functions;
    private readonly AccessBridgeEvents _events;
    private bool _disposed;

    public AccessBridge() {
      _events = new AccessBridgeEvents(this);
    }

    public AccessBridgeFunctions Functions {
      get {
        Initialize();
        return _functions;
      }
    }

    public AccessBridgeEvents Events {
      get { return _events; }
    }

    public void Initialize() {
      ThrowIfDisposed();
      if (_library != null)
        return;

      var library = LoadLibrary();
      var functions = LoadFunctions(library);
      _library = library;
      _functions = functions;
      _events.SetHandlers();
      _functions.Windows_run();
    }

    public void Dispose() {
      if (_disposed)
        return;

      if (_events != null) {
        _events.Dispose();
      }

      if (_library != null) {
        _library.Dispose();
        _functions = null;
        _library = null;
      }

      _disposed = true;
    }

    private void ThrowIfDisposed() {
      if (_disposed)
        throw new ObjectDisposedException("access bridge");
    }

    public List<AccessibleJvm> EnumWindows() {
      Initialize();

      var windows = new List<AccessibleWindow>();
      var success = Win32.NativeMethods.EnumWindows((hWnd, lParam) => {
        var rc = Functions.IsJavaWindow(hWnd);
        if (rc != 0) {
          int vmId;
          IntPtr ac;
          if (Functions.GetAccessibleContextFromHWND(hWnd, out vmId, out ac) != 0) {
            windows.Add(new AccessibleWindow(this, hWnd, new JavaObjectHandle(vmId, ac)));
          }
        }
        return true;
      }, IntPtr.Zero);

      if (!success) {
        var hr = Marshal.GetHRForLastWin32Error();
        Marshal.ThrowExceptionForHR(hr);
      }

      // Group windows by JVM id
      return windows.GroupBy(x => x.JvmId).Select(g => {
        var result = new AccessibleJvm(this, g.Key);
        result.Windows.AddRange(g.OrderBy(x => x.GetDisplaySortString()));
        return result;
      }).OrderBy(x => x.JvmId).ToList();
    }

    private static UnmanagedLibrary LoadLibrary() {
      UnmanagedLibrary library;
      if (IntPtr.Size == 4) {
        library = new UnmanagedLibrary("WindowsAccessBridge-32.dll");
      } else if (IntPtr.Size == 8) {
        library = new UnmanagedLibrary("WindowsAccessBridge-64.dll");
      } else {
        throw new InvalidOperationException("Unknown platform.");
      }

      return library;
    }

    private static AccessBridgeFunctions LoadFunctions(UnmanagedLibrary library) {
      var functions = new AccessBridgeFunctions();
      var publicMembers = BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance;
      foreach (var property in functions.GetType().GetProperties(publicMembers)) {
        var name = property.Name;

        // All entry point names have lower case, except for "Windows_run"
        switch (name) {
          case "Windows_run":
            break;

          default:
            name = char.ToLower(name[0], CultureInfo.InvariantCulture) + name.Substring(1);
            break;
        }

        // Setters hae a "FP" suffix
        if (name.StartsWith("set")) {
          name += "FP";
        }

        try {
          var function = library.GetUnmanagedFunction(name, property.PropertyType);
          if (function == null) {
            throw new ApplicationException(string.Format("Function {0} not found in AccessBridge", name));
          }
          property.SetValue(functions, function, null);
        } catch (Exception e) {
          throw new ArgumentException(string.Format("Error loading function {0} from access bridge library", name), e);
        }
      }
      return functions;
    }
  }
}
