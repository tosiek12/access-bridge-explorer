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
using System.IO;
using System.Linq;
using System.Reflection;
using CodeGen.Definitions;
using CodeGen.Interop;
using CodeGen.Interop.NativeStructures;

namespace CodeGen {
  public class CodeGen {
    private string _outputFilename;

    public CodeGen(string outputFilename) {
      _outputFilename = outputFilename;
    }

    public void Generate() {
      var model = CollectModel();
      WriteFile(model);
    }

    private void WriteFile(WindowsAccessBridgeDefinition model) {
      using (var writer = File.CreateText(_outputFilename)) {
        using (var sourceWriter = new SourceCodeWriter(writer)) {
          sourceWriter.WriteLine("// Copyright 2016 Google Inc. All Rights Reserved.");
          sourceWriter.WriteLine("// ");
          sourceWriter.WriteLine("// Licensed under the Apache License, Version 2.0 (the \"License\");");
          sourceWriter.WriteLine("// you may not use this file except in compliance with the License.");
          sourceWriter.WriteLine("// You may obtain a copy of the License at");
          sourceWriter.WriteLine("// ");
          sourceWriter.WriteLine("//     http://www.apache.org/licenses/LICENSE-2.0");
          sourceWriter.WriteLine("// ");
          sourceWriter.WriteLine("// Unless required by applicable law or agreed to in writing, software");
          sourceWriter.WriteLine("// distributed under the License is distributed on an \"AS IS\" BASIS,");
          sourceWriter.WriteLine("// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.");
          sourceWriter.WriteLine("// See the License for the specific language governing permissions and");
          sourceWriter.WriteLine("// limitations under the License.");
          sourceWriter.WriteLine();

          sourceWriter.AddUsing("System");
          //sourceWriter.AddUsing("System.Diagnostics.CodeAnalysis");
          sourceWriter.AddUsing("System.Runtime.InteropServices");
          sourceWriter.AddUsing("System.Text");
          sourceWriter.AddUsing("WindowHandle = System.IntPtr");
          sourceWriter.WriteLine("// ReSharper disable InconsistentNaming");
          sourceWriter.WriteLine();

          sourceWriter.StartNamespace("AccessBridgeExplorer.WindowsAccessBridge");
          WriteApplicationLevelInterface(model, sourceWriter);
          model.Enums.ForEach(x => {
            writer.WriteLine();
            WriteEnum(model, sourceWriter, x);
          });
          sourceWriter.IsNativeTypes = true;
          model.Structs.ForEach(x => {
            writer.WriteLine();
            WriteStruct(model, sourceWriter, x);
          });
          model.Classes.ForEach(x => {
            writer.WriteLine();
            WriteClass(model, sourceWriter, x);
          });
          sourceWriter.EndNamespace();
        }
      }
    }

    private void WriteApplicationLevelInterface(WindowsAccessBridgeDefinition model, SourceCodeWriter sourceWriter) {
      sourceWriter.IsNativeTypes = false;
      sourceWriter.IsLegacy = false;
      sourceWriter.WriteLine("/// <summary>");
      sourceWriter.WriteLine("/// Platform agnostic abstraction over WindowAccessBridge DLL entry points");
      sourceWriter.WriteLine("/// </summary>");
      sourceWriter.WriteLine("public interface IAccessBridgeFunctions {{");
      sourceWriter.IncIndent();
      foreach (var function in model.Functions) {
        sourceWriter.WriteLine("{0};", function.ToString());
      }
      foreach (var eventDefinition in model.Events) {
        sourceWriter.WriteLine("bool Set{0}(Delegate handler);", eventDefinition.Name);
        //sourceWriter.WriteLine("{0};", function.ToString());
      }
      sourceWriter.DecIndent();
      sourceWriter.WriteLine("}}");
    }

    private void WriteStruct(WindowsAccessBridgeDefinition model, SourceCodeWriter sourceWriter, StrucDefinition definition) {
      sourceWriter.WriteLine("[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]");
      sourceWriter.WriteLine("public struct {0} {{", definition.Name);
      WriteFields(model, sourceWriter, definition);
      sourceWriter.WriteLine("}}");
    }

    private void WriteEnum(WindowsAccessBridgeDefinition model, SourceCodeWriter sourceWriter, EnumDefinition definition) {
      if (definition.IsFlags)
        sourceWriter.WriteLine("[Flags]");
      sourceWriter.WriteIndent();
      sourceWriter.Write("public enum {0}", definition.Name);
      if (definition.Type.Name != "int") {
        sourceWriter.Write(" : ");
        sourceWriter.WriteType(definition.Type);
      }
      sourceWriter.Write(" {{");
      sourceWriter.WriteLine();
      WriteEnumMembers(model, sourceWriter, definition);
      sourceWriter.WriteLine("}}");
    }

    private void WriteFields(WindowsAccessBridgeDefinition model, SourceCodeWriter sourceWriter, BaseTypeDefinition definition) {
      sourceWriter.IncIndent();
      definition.Fields.ForEach(f => WriteField(model, sourceWriter, f));
      sourceWriter.DecIndent();
    }

    private void WriteField(WindowsAccessBridgeDefinition model, SourceCodeWriter sourceWriter, FieldDefinition definition) {
      sourceWriter.WriteIndent();
      sourceWriter.Write("public ");
      sourceWriter.WriteType(definition.Type);
      sourceWriter.Write(" ");
      sourceWriter.Write("{0};", definition.Name);
      sourceWriter.WriteLine();
    }

    private void WriteEnumMembers(WindowsAccessBridgeDefinition model, SourceCodeWriter sourceWriter, EnumDefinition definition) {
      sourceWriter.IncIndent();
      definition.Members.ForEach(x => {
        sourceWriter.WriteIndent();
        sourceWriter.Write("{0}", x.Name);
        if (!string.IsNullOrEmpty(x.Value)) {
          sourceWriter.Write(" = {0}", x.Value);
        }
        sourceWriter.Write(",");
        sourceWriter.WriteLine();
      });
      sourceWriter.DecIndent();
    }

    private void WriteClass(WindowsAccessBridgeDefinition model, SourceCodeWriter sourceWriter, ClassDefinition classDefinition) {
      sourceWriter.WriteLine("[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]");
      sourceWriter.WriteLine("public class {0} {{", classDefinition.Name);
      WriteFields(model, sourceWriter, classDefinition);
      sourceWriter.WriteLine("}}");
    }

    private WindowsAccessBridgeDefinition CollectModel() {
      var model = new WindowsAccessBridgeDefinition();
      CollectFunctions(model);
      CollectEvents(model);
      CollectEnums(model);
      CollectStructs(model);
      CollectClasses(model);
      return model;
    }

    private void CollectFunctions(WindowsAccessBridgeDefinition model) {
      var type = typeof(IAccessBridgeFunctions);
      var functions = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
      model.Functions.AddRange(functions.Where(f => !f.IsSpecialName).Select(f => CollectFunction(f)));
    }

    private void CollectEvents(WindowsAccessBridgeDefinition model) {
      var type = typeof(IAccessBridgeFunctions);
      var events = type.GetEvents(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
      model.Events.AddRange(events.Select(x => CollectEvent(x)));
    }

    private void CollectEnums(WindowsAccessBridgeDefinition model) {
      var sampleType = typeof(AccessibleKeyCode);
      var types = typeof(IAccessBridgeFunctions).Assembly.GetExportedTypes()
        .Where(t => t.Namespace == sampleType.Namespace)
        .Where(t => t.IsValueType && t.IsEnum);
      model.Enums.AddRange(types.Select(t => CollectEnum(t)));
    }

    private void CollectStructs(WindowsAccessBridgeDefinition model) {
      var sampleStruct = typeof(AccessibleContextInfo);
      var types = typeof(IAccessBridgeFunctions).Assembly.GetExportedTypes()
        .Where(t => t.Namespace == sampleStruct.Namespace)
        .Where(t => t.IsValueType && !t.IsEnum);
      model.Structs.AddRange(types.Select(t => CollectStruct(t)));
    }

    private void CollectClasses(WindowsAccessBridgeDefinition model) {
      var sampleStruct = typeof(AccessibleContextInfo);
      var types = typeof(IAccessBridgeFunctions).Assembly.GetExportedTypes()
        .Where(t => t.Namespace == sampleStruct.Namespace)
        .Where(t => t.IsClass);
      model.Classes.AddRange(types.Select(t => CollectClass(t)));
    }

    private StrucDefinition CollectStruct(Type type) {
      return new StrucDefinition {
        Name = type.Name,
        Fields = CollectFields(type).ToList(),
      };
    }

    private EnumDefinition CollectEnum(Type type) {
      return new EnumDefinition {
        Name = type.Name,
        IsFlags = type.GetCustomAttributes(typeof(FlagsAttribute), false).Any(),
        Type = (NameTypeReference)ConvertType(type.GetEnumUnderlyingType()),
        Members = CollectEnumMembers(type).ToList(),
      };
    }

    private ClassDefinition CollectClass(Type type) {
      return new ClassDefinition {
        Name = type.Name,
        Fields = CollectFields(type).ToList(),
      };
    }

    private IEnumerable<FieldDefinition> CollectFields(Type type) {
      return type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
        .Select(x => new FieldDefinition {
          Name = x.Name,
          Type = ConvertType(x.FieldType)
        });
    }

    private IEnumerable<EnumMemberDefinition> CollectEnumMembers(Type type) {
      return type.GetFields(BindingFlags.Public | BindingFlags.Static)
        .Select(x => new EnumMemberDefinition {
          Name = x.Name,
          Value = x.GetRawConstantValue().ToString()
          //Type = ConvertType(x.FieldType)
        });
    }

    private FunctionDefinition CollectFunction(MethodInfo methodInfo) {
      return new FunctionDefinition {
        Name = methodInfo.Name,
        ReturnType = ConvertType(methodInfo.ReturnType),
        Parameters = CollectParameters(methodInfo.GetParameters()).ToList()
      };
    }

    private EventDefinition CollectEvent(EventInfo eventInfo) {
      return new EventDefinition {
        Name = eventInfo.Name,
        Type = ConvertType(eventInfo.EventHandlerType),
      };
    }

    private IEnumerable<ParameterDefinition> CollectParameters(ParameterInfo[] getParameters) {
      foreach (var p in getParameters) {
        yield return new ParameterDefinition {
          Name = p.Name,
          Type = ConvertType(p.ParameterType),
          IsOutAttribute = !p.ParameterType.IsByRef && p.IsOut,
          IsOut = p.ParameterType.IsByRef && p.IsOut,
          IsRef = p.ParameterType.IsByRef,

        };
      }
    }

    private TypeReference ConvertType(System.Type type) {
      if (type.IsByRef)
        type = type.GetElementType();

      var name = type.Name;
      if (type == typeof(void))
        name = "void";
      else if (type == typeof(bool))
        name = "bool";
      else if (type == typeof(string))
        name = "string";
      else if (type == typeof(float))
        name = "float";
      else if (type == typeof(double))
        name = "double";
      else if (type == typeof(int))
        name = "int";
      else if (type == typeof(short))
        name = "short";
      else if (type == typeof(long))
        name = "long";
      else if (type == typeof(uint))
        name = "uint";
      else if (type == typeof(ushort))
        name = "ushort";
      else if (type == typeof(ulong))
        name = "ulong";

      if (type.IsArray) {
        return new ArrayTypeReference {
          ElementType = ConvertType(type.GetElementType())
        };
      }

      return new NameTypeReference { Name = name };
    }
  }
}