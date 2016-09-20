// <copyright file="ServiceClientHandle.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Service
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
#if !NETSTANDARD1_5
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
#endif
#if !NETSTANDARD1_5
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
#endif
    public partial class ServiceClientHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        private string creationStackTrace;
        
        private ILibiMobileDevice api;
        
        protected ServiceClientHandle() : 
                base(true)
        {
            this.creationStackTrace = System.Environment.StackTrace;
        }
        
        protected ServiceClientHandle(bool ownsHandle) : 
                base(ownsHandle)
        {
            this.creationStackTrace = System.Environment.StackTrace;
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
        
        public static ServiceClientHandle Zero
        {
            get
            {
                return ServiceClientHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
#if !NETSTANDARD1_5
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
#endif
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1} using {2}. This object was created at {3}", this.GetType().Name, this.handle, this.Api, this.creationStackTrace);
            return (this.Api.Service.service_client_free(this.handle) == ServiceError.Success);
        }
        
        public static ServiceClientHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            ServiceClientHandle safeHandle;
            safeHandle = new ServiceClientHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static ServiceClientHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return ServiceClientHandle.DangerousCreate(unsafeHandle, true);
        }
        
        public override string ToString()
        {
            return string.Format("{0} ({1})", this.handle, "ServiceClientHandle");
        }
        
        public override bool Equals(object obj)
        {
            if (((obj != null) & (obj.GetType() == typeof(ServiceClientHandle))))
            {
                return ((ServiceClientHandle)obj).handle.Equals(this.handle);
            }
            else
            {
                return false;
            }
        }
        
        public override int GetHashCode()
        {
            return this.handle.GetHashCode();
        }
    }
}
