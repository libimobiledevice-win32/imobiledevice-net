// <copyright file="MobileactivationClientHandle.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Mobileactivation
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
#if !NETSTANDARD1_5
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
#endif
#if !NETSTANDARD1_5
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
#endif
    public partial class MobileactivationClientHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        private string creationStackTrace;
        
        private ILibiMobileDevice api;
        
        protected MobileactivationClientHandle() : 
                base(true)
        {
            this.creationStackTrace = System.Environment.StackTrace;
        }
        
        protected MobileactivationClientHandle(bool ownsHandle) : 
                base(ownsHandle)
        {
            this.creationStackTrace = System.Environment.StackTrace;
        }
        
        public ILibiMobileDevice Api
        {
            get
            {
                return this.api;
            }
            set
            {
                this.api = value;
            }
        }
        
        public static MobileactivationClientHandle Zero
        {
            get
            {
                return MobileactivationClientHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
#if !NETSTANDARD1_5
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
#endif
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1} using {2}. This object was created at {3}", this.GetType().Name, this.handle, this.Api, this.creationStackTrace);
            return (this.Api.Mobileactivation.mobileactivation_client_free(this.handle) == MobileactivationError.Success);
        }
        
        public static MobileactivationClientHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            MobileactivationClientHandle safeHandle;
            safeHandle = new MobileactivationClientHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static MobileactivationClientHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return MobileactivationClientHandle.DangerousCreate(unsafeHandle, true);
        }
        
        public override string ToString()
        {
            return string.Format("{0} ({1})", this.handle, "MobileactivationClientHandle");
        }
        
        public override bool Equals(object obj)
        {
            if (((obj != null) & (obj.GetType() == typeof(MobileactivationClientHandle))))
            {
                return ((MobileactivationClientHandle)obj).handle.Equals(this.handle);
            }
            else
            {
                return false;
            }
        }
        
        public override int GetHashCode()
        {
            return this.handle.GetHashCode();
        }
    }
}
