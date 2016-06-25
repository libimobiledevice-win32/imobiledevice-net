// <copyright file="SyslogRelayErrorExtensions.cs" company="Quamotion">
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
    
    
    public static class SyslogRelayErrorExtensions
    {
        
        public static void ThrowOnError(this SyslogRelayError value)
        {
            if ((value != SyslogRelayError.Success))
            {
                throw new SyslogRelayException(value);
            }
        }
        
        public static bool IsError(this SyslogRelayError value)
        {
            return (value != SyslogRelayError.Success);
        }
    }
}
