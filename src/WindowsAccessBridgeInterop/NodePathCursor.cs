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

using System.Collections.Generic;

namespace WindowsAccessBridgeInterop {
  public class NodePathCursor {
    private readonly List<AccessibleNode> _nodes;
    private readonly int _start;
    private readonly int _end;
    private int _index;

    public NodePathCursor(List<AccessibleNode> nodes, int start, int end) {
      _nodes = nodes;
      _start = start;
      _end = end;
      _index = start;
    }

    public AccessibleNode Node {
      get {
        if (_start <= _index && _index < _end) {
          return _nodes[_index];
        }
        return null;
      }
    }

    public bool IsValid {
      get { return Node != null; }
    }

    public NodePathCursor Clone() {
      return new NodePathCursor(_nodes, _index, _end);
    }

    public NodePathCursor MoveNext() {
      if (_index < _end) {
        _index++;
      }
      return this;
    }

    public NodePathCursor MovePrevious() {
      if (_index >= _start) {
        _index--;
      }
      return this;
    }
  }
}