// <copyright file="MobileImageMounterClientHandle.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.MobileImageMounter
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
    public partial class MobileImageMounterClientHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        protected MobileImageMounterClientHandle() : 
                base(true)
        {
        }
        
        protected MobileImageMounterClientHandle(bool ownsHandle) : 
                base(ownsHandle)
        {
        }
        
        public static MobileImageMounterClientHandle Zero
        {
            get
            {
                return MobileImageMounterClientHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
            return (LibiMobileDevice.Instance.MobileImageMounter.mobile_image_mounter_free(this.handle) == MobileImageMounterError.Success);
        }
        
        public static MobileImageMounterClientHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            MobileImageMounterClientHandle safeHandle;
            safeHandle = new MobileImageMounterClientHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static MobileImageMounterClientHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return MobileImageMounterClientHandle.DangerousCreate(unsafeHandle, true);
        }
    }
}
