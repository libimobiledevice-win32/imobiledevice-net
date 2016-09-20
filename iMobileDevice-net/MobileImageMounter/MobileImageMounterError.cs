// <copyright file="MobileImageMounterError.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.MobileImageMounter
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
    public enum MobileImageMounterError : int
    {
        
        Success = 0,
        
        InvalidArg = -1,
        
        PlistError = -2,
        
        ConnFailed = -3,
        
        CommandFailed = -4,
        
        UnknownError = -256,
    }
}
