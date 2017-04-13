// <copyright file="MobileSyncAnchors.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.MobileSync
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct MobileSyncAnchors
    {
        
        public System.IntPtr device_anchor;
        
        public System.IntPtr computer_anchor;
        
        public string device_anchorString
        {
            get
            {
                return Utf8Marshal.PtrToStringUtf8(this.device_anchor);
            }
        }
        
        public string computer_anchorString
        {
            get
            {
                return Utf8Marshal.PtrToStringUtf8(this.computer_anchor);
            }
        }
    }
}
