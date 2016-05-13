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

namespace AccessBridgeExplorer.Utils {
  public abstract class UserSetting<T> {
    public abstract T Value { get; set; }

    /// <summary>
    /// Invoked when the setting is (initially) loaded from the user settings store.
    /// </summary>
    public abstract event EventHandler Loaded;
    /// <summary>
    /// Invoked when the setting value is changed.
    /// </summary>
    public abstract event EventHandler<ChangedEventArgs<T>> Changed;
    /// <summary>
    /// Invoked when the setting value is (initially) loaded or changed.
    /// </summary>
    public abstract event EventHandler Sync;
  }

  public class ChangedEventArgs<T> : EventArgs {
    private readonly UserSetting<T> _userSetting;
    private readonly T _previousValue;
    private readonly T _newValue;

    public ChangedEventArgs(UserSetting<T> userSetting, T previousValue, T newValue) {
      _userSetting = userSetting;
      _previousValue = previousValue;
      _newValue = newValue;
    }

    public UserSetting<T> Setting {
      get { return _userSetting; }
    }

    public T PreviousValue {
      get { return _previousValue; }
    }

    public T NewValue {
      get { return _newValue; }
    }
  }
}