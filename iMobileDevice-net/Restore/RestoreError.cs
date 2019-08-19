//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// <copyright file="RestoreError.cs" company="Quamotion">
// Copyright (c) 2016-2019 Quamotion. All rights reserved.
// </copyright>
#pragma warning disable 1591
#pragma warning disable 1572
#pragma warning disable 1573

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
