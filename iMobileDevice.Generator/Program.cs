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
    using System.Runtime.InteropServices;
    using System.Reflection;
    using System.IO.Compression;

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
                targetDirectory = @"..\..\..\..\..\iMobileDevice-net";
            }

            RestoreClang();

            var packagesDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages");

            sourceDirectory = Path.GetFullPath(sourceDirectory);
            targetDirectory = Path.GetFullPath(targetDirectory);

            Console.WriteLine($"Reading libimobiledevice headers from: {sourceDirectory}");
            Console.WriteLine($"Writing the C# files to: {targetDirectory}");

            ModuleGenerator generator = new ModuleGenerator();
            generator.IncludeDirectories.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Microsoft Visual Studio 14.0\VC\include"));
            generator.IncludeDirectories.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Microsoft Visual Studio\Shared\14.0\VC\include"));
            generator.IncludeDirectories.Add(GetWindowsKitUcrtFolder());
            generator.IncludeDirectories.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Windows Kits", "8.1", "include", "shared"));
            generator.IncludeDirectories.Add(Path.Combine(packagesDirectory, @"libusbmuxd\1.0.10.92\build\native\include\"));
            generator.IncludeDirectories.Add(Path.Combine(packagesDirectory, @"libimobiledevice\1.2.1.196\build\native\include\"));
            generator.IncludeDirectories.Add(Path.Combine(packagesDirectory, @"libplist\2.0.1.171\build\native\include"));
            generator.IncludeDirectories.Add(Path.Combine(packagesDirectory, @"libideviceactivation\1.0.0.23\build\native\include\libideviceactivation"));

            Collection<string> names = new Collection<string>();

            var files = new List<string>();
            files.Add(Path.Combine(packagesDirectory, @"libusbmuxd\1.0.10.92\build\native\include\usbmuxd.h"));
            files.Add(Path.Combine(packagesDirectory, @"libplist\2.0.1.171\build\native\include\plist\plist.h"));
            files.Add(Path.Combine(packagesDirectory, @"libideviceactivation\1.0.0.23\build\native\include\libideviceactivation\libideviceactivation.h"));

            var iMobileDeviceDirectory = Path.Combine(packagesDirectory, @"libimobiledevice\1.2.1.196\build\native\include\libimobiledevice");
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

                if (string.Equals(Path.GetFileName(file), "libideviceactivation.h", StringComparison.OrdinalIgnoreCase))
                {
                    generator.Generate(targetDirectory, "ideviceactivation");
                }
                else
                {
                    generator.Generate(targetDirectory);
                }

                generator.Types.Clear();

                names.Add(generator.Name);
            }

            ApiGenerator apiGenerator = new ApiGenerator();
            apiGenerator.Generate(names, targetDirectory);
        }

        static void RestoreClang()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (!File.Exists("libclang.dll"))
                {
                    var assembly = Assembly.Load(new AssemblyName("Native.LibClang"));
                    using (var stream = assembly.GetManifestResourceStream("Native.LibClang.LLVM.zip"))
                    using (var zipArchive = new ZipArchive(stream))
                    {
                        var entry = zipArchive.GetEntry("LLVM/bin/libclang.dll");
                        using (var entryStream = entry.Open())
                        using (var libraryStream = File.Open("libclang.dll", FileMode.Create, FileAccess.ReadWrite))
                        {
                            entryStream.CopyTo(libraryStream);
                        }
                    }
                }
            }
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
