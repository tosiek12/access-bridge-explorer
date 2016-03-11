// Copyright 2016 Google Inc. All Rights Reserved.
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

namespace AccessBridgeExplorer.WindowsAccessBridge {
  /// <summary>
  /// Single entry point to access all functions exposed by the Java Access Bridge
  /// DLL (<see cref="AccessBridge.Functions"/>).
  /// </summary>
  public partial class AccessBridgeFunctions {
    private readonly AccessBridgeLibraryFunctions _libraryFunctions;

    public AccessBridgeFunctions(AccessBridgeLibraryFunctions libraryFunctions) {
      _libraryFunctions = libraryFunctions;
    }

    public AccessBridgeLibraryFunctions LibraryFunctions {
      get { return _libraryFunctions; }
    }

    private bool ToBool(int value) {
      return value != 0;
    }

    private bool Succeeded(int value) {
      return ToBool(value);
    }

    private JavaObjectHandle Wrap(int vmid, JOBJECT64 handle) {
      return new JavaObjectHandle(vmid, handle);
    }

    private JOBJECT64 Unwrap(int vmid, JavaObjectHandle objectHandle) {
      return objectHandle.Handle;
    }
  }
}