using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace iMobileDevice
{
    /// <summary>
    /// Provides methods for retrieving the directory in which the libimobiledevice library and tools are located.
    /// </summary>
    /// <remarks>
    /// <para>
    /// To run the and tools, such as <c>iproxy</c>, we need to know where they are located.
    /// </para>
    /// <para>
    /// The libimobiledevice library and tools are delivered via the <c>imobiledevice-net</c> NuGet packages on Windows and macOS,
    /// and, as a special case, Ubuntu 16.04.
    /// On other Linux distributions, you can use the Quamotion PPA at https://launchpad.net/~quamotion/+archive/ubuntu/ppa,
    /// or compile from source.
    /// </para>
    /// <para>
    /// When running in development mode, or via <c>dotnet run</c>, .NET Core does not copy the contents of the NuGet packages to the
    /// output directory, but references them instead. In most cases, this is transparent - assemblies, managed and native, are
    /// loaded via .NET anyway, so as long as .NET knows where they are, we are good.
    /// </para>
    /// <para>
    /// For executables, that's a different story. We can't determine the path ourselves, because we don't know, say, the version number,
    /// nor is there an API in CoreCLR we can use to query the location.
    /// </para>
    /// <para>
    /// So we take a detour. The NuGet packages ship with a 'lighthouse' library which contains a <c>get_moduleFileName</c> function, which
    /// returns the location of the library. We can call <c>get_moduleFileName</c> from managed code, and determine the directory from there.
    /// </para>
    /// <para>
    /// Although the same function exists on Windows, Linux and OS X, there are some slight differences. For example, the Windows function uses
    /// Unicode, whereas the Unix ones use UTF-8.
    /// </para>
    /// </remarks>
    public static class Utilities
    {
        /// <summary>
        /// Caches the result of <see cref="GetMobileDeviceDotNetDirectory"/>, so only a single call is required.
        /// </summary>
        private static string mobileDeviceDotNetDirectory;
        private static bool sourcedFromNuGet = true;

        private static readonly bool isWindows =
#if NET45
            true;
#else
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif

        private static readonly bool isLinux =
#if NET45
            false;
#else
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
#endif

        /// <summary>
        /// Gets the directory in which the libimobiledevice library and tools are located.
        /// </summary>
        /// <returns>
        /// The directory in which the libimobiledevice library and tools are located.
        /// </returns>
        public static string GetMobileDeviceDotNetDirectory()
        {
            if (mobileDeviceDotNetDirectory == null)
            {
                try
                {
                    StringBuilder builder = new StringBuilder(512);
                    int ret = 0;

                    if (isWindows)
                    {
                        ret = GetMobileDeviceDotNetDirWin(builder, builder.Capacity);
                    }
                    else
                    {
                        ret = GetMobileDeviceDotNetDirUnix(builder, builder.Capacity);
                    }

                    if (ret != 0)
                    {
                        throw new Exception();
                    }

                    var moduleFileName = builder.ToString();
                    mobileDeviceDotNetDirectory = Path.GetDirectoryName(moduleFileName);
                }
                catch (DllNotFoundException)
                {
                    // On Linux, we also support loading the libimobiledevice-related utilities which have been
                    // installed using the package manager (usbmuxd-utils and libimobiledevice).
                    if (isLinux)
                    {
                        if (!File.Exists("/usr/bin/iproxy"))
                        {
                            throw new Exception("Could not find iproxy. Make sure that the libusbmuxd-utils package has been installed.");
                        }

                        mobileDeviceDotNetDirectory = "/usr/bin/";
                        sourcedFromNuGet = false;
                    }
                    else
                    {
                        // Not on Linux; we ship the required .dylib/.dll/.exe files on macOS & Windows as part of the NuGet package.
                        throw;
                    }
                }
            }

            return mobileDeviceDotNetDirectory;
        }

        /// <summary>
        /// Gets the full path of the <c>iproxy.exe</c> executable.
        /// </summary>
        /// <returns>
        /// The full path of the <c>iproxy.exe</c> executable.
        /// </returns>
        public static string GetProxyPath()
        {
            var executable = isWindows ? "iproxy.exe" : "iproxy";

            var proxyPath = Path.Combine(GetMobileDeviceDotNetDirectory(), executable);
            return proxyPath;
        }

        /// <summary>
        /// Returns the filename of the <c>mobiledevice-net-lighthouse-x64</c> module.
        /// </summary>
        /// <param name="filename">
        /// A <see cref="StringBuilder"/> which will receive the filename of the module.
        /// </param>
        /// <param name="length">
        /// The maximum capacity of the module.
        /// </param>
        /// <returns>
        /// <c>0</c> if the operation completed successfully, or an error code otherwise.
        /// </returns>
        [DllImport("imobiledevice-net-lighthouse", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "get_moduleFileName")]
        private static extern int GetMobileDeviceDotNetDirUnix(StringBuilder filename, int length);

        /// <summary>
        /// Returns the filename of the <c>mobiledevice-net-lighthouse-x64</c> module.
        /// </summary>
        /// <param name="filename">
        /// A <see cref="StringBuilder"/> which will receive the filename of the module.
        /// </param>
        /// <param name="length">
        /// The maximum capacity of the module.
        /// </param>
        /// <returns>
        /// <c>0</c> if the operation completed successfully, or an error code otherwise.
        /// </returns>
        [DllImport("imobiledevice-net-lighthouse", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "get_moduleFileName")]
        private static extern int GetMobileDeviceDotNetDirWin(StringBuilder filename, int length);
    }
}
