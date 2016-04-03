// <copyright file="CXTypeExtensions.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
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
    }
}
