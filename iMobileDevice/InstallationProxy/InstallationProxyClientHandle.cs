// <copyright file="InstallationProxyClientHandle.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.InstallationProxy
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
    public partial class InstallationProxyClientHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        private ILibiMobileDevice api;
        
        protected InstallationProxyClientHandle() : 
                base(true)
        {
        }
        
        protected InstallationProxyClientHandle(bool ownsHandle) : 
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
        
        public static InstallationProxyClientHandle Zero
        {
            get
            {
                return InstallationProxyClientHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
            return (this.Api.InstallationProxy.instproxy_client_free(this.handle) == InstallationProxyError.Success);
        }
        
        public static InstallationProxyClientHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            InstallationProxyClientHandle safeHandle;
            safeHandle = new InstallationProxyClientHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static InstallationProxyClientHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return InstallationProxyClientHandle.DangerousCreate(unsafeHandle, true);
        }
    }
}
