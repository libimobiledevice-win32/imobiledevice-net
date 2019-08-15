using System.Runtime.InteropServices;

namespace iMobileDevice.Generator.Clang
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXString
    {
        public void* data;
        public uint private_flags;
    }
}
