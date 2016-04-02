// <copyright file="ModuleGenerator.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using ClangSharp;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    internal class ModuleGenerator
    {
        public ModuleGenerator(string inputFile)
        {
            this.InputFile = inputFile;

            this.IncludeDirectories.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Microsoft Visual Studio 14.0\VC\include"));
        }

        public string InputFile
        {
            get;
            private set;
        }

        public Collection<string> IncludeDirectories
        { get; } = new Collection<string>();

        public void Generate()
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

            var enumVisitor = new EnumVisitor();
            clang.visitChildren(clang.getTranslationUnitCursor(translationUnit), enumVisitor.Visit, new CXClientData(IntPtr.Zero));

            clang.disposeTranslationUnit(translationUnit);
            clang.disposeIndex(createIndex);

            // Write the enum files
            foreach (var enumDeclaration in enumVisitor.Enums)
            {
                // Generate the container unit
                CodeCompileUnit program = new CodeCompileUnit();

                // Generate the namespace
                CodeNamespace ns = new CodeNamespace("iMobileDevice");
                ns.Types.Add(enumDeclaration);
                program.Namespaces.Add(ns);

                using (var outFile = File.Open($"{enumDeclaration.Name}.cs", FileMode.Create))
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
            }
        }
    }
}
