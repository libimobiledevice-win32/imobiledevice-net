using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
            var errorEnum = this.generator.Types.Single(t => t.Name.EndsWith("Error"));
            var excepionType = this.generator.Types.Single(t => t.Name.EndsWith("Exception"));

            CodeTypeDeclaration extensionsType = new CodeTypeDeclaration();
            extensionsType.Name = $"{errorEnum.Name}Extensions";
            extensionsType.Attributes = MemberAttributes.Public;

            // Add the checkError method
            CodeMemberMethod checkErrorMethod = new CodeMemberMethod();
            checkErrorMethod.Name = "ThrowOnError";
            checkErrorMethod.Attributes = MemberAttributes.Static | MemberAttributes.Public;

            var parameter = new CodeParameterDeclarationExpression();
            parameter.Name = "value";
            parameter.Type = new CodeTypeReference("this " + errorEnum.Name);
            checkErrorMethod.Parameters.Add(parameter);

            checkErrorMethod.Statements.Add(
                new CodeConditionStatement(
                        new CodeBinaryOperatorExpression(
                            new CodeArgumentReferenceExpression("value"),
                            CodeBinaryOperatorType.IdentityInequality,
                            new CodePropertyReferenceExpression(
                                new CodeTypeReferenceExpression(errorEnum.Name),
                                "Success")),
                        new CodeThrowExceptionStatement(
                            new CodeObjectCreateExpression(
                                new CodeTypeReference(excepionType.Name),
                                new CodeArgumentReferenceExpression("value")))));


            extensionsType.Members.Add(checkErrorMethod);

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
