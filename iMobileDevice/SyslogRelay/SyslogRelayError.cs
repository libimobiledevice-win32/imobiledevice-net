// <copyright file="SyslogRelayError.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.SyslogRelay
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
    public enum SyslogRelayError : int
    {
        
        Success = 0,
        
        InvalidArg = -1,
        
        MuxError = -2,
        
        SslError = -3,
        
        UnknownError = -256,
    }
}
