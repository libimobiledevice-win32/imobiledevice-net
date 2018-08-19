// <copyright file="StructVisitor.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.CodeDom;
    using System.Runtime.InteropServices;
    using Core.Clang;

    internal class StructVisitor
    {
        private readonly ModuleGenerator generator;
        private int fieldPosition;
        private CodeTypeDeclaration current;

        public StructVisitor(ModuleGenerator generator)
        {
            this.generator = generator;
        }

        public ChildVisitResult Visit(Cursor cursor, Cursor parent)
        {
            if (!cursor.GetLocation().IsFromMainFile())
            {
                return ChildVisitResult.Continue;
            }

            CursorKind curKind = cursor.Kind;
            if (curKind == CursorKind.StructDecl)
            {
                this.fieldPosition = 0;
                var nativeName = cursor.GetSpelling();

                // struct names can be empty, and so we visit its sibling to find the name
                if (string.IsNullOrEmpty(nativeName))
                {
                    var forwardDeclaringVisitor = new ForwardDeclarationVisitor(cursor, skipSystemHeaderCheck: true);
                    forwardDeclaringVisitor.VisitChildren(cursor.GetSemanticParent());
                    nativeName = forwardDeclaringVisitor.ForwardDeclarationCursor.GetSpelling();

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

                    var visitor = new DelegatingCursorVisitor(this.Visit);
                    visitor.VisitChildren(cursor);
                }

                return ChildVisitResult.Continue;
            }

            if (curKind == CursorKind.FieldDecl)
            {
                var fieldName = cursor.GetSpelling();
                if (string.IsNullOrEmpty(fieldName))
                {
                    fieldName = "field" + this.fieldPosition; // what if they have fields called field*? :)
                }

                this.fieldPosition++;

                foreach (var member in cursor.ToCodeTypeMember(fieldName, this.generator))
                {
                    this.current.Members.Add(member);
                }

                return ChildVisitResult.Continue;
            }

            return ChildVisitResult.Recurse;
        }
    }
}
