// <copyright file="HouseArrestClientHandle.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.HouseArrest
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
    public partial class HouseArrestClientHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        protected HouseArrestClientHandle() : 
                base(true)
        {
        }
        
        protected HouseArrestClientHandle(bool ownsHandle) : 
                base(ownsHandle)
        {
        }
        
        public static HouseArrestClientHandle Zero
        {
            get
            {
                return HouseArrestClientHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
            return (LibiMobileDevice.Instance.HouseArrest.house_arrest_client_free(this.handle) == HouseArrestError.Success);
        }
        
        public static HouseArrestClientHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            HouseArrestClientHandle safeHandle;
            safeHandle = new HouseArrestClientHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static HouseArrestClientHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return HouseArrestClientHandle.DangerousCreate(unsafeHandle, true);
        }
    }
}
