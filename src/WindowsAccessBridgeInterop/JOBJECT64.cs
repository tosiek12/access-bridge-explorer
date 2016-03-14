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

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace AccessBridgeExplorer.WindowsAccessBridge {
  /// <summary>
  /// Wrapper around a 64-bit integer. This makes code slightly more typesafe
  /// than using Int64 values directly.
  /// </summary>
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  public struct JOBJECT64 {
    public Int64 Value;

    public static JOBJECT64 Zero = default(JOBJECT64);

    public static bool operator ==(JOBJECT64 x, JOBJECT64 y) {
      return x.Value == y.Value;
    }

    public static bool operator !=(JOBJECT64 x, JOBJECT64 y) {
      return x.Value == y.Value;
    }

    public override bool Equals(object obj) {
      if (obj is JOBJECT64) {
        return this == (JOBJECT64) obj;
      }
      return false;
    }

    public override int GetHashCode() {
      return Value.GetHashCode();
    }
  }
}