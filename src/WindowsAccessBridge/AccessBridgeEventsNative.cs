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
using System.Linq;
using System.Reflection;

namespace AccessBridgeExplorer.WindowsAccessBridge {
  public partial class AccessBridgeEventsNative {
    private readonly AccessBridgeLibraryFunctions _libraryFunctions;
    private readonly List<Delegate> _handlers = new List<Delegate>();

    public AccessBridgeEventsNative(AccessBridgeLibraryFunctions libraryFunctions) {
      _libraryFunctions = libraryFunctions;
    }

    public void SetHandlers() {
      // Call "_bridge.Functions.SetXxxFP(OnXxx)" for all events.
      var publicMembers = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;
      var privateMembers = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic;
      foreach (var evt in GetNativeEvents()) {
        var name = evt.Name;

        // Create delegate to the "OnXxx" instance method
        var methodName = "On" + evt.Name;
        var method = GetType().GetMethod(methodName, privateMembers);
        var handler = Delegate.CreateDelegate(evt.EventHandlerType, this, method);
        _handlers.Add(handler); // Hold delegate instance in list so it does not get GC'ed

        // Find the native access bridge event setter method to call
        var setName = "Set" + name;
        var propertyMethod = _libraryFunctions.GetType().GetProperty(setName, publicMembers);
        if (propertyMethod == null) {
          throw new ApplicationException(string.Format("Type \"{0}\" should contain a method named \"{1}\"",
            _libraryFunctions.GetType().Name, setName));
        }
        var getter = propertyMethod.GetGetMethod();
        var action = getter.Invoke(_libraryFunctions, new object[] {});

        // Call it with the handler instance
        ((Delegate) action).DynamicInvoke(new object[] {handler});
      }
    }

    private IEnumerable<EventInfo> GetNativeEvents() {
      var publicMembers = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;
      return GetType().GetEvents(publicMembers).Where(x => !x.Name.Contains("Mouse"));
    }

    public void ReleaseHandlers() {
      // Call "_bridge.Functions.SetXxxFP(null)" for all events.
      var publicMembers = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public;
      foreach (var evt in GetNativeEvents()) {
        var name = evt.Name;

        // Find the native access bridge event setter method to call
        var setName = "Set" + name;
        var getter = _libraryFunctions.GetType().GetProperty(setName, publicMembers).GetGetMethod();
        var action = getter.Invoke(_libraryFunctions, new object[] {});

        // Call it with "null"
        ((Delegate) action).DynamicInvoke(new object[] {null});
      }

      // We don't need to hold ref. to delegate instances anymore
      _handlers.Clear();
    }
  }
}