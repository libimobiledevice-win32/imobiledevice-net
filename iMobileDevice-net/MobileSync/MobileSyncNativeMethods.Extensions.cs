// <copyright file="MobileSyncNativeMethods.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.MobileSync
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial class MobileSyncNativeMethods
    {
        
        public static MobileSyncError mobilesync_start(MobileSyncClientHandle client, byte[] dataClass, MobileSyncAnchorsHandle anchors, ulong computerDataClassVersion, ref MobileSyncSyncType syncType, ref ulong deviceDataClassVersion, out string errorDescription)
        {
            System.Runtime.InteropServices.ICustomMarshaler errorDescriptionMarshaler = NativeStringMarshaler.GetInstance(null);
            System.IntPtr errorDescriptionNative = System.IntPtr.Zero;
            MobileSyncError returnValue = MobileSyncNativeMethods.mobilesync_start(client, dataClass, anchors, computerDataClassVersion, ref syncType, ref deviceDataClassVersion, out errorDescriptionNative);
            errorDescription = ((string)errorDescriptionMarshaler.MarshalNativeToManaged(errorDescriptionNative));
            errorDescriptionMarshaler.CleanUpNativeData(errorDescriptionNative);
            return returnValue;
        }
    }
}
