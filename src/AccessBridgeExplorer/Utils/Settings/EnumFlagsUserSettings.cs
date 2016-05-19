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
using System.Collections.Generic;
using System.Reflection;

namespace AccessBridgeExplorer.Utils.Settings {
  public class EnumFlagsUserSetting<T> : UserSetting<T> {
    private readonly IUserSettings _userSettings;
    private readonly string _key;
    private readonly T _defaultValue;
    private readonly Dictionary<T, UserSetting<bool>> _settings = new Dictionary<T, UserSetting<bool>>();

    public EnumFlagsUserSetting(IUserSettings userSettings, string key, T defaultValue) {
      if (!typeof(T).IsEnum) {
        throw new ArgumentException("Type must be an enum type");
      }
      _userSettings = userSettings;
      _key = key;
      _defaultValue = defaultValue;
      CreateUserSettings();
      _userSettings.Loaded += (sender, args) => {
        OnLoaded();
        OnSync(new SyncEventArgs<T>(this, Value));
      };
    }

    public override event EventHandler Loaded;
    public override event EventHandler<ChangedEventArgs<T>> Changed;
    public override event EventHandler<SyncEventArgs<T>> Sync;

    public override string Key {
      get { return _key; }
    }

    public override T Value {
      get {
        long result = 0;
        foreach (var x in _settings) {
          if (x.Value.Value) {
            result |= ToLong(x.Key);
          }
        }
        return FromLong(result);
      }
      set {
        var previous = Value;
        if (Equals(previous, value)) {
          return;
        }

        var flags = ToLong(value);
        foreach (var x in _settings) {
          bool isSet = (flags & ToLong(x.Key)) != 0;
          x.Value.Value = isSet;
        }

        OnChanged(new ChangedEventArgs<T>(this, previous, value));
        OnSync(new SyncEventArgs<T>(this, value));
      }
    }

    private void CreateUserSettings() {
      int index = 0;
      foreach (var field in typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public)) {
        CreateUserSetting(field, index);
        index++;
      }
    }

    private void CreateUserSetting(FieldInfo field, int index) {
      var name = field.Name;
      T value = (T)field.GetValue(null);
      var setting = new BoolUserSetting(_userSettings, _key + "." + name, (ToLong(_defaultValue) & ToLong(value)) != 0);
      _settings.Add(value, setting);
    }

    private long ToLong(T value) {
      return Convert.ToInt64(value);
    }

    private static T FromLong(long result) {
      return (T)Enum.ToObject(typeof(T), result);
    }

    protected virtual void OnChanged(ChangedEventArgs<T> e) {
      var handler = Changed;
      if (handler != null) handler(this, e);
    }

    protected virtual void OnLoaded() {
      var handler = Loaded;
      if (handler != null) handler(this, EventArgs.Empty);
    }

    protected virtual void OnSync(SyncEventArgs<T> e) {
      var handler = Sync;
      if (handler != null) handler(this, e);
    }
  }
}