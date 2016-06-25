// <copyright file="LockdownMarshaler.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Lockdown
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public class LockdownMarshaler : NativeStringArrayMarshaler
    {
        
        public static System.Runtime.InteropServices.ICustomMarshaler GetInstance(string cookie)
        {
            return new LockdownMarshaler();
        }
        
        public override void CleanUpNativeData(System.IntPtr nativeData)
        {
            LibiMobileDevice.Instance.Lockdown.lockdownd_data_classes_free(nativeData).ThrowOnError();
        }
    }
}
