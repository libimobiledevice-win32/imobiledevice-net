// <copyright file="RestoreClientHandle.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Restore
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
    public partial class RestoreClientHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        protected RestoreClientHandle() : 
                base(true)
        {
        }
        
        protected RestoreClientHandle(bool ownsHandle) : 
                base(ownsHandle)
        {
        }
        
        public static RestoreClientHandle Zero
        {
            get
            {
                return RestoreClientHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
            return (LibiMobileDevice.Instance.Restore.restored_client_free(this.handle) == RestoreError.Success);
        }
        
        public static RestoreClientHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            RestoreClientHandle safeHandle;
            safeHandle = new RestoreClientHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static RestoreClientHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return RestoreClientHandle.DangerousCreate(unsafeHandle, true);
        }
    }
}
