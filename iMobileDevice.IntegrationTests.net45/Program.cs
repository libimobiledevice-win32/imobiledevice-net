using iMobileDevice.iDevice;
using iMobileDevice.iDeviceActivation;
using iMobileDevice.Lockdown;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace iMobileDevice.IntegrationTests.net45
{
    class Program
    {
        static int Main(string[] args)
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(typeof(NativeLibraries).Assembly.Location);
            Console.WriteLine($"Running tests for imobiledevice-net");
            Console.WriteLine($"- Version {fvi.FileVersion}");
            Console.WriteLine($"- .NET 4.5");

            if (Environment.Is64BitProcess)
            {
                Console.WriteLine("- 64-bit process");
            }
            else
            {
                Console.WriteLine("- 32-bit process");
            }

            // Make sure a call to NativeLibraries.Load() works
            Console.WriteLine("Loading native libraries");
            NativeLibraries.Load();

            LibiMobileDevice api = new LibiMobileDevice();

            // Test a method exposed in libplist
            Console.WriteLine("libplist - Creating a property list");
            using (var plist = api.Plist.plist_new_dict())
            {
            }

            // Test a method exposed in libusbmuxd
            Console.WriteLine("libusbmuxd - Set debug level");
            api.Usbmuxd.libusbmuxd_set_debug_level(1);

            // Test a method exposed in libimobiledevice
            Console.WriteLine("libimobiledevice - Creating installation proxy options");
            using (var options = api.InstallationProxy.instproxy_client_options_new())
            {
            }

            // List all devices
            Console.WriteLine("libimobiledevice - Listing devices");
            ListDevices();

            // Test a method exposed in libideviceactivation
            Console.WriteLine("libideviceactivation - Creating a new activation request");
            iDeviceActivationRequestHandle request;
            api.iDeviceActivation.idevice_activation_request_new(iDeviceActivationClientType.ClientItunes, out request);
            request.Dispose();

            // Make sure usbmuxd, iproxy.exe exist
            Console.WriteLine("utilities - making sure iproxy can be found");
            if (!File.Exists(Utilities.GetProxyPath()))
            {
                return -1;
            }

            Console.WriteLine("utilities - making sure usbmuxd can be found");
            if (!File.Exists(Utilities.GetUsbMuxdPath()))
            {
                return -1;
            }

            return 0;
        }

        static void ListDevices()
        {
            ReadOnlyCollection<string> udids;
            int count = 0;

            var idevice = LibiMobileDevice.Instance.iDevice;
            var lockdown = LibiMobileDevice.Instance.Lockdown;

            var ret = idevice.idevice_get_device_list(out udids, ref count);

            if (ret == iDeviceError.NoDevice)
            {
                // Not actually an error in our case
                return;
            }

            ret.ThrowOnError();

            // Get the device name
            foreach (var udid in udids)
            {
                iDeviceHandle deviceHandle;
                idevice.idevice_new(out deviceHandle, udid).ThrowOnError();

                LockdownClientHandle lockdownHandle;
                lockdown.lockdownd_client_new_with_handshake(deviceHandle, out lockdownHandle, "Quamotion").ThrowOnError();

                string deviceName;
                lockdown.lockdownd_get_device_name(lockdownHandle, out deviceName).ThrowOnError();

                deviceHandle.Dispose();
                lockdownHandle.Dispose();
            }
        }
    }
}
