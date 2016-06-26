// <copyright file="FileRelayError.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.FileRelay
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
    public enum FileRelayError : int
    {
        
        Success = 0,
        
        InvalidArg = -1,
        
        PlistError = -2,
        
        MuxError = -3,
        
        InvalidSource = -4,
        
        StagingEmpty = -5,
        
        PermissionDenied = -6,
        
        UnknownError = -256,
    }
}
