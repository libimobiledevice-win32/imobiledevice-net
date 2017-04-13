// <copyright file="ScreenshotrErrorExtensions.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Screenshotr
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public static class ScreenshotrErrorExtensions
    {
        
        public static void ThrowOnError(this ScreenshotrError value)
        {
            if ((value != ScreenshotrError.Success))
            {
                throw new ScreenshotrException(value);
            }
        }
        
        public static void ThrowOnError(this ScreenshotrError value, string message)
        {
            if ((value != ScreenshotrError.Success))
            {
                throw new ScreenshotrException(value, message);
            }
        }
        
        public static bool IsError(this ScreenshotrError value)
        {
            return (value != ScreenshotrError.Success);
        }
    }
}
