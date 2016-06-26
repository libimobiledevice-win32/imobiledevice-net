// <copyright file="StructVisitor.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.CodeDom;
    using ClangSharp;
    using System.Runtime.InteropServices;
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
            if (clang.Location_isFromMainFile(clang.getCursorLocation(cursor)) == 0)
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
                    this.current.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                    this.generator.AddType(nativeName, this.current);

                    var layoutAttribute =
                        new CodeAttributeDeclaration(
                            new CodeTypeReference(typeof(StructLayoutAttribute)),
                            new CodeAttributeArgument(
                                new CodePropertyReferenceExpression(
                                    new CodeTypeReferenceExpression(typeof(LayoutKind)),
                                    nameof(LayoutKind.Sequential))));

                    this.current.CustomAttributes.Add(layoutAttribute);

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

                foreach (var member in cursor.ToCodeTypeMember(fieldName, this.generator))
                {
                    this.current.Members.Add(member);
                }

                return CXChildVisitResult.CXChildVisit_Continue;
            }

            return CXChildVisitResult.CXChildVisit_Recurse;
        }
    }
}
