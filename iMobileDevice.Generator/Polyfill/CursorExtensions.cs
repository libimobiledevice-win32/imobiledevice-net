using Core.Clang;
using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace iMobileDevice.Generator.Polyfill
{
    internal static class CursorExtensions
    {
        public static unsafe CXCursor ToCXCursor(this Cursor cursor)
        {
            var structProperty = typeof(Cursor).GetProperty("Struct", BindingFlags.Instance | BindingFlags.NonPublic);
            var nativeCursor = structProperty.GetValue(cursor);

            var data = nativeCursor.GetType().GetField("data").GetValue(nativeCursor);
            ulong[] data2 = new ulong[3];

            GCHandle hdl = GCHandle.Alloc(data, GCHandleType.Pinned);
            ulong* native = (ulong*)hdl.AddrOfPinnedObject();
            for(int i = 0; i < 3; i++)
            {
                data2[i] = native[i];

            }
            hdl.Free();

            return new CXCursor()
            {
                kind = (CXCursorKind)nativeCursor.GetType().GetField("kind").GetValue(nativeCursor),
                data = data2,
                xdata = (int)nativeCursor.GetType().GetField("xdata").GetValue(nativeCursor),
            };
        }
    }
}
