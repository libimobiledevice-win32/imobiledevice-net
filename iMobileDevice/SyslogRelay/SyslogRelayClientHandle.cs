// <copyright file="SyslogRelayClientHandle.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.SyslogRelay
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
    public partial class SyslogRelayClientHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        protected SyslogRelayClientHandle() : 
                base(true)
        {
        }
        
        protected SyslogRelayClientHandle(bool ownsHandle) : 
                base(ownsHandle)
        {
        }
        
        public static SyslogRelayClientHandle Zero
        {
            get
            {
                return SyslogRelayClientHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
            return (LibiMobileDevice.Instance.SyslogRelay.syslog_relay_client_free(this.handle) == SyslogRelayError.Success);
        }
        
        public static SyslogRelayClientHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            SyslogRelayClientHandle safeHandle;
            safeHandle = new SyslogRelayClientHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static SyslogRelayClientHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return SyslogRelayClientHandle.DangerousCreate(unsafeHandle, true);
        }
    }
}
