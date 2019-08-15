// <copyright file="FunctionVisitor.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using Core.Clang;
    using iMobileDevice.Generator.Clang;
    using System;
    using System.CodeDom;
    using System.Runtime.InteropServices;
    using System.Text;

    internal sealed class FunctionVisitor
    {
        private readonly string libraryName;
        private readonly ModuleGenerator generator;
        private CodeTypeDeclaration nativeMethods;

        public FunctionVisitor(ModuleGenerator generator, string libraryName)
        {
            this.generator = generator;
            this.libraryName = libraryName;
        }

        public CodeTypeDeclaration NativeMethods
        {
            get
            {
                return this.nativeMethods;
            }
        }

        public ChildVisitResult Visit(Cursor cursor, Cursor parent)
        {
            if (!cursor.GetLocation().IsFromMainFile())
            {
                return ChildVisitResult.Continue;
            }

            if (this.nativeMethods == null)
            {
                var name = this.generator.Name + "NativeMethods";
                this.nativeMethods = new CodeTypeDeclaration();
                this.nativeMethods.Name = name;
                this.nativeMethods.Attributes = /*MemberAttributes.Static |*/ MemberAttributes.Public | MemberAttributes.Final;
                this.nativeMethods.IsPartial = true;
                this.nativeMethods.Members.Add(
                    new CodeMemberField(typeof(string), "libraryName")
                    {
                        Attributes = MemberAttributes.Const,
                        InitExpression = new CodePrimitiveExpression(this.libraryName)
                    });

                this.generator.Types.Add(new CodeDomGeneratedType(this.nativeMethods));
            }

            CursorKind curKind = cursor.Kind;

            // look only at function decls
            /*
            if (curKind == CursorKind.Cursor_FirstDecl)
            {
                return ChildVisitResult.Recurse;
            }*/

            if (curKind == CursorKind.UnexposedDecl)
            {
                return ChildVisitResult.Recurse;
            }

            if (curKind == CursorKind.FunctionDecl)
            {
                var function = this.WriteFunctionInfoHelper(cursor);
                this.nativeMethods.Members.Add(function);
                return ChildVisitResult.Continue;
            }

            return ChildVisitResult.Continue;
        }

        private CodeMemberMethod WriteFunctionInfoHelper(Cursor cursor)
        {
            var functionType = cursor.GetTypeInfo();
            var nativeName = cursor.GetSpelling();
            var resultType = cursor.GetResultType();

            CodeMemberMethod method = new CodeMemberMethod();
            method.CustomAttributes.Add(this.DllImportAttribute(nativeName, functionType.GetCallingConvention()));

            // These methods really should not be abstract, but extern, but CodeDOM doesn't support it -
            // so we'll do post-processing to fix this (sigh).
            method.Attributes = MemberAttributes.Public | MemberAttributes.Abstract;
            method.ReturnType = resultType.ToCodeTypeReference(cursor, this.generator);
            method.Name = nativeName;

            var functionKind = FunctionType.None;

            if (nativeName.Contains("_new"))
            {
                functionKind = FunctionType.New;
            }
            else if (nativeName.Contains("_free"))
            {
                functionKind = FunctionType.Free;
            }
            else if (nativeName.Contains("pinvoke"))
            {
                functionKind = FunctionType.PInvoke;
            }

            int numArgTypes = functionType.GetNumArgTypes();

            for (uint i = 0; i < numArgTypes; ++i)
            {
                var argument = Argument.GenerateArgument(this.generator, functionType, cursor.GetArgument(i), i, functionKind);
                method.Parameters.Add(argument);
            }

            var comment = this.GetComment(cursor);

            if (comment != null)
            {
                method.Comments.Add(comment);
            }

            return method;
        }

        private CodeAttributeDeclaration DllImportAttribute(string entryPoint, CallingConvention callingConvention)
        {
            return new CodeAttributeDeclaration(
                new CodeTypeReference(typeof(DllImportAttribute)),
                new CodeAttributeArgument(new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(this.nativeMethods.Name), "libraryName")),
                new CodeAttributeArgument(
                    "EntryPoint",
                    new CodePrimitiveExpression(entryPoint)),
                new CodeAttributeArgument(
                    "CallingConvention",
                    new CodePropertyReferenceExpression(
                        new CodeTypeReferenceExpression(typeof(CallingConvention)),
                        callingConvention.ToString())));
        }

        public CodeCommentStatement GetComment(Cursor cursor)
        {
            // Standard hierarchy:
            // - Full Comment
            // - Paragraph Comment or ParamCommand comment
            // - Text Comment
            var fullComment = cursor.GetParsedComment();
            var fullCommentKind = fullComment.Kind;
            var fullCommentChildren = fullComment.ChildrenCount;

            if (fullCommentKind != CommentKind.FullComment || fullCommentChildren < 1)
            {
                return null;
            }

            StringBuilder summary = new StringBuilder();
            StringBuilder parameters = new StringBuilder();
            StringBuilder remarks = new StringBuilder();
            StringBuilder returnValue = new StringBuilder();

            bool hasComment = false;
            bool hasParameter = false;

            for (int i = 0; i < fullCommentChildren; i++)
            {
                var childComment = fullComment.GetChildAt(i);
                var childCommentKind = childComment.Kind;

                if (childCommentKind != CommentKind.Paragraph
                    && childCommentKind != CommentKind.ParamCommand
                    && childCommentKind != CommentKind.BlockCommand)
                {
                    continue;
                }

                StringBuilder textBuilder = new StringBuilder();
                this.GetCommentInnerText(childComment, textBuilder);
                string text = textBuilder.ToString();

                if (string.IsNullOrWhiteSpace(text))
                {
                    continue;
                }

                if (childCommentKind == CommentKind.Paragraph)
                {
                    summary.Append(text);
                    hasComment = true;
                }
                else if (childCommentKind == CommentKind.ParamCommand)
                {
                    // Get the parameter name
                    var paramName = childComment.GetParamName();

                    if (hasParameter)
                    {
                        parameters.AppendLine();
                    }

                    parameters.AppendLine($" <param name=\"{paramName}\">");
                    parameters.Append(text);
                    parameters.Append(" </param>");
                    hasComment = true;
                    hasParameter = true;
                }
                else if (childCommentKind == CommentKind.BlockCommand)
                {
                    var name = childComment.GetCommandName();

                    if (name == "note")
                    {
                        remarks.Append(text);
                    }
                    else if (name == "return")
                    {
                        returnValue.Append(text);
                    }
                }
            }

            if (!hasComment)
            {
                return null;
            }

            StringBuilder comment = new StringBuilder();

            if (summary.Length > 0)
            {
                comment.AppendLine("<summary>");
                comment.Append(summary.ToString());
                comment.Append(" </summary>");
            }

            if (parameters.Length > 0)
            {
                comment.AppendLine();
                comment.Append(parameters.ToString());
            }

            if (returnValue.Length > 0)
            {
                comment.AppendLine();
                comment.AppendLine(" <returns>");
                comment.Append(returnValue.ToString());
                comment.Append(" </returns>");
            }

            if (remarks.Length > 0)
            {
                comment.AppendLine();
                comment.AppendLine(" <remarks>");
                comment.Append(remarks.ToString());
                comment.Append(" </remarks>");
            }

            return new CodeCommentStatement(comment.ToString(), docComment: true);
        }

        private void GetCommentInnerText(Comment comment, StringBuilder builder)
        {
            var commentKind = comment.Kind;

            if (commentKind == CommentKind.Text)
            {
                var text = comment.GetText();
                text = text.Trim();

                if (!string.IsNullOrWhiteSpace(text))
                {
                    builder.Append(" ");
                    builder.AppendLine(text);
                }
            }
            else
            {
                // Recurse
                var childCount = comment.ChildrenCount;

                for (int i = 0; i < childCount; i++)
                {
                    var child = comment.GetChildAt(i);
                    this.GetCommentInnerText(child, builder);
                }
            }
        }
    }
}