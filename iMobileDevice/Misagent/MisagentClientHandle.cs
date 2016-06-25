// <copyright file="MisagentClientHandle.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Misagent
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
    public partial class MisagentClientHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        protected MisagentClientHandle() : 
                base(true)
        {
        }
        
        protected MisagentClientHandle(bool ownsHandle) : 
                base(ownsHandle)
        {
        }
        
        public static MisagentClientHandle Zero
        {
            get
            {
                return MisagentClientHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
            return (LibiMobileDevice.Instance.Misagent.misagent_client_free(this.handle) == MisagentError.Success);
        }
        
        public static MisagentClientHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            MisagentClientHandle safeHandle;
            safeHandle = new MisagentClientHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static MisagentClientHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return MisagentClientHandle.DangerousCreate(unsafeHandle, true);
        }
    }
}
