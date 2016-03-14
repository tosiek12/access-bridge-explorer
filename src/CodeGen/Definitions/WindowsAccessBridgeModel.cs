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
using System.Linq;

namespace CodeGen.Definitions {
  public class WindowsAccessBridgeModel {
    private readonly List<FunctionDefinition> _functions = new List<FunctionDefinition>();
    private readonly List<EventDefinition> _events = new List<EventDefinition>();
    private readonly List<EnumDefinition> _enums = new List<EnumDefinition>();
    private readonly List<StrucDefinition> _structs = new List<StrucDefinition>();
    private readonly List<ClassDefinition> _classes = new List<ClassDefinition>();

    public List<FunctionDefinition> Functions {
      get { return _functions; }
    }

    public List<EventDefinition> Events {
      get { return _events; }
    }

    public List<EnumDefinition> Enums {
      get { return _enums; }
    }

    public List<StrucDefinition> Structs {
      get { return _structs; }
    }

    public List<ClassDefinition> Classes {
      get { return _classes; }
    }

    public bool IsStructName(string name) {
      return Structs.Any(c => c.Name == name);
    }

    public bool IsClassName(string name) {
      return Classes.Any(c => c.Name == name);
    }

    public bool IsStruct(TypeReference type) {
      var name = type as NameTypeReference;
      if (name == null)
        return false;
      return IsStructName(name.Name);
    }

    public bool IsClass(TypeReference type) {
      var name = type as NameTypeReference;
      if (name == null)
        return false;
      return IsClassName(name.Name);
    }
  }
}