// <copyright file="MobileBackupErrorExtensions.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.MobileBackup
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public static class MobileBackupErrorExtensions
    {
        
        public static void ThrowOnError(this MobileBackupError value)
        {
            if ((value != MobileBackupError.Success))
            {
                throw new MobileBackupException(value);
            }
        }
        
        public static void ThrowOnError(this MobileBackupError value, string message)
        {
            if ((value != MobileBackupError.Success))
            {
                throw new MobileBackupException(value, message);
            }
        }
        
        public static bool IsError(this MobileBackupError value)
        {
            return (value != MobileBackupError.Success);
        }
    }
}
