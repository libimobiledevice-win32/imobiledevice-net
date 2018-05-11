// <copyright file="EnumVisitor.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.CodeDom;
    using Core.Clang;
    using iMobileDevice.Generator.Polyfill;

    internal sealed class EnumVisitor
    {
        private readonly ModuleGenerator generator;

        public EnumVisitor(ModuleGenerator generator)
        {
            this.generator = generator;
        }

        public ChildVisitResult Visit(Cursor cursor, Cursor parent)
        {
            if (!cursor.GetLocation().IsFromMainFile())
            {
                return ChildVisitResult.Continue;
            }

            CursorKind curKind = cursor.Kind;

            if (curKind == CursorKind.EnumDecl)
            {
                var nativeName = cursor.GetSpelling();
                var type = cursor.GetEnumDeclIntegerType().ToClrType();
                var enumComment = this.GetComment(cursor, forType: true);

                // enumName can be empty because of typedef enum { .. } enumName;
                // so we have to find the sibling, and this is the only way I've found
                // to do with libclang, maybe there is a better way?
                if (string.IsNullOrEmpty(nativeName))
                {
                    var forwardDeclaringVisitor = new ForwardDeclarationVisitor(cursor, skipSystemHeaderCheck: true);
                    forwardDeclaringVisitor.VisitChildren(cursor.GetLexicalParent());
                    nativeName = forwardDeclaringVisitor.ForwardDeclarationCursor.GetSpelling();

                    if (string.IsNullOrEmpty(nativeName))
                    {
                        nativeName = "_";
                    }
                }

                var clrName = NameConversions.ToClrName(nativeName, NameConversion.Type);

                // if we've printed these previously, skip them
                if (this.generator.NameMapping.ContainsKey(nativeName))
                {
                    return ChildVisitResult.Continue;
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
                DelegatingCursorVisitor visitor = new DelegatingCursorVisitor(
                    (c, vistor) =>
                    {
                        var field =
                            new CodeMemberField()
                            {
                                Name = NameConversions.ToClrName(c.GetSpelling(), NameConversion.Field),
                                InitExpression = new CodePrimitiveExpression(c.GetEnumConstantDeclValue())
                            };

                        var fieldComment = this.GetComment(c, forType: false);
                        if (fieldComment != null)
                        {
                            field.Comments.Add(fieldComment);
                        }

                        enumDeclaration.Members.Add(field);
                        return ChildVisitResult.Continue;
                    });
                visitor.VisitChildren(cursor);

                this.generator.AddType(nativeName, enumDeclaration);
            }

            return ChildVisitResult.Recurse;
        }

        private CodeCommentStatement GetComment(Cursor cursor, bool forType)
        {
            // Standard hierarchy:
            // - Full Comment
            // - Paragraph Comment
            // - Text Comment
            var fullComment = Polyfill.NativeMethods.Cursor_getParsedComment(cursor.ToCXCursor());
            var fullCommentKind = Polyfill.NativeMethods.Comment_getKind(fullComment);
            var fullCommentChildren = Polyfill.NativeMethods.Comment_getNumChildren(fullComment);

            if (fullCommentKind != Polyfill.CommentKind.FullComment || fullCommentChildren != 1)
            {
                return null;
            }

            var paragraphComment = Polyfill.NativeMethods.Comment_getChild(fullComment, 0);
            var paragraphCommentKind = Polyfill.NativeMethods.Comment_getKind(paragraphComment);
            var paragraphCommentChildren = Polyfill.NativeMethods.Comment_getNumChildren(paragraphComment);

            if (paragraphCommentKind != Polyfill.CommentKind.Paragraph || paragraphCommentChildren != 1)
            {
                return null;
            }

            var textComment = Polyfill.NativeMethods.Comment_getChild(paragraphComment, 0);
            var textCommentKind = Polyfill.NativeMethods.Comment_getKind(textComment);

            if (textCommentKind != Polyfill.CommentKind.Text)
            {
                return null;
            }

            var text = Polyfill.NativeMethods.TextComment_getText(textComment).ToString();

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
