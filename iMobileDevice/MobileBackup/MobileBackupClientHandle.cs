// <copyright file="MobileBackupClientHandle.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.MobileBackup
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, UnmanagedCode=true)]
    [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, UnmanagedCode=true)]
    public partial class MobileBackupClientHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
    {
        
        protected MobileBackupClientHandle() : 
                base(true)
        {
        }
        
        protected MobileBackupClientHandle(bool ownsHandle) : 
                base(ownsHandle)
        {
        }
        
        public static MobileBackupClientHandle Zero
        {
            get
            {
                return MobileBackupClientHandle.DangerousCreate(System.IntPtr.Zero);
            }
        }
        
        [System.Runtime.ConstrainedExecution.ReliabilityContractAttribute(System.Runtime.ConstrainedExecution.Consistency.WillNotCorruptState, System.Runtime.ConstrainedExecution.Cer.MayFail)]
        protected override bool ReleaseHandle()
        {
            System.Diagnostics.Debug.WriteLine("Releasing {0} {1}", this.GetType().Name, this.handle);
            return (LibiMobileDevice.Instance.MobileBackup.mobilebackup_client_free(this.handle) == MobileBackupError.Success);
        }
        
        public static MobileBackupClientHandle DangerousCreate(System.IntPtr unsafeHandle, bool ownsHandle)
        {
            MobileBackupClientHandle safeHandle;
            safeHandle = new MobileBackupClientHandle(ownsHandle);
            safeHandle.SetHandle(unsafeHandle);
            return safeHandle;
        }
        
        public static MobileBackupClientHandle DangerousCreate(System.IntPtr unsafeHandle)
        {
            return MobileBackupClientHandle.DangerousCreate(unsafeHandle, true);
        }
    }
}
