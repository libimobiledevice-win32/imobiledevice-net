using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace iMobileDevice
{
    public class NativeStringArrayMarshaler : ICustomMarshaler
    {
        public static ICustomMarshaler GetInstance(string cookie)
        {
            return new NativeStringArrayMarshaler();
        }

        public void CleanUpManagedData(object ManagedObj)
        {
            return;
        }

        public virtual void CleanUpNativeData(IntPtr nativeData)
        {
            // Free all the individual strings
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

                    Marshal.FreeHGlobal(stringPointer);
                    arrayIndex += IntPtr.Size;
                }
            }

            Marshal.FreeHGlobal(nativeData);
        }

        public int GetNativeDataSize()
        {
            return -1;
        }

        public IntPtr MarshalManagedToNative(object managedObj)
        {
            var values = managedObj as ReadOnlyCollection<string>;

            if (values == null)
            {
                return IntPtr.Zero;
            }

            IntPtr[] unmanagedArray = new IntPtr[values.Count + 1];

            for (int i = 0; i < values.Count; i++)
            {
                unmanagedArray[i] = Utf8Marshal.StringToHGlobalUtf8(values[i]);
            }

            unmanagedArray[values.Count] = IntPtr.Zero;

            IntPtr unmanagedData = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(IntPtr)) * unmanagedArray.Length);
            Marshal.Copy(unmanagedArray, 0, unmanagedData, unmanagedArray.Length);

            return unmanagedData;
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

                    string value = Utf8Marshal.PtrToStringUtf8(stringPointer);

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
