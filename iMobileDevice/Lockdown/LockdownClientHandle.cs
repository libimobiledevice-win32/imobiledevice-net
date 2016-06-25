// <copyright file="LockdownClientHandle.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Lockdown
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
    public partial class LockdownClientHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        protected LockdownClientHandle() : 
                base(true)
        {
        }
        
        protected LockdownClientHandle(bool ownsHandle) : 
                base(ownsHandle)
        {
        }
        
        public static LockdownClientHandle Zero
        {
            get
            {
                return LockdownClientHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
            return (LibiMobileDevice.Instance.Lockdown.lockdownd_client_free(this.handle) == LockdownError.Success);
        }
        
        public static LockdownClientHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            LockdownClientHandle safeHandle;
            safeHandle = new LockdownClientHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static LockdownClientHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return LockdownClientHandle.DangerousCreate(unsafeHandle, true);
        }
    }
}
