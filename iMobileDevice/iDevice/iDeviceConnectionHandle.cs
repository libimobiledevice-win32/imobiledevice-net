// <copyright file="iDeviceConnectionHandle.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.iDevice
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
    public partial class iDeviceConnectionHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        protected iDeviceConnectionHandle() : 
                base(true)
        {
        }
        
        protected iDeviceConnectionHandle(bool ownsHandle) : 
                base(ownsHandle)
        {
        }
        
        public static iDeviceConnectionHandle Zero
        {
            get
            {
                return iDeviceConnectionHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
            return (LibiMobileDevice.Instance.iDevice.idevice_disconnect(this.handle) == iDeviceError.Success);
        }
        
        public static iDeviceConnectionHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            iDeviceConnectionHandle safeHandle;
            safeHandle = new iDeviceConnectionHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static iDeviceConnectionHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return iDeviceConnectionHandle.DangerousCreate(unsafeHandle, true);
        }
    }
}
