// <copyright file="Program.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System.IO;

    internal class Program
    {
        public static void Main(string[] args)
        {
            var sourceDirectory = args[0];
            var targetDirectory = args[1];

            foreach (var file in Directory.GetFiles(sourceDirectory, "*.h"))
            {
                ModuleGenerator generator = new ModuleGenerator(file);
                generator.Generate(targetDirectory);
            }
        }
    }
}
