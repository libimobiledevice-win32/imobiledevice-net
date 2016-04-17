using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace iMobileDevice
{
    public class NativeStringArrayMarshaler : ICustomMarshaler
    {
        private readonly Collection<IntPtr> allocatedHere = new Collection<IntPtr>();

        public void CleanUpManagedData(object ManagedObj)
        {
            return;
        }

        public virtual void CleanUpNativeData(IntPtr nativeData)
        {
            if (allocatedHere.Contains(nativeData))
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

                var handle = GCHandle.FromIntPtr(nativeData);
                handle.Free();
            }
        }

        public int GetNativeDataSize()
        {
            return -1;
        }

        public IntPtr MarshalManagedToNative(object managedObj)
        {
            var values = managedObj as ReadOnlyCollection<string>();

            if (values == null)
            {
                return IntPtr.Zero;
            }

            IntPtr[] unmanagedArray = new IntPtr[values.Count + 1];

            for (int i = 0; i < unmanagedArray.Length; i++)
            {
                unmanagedArray[i] = Marshal.StringToHGlobalAnsi(values[i]);
            }

            unmanagedArray[values.Count] = IntPtr.Zero;

            GCHandle unmanagedHandle = GCHandle.Alloc(unmanagedArray, GCHandleType.Pinned);
            IntPtr unmanagedValue = GCHandle.ToIntPtr(unmanagedHandle);

            allocatedHere.Add(unmanagedValue);
            return unmanagedValue;
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
