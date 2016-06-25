// <copyright file="iDeviceErrorExtensions.cs" company="Quamotion">
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
    
    
    public static class iDeviceErrorExtensions
    {
        
        public static void ThrowOnError(this iDeviceError value)
        {
            if ((value != iDeviceError.Success))
            {
                throw new iDeviceException(value);
            }
        }
        
        public static bool IsError(this iDeviceError value)
        {
            return (value != iDeviceError.Success);
        }
    }
}
