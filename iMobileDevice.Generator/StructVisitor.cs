// <copyright file="StructVisitor.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.CodeDom;
    using ClangSharp;

    internal class StructVisitor
    {
        private readonly ModuleGenerator generator;
        private int fieldPosition;
        private CodeTypeDeclaration current;

        public StructVisitor(ModuleGenerator generator)
        {
            this.generator = generator;
        }

        public CXChildVisitResult Visit(CXCursor cursor, CXCursor parent, IntPtr data)
        {
            if (cursor.IsInSystemHeader())
            {
                return CXChildVisitResult.CXChildVisit_Continue;
            }

            CXCursorKind curKind = clang.getCursorKind(cursor);
            if (curKind == CXCursorKind.CXCursor_StructDecl)
            {
                this.fieldPosition = 0;
                var nativeName = clang.getCursorSpelling(cursor).ToString();

                // struct names can be empty, and so we visit its sibling to find the name
                if (string.IsNullOrEmpty(nativeName))
                {
                    var forwardDeclaringVisitor = new ForwardDeclarationVisitor(cursor);
                    clang.visitChildren(clang.getCursorSemanticParent(cursor), forwardDeclaringVisitor.Visit, new CXClientData(IntPtr.Zero));
                    nativeName = clang.getCursorSpelling(forwardDeclaringVisitor.ForwardDeclarationCursor).ToString();

                    if (string.IsNullOrEmpty(nativeName))
                    {
                        nativeName = "_";
                    }
                }

                var clrName = NameConversions.ToClrName(nativeName, NameConversion.Type);

                if (!this.generator.NameMapping.ContainsKey(nativeName))
                {
                    this.current = new CodeTypeDeclaration(clrName);
                    this.current.IsStruct = true;
                    this.generator.AddType(nativeName, this.current);

                    clang.visitChildren(cursor, this.Visit, new CXClientData(IntPtr.Zero));
                }

                return CXChildVisitResult.CXChildVisit_Continue;
            }

            if (curKind == CXCursorKind.CXCursor_FieldDecl)
            {
                var fieldName = clang.getCursorSpelling(cursor).ToString();
                if (string.IsNullOrEmpty(fieldName))
                {
                    fieldName = "field" + this.fieldPosition; // what if they have fields called field*? :)
                }

                this.fieldPosition++;

                this.current.Members.Add(cursor.ToCodeTypeMember(fieldName, this.generator));
                return CXChildVisitResult.CXChildVisit_Continue;
            }

            return CXChildVisitResult.CXChildVisit_Recurse;
        }
    }
}
