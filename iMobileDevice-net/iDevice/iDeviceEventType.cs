// <copyright file="iDeviceEventType.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.iDevice
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    /// <summary>
    /// The event type for device add or removal 
    /// </summary>
    public enum iDeviceEventType : int
    {
        
        DeviceAdd = 1,
        
        DeviceRemove = 2,
        
        DevicePaired = 3,
    }
}
