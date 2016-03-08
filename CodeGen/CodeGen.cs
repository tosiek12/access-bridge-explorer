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
using System.Runtime.InteropServices;
using CodeGen.Definitions;
using CodeGen.Interop.NativeStructures;
using WindowsAccessBridgeDefinition = CodeGen.Interop.WindowsAccessBridgeDefinition;

namespace CodeGen {
  public class CodeGen {
    private readonly string _outputFilename;

    public CodeGen(string outputFilename) {
      _outputFilename = outputFilename;
    }

    public void Generate() {
      var model = CollectModel();
      WriteFile(model);
    }

    private void WriteFile(LibraryDefinition model) {
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

          sourceWriter.WriteLine("// ReSharper disable InconsistentNaming");
          sourceWriter.AddUsing("System");
          //sourceWriter.AddUsing("System.Diagnostics.CodeAnalysis");
          sourceWriter.AddUsing("System.Runtime.InteropServices");
          sourceWriter.AddUsing("System.Text");
          sourceWriter.AddUsing("WindowHandle = System.IntPtr");
          sourceWriter.AddUsing("BOOL = System.Int32");
          sourceWriter.WriteLine();

          sourceWriter.StartNamespace("AccessBridgeExplorer.WindowsAccessBridge");
          WriteApplicationLevelInterface(model, sourceWriter);
          WriteApplicationLevelEventHandlerTypes(model, sourceWriter);
          WriteApplicationLevelInterfaceImplementation(model, sourceWriter);
          WriteLibrayrFunctionsClass(model, sourceWriter);
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

    private void WriteApplicationLevelInterface(LibraryDefinition model, SourceCodeWriter sourceWriter) {
      sourceWriter.IsNativeTypes = false;
      sourceWriter.IsLegacy = false;
      sourceWriter.WriteLine("/// <summary>");
      sourceWriter.WriteLine("/// Platform agnostic abstraction over WindowAccessBridge DLL entry points");
      sourceWriter.WriteLine("/// </summary>");
      sourceWriter.WriteLine("public interface IAccessBridgeFunctions {{");
      sourceWriter.IncIndent();
      foreach (var function in model.Functions) {
        WriteFunction(sourceWriter, function);
      }
      //sourceWriter.WriteLine();
      //foreach (var eventDefinition in model.Events) {
      //  WriteEvent(sourceWriter, eventDefinition);
      //}
      sourceWriter.DecIndent();
      sourceWriter.WriteLine("}}");
      sourceWriter.WriteLine();

      sourceWriter.WriteLine("/// <summary>");
      sourceWriter.WriteLine("/// Platform agnostic abstraction over WindowAccessBridge DLL entry points");
      sourceWriter.WriteLine("/// </summary>");
      sourceWriter.WriteLine("public interface IAccessBridgeEvents {{");
      sourceWriter.IncIndent();
      foreach (var eventDefinition in model.Events) {
        WriteEvent(sourceWriter, eventDefinition);
      }
      sourceWriter.DecIndent();
      sourceWriter.WriteLine("}}");
      sourceWriter.WriteLine();
    }

    private void WriteApplicationLevelEventHandlerTypes(LibraryDefinition model, SourceCodeWriter sourceWriter) {
      sourceWriter.IsNativeTypes = false;
      sourceWriter.IsLegacy = false;
      foreach (var eventDefinition in model.Events) {
        WriteEventHandlerType(sourceWriter, eventDefinition);
      }
      sourceWriter.WriteLine();
    }

    private void WriteApplicationLevelInterfaceImplementation(LibraryDefinition model, SourceCodeWriter sourceWriter) {
      sourceWriter.IsNativeTypes = false;
      sourceWriter.IsLegacy = false;
      sourceWriter.WriteLine("/// <summary>");
      sourceWriter.WriteLine("/// Platform agnostic abstraction over WindowAccessBridge DLL entry points");
      sourceWriter.WriteLine("/// </summary>");
      sourceWriter.WriteLine("public interface IAccessBridgeFunctionsTODO /*: IAccessBridgeFunctions*/ {{");
      sourceWriter.IncIndent();
      foreach (var function in model.Functions) {
        WriteFunction(sourceWriter, function);
      }
      foreach (var eventDefinition in model.Events) {
        WriteEvent(sourceWriter, eventDefinition);
      }
      sourceWriter.DecIndent();
      sourceWriter.WriteLine("}}");
      sourceWriter.WriteLine();
    }

    private void WriteLibrayrFunctionsClass(LibraryDefinition model, SourceCodeWriter sourceWriter) {
      sourceWriter.IsNativeTypes = true;
      sourceWriter.IsLegacy = false;
      sourceWriter.WriteLine("/// <summary>");
      sourceWriter.WriteLine("/// Container of WindowAccessBridge DLL entry points");
      sourceWriter.WriteLine("/// </summary>");
      sourceWriter.WriteLine("public class AccessBridgeLibraryFunctions {{");
      sourceWriter.IncIndent();

      sourceWriter.WriteLine("#region Function delegate types");
      foreach (var function in model.Functions) {
        WriteLibrayrFunctionsDelegate(sourceWriter, function);
      }
      sourceWriter.WriteLine("#endregion");
      sourceWriter.WriteLine();

      sourceWriter.WriteLine("#region Functions");
      foreach (var function in model.Functions) {
        WriteLibraryFunctionProperty(sourceWriter, function);
      }
      sourceWriter.WriteLine("#endregion");
      sourceWriter.WriteLine();

      sourceWriter.WriteLine("#region Event delegate types");
      foreach (var definition in model.Events) {
        sourceWriter.WriteLine("[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]");
        WriteDelegateType(sourceWriter, definition.DelegateFunction);
      }
      sourceWriter.WriteLine();
      foreach (var eventDefinition in model.Events) {
        sourceWriter.WriteLine("[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]");
        WriteLibraryEventDelegateType(sourceWriter, eventDefinition);
      }
      sourceWriter.WriteLine("#endregion");
      sourceWriter.WriteLine();

      sourceWriter.WriteLine("#region Event functions");
      foreach (var eventDefinition in model.Events) {
        WriteLibraryEventProperty(sourceWriter, eventDefinition);
      }
      sourceWriter.WriteLine("#endregion");
      sourceWriter.DecIndent();
      sourceWriter.WriteLine("}}");
    }

    private void WriteFunction(SourceCodeWriter sourceWriter, FunctionDefinition definition) {
      sourceWriter.WriteIndent();
      WriteFunctionSignature(sourceWriter, definition);
      sourceWriter.Write(";");
      sourceWriter.WriteLine();
    }

    private void WriteDelegateType(SourceCodeWriter sourceWriter, FunctionDefinition definition) {
      WriteMarshalAsLine(sourceWriter, definition.MarshalAs);
      sourceWriter.WriteIndent();
      sourceWriter.Write("public delegate ");
      WriteFunctionSignature(sourceWriter, definition);
      sourceWriter.Write(";");
      sourceWriter.WriteLine();
    }

    private void WriteFunctionSignature(SourceCodeWriter sourceWriter, FunctionDefinition definition) {
      sourceWriter.WriteType(definition.ReturnType);
      sourceWriter.Write(" ");
      sourceWriter.Write(definition.Name);
      sourceWriter.Write("(");
      bool first = true;
      foreach (var p in definition.Parameters) {
        if (first)
          first = false;
        else {
          sourceWriter.Write(", ");
        }
        WriteParameter(sourceWriter, p);
      }
      sourceWriter.Write(")");
    }

    private void WriteLibrayrFunctionsDelegate(SourceCodeWriter sourceWriter, FunctionDefinition function) {
      sourceWriter.WriteLine("[UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]");
      sourceWriter.WriteIndent();
      sourceWriter.Write("public delegate ");
      sourceWriter.WriteType(function.ReturnType);
      sourceWriter.Write(" ");
      sourceWriter.Write("{0}FP", function.Name);
      sourceWriter.Write("(");
      bool first = true;
      foreach (var p in function.Parameters) {
        if (first)
          first = false;
        else {
          sourceWriter.Write(", ");
        }
        WriteParameter(sourceWriter, p);
      }
      sourceWriter.Write(");");
      sourceWriter.WriteLine();
    }

    private void WriteLibraryFunctionProperty(SourceCodeWriter sourceWriter, FunctionDefinition function) {
      sourceWriter.WriteLine("public {0}FP {0} {{ get; set; }}", function.Name);
    }

    private void WriteLibraryEventDelegateType(SourceCodeWriter sourceWriter, EventDefinition definition) {
      sourceWriter.WriteIndent();
      sourceWriter.Write("public delegate BOOL {0}FP({0}EventHandler handler)", definition.Name);
      sourceWriter.Write(";");
      sourceWriter.WriteLine();
    }

    private void WriteLibraryEventProperty(SourceCodeWriter sourceWriter, EventDefinition definition) {
      sourceWriter.WriteLine("public {0}FP Set{0} {{ get; set; }}", definition.Name);
    }

    private void WriteParameter(SourceCodeWriter sourceWriter, ParameterDefinition parameterDefinition) {
      if (parameterDefinition.IsOutAttribute) {
        sourceWriter.Write("[Out]");
      }

      if (parameterDefinition.IsOut) {
        sourceWriter.Write("out ");
      } else if (parameterDefinition.IsRef) {
        sourceWriter.Write("ref ");
      }
      sourceWriter.WriteMashalAs(parameterDefinition.MarshalAs);
      sourceWriter.WriteType(parameterDefinition.Type);
      sourceWriter.Write(" ");
      sourceWriter.Write(parameterDefinition.Name);
    }

    private void WriteEvent(SourceCodeWriter sourceWriter, EventDefinition eventDefinition) {
      sourceWriter.WriteLine("event {0}EventHandler {0};", eventDefinition.Name);
    }

    private void WriteEventHandlerType(SourceCodeWriter sourceWriter, EventDefinition eventDefinition) {
      WriteDelegateType(sourceWriter, eventDefinition.DelegateFunction);
    }

    private void WriteStruct(LibraryDefinition model, SourceCodeWriter sourceWriter, StrucDefinition definition) {
      sourceWriter.WriteLine("[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]");
      sourceWriter.WriteLine("public struct {0} {{", definition.Name);
      WriteFields(model, sourceWriter, definition);
      sourceWriter.WriteLine("}}");
    }

    private void WriteEnum(LibraryDefinition model, SourceCodeWriter sourceWriter, EnumDefinition definition) {
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

    private void WriteFields(LibraryDefinition model, SourceCodeWriter sourceWriter, BaseTypeDefinition definition) {
      sourceWriter.IncIndent();
      definition.Fields.ForEach(f => WriteField(model, sourceWriter, f));
      sourceWriter.DecIndent();
    }

    private void WriteField(LibraryDefinition model, SourceCodeWriter sourceWriter, FieldDefinition definition) {
      WriteMarshalAsLine(sourceWriter, definition.MarshalAs);
      sourceWriter.WriteIndent();
      sourceWriter.Write("public ");
      sourceWriter.WriteType(definition.Type);
      sourceWriter.Write(" ");
      sourceWriter.Write("{0};", definition.Name);
      sourceWriter.WriteLine();
    }

    private static void WriteMarshalAsLine(SourceCodeWriter sourceWriter, MarshalAsAttribute marshalAs) {
      if (marshalAs != null) {
        sourceWriter.WriteIndent();
        sourceWriter.WriteMashalAs(marshalAs);
        sourceWriter.WriteLine();
      }
    }

    private void WriteEnumMembers(LibraryDefinition model, SourceCodeWriter sourceWriter, EnumDefinition definition) {
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

    private void WriteClass(LibraryDefinition model, SourceCodeWriter sourceWriter, ClassDefinition classDefinition) {
      sourceWriter.WriteLine("[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]");
      sourceWriter.WriteLine("public class {0} {{", classDefinition.Name);
      WriteFields(model, sourceWriter, classDefinition);
      sourceWriter.WriteLine("}}");
    }

    private LibraryDefinition CollectModel() {
      var model = new Definitions.LibraryDefinition();
      CollectFunctions(model);
      CollectEvents(model);
      CollectEnums(model);
      CollectStructs(model);
      CollectClasses(model);
      return model;
    }

    private void CollectFunctions(LibraryDefinition model) {
      var type = typeof(WindowsAccessBridgeDefinition);
      var functions = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
      model.Functions.AddRange(functions.Where(f => !f.IsSpecialName).Select(f => CollectFunction(f)));
    }

    private void CollectEvents(LibraryDefinition model) {
      var type = typeof(WindowsAccessBridgeDefinition);
      var events = type.GetEvents(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
      model.Events.AddRange(events.Select(x => CollectEvent(x)));
    }

    private void CollectEnums(LibraryDefinition model) {
      var sampleType = typeof(AccessibleKeyCode);
      var types = typeof(WindowsAccessBridgeDefinition).Assembly.GetExportedTypes()
        .Where(t => t.Namespace == sampleType.Namespace)
        .Where(t => t.IsValueType && t.IsEnum);
      model.Enums.AddRange(types.Select(t => CollectEnum(t)));
    }

    private void CollectStructs(LibraryDefinition model) {
      var sampleStruct = typeof(AccessibleContextInfo);
      var types = typeof(WindowsAccessBridgeDefinition).Assembly.GetExportedTypes()
        .Where(t => t.Namespace == sampleStruct.Namespace)
        .Where(t => t.IsValueType && !t.IsEnum);
      model.Structs.AddRange(types.Select(t => CollectStruct(t)));
    }

    private void CollectClasses(LibraryDefinition model) {
      var sampleStruct = typeof(AccessibleContextInfo);
      var types = typeof(WindowsAccessBridgeDefinition).Assembly.GetExportedTypes()
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
          Type = ConvertType(x.FieldType),
          MarshalAs = ConvertMashalAs(x)
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

    private FunctionDefinition CollectDelegateType(Type delegateType) {
      var methodInfo = delegateType.GetMethod("Invoke");
      return new FunctionDefinition {
        Name = delegateType.Name,
        ReturnType = ConvertType(methodInfo.ReturnType),
        Parameters = CollectParameters(methodInfo.GetParameters()).ToList(),
        MarshalAs = ConvertMashalAs(delegateType)
      };
    }

    private EventDefinition CollectEvent(EventInfo eventInfo) {
      return new EventDefinition {
        Name = eventInfo.Name,
        Type = ConvertType(eventInfo.EventHandlerType),
        DelegateFunction = CollectDelegateType(eventInfo.EventHandlerType)
      };
    }

    private IEnumerable<ParameterDefinition> CollectParameters(ParameterInfo[] parameters) {
      foreach (var p in parameters) {
        yield return new ParameterDefinition {
          Name = p.Name,
          Type = ConvertType(p.ParameterType),
          MarshalAs = ConvertMashalAs(p),
          IsOutAttribute = !p.ParameterType.IsByRef && p.IsOut,
          IsOut = p.ParameterType.IsByRef && p.IsOut,
          IsRef = p.ParameterType.IsByRef,
        };
      }
    }

    private MarshalAsAttribute ConvertMashalAs(ICustomAttributeProvider provider) {
      return (MarshalAsAttribute)provider.GetCustomAttributes(typeof(MarshalAsAttribute), false).FirstOrDefault();
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