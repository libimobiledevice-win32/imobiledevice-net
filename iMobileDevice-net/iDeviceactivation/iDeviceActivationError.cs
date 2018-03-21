// <copyright file="iDeviceActivationError.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.iDeviceActivation
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public enum iDeviceActivationError : int
    {
        
        Success = 0,
        
        IncompleteInfo = -1,
        
        OutOfMemory = -2,
        
        UnknownContentType = -3,
        
        BuddymlParsingError = -4,
        
        PlistParsingError = -5,
        
        HtmlParsingError = -6,
        
        UnsupportedFieldType = -7,
        
        InternalError = -255,
    }
}
