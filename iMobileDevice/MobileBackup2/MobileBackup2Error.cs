// <copyright file="MobileBackup2Error.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.MobileBackup2
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
    public enum MobileBackup2Error : int
    {
        
        Success = 0,
        
        InvalidArg = -1,
        
        PlistError = -2,
        
        MuxError = -3,
        
        BadVersion = -4,
        
        ReplyNotOk = -5,
        
        NoCommonVersion = -6,
        
        UnknownError = -256,
    }
}
