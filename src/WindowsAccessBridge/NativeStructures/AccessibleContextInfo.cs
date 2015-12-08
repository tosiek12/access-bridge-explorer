using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using jint = System.Int32;

namespace AccessBridgeExplorer.WindowsAccessBridge.NativeStructures {
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct AccessibleContextInfo {

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.MAX_STRING_SIZE)]
    public string name;          // the AccessibleName of the object
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.MAX_STRING_SIZE)]
    public string description;   // the AccessibleDescription of the object

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SHORT_STRING_SIZE)]
    public string role;        // localized AccesibleRole string
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SHORT_STRING_SIZE)]
    public string role_en_US;  // AccesibleRole string in the en_US locale
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SHORT_STRING_SIZE)]
    public string states;      // localized AccesibleStateSet string (comma separated)
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SHORT_STRING_SIZE)]
    public string states_en_US; // AccesibleStateSet string in the en_US locale (comma separated)

    public jint indexInParent;                     // index of object in parent
    public jint childrenCount;                     // # of children, if any

    public jint x;                                 // screen coords in pixels
    public jint y;                                 // "
    public jint width;                             // pixel width of object
    public jint height;                            // pixel height of object

    public jint accessibleComponent;               // flags for various additional
    public jint accessibleAction;                  //  Java Accessibility interfaces
    public jint accessibleSelection;               //  FALSE if this object doesn't
    public jint accessibleText;                    //  implement the additional interface
    //  in question

    // BOOL accessibleValue;                // old BOOL indicating whether AccessibleValue is supported
    public AccessibleInterfaces accessibleInterfaces;              // new bitfield containing additional interface flags
  }
}
