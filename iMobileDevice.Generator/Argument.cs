// <copyright file="Argument.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.CodeDom;
    using System.Runtime.InteropServices;
    using ClangSharp;
    using System.Collections.ObjectModel;
    internal static class Argument
    {
        public static CodeAttributeDeclaration MarshalAsFixedLengthStringDeclaration(int size)
        {
            var value = new CodeAttributeDeclaration(
                new CodeTypeReference(typeof(MarshalAsAttribute)),
                new CodeAttributeArgument(
                    new CodePropertyReferenceExpression(
                        new CodeTypeReferenceExpression(typeof(UnmanagedType)),
                        UnmanagedType.ByValTStr.ToString())));

            value.Arguments.Add(
                new CodeAttributeArgument(
                    "SizeConst",
                    new CodePrimitiveExpression(size)));

            return value;
        }

        public static CodeAttributeDeclaration MarshalAsDeclaration(UnmanagedType type, CodeTypeReference customMarshaler = null)
        {
            var value = new CodeAttributeDeclaration(
                new CodeTypeReference(typeof(MarshalAsAttribute)),
                new CodeAttributeArgument(
                    new CodePropertyReferenceExpression(
                        new CodeTypeReferenceExpression(typeof(UnmanagedType)),
                        type.ToString())));

            if (type == UnmanagedType.CustomMarshaler)
            {
                value.Arguments.Add(
                    new CodeAttributeArgument(
                        "MarshalTypeRef",
                        new CodeTypeOfExpression(
                            customMarshaler)));
            }

            return value;
        }

        public static CodeParameterDeclarationExpression GenerateArgument(this ModuleGenerator generator, CXType functionType, CXCursor paramCursor, uint index, FunctionType functionKind)
        {
            var numArgTypes = clang.getNumArgTypes(functionType);
            var type = clang.getArgType(functionType, index);
            var cursorType = clang.getCursorType(paramCursor);

            var name = clang.getCursorSpelling(paramCursor).ToString();
            if (string.IsNullOrEmpty(name))
            {
                name = "param" + index;
            }

            name = NameConversions.ToClrName(name, NameConversion.Parameter);

            CodeParameterDeclarationExpression parameter = new CodeParameterDeclarationExpression();
            parameter.Name = name;

            bool isPointer = false;

            if (functionKind != FunctionType.Free
                && type.IsDoubleCharPointer())
            {
                parameter.Type = new CodeTypeReference(typeof(string));
                parameter.Direction = FieldDirection.Out;

                parameter.CustomAttributes.Add(MarshalAsDeclaration(UnmanagedType.CustomMarshaler, new CodeTypeReference("NativeStringMarshaler")));
            }
            else if (type.IsTripleCharPointer() && generator.StringArrayMarshalerType != null)
            {
                parameter.Type = new CodeTypeReference(typeof(ReadOnlyCollection<string>));
                parameter.Direction = FieldDirection.Out;

                parameter.CustomAttributes.Add(MarshalAsDeclaration(UnmanagedType.CustomMarshaler, new CodeTypeReference(generator.StringArrayMarshalerType.Name)));
            }
            else
            {
                switch (type.kind)
                {
                    case CXTypeKind.CXType_Pointer:
                        var pointee = clang.getPointeeType(type);
                        switch (pointee.kind)
                        {
                            case CXTypeKind.CXType_Pointer:
                                parameter.Type = new CodeTypeReference(typeof(IntPtr));
                                isPointer = true;

                                break;

                            case CXTypeKind.CXType_FunctionProto:
                                parameter.Type = new CodeTypeReference(cursorType.ToClrType());
                                break;

                            case CXTypeKind.CXType_Void:
                                parameter.Type = new CodeTypeReference(typeof(IntPtr));
                                break;

                            case CXTypeKind.CXType_Char_S:
                                // In some of the read/write functions, const char is also used to represent data -- in that
                                // case, it maps to a byte[] array or just an IntPtr.
                                if (type.IsPtrToConstChar())
                                {
                                    if (!name.Contains("data") && name != "signature")
                                    {
                                        parameter.Type = new CodeTypeReference(typeof(string));
                                        parameter.CustomAttributes.Add(MarshalAsDeclaration(UnmanagedType.LPStr));
                                    }
                                    else
                                    {
                                        parameter.Type = new CodeTypeReference(typeof(byte[]));
                                    }
                                }
                                else if(type.IsPtrToChar() && name.Contains("data"))
                                {
                                    parameter.Type = new CodeTypeReference(typeof(byte[]));
                                }
                                else
                                {
                                    // if it's not a const, it's best to go with IntPtr
                                    parameter.Type = new CodeTypeReference(typeof(IntPtr));
                                }

                                break;

                            case CXTypeKind.CXType_WChar:
                                if (type.IsPtrToConstChar())
                                {
                                    parameter.Type = new CodeTypeReference(typeof(string));
                                    parameter.CustomAttributes.Add(MarshalAsDeclaration(UnmanagedType.LPWStr));
                                }
                                else
                                {
                                    parameter.Type = new CodeTypeReference(typeof(IntPtr));
                                }

                                break;

                            case CXTypeKind.CXType_Record:
                                var recordTypeCursor = clang.getTypeDeclaration(pointee);
                                var recordType = clang.getCursorType(recordTypeCursor);

                                // Get the CLR name for the record
                                var clrName = generator.NameMapping[recordType.ToString()];
                                parameter.Type = new CodeTypeReference(clrName);
                                isPointer = true;
                                break;

                            default:
                                parameter.Type = pointee.ToCodeTypeReference(paramCursor, generator);
                                isPointer = true;
                                break;
                        }

                        break;

                    default:
                        parameter.Type = type.ToCodeTypeReference(paramCursor, generator);
                        break;
                }
            }

            if (isPointer)
            {
                switch (functionKind)
                {
                    case FunctionType.None:
                        if (parameter.Type.BaseType.EndsWith("Handle"))
                        {
                            // Handles are always out parameters
                            parameter.Direction = FieldDirection.Out;
                        }
                        else
                        {
                            // For IntPtrs, we don't know - so we play on the safe side.
                            parameter.Direction = FieldDirection.Ref;
                        }
                        break;

                    case FunctionType.New:
                        parameter.Direction = FieldDirection.Out;
                        break;

                    case FunctionType.Free:
                        parameter.Direction = FieldDirection.In;
                        break;
                }
            }

            return parameter;
        }
    }
}
