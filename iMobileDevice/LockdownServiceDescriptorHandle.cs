using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace iMobileDevice.Lockdown
{
    public partial class LockdownServiceDescriptorHandle
    {
        public virtual LockdownServiceDescriptor Value
        {
            get
            {
                if (this.IsInvalid)
                {
                    throw new InvalidOperationException("Cannot get the value of a Lockdown Service Descriptor Handle because the handle is invalid.");
                }

                if (this.IsClosed)
                {
                    throw new ObjectDisposedException(nameof(LockdownServiceDescriptorHandle));
                }

                LockdownServiceDescriptor value =
                    (LockdownServiceDescriptor)Marshal.PtrToStructure(
                        this.DangerousGetHandle(),
                        typeof(LockdownServiceDescriptor));

                return value;
            }
        }
    }
}
