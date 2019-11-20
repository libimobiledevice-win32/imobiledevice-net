// <copyright file="CXTypeExtensions.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using ClangSharp.Interop;
    using System;
    using System.CodeDom;
    using System.Runtime.InteropServices;

    internal static class CXTypeExtensions
    {
        public static CallingConvention GetCallingConvention(this CXType type)
        {
            return CallingConvention.Cdecl;
        }

        public static bool IsDoubleCharPointer(this CXType type)
        {
            return IsCharPointer(type, 1);
        }

        public static bool IsTripleCharPointer(this CXType type)
        {
            return IsCharPointer(type, 2);
        }

        public static bool IsCharPointer(this CXType type, int depth)
        {
            var pointee = type;

            for (int i = 0; i < depth; i++)
            {
                pointee = pointee.PointeeType;

                if (pointee.kind != CXTypeKind.CXType_Pointer)
                {
                    return false;
                }
            }

            pointee = pointee.PointeeType;

            return pointee.kind == CXTypeKind.CXType_Char_S;
        }

        public static bool IsPtrToChar(this CXType type)
        {
            var pointee = type.PointeeType;

            if (!pointee.IsConstQualified)
            {
                switch (pointee.kind)
                {
                    case CXTypeKind.CXType_Char_S:
                        return true;
                }
            }

            return false;
        }

        public static bool IsPtrToConstChar(this CXType type)
        {
            var pointee = type.PointeeType;

            if (pointee.IsConstQualified)
            {
                switch (pointee.kind)
                {
                    case CXTypeKind.CXType_Char_S:
                        return true;
                }
            }

            return false;
        }

        public static bool IsArrayOfCharPointers(this CXType type)
        {
            if (type.kind != CXTypeKind.CXType_IncompleteArray)
            {
                return false;
            }

            var elementType = type.ArrayElementType;

            if (elementType.kind != CXTypeKind.CXType_Pointer)
            {
                return false;
            }

            var pointeeType = elementType.PointeeType;

            return pointeeType.kind == CXTypeKind.CXType_Char_S;
        }

        public static bool IsDoublePtrToConstChar(this CXType type)
        {
            if (type.kind != CXTypeKind.CXType_Pointer)
            {
                return false;
            }

            var pointee = type.PointeeType;

            if (pointee.kind != CXTypeKind.CXType_Pointer)
            {
                return false;
            }

            return pointee.IsPtrToConstChar();
        }

        public unsafe static CodeTypeDelegate ToDelegate(this CXType type, string nativeName, CXCursor cursor, ModuleGenerator generator)
        {
            if (type.kind != CXTypeKind.CXType_FunctionProto
                && type.kind != CXTypeKind.CXType_Unexposed)
            {
                throw new InvalidOperationException();
            }

            var clrName = NameConversions.ToClrName(nativeName, NameConversion.Type);

            CodeTypeDelegate delegateType = new CodeTypeDelegate();
            delegateType.CustomAttributes.Add(
                new CodeAttributeDeclaration(
                    new CodeTypeReference(typeof(UnmanagedFunctionPointerAttribute)),
                    new CodeAttributeArgument(
                        new CodePropertyReferenceExpression(
                            new CodeTypeReferenceExpression(typeof(CallingConvention)),
                            type.GetCallingConvention().ToString()))));

            delegateType.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            delegateType.Name = clrName;
            delegateType.ReturnType = new CodeTypeReference(type.ResultType.ToClrType());

            uint argumentCounter = 0;

            var cursorVisitor = new DelegatingCXCursorVisitor(
                delegate (CXCursor c, CXCursor parent1)
                {
                    if (c.Kind == CXCursorKind.CXCursor_ParmDecl)
                    {
                        delegateType.Parameters.Add(Argument.GenerateArgument(generator, type, c, argumentCounter++, FunctionType.Delegate));
                    }

                    return CXChildVisitResult.CXChildVisit_Continue;
                });
            cursor.VisitChildren(cursorVisitor.Visit, new CXClientData());

            return delegateType;
        }
    }
}
