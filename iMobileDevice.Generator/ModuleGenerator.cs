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
    using ClangSharp;

    internal class ModuleGenerator
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

        public Collection<CodeTypeDeclaration> Types
        { get; } = new Collection<CodeTypeDeclaration>();

        public Dictionary<string, string> NameMapping
        { get; } = new Dictionary<string, string>();

        public void AddType(string nativeName, CodeTypeDeclaration type)
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

        public void Generate(string targetDirectory)
        {
            var createIndex = clang.createIndex(0, 0);

            string[] arguments = { "-x", "c++", "-Wdocumentation" };
            arguments = arguments.Concat(this.IncludeDirectories.Select(x => "-I" + x)).ToArray();

            CXTranslationUnit translationUnit;
            CXUnsavedFile unsavedFile;
            var translationUnitError = clang.parseTranslationUnit2(createIndex, this.InputFile, arguments, arguments.Length, out unsavedFile, 0, 0, out translationUnit);

            if (translationUnitError != CXErrorCode.CXError_Success)
            {
                StringWriter errorWriter = new StringWriter();

                errorWriter.WriteLine("Error: " + translationUnitError);
                var numDiagnostics = clang.getNumDiagnostics(translationUnit);

                for (uint i = 0; i < numDiagnostics; ++i)
                {
                    var diagnostic = clang.getDiagnostic(translationUnit, i);
                    errorWriter.WriteLine(clang.getDiagnosticSpelling(diagnostic).ToString());
                    clang.disposeDiagnostic(diagnostic);
                }

                throw new Exception(errorWriter.ToString());
            }

            var enumVisitor = new EnumVisitor(this);
            clang.visitChildren(clang.getTranslationUnitCursor(translationUnit), enumVisitor.Visit, new CXClientData(IntPtr.Zero));

            var structVisitor = new StructVisitor(this);
            clang.visitChildren(clang.getTranslationUnitCursor(translationUnit), structVisitor.Visit, new CXClientData(IntPtr.Zero));

            var typeDefVisitor = new TypeDefVisitor(this);
            clang.visitChildren(clang.getTranslationUnitCursor(translationUnit), typeDefVisitor.Visit, new CXClientData(IntPtr.Zero));

            var functionVisitor = new FunctionVisitor(this, "libimobiledevice");
            clang.visitChildren(clang.getTranslationUnitCursor(translationUnit), functionVisitor.Visit, new CXClientData(IntPtr.Zero));

            clang.disposeTranslationUnit(translationUnit);
            clang.disposeIndex(createIndex);

            var moduleDirectory = Path.Combine(targetDirectory, this.Name);

            if (!Directory.Exists(moduleDirectory))
            {
                Directory.CreateDirectory(moduleDirectory);
            }

            // Extract the API interface and class, as well as the Exception class. Used for DI.
            ApiExtractor extractor = new ApiExtractor(this, functionVisitor);
            extractor.Generate();

            // Add the 'Error' extension IsError and ThrowOnError extension methods
            var extensionsExtractor = new ErrorExtensionExtractor(this, functionVisitor);
            extensionsExtractor.Generate();

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
            }

            // Write the files
            foreach (var declaration in this.Types)
            {
                if (declaration.Name.EndsWith("Private"))
                {
                    continue;
                }

                // Generate the container unit
                CodeCompileUnit program = new CodeCompileUnit();

                // Generate the namespace
                CodeNamespace ns = new CodeNamespace($"iMobileDevice.{this.Name}");
                ns.Imports.Add(new CodeNamespaceImport("System.Runtime.InteropServices"));
                ns.Imports.Add(new CodeNamespaceImport("iMobileDevice.iDevice"));
                ns.Imports.Add(new CodeNamespaceImport("iMobileDevice.Lockdown"));
                ns.Imports.Add(new CodeNamespaceImport("iMobileDevice.Afc"));
                ns.Types.Add(declaration);
                program.Namespaces.Add(ns);

                string path = Path.Combine(moduleDirectory, $"{declaration.Name}.cs");

                using (var outFile = File.Open(path, FileMode.Create))
                using (var fileWriter = new StreamWriter(outFile))
                using (var indentedTextWriter = new IndentedTextWriter(fileWriter, "    "))
                {
                    // Generate source code using the code provider.
                    var provider = new Microsoft.CSharp.CSharpCodeProvider();
                    provider.GenerateCodeFromCompileUnit(
                        program,
                        indentedTextWriter,
                        new CodeGeneratorOptions() { BracingStyle = "C" });
                }

                if (declaration.Name.EndsWith("NativeMethods"))
                {
                    string content = File.ReadAllText(path);
                    content = content.Replace("public abstract", "public static extern");
                    File.WriteAllText(path, content);
                }

                if (declaration.Name.EndsWith("Extensions"))
                {
                    string content = File.ReadAllText(path);
                    content = content.Replace("public class", "public static class");
                    File.WriteAllText(path, content);
                }
            }
        }
    }
}
