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
  /// Handle for objects returned from the WindowAccessBridge-XX dll.
  /// This ensures that all java objects references are released when not used
  /// by any C# object.
  /// 
  /// Note we cannot use the <see cref="SafeHandle"/> class, because
  /// the 32-bit version of WindowAccessBridge uses 64-bit pointers
  /// for object handles (<see cref="SafeHandle"/> uses the <see cref="IntPtr"/>
  /// type, which is 32-bit on 32-bit platforms).
  /// </summary>
  public class JavaObjectHandle : IDisposable {
    private readonly int _jvmId;
    private readonly JOBJECT64 _handle;
    private bool _disposed;

    public JavaObjectHandle(int jvmId, JOBJECT64 handle) {
      _jvmId = jvmId;
      _handle = handle;
      if (handle.Value == 0) {
        GC.SuppressFinalize(this);
      }
    }

    ~JavaObjectHandle() {
      Dispose(false);
    }

    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    public void Dispose(bool disposing) {
      if (_disposed)
        return;

      if (_handle.Value != 0) {
        // Note: We need the "ReleaseXxx" method to be static, as we can't depend
        // on any other managed object for calling into the WindowsAccessBridge
        // DLL. Also, we depend on the fact the CLR tries to load the method from
        // the DLL only when the method is actually called. This allows us to work
        // correctly on either a 64-bit or 32-bit.
        if (IntPtr.Size == 4) {
          ReleaseJavaObjectFP_32(_jvmId, _handle);
        } else {
          ReleaseJavaObjectFP_64(_jvmId, _handle);
        }
      }
      _disposed = true;
    }

    public int JvmId {
      get { return _jvmId; }
    }

    public JOBJECT64 Handle {
      get { return _handle; }
    }

    public bool IsClosed {
      get { return _disposed; }
    }

    public bool IsNull {
      get { return _handle.Value == 0; }
    }

    [DllImport("WindowsAccessBridge-32.dll", EntryPoint = "releaseJavaObject", CallingConvention = CallingConvention.Cdecl)]
    private static extern void ReleaseJavaObjectFP_32(int jvmId, JOBJECT64 javaObject);

    [DllImport("WindowsAccessBridge-64.dll", EntryPoint = "releaseJavaObject", CallingConvention = CallingConvention.Cdecl)]
    private static extern void ReleaseJavaObjectFP_64(int jvmId, JOBJECT64 javaObject);
  }
}