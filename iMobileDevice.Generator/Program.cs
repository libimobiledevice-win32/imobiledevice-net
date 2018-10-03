// <copyright file="Program.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using Microsoft.Extensions.CommandLineUtils;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;

    internal class Program
    {
        public static int Main(string[] args)
        {
            CommandLineApplication commandLineApplication =
                new CommandLineApplication(throwOnUnexpectedArg: false);

            commandLineApplication.Name = "iMobileDevice.Generator";
            commandLineApplication.HelpOption("-?|-h|--help");

            commandLineApplication.Command(
                "generate",
                (runCommand) =>
                {
                    runCommand.Description = "Generates the Interop source for imobiledevice-net based on the libimobiledevice headers";

                    CommandOption outputArgument = runCommand.Option(
                        "-o|--output <dir>",
                        "The output directory. The C# code will be generated in this directory.",
                        CommandOptionType.SingleValue);

                    CommandOption includeArgument = runCommand.Option(
                        "-i|--include <dir>",
                        "Include directory to use. Defaults to the include directory in VCPKG_ROOT.",
                        CommandOptionType.SingleValue);

                    runCommand.HelpOption("-? | -h | --help");

                    runCommand.OnExecute(() =>
                    {
                        string targetDirectory = @"..\..\..\..\..\iMobileDevice-net";
                        if (outputArgument.HasValue())
                        {
                            targetDirectory = outputArgument.Value();
                        }

                        targetDirectory = Path.GetFullPath(targetDirectory);

                        RestoreClang();

                        ModuleGenerator generator = new ModuleGenerator();
                        generator.IncludeDirectories.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Microsoft Visual Studio 14.0\VC\include"));
                        generator.IncludeDirectories.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Microsoft Visual Studio\Shared\14.0\VC\include"));
                        generator.IncludeDirectories.Add(GetWindowsKitUcrtFolder());
                        generator.IncludeDirectories.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Windows Kits", "8.1", "include", "shared"));

                        string sourceDir = null;

                        if (includeArgument.HasValue())
                        {
                            sourceDir = includeArgument.Value();
                        }
                        else
                        {
                            var vcpkgPath = Environment.GetEnvironmentVariable("VCPKG_ROOT");

                            if (vcpkgPath == null)
                            {
                                Console.Error.WriteLine("Please set the VCPKG_ROOT environment variable to the folder where you've installed VCPKG.");
                                return -1;
                            }

                            vcpkgPath = Path.Combine(vcpkgPath, "installed", "x86-windows", "include");
                            Console.WriteLine($"Reading include files from {vcpkgPath}");
                            sourceDir = vcpkgPath;
                        }

                        generator.IncludeDirectories.Add(sourceDir);

                        Console.WriteLine($"Writing the C# files to: {targetDirectory}");

                        Collection<string> names = new Collection<string>();

                        var files = new List<string>();
                        files.Add(Path.Combine(sourceDir, "usbmuxd.h"));
                        files.Add(Path.Combine(sourceDir, "plist/plist.h"));
                        files.Add(Path.Combine(sourceDir, "libideviceactivation.h"));
                        var iMobileDeviceDirectory = Path.Combine(sourceDir, "libimobiledevice");
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
                            else if(string.Equals(Path.GetFileName(file), "plist.h", StringComparison.OrdinalIgnoreCase))
                            {
                                generator.Generate(targetDirectory, "plist");
                            }
                            else if (string.Equals(Path.GetFileName(file), "usbmuxd.h", StringComparison.OrdinalIgnoreCase))
                            {
                                generator.Generate(targetDirectory, "usbmuxd");
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

                        return 0;
                    });
                });

            return commandLineApplication.Execute(args);
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
