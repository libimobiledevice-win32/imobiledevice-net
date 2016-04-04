using iMobileDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MobileDevice.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            // This demo application will use the libimobiledevice API to list all iOS devices currently
            // connected to this PC.

            // First, we need to make sure the unmanaged (native) libimobiledevice libraries are loaded correctly
            NativeLibraries.Load();

            IntPtr devices = IntPtr.Zero;
            int count = 0;

            var idevice = new iDeviceApi();

            var ret = idevice.idevice_get_device_list(ref devices, ref count);

            if (ret == iDeviceError.NoDevice)
            {
                // Not actually an error in our case
                return;
            }

            // MobileDevice.CheckError(ret);

            string[] udids = new string[count];

            for (int i = 0; i < count; i++)
            {
                var udid = Marshal.PtrToStringAnsi(Marshal.ReadIntPtr(devices + i * IntPtr.Size));
                udids[i] = udid;
            }

            idevice.idevice_device_list_free(devices);

            // Get the device name
            foreach (var udid in udids)
            {
                iDeviceHandle deviceHandle;
                var device = idevice.idevice_new(out deviceHandle, udid);

                var lockdown = new LockdownApi();
                LockdownClientHandle lockdownHandle;
                lockdown.lockdownd_client_new_with_handshake(deviceHandle, out lockdownHandle, "Quamotion");

                IntPtr deviceNamePtr = IntPtr.Zero;
                lockdown.lockdownd_get_device_name(lockdownHandle, ref deviceNamePtr);

                var deviceName = Marshal.PtrToStringAnsi(deviceNamePtr);
                lockdown.lockdownd_client_free(lockdownHandle);
                idevice.idevice_free(deviceHandle);
            }
        }
    }
}
