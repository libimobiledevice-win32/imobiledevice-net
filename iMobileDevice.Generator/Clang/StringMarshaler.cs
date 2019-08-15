using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace iMobileDevice.Generator.Clang
{
    internal class StringMarshaler : ICustomMarshaler
    {
        protected static readonly StringMarshaler STATIC_INSTANCE = new StringMarshaler();

        public static ICustomMarshaler GetInstance(string cookie)
        {
            return STATIC_INSTANCE;
        }

        public object MarshalNativeToManaged(IntPtr pNativeData)
        {
            var data = new MemoryStream(128);
            var offset = 0;
            while (true)
            {
                var b = Marshal.ReadByte(pNativeData, offset);
                if (b == 0)
                {
                    break;
                }
                data.WriteByte(b);
                offset++;
            }
            return Encoding.UTF8.GetString(data.GetBuffer(), 0, (int)data.Length);
        }

        public IntPtr MarshalManagedToNative(object managedObj)
        {
            var str = (string)managedObj;
            var bytes = Encoding.UTF8.GetBytes(str);
            var ptr = Marshal.AllocCoTaskMem(bytes.Length + 1);
            Marshal.Copy(bytes, 0, ptr, bytes.Length);
            Marshal.WriteByte(ptr, bytes.Length, 0);
            return ptr;
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            // Nothing to do
        }

        public void CleanUpManagedData(object managedObj)
        {
            // Nothing to do
        }

        public int GetNativeDataSize()
        {
            return -1;
        }
    }
}
