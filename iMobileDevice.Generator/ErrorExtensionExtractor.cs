using iMobileDevice.Generator.Nustache;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iMobileDevice.Generator
{
    internal class ErrorExtensionExtractor
    {
        private ModuleGenerator generator;
        private FunctionVisitor visitor;

        public ErrorExtensionExtractor(ModuleGenerator generator, FunctionVisitor visitor)
        {
            if (generator == null)
            {
                throw new ArgumentNullException(nameof(generator));
            }

            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            this.generator = generator;
            this.visitor = visitor;
        }

        public void Generate()
        {
            var errorEnum = this.generator.Types.SingleOrDefault(t => t.Name.EndsWith("Error"));

            if (errorEnum == null)
            {
                return;
            }

            // Add the exception type

            // Generate the {Name}Exception class
            var exceptionTypeName = $"{this.generator.Name}Exception";
            this.generator.AddType(
                exceptionTypeName,
                new NustacheGeneratedType(
                    new NustacheType(this.generator.Name, exceptionTypeName),
                    "Exception.cs.template"));

            var extensionsTypeName = $"{errorEnum.Name}Extensions";
            this.generator.AddType(
                extensionsTypeName,
                new NustacheGeneratedType(
                    new NustacheType(this.generator.Name, extensionsTypeName),
                    "ErrorExtensions.cs.template"));
        }
    }
}
