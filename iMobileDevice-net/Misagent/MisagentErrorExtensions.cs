// <copyright file="MisagentErrorExtensions.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Misagent
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public static class MisagentErrorExtensions
    {
        
        public static void ThrowOnError(this MisagentError value)
        {
            if ((value != MisagentError.Success))
            {
                throw new MisagentException(value);
            }
        }
        
        public static void ThrowOnError(this MisagentError value, string message)
        {
            if ((value != MisagentError.Success))
            {
                throw new MisagentException(value, message);
            }
        }
        
        public static bool IsError(this MisagentError value)
        {
            return (value != MisagentError.Success);
        }
    }
}
