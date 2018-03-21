// <copyright file="MobileactivationErrorExtensions.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Mobileactivation
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public static class MobileactivationErrorExtensions
    {
        
        public static void ThrowOnError(this MobileactivationError value)
        {
            if ((value != MobileactivationError.Success))
            {
                throw new MobileactivationException(value);
            }
        }
        
        public static void ThrowOnError(this MobileactivationError value, string message)
        {
            if ((value != MobileactivationError.Success))
            {
                throw new MobileactivationException(value, message);
            }
        }
        
        public static bool IsError(this MobileactivationError value)
        {
            return (value != MobileactivationError.Success);
        }
    }
}
