// <copyright file="CXTypeExtensions.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.CodeDom;
    using System.Runtime.InteropServices;
    using ClangSharp;

    internal static class CXTypeExtensions
    {
        public static CallingConvention GetCallingConvention(this CXType type)
        {
            var callingConvention = clang.getFunctionTypeCallingConv(type);
            switch (callingConvention)
            {
                case CXCallingConv.CXCallingConv_X86StdCall:
                case CXCallingConv.CXCallingConv_X86_64Win64:
                    return CallingConvention.StdCall;
                default:
                    return CallingConvention.Cdecl;
            }
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
                pointee = clang.getPointeeType(pointee);

                if (pointee.kind != CXTypeKind.CXType_Pointer)
                {
                    return false;
                }
            }

            pointee = clang.getPointeeType(pointee);

            return pointee.kind == CXTypeKind.CXType_Char_S;
        }

        public static bool IsPtrToChar(this CXType type)
        {
            var pointee = clang.getPointeeType(type);

            if (clang.isConstQualifiedType(pointee) == 0)
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
            var pointee = clang.getPointeeType(type);

            if (clang.isConstQualifiedType(pointee) != 0)
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

            var elementType = clang.getArrayElementType(type);

            if (elementType.kind != CXTypeKind.CXType_Pointer)
            {
                return false;
            }

            var pointeeType = clang.getPointeeType(elementType);

            return pointeeType.kind == CXTypeKind.CXType_Char_S;
        }

        public static bool IsDoublePtrToConstChar(this CXType type)
        {
            if (type.kind != CXTypeKind.CXType_Pointer)
            {
                return false;
            }

            var pointee = clang.getPointeeType(type);

            if (pointee.kind != CXTypeKind.CXType_Pointer)
            {
                return false;
            }

            return pointee.IsPtrToConstChar();
        }

        public static CodeTypeDelegate ToDelegate(this CXType type, string nativeName, CXCursor cursor, ModuleGenerator generator)
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

            delegateType.Attributes = MemberAttributes.Public;
            delegateType.Name = clrName;
            delegateType.ReturnType = new CodeTypeReference(clang.getResultType(type).ToClrType());

            uint argumentCounter = 0;

            clang.visitChildren(
                cursor,
                delegate (CXCursor cxCursor, CXCursor parent1, IntPtr ptr)
                {
                    if (cxCursor.kind == CXCursorKind.CXCursor_ParmDecl)
                    {
                        delegateType.Parameters.Add(Argument.GenerateArgument(generator, type, cxCursor, argumentCounter++, FunctionType.Delegate));
                    }

                    return CXChildVisitResult.CXChildVisit_Continue;
                },
                new CXClientData(IntPtr.Zero));

            return delegateType;
        }
    }
}
