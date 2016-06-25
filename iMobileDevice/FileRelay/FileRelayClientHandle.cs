// <copyright file="FileRelayClientHandle.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.FileRelay
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
    public partial class FileRelayClientHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        protected FileRelayClientHandle() : 
                base(true)
        {
        }
        
        protected FileRelayClientHandle(bool ownsHandle) : 
                base(ownsHandle)
        {
        }
        
        public static FileRelayClientHandle Zero
        {
            get
            {
                return FileRelayClientHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
            return (LibiMobileDevice.Instance.FileRelay.file_relay_client_free(this.handle) == FileRelayError.Success);
        }
        
        public static FileRelayClientHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            FileRelayClientHandle safeHandle;
            safeHandle = new FileRelayClientHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static FileRelayClientHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return FileRelayClientHandle.DangerousCreate(unsafeHandle, true);
        }
    }
}
