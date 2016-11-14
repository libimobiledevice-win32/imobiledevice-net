using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace iMobileDevice
{
    public static class NativeLibraries
    {
        public static bool LibraryFound
        {
#if !NETSTANDARD1_5
            get;
            private set;
#else
            get 
            { 
                // .NET Core has a good story for loading unmanaged assemblies - you include them in the
                // runtimes/<runtime>/native folder of the NuGet package. So we're letting .NET Core
                // handle finding the assemblies and loading them.
                return true; 
            }
#endif
        }

        public static void Load()
        {
            Load(Path.GetDirectoryName(typeof(NativeLibraries).GetTypeInfo().Assembly.Location));
        }

        public static void Load(string directory)
        {
#if !NETSTANDARD1_5
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

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    isWindows = true;
                    break;

                case PlatformID.Unix:
                    isLinux = true;
                    break;
            }

            if (isWindows)
            {
                string[] windowsLibariesToLoad = new string[]
                {
                    "msvcr110",
                    "vcruntime140",
                    "zlib",
                    "libiconv",
                    "getopt",
                    "libeay32",
                    "ssleay32",
                    "imobiledevice",
                };

                string nativeLibrariesDirectory;

                if (Environment.Is64BitProcess)
                {
                    nativeLibrariesDirectory = Path.Combine(directory, "win7-x64");
                }
                else
                {
                    nativeLibrariesDirectory = Path.Combine(directory, "win7-x86");
                }

                if (!Directory.Exists(nativeLibrariesDirectory))
                {
                    throw new ArgumentOutOfRangeException(nameof(directory), $"The directory '{directory}' does not contain a subdirectory for the current architecture. The directory '{nativeLibrariesDirectory}' does not exist.");
                }

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
            else if (isLinux)
            {
                string[] linuxLibariesToLoad = new string[]
                    {
                        "libimobiledevice",
                    };

                // Clear any value from dlerror
                NativeMethods.dlerror();

                foreach (var libraryToLoad in linuxLibariesToLoad)
                {
                    // Attempt to load the libraries. If they are not found, throw an error.
                    IntPtr result = NativeMethods.dlopen($"{libraryToLoad}.so", DlOpenFlags.RTLD_NOW);

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
                throw new NotSupportedException("imobiledevice-net is supported on Windows (.NET FX, .NET Core), Linux (Mono, .NET Core) and OS X (.NET Core)");
            }
#else
            throw new NotSupportedException("Load is supported on .NET FX and Mono only. When using .NET Core, add the runtime.*.imobiledevice-net packages to your project to add the native libraries.");
#endif
        }
    }
}
