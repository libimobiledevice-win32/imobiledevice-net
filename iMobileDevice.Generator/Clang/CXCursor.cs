using System.Runtime.InteropServices;

namespace iMobileDevice.Generator.Clang
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXCursor
    {
        public int kind;
        public int xdata;
        public ulong data_0;
        public ulong data_1;
        public ulong data_2;
    }
}
