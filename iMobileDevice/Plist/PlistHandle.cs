// <copyright file="PlistHandle.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Plist
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
    public partial class PlistHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        private ILibiMobileDevice api;
        
        protected PlistHandle() : 
                base(true)
        {
        }
        
        protected PlistHandle(bool ownsHandle) : 
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
        
        public static PlistHandle Zero
        {
            get
            {
                return PlistHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
            this.Api.Plist.plist_free(this.handle);
            return true;
        }
        
        public static PlistHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            PlistHandle safeHandle;
            safeHandle = new PlistHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static PlistHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return PlistHandle.DangerousCreate(unsafeHandle, true);
        }
    }
}
