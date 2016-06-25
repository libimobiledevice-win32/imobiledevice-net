// <copyright file="DebugServerCommandHandle.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.DebugServer
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
    public partial class DebugServerCommandHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        protected DebugServerCommandHandle() : 
                base(true)
        {
        }
        
        protected DebugServerCommandHandle(bool ownsHandle) : 
                base(ownsHandle)
        {
        }
        
        public static DebugServerCommandHandle Zero
        {
            get
            {
                return DebugServerCommandHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
            return (LibiMobileDevice.Instance.DebugServer.debugserver_command_free(this.handle) == DebugServerError.Success);
        }
        
        public static DebugServerCommandHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            DebugServerCommandHandle safeHandle;
            safeHandle = new DebugServerCommandHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static DebugServerCommandHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return DebugServerCommandHandle.DangerousCreate(unsafeHandle, true);
        }
    }
}
