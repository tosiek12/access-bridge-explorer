using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace AccessBridgeExplorer.WindowsAccessBridge {
  /// <summary>
  /// Safe handle for all java objects returned from the WindowAccessBridge dll.
  /// This ensures that all java objects references are released when not used
  /// by any C# object.
  /// </summary>
  public class JavaObjectHandle : SafeHandleZeroOrMinusOneIsInvalid {
    private readonly int _jvmId;

    public JavaObjectHandle(int jvmId, IntPtr handle) : base(true) {
      _jvmId = jvmId;
      this.handle = handle;
    }

    public bool IsNull { get { return IsInvalid; } }

    public int JvmId {
      get { return _jvmId; }
    }

    protected override bool ReleaseHandle() {
      // Note: We need the "ReleaseXxx" method to be static, as we can't depend
      // on any other managed object for calling into the WindowsAccessBridge
      // DLL. Also, we depend on the fact the CLR tries to load the method from
      // the DLL only when the method is actually called. This allows us to work
      // correctly on either a 64-bit or 32-bit.
      if (IntPtr.Size == 4) {
        ReleaseJavaObjectFP_32(_jvmId, handle);
      } else {
        ReleaseJavaObjectFP_64(_jvmId, handle);
      }
      return true;
    }

    [DllImport("WindowsAccessBridge-32.dll", EntryPoint = "releaseJavaObject", CallingConvention = CallingConvention.Cdecl)]
    private static extern void ReleaseJavaObjectFP_32(int jvmId, IntPtr javaObject);

    [DllImport("WindowsAccessBridge-64.dll", EntryPoint = "releaseJavaObject", CallingConvention = CallingConvention.Cdecl)]
    private static extern void ReleaseJavaObjectFP_64(int jvmId, IntPtr javaObject);
  }
}