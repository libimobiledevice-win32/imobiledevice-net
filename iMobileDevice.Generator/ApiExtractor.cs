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
            nativeInterface.IsPartial = true;
            nativeInterface.Attributes |= MemberAttributes.Public;

            // Create the {Name}Api class
            CodeTypeDeclaration nativeClass = new CodeTypeDeclaration();
            nativeClass.Name = $"{this.generator.Name}Api";
            nativeClass.BaseTypes.Add(nativeInterface.Name);
            nativeClass.IsPartial = true;
            nativeClass.Attributes |= MemberAttributes.Public;

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
        }
    }
}
