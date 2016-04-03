// <copyright file="FunctionVisitor.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.CodeDom;
    using System.Runtime.InteropServices;
    using ClangSharp;

    internal sealed class FunctionVisitor
    {
        private readonly string libraryName;
        private readonly ModuleGenerator generator;
        private CodeTypeDeclaration nativeMethods;

        public FunctionVisitor(ModuleGenerator generator, string libraryName)
        {
            this.generator = generator;
            this.libraryName = libraryName;
        }

        public CXChildVisitResult Visit(CXCursor cursor, CXCursor parent, IntPtr data)
        {
            if (clang.Location_isFromMainFile(clang.getCursorLocation(cursor)) == 0)
            {
                return CXChildVisitResult.CXChildVisit_Continue;
            }

            if (this.nativeMethods == null)
            {
                var name = this.generator.Name + "NativeMethods";
                this.nativeMethods = new CodeTypeDeclaration();
                this.nativeMethods.Name = name;
                this.nativeMethods.Attributes = MemberAttributes.Static;
                this.nativeMethods.Members.Add(
                    new CodeMemberField(typeof(string), "libraryName")
                    {
                        Attributes = MemberAttributes.Const,
                        InitExpression = new CodePrimitiveExpression(this.libraryName)
                    });

                this.generator.Types.Add(this.nativeMethods);
            }

            CXCursorKind curKind = clang.getCursorKind(cursor);

            // look only at function decls
            if (curKind == CXCursorKind.CXCursor_FirstDecl)
            {
                return CXChildVisitResult.CXChildVisit_Recurse;
            }

            if (curKind == CXCursorKind.CXCursor_FunctionDecl)
            {
                var function = this.WriteFunctionInfoHelper(cursor);
                this.nativeMethods.Members.Add(function);
                return CXChildVisitResult.CXChildVisit_Continue;
            }

            return CXChildVisitResult.CXChildVisit_Continue;
        }

        private CodeMemberMethod WriteFunctionInfoHelper(CXCursor cursor)
        {
            var functionType = clang.getCursorType(cursor);
            var nativeName = clang.getCursorSpelling(cursor).ToString();
            var resultType = clang.getCursorResultType(cursor);

            CodeMemberMethod method = new CodeMemberMethod();
            method.CustomAttributes.Add(this.DllImportAttribute(nativeName, functionType.GetCallingConvention()));

            // These methods really should not be abstract, but extern, but CodeDOM doesn't support it -
            // so we'll do post-processing to fix this (sigh).
            method.Attributes = MemberAttributes.Public | MemberAttributes.Abstract;
            method.ReturnType = resultType.ToCodeTypeReference(cursor, this.generator);
            method.Name = nativeName;

            int numArgTypes = clang.getNumArgTypes(functionType);

            for (uint i = 0; i < numArgTypes; ++i)
            {
                var argument = Argument.GenerateArgument(this.generator, functionType, clang.Cursor_getArgument(cursor, i), i);
                method.Parameters.Add(argument);
            }

            return method;
        }

        private CodeAttributeDeclaration DllImportAttribute(string entryPoint, CallingConvention callingConvention)
        {
            return new CodeAttributeDeclaration(
                new CodeTypeReference(typeof(DllImportAttribute)),
                new CodeAttributeArgument(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(this.nativeMethods.Name), "libraryName")),
                new CodeAttributeArgument(
                    "EntryPoint",
                    new CodePrimitiveExpression(entryPoint)),
                new CodeAttributeArgument(
                    "CallingConvention",
                    new CodePropertyReferenceExpression(
                        new CodeTypeReferenceExpression(typeof(CallingConvention)),
                        callingConvention.ToString())));
        }
    }
}