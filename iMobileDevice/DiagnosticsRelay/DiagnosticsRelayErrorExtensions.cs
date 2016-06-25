// <copyright file="DiagnosticsRelayErrorExtensions.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.DiagnosticsRelay
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public static class DiagnosticsRelayErrorExtensions
    {
        
        public static void ThrowOnError(this DiagnosticsRelayError value)
        {
            if ((value != DiagnosticsRelayError.Success))
            {
                throw new DiagnosticsRelayException(value);
            }
        }
        
        public static bool IsError(this DiagnosticsRelayError value)
        {
            return (value != DiagnosticsRelayError.Success);
        }
    }
}
