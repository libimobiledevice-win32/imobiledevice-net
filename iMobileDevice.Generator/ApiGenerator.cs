using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMobileDevice.Generator
{
    internal class ApiGenerator
    {
        public void Generate(IEnumerable<string> names, string targetDirectory)
        {
            CodeNamespaceImportCollection imports = new CodeNamespaceImportCollection();

            // Generate the ILibiMobileDevice interface
            CodeTypeDeclaration interfaceType = new CodeTypeDeclaration();
            interfaceType.Name = "ILibiMobileDevice";
            interfaceType.IsInterface = true;

            foreach (var name in names)
            {
                interfaceType.Members.Add(
                    new CodeMemberProperty()
                    {
                        Name = name,
                        Type = new CodeTypeReference($"I{name}Api"),
                        HasGet = true
                    });

                imports.Add(new CodeNamespaceImport($"iMobileDevice.{name}"));
            }

            // Generate the interface implementation
            CodeTypeDeclaration classType = new CodeTypeDeclaration();
            classType.Name = "LibiMobileDevice";
            classType.BaseTypes.Add("ILibiMobileDevice");

            var constructor = new CodeConstructor();
            constructor.Attributes = MemberAttributes.Public;
            classType.Members.Add(constructor);

            var instanceField = new CodeMemberField();
            instanceField.Attributes = MemberAttributes.Static;
            instanceField.Name = "instance";
            instanceField.Type = new CodeTypeReference("ILibiMobileDevice");
            instanceField.InitExpression =
                new CodeObjectCreateExpression(
                    new CodeTypeReference("LibiMobileDevice"));
            classType.Members.Add(instanceField);

            var instanceProperty = new CodeMemberProperty();
            instanceProperty.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            instanceProperty.Name = "Instance";
            instanceProperty.HasGet = true;
            instanceProperty.Type = new CodeTypeReference("ILibiMobileDevice");

            instanceProperty.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeFieldReferenceExpression(
                        new CodeTypeReferenceExpression("LibiMobileDevice"),
                        "instance")));

            instanceProperty.SetStatements.Add(
                new CodeAssignStatement(
                    new CodeFieldReferenceExpression(
                        new CodeTypeReferenceExpression("LibiMobileDevice"),
                        "instance"),
                    new CodeArgumentReferenceExpression("value")));

            classType.Members.Add(instanceProperty);

            foreach (var name in names)
            {
                string camelCased = null;

                if (name[0] != 'i')
                {
                    camelCased = char.ToLower(name[0]) + name.Substring(1);
                }
                else
                {
                    camelCased = "i" + char.ToLower(name[1]) + name.Substring(2);
                }

                // Add the backing field
                classType.Members.Add(
                    new CodeMemberField()
                    {
                        Name = camelCased,
                        Type = new CodeTypeReference($"I{name}Api")
                    });

                // Add the property + getter
                var property =
                    new CodeMemberProperty()
                    {
                        Name = name,
                        Type = new CodeTypeReference($"I{name}Api"),
                        HasGet = true,
                        HasSet = true,
                        Attributes = MemberAttributes.Public
                    };
                property.GetStatements.Add(
                    new CodeMethodReturnStatement(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            camelCased)));
                property.SetStatements.Add(
                    new CodeAssignStatement(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            camelCased),
                        new CodeArgumentReferenceExpression("value")));
                classType.Members.Add(property);

                constructor.Statements.Add(
                    new CodeAssignStatement(
                        new CodeFieldReferenceExpression(
                            new CodeThisReferenceExpression(),
                            camelCased),
                        new CodeObjectCreateExpression(
                            new CodeTypeReference($"{name}Api"),
                            new CodeThisReferenceExpression())));
            }

            // Add the LibraryFound property
            var libraryFoundInterfaceProperty = new CodeMemberProperty();
            libraryFoundInterfaceProperty.Name = "LibraryFound";
            libraryFoundInterfaceProperty.Type = new CodeTypeReference(typeof(bool));
            libraryFoundInterfaceProperty.HasGet = true;
            interfaceType.Members.Add(libraryFoundInterfaceProperty);

            var libraryFoundClassProperty = new CodeMemberProperty();
            libraryFoundClassProperty.Name = "LibraryFound";
            libraryFoundClassProperty.Attributes = MemberAttributes.Public;
            libraryFoundClassProperty.Type = new CodeTypeReference(typeof(bool));
            libraryFoundClassProperty.HasGet = true;
            libraryFoundClassProperty.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodePropertyReferenceExpression(
                        new CodeTypeReferenceExpression("NativeLibraries"),
                        "LibraryFound")));
            classType.Members.Add(libraryFoundClassProperty);

            foreach (var type in new CodeTypeDeclaration[] { interfaceType, classType })
            {
                // Generate the container unit
                CodeCompileUnit program = new CodeCompileUnit();

                // Generate the namespace
                CodeNamespace ns = new CodeNamespace($"iMobileDevice");
                ns.Imports.AddRange(imports.OfType<CodeNamespaceImport>().ToArray());
                ns.Types.Add(type);
                program.Namespaces.Add(ns);

                string path = Path.Combine(targetDirectory, $"{type.Name}.cs");

                using (var outFile = File.Open(path, FileMode.Create))
                using (var fileWriter = new StreamWriter(outFile))
                using (var indentedTextWriter = new IndentedTextWriter(fileWriter, "    "))
                {
                    // Generate source code using the code provider.
                    var provider = new Microsoft.CSharp.CSharpCodeProvider();
                    provider.GenerateCodeFromCompileUnit(
                        program,
                        indentedTextWriter,
                        new CodeGeneratorOptions() { BracingStyle = "C" });
                }
            }
        }
    }
}
