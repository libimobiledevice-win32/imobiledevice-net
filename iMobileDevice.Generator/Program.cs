// <copyright file="Program.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.IO;
    using System.Globalization;
    internal class Program
    {
        public static void Main(string[] args)
        {
            string sourceDirectory = null;
            string targetDirectory = null;

            if (args.Length >= 1)
            {
                sourceDirectory = args[0];
            }
            else
            {
                // Default value so that you can just F5 from within Visual Studio
                sourceDirectory = @"..\..\..\..\";
            }

            if (args.Length >= 2)
            {
                targetDirectory = args[1];
            }
            else
            {
                targetDirectory = @"..\..\..\..\iMobileDevice-net";
            }

            sourceDirectory = Path.GetFullPath(sourceDirectory);
            targetDirectory = Path.GetFullPath(targetDirectory);

            Console.WriteLine($"Reading libimobiledevice headers from: {sourceDirectory}");
            Console.WriteLine($"Writing the C# files to: {targetDirectory}");

            ModuleGenerator generator = new ModuleGenerator();

            generator.IncludeDirectories.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Microsoft Visual Studio 14.0\VC\include"));
            generator.IncludeDirectories.Add(GetWindowsKitUcrtFolder());
            generator.IncludeDirectories.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Windows Kits", "8.1", "include", "shared"));
            generator.IncludeDirectories.Add(Path.Combine(sourceDirectory, @"packages\libusbmuxd.1.0.10.13\build\native\include\"));
            generator.IncludeDirectories.Add(Path.Combine(sourceDirectory, @"packages\libimobiledevice.1.2.0.56\build\native\include\"));
            generator.IncludeDirectories.Add(Path.Combine(sourceDirectory, @"packages\libplist.1.12.48\build\native\include"));
            generator.IncludeDirectories.Add(Path.Combine(sourceDirectory, @"packages\libusbmuxd.1.0.10.13\build\native\include"));

            Collection<string> names = new Collection<string>();

            var files = new List<string>();
            files.Add(Path.Combine(sourceDirectory, @"packages\libusbmuxd.1.0.10.13\build\native\include\usbmuxd.h"));
            files.Add(Path.Combine(sourceDirectory, @"packages\libplist.1.12.48\build\native\include\plist\plist.h"));

            var iMobileDeviceDirectory = Path.Combine(sourceDirectory, @"packages\libimobiledevice.1.2.0.56\build\native\include\libimobiledevice");
            files.Add(Path.Combine(iMobileDeviceDirectory, "libimobiledevice.h"));
            files.Add(Path.Combine(iMobileDeviceDirectory, "lockdown.h"));
            files.Add(Path.Combine(iMobileDeviceDirectory, "afc.h"));

            var iMobileDeviceFileNames = Directory.GetFiles(iMobileDeviceDirectory, "*.h")
                .Where(f => !files.Contains(f, StringComparer.OrdinalIgnoreCase));

            files.AddRange(iMobileDeviceFileNames);

            foreach (var file in files)
            {
                Console.WriteLine($"Processing {Path.GetFileName(file)}");
                generator.InputFile = file;
                generator.Generate(targetDirectory);
                generator.Types.Clear();

                names.Add(generator.Name);
            }

            ApiGenerator apiGenerator = new ApiGenerator();
            apiGenerator.Generate(names, targetDirectory);

            // StylecopFixer.Run(Path.Combine(targetDirectory, "iMobileDevice.csproj"));
        }

        static string GetWindowsKitUcrtFolder()
        {
            var windowsKitsRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Windows Kits");

            var windowsKits = new DirectoryInfo(windowsKitsRoot)
                .EnumerateDirectories()
                .Where(d => d.Name != "NETFXSDK")
                .OrderByDescending(d => float.Parse(d.Name, CultureInfo.InvariantCulture))
                .Select(d => d.FullName)
                .ToArray();

            var windowsKit = windowsKits.First();

            Console.WriteLine($"Opening Windows Kit {windowsKit}");

            var includeRoot = Path.Combine(windowsKit, "Include");

            var includeDirs = new DirectoryInfo(includeRoot)
                .EnumerateDirectories()
                .OrderByDescending(d => TryGetVersion(d.Name))
                .Select(d => d.FullName)
                .ToArray();

            var includeDir = includeDirs.First();

            Console.WriteLine($"Opening Windows Kit include directory {includeDir}. Contains the following directories:");

            foreach (var directory in new DirectoryInfo(includeDir).GetDirectories())
            {
                Console.WriteLine($" {directory.Name}");
            }

            var ucrt = Path.Combine(includeDir, "ucrt");

            Console.Write("ucrt contains the following files:");
            foreach (var file in new DirectoryInfo(ucrt).GetFiles())
            {
                Console.WriteLine($" {file.Name}");
            }

            return ucrt;
        }

        private static Version TryGetVersion(string name)
        {
            Version version;
            if (Version.TryParse(name, out version))
            {
                return version;
            }
            else
            {
                Console.WriteLine($"Encountered unexpected value {name}");
                return new Version("0.0.0.0");
            }
        }
    }
}
