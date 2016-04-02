// <copyright file="CXTypeKindExtensions.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using ClangSharp;

    internal static class CXTypeKindExtensions
    {
        public static Type ToClrType(this CXType type)
        {
            var canonical = clang.getCanonicalType(type);

            switch (type.kind)
            {
                case CXTypeKind.CXType_Bool:
                    return typeof(bool);

                case CXTypeKind.CXType_UChar:
                case CXTypeKind.CXType_Char_U:
                    return typeof(char);

                case CXTypeKind.CXType_SChar:
                case CXTypeKind.CXType_Char_S:
                    return typeof(sbyte);

                case CXTypeKind.CXType_UShort:
                    return typeof(ushort);

                case CXTypeKind.CXType_Short:
                    return typeof(short);

                case CXTypeKind.CXType_Float:
                    return typeof(float);

                case CXTypeKind.CXType_Double:
                    return typeof(double);

                case CXTypeKind.CXType_Int:
                    return typeof(int);

                case CXTypeKind.CXType_UInt:
                    return typeof(uint);

                case CXTypeKind.CXType_Pointer:
                    return typeof(IntPtr);

                case CXTypeKind.CXType_Long:
                    return typeof(int);

                case CXTypeKind.CXType_ULong:
                    return typeof(int);

                case CXTypeKind.CXType_LongLong:
                    return typeof(long);

                case CXTypeKind.CXType_ULongLong:
                    return typeof(ulong);

                case CXTypeKind.CXType_Void:
                    return typeof(void);

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
