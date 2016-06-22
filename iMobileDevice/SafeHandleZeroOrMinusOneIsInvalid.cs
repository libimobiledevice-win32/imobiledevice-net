using System;
using System.Runtime.InteropServices;
using System.Security;

#if NETSTANDARD1_5
namespace Microsoft.Win32.SafeHandles
{
    // Class of safe handle which uses 0 or -1 as an invalid handle.
    [SecurityCritical]
    public abstract class SafeHandleZeroOrMinusOneIsInvalid : SafeHandle
    {
        protected SafeHandleZeroOrMinusOneIsInvalid(bool ownsHandle)
            : base(IntPtr.Zero, ownsHandle)
        {
        }

        public override bool IsInvalid
        {
            get
            {
                return handle == IntPtr.Zero || handle == new IntPtr(-1);
            }
        }
    }
}
#endif
