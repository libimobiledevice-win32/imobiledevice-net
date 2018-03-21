// <copyright file="DebugServerError.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.DebugServer
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
    public enum DebugServerError : int
    {
        
        Success = 0,
        
        InvalidArg = -1,
        
        MuxError = -2,
        
        SslError = -3,
        
        ResponseError = -4,
        
        UnknownError = -256,
    }
}
