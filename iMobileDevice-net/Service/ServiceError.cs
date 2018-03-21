// <copyright file="ServiceError.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Service
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
    public enum ServiceError : int
    {
        
        Success = 0,
        
        InvalidArg = -1,
        
        MuxError = -3,
        
        SslError = -4,
        
        StartServiceError = -5,
        
        UnknownError = -256,
    }
}
