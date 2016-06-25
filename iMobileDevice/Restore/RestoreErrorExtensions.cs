// <copyright file="RestoreErrorExtensions.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Restore
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public static class RestoreErrorExtensions
    {
        
        public static void ThrowOnError(this RestoreError value)
        {
            if ((value != RestoreError.Success))
            {
                throw new RestoreException(value);
            }
        }
        
        public static bool IsError(this RestoreError value)
        {
            return (value != RestoreError.Success);
        }
    }
}
