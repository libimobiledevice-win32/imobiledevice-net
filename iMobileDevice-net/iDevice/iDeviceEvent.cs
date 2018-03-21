// <copyright file="iDeviceEvent.cs" company="Quamotion">
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
    
    
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct iDeviceEvent
    {
        
        public iDeviceEventType @event;
        
        public System.IntPtr udid;
        
        public int conn_type;
        
        public string udidString
        {
            get
            {
                return Utf8Marshal.PtrToStringUtf8(this.udid);
            }
        }
    }
}
