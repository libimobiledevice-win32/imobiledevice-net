// <copyright file="EnumVisitor.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using ClangSharp.Interop;
    using System;
    using System.CodeDom;

    internal sealed class EnumVisitor
    {
        private readonly ModuleGenerator generator;

        public EnumVisitor(ModuleGenerator generator)
        {
            this.generator = generator;
        }

        public unsafe CXChildVisitResult Visit(CXCursor cursor, CXCursor parent)
        {
            if (!cursor.Location.IsFromMainFile)
            {
                return CXChildVisitResult.CXChildVisit_Continue;
            }

            CXCursorKind curKind = cursor.Kind;

            if (curKind == CXCursorKind.CXCursor_EnumDecl)
            {
                var nativeName = cursor.Spelling.CString;
                var type = cursor.EnumDecl_IntegerType.ToClrType();
                var enumComment = this.GetComment(cursor, forType: true);

                // enumName can be empty because of typedef enum { .. } enumName;
                // so we have to find the sibling, and this is the only way I've found
                // to do with libclang, maybe there is a better way?
                if (string.IsNullOrEmpty(nativeName))
                {
                    var forwardDeclaringVisitor = new ForwardDeclarationVisitor(cursor, skipSystemHeaderCheck: true);
                    cursor.LexicalParent.VisitChildren(forwardDeclaringVisitor.Visit, new CXClientData());
                    nativeName = forwardDeclaringVisitor.ForwardDeclarationCXCursor.Spelling.CString;

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
                DelegatingCXCursorVisitor visitor = new DelegatingCXCursorVisitor(
                    (c, vistor) =>
                    {
                        var field =
                            new CodeMemberField()
                            {
                                Name = NameConversions.ToClrName(c.Spelling.CString, NameConversion.Field),
                                InitExpression = new CodePrimitiveExpression(c.EnumConstantDeclValue)
                            };

                        var fieldComment = this.GetComment(c, forType: true);
                        if (fieldComment != null)
                        {
                            field.Comments.Add(fieldComment);
                        }

                        enumDeclaration.Members.Add(field);
                        return CXChildVisitResult.CXChildVisit_Continue;
                    });
                cursor.VisitChildren(visitor.Visit, new CXClientData());

                this.generator.AddType(nativeName, new CodeDomGeneratedType(enumDeclaration));
            }

            return CXChildVisitResult.CXChildVisit_Recurse;
        }

        private CodeCommentStatement GetComment(CXCursor cursor, bool forType)
        {
            // Standard hierarchy:
            // - Full Comment
            // - Paragraph Comment
            // - Text Comment
            var fullComment = cursor.ParsedComment;
            var fullCommentKind = fullComment.Kind;
            var fullCommentChildren = fullComment.NumChildren;

            if (fullCommentKind != CXCommentKind.CXComment_FullComment || fullCommentChildren != 1)
            {
                return null;
            }

            var paragraphComment = fullComment.GetChild(0);
            var paragraphCommentKind = paragraphComment.Kind;
            var paragraphCommentChildren = paragraphComment.NumChildren;

            if (paragraphCommentKind != CXCommentKind.CXComment_Paragraph || paragraphCommentChildren != 1)
            {
                return null;
            }

            var textComment = paragraphComment.GetChild(0);
            var textCommentKind = textComment.Kind;

            if (textCommentKind != CXCommentKind.CXComment_Text)
            {
                return null;
            }

            var text = textComment.TextComment_Text.CString;

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
