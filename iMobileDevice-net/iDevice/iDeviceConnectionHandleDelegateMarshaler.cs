// <copyright file="iDeviceConnectionHandleDelegateMarshaler.cs" company="Quamotion">
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
    
    
    public partial class iDeviceConnectionHandleDelegateMarshaler : System.Runtime.InteropServices.ICustomMarshaler
    {
        
        public static System.Runtime.InteropServices.ICustomMarshaler GetInstance(string cookie)
        {
            return new iDeviceConnectionHandleDelegateMarshaler();
        }
        
        public void CleanUpManagedData(object managedObject)
        {
        }
        
        public void CleanUpNativeData(System.IntPtr nativeData)
        {
        }
        
        public int GetNativeDataSize()
        {
            return -1;
        }
        
        public System.IntPtr MarshalManagedToNative(object managedObject)
        {
            return System.IntPtr.Zero;
        }
        
        public object MarshalNativeToManaged(System.IntPtr nativeData)
        {
            return iDeviceConnectionHandle.DangerousCreate(nativeData, false);
        }
    }
}
