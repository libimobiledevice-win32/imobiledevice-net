// <copyright file="CXCursorExtensions.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using ClangSharp.Interop;
    using System;
    using System.CodeDom;
    using System.Collections.Generic;

    internal static class CXCursorExtensions
    {
        public static bool IsInSystemHeader(this CXCursor cursor)
        {
            try
            {
                return cursor.Location.IsInSystemHeader;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static IEnumerable<CodeTypeMember> ToCodeTypeMember(this CXCursor cursor, string cursorSpelling, ModuleGenerator generator)
        {
            var canonical = cursor.Type.CanonicalType.CanonicalType;

            switch (canonical.kind)
            {
                case CXTypeKind.CXType_ConstantArray:
                    if (canonical.ArrayElementType.CanonicalType.kind == CXTypeKind.CXType_Char_S)
                    {
                        var size = canonical.ArraySize;

                        CodeMemberField fixedLengthString = new CodeMemberField();
                        fixedLengthString.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                        fixedLengthString.Name = cursorSpelling;
                        fixedLengthString.Type = new CodeTypeReference(typeof(string));
                        fixedLengthString.CustomAttributes.Add(Argument.MarshalAsFixedLengthStringDeclaration((int)size));
                        yield return fixedLengthString;
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    break;

                case CXTypeKind.CXType_Pointer:
                    var pointeeType = canonical.PointeeType.CanonicalType;

                    CodeMemberField intPtrMember = new CodeMemberField();
                    intPtrMember.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                    intPtrMember.Name = cursorSpelling;
                    intPtrMember.Type = new CodeTypeReference(typeof(IntPtr));
                    yield return intPtrMember;

                    if (pointeeType.kind == CXTypeKind.CXType_Char_S)
                    {
                        CodeMemberProperty stringMember = new CodeMemberProperty();
                        stringMember.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                        stringMember.Name = cursorSpelling + "String";
                        stringMember.Type = new CodeTypeReference(typeof(string));

                        stringMember.HasGet = true;
                        stringMember.GetStatements.Add(
                            new CodeMethodReturnStatement(
                                new CodeMethodInvokeExpression(
                                    new CodeMethodReferenceExpression(
                                        new CodeTypeReferenceExpression("Utf8Marshal"),
                                        "PtrToStringUtf8"),
                                    new CodeFieldReferenceExpression(
                                        new CodeThisReferenceExpression(),
                                        intPtrMember.Name))));

                        yield return stringMember;
                    }

                    break;

                case CXTypeKind.CXType_Enum:
                    var enumField = new CodeMemberField();
                    enumField.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                    enumField.Name = cursorSpelling;
                    enumField.Type = new CodeTypeReference(generator.NameMapping[canonical.Spelling.CString]);
                    yield return enumField;
                    break;

                case CXTypeKind.CXType_Record:
                    var recordField = new CodeMemberField();
                    recordField.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                    recordField.Name = cursorSpelling;
                    recordField.Type = new CodeTypeReference(generator.NameMapping[canonical.Spelling.CString]);
                    yield return recordField;
                    break;

                default:
                    var field = new CodeMemberField();
                    field.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                    field.Name = cursorSpelling;
                    field.Type = new CodeTypeReference(canonical.ToClrType());
                    yield return field;
                    break;
            }
        }
    }
}
