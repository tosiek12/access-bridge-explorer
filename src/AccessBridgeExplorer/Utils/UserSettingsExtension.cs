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
  public static class UserSettingsExtension {
    public static UserSetting<string> CreateUserSetting(this IUserSettings userSettings, string key, string defaultValue) {
      return new UserSettingImpl<string>(
        userSettings,
        userSettings.GetValue,
        userSettings.SetValue,
        key, defaultValue);
    }

    public static UserSetting<int> CreateUserSetting(this IUserSettings userSettings, string key, int defaultValue) {
      return new UserSettingImpl<int>(
        userSettings,
        userSettings.GetIntValue,
        userSettings.SetIntValue,
        key, defaultValue);
    }

    public static UserSetting<bool> CreateUserSetting(this IUserSettings userSettings, string key, bool defaultValue) {
      return new UserSettingImpl<bool>(
        userSettings,
        userSettings.GetBoolValue,
        userSettings.SetBoolValue,
        key, defaultValue);
    }

    public static UserSetting<T> CreateEnumUserSetting<T>(this IUserSettings userSettings, string key, T defaultValue) {
      if (!typeof (T).IsEnum) {
        throw new ArgumentException("Type must be an enum type");
      }

      return new UserSettingImpl<T>(
        userSettings,
        (k, v) => (T)(object)userSettings.GetIntValue(k, (int)(object)v),
        (k, v) => userSettings.SetIntValue(k, (int)(object)v),
        key, defaultValue);
    }

    private class UserSettingImpl<T> : UserSetting<T> {
      private readonly IUserSettings _userSettings;
      private readonly Func<string, T, T> _getter;
      private readonly Action<string, T> _setter;
      private readonly string _key;
      private readonly T _defaultValue;

      public UserSettingImpl(IUserSettings userSettings, Func<string, T, T> getter, Action<string, T> setter, string key, T defaultValue) {
        _userSettings = userSettings;
        _getter = getter;
        _setter = setter;
        _key = key;
        _defaultValue = defaultValue;

        _userSettings.Loaded += (sender, args) => {
          OnLoaded();
          OnSync(new SyncEventArgs<T>(this, Value));
        };
      }

      public override event EventHandler<SyncEventArgs<T>> Sync;
      public override event EventHandler Loaded;
      public override event EventHandler<ChangedEventArgs<T>> Changed;

      public override T Value {
        get { return _getter(_key, _defaultValue); }
        set {
          var oldValue = Value;
          if (Equals(value, oldValue)) {
            return;
          }

          if (Equals(value, _defaultValue)) {
            _userSettings.Remove(_key);
          } else {
            _setter(_key, value);
          }
          OnChanged(new ChangedEventArgs<T>(this, oldValue, value));
          OnSync(new SyncEventArgs<T>(this, value));
        }
      }

      protected virtual void OnChanged(ChangedEventArgs<T> e) {
        var handler = Changed;
        if (handler != null) handler(this, e);
      }

      protected virtual void OnSync(SyncEventArgs<T> e) {
        var handler = Sync;
        if (handler != null) handler(this, e);
      }

      protected virtual void OnLoaded() {
        var handler = Loaded;
        if (handler != null) handler(this, EventArgs.Empty);
      }
    }
  }
}