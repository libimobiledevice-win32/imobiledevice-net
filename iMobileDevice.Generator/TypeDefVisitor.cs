// <copyright file="TypeDefVisitor.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using ClangSharp.Interop;
    using System.CodeDom;
    using System.Linq;

    internal class TypeDefVisitor
    {
        private readonly ModuleGenerator generator;

        public TypeDefVisitor(ModuleGenerator generator)
        {
            this.generator = generator;
        }

        public CXChildVisitResult Visit(CXCursor cursor, CXCursor parent)
        {
            if (!cursor.Location.IsFromMainFile)
            {
                return CXChildVisitResult.CXChildVisit_Continue;
            }

            CXCursorKind curKind = cursor.Kind;
            if (curKind == CXCursorKind.CXCursor_TypedefDecl)
            {
                var nativeName = cursor.Spelling.CString;
                var clrName = NameConversions.ToClrName(nativeName, NameConversion.Type);

                // if we've printed these previously, skip them
                if (this.generator.NameMapping.ContainsKey(nativeName))
                {
                    return CXChildVisitResult.CXChildVisit_Continue;
                }

                CXType type = cursor.TypedefDeclUnderlyingType.CanonicalType;

                // we handle enums and records in struct and enum visitors with forward declarations also
                if (type.kind == CXTypeKind.CXType_Record|| type.kind == CXTypeKind.CXType_Enum)
                {
                    return CXChildVisitResult.CXChildVisit_Continue;
                }

                if (type.kind == CXTypeKind.CXType_Pointer)
                {
                    var pointee = type.PointeeType;
                    if (pointee.kind == CXTypeKind.CXType_Record|| pointee.kind == CXTypeKind.CXType_Void)
                    {
                        var types = Handles.CreateSafeHandle(clrName, this.generator).ToArray();
                        this.generator.AddType(nativeName, types[0]);

                        for (int i = 1; i < types.Length; i++)
                        {
                            this.generator.AddType(types[i].Name, types[i]);
                        }

                        return CXChildVisitResult.CXChildVisit_Continue;
                    }

                    if (pointee.kind == CXTypeKind.CXType_FunctionProto)
                    {
                        var functionType = cursor.TypedefDeclUnderlyingType;
                        var pt = functionType.PointeeType;
                        CodeTypeDelegate delegateType = pt.ToDelegate(nativeName, cursor, this.generator);
                        this.generator.AddType(nativeName, new CodeDomGeneratedType(delegateType));

                        return CXChildVisitResult.CXChildVisit_Continue;
                    }
                }

                return CXChildVisitResult.CXChildVisit_Continue;
            }

            return CXChildVisitResult.CXChildVisit_Recurse;
        }
    }
}
