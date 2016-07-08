// <copyright file="InstallationProxyErrorExtensions.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.InstallationProxy
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public static class InstallationProxyErrorExtensions
    {
        
        public static void ThrowOnError(this InstallationProxyError value)
        {
            if ((value != InstallationProxyError.Success))
            {
                throw new InstallationProxyException(value);
            }
        }
        
        public static void ThrowOnError(this InstallationProxyError value, string message)
        {
            if ((value != InstallationProxyError.Success))
            {
                throw new InstallationProxyException(value, message);
            }
        }
        
        public static bool IsError(this InstallationProxyError value)
        {
            return (value != InstallationProxyError.Success);
        }
    }
}
