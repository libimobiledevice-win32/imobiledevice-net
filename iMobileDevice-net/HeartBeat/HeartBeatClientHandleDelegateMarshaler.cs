// <copyright file="HeartBeatClientHandleDelegateMarshaler.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.HeartBeat
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial class HeartBeatClientHandleDelegateMarshaler : System.Runtime.InteropServices.ICustomMarshaler
    {
        
        public static System.Runtime.InteropServices.ICustomMarshaler GetInstance(string cookie)
        {
            return new HeartBeatClientHandleDelegateMarshaler();
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
            return HeartBeatClientHandle.DangerousCreate(nativeData, false);
        }
    }
}
