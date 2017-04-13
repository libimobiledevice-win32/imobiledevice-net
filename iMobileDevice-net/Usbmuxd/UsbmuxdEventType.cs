// <copyright file="UsbmuxdEventType.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Usbmuxd
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    /// <summary>
    /// event types for event callback function
    /// </summary>
    public enum UsbmuxdEventType : int
    {
        
        DeviceAdd = 1,
        
        DeviceRemove = 2,
        
        DevicePaired = 3,
    }
}
