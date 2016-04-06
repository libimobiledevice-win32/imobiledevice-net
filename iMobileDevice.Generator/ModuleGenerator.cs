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

        public CodeTypeDeclaration StringArrayMarshalerType
        { get; set; }

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

            StringWriter errorWriter = new StringWriter();
            var numDiagnostics = clang.getNumDiagnostics(translationUnit);

            bool hasError = false;
            bool hasWarning = false;

            for (uint i = 0; i < numDiagnostics; ++i)
            {
                var diagnostic = clang.getDiagnostic(translationUnit, i);

                var severity = clang.getDiagnosticSeverity(diagnostic);

                switch (severity)
                {
                    case CXDiagnosticSeverity.CXDiagnostic_Error:
                    case CXDiagnosticSeverity.CXDiagnostic_Fatal:
                        hasError = true;
                        break;

                    case CXDiagnosticSeverity.CXDiagnostic_Warning:
                        hasWarning = true;
                        break;
                }

                var location = clang.getDiagnosticLocation(diagnostic);
                CXFile file;
                uint line;
                uint column;
                uint offset;

                clang.getFileLocation(location, out file, out line, out column, out offset);

                var fileName = clang.getFileName(file).ToString();

                var message = clang.getDiagnosticSpelling(diagnostic).ToString();
                errorWriter.WriteLine($"{severity}: {fileName}:{line} {message}");
                clang.disposeDiagnostic(diagnostic);
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
            clang.visitChildren(clang.getTranslationUnitCursor(translationUnit), enumVisitor.Visit, new CXClientData(IntPtr.Zero));

            // Creates structs
            var structVisitor = new StructVisitor(this);
            clang.visitChildren(clang.getTranslationUnitCursor(translationUnit), structVisitor.Visit, new CXClientData(IntPtr.Zero));

            // Creates safe handles & delegates
            var typeDefVisitor = new TypeDefVisitor(this);
            clang.visitChildren(clang.getTranslationUnitCursor(translationUnit), typeDefVisitor.Visit, new CXClientData(IntPtr.Zero));

            // Creates functions in a NativeMethods class
            var functionVisitor = new FunctionVisitor(this, "libimobiledevice");
            clang.visitChildren(clang.getTranslationUnitCursor(translationUnit), functionVisitor.Visit, new CXClientData(IntPtr.Zero));

            clang.disposeTranslationUnit(translationUnit);
            clang.disposeIndex(createIndex);

            var moduleDirectory = Path.Combine(targetDirectory, this.Name);

            if (!Directory.Exists(moduleDirectory))
            {
                Directory.CreateDirectory(moduleDirectory);
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

                var releaseMethod = handle.Members.OfType<CodeMemberMethod>().Single(m => m.Name == "ReleaseHandle");

                // Sample statement:
                // return !LibiMobileDevice.Instance.iDevice.idevice_free(this).IsError();
                releaseMethod.Statements.Clear();

                var freeMethodInvokeExpression =
                    new CodeMethodInvokeExpression(
                                    new CodeMethodReferenceExpression(
                                    new CodePropertyReferenceExpression(
                                        new CodePropertyReferenceExpression(
                                            new CodeTypeReferenceExpression("LibiMobileDevice"),
                                            "Instance"),
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
                ns.Imports.Add(new CodeNamespaceImport("iMobileDevice.Plist"));
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
