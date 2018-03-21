// <copyright file="DebugServerErrorExtensions.cs" company="Quamotion">
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
    
    
    public static class DebugServerErrorExtensions
    {
        
        public static void ThrowOnError(this DebugServerError value)
        {
            if ((value != DebugServerError.Success))
            {
                throw new DebugServerException(value);
            }
        }
        
        public static void ThrowOnError(this DebugServerError value, string message)
        {
            if ((value != DebugServerError.Success))
            {
                throw new DebugServerException(value, message);
            }
        }
        
        public static bool IsError(this DebugServerError value)
        {
            return (value != DebugServerError.Success);
        }
    }
}
