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
using System.Linq;
using CodeGen.Interop;

namespace CodeGen.Definitions {
  public class WindowsAccessBridgeModel {
    private readonly List<FunctionDefinition> _functions = new List<FunctionDefinition>();
    private readonly List<EventDefinition> _events = new List<EventDefinition>();
    private readonly List<EnumDefinition> _enums = new List<EnumDefinition>();
    private readonly List<StructDefinition> _structs = new List<StructDefinition>();
    private readonly List<ClassDefinition> _classes = new List<ClassDefinition>();
    private XmlDocCommentCollector _xmlDoc = new XmlDocCommentCollector();

    public List<FunctionDefinition> Functions {
      get { return _functions; }
    }

    public List<EventDefinition> Events {
      get { return _events; }
    }

    public List<EnumDefinition> Enums {
      get { return _enums; }
    }

    public List<StructDefinition> Structs {
      get { return _structs; }
    }

    public List<ClassDefinition> Classes {
      get { return _classes; }
    }

    public XmlDocCommentCollector XmlDoc {
      get { return _xmlDoc; }
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

    public bool StructNeedsWrapper(StructDefinition definition) {
      return BaseTypeNeedsWrapper(definition);
    }

    public bool StructNeedsWrapper(string name) {
      var definition = Structs.FirstOrDefault(x => x.Name == name);
      return definition == null ? false : StructNeedsWrapper(definition);
    }

    public bool ClassNeedsWrapper(ClassDefinition definition) {
      return BaseTypeNeedsWrapper(definition);
    }

    public bool ClassNeedsWrapper(string name) {
      var definition = Classes.FirstOrDefault(x => x.Name == name);
      return definition == null ? false : ClassNeedsWrapper(definition);
    }

    public bool TypeNameNeedsWrapper(NameTypeReference reference) {
      var name = reference.Name;
      if (name == typeof (JavaObjectHandle).Name)
        return true;

      if (Classes.Any(c => c.Name == reference.Name && ClassNeedsWrapper(c)))
        return true;

      if (Structs.Any(c => c.Name == reference.Name && StructNeedsWrapper(c)))
        return true;

      return false;
    }

    public bool BaseTypeNeedsWrapper(BaseTypeDefinition definiton) {
      return definiton.Fields.Any(field => TypeReferenceNeedsWrapper(field.Type));
    }

    public bool TypeReferenceNeedsWrapper(TypeReference reference) {
      if (reference is NameTypeReference) {
        return TypeNameNeedsWrapper(reference as NameTypeReference);
      } else if (reference is ArrayTypeReference) {
        return TypeReferenceNeedsWrapper((reference as ArrayTypeReference).ElementType);
      } else {
        throw new InvalidOperationException("Unknown type reference type");
      }
    }
  }
}