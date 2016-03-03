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
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using AccessBridgeExplorer.Win32;

namespace AccessBridgeExplorer.WindowsAccessBridge {
  /// <summary>
  /// Class used to dynamically load and access the Java Access Bridge DLL.
  /// </summary>
  public class AccessBridge : IDisposable {
    private UnmanagedLibrary _library;
    private AccessBridgeFunctions _functions;
    private AccessBridgeEvents _events;
    private bool _disposed;

    public AccessBridge() {
    }

    public AccessBridgeFunctions Functions {
      get {
        ThrowIfDisposed();
        Initialize();
        return _functions;
      }
    }

    public AccessBridgeEvents Events {
      get {
        ThrowIfDisposed();
        Initialize();
        return _events;
      }
    }

    public UnmanagedLibrary Library {
      get {
        ThrowIfDisposed();
        Initialize();
        return _library;
      }
    }

    public bool IsLoaded { get { return _library != null; } }

    public void Initialize() {
      ThrowIfDisposed();
      if (_library != null)
        return;

      var library = LoadLibrary();
      var functions = LoadFunctions(library);
      var events = new AccessBridgeEvents(this);

      // Everything is initialized correctly, save to member variables.
      _library = library;
      _functions = functions;
      _events = events;
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
        throw new ObjectDisposedException("Access Bridge library has been disposed");
    }

    public List<AccessibleJvm> EnumWindows() {
      if (_library == null)
        return new List<AccessibleJvm>();

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
      try {
        UnmanagedLibrary library;
        if (IntPtr.Size == 4) {
          library = new UnmanagedLibrary("WindowsAccessBridge-32.dll");
        } else if (IntPtr.Size == 8) {
          library = new UnmanagedLibrary("WindowsAccessBridge-64.dll");
        } else {
          throw new InvalidOperationException("Unknown platform.");
        }
        return library;
      } catch (Exception e) {
        var sb = new StringBuilder();
        sb.Append("Error loading the Java Access Bridge DLL. This usually happens if the Java Access Bridge is not installed. ");
        if (IntPtr.Size == 8)
          sb.Append("Please make sure to install the 64-bit version of the Java SE Runtime Environment version 7 or later.");
        else
          sb.Append("Please make sure to install the 32-bit version of the Java SE Runtime Environment version 7 or later.");
        throw new ApplicationException(sb.ToString(), e);
      }
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
