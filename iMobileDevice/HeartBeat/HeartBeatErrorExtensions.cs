// <copyright file="HeartBeatErrorExtensions.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.HeartBeat
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public static class HeartBeatErrorExtensions
    {
        
        public static void ThrowOnError(this HeartBeatError value)
        {
            if ((value != HeartBeatError.Success))
            {
                throw new HeartBeatException(value);
            }
        }
        
        public static void ThrowOnError(this HeartBeatError value, string message)
        {
            if ((value != HeartBeatError.Success))
            {
                throw new HeartBeatException(value, message);
            }
        }
        
        public static bool IsError(this HeartBeatError value)
        {
            return (value != HeartBeatError.Success);
        }
    }
}
