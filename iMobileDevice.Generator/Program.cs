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

            ModuleGenerator generator = new ModuleGenerator();

            generator.IncludeDirectories.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Microsoft Visual Studio 14.0\VC\include"));
            generator.IncludeDirectories.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Windows Kits\10\Include\10.0.10240.0\ucrt\"));
            generator.IncludeDirectories.Add(Path.Combine(sourceDirectory, @"libimobiledevice\include"));
            generator.IncludeDirectories.Add(Path.Combine(sourceDirectory, @"libplist\include"));
            generator.IncludeDirectories.Add(Path.Combine(sourceDirectory, @"libusbmuxd\include"));

            Collection<string> names = new Collection<string>();

            foreach (var file in Directory.GetFiles(Path.Combine(sourceDirectory, @"libimobiledevice\include\libimobiledevice"), "*.h"))
            {
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
