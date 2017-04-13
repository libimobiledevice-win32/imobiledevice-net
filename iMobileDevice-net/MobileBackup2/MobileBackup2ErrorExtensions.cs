// <copyright file="MobileBackup2ErrorExtensions.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.MobileBackup2
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public static class MobileBackup2ErrorExtensions
    {
        
        public static void ThrowOnError(this MobileBackup2Error value)
        {
            if ((value != MobileBackup2Error.Success))
            {
                throw new MobileBackup2Exception(value);
            }
        }
        
        public static void ThrowOnError(this MobileBackup2Error value, string message)
        {
            if ((value != MobileBackup2Error.Success))
            {
                throw new MobileBackup2Exception(value, message);
            }
        }
        
        public static bool IsError(this MobileBackup2Error value)
        {
            return (value != MobileBackup2Error.Success);
        }
    }
}
