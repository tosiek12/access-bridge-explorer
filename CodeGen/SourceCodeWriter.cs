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
using System.IO;
using System.Runtime.InteropServices;
using CodeGen.Definitions;
using CodeGen.Interop;

namespace CodeGen {
  public class SourceCodeWriter : IDisposable {
    private readonly TextWriter _textWriter;
    private string _indent = "";

    public SourceCodeWriter(TextWriter textWriter) {
      _textWriter = textWriter;
    }

    public TextWriter TextWriter {
      get { return _textWriter; }
    }

    public bool IsLegacy { get; set; }
    public bool IsNativeTypes { get; set; }

    public void Dispose() {
    }

    public void AddUsing(string value) {
      WriteLine("using {0};", value);
    }

    public void Write(string format, params object[] args) {
      _textWriter.Write(format, args);
    }

    public void WriteLine() {
      _textWriter.WriteLine();
    }

    public void WriteLine(string format, params object[] args) {
      _textWriter.Write(_indent);
      _textWriter.WriteLine(format, args);
    }

    public void StartNamespace(string value) {
      WriteLine("namespace {0} {{", value);
      IncIndent();
    }

    public void WriteIndent() {
      _textWriter.Write(_indent);
    }

    public void IncIndent() {
      _indent = new string(' ', _indent.Length + 2);
    }

    public void DecIndent() {
      _indent = new string(' ', _indent.Length - 2);
    }

    public void EndNamespace() {
      DecIndent();
      WriteLine("}}");
    }

    public void WriteType(TypeReference typeReference) {
      if (typeReference is ArrayTypeReference) {
        WriteType((ArrayTypeReference)typeReference);
      } else if (typeReference is NameTypeReference) {
        WriteType((NameTypeReference)typeReference);
      } else {
        throw new ArgumentException();
      }
    }

    public void WriteType(ArrayTypeReference typeReference) {
      WriteType(typeReference.ElementType);
      Write("[]");
    }

    public void WriteType(NameTypeReference typeReference) {
      Write("{0}", GetTypeName(typeReference));
    }

    public void WriteMashalAs(MarshalAsAttribute attribute) {
      if (attribute == null)
        return;
      if (!IsNativeTypes)
        return;

      Write("[{0}(", "MarshalAs");
      Write("{0}.{1}", typeof(UnmanagedType).Name, Enum.GetName(typeof(UnmanagedType), attribute.Value));
      if (attribute.SizeConst > 0) {
        Write(", SizeConst = {0}", attribute.SizeConst);
      }
      if (attribute.SizeParamIndex > 0) {
        Write(", SizeParamIndex = {0}", attribute.SizeParamIndex);
      }
      Write(")]");
    }

    public string GetTypeName(NameTypeReference type) {
      if (type.Name == typeof(JavaObjectHandle).Name) {
        if (IsNativeTypes) {
          if (IsLegacy)
            return "JOBJECT32";
          else
            return "JOBJECT64";
        }
      } else if (type.Name == "bool") {
        if (IsNativeTypes)
          return "BOOL";
      }

      return type.Name;
    }
  }
}