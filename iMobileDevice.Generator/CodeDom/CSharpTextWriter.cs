namespace iMobileDevice.Generator.CodeDom
{
    using System;
    using System.CodeDom;
    using System.CodeDom.Compiler;
    using System.IO;
    using System.Linq;

    internal partial class CSharpTextWriter : IndentedTextWriter
    {
        public CSharpTextWriter(TextWriter writer, string tabString)
            : base(writer, tabString)
        {
        }

        internal void Generate(CodeNamespace ns)
        {
            this.WriteLine($@"// <copyright file=""{ns.Types.OfType<CodeTypeDeclaration>().First().Name}.cs"" company=""Quamotion"">");
            this.WriteLine($@"// Copyright (c) {DateTime.Now.Year} Quamotion. All rights reserved.");
            this.WriteLine($@"// </copyright>");
            this.WriteLine();
            this.WriteLine($@"namespace {ns.Name}");
            this.WriteLine("{");
            this.Indent++;

            foreach (CodeNamespaceImport usingStatement in ns.Imports)
            {
                this.Write("using ");
                this.Write(usingStatement.Namespace);
                this.Write(";");
                this.WriteLine();
            }

            this.WriteLine();
            this.WriteLine();

            foreach (CodeTypeDeclaration type in ns.Types)
            {
                this.WriteDocumentation(type.Comments);

                if (type is CodeTypeDelegate)
                {
                    this.Generate((CodeTypeDelegate)type);
                }
                else
                {
                    this.Generate(type);
                }
            }

            this.Indent--;
            this.WriteLine("}");
        }

        private void Generate(CodeTypeDelegate @delegate)
        {
            this.WriteCustomAttributes(@delegate.CustomAttributes);
            this.WriteAttributes(@delegate.Attributes);
            this.Write("delegate ");
            this.Generate(@delegate.ReturnType);
            this.Write(" ");
            this.WriteName(@delegate.Name);
            this.Write("(");
            this.Generate(@delegate.Parameters);
            this.WriteLine(");");
        }

        private void Generate(CodeTypeDeclaration type)
        {
            this.WriteCustomAttributes(type.CustomAttributes);

            var attributes = type.Attributes;

            if (type.IsEnum)
            {
                attributes |= MemberAttributes.Final;
            }

            this.WriteAttributes(attributes);
            this.WriteIfTrue(type.IsPartial, "partial");
            this.WriteIfTrue(type.IsClass, "class");
            this.WriteIfTrue(type.IsStruct, "struct");
            this.WriteIfTrue(type.IsEnum, "enum");
            this.WriteIfTrue(type.IsInterface, "interface");
            this.Write(type.Name);

            if (type.BaseTypes.Count > 0)
            {
                this.Write(" : ");

                bool isFirst = true;

                foreach (CodeTypeReference baseType in type.BaseTypes)
                {
                    if (!isFirst)
                    {
                        this.Write(", ");
                    }
                    else
                    {
                        isFirst = false;
                    }

                    this.Generate(baseType);
                }
            }

            // Write the methods
            this.WriteLine();
            this.WriteLine("{");
            this.Indent++;
            foreach (var member in type.Members.OfType<CodeMemberField>())
            {
                this.WriteLine();
                this.Generate(member);
            }

            foreach (var member in type.Members.OfType<CodeConstructor>())
            {
                this.WriteLine();
                this.Generate(type, member);
            }

            foreach (var member in type.Members.OfType<CodeMemberProperty>())
            {
                this.WriteLine();
                this.Generate(member);
            }

            foreach (var member in type.Members.OfType<CodeMemberMethod>())
            {
                if (member is CodeConstructor)
                {
                    continue;
                }

                this.WriteLine();
                this.Generate(member, isInterface: type.IsInterface);
            }

            this.Indent--;
            this.WriteLine("}");
        }

        private void WriteCustomAttributes(CodeAttributeDeclarationCollection customAttributes, bool inLine = false)
        {
            foreach (CodeAttributeDeclaration attribute in customAttributes)
            {
                this.Write("[");
                this.Generate(attribute.AttributeType);

                this.Write("(");

                bool isFirst = true;

                foreach (CodeAttributeArgument argument in attribute.Arguments)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        this.Write(", ");
                    }

                    if (!string.IsNullOrEmpty(argument.Name))
                    {
                        this.Write(argument.Name);
                        this.Write("=");
                    }

                    this.Generate(argument.Value);
                }

                this.Write(")]");

                if (inLine)
                {
                    this.Write(" ");
                }
                else
                {
                    this.WriteLine();
                }
            }
        }

        private void WriteIfTrue(bool condition, string value)
        {
            if (condition)
            {
                this.Write(value);
                this.Write(" ");
            }
        }

        private void Generate(CodeTypeReference type)
        {
            if (type.TypeArguments == null || type.TypeArguments.Count == 0)
            {
                if (type.BaseType == typeof(string).FullName)
                {
                    this.Write("string");
                }
                else if (type.BaseType == typeof(byte).FullName)
                {
                    this.Write("byte");
                }
                else if (type.BaseType == typeof(sbyte).FullName)
                {
                    this.Write("sbyte");
                }
                else if (type.BaseType == typeof(char).FullName)
                {
                    this.Write("char");
                }
                else if (type.BaseType == typeof(ushort).FullName)
                {
                    this.Write("ushort");
                }
                else if (type.BaseType == typeof(short).FullName)
                {
                    this.Write("short");
                }
                else if (type.BaseType == typeof(uint).FullName)
                {
                    this.Write("uint");
                }
                else if (type.BaseType == typeof(int).FullName)
                {
                    this.Write("int");
                }
                else if (type.BaseType == typeof(ulong).FullName)
                {
                    this.Write("ulong");
                }
                else if (type.BaseType == typeof(long).FullName)
                {
                    this.Write("long");
                }
                else if (type.BaseType == typeof(double).FullName)
                {
                    this.Write("double");
                }
                else if (type.BaseType == typeof(bool).FullName)
                {
                    this.Write("bool");
                }
                else if (type.BaseType == typeof(void).FullName)
                {
                    this.Write("void");
                }
                else if (type.BaseType == typeof(object).FullName)
                {
                    this.Write("object");
                }
                else
                {
                    this.Write(type.BaseType);
                }
            }
            else
            {
                string typeName = type.BaseType;
                typeName = typeName.Substring(0, typeName.IndexOf("`"));

                this.Write(typeName);
                this.Write("<");

                bool isFirst = true;

                foreach (CodeTypeReference argument in type.TypeArguments)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        this.Write(", ");
                    }

                    this.Generate(argument);
                }

                this.Write(">");
            }

            for (int i = 0; i < type.ArrayRank; i++)
            {
                this.Write("[]");
            }
        }
    }
}
