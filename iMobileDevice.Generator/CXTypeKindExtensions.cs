// <copyright file="TypeKindExtensions.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using ClangSharp.Interop;
    using System;
    using System.CodeDom;

    internal static class TypeKindExtensions
    {
        public static CodeTypeReference ToCodeTypeReference(this CXType type, CXCursor cursor, ModuleGenerator generator)
        {
            var nativeName = type.Spelling.CString;
            var canonical = type.CanonicalType;

            // Special case: function prototypes embedded in the function declaration
            if (canonical.kind == CXTypeKind.CXType_FunctionProto)
            {
                // Generate the delegate and add it to the list of members
                nativeName = cursor.Spelling.CString;
                var delegateType = type.ToDelegate(nativeName, cursor, generator);
                generator.AddType(nativeName, new CodeDomGeneratedType(delegateType));
            }

            if (nativeName.StartsWith("const "))
            {
                nativeName = nativeName.Substring(6);
            }

            if (generator.NameMapping.ContainsKey(nativeName))
            {
                return new CodeTypeReference(generator.NameMapping[nativeName]);
            }
            else
            {
                return new CodeTypeReference(type.ToClrType());
            }
        }

        public static Type ToClrType(this CXType type)
        {
            var canonical = type.CanonicalType;

            switch (canonical.kind)
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
                case CXTypeKind.CXType_Enum:
                    return typeof(int);

                case CXTypeKind.CXType_UInt:
                    return typeof(uint);

                case CXTypeKind.CXType_Pointer:
                case CXTypeKind.CXType_IncompleteArray:
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
