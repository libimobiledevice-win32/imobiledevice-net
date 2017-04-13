// <copyright file="HouseArrestErrorExtensions.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.HouseArrest
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public static class HouseArrestErrorExtensions
    {
        
        public static void ThrowOnError(this HouseArrestError value)
        {
            if ((value != HouseArrestError.Success))
            {
                throw new HouseArrestException(value);
            }
        }
        
        public static void ThrowOnError(this HouseArrestError value, string message)
        {
            if ((value != HouseArrestError.Success))
            {
                throw new HouseArrestException(value, message);
            }
        }
        
        public static bool IsError(this HouseArrestError value)
        {
            return (value != HouseArrestError.Success);
        }
    }
}
