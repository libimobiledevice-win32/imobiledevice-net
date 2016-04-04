// <copyright file="Program.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;

    internal class Program
    {
        public static void Main(string[] args)
        {
            var sourceDirectory = args[0];
            var targetDirectory = args[1];

            sourceDirectory = Path.GetFullPath(sourceDirectory);
            targetDirectory = Path.GetFullPath(targetDirectory);

            Console.WriteLine($"Reading libimobiledevice headers from: {sourceDirectory}");
            Console.WriteLine($"Writing the C# files to: {targetDirectory}");

            ModuleGenerator generator = new ModuleGenerator();

            generator.IncludeDirectories.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Microsoft Visual Studio 14.0\VC\include"));
            generator.IncludeDirectories.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Windows Kits\10\Include\10.0.10240.0\ucrt\"));
            generator.IncludeDirectories.Add(Path.Combine(sourceDirectory, @"packages\libimobiledevice.1.2.0\build\native\include\"));
            generator.IncludeDirectories.Add(Path.Combine(sourceDirectory, @"packages\libplist.1.12.47\build\native\include"));
            generator.IncludeDirectories.Add(Path.Combine(sourceDirectory, @"packages\libusbmuxd.1.0.10.13\build\native\include"));

            Collection<string> names = new Collection<string>();

            foreach (var file in Directory.GetFiles(Path.Combine(sourceDirectory, @"packages\libimobiledevice.1.2.0\build\native\include\libimobiledevice"), "*.h"))
            {
                Console.WriteLine($"Processing {Path.GetFileName(file)}");
                generator.InputFile = file;
                generator.Generate(targetDirectory);
                generator.Types.Clear();

                names.Add(generator.Name);
            }

            ApiGenerator apiGenerator = new ApiGenerator();
            apiGenerator.Generate(names, targetDirectory);
        }
    }
}
