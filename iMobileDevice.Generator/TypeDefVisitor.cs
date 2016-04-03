// <copyright file="TypeDefVisitor.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.CodeDom;
    using System.Runtime.InteropServices;
    using ClangSharp;

    internal class TypeDefVisitor
    {
        private readonly ModuleGenerator generator;

        public TypeDefVisitor(ModuleGenerator generator)
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
            if (curKind == CXCursorKind.CXCursor_TypedefDecl)
            {
                var nativeName = clang.getCursorSpelling(cursor).ToString();
                var clrName = NameConversions.ToClrName(nativeName, NameConversion.Type);

                // if we've printed these previously, skip them
                if (this.generator.NameMapping.ContainsKey(nativeName))
                {
                    return CXChildVisitResult.CXChildVisit_Continue;
                }

                CXType type = clang.getCanonicalType(clang.getTypedefDeclUnderlyingType(cursor));

                // we handle enums and records in struct and enum visitors with forward declarations also
                if (type.kind == CXTypeKind.CXType_Record || type.kind == CXTypeKind.CXType_Enum)
                {
                    return CXChildVisitResult.CXChildVisit_Continue;
                }

                if (type.kind == CXTypeKind.CXType_Pointer)
                {
                    var pointee = clang.getPointeeType(type);
                    if (pointee.kind == CXTypeKind.CXType_Record || pointee.kind == CXTypeKind.CXType_Void)
                    {
                        this.generator.AddType(nativeName, Handles.CreateSafeHandle(clrName));
                        return CXChildVisitResult.CXChildVisit_Continue;
                    }

                    if (pointee.kind == CXTypeKind.CXType_FunctionProto)
                    {
                        CodeTypeDelegate delegateType = new CodeTypeDelegate();
                        delegateType.CustomAttributes.Add(
                            new CodeAttributeDeclaration(
                                new CodeTypeReference(typeof(UnmanagedFunctionPointerAttribute)),
                                new CodeAttributeArgument(
                                    new CodePropertyReferenceExpression(
                                        new CodeTypeReferenceExpression(typeof(CallingConvention)),
                                        pointee.GetCallingConvention().ToString()))));

                        delegateType.Attributes = MemberAttributes.Public;
                        delegateType.Name = clrName;
                        delegateType.ReturnType = new CodeTypeReference(clang.getResultType(pointee).ToClrType());

                        uint argumentCounter = 0;

                        clang.visitChildren(
                            cursor,
                            delegate(CXCursor cxCursor, CXCursor parent1, IntPtr ptr)
                            {
                                if (cxCursor.kind == CXCursorKind.CXCursor_ParmDecl)
                                {
                                    delegateType.Parameters.Add(Argument.GenerateArgument(this.generator, pointee, cxCursor, argumentCounter++));
                                }

                                return CXChildVisitResult.CXChildVisit_Continue;
                            },
                            new CXClientData(IntPtr.Zero));

                        this.generator.AddType(nativeName, delegateType);

                        return CXChildVisitResult.CXChildVisit_Continue;
                    }
                }

                if (clang.isPODType(type) != 0)
                {
                    this.generator.AddType(nativeName, Handles.CreateSafeHandle(clrName));
                }

                return CXChildVisitResult.CXChildVisit_Continue;
            }

            return CXChildVisitResult.CXChildVisit_Recurse;
        }
    }
}
