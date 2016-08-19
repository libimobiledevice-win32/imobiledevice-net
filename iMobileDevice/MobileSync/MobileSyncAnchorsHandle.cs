// <copyright file="MobileSyncAnchorsHandle.cs" company="Quamotion">
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
    public partial class MobileSyncAnchorsHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        private ILibiMobileDevice api;
        
        protected MobileSyncAnchorsHandle() : 
                base(true)
        {
        }
        
        protected MobileSyncAnchorsHandle(bool ownsHandle) : 
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
        
        public static MobileSyncAnchorsHandle Zero
        {
            get
            {
                return MobileSyncAnchorsHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
            return (this.Api.MobileSync.mobilesync_anchors_free(this.handle) == MobileSyncError.Success);
        }
        
        public static MobileSyncAnchorsHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            MobileSyncAnchorsHandle safeHandle;
            safeHandle = new MobileSyncAnchorsHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static MobileSyncAnchorsHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return MobileSyncAnchorsHandle.DangerousCreate(unsafeHandle, true);
        }
    }
}
