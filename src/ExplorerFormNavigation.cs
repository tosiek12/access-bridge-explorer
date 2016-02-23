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

namespace AccessBridgeExplorer {
  public class ExplorerFormNavigation : IExplorerFormNavigation {
    private readonly Stack<Action> _backwardActions = new Stack<Action>();
    private readonly Stack<Action> _forwardActions = new Stack<Action>();

    public bool ForwardAvailable {
      get { return _forwardActions.Count > 0; }
    }

    public bool BackwardAvailable {
      get { return _backwardActions.Count > 0; }
    }

    public void Clear() {
      _backwardActions.Clear();
      _forwardActions.Clear();
    }

    public void AddNavigationAction(Action action) {
      _forwardActions.Clear();
      _backwardActions.Push(action);
    }

    public void NavigateForward() {
      if (!ForwardAvailable)
        return;

      var action = _forwardActions.Pop();
      _backwardActions.Push(action);
      action();
    }

    public void NavigateBackward() {
      if (!BackwardAvailable)
        return;

      var action = _backwardActions.Pop();
      _forwardActions.Push(action);
      action();
    }
  }
}
