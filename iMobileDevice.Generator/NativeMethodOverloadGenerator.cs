namespace iMobileDevice.Generator
{
    using System;
    using System.CodeDom;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    internal static class NativeMethodOverloadGenerator
    {
        public static void Generate(ModuleGenerator generator)
        {
            var nativeMethods = generator.Types.Single(t => t.Name.EndsWith("NativeMethods"));

            CodeTypeDeclaration overloads = new CodeTypeDeclaration();
            generator.Types.Add(overloads);
            overloads.UserData.Add("FileNameSuffix", ".Extensions");
            overloads.Name = nativeMethods.Name;
            overloads.IsPartial = true;
            nativeMethods.IsPartial = true;

            overloads.Name = nativeMethods.Name;

            foreach (var method in nativeMethods.Members.OfType<CodeMemberMethod>())
            {
                bool needsPatching = method
                    .Parameters
                    .OfType<CodeParameterDeclarationExpression>()
                    .Any(p => HasCustomMarshaler(p));

                if (!needsPatching)
                {
                    continue;
                }

                CodeMemberMethod customMethod = new CodeMemberMethod();
                overloads.Members.Add(customMethod);
                customMethod.Attributes = MemberAttributes.Public | MemberAttributes.Static;
                customMethod.Name = method.Name;
                customMethod.ReturnType = method.ReturnType;

                bool hasReturnValue = customMethod.ReturnType != null
                    && customMethod.ReturnType.BaseType != "System.Void";

                var invokeStatement = new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        new CodeTypeReferenceExpression(nativeMethods.Name),
                        method.Name));

                if (!hasReturnValue)
                {
                    customMethod.Statements.Add(invokeStatement);
                }
                else
                {
                    var resultStatement = new CodeVariableDeclarationStatement(
                        customMethod.ReturnType,
                        "returnValue",
                        invokeStatement);

                    customMethod.Statements.Add(resultStatement);
                }

                foreach (CodeParameterDeclarationExpression parameter in method.Parameters)
                {
                    var customParameter =
                        new CodeParameterDeclarationExpression(
                            parameter.Type,
                            parameter.Name);
                    customParameter.Direction = parameter.Direction;

                    customMethod.Parameters.Add(customParameter);

                    var marshalerAttribute = GetCustomMarshaler(parameter);

                    if (marshalerAttribute != null)
                    {
                        Debug.Assert(marshalerAttribute.Name.EndsWith("MarshalAsAttribute"), "Marshaler");
                        var marshaler = ((CodeTypeOfExpression)marshalerAttribute.Arguments[1].Value).Type;

                        parameter.CustomAttributes.Clear();
                        bool marshalInput = parameter.Direction == FieldDirection.In || parameter.Direction == FieldDirection.Ref;
                        bool marshalOutput = parameter.Direction == FieldDirection.Out || parameter.Direction == FieldDirection.Ref;

                        // Create the marshaler
                        // ICustomMarshaler [paramName]marshaler = [Marshaler].GetInstance(null);
                        customMethod.Statements.Insert(0,
                            new CodeVariableDeclarationStatement(
                                new CodeTypeReference(typeof(ICustomMarshaler)),
                                parameter.Name + "Marshaler",
                                new CodeMethodInvokeExpression(
                                    new CodeMethodReferenceExpression(
                                        new CodeTypeReferenceExpression(marshaler),
                                        "GetInstance"),
                                    new CodePrimitiveExpression(null))));

                        // If the variable is [in] or [ref],
                        // marshal the type from managed to unmanaged,
                        // else, initialize to IntPtr.Zero
                        if (marshalInput)
                        {
                            customMethod.Statements.Insert(1,
                                new CodeVariableDeclarationStatement(
                                    new CodeTypeReference(typeof(IntPtr)),
                                    parameter.Name + "Native",
                                    new CodeMethodInvokeExpression(
                                        new CodeMethodReferenceExpression(
                                            new CodeVariableReferenceExpression(parameter.Name + "Marshaler"),
                                            "MarshalManagedToNative"),
                                        new CodeArgumentReferenceExpression(
                                            parameter.Name))));
                        }
                        else
                        {
                            customMethod.Statements.Insert(1,
                                new CodeVariableDeclarationStatement(
                                    new CodeTypeReference(typeof(IntPtr)),
                                    parameter.Name + "Native",
                                    new CodePropertyReferenceExpression(
                                        new CodeTypeReferenceExpression(typeof(IntPtr)),
                                        "Zero")));
                        }

                        // Invoke the method with the base type
                        invokeStatement.Parameters.Add(
                            new CodeDirectionExpression(
                                parameter.Direction,
                                new CodeVariableReferenceExpression(parameter.Name + "Native")));

                        // Convert from unmanaged to managed, if required
                        if (marshalOutput)
                        {
                            customMethod.Statements.Add(
                                new CodeAssignStatement(
                                    new CodeArgumentReferenceExpression(parameter.Name),
                                    new CodeCastExpression(
                                        parameter.Type,
                                        new CodeMethodInvokeExpression(
                                            new CodeMethodReferenceExpression(
                                                new CodeVariableReferenceExpression(parameter.Name + "Marshaler"),
                                                "MarshalNativeToManaged"),
                                            new CodeArgumentReferenceExpression(
                                                parameter.Name + "Native")))));

                            customMethod.Statements.Add(
                                new CodeMethodInvokeExpression(
                                    new CodeMethodReferenceExpression(
                                        new CodeVariableReferenceExpression(parameter.Name + "Marshaler"),
                                        "CleanUpNativeData"),
                                    new CodeArgumentReferenceExpression(
                                        parameter.Name + "Native")));
                        }

                        // Set the unmanaged type to the primitive
                        parameter.Type = new CodeTypeReference(typeof(IntPtr));
                    }
                    else
                    {
                        invokeStatement.Parameters.Add(
                            new CodeDirectionExpression(
                                parameter.Direction,
                                new CodeArgumentReferenceExpression(parameter.Name)));
                    }
                }

                if (hasReturnValue)
                {
                    customMethod.Statements.Add(
                        new CodeMethodReturnStatement(
                            new CodeVariableReferenceExpression("returnValue")));
                }
            }
        }

        private static bool HasCustomMarshaler(CodeParameterDeclarationExpression parameter)
        {
            return GetCustomMarshaler(parameter) != null;
        }

        private static CodeAttributeDeclaration GetCustomMarshaler(CodeParameterDeclarationExpression parameter)
        {
            var customAttributes = parameter.CustomAttributes.OfType<CodeAttributeDeclaration>();
            var marshalerAttribute = customAttributes.SingleOrDefault();

            if (marshalerAttribute != null && marshalerAttribute.Arguments.Count == 2)
            {
                return marshalerAttribute;
            }
            else
            {
                return null;
            }
        }
    }
}
