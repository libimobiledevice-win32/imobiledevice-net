using System;
using System.Runtime.InteropServices;

namespace iMobileDevice.Generator.Polyfill
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct CXCursor
    {
        public CXCursorKind kind;
        public int xdata;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public ulong[] data;
    }
}
