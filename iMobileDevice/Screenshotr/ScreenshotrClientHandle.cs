// <copyright file="ScreenshotrClientHandle.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Screenshotr
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
    public partial class ScreenshotrClientHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        private ILibiMobileDevice api;
        
        protected ScreenshotrClientHandle() : 
                base(true)
        {
        }
        
        protected ScreenshotrClientHandle(bool ownsHandle) : 
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
        
        public static ScreenshotrClientHandle Zero
        {
            get
            {
                return ScreenshotrClientHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
            return (this.Api.Screenshotr.screenshotr_client_free(this.handle) == ScreenshotrError.Success);
        }
        
        public static ScreenshotrClientHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            ScreenshotrClientHandle safeHandle;
            safeHandle = new ScreenshotrClientHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static ScreenshotrClientHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return ScreenshotrClientHandle.DangerousCreate(unsafeHandle, true);
        }
    }
}
