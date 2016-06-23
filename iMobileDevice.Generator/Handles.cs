// <copyright file="Handles.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System.CodeDom;
    using System.Runtime.ConstrainedExecution;
    using System.Security.Permissions;
    using Microsoft.Win32.SafeHandles;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    internal static class Handles
    {
        public static CodeAttributeDeclaration SecurityPermissionDeclaration(SecurityAction action, bool unmanagedCode)
        {
            return new CodeAttributeDeclaration(
                new CodeTypeReference(typeof(SecurityPermissionAttribute)),
                new CodeAttributeArgument(
                    new CodePropertyReferenceExpression(
                        new CodeTypeReferenceExpression(typeof(SecurityAction)),
                        action.ToString())),
                new CodeAttributeArgument(
                    "UnmanagedCode", new CodePrimitiveExpression(unmanagedCode)));
        }

        public static CodeAttributeDeclaration ReliabilityContractDeclaration(Consistency consistency, Cer cer)
        {
            return new CodeAttributeDeclaration(
                new CodeTypeReference(typeof(ReliabilityContractAttribute)),
                new CodeAttributeArgument(
                    new CodePropertyReferenceExpression(
                        new CodeTypeReferenceExpression(typeof(Consistency)),
                        consistency.ToString())),
                new CodeAttributeArgument(
                    new CodePropertyReferenceExpression(
                        new CodeTypeReferenceExpression(typeof(Cer)),
                        cer.ToString())));
        }

        public static IEnumerable<CodeTypeDeclaration> CreateSafeHandle(string name, ModuleGenerator generator)
        {
            CodeTypeDeclaration safeHandle = new CodeTypeDeclaration(name + "Handle");

            safeHandle.CustomAttributes.Add(SecurityPermissionDeclaration(SecurityAction.InheritanceDemand, true));
            safeHandle.CustomAttributes.Add(SecurityPermissionDeclaration(SecurityAction.Demand, true));
            safeHandle.IsPartial = true;
            safeHandle.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            safeHandle.BaseTypes.Add(typeof(SafeHandleZeroOrMinusOneIsInvalid));

            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Family;
            constructor.BaseConstructorArgs.Add(new CodePrimitiveExpression(true));
            safeHandle.Members.Add(constructor);

            CodeConstructor ownsHandleConstructor = new CodeConstructor();
            ownsHandleConstructor.Attributes = MemberAttributes.Family;
            ownsHandleConstructor.Parameters.Add(new CodeParameterDeclarationExpression(new CodeTypeReference(typeof(bool)), "ownsHandle"));
            ownsHandleConstructor.BaseConstructorArgs.Add(new CodeArgumentReferenceExpression("ownsHandle"));
            safeHandle.Members.Add(ownsHandleConstructor);

            CodeMemberMethod releaseHandle = new CodeMemberMethod();
            releaseHandle.Name = "ReleaseHandle";
            releaseHandle.Attributes = MemberAttributes.Override | MemberAttributes.Family;
            releaseHandle.ReturnType = new CodeTypeReference(typeof(bool));
            releaseHandle.CustomAttributes.Add(ReliabilityContractDeclaration(Consistency.WillNotCorruptState, Cer.MayFail));

            releaseHandle.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(true)));
            safeHandle.Members.Add(releaseHandle);

            // Add an "Api" property which provides a reference to the instance of the API which was used to
            // create this handle - and which will also be used to destroy this handle.
            CodeMemberField apiField = new CodeMemberField();
            apiField.Name = "api";
            apiField.Attributes = MemberAttributes.Private | MemberAttributes.Final;
            apiField.Type = new CodeTypeReference("ILibiMobileDevice");
            safeHandle.Members.Add(apiField);

            CodeMemberProperty apiProperty = new CodeMemberProperty();
            apiProperty.Name = "Api";
            apiProperty.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            apiProperty.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        apiField.Name)));
            apiProperty.SetStatements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodeThisReferenceExpression(),
                        apiField.Name),
                    new CodeVariableReferenceExpression("value")));
            apiProperty.Type = new CodeTypeReference("ILibiMobileDevice");
            safeHandle.Members.Add(apiProperty);

            // Add a "DangeousCreate" method which creates a new safe handle from an IntPtr
            CodeMemberMethod dangerousCreate = new CodeMemberMethod();
            dangerousCreate.Name = "DangerousCreate";
            dangerousCreate.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            dangerousCreate.ReturnType = new CodeTypeReference(safeHandle.Name);

            dangerousCreate.Parameters.Add(
                new CodeParameterDeclarationExpression(
                    new CodeTypeReference(typeof(IntPtr)),
                    "unsafeHandle"));

            dangerousCreate.Parameters.Add(
                new CodeParameterDeclarationExpression(
                    new CodeTypeReference(typeof(bool)),
                    "ownsHandle"));

            dangerousCreate.Statements.Add(
                new CodeVariableDeclarationStatement(
                    new CodeTypeReference(safeHandle.Name),
                    "safeHandle"));

            dangerousCreate.Statements.Add(
                new CodeAssignStatement(
                    new CodeVariableReferenceExpression("safeHandle"),
                    new CodeObjectCreateExpression(
                        new CodeTypeReference(safeHandle.Name),
                        new CodeArgumentReferenceExpression("ownsHandle"))));

            dangerousCreate.Statements.Add(
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        new CodeVariableReferenceExpression("safeHandle"),
                        "SetHandle"),
                    new CodeArgumentReferenceExpression("unsafeHandle")));

            dangerousCreate.Statements.Add(
                new CodeMethodReturnStatement(
                    new CodeVariableReferenceExpression("safeHandle")));

            safeHandle.Members.Add(dangerousCreate);

            // Add a "DangeousCreate" method which creates a new safe handle from an IntPtr
            CodeMemberMethod simpleDangerousCreate = new CodeMemberMethod();
            simpleDangerousCreate.Name = "DangerousCreate";
            simpleDangerousCreate.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            simpleDangerousCreate.ReturnType = new CodeTypeReference(safeHandle.Name);

            simpleDangerousCreate.Parameters.Add(
                new CodeParameterDeclarationExpression(
                    new CodeTypeReference(typeof(IntPtr)),
                    "unsafeHandle"));

            simpleDangerousCreate.Statements.Add(
                new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                            new CodeTypeReferenceExpression(safeHandle.Name),
                            "DangerousCreate"),
                        new CodeArgumentReferenceExpression("unsafeHandle"),
                        new CodePrimitiveExpression(true))));

            safeHandle.Members.Add(simpleDangerousCreate);

            // Add a "Zero" property which returns an invalid handle
            CodeMemberProperty zeroProperty = new CodeMemberProperty();
            zeroProperty.Name = "Zero";
            zeroProperty.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            zeroProperty.Type = new CodeTypeReference(safeHandle.Name);

            zeroProperty.HasGet = true;

            zeroProperty.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                            new CodeTypeReferenceExpression(safeHandle.Name),
                            dangerousCreate.Name),
                        new CodePropertyReferenceExpression(
                            new CodeTypeReferenceExpression(typeof(IntPtr)),
                            nameof(IntPtr.Zero)))));

            safeHandle.Members.Add(zeroProperty);

            // Create the ToString method which returns:
            // {handle} ({type})
            CodeMemberMethod toStringMethod = new CodeMemberMethod();
            toStringMethod.Name = "ToString";
            toStringMethod.Attributes = MemberAttributes.Public | MemberAttributes.Override;
            toStringMethod.ReturnType = new CodeTypeReference(typeof(string));
            toStringMethod.Statements.Add(
                new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(
                        new CodeTypeReferenceExpression(typeof(string)),
                        "Format",
                        new CodePrimitiveExpression("{0} ({1})"),
                        new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), "handle"),
                        new CodePrimitiveExpression(safeHandle.Name))));
            safeHandle.Members.Add(toStringMethod);

            // Create the Equals method:
            //
            // if (!(obj is AfcClientHandle))
            // {
            //    return false;
            // }
            //
            // return ((AfcClientHandle)obj).handle.Equals(this.handle);
            CodeMemberMethod equalsMethod = new CodeMemberMethod();
            equalsMethod.Name = "Equals";
            equalsMethod.Attributes = MemberAttributes.Public | MemberAttributes.Override;
            equalsMethod.ReturnType = new CodeTypeReference(typeof(bool));
            equalsMethod.Parameters.Add(
                new CodeParameterDeclarationExpression(
                    new CodeTypeReference(typeof(object)),
                    "obj"));

            equalsMethod.Statements.Add(
                new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(
                        new CodeBinaryOperatorExpression(
                            new CodeArgumentReferenceExpression("obj"),
                            CodeBinaryOperatorType.IdentityInequality,
                            new CodePrimitiveExpression(null)),
                        CodeBinaryOperatorType.BooleanAnd,
                        new CodeBinaryOperatorExpression(
                            new CodeMethodInvokeExpression(
                                new CodeArgumentReferenceExpression("obj"),
                                "GetType"),
                            CodeBinaryOperatorType.IdentityEquality,
                            new CodeTypeOfExpression(safeHandle.Name))),
                    new CodeStatement[]
                    {
                        new CodeMethodReturnStatement(
                            new CodeMethodInvokeExpression(
                                new CodeFieldReferenceExpression(
                            new CodeCastExpression(
                                new CodeTypeReference(safeHandle.Name),
                                new CodeArgumentReferenceExpression("obj")),
                            "handle"),
                            "Equals",
                            new CodeFieldReferenceExpression(
                                new CodeThisReferenceExpression(),
                                "handle")))
                    },
                    new CodeStatement[]
                    {
                        new CodeMethodReturnStatement(
                            new CodePrimitiveExpression(false))
                    }));

            safeHandle.Members.Add(equalsMethod);

            // Create the GetHashCode method
            // return this.handle.GetHashCode();
            CodeMemberMethod getHashCodeMethod = new CodeMemberMethod();
            getHashCodeMethod.Name = "GetHashCode";
            getHashCodeMethod.Attributes = MemberAttributes.Public | MemberAttributes.Override;
            getHashCodeMethod.ReturnType = new CodeTypeReference(typeof(int));
            getHashCodeMethod.Statements.Add(
                new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            "handle"),
                        "GetHashCode")));
            safeHandle.Members.Add(getHashCodeMethod);

            yield return safeHandle;

            // Create the marshaler type
            CodeTypeDeclaration safeHandleMarshaler = new CodeTypeDeclaration();
            safeHandleMarshaler.Name = name + "HandleDelegateMarshaler";
            safeHandleMarshaler.IsPartial = true;
            safeHandleMarshaler.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            safeHandleMarshaler.BaseTypes.Add(typeof(ICustomMarshaler));

            // Create the GetInstance method
            CodeMemberMethod getInstanceMethod = new CodeMemberMethod();
            getInstanceMethod.Name = "GetInstance";
            getInstanceMethod.ReturnType = new CodeTypeReference(typeof(ICustomMarshaler));
            getInstanceMethod.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            getInstanceMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(string), "cookie"));
            getInstanceMethod.Statements.Add(
                new CodeMethodReturnStatement(
                    new CodeObjectCreateExpression(safeHandleMarshaler.Name)));
            safeHandleMarshaler.Members.Add(getInstanceMethod);

            // Create the CleanUpManagedData method
            CodeMemberMethod cleanUpManagedData = new CodeMemberMethod();
            cleanUpManagedData.Name = "CleanUpManagedData";
            cleanUpManagedData.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            cleanUpManagedData.Parameters.Add(new CodeParameterDeclarationExpression(typeof(object), "managedObject"));
            safeHandleMarshaler.Members.Add(cleanUpManagedData);

            // Create the CleanUpNativeData method
            CodeMemberMethod cleanUpNativeDataMethod = new CodeMemberMethod();
            cleanUpNativeDataMethod.Name = "CleanUpNativeData";
            cleanUpNativeDataMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            cleanUpNativeDataMethod.Parameters.Add(new CodeParameterDeclarationExpression(typeof(IntPtr), "nativeData"));
            safeHandleMarshaler.Members.Add(cleanUpNativeDataMethod);

            // Create the GetNativeDataSize method
            CodeMemberMethod getNativeDataSizeMethod = new CodeMemberMethod();
            getNativeDataSizeMethod.Name = "GetNativeDataSize";
            getNativeDataSizeMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            getNativeDataSizeMethod.ReturnType = new CodeTypeReference(typeof(int));
            getNativeDataSizeMethod.Statements.Add(
                new CodeMethodReturnStatement(
                    new CodePrimitiveExpression(-1)));
            safeHandleMarshaler.Members.Add(getNativeDataSizeMethod);

            // Create the MarshalManagedToNative method
            CodeMemberMethod marshalManagedToNativeMethod = new CodeMemberMethod();
            marshalManagedToNativeMethod.Name = "MarshalManagedToNative";
            marshalManagedToNativeMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            marshalManagedToNativeMethod.ReturnType = new CodeTypeReference(typeof(IntPtr));
            marshalManagedToNativeMethod.Parameters.Add(
                new CodeParameterDeclarationExpression(
                    typeof(object),
                    "managedObject"));
            marshalManagedToNativeMethod.Statements.Add(
                new CodeMethodReturnStatement(
                    new CodePropertyReferenceExpression(
                        new CodeTypeReferenceExpression(typeof(IntPtr)),
                        nameof(IntPtr.Zero))));
            safeHandleMarshaler.Members.Add(marshalManagedToNativeMethod);

            // Create the MarshalNativeToManaged method
            CodeMemberMethod marshalNativeToManagedMethod = new CodeMemberMethod();
            marshalNativeToManagedMethod.Name = "MarshalNativeToManaged";
            marshalNativeToManagedMethod.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            marshalNativeToManagedMethod.ReturnType = new CodeTypeReference(typeof(object));
            marshalNativeToManagedMethod.Parameters.Add(
                new CodeParameterDeclarationExpression(
                    typeof(IntPtr),
                    "nativeData"));
            marshalNativeToManagedMethod.Statements.Add(
                new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                            new CodeTypeReferenceExpression(safeHandle.Name),
                            "DangerousCreate"),
                        new CodeArgumentReferenceExpression("nativeData"),

                        // ownsHandle: false
                        new CodePrimitiveExpression(false))));
            safeHandleMarshaler.Members.Add(marshalNativeToManagedMethod);

            yield return safeHandleMarshaler;
        }
    }
}
