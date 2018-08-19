// <copyright file="TypeKindExtensions.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.CodeDom;
    using Core.Clang;

    internal static class TypeKindExtensions
    {
        public static CodeTypeReference ToCodeTypeReference(this TypeInfo type, Cursor cursor, ModuleGenerator generator)
        {
            var nativeName = type.GetSpelling();
            var canonical = type.GetCanonicalType();

            // Special case: function prototypes embedded in the function declaration
            if (canonical.Kind == TypeKind.FunctionProto)
            {
                // Generate the delegate and add it to the list of members
                nativeName = cursor.GetSpelling();
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

        public static Type ToClrType(this TypeInfo type)
        {
            var canonical = type.GetCanonicalType();

            switch (canonical.Kind)
            {
                case TypeKind.Bool:
                    return typeof(bool);

                case TypeKind.UChar:
                case TypeKind.Char_U:
                    return typeof(char);

                case TypeKind.SChar:
                case TypeKind.Char_S:
                    return typeof(sbyte);

                case TypeKind.UShort:
                    return typeof(ushort);

                case TypeKind.Short:
                    return typeof(short);

                case TypeKind.Float:
                    return typeof(float);

                case TypeKind.Double:
                    return typeof(double);

                case TypeKind.Int:
                case TypeKind.Enum:
                    return typeof(int);

                case TypeKind.UInt:
                    return typeof(uint);

                case TypeKind.Pointer:
                case TypeKind.IncompleteArray:
                    return typeof(IntPtr);

                case TypeKind.Long:
                    return typeof(int);

                case TypeKind.ULong:
                    return typeof(int);

                case TypeKind.LongLong:
                    return typeof(long);

                case TypeKind.ULongLong:
                    return typeof(ulong);

                case TypeKind.Void:
                    return typeof(void);

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
