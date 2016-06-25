// <copyright file="CSharpTextWriter.Expressions.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator.CodeDom
{
    using System;
    using System.CodeDom;

    internal partial class CSharpTextWriter
    {
        private void Generate(CodeExpression expression)
        {
            if (expression is CodeMethodInvokeExpression)
            {
                this.Generate((CodeMethodInvokeExpression)expression);
            }
            else if (expression is CodeThisReferenceExpression)
            {
                this.Write("this");
            }
            else if (expression is CodeArgumentReferenceExpression)
            {
                this.Write(((CodeArgumentReferenceExpression)expression).ParameterName);
            }
            else if (expression is CodeVariableReferenceExpression)
            {
                this.Write(((CodeVariableReferenceExpression)expression).VariableName);
            }
            else if (expression is CodePrimitiveExpression)
            {
                this.Write(((CodePrimitiveExpression)expression).Value);
            }
            else if (expression is CodeTypeReferenceExpression)
            {
                this.Generate(((CodeTypeReferenceExpression)expression).Type);
            }
            else if (expression is CodeDirectionExpression)
            {
                this.Generate((CodeDirectionExpression)expression);
            }
            else if (expression is CodeCastExpression)
            {
                this.Generate((CodeCastExpression)expression);
            }
            else if (expression is CodePropertyReferenceExpression)
            {
                this.Generate((CodePropertyReferenceExpression)expression);
            }
            else if (expression is CodeFieldReferenceExpression)
            {
                this.Generate((CodeFieldReferenceExpression)expression);
            }
            else if (expression is CodeObjectCreateExpression)
            {
                this.Generate((CodeObjectCreateExpression)expression);
            }
            else if (expression is CodeBinaryOperatorExpression)
            {
                this.Generate((CodeBinaryOperatorExpression)expression);
            }
            else if (expression is CodeTypeOfExpression)
            {
                this.Generate((CodeTypeOfExpression)expression);
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        private void Generate(CodeTypeOfExpression expression)
        {
            this.Write("typeof(");
            this.Write(expression.Type);
            this.Write(")");
        }

        private void Generate(CodeBinaryOperatorExpression expression)
        {
            this.Generate(expression.Left);
            this.Write(" ");

            switch (expression.Operator)
            {
                case CodeBinaryOperatorType.IdentityEquality:
                    this.Write("==");
                    break;

                case CodeBinaryOperatorType.IdentityInequality:
                    this.Write("!=");
                    break;

                case CodeBinaryOperatorType.BooleanAnd:
                    this.Write("&");
                    break;

                default:
                    throw new NotSupportedException();
            }

            this.Write(" ");
            this.Generate(expression.Right);
        }

        private void Generate(CodeObjectCreateExpression expression)
        {
            this.Write("new ");
            this.Generate(expression.CreateType);
        }

        private void Generate(CodeFieldReferenceExpression expression)
        {
            this.Generate(expression.TargetObject);
            this.Write(".");
            this.Write(expression.FieldName);
        }

        private void Generate(CodePropertyReferenceExpression expression)
        {
            this.Generate(expression.TargetObject);
            this.Write(".");
            this.Write(expression.PropertyName);
        }

        private void Generate(CodeCastExpression expression)
        {
            this.Write("(");
            this.Generate(expression.TargetType);
            this.Write(")");
            this.Generate(expression.Expression);
        }

        private void Generate(CodeDirectionExpression expression)
        {
            if (expression.Direction == FieldDirection.Out)
            {
                this.Write("out ");
            }
            else if (expression.Direction == FieldDirection.Ref)
            {
                this.Write("ref ");
            }

            this.Generate(expression.Expression);
        }

        private void Generate(CodeMethodInvokeExpression expression)
        {
            this.Generate(expression.Method.TargetObject);
            this.Write(".");
            this.Write(expression.Method.MethodName);

            this.Write("(");
            bool isFirst = true;
            foreach (CodeExpression parameter in expression.Parameters)
            {
                if (!isFirst)
                {
                    this.Write(", ");
                }
                else
                {
                    isFirst = false;
                }

                this.Generate(parameter);
            }

            this.Write(")");
        }
    }
}
