// <copyright file="EnumVisitor.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.CodeDom;
    using ClangSharp;

    internal sealed class EnumVisitor
    {
        private readonly ModuleGenerator generator;

        public EnumVisitor(ModuleGenerator generator)
        {
            this.generator = generator;
        }

        public CXChildVisitResult Visit(CXCursor cursor, CXCursor parent, IntPtr data)
        {
            if (clang.Location_isFromMainFile(clang.getCursorLocation(cursor)) == 0)
            {
                return CXChildVisitResult.CXChildVisit_Continue;
            }

            CXCursorKind curKind = clang.getCursorKind(cursor);

            if (curKind == CXCursorKind.CXCursor_EnumDecl)
            {
                var nativeName = clang.getCursorSpelling(cursor).ToString();
                var type = clang.getEnumDeclIntegerType(cursor).ToClrType();
                var enumComment = this.GetComment(cursor, forType: true);

                // enumName can be empty because of typedef enum { .. } enumName;
                // so we have to find the sibling, and this is the only way I've found
                // to do with libclang, maybe there is a better way?
                if (string.IsNullOrEmpty(nativeName))
                {
                    var forwardDeclaringVisitor = new ForwardDeclarationVisitor(cursor);
                    clang.visitChildren(clang.getCursorLexicalParent(cursor), forwardDeclaringVisitor.Visit, new CXClientData(IntPtr.Zero));
                    nativeName = clang.getCursorSpelling(forwardDeclaringVisitor.ForwardDeclarationCursor).ToString();

                    if (string.IsNullOrEmpty(nativeName))
                    {
                        nativeName = "_";
                    }
                }

                var clrName = NameConversions.ToClrName(nativeName, NameConversion.Type);

                // if we've printed these previously, skip them
                if (this.generator.NameMapping.ContainsKey(nativeName))
                {
                    return CXChildVisitResult.CXChildVisit_Continue;
                }

                CodeTypeDeclaration enumDeclaration = new CodeTypeDeclaration();
                enumDeclaration.Attributes = MemberAttributes.Public;
                enumDeclaration.IsEnum = true;
                enumDeclaration.Name = clrName;
                enumDeclaration.BaseTypes.Add(type);

                if (enumComment != null)
                {
                    enumDeclaration.Comments.Add(enumComment);
                }

                // visit all the enum values
                clang.visitChildren(
                    cursor,
                    (cxCursor, vistor, clientData) =>
                    {
                        var field =
                            new CodeMemberField()
                            {
                                Name = NameConversions.ToClrName(clang.getCursorSpelling(cxCursor).ToString(), NameConversion.Field),
                                InitExpression = new CodePrimitiveExpression(clang.getEnumConstantDeclValue(cxCursor))
                            };

                        var fieldComment = this.GetComment(cxCursor, forType: false);
                        if (fieldComment != null)
                        {
                            field.Comments.Add(fieldComment);
                        }

                        enumDeclaration.Members.Add(field);
                        return CXChildVisitResult.CXChildVisit_Continue;
                    },
                    new CXClientData(IntPtr.Zero));

                this.generator.AddType(nativeName, enumDeclaration);
            }

            return CXChildVisitResult.CXChildVisit_Recurse;
        }

        private CodeCommentStatement GetComment(CXCursor cursor, bool forType)
        {
            // Standard hierarchy:
            // - Full Comment
            // - Paragraph Comment
            // - Text Comment
            var fullComment = clang.Cursor_getParsedComment(cursor);
            var fullCommentKind = clang.Comment_getKind(fullComment);
            var fullCommentChildren = clang.Comment_getNumChildren(fullComment);

            if (fullCommentKind != CXCommentKind.CXComment_FullComment || fullCommentChildren != 1)
            {
                return null;
            }

            var paragraphComment = clang.Comment_getChild(fullComment, 0);
            var paragraphCommentKind = clang.Comment_getKind(paragraphComment);
            var paragraphCommentChildren = clang.Comment_getNumChildren(paragraphComment);

            if (paragraphCommentKind != CXCommentKind.CXComment_Paragraph || paragraphCommentChildren != 1)
            {
                return null;
            }

            var textComment = clang.Comment_getChild(paragraphComment, 0);
            var textCommentKind = clang.Comment_getKind(textComment);

            if (textCommentKind != CXCommentKind.CXComment_Text)
            {
                return null;
            }

            var text = clang.TextComment_getText(textComment).ToString();

            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            if (forType)
            {
                text = $"<summary>{Environment.NewLine}{text}{Environment.NewLine}</summary>";
            }

            return new CodeCommentStatement(text, docComment: true);
        }
    }
}
