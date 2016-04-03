using System;
using System.CodeDom;
using System.Linq;
using System.Runtime.Serialization;

namespace iMobileDevice.Generator
{
    internal class ApiExtractor
    {
        private ModuleGenerator generator;
        private FunctionVisitor visitor;

        public ApiExtractor(ModuleGenerator generator, FunctionVisitor visitor)
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
            if (this.visitor.NativeMethods == null)
            {
                return;
            }

            // Create the I{Name}Api interface
            CodeTypeDeclaration nativeInterface = new CodeTypeDeclaration();
            nativeInterface.Name = $"I{this.generator.Name}Api";
            nativeInterface.IsInterface = true;

            // Create the {Name}Api class
            CodeTypeDeclaration nativeClass = new CodeTypeDeclaration();
            nativeClass.Name = $"{this.generator.Name}Api";
            nativeClass.BaseTypes.Add(nativeInterface.Name);

            foreach (var method in this.visitor.NativeMethods.Members.OfType<CodeMemberMethod>())
            {
                var interfaceMethod = new CodeMemberMethod();
                interfaceMethod.Name = method.Name;
                interfaceMethod.ReturnType = method.ReturnType;
                interfaceMethod.Comments.AddRange(method.Comments);

                var classMethod = new CodeMemberMethod();
                classMethod.Name = method.Name;
                classMethod.ReturnType = method.ReturnType;
                classMethod.Comments.AddRange(method.Comments);
                classMethod.Attributes = MemberAttributes.Public;

                var nativeInvocation = new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        new CodeTypeReferenceExpression(this.visitor.NativeMethods.Name),
                        method.Name));

                foreach (var parameter in method.Parameters.OfType<CodeParameterDeclarationExpression>())
                {
                    var interfaceParameter = new CodeParameterDeclarationExpression();
                    interfaceParameter.Name = parameter.Name;
                    interfaceParameter.Direction = parameter.Direction;
                    interfaceParameter.Type = parameter.Type;

                    interfaceMethod.Parameters.Add(interfaceParameter);

                    var classParameter = new CodeParameterDeclarationExpression();
                    classParameter.Name = parameter.Name;
                    classParameter.Direction = parameter.Direction;
                    classParameter.Type = parameter.Type;

                    classMethod.Parameters.Add(classParameter);

                    var argumentRef = new CodeArgumentReferenceExpression(parameter.Name);
                    nativeInvocation.Parameters.Add(new CodeDirectionExpression(parameter.Direction, argumentRef));
                }

                if (method.ReturnType == null || method.ReturnType.BaseType == "System.Void")
                {
                    classMethod.Statements.Add(nativeInvocation);
                }
                else
                {
                    classMethod.Statements.Add(new CodeMethodReturnStatement(nativeInvocation));
                }

                nativeInterface.Members.Add(interfaceMethod);
                nativeClass.Members.Add(classMethod);
            }

            this.generator.AddType(nativeInterface.Name, nativeInterface);
            this.generator.AddType(nativeClass.Name, nativeClass);

            // Generate the {Name}Exception class
            CodeTypeDeclaration exception = new CodeTypeDeclaration();
            exception.Name = $"{this.generator.Name}Exception";
            exception.BaseTypes.Add(typeof(Exception));
            exception.Comments.Add(
                new CodeCommentStatement(
                    $"Represents an exception that occurred when interacting with the {this.generator.Name} API.",
                    true));
            exception.CustomAttributes.Add(
                new CodeAttributeDeclaration(
                    new CodeTypeReference(
                        typeof(SerializableAttribute))));

            var defaultConstrutor = new CodeConstructor();
            defaultConstrutor.Attributes = MemberAttributes.Public;
            defaultConstrutor.Comments.Add(
                new CodeCommentStatement(
                    $"<summary>\r\n Initializes a new instance of the <see cref=\"{exception.Name}\"/> class.\r\n </summary>",
                    true));
            exception.Members.Add(defaultConstrutor);

            var errorConstructor = new CodeConstructor();
            errorConstructor.Attributes = MemberAttributes.Public;
            errorConstructor.Comments.Add(
                new CodeCommentStatement(
                    $"<summary>\r\n Initializes a new instance of the <see cref=\"{exception.Name}\"/> class with a specified error code.\r\n <summary>",
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
                    new CodePrimitiveExpression($"An {this.generator.Name} error occurred. The error code was {0}"),
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
            exception.Members.Add(errorConstructor);

            var errorCodeField = new CodeMemberField();
            errorCodeField.Name = "errorCode";
            errorCodeField.Type = new CodeTypeReference($"{this.generator.Name}Error");
            errorCodeField.Comments.Add(
                new CodeCommentStatement(
                    "<summary>\r\n Backing field for the <see cref=\"ErrorCode\"/> property.\r\n </summary>",
                    true));
            exception.Members.Add(errorCodeField);

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
            exception.Members.Add(errorCodeProperty);

            var messageConstructor = new CodeConstructor();
            messageConstructor.Attributes = MemberAttributes.Public;
            messageConstructor.Comments.Add(
                new CodeCommentStatement(
                    $"<summary>\r\n Initializes a new instance of the <see cref=\"{exception.Name}\"/> class with a specified error message.\r\n</summary>",
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
            exception.Members.Add(messageConstructor);

            var messageAndInnerConstructor = new CodeConstructor();
            messageAndInnerConstructor.Attributes = MemberAttributes.Public;
            messageAndInnerConstructor.Comments.Add(
                new CodeCommentStatement(
                    $"<summary>\r\n Initializes a new instance of the <see cref=\"{exception.Name}\"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.\r\n </summary>",
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
            exception.Members.Add(messageAndInnerConstructor);

            var serializedConstructor = new CodeConstructor();
            serializedConstructor.Attributes = MemberAttributes.Family;
            serializedConstructor.Comments.Add(
                new CodeCommentStatement(
                    $"<summary>\r\n Initializes a new instance of the <see cref=\"{exception.Name}\"/> class with serialized data.\r\n </summary>",
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
            exception.Members.Add(serializedConstructor);

            this.generator.AddType(exception.Name, exception);
        }
    }
}
