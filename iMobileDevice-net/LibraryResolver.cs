using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace iMobileDevice
{
    internal static class LibraryResolver
    {
        static LibraryResolver()
        {
#if !NETCOREAPP2_0 && !NET45
            NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), DllImportResolver);
#endif
        }

        public static void EnsureRegistered()
        {
            // Dummy call to trigger the static constructor
        }

#if !NETCOREAPP2_0 && !NET45
        private static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName == Plist.PlistNativeMethods.LibraryName)
            {
                return LoadPlistLibrary();
            }

            if (libraryName == Usbmuxd.UsbmuxdNativeMethods.LibraryName)
            {
                return LoadUsbmuxdLibrary();
            }

            if (libraryName == iDevice.iDeviceNativeMethods.LibraryName)
            {
                return LoadMobileDeviceLibrary();
            }

            if (libraryName == iDeviceActivation.iDeviceActivationNativeMethods.LibraryName)
            {
                return LoadDeviceActivationLibrary();
            }

            return IntPtr.Zero;
        }

        private static IntPtr LoadPlistLibrary()
        {
            IntPtr lib = IntPtr.Zero;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (NativeLibrary.TryLoad("libplist.so.3", out lib))
                {
                    return lib;
                }
                else if (NativeLibrary.TryLoad("libplist.so", out lib))
                {
                    return lib;
                }
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                if (NativeLibrary.TryLoad("libplist.dylib", out lib))
                {
                    return lib;
                }
            }

            return IntPtr.Zero;
        }

        private static IntPtr LoadUsbmuxdLibrary()
        {
            IntPtr lib = IntPtr.Zero;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (NativeLibrary.TryLoad("libusbmuxd.so.4", out lib))
                {
                    return lib;
                }
                else if (NativeLibrary.TryLoad("libusbmuxd.so", out lib))
                {
                    return lib;
                }
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                if (NativeLibrary.TryLoad("libusbmuxd.dylib", out lib))
                {
                    return lib;
                }
            }

            return IntPtr.Zero;
        }

        private static IntPtr LoadMobileDeviceLibrary()
        {
            IntPtr lib = IntPtr.Zero;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (NativeLibrary.TryLoad("libimobiledevice.so.6", out lib))
                {
                    return lib;
                }
                else if (NativeLibrary.TryLoad("libimobiledevice.so", out lib))
                {
                    return lib;
                }
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                if (NativeLibrary.TryLoad("libimobiledevice.dylib", out lib))
                {
                    return lib;
                }
            }

            return IntPtr.Zero;
        }

        private static IntPtr LoadDeviceActivationLibrary()
        {
            IntPtr lib = IntPtr.Zero;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (NativeLibrary.TryLoad("libideviceactivation.so.2", out lib))
                {
                    return lib;
                }
                else if (NativeLibrary.TryLoad("libideviceactivation.so", out lib))
                {
                    return lib;
                }
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                if (NativeLibrary.TryLoad("libideviceactivation.dylib", out lib))
                {
                    return lib;
                }
            }

            return IntPtr.Zero;
        }
#endif
    }
}