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
            this.WriteCustomAttributes(property.CustomAttributes);
            this.WriteAttributes(property.Attributes);
            this.Generate(property.Type);
            this.Write(" ");
            this.WriteName(property.Name);
            this.WriteLine();
            this.WriteLine("{");
            this.Indent++;

            if (property.GetStatements != null && property.GetStatements.Count > 0)
            {
                this.WriteLine("get");
                this.WriteLine("{");
                this.Indent++;

                this.Generate(property.GetStatements);

                this.Indent--;
                this.WriteLine("}");
            }
            else if (property.HasGet)
            {
                this.WriteLine("get;");
            }

            if (property.SetStatements != null && property.SetStatements.Count > 0)
            {
                this.WriteLine("set");
                this.WriteLine("{");
                this.Indent++;

                this.Generate(property.SetStatements);

                this.Indent--;
                this.WriteLine("}");
            }
            else if (property.HasSet)
            {
                this.WriteLine("set;");
            }

            this.Indent--;
            this.WriteLine("}");
        }

        private void Generate(CodeMemberField field)
        {
            // This happens for enums
            bool isEnum = field.Type.BaseType == "System.Void";

            this.WriteDocumentation(field.Comments);
            this.WriteCustomAttributes(field.CustomAttributes);

            if (!isEnum)
            {
                this.WriteAttributes(field.Attributes);
                this.Generate(field.Type);
                this.Write(" ");
            }

            this.WriteName(field.Name);

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
            if (HasAttribute(attributes, MemberAttributes.Public))
            {
                this.Write("public ");
            }
            else if (HasAttribute(attributes, MemberAttributes.Family))
            {
                this.Write("protected ");
            }
            else if (HasAttribute(attributes, MemberAttributes.Private))
            {
                this.Write("private ");
            }

            if (HasAttribute(attributes, MemberAttributes.Const))
            {
                this.Write("const ");
            }
            else if (HasAttribute(attributes, MemberAttributes.Static))
            {
                this.Write("static ");
            }
            else if (HasAttribute(attributes, MemberAttributes.Abstract))
            {
                this.Write("abstract ");
            }
            else if (HasAttribute(attributes, MemberAttributes.Override))
            {
                this.Write("override ");
            }
            else if (!HasAttribute(attributes, MemberAttributes.Final))
            {
                this.Write("virtual ");
            }
        }

        private void Generate(CodeTypeDeclaration type, CodeConstructor member)
        {
            this.WriteDocumentation(member.Comments);
            var attributes = member.Attributes;
            attributes |= MemberAttributes.Final;
            this.WriteAttributes(attributes);

            this.WriteName(type.Name);
            this.Write("(");
            this.Generate(member.Parameters);
            this.Write(")");

            if (member.BaseConstructorArgs != null && member.BaseConstructorArgs.Count > 0)
            {
                this.WriteLine(" : ");

                this.Indent += 2;
                this.Write("base(");

                bool isFirst = true;

                foreach (CodeExpression arg in member.BaseConstructorArgs)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                    }
                    else
                    {
                        this.Write(", ");
                    }

                    this.Generate(arg);
                }

                this.WriteLine(")");
                this.Indent -= 2;
            }
            else
            {
                this.WriteLine();
            }

            this.WriteLine("{");
            this.Indent++;

            this.Generate(member.Statements);

            this.Indent--;
            this.WriteLine("}");
        }

        private void Generate(CodeMemberMethod member, bool isInterface = false)
        {
            this.WriteDocumentation(member.Comments);
            this.WriteCustomAttributes(member.CustomAttributes);

            if (!isInterface)
            {
                this.WriteAttributes(member.Attributes);
            }

            this.Generate(member.ReturnType);
            this.Write(" ");
            this.WriteName(member.Name);
            this.Write("(");
            this.Generate(member.Parameters);
            this.Write(")");

            if (!isInterface && (!HasAttribute(member.Attributes, MemberAttributes.Abstract)
                || HasAttribute(member.Attributes, MemberAttributes.Static)))
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
        }

        private void Generate(CodeParameterDeclarationExpressionCollection parameters)
        {
            bool isFirst = true;

            foreach (CodeParameterDeclarationExpression parameter in parameters)
            {
                if (!isFirst)
                {
                    this.Write(", ");
                }
                else
                {
                    isFirst = false;
                }

                this.WriteCustomAttributes(parameter.CustomAttributes, inLine: true);

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
                this.WriteName(parameter.Name);
            }
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

        private void WriteName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            name = name.Trim();

            // Should match all keywords, currently event/string are the only keywords we encounter.
            if (name == "event")
            {
                this.Write("@event");
            }
            else if (name == "string")
            {
                this.Write("@string");
            }
            else
            {
                this.Write(name);
            }
        }
    }
}
