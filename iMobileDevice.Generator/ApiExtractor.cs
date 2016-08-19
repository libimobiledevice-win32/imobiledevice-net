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

            // Create the "Parent" property for the API & the interface,
            CodeMemberProperty parentInterfaceProperty = new CodeMemberProperty();
            parentInterfaceProperty.Comments.Add(new CodeCommentStatement("<summary>", true));
            parentInterfaceProperty.Comments.Add(new CodeCommentStatement($"Gets or sets the <see cref=\"ILibiMobileDeviceApi\"/> which owns this <see cref=\"{this.generator.Name}\"/>.", true));
            parentInterfaceProperty.Comments.Add(new CodeCommentStatement("</summary>", true));
            parentInterfaceProperty.Name = "Parent";
            parentInterfaceProperty.Type = new CodeTypeReference("ILibiMobileDevice");
            parentInterfaceProperty.HasGet = true;
            parentInterfaceProperty.Attributes = MemberAttributes.Final;

            nativeInterface.Members.Add(parentInterfaceProperty);

            CodeMemberField parentField = new CodeMemberField();
            parentField.Comments.Add(new CodeCommentStatement("<summary>", true));
            parentField.Comments.Add(new CodeCommentStatement("Backing field for the <see cref=\"Parent\"/> property", true));
            parentField.Comments.Add(new CodeCommentStatement("</summary>", true));
            parentField.Name = "parent";
            parentField.Type = new CodeTypeReference("ILibiMobileDevice");
            parentField.Attributes |= MemberAttributes.Private | MemberAttributes.Final;
            nativeClass.Members.Add(parentField);

            CodeMemberProperty parentProperty = new CodeMemberProperty();
            parentProperty.Comments.Add(new CodeCommentStatement("<inheritdoc/>", true));
            parentProperty.Name = "Parent";
            parentProperty.Type = new CodeTypeReference("ILibiMobileDevice");
            parentProperty.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        "parent")));
            parentProperty.Attributes |= MemberAttributes.Public;
            nativeClass.Members.Add(parentProperty);

            CodeConstructor constructor = new CodeConstructor();
            constructor.Comments.Add(new CodeCommentStatement("<summary>", true));
            constructor.Comments.Add(new CodeCommentStatement($"Initializes a new instance of the <see cref\"{nativeClass.Name}\"/> class", true));
            constructor.Comments.Add(new CodeCommentStatement("</summary>", true));
            constructor.Comments.Add(new CodeCommentStatement("<param name=\"parent\">", true));
            constructor.Comments.Add(new CodeCommentStatement($"The <see cref=\"ILibiMobileDeviceApi\"/> which owns this <see cref=\"{this.generator.Name}\"/>.", true));
            constructor.Comments.Add(new CodeCommentStatement("</summary>", true));
            constructor.Attributes = MemberAttributes.Public;
            constructor.Parameters.Add(
                new CodeParameterDeclarationExpression(
                    new CodeTypeReference("ILibiMobileDevice"),
                    "parent"));
            constructor.Statements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        "parent"),
                    new CodeArgumentReferenceExpression("parent")));
            nativeClass.Members.Add(constructor);

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
                    // If there are "out" parameters which are safe handles, we should special case.
                    // Otherwise, just call the method and return the result
                    if (!method.Parameters.OfType<CodeParameterDeclarationExpression>().Any(
                        p => p.Direction == FieldDirection.Out
                        && p.Type.BaseType.EndsWith("Handle")))
                    {
                        classMethod.Statements.Add(
                            new CodeMethodReturnStatement(
                                nativeInvocation));
                    }
                    else
                    {
                        // Store the result in a variable and perform the invoke
                        classMethod.Statements.Add(
                            new CodeVariableDeclarationStatement(
                                method.ReturnType,
                                "returnValue"));

                        classMethod.Statements.Add(
                            new CodeAssignStatement(
                                new CodeVariableReferenceExpression("returnValue"),
                                nativeInvocation));

                        // For all "out" parameters which are safehandles, update the .Api property pointing
                        // to this instance of the API - making sure the same API which created the safe handle
                        // will also release it. Useful when mocking multiple APIs in parallel (e.g. Xunit).
                        foreach (var parameter in method.Parameters.OfType<CodeParameterDeclarationExpression>())
                        {
                            if (parameter.Direction == FieldDirection.Out && parameter.Type.BaseType.EndsWith("Handle"))
                            {
                                classMethod.Statements.Add(
                                    new CodeAssignStatement(
                                        new CodePropertyReferenceExpression(
                                            new CodeVariableReferenceExpression(parameter.Name),
                                            "Api"),
                                        new CodePropertyReferenceExpression(
                                            new CodeThisReferenceExpression(),
                                            "Parent")));
                            }
                        }

                        classMethod.Statements.Add(
                            new CodeMethodReturnStatement(
                                new CodeVariableReferenceExpression("returnValue")));
                    }
                }

                nativeInterface.Members.Add(interfaceMethod);
                nativeClass.Members.Add(classMethod);
            }

            this.generator.AddType(nativeInterface.Name, nativeInterface);
            this.generator.AddType(nativeClass.Name, nativeClass);
        }
    }
}
