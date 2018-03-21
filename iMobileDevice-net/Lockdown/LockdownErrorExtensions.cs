// <copyright file="LockdownErrorExtensions.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Lockdown
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public static class LockdownErrorExtensions
    {
        
        public static void ThrowOnError(this LockdownError value)
        {
            if ((value != LockdownError.Success))
            {
                throw new LockdownException(value);
            }
        }
        
        public static void ThrowOnError(this LockdownError value, string message)
        {
            if ((value != LockdownError.Success))
            {
                throw new LockdownException(value, message);
            }
        }
        
        public static bool IsError(this LockdownError value)
        {
            return (value != LockdownError.Success);
        }
    }
}
