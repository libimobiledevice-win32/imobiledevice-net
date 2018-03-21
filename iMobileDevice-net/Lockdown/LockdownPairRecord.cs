// <copyright file="LockdownPairRecord.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Lockdown
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct LockdownPairRecord
    {
        
        public System.IntPtr device_certificate;
        
        public System.IntPtr host_certificate;
        
        public System.IntPtr root_certificate;
        
        public System.IntPtr host_id;
        
        public System.IntPtr system_buid;
        
        public string device_certificateString
        {
            get
            {
                return Utf8Marshal.PtrToStringUtf8(this.device_certificate);
            }
        }
        
        public string host_certificateString
        {
            get
            {
                return Utf8Marshal.PtrToStringUtf8(this.host_certificate);
            }
        }
        
        public string root_certificateString
        {
            get
            {
                return Utf8Marshal.PtrToStringUtf8(this.root_certificate);
            }
        }
        
        public string host_idString
        {
            get
            {
                return Utf8Marshal.PtrToStringUtf8(this.host_id);
            }
        }
        
        public string system_buidString
        {
            get
            {
                return Utf8Marshal.PtrToStringUtf8(this.system_buid);
            }
        }
    }
}
