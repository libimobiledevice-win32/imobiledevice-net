// <copyright file="FileRelayErrorExtensions.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.FileRelay
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public static class FileRelayErrorExtensions
    {
        
        public static void ThrowOnError(this FileRelayError value)
        {
            if ((value != FileRelayError.Success))
            {
                throw new FileRelayException(value);
            }
        }
        
        public static void ThrowOnError(this FileRelayError value, string message)
        {
            if ((value != FileRelayError.Success))
            {
                throw new FileRelayException(value, message);
            }
        }
        
        public static bool IsError(this FileRelayError value)
        {
            return (value != FileRelayError.Success);
        }
    }
}
