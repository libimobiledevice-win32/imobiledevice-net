// <copyright file="MobileSyncError.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.MobileSync
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    /// <summary>
    /// Error Codes 
    ///</summary>
    public enum MobileSyncError : int
    {
        
        Success = 0,
        
        InvalidArg = -1,
        
        PlistError = -2,
        
        MuxError = -3,
        
        BadVersion = -4,
        
        SyncRefused = -5,
        
        Cancelled = -6,
        
        WrongDirection = -7,
        
        NotReady = -8,
        
        UnknownError = -256,
    }
}
