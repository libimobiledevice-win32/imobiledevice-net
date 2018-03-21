// <copyright file="MobileSyncErrorExtensions.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.MobileSync
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public static class MobileSyncErrorExtensions
    {
        
        public static void ThrowOnError(this MobileSyncError value)
        {
            if ((value != MobileSyncError.Success))
            {
                throw new MobileSyncException(value);
            }
        }
        
        public static void ThrowOnError(this MobileSyncError value, string message)
        {
            if ((value != MobileSyncError.Success))
            {
                throw new MobileSyncException(value, message);
            }
        }
        
        public static bool IsError(this MobileSyncError value)
        {
            return (value != MobileSyncError.Success);
        }
    }
}
