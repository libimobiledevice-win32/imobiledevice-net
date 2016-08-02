using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iMobileDevice.Generator
{
    internal class ErrorExtensionExtractor
    {
        private ModuleGenerator generator;
        private FunctionVisitor visitor;

        public ErrorExtensionExtractor(ModuleGenerator generator, FunctionVisitor visitor)
        {
            if (generator == null)
            {
                throw new ArgumentNullException(nameof(generator));
            }

            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            this.generator = generator;
            this.visitor = visitor;
        }

        public void Generate()
        {
            var errorEnum = this.generator.Types.SingleOrDefault(t => t.Name.EndsWith("Error"));

            if (errorEnum == null)
            {
                return;
            }

            // Add the exception type

            // Generate the {Name}Exception class
            CodeTypeDeclaration exceptionType = new CodeTypeDeclaration();
            exceptionType.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            exceptionType.Name = $"{this.generator.Name}Exception";
            exceptionType.BaseTypes.Add(typeof(Exception));
            exceptionType.Comments.Add(
                new CodeCommentStatement(
                    $"Represents an exception that occurred when interacting with the {this.generator.Name} API.",
                    true));
            exceptionType.CustomAttributes.Add(
                new CodeAttributeDeclaration(
                    new CodeTypeReference(
                        typeof(SerializableAttribute))));

            var defaultConstrutor = new CodeConstructor();
            defaultConstrutor.Attributes = MemberAttributes.Public;
            defaultConstrutor.Comments.Add(
                new CodeCommentStatement(
                    $"<summary>\r\n Initializes a new instance of the <see cref=\"{exceptionType.Name}\"/> class.\r\n </summary>",
                    true));
            exceptionType.Members.Add(defaultConstrutor);

            // Add the constructor which takes an error code
            var errorConstructor = new CodeConstructor();
            errorConstructor.Attributes = MemberAttributes.Public;
            errorConstructor.Comments.Add(
                new CodeCommentStatement(
                    $"<summary>\r\n Initializes a new instance of the <see cref=\"{exceptionType.Name}\"/> class with a specified error code.\r\n <summary>",
                    true));
            errorConstructor.Comments.Add(
                new CodeCommentStatement(
                    $"<param name=\"error\">\r\n The error code of the error that occurred.\r\n </param>",
                    true));
            errorConstructor.BaseConstructorArgs.Add(
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        new CodeTypeReferenceExpression(typeof(string)),
                        "Format"),
                    new CodePrimitiveExpression($"An {this.generator.Name} error occurred. The error code was {{0}}"),
                    new CodeArgumentReferenceExpression("error")));
            errorConstructor.Statements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        "errorCode"),
                    new CodeArgumentReferenceExpression("error")));
            errorConstructor.Parameters.Add(
                new CodeParameterDeclarationExpression(
                    new CodeTypeReference($"{this.generator.Name}Error"),
                    "error"));
            exceptionType.Members.Add(errorConstructor);

            // Add the constructor which takes an error code and an error message
            var errorWithMessageConstructor = new CodeConstructor();
            errorWithMessageConstructor.Attributes = MemberAttributes.Public;
            errorWithMessageConstructor.Comments.Add(
                new CodeCommentStatement(
                    $"<summary>\r\n Initializes a new instance of the <see cref=\"{exceptionType.Name}\"/> class with a specified error code and error message.\r\n <summary>",
                    true));
            errorWithMessageConstructor.Comments.Add(
                new CodeCommentStatement(
                    $"<param name=\"error\">\r\n The error code of the error that occurred.\r\n </param>",
                    true));
            errorWithMessageConstructor.Comments.Add(
                new CodeCommentStatement(
                    $"<param name=\"message\">\r\n A message which describes the error.\r\n </param>",
                    true));
            errorWithMessageConstructor.BaseConstructorArgs.Add(
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        new CodeTypeReferenceExpression(typeof(string)),
                        "Format"),
                    new CodePrimitiveExpression($"An {this.generator.Name} error occurred. {{1}}. The error code was {{0}}"),
                    new CodeArgumentReferenceExpression("error"),
                    new CodeArgumentReferenceExpression("message")));
            errorWithMessageConstructor.Statements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        "errorCode"),
                    new CodeArgumentReferenceExpression("error")));
            errorWithMessageConstructor.Parameters.Add(
                new CodeParameterDeclarationExpression(
                    new CodeTypeReference($"{this.generator.Name}Error"),
                    "error"));
            errorWithMessageConstructor.Parameters.Add(
                new CodeParameterDeclarationExpression(
                    new CodeTypeReference(typeof(string)),
                    "message"));
            exceptionType.Members.Add(errorWithMessageConstructor);

            // Add the Error field
            var errorCodeField = new CodeMemberField();
            errorCodeField.Name = "errorCode";
            errorCodeField.Type = new CodeTypeReference($"{this.generator.Name}Error");
            errorCodeField.Comments.Add(
                new CodeCommentStatement(
                    "<summary>\r\n Backing field for the <see cref=\"ErrorCode\"/> property.\r\n </summary>",
                    true));
            exceptionType.Members.Add(errorCodeField);

            var errorCodeProperty = new CodeMemberProperty();
            errorCodeProperty.Attributes = MemberAttributes.Public;
            errorCodeProperty.Name = "ErrorCode";
            errorCodeProperty.Type = new CodeTypeReference($"{this.generator.Name}Error");
            errorCodeProperty.HasGet = true;
            errorCodeProperty.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        "errorCode")));
            errorCodeProperty.Comments.Add(
                new CodeCommentStatement(
                    "<summary>\r\n Gets the error code that represents the error.\r\n </summary>",
                    true));
            exceptionType.Members.Add(errorCodeProperty);

            var messageConstructor = new CodeConstructor();
            messageConstructor.Attributes = MemberAttributes.Public;
            messageConstructor.Comments.Add(
                new CodeCommentStatement(
                    $"<summary>\r\n Initializes a new instance of the <see cref=\"{exceptionType.Name}\"/> class with a specified error message.\r\n</summary>",
                    true));
            messageConstructor.Comments.Add(
                new CodeCommentStatement(
                    $"<param name=\"message\">\r\n The message that describes the error.\r\n</param>",
                    true));
            messageConstructor.BaseConstructorArgs.Add(
                new CodeArgumentReferenceExpression("message"));
            messageConstructor.Parameters.Add(
                new CodeParameterDeclarationExpression(
                    typeof(string),
                    "message"));
            exceptionType.Members.Add(messageConstructor);

            var messageAndInnerConstructor = new CodeConstructor();
            messageAndInnerConstructor.Attributes = MemberAttributes.Public;
            messageAndInnerConstructor.Comments.Add(
                new CodeCommentStatement(
                    $"<summary>\r\n Initializes a new instance of the <see cref=\"{exceptionType.Name}\"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.\r\n </summary>",
                    true));
            messageAndInnerConstructor.Comments.Add(
                new CodeCommentStatement(
                    $"<param name=\"message\">\r\n The error message that explains the reason for the exception.\r\n </param>",
                    true));
            messageAndInnerConstructor.Comments.Add(
                new CodeCommentStatement(
                    $"<param name=\"inner\">\r\n The exception that is the cause of the current exception, or <see langword=\"null\"/> if no inner exception is specified.\r\n </param>",
                    true));
            messageAndInnerConstructor.BaseConstructorArgs.Add(
                new CodeArgumentReferenceExpression("message"));
            messageAndInnerConstructor.BaseConstructorArgs.Add(
                new CodeArgumentReferenceExpression("inner"));
            messageAndInnerConstructor.Parameters.Add(
                new CodeParameterDeclarationExpression(
                    typeof(string),
                    "message"));
            messageAndInnerConstructor.Parameters.Add(
                new CodeParameterDeclarationExpression(
                    typeof(Exception),
                    "inner"));
            exceptionType.Members.Add(messageAndInnerConstructor);

            var serializedConstructor = new CodeConstructor();
            serializedConstructor.Attributes = MemberAttributes.Family;
            serializedConstructor.Comments.Add(
                new CodeCommentStatement(
                    $"<summary>\r\n Initializes a new instance of the <see cref=\"{exceptionType.Name}\"/> class with serialized data.\r\n </summary>",
                    true));
            serializedConstructor.Comments.Add(
                new CodeCommentStatement(
                    $"<param name=\"info\">\r\n The <see cref=\"System.Runtime.Serialization.SerializationInfo\"/> that holds the serialized object data about the exception being thrown.\r\n </param>",
                    true));
            serializedConstructor.Comments.Add(
                new CodeCommentStatement(
                    $"<param name=\"context\">\r\n The <see cref=\"System.Runtime.Serialization.StreamingContext\"/> that contains contextual information about the source or destination.\r\n </param>",
                    true));
            serializedConstructor.BaseConstructorArgs.Add(
                new CodeArgumentReferenceExpression("info"));
            serializedConstructor.BaseConstructorArgs.Add(
                new CodeArgumentReferenceExpression("context"));
            serializedConstructor.Parameters.Add(
                new CodeParameterDeclarationExpression(
                    typeof(SerializationInfo),
                    "info"));
            serializedConstructor.Parameters.Add(
                new CodeParameterDeclarationExpression(
                    typeof(StreamingContext),
                    "context"));
            exceptionType.Members.Add(serializedConstructor);

            this.generator.AddType(exceptionType.Name, exceptionType);

            CodeTypeDeclaration extensionsType = new CodeTypeDeclaration();
            extensionsType.Name = $"{errorEnum.Name}Extensions";
            extensionsType.Attributes = MemberAttributes.Public | MemberAttributes.Static;

            // Add the ThrowOnError method
            CodeMemberMethod throwOnErrorMethod = new CodeMemberMethod();
            throwOnErrorMethod.Name = "ThrowOnError";
            throwOnErrorMethod.Attributes = MemberAttributes.Static | MemberAttributes.Public;

            var parameter = new CodeParameterDeclarationExpression();
            parameter.Name = "value";
            parameter.Type = new CodeTypeReference("this " + errorEnum.Name);
            throwOnErrorMethod.Parameters.Add(parameter);

            throwOnErrorMethod.Statements.Add(
                new CodeConditionStatement(
                        new CodeBinaryOperatorExpression(
                            new CodeArgumentReferenceExpression("value"),
                            CodeBinaryOperatorType.IdentityInequality,
                            new CodePropertyReferenceExpression(
                                new CodeTypeReferenceExpression(errorEnum.Name),
                                "Success")),
                        new CodeThrowExceptionStatement(
                            new CodeObjectCreateExpression(
                                new CodeTypeReference(exceptionType.Name),
                                new CodeArgumentReferenceExpression("value")))));

            extensionsType.Members.Add(throwOnErrorMethod);

            // Add the ThrowOnError overload which takes an error message 
            CodeMemberMethod throwOnErrorWithMessageMethod = new CodeMemberMethod();
            throwOnErrorWithMessageMethod.Name = "ThrowOnError";
            throwOnErrorWithMessageMethod.Attributes = MemberAttributes.Static | MemberAttributes.Public;

            var throwOnErrorWithMessageMethodParameter = new CodeParameterDeclarationExpression();
            throwOnErrorWithMessageMethodParameter.Name = "value";
            throwOnErrorWithMessageMethodParameter.Type = new CodeTypeReference("this " + errorEnum.Name);
            throwOnErrorWithMessageMethod.Parameters.Add(throwOnErrorWithMessageMethodParameter);

            var messageParameter = new CodeParameterDeclarationExpression();
            messageParameter.Name = "message";
            messageParameter.Type = new CodeTypeReference(typeof(string));
            throwOnErrorWithMessageMethod.Parameters.Add(messageParameter);

            throwOnErrorWithMessageMethod.Statements.Add(
                new CodeConditionStatement(
                        new CodeBinaryOperatorExpression(
                            new CodeArgumentReferenceExpression("value"),
                            CodeBinaryOperatorType.IdentityInequality,
                            new CodePropertyReferenceExpression(
                                new CodeTypeReferenceExpression(errorEnum.Name),
                                "Success")),
                        new CodeThrowExceptionStatement(
                            new CodeObjectCreateExpression(
                                new CodeTypeReference(exceptionType.Name),
                                new CodeArgumentReferenceExpression("value"),
                                new CodeArgumentReferenceExpression("message")))));

            extensionsType.Members.Add(throwOnErrorWithMessageMethod);

            // Add the CheckError method
            CodeMemberMethod isErrorMethod = new CodeMemberMethod();
            isErrorMethod.Name = "IsError";
            isErrorMethod.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            isErrorMethod.ReturnType = new CodeTypeReference(typeof(bool));

            isErrorMethod.Parameters.Add(parameter);

            isErrorMethod.Statements.Add(
                new CodeMethodReturnStatement(
                        new CodeBinaryOperatorExpression(
                            new CodeArgumentReferenceExpression("value"),
                            CodeBinaryOperatorType.IdentityInequality,
                            new CodePropertyReferenceExpression(
                                new CodeTypeReferenceExpression(errorEnum.Name),
                                "Success"))));

            extensionsType.Members.Add(isErrorMethod);

            this.generator.AddType(extensionsType.Name, extensionsType);
        }
    }
}
