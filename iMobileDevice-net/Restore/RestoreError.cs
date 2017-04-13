// <copyright file="RestoreError.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Restore
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
    public enum RestoreError : int
    {
        
        Success = 0,
        
        InvalidArg = -1,
        
        InvalidConf = -2,
        
        PlistError = -3,
        
        DictError = -4,
        
        NotEnoughData = -5,
        
        MuxError = -6,
        
        StartRestoreFailed = -7,
        
        DeviceError = -8,
        
        UnknownError = -256,
    }
}
