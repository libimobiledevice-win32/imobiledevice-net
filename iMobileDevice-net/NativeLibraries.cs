﻿using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace iMobileDevice
{
    /// <summary>
    /// Provides access to the native libimobiledevice libraries. Theses are the .dll, .so or .dylib files libimobiledevice-net uses.
    /// </summary>
    public static class NativeLibraries
    {
        private const string WindowsRuntime64Bit = "win-x64";
        private const string WindowsRuntime32Bit = "win-x86";

        /// <summary>
        /// Gets or sets a value indicating whether the native libraries have been found. This value is only relevant on the full .NET Framework;
        /// it is always set to <see langword="true"/> on .NET Core.
        /// </summary>
        public static bool LibraryFound
        {
            get;
            private set;
        }
#if NETCOREAPP
            // .NET Core has a good story for loading unmanaged assemblies - you include them in the
            // runtimes/<runtime>/native folder of the NuGet package. So we're letting .NET Core
            // handle finding the assemblies and loading them.
            = true;
#endif

        /// <summary>
        /// Loads the native libraries.
        /// </summary>
        public static void Load()
        {
            if (LibraryFound)
            {
                return;
            }

            Load(Path.GetDirectoryName(typeof(NativeLibraries).GetTypeInfo().Assembly.Location));
        }

        /// <summary>
        /// Loads the native libraries.
        /// </summary>
        /// <param name="directory">
        /// The path in which the native libraries are located.
        /// </param>
        public static void Load(string directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            if (!Directory.Exists(directory))
            {
                throw new ArgumentOutOfRangeException(nameof(directory), $"The directory '{directory}' does not exist.");
            }

            // When the library is first called, call LoadLibrary with the full path to the
            // path of the various libaries, to make sure they are loaded from the exact
            // path we specify.

            // Any load errors would also be caught by us here, making it easier to troubleshoot.

            bool isWindows = false;
            bool isLinux = false;
            bool isMacOs = false;

#if NETCOREAPP
            isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            isMacOs = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#else
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    isWindows = true;
                    break;

                case PlatformID.Unix:
                    isLinux = true;
                    break;
            }
#endif

            if (isWindows)
            {
                string[] windowsLibariesToLoad = new string[]
                {
                    "imobiledevice",
                    "ideviceactivation",
                };

                string nativeLibrariesDirectory = directory;

                if (Environment.Is64BitProcess && Directory.Exists(Path.Combine(directory, WindowsRuntime64Bit)))
                {
                    nativeLibrariesDirectory = Path.Combine(directory, WindowsRuntime64Bit);
                }
                else if (Directory.Exists(Path.Combine(directory, WindowsRuntime32Bit)))
                {
                    nativeLibrariesDirectory = Path.Combine(directory, WindowsRuntime32Bit);
                }

                if (!Directory.Exists(nativeLibrariesDirectory))
                {
                    throw new ArgumentOutOfRangeException(nameof(directory), $"The directory '{nativeLibrariesDirectory}' does not exist.");
                }

                // Do a safety check first, and make sure the core files actually exist.
                foreach (var libraryToLoad in windowsLibariesToLoad)
                {
                    string path = Path.Combine(nativeLibrariesDirectory, string.Format("{0}.dll", libraryToLoad));

                    // Attempt to load the libraries. If they are not found, throw an error.
                    // See also http://blogs.msdn.com/b/adam_nathan/archive/2003/04/25/56643.aspx for
                    // more information about GetLastWin32Error
                    if (!File.Exists(path))
                    {
                        throw new FileNotFoundException($"Could not load the library '{libraryToLoad}' at '{path}', because the file does not exist", path);
                    }
                }

                // Add the directory to the search path, and try to load the libraries one by one.
                NativeMethods.SetDllDirectory(nativeLibrariesDirectory);

                foreach (var libraryToLoad in windowsLibariesToLoad)
                {
                    string path = Path.Combine(nativeLibrariesDirectory, string.Format("{0}.dll", libraryToLoad));

                    IntPtr result = NativeMethods.LoadLibrary(path);
                    if (result == IntPtr.Zero)
                    {
                        var lastError = Marshal.GetLastWin32Error();
                        var error = new Win32Exception(lastError, $"Could not load the library '{libraryToLoad}' at '{path}'. The error code was {lastError}");
                        throw error;
                    }
                }

                LibraryFound = true;
            }
            else if (isLinux || isMacOs)
            {
                string suffix = isLinux ? "so" : "dylib";

                string[] linuxLibariesToLoad = new string[]
                    {
                        "libimobiledevice",
                    };

                // Clear any value from dlerror
                NativeMethods.dlerror();

                foreach (var libraryToLoad in linuxLibariesToLoad)
                {
                    // Attempt to load the libraries. If they are not found, throw an error.
                    IntPtr result = NativeMethods.dlopen($"{libraryToLoad}.{suffix}", DlOpenFlags.RTLD_NOW);

                    if (result == IntPtr.Zero)
                    {
                        var lastError = NativeMethods.dlerror();
                        var errorMessage = Marshal.PtrToStringAnsi(lastError);

                        return;
                    }
                }

                LibraryFound = true;
            }
            else
            {
                throw new NotSupportedException("imobiledevice-net is supported on Windows (.NET FX, .NET Core), Linux (.NET Core) and OS X (.NET Core)");
            }
        }
    }
}
