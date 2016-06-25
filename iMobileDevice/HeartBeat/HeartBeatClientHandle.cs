// <copyright file="HeartBeatClientHandle.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.HeartBeat
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
    public partial class HeartBeatClientHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        protected HeartBeatClientHandle() : 
                base(true)
        {
        }
        
        protected HeartBeatClientHandle(bool ownsHandle) : 
                base(ownsHandle)
        {
        }
        
        public static HeartBeatClientHandle Zero
        {
            get
            {
                return HeartBeatClientHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
            return (LibiMobileDevice.Instance.HeartBeat.heartbeat_client_free(this.handle) == HeartBeatError.Success);
        }
        
        public static HeartBeatClientHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            HeartBeatClientHandle safeHandle;
            safeHandle = new HeartBeatClientHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static HeartBeatClientHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return HeartBeatClientHandle.DangerousCreate(unsafeHandle, true);
        }
    }
}
