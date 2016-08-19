// <copyright file="MobileSyncClientHandle.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.MobileSync
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
    public partial class MobileSyncClientHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        private ILibiMobileDevice api;
        
        protected MobileSyncClientHandle() : 
                base(true)
        {
        }
        
        protected MobileSyncClientHandle(bool ownsHandle) : 
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
        
        public static MobileSyncClientHandle Zero
        {
            get
            {
                return MobileSyncClientHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
            return (this.Api.MobileSync.mobilesync_client_free(this.handle) == MobileSyncError.Success);
        }
        
        public static MobileSyncClientHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            MobileSyncClientHandle safeHandle;
            safeHandle = new MobileSyncClientHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static MobileSyncClientHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return MobileSyncClientHandle.DangerousCreate(unsafeHandle, true);
        }
    }
}
