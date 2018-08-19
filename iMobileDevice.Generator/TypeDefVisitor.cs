// <copyright file="TypeDefVisitor.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.CodeDom;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Core.Clang;

    internal class TypeDefVisitor
    {
        private readonly ModuleGenerator generator;

        public TypeDefVisitor(ModuleGenerator generator)
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
            if (curKind == CursorKind.TypedefDecl)
            {
                var nativeName = cursor.GetSpelling();
                var clrName = NameConversions.ToClrName(nativeName, NameConversion.Type);

                // if we've printed these previously, skip them
                if (this.generator.NameMapping.ContainsKey(nativeName))
                {
                    return ChildVisitResult.Continue;
                }

                TypeInfo type = cursor.GetTypedefDeclUnderlyingType().GetCanonicalType();

                // we handle enums and records in struct and enum visitors with forward declarations also
                if (type.Kind == TypeKind.Record || type.Kind == TypeKind.Enum)
                {
                    return ChildVisitResult.Continue;
                }

                if (type.Kind == TypeKind.Pointer)
                {
                    var pointee = type.GetPointeeType();
                    if (pointee.Kind == TypeKind.Record || pointee.Kind == TypeKind.Void)
                    {
                        var types = Handles.CreateSafeHandle(clrName, this.generator).ToArray();
                        this.generator.AddType(nativeName, types[0]);

                        for (int i = 1; i < types.Length; i++)
                        {
                            this.generator.AddType(types[i].Name, types[i]);
                        }

                        return ChildVisitResult.Continue;
                    }

                    if (pointee.Kind == TypeKind.FunctionProto)
                    {
                        var functionType = cursor.GetTypedefDeclUnderlyingType();
                        var pt = functionType.GetPointeeType();
                        CodeTypeDelegate delegateType = pt.ToDelegate(nativeName, cursor, this.generator);
                        this.generator.AddType(nativeName, new CodeDomGeneratedType(delegateType));

                        return ChildVisitResult.Continue;
                    }
                }

                return ChildVisitResult.Continue;
            }

            return ChildVisitResult.Recurse;
        }
    }
}
