// <copyright file="StructVisitor.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using ClangSharp.Interop;
    using System.CodeDom;
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

        public unsafe CXChildVisitResult Visit(CXCursor cursor, CXCursor parent)
        {
            if (!cursor.Location.IsFromMainFile)
            {
                return CXChildVisitResult.CXChildVisit_Continue;
            }

            CXCursorKind curKind = cursor.Kind;
            if (curKind == CXCursorKind.CXCursor_StructDecl)
            {
                this.fieldPosition = 0;
                var nativeName = cursor.Spelling.CString;

                // struct names can be empty, and so we visit its sibling to find the name
                if (string.IsNullOrEmpty(nativeName))
                {
                    var forwardDeclaringVisitor = new ForwardDeclarationVisitor(cursor, skipSystemHeaderCheck: true);
                    cursor.SemanticParent.VisitChildren(forwardDeclaringVisitor.Visit, new CXClientData());
                    nativeName = forwardDeclaringVisitor.ForwardDeclarationCXCursor.Spelling.CString;

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
                    this.generator.AddType(nativeName, new CodeDomGeneratedType(this.current));

                    var layoutAttribute =
                        new CodeAttributeDeclaration(
                            new CodeTypeReference(typeof(StructLayoutAttribute)),
                            new CodeAttributeArgument(
                                new CodePropertyReferenceExpression(
                                    new CodeTypeReferenceExpression(typeof(LayoutKind)),
                                    nameof(LayoutKind.Sequential))));

                    this.current.CustomAttributes.Add(layoutAttribute);

                    var visitor = new DelegatingCXCursorVisitor(this.Visit);
                    cursor.VisitChildren(visitor.Visit, new CXClientData());
                }

                return CXChildVisitResult.CXChildVisit_Continue;
            }

            if (curKind == CXCursorKind.CXCursor_FieldDecl)
            {
                var fieldName = cursor.Spelling.CString;
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
