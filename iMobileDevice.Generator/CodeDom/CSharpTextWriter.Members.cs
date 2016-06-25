namespace iMobileDevice.Generator.CodeDom
{
    using System;
    using System.CodeDom;

    partial class CSharpTextWriter
    {
        private void Generate(CodeTypeMember member)
        {
            if (member is CodeMemberMethod)
            {
                this.Generate((CodeMemberMethod)member);
            }
            else if (member is CodeMemberField)
            {
                this.Generate((CodeMemberField)member);
            }
            else if (member is CodeMemberProperty)
            {
                this.Generate((CodeMemberProperty)member);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private void Generate(CodeMemberProperty property)
        {
            this.WriteDocumentation(property.Comments);
            this.WriteAttributes(property.Attributes);
            this.Write(" ");
            this.Generate(property.Type);
            this.Write(" ");
            this.Write(property.Name);
            this.WriteLine();
            this.WriteLine("{");
            this.Indent++;

            if (property.GetStatements != null)
            {
                this.WriteLine("get");
                this.WriteLine("{");
                this.Indent++;

                this.Generate(property.GetStatements);

                this.Indent--;
                this.WriteLine("}");

            }

            if (property.GetStatements != null)
            {
                this.WriteLine("set");
                this.WriteLine("{");
                this.Indent++;

                this.Generate(property.SetStatements);

                this.Indent--;
                this.WriteLine("}");
            }

            this.Indent--;
            this.WriteLine("}");
        }

        private void Generate(CodeMemberField field)
        {
            // This happens for enums
            bool isEnum = field.Type.BaseType == "System.Void";

            this.WriteDocumentation(field.Comments);
            this.WriteAttributes(field.Attributes);
            this.Write(" ");

            if (!isEnum)
            {
                this.Generate(field.Type);
                this.Write(" ");
            }

            this.Write(field.Name);

            if (field.InitExpression != null)
            {
                this.Write(" = ");
                this.Generate(field.InitExpression);
            }

            if (!isEnum)
            {
                this.Write(";");
            }
            else
            {
                this.Write(",");
            }

            this.WriteLine();
        }

        private void WriteAttributes(MemberAttributes attributes)
        {
            this.WriteIfAttribute(attributes, MemberAttributes.Public, "public");
            this.WriteIfNotAttribute(attributes, MemberAttributes.Final, "virtual");
        }

        private void Generate(CodeMemberMethod member)
        {
            this.WriteDocumentation(member.Comments);
            this.WriteAttributes(member.Attributes);
            this.Generate(member.ReturnType);
            this.Write(" ");
            this.Write(member.Name);
            this.Write("(");

            bool isFirst = true;

            foreach (CodeParameterDeclarationExpression parameter in member.Parameters)
            {
                if (!isFirst)
                {
                    this.Write(", ");
                }
                else
                {
                    isFirst = false;
                }

                switch (parameter.Direction)
                {
                    case FieldDirection.Out:
                        this.Write("out ");
                        break;

                    case FieldDirection.Ref:
                        this.Write("ref ");
                        break;
                }

                this.Generate(parameter.Type);
                this.Write(" ");
                this.Write(parameter.Name);
            }

            this.Write(")");

            if (member.Statements != null && member.Statements.Count > 0)
            {
                this.WriteLine();
                this.WriteLine("{");
                this.Indent++;

                this.Generate(member.Statements);

                this.Indent--;
                this.WriteLine("}");
            }
            else
            {
                // Interfaces and abstract methods
                this.WriteLine(";");
            }

            this.WriteLine();
        }

        private void WriteIfAttribute(MemberAttributes attributes, MemberAttributes expected, string value)
        {
            this.WriteIfTrue(HasAttribute(attributes, expected), value);
        }

        private void WriteIfNotAttribute(MemberAttributes attributes, MemberAttributes expected, string value)
        {
            this.WriteIfTrue(!HasAttribute(attributes, expected), value);
        }

        private static bool HasAttribute(MemberAttributes attribute, MemberAttributes expected)
        {
            return (attribute & expected) == expected;
        }
    }
}
