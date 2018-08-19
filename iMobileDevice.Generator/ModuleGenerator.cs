// <copyright file="ModuleGenerator.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Diagnostics;
#if !NETSTANDARD1_5
    using System.Security.Permissions;
    using System.Runtime.ConstrainedExecution;
#endif
    using CodeDom;
    using System.Runtime.Serialization;
    using System.Runtime.ExceptionServices;
    using Core.Clang;
    using Core.Clang.Diagnostics;
    using System.Text;
    using global::Nustache.Core;

    public class ModuleGenerator
    {
        public string Name
        {
            get
            {
                return NameConversions.ToClrName(Path.GetFileNameWithoutExtension(this.InputFile), NameConversion.Type);
            }
        }

        public string InputFile
        {
            get;
            set;
        }

        public Collection<string> IncludeDirectories
        { get; } = new Collection<string>();

        public Collection<IGeneratedType> Types
        { get; } = new Collection<IGeneratedType>();

        public Dictionary<string, string> NameMapping
        { get; } = new Dictionary<string, string>();

        public CodeTypeDeclaration StringArrayMarshalerType
        { get; set; }

        public void AddType(string nativeName, IGeneratedType type)
        {
            if (string.IsNullOrEmpty(nativeName))
            {
                throw new ArgumentOutOfRangeException(nameof(nativeName));
            }

            if (type == null)
            {
                throw new ArgumentOutOfRangeException(nativeName);
            }

            if (this.Types.Contains(type))
            {
                throw new InvalidOperationException();
            }

            if (this.NameMapping.ContainsValue(type.Name))
            {
                throw new InvalidOperationException();
            }

            if (this.NameMapping.ContainsKey(nativeName))
            {
                throw new InvalidOperationException();
            }

            this.Types.Add(type);
            this.NameMapping.Add(nativeName, type.Name);
        }

        public void Generate(string targetDirectory, string libraryName = "imobiledevice")
        {
            this.GenerateTypes(libraryName);
            this.WriteTypes(targetDirectory);
        }

        public void GenerateTypes(string libraryName = "imobiledevice")
        {
            string[] arguments =
            {
                // Use the C++ backend
                "-x",
                "c++",

                // Parse the doxygen comments
                "-Wdocumentation",

                // Target 32-bit OS
                "-m32"
            };

            arguments = arguments.Concat(this.IncludeDirectories.Select(x => "-I" + x)).ToArray();

            FunctionVisitor functionVisitor;

            using (var createIndex = new Index(false, true))
            using (var translationUnit = createIndex.ParseTranslationUnit(this.InputFile, arguments))
            {
                StringWriter errorWriter = new StringWriter();
                var set = DiagnosticSet.FromTranslationUnit(translationUnit);
                var numDiagnostics = set.GetNumDiagnostics();

                bool hasError = false;
                bool hasWarning = false;

                for (uint i = 0; i < numDiagnostics; ++i)
                {
                    Diagnostic diagnostic = set.GetDiagnostic(i);
                    var severity = diagnostic.GetSeverity();

                    switch (severity)
                    {
                        case DiagnosticSeverity.Error:
                        case DiagnosticSeverity.Fatal:
                            hasError = true;
                            break;

                        case DiagnosticSeverity.Warning:
                            hasWarning = true;
                            break;
                    }

                    var location = diagnostic.GetLocation();
                    var fileName = location.SourceFile;
                    var line = location.Line;

                    var message = diagnostic.GetSpelling();
                    errorWriter.WriteLine($"{severity}: {fileName}:{line} {message}");
                }

                if (hasError)
                {
                    throw new Exception(errorWriter.ToString());
                }

                if (hasWarning)
                {
                    // Dump the warnings to the console output.
                    Console.WriteLine(errorWriter.ToString());
                }

                // Generate the marhaler types for string arrays (char **)
                var arrayMarshalerGenerator = new ArrayMarshalerGenerator(this);
                arrayMarshalerGenerator.Generate();

                // Creates enums
                var enumVisitor = new EnumVisitor(this);
                var realEnumVisitor = new DelegatingCursorVisitor(enumVisitor.Visit);
                var cursor = translationUnit.GetCursor();
                realEnumVisitor.VisitChildren(cursor);

                // Creates structs
                var structVisitor = new StructVisitor(this);
                var realStructVisitor = new DelegatingCursorVisitor(structVisitor.Visit);
                realStructVisitor.VisitChildren(translationUnit.GetCursor());

                // Creates safe handles & delegates
                var typeDefVisitor = new TypeDefVisitor(this);
                var realTypeDefVisitor = new DelegatingCursorVisitor(typeDefVisitor.Visit);
                realTypeDefVisitor.VisitChildren(translationUnit.GetCursor());

                // Creates functions in a NativeMethods class
                functionVisitor = new FunctionVisitor(this, libraryName);
                var realFunctionVisitor = new DelegatingCursorVisitor(functionVisitor.Visit);
                realFunctionVisitor.VisitChildren(translationUnit.GetCursor());

                createIndex.Dispose();
            }

            // Update the SafeHandle to call the _free method
            var handles = this.Types.Where(t => t.Name.EndsWith("Handle"));

            foreach (var handle in handles)
            {
                var freeMethod = functionVisitor.NativeMethods.Members
                    .OfType<CodeMemberMethod>()
                    .Where(m =>
                        (m.Name.EndsWith("_free") || m.Name.EndsWith("_disconnect"))
                        && m.Parameters.Count == 1
                        && m.Parameters[0].Type.BaseType == handle.Name)
                    .SingleOrDefault();

                if (freeMethod == null)
                {
                    continue;
                }

                // Directly pass the IntPtr, becuase the handle itself will already be in the 'closed' state
                // when this method is called.
                freeMethod.Parameters[0].Type = new CodeTypeReference(typeof(IntPtr));
                freeMethod.Parameters[0].Direction = FieldDirection.In;

                var releaseMethod = ((CodeDomGeneratedType)handle).Declaration.Members.OfType<CodeMemberMethod>().Single(m => m.Name == "ReleaseHandle");

                // Sample statement:
                //   System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
                //   this.Api.Plist.plist_free(this.handle);
                //   return true;
                releaseMethod.Statements.Clear();

                // Trace the release call:
                // Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
                releaseMethod.Statements.Add(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                            new CodeTypeReferenceExpression(typeof(Debug)),
                            "WriteLine"),
                        new CodePrimitiveExpression("Releasing {0} {1} using {2}. This object was created at {3}"),
                        new CodePropertyReferenceExpression(
                            new CodeMethodInvokeExpression(
                                new CodeThisReferenceExpression(),
                                nameof(GetType)),
                                "Name"),
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            "handle"),
                        new CodePropertyReferenceExpression(
                            new CodeThisReferenceExpression(),
                            "Api"),
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            "creationStackTrace")));

                var freeMethodInvokeExpression =
                    new CodeMethodInvokeExpression(
                                    new CodeMethodReferenceExpression(
                                    new CodePropertyReferenceExpression(
                                        new CodePropertyReferenceExpression(
                                            new CodeThisReferenceExpression(),
                                            "Api"),
                                            this.Name),
                                    freeMethod.Name),
                                    new CodeFieldReferenceExpression(
                                        new CodeThisReferenceExpression(),
                                        "handle"));

                if (freeMethod.ReturnType.BaseType != "System.Void")
                {
                    // If the free method returns a value, it's an error code, and we can make sure the value indicates
                    // success.
                    releaseMethod.Statements.Add(
                        new CodeMethodReturnStatement(
                            new CodeBinaryOperatorExpression(
                                freeMethodInvokeExpression,
                                CodeBinaryOperatorType.IdentityEquality,
                                new CodePropertyReferenceExpression(
                                    new CodeTypeReferenceExpression($"{this.Name}Error"),
                                    "Success"))));
                }
                else
                {
                    // If it does not, we always return true (which is a pitty, but this is how plist is implemented for now)
                    // - on the other hand, in how many ways can free() really fail? :-)
                    releaseMethod.Statements.Add(freeMethodInvokeExpression);
                    releaseMethod.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(true)));
                }
            }

            // Extract the API interface and class, as well as the Exception class. Used for DI.
            ApiExtractor extractor = new ApiExtractor(this, functionVisitor);
            extractor.Generate();

            // Add the 'Error' extension IsError and ThrowOnError extension methods
            var extensionsExtractor = new ErrorExtensionExtractor(this, functionVisitor);
            extensionsExtractor.Generate();

            // Patch the native methods to be compatible with .NET Core - basically,
            // do the marshalling ourselves
            NativeMethodOverloadGenerator.Generate(this);
        }

        public void WriteTypes(string targetDirectory)
        {
            var moduleDirectory = Path.Combine(targetDirectory, this.Name);

            if (!Directory.Exists(moduleDirectory))
            {
                Directory.CreateDirectory(moduleDirectory);
            }

            // Write the files
            foreach (var declaration in this.Types)
            {
                if (declaration.Name.EndsWith("Private"))
                {
                    continue;
                }

                string path = Path.Combine(moduleDirectory, $"{declaration.Name}{declaration.FileNameSuffix}.cs");

                using (var outFile = File.Open(path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    if (declaration is CodeDomGeneratedType)
                    {
                        WriteType(outFile, (CodeDomGeneratedType)declaration, declaration.FileNameSuffix);
                    }
                    else if (declaration is NustacheGeneratedType)
                    {
                        WriteType(outFile, (NustacheGeneratedType)declaration, declaration.FileNameSuffix);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }
            }
        }

        public void WriteType(Stream stream, NustacheGeneratedType generatedType, string suffix)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (generatedType == null)
            {
                throw new ArgumentNullException(nameof(generatedType));
            }

            if (generatedType.Type == null)
            {
                throw new ArgumentOutOfRangeException(nameof(generatedType));
            }

            using (var reader = new StreamReader(generatedType.Template))
            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 4096, leaveOpen: true))
            {
                Render.Template(reader, generatedType.Type, writer);
            }

        }

        public void WriteType(Stream stream, CodeDomGeneratedType generatedType, string suffix)
        {
            var declaration = generatedType.Declaration;

            // Generate the container unit
            CodeCompileUnit program = new CodeCompileUnit();

            // Generate the namespace
            CodeNamespace ns = new CodeNamespace($"iMobileDevice.{this.Name}");
            ns.Imports.Add(new CodeNamespaceImport("System.Runtime.InteropServices"));
            ns.Imports.Add(new CodeNamespaceImport("System.Diagnostics"));
            ns.Imports.Add(new CodeNamespaceImport("iMobileDevice.iDevice"));
            ns.Imports.Add(new CodeNamespaceImport("iMobileDevice.Lockdown"));
            ns.Imports.Add(new CodeNamespaceImport("iMobileDevice.Afc"));
            ns.Imports.Add(new CodeNamespaceImport("iMobileDevice.Plist"));
            ns.Types.Add(declaration);
            program.Namespaces.Add(ns);

            StringBuilder builder = new StringBuilder();

            using (var fileWriter = new StringWriter(builder))
            using (var indentedTextWriter = new CSharpTextWriter(fileWriter, "    "))
            {
                // Generate source code using the code provider.
                indentedTextWriter.Generate(ns);
                indentedTextWriter.Flush();
            }

            string content = builder.ToString();

            // Add #if statements for code that doesn't work on .NET Core
            if (true)
            {
                content = content.Replace("#region !core", "#if !NETSTANDARD1_5");
                content = content.Replace("#endregion", "#endif");

                builder.Clear();

                using (StringReader reader = new StringReader(content))
                using (var writer = new StringWriter(builder))
                {
                    while (reader.Peek() >= 0)
                    {
                        string line = reader.ReadLine();

                        if (line.Contains(nameof(SecurityPermissionAttribute))
                            || line.Contains(nameof(ReliabilityContractAttribute))
                            || line.Contains(nameof(SerializableAttribute)))
                        {
                            writer.WriteLine("#if !NETSTANDARD1_5");
                            writer.WriteLine(line);
                            writer.WriteLine("#endif");
                        }
                        else if (line.Contains("SerializationInfo info"))
                        {
                            writer.WriteLine("#if !NETSTANDARD1_5");
                            writer.WriteLine(line);
                            writer.WriteLine(reader.ReadLine());
                            writer.WriteLine(reader.ReadLine());
                            writer.WriteLine(reader.ReadLine());
                            writer.WriteLine("#endif");
                        }
                        else
                        {
                            writer.WriteLine(line);
                        }
                    }
                }

                content = builder.ToString();
            }

            // Fix other CodeDOM shortcomings
            if (declaration.Name.EndsWith("NativeMethods") && string.IsNullOrEmpty(suffix))
            {
                content = content.Replace("public abstract", "public static extern");
            }

            if (declaration.Name.EndsWith("Extensions"))
            {
                content = content.Replace("public class", "public static class");
            }

            using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 4096, leaveOpen: true))
            {
                writer.Write(content);
            }
        }
    }
}
