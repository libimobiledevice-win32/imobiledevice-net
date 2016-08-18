// <copyright file="SpringBoardServicesClientHandle.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.SpringBoardServices
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
    public partial class SpringBoardServicesClientHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        private ILibiMobileDevice api;
        
        protected SpringBoardServicesClientHandle() : 
                base(true)
        {
        }
        
        protected SpringBoardServicesClientHandle(bool ownsHandle) : 
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
        
        public static SpringBoardServicesClientHandle Zero
        {
            get
            {
                return SpringBoardServicesClientHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
            return (this.Api.SpringBoardServices.sbservices_client_free(this.handle) == SpringBoardServicesError.Success);
        }
        
        public static SpringBoardServicesClientHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            SpringBoardServicesClientHandle safeHandle;
            safeHandle = new SpringBoardServicesClientHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static SpringBoardServicesClientHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return SpringBoardServicesClientHandle.DangerousCreate(unsafeHandle, true);
        }
    }
}
