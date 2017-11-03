// <copyright file="UsbmuxdSocketType.cs" company="Quamotion">
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
    /// specifies how libusbmuxd should connect to usbmuxd
    /// </summary>
    public enum UsbmuxdSocketType : int
    {
        
        TypeUnix = 1,
        
        TypeTcp = 2,
    }
}
