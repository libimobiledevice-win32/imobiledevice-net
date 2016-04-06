using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace iMobileDevice
{
    public abstract class NativeStringArrayMarshaler : ICustomMarshaler
    {
        public void CleanUpManagedData(object ManagedObj)
        {
            return;
        }

        public abstract void CleanUpNativeData(IntPtr nativeData);

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
            List<string> values = new List<string>();

            if (nativeData != IntPtr.Zero)
            {
                IntPtr arrayIndex = nativeData;

                while (true)
                {
                    IntPtr stringPointer = Marshal.ReadIntPtr(arrayIndex);

                    if (stringPointer == IntPtr.Zero)
                    {
                        break;
                    }

                    string value = Marshal.PtrToStringAnsi(stringPointer);

                    if (value == string.Empty)
                    {
                        break;
                    }

                    values.Add(value);
                    arrayIndex += IntPtr.Size;
                }
            }

            return new ReadOnlyCollection<string>(values);
        }
    }
}
