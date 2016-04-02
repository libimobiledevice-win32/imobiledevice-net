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
            var directory = args[0];

            foreach (var file in Directory.GetFiles(directory, "*.h"))
            {
                ModuleGenerator generator = new ModuleGenerator(file);
                generator.Generate();
            }
        }
    }
}
