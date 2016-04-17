using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace iMobileDevice
{
    public static class Utf8Marshal
    {
        public static string PtrToStringUtf8(IntPtr value)
        {
            if (value == IntPtr.Zero)
            {
                return null;
            }

            ulong size = 0;
            LibiMobileDevice.Instance.Pinvoke.pinvoke_get_string_length(value, out size);
            byte[] data = new byte[size];
            Marshal.Copy(value, data, 0, (int)size);
            string result = Encoding.UTF8.GetString(data);
            return result;
        }

        public static IntPtr StringToHGlobalUtf8(string value)
        {
            if (value == null)
            {
                return IntPtr.Zero;
            }

            byte[] data = Encoding.UTF8.GetBytes(value);

            // Make sure the string is 0-terminated
            IntPtr handle = Marshal.AllocHGlobal(data.Length + 1);
            Marshal.Copy(data, 0, handle, data.Length);

            return handle;
        }
    }
}
