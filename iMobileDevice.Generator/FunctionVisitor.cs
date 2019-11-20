// <copyright file="FunctionVisitor.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using ClangSharp.Interop;
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

        public CXChildVisitResult Visit(CXCursor cursor, CXCursor parent)
        {
            if (!cursor.Location.IsFromMainFile)
            {
                return CXChildVisitResult.CXChildVisit_Continue;
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

            CXCursorKind curKind = cursor.Kind;

            // look only at function decls
            /*
            if (curKind == CXCursorKind.CXCursor_FirstDecl)
            {
                return CXChildVisitResult.CXChildVisit_Recurse;
            }*/

            if (curKind == CXCursorKind.CXCursor_UnexposedDecl)
            {
                return CXChildVisitResult.CXChildVisit_Recurse;
            }

            if (curKind == CXCursorKind.CXCursor_FunctionDecl)
            {
                var function = this.WriteFunctionInfoHelper(cursor);
                this.nativeMethods.Members.Add(function);
                return CXChildVisitResult.CXChildVisit_Continue;
            }

            return CXChildVisitResult.CXChildVisit_Continue;
        }

        private CodeMemberMethod WriteFunctionInfoHelper(CXCursor cursor)
        {
            var functionType = cursor.Type;
            var nativeName = cursor.Spelling.CString;
            var resultType = cursor.ResultType;

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

            int numArgTypes = functionType.NumArgTypes;

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

        public CodeCommentStatement GetComment(CXCursor cursor)
        {
            // Standard hierarchy:
            // - Full Comment
            // - Paragraph Comment or ParamCommand comment
            // - Text Comment
            var fullComment = cursor.ParsedComment;
            var fullCommentKind = fullComment.Kind;
            var fullCommentChildren = fullComment.NumChildren;

            if (fullCommentKind != CXCommentKind.CXComment_FullComment || fullCommentChildren < 1)
            {
                return null;
            }

            StringBuilder summary = new StringBuilder();
            StringBuilder parameters = new StringBuilder();
            StringBuilder remarks = new StringBuilder();
            StringBuilder returnValue = new StringBuilder();

            bool hasComment = false;
            bool hasParameter = false;

            for (uint i = 0; i < fullCommentChildren; i++)
            {
                var childComment = fullComment.GetChild(i);
                var childCommentKind = childComment.Kind;

                if (childCommentKind != CXCommentKind.CXComment_Paragraph
                    && childCommentKind != CXCommentKind.CXComment_ParamCommand
                    && childCommentKind != CXCommentKind.CXComment_BlockCommand)
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

                if (childCommentKind == CXCommentKind.CXComment_Paragraph)
                {
                    summary.Append(text);
                    hasComment = true;
                }
                else if (childCommentKind == CXCommentKind.CXComment_ParamCommand)
                {
                    // Get the parameter name
                    var paramName = childComment.ParamCommandComment_ParamName.CString;

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
                else if (childCommentKind == CXCommentKind.CXComment_BlockCommand)
                {
                    var name = childComment.BlockCommandComment_CommandName.CString;

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

        private void GetCommentInnerText(CXComment comment, StringBuilder builder)
        {
            var commentKind = comment.Kind;

            if (commentKind == CXCommentKind.CXComment_Text)
            {
                var text = comment.TextComment_Text.CString;
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
                var childCount = comment.NumChildren;

                for (uint i = 0; i < childCount; i++)
                {
                    var child = comment.GetChild(i);
                    this.GetCommentInnerText(child, builder);
                }
            }
        }
    }
}