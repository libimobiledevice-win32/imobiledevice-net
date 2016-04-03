// <copyright file="Argument.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.CodeDom;
    using System.Runtime.InteropServices;
    using ClangSharp;

    internal static class Argument
    {
        public static CodeAttributeDeclaration MarshalAsDeclaration(UnmanagedType value)
        {
            return new CodeAttributeDeclaration(
                new CodeTypeReference(typeof(MarshalAsAttribute)),
                new CodeAttributeArgument(
                    new CodePropertyReferenceExpression(
                        new CodeTypeReferenceExpression(typeof(UnmanagedType)),
                        value.ToString())));
        }

        public static CodeParameterDeclarationExpression GenerateArgument(this ModuleGenerator generator, CXType functionType, CXCursor paramCursor, uint index)
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

            switch (type.kind)
            {
                case CXTypeKind.CXType_Pointer:
                    var pointee = clang.getPointeeType(type);
                    switch (pointee.kind)
                    {
                        case CXTypeKind.CXType_Pointer:

                            if (pointee.IsPtrToConstChar() && clang.isConstQualifiedType(pointee) != 0)
                            {
                                parameter.Type = new CodeTypeReference(typeof(string[]));
                            }
                            else
                            {
                                parameter.Type = new CodeTypeReference(typeof(IntPtr));
                                parameter.Direction = FieldDirection.Out;
                            }

                            break;

                        case CXTypeKind.CXType_FunctionProto:
                            parameter.Type = new CodeTypeReference(cursorType.ToClrType());
                            break;

                        case CXTypeKind.CXType_Void:
                            parameter.Type = new CodeTypeReference(typeof(IntPtr));
                            break;

                        case CXTypeKind.CXType_Char_S:
                            if (type.IsPtrToConstChar())
                            {
                                parameter.Type = new CodeTypeReference(typeof(string));
                                parameter.CustomAttributes.Add(MarshalAsDeclaration(UnmanagedType.LPStr));
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
                            parameter.Direction = FieldDirection.Ref;
                            parameter.Type = new CodeTypeReference(clrName);
                            break;

                        default:
                            parameter.Type = pointee.ToCodeTypeReference(paramCursor, generator);
                            parameter.Direction = FieldDirection.Ref;
                            break;
                    }

                    break;

                default:
                    parameter.Type = type.ToCodeTypeReference(paramCursor, generator);
                    break;
            }

            return parameter;
        }
    }
}
