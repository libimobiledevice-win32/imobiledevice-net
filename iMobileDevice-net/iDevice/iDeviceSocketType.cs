// <copyright file="iDeviceSocketType.cs" company="Quamotion">
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
    /// specifies how libusbmuxd should connect to usbmuxd
    /// </summary>
    public enum iDeviceSocketType : int
    {
        
        SocketTypeUnix = 1,
        
        SocketTypeTcp = 2,
    }
}
