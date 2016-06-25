// <copyright file="DiagnosticsRelayClientHandle.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.DiagnosticsRelay
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
    public partial class DiagnosticsRelayClientHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        protected DiagnosticsRelayClientHandle() : 
                base(true)
        {
        }
        
        protected DiagnosticsRelayClientHandle(bool ownsHandle) : 
                base(ownsHandle)
        {
        }
        
        public static DiagnosticsRelayClientHandle Zero
        {
            get
            {
                return DiagnosticsRelayClientHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
            return (LibiMobileDevice.Instance.DiagnosticsRelay.diagnostics_relay_client_free(this.handle) == DiagnosticsRelayError.Success);
        }
        
        public static DiagnosticsRelayClientHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            DiagnosticsRelayClientHandle safeHandle;
            safeHandle = new DiagnosticsRelayClientHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static DiagnosticsRelayClientHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return DiagnosticsRelayClientHandle.DangerousCreate(unsafeHandle, true);
        }
    }
}
