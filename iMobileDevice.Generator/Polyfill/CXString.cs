using System.Runtime.InteropServices;

namespace iMobileDevice.Generator.Polyfill
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXString
    {
        public void* data;
        public uint private_flags;

        public override string ToString()
        {
            using (String value = new String(this))
            {
                return value.ToString();
            }
        }
    }
}
