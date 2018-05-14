// <copyright file="CXTypeExtensions.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.CodeDom;
    using System.Runtime.InteropServices;
    using Core.Clang;

    internal static class CXTypeExtensions
    {
        public static CallingConvention GetCallingConvention(this TypeInfo type)
        {
            return CallingConvention.Cdecl;
        }

        public static bool IsDoubleCharPointer(this TypeInfo type)
        {
            return IsCharPointer(type, 1);
        }

        public static bool IsTripleCharPointer(this TypeInfo type)
        {
            return IsCharPointer(type, 2);
        }

        public static bool IsCharPointer(this TypeInfo type, int depth)
        {
            var pointee = type;

            for (int i = 0; i < depth; i++)
            {
                pointee = pointee.GetPointeeType();

                if (pointee.Kind != TypeKind.Pointer)
                {
                    return false;
                }
            }

            pointee = pointee.GetPointeeType();

            return pointee.Kind == TypeKind.Char_S;
        }

        public static bool IsPtrToChar(this TypeInfo type)
        {
            var pointee = type.GetPointeeType();

            if (!pointee.IsConstQualified())
            {
                switch (pointee.Kind)
                {
                    case TypeKind.Char_S:
                        return true;
                }
            }

            return false;
        }

        public static bool IsPtrToConstChar(this TypeInfo type)
        {
            var pointee = type.GetPointeeType();

            if (pointee.IsConstQualified())
            {
                switch (pointee.Kind)
                {
                    case TypeKind.Char_S:
                        return true;
                }
            }

            return false;
        }

        public static bool IsArrayOfCharPointers(this TypeInfo type)
        {
            if (type.Kind != TypeKind.IncompleteArray)
            {
                return false;
            }

            var elementType = type.GetArrayElementType();

            if (elementType.Kind != TypeKind.Pointer)
            {
                return false;
            }

            var pointeeType = elementType.GetPointeeType();

            return pointeeType.Kind == TypeKind.Char_S;
        }

        public static bool IsDoublePtrToConstChar(this TypeInfo type)
        {
            if (type.Kind != TypeKind.Pointer)
            {
                return false;
            }

            var pointee = type.GetPointeeType();

            if (pointee.Kind != TypeKind.Pointer)
            {
                return false;
            }

            return pointee.IsPtrToConstChar();
        }

        public static CodeTypeDelegate ToDelegate(this TypeInfo type, string nativeName, Cursor cursor, ModuleGenerator generator)
        {
            if (type.Kind != TypeKind.FunctionProto
                && type.Kind != TypeKind.Unexposed)
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
            delegateType.ReturnType = new CodeTypeReference(type.GetResultType().ToClrType());

            uint argumentCounter = 0;

            var cursorVisitor = new DelegatingCursorVisitor(
                delegate (Cursor c, Cursor parent1)
                {
                    if (c.Kind == CursorKind.ParmDecl)
                    {
                        delegateType.Parameters.Add(Argument.GenerateArgument(generator, type, c, argumentCounter++, FunctionType.Delegate));
                    }

                    return ChildVisitResult.Continue;
                });
            cursorVisitor.VisitChildren(cursor);

            return delegateType;
        }
    }
}
