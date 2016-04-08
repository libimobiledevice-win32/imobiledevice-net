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

        public static CodeTypeDeclaration CreateSafeHandle(string name)
        {
            CodeTypeDeclaration safeHandle = new CodeTypeDeclaration(name + "Handle");

            safeHandle.CustomAttributes.Add(SecurityPermissionDeclaration(SecurityAction.InheritanceDemand, true));
            safeHandle.CustomAttributes.Add(SecurityPermissionDeclaration(SecurityAction.Demand, true));
            safeHandle.BaseTypes.Add(typeof(SafeHandleZeroOrMinusOneIsInvalid));

            CodeConstructor constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Private;
            constructor.BaseConstructorArgs.Add(new CodePrimitiveExpression(true));
            safeHandle.Members.Add(constructor);

            CodeMemberMethod releaseHandle = new CodeMemberMethod();
            releaseHandle.Name = "ReleaseHandle";
            releaseHandle.Attributes = MemberAttributes.Override | MemberAttributes.Family;
            releaseHandle.ReturnType = new CodeTypeReference(typeof(bool));
            releaseHandle.CustomAttributes.Add(ReliabilityContractDeclaration(Consistency.WillNotCorruptState, Cer.MayFail));
            releaseHandle.Statements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(true)));
            safeHandle.Members.Add(releaseHandle);

            // Add a "DangeousCreate" method which creates a new safe handle from an IntPtr
            CodeMemberMethod dangerousCreate = new CodeMemberMethod();
            dangerousCreate.Name = "DangerousCreate";
            dangerousCreate.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            dangerousCreate.ReturnType = new CodeTypeReference(safeHandle.Name);

            dangerousCreate.Parameters.Add(
                new CodeParameterDeclarationExpression(
                    new CodeTypeReference(typeof(IntPtr)),
                    "unsafeHandle"));

            dangerousCreate.Statements.Add(
                new CodeVariableDeclarationStatement(
                    new CodeTypeReference(safeHandle.Name),
                    "safeHandle"));

            dangerousCreate.Statements.Add(
                new CodeAssignStatement(
                    new CodeVariableReferenceExpression("safeHandle"),
                    new CodeObjectCreateExpression(
                        new CodeTypeReference(safeHandle.Name))));

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

            return safeHandle;
        }
    }
}
