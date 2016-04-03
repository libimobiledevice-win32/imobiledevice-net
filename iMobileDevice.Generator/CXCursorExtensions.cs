// <copyright file="CXCursorExtensions.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.CodeDom;
    using System.Runtime.InteropServices;
    using ClangSharp;

    internal static class CXCursorExtensions
    {
        public static bool IsInSystemHeader(this CXCursor cursor)
        {
            return clang.Location_isInSystemHeader(clang.getCursorLocation(cursor)) != 0;
        }

        public static CodeTypeMember ToCodeTypeMember(this CXCursor cursor, string cursorSpelling, ModuleGenerator generator)
        {
            var canonical = clang.getCanonicalType(clang.getCursorType(cursor));

            switch (canonical.kind)
            {
                case CXTypeKind.CXType_ConstantArray:
                    throw new NotImplementedException();

                case CXTypeKind.CXType_Pointer:
                    var pointeeType = clang.getCanonicalType(clang.getPointeeType(canonical));
                    switch (pointeeType.kind)
                    {
                        case CXTypeKind.CXType_Char_S:
                            CodeMemberField member = new CodeMemberField();
                            member.Attributes = MemberAttributes.Public;
                            member.Name = cursorSpelling;
                            member.Type = new CodeTypeReference(typeof(string));
                            member.CustomAttributes.Add(Argument.MarshalAsDeclaration(UnmanagedType.LPStr));
                            return member;

                        case CXTypeKind.CXType_WChar:
                            CodeMemberField wcharMember = new CodeMemberField();
                            wcharMember.Attributes = MemberAttributes.Public;
                            wcharMember.Name = cursorSpelling;
                            wcharMember.Type = new CodeTypeReference(typeof(string));
                            wcharMember.CustomAttributes.Add(Argument.MarshalAsDeclaration(UnmanagedType.LPWStr));
                            return wcharMember;

                        default:
                            CodeMemberField intPtrMember = new CodeMemberField();
                            intPtrMember.Attributes = MemberAttributes.Public;
                            intPtrMember.Attributes = MemberAttributes.Public;
                            intPtrMember.Name = cursorSpelling;
                            intPtrMember.Type = new CodeTypeReference(typeof(IntPtr));
                            return intPtrMember;
                    }

                case CXTypeKind.CXType_Enum:
                    var enumField = new CodeMemberField();
                    enumField.Attributes = MemberAttributes.Public;
                    enumField.Name = cursorSpelling;
                    enumField.Type = new CodeTypeReference(generator.NameMapping[canonical.ToString()]);
                    return enumField;

                case CXTypeKind.CXType_Record:
                    throw new NotSupportedException();

                default:
                    var field = new CodeMemberField();
                    field.Attributes = MemberAttributes.Public;
                    field.Name = cursorSpelling;
                    field.Type = new CodeTypeReference(canonical.ToClrType());
                    return field;
            }
        }
    }
}
