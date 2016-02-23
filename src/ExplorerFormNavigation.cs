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
    /// <summary>The current action, or "null" if the navigation stack is empty.</summary>
    private Action _currentAction;
    /// <summary>Stack of actions before <see cref="_currentAction"/></summary>
    private readonly Stack<Action> _backwardActions = new Stack<Action>();
    /// <summary>Stack of actions after <see cref="_currentAction"/></summary>
    private readonly Stack<Action> _forwardActions = new Stack<Action>();

    private int _navigationNesting;

    public bool ForwardAvailable {
      get { return _forwardActions.Count > 0; }
    }

    public bool BackwardAvailable {
      get { return _backwardActions.Count > 0; }
    }

    public void Clear() {
      _backwardActions.Clear();
      _forwardActions.Clear();
      _currentAction = null;
    }

    public void AddNavigationAction(Action action) {
      if (_navigationNesting >= 1)
        return;

      _forwardActions.Clear();
      if (_currentAction != null) {
        _backwardActions.Push(_currentAction);
      }
      _currentAction = action;
    }

    public void NavigateForward() {
      if (_navigationNesting >= 1)
        return;

      if (!ForwardAvailable)
        return;

      if (_currentAction != null) {
        _backwardActions.Push(_currentAction);
      }
      _currentAction = _forwardActions.Pop();
      DoNavigateAction(_currentAction);
    }

    public void NavigateBackward() {
      if (_navigationNesting >= 1)
        return;

      if (!BackwardAvailable)
        return;

      if (_currentAction != null) {
        _forwardActions.Push(_currentAction);
      }
      _currentAction = _backwardActions.Pop();
      DoNavigateAction(_currentAction);
    }

    private void DoNavigateAction(Action action) {
      _navigationNesting++;
      try {
        action();
      } finally {
        _navigationNesting--;
      }
    }
  }
}
