using System;
using System.CodeDom;
using System.Runtime.InteropServices;

namespace iMobileDevice.Generator
{
    internal class ArrayMarshalerGenerator
    {
        private ModuleGenerator generator;

        public ArrayMarshalerGenerator(ModuleGenerator generator)
        {
            if (generator == null)
            {
                throw new ArgumentNullException(nameof(generator));
            }

            this.generator = generator;
        }

        public void Generate()
        {
            // We only generate string array marshalers for a very limited number of
            // modules. Each module has a different _free method, so we want to respect that.
            string freeMethodName = null;
            string marshalerName = null;

            if (this.generator.Name == "Afc")
            {
                freeMethodName = "afc_dictionary_free";
                marshalerName = "AfcDictionaryMarshaler";
            }
            else if (this.generator.Name == "iDevice")
            {
                freeMethodName = "idevice_device_list_free";
                marshalerName = "iDeviceListMarshaler";
            }
            else if (this.generator.Name == "Lockdown")
            {
                freeMethodName = "lockdownd_data_classes_free";
                marshalerName = "LockdownMarshaler";
            }

            if (freeMethodName == null || marshalerName == null)
            {
                return;
            }

            CodeTypeDeclaration marshaler = new CodeTypeDeclaration();
            marshaler.Attributes = MemberAttributes.Public | MemberAttributes.Final;
            marshaler.Name = marshalerName;
            marshaler.BaseTypes.Add(new CodeTypeReference("NativeStringArrayMarshaler"));

            // Create the following two methods:
            // public static ICustomMarshaler GetInstance(string cookie)
            // {
            //      return new DeviceeListMarshaler();
            // }
            //
            // public override void CleanUpNativeData(IntPtr nativeData)
            // {
            //    LibiMobileDevice.Instance.iDevice.idevice_device_list_free(nativeData).ThrowOnError();
            // }

            CodeMemberMethod getInstanceMethod = new CodeMemberMethod();
            getInstanceMethod.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            getInstanceMethod.Name = "GetInstance";
            getInstanceMethod.Parameters.Add(
                new CodeParameterDeclarationExpression(
                    new CodeTypeReference(typeof(string)),
                    "cookie"));
            getInstanceMethod.ReturnType = new CodeTypeReference(typeof(ICustomMarshaler));

            getInstanceMethod.Statements.Add(
                new CodeMethodReturnStatement(
                    new CodeObjectCreateExpression(marshalerName)));
            marshaler.Members.Add(getInstanceMethod);

            CodeMemberMethod cleanUpNativeDataMethod = new CodeMemberMethod();
            cleanUpNativeDataMethod.Name = "CleanUpNativeData";
            cleanUpNativeDataMethod.Attributes = MemberAttributes.Public | MemberAttributes.Override;
            cleanUpNativeDataMethod.Parameters.Add(
                new CodeParameterDeclarationExpression(
                    new CodeTypeReference(typeof(IntPtr)),
                    "nativeData"));

            // LibiMobileDevice.Instance.iDevice
            var api =
                new CodePropertyReferenceExpression(
                    new CodePropertyReferenceExpression(
                    new CodeTypeReferenceExpression("LibiMobileDevice"),
                    "Instance"),
                    this.generator.Name);

            var freeInvoke =
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        api,
                        freeMethodName),
                    new CodeArgumentReferenceExpression("nativeData"));

            var throwOnErrorInvoke =
                new CodeMethodInvokeExpression(
                    new CodeMethodReferenceExpression(
                        freeInvoke,
                        "ThrowOnError"));
            cleanUpNativeDataMethod.Statements.Add(throwOnErrorInvoke);
            marshaler.Members.Add(cleanUpNativeDataMethod);

            this.generator.AddType(marshaler.Name, marshaler);
            this.generator.StringArrayMarshalerType = marshaler;
        }
    }
}
