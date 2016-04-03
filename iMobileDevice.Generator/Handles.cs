// <copyright file="Handles.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System.CodeDom;
    using System.Runtime.ConstrainedExecution;
    using System.Security.Permissions;
    using Microsoft.Win32.SafeHandles;

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

            return safeHandle;
        }
    }
}
