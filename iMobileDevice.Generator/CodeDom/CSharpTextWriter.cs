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
            this.WriteLine($@"// </copyright");
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

            foreach (CodeTypeDeclaration type in ns.Types)
            {
                this.WriteDocumentation(type.Comments);
                this.Generate(type);
            }

            this.Indent--;
            this.WriteLine("}");
        }

        private void Generate(CodeTypeDeclaration type)
        {
            this.WriteAttributes(type.Attributes);
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

                    this.Write(baseType.BaseType);
                }
            }

            // Write the methods
            this.WriteLine();
            this.WriteLine("{");
            this.Indent++;
            foreach (CodeTypeMember member in type.Members)
            {
                this.Generate(member);
            }

            this.Indent--;
            this.WriteLine("}");
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
                this.Write(type.BaseType);
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
        }
    }
}
