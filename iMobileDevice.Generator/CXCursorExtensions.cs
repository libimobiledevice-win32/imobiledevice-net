// <copyright file="CursorExtensions.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.CodeDom;
    using System.Runtime.InteropServices;
    using System.Collections.Generic;
    using Core.Clang;

    internal static class CursorExtensions
    {
        public static bool IsInSystemHeader(this Cursor cursor)
        {
            try
            {
                return cursor.GetLocation().IsInSystemHeader();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static IEnumerable<CodeTypeMember> ToCodeTypeMember(this Cursor cursor, string cursorSpelling, ModuleGenerator generator)
        {
            var canonical = cursor.GetTypeInfo().GetCanonicalType();

            switch (canonical.Kind)
            {
                case TypeKind.ConstantArray:
                    if (canonical.GetArrayElementType().GetCanonicalType().Kind == TypeKind.Char_S)
                    {
                        var size = canonical.GetArraySize();

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

                case TypeKind.Pointer:
                    var pointeeType = canonical.GetPointeeType().GetCanonicalType();

                    CodeMemberField intPtrMember = new CodeMemberField();
                    intPtrMember.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                    intPtrMember.Name = cursorSpelling;
                    intPtrMember.Type = new CodeTypeReference(typeof(IntPtr));
                    yield return intPtrMember;

                    if (pointeeType.Kind == TypeKind.Char_S)
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

                case TypeKind.Enum:
                    var enumField = new CodeMemberField();
                    enumField.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                    enumField.Name = cursorSpelling;
                    enumField.Type = new CodeTypeReference(generator.NameMapping[canonical.GetSpelling()]);
                    yield return enumField;
                    break;

                case TypeKind.Record:
                    var recordField = new CodeMemberField();
                    recordField.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                    recordField.Name = cursorSpelling;
                    recordField.Type = new CodeTypeReference(generator.NameMapping[canonical.GetSpelling()]);
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
