// <copyright file="LockdownServiceDescriptorHandle.cs" company="Quamotion">
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
    public partial class LockdownServiceDescriptorHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        private ILibiMobileDevice api;
        
        protected LockdownServiceDescriptorHandle() : 
                base(true)
        {
        }
        
        protected LockdownServiceDescriptorHandle(bool ownsHandle) : 
                base(ownsHandle)
        {
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
        
        public static LockdownServiceDescriptorHandle Zero
        {
            get
            {
                return LockdownServiceDescriptorHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
            return (this.Api.Lockdown.lockdownd_service_descriptor_free(this.handle) == LockdownError.Success);
        }
        
        public static LockdownServiceDescriptorHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            LockdownServiceDescriptorHandle safeHandle;
            safeHandle = new LockdownServiceDescriptorHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static LockdownServiceDescriptorHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return LockdownServiceDescriptorHandle.DangerousCreate(unsafeHandle, true);
        }
    }
}
