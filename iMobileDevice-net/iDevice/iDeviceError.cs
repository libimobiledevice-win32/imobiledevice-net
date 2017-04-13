// <copyright file="iDeviceError.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
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
    /// Error Codes 
    /// </summary>
    public enum iDeviceError : int
    {
        
        Success = 0,
        
        InvalidArg = -1,
        
        UnknownError = -2,
        
        NoDevice = -3,
        
        NotEnoughData = -4,
        
        BadHeader = -5,
        
        SslError = -6,
    }
}
