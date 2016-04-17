using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using iMobileDevice.Pinvoke;
using System.IO;
using System.Text;

namespace iMobileDevice
{
    public class NativeStringMarshaler : ICustomMarshaler
    {
        public static ICustomMarshaler GetInstance(string cookie)
        {
            return new NativeStringMarshaler();
        }

        public void CleanUpManagedData(object ManagedObj)
        {
            return;
        }

        public void CleanUpNativeData(IntPtr nativeData)
        {
            LibiMobileDevice.Instance.Pinvoke.pinvoke_free_string(nativeData).ThrowOnError();
        }

        public int GetNativeDataSize()
        {
            return -1;
        }

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            return IntPtr.Zero;
        }

        public object MarshalNativeToManaged(IntPtr nativeData)
        {
            // Get the the size of the string
            string value = Utf8Marshal.PtrToStringUtf8(nativeData);
            return value;
        }
    }
}
