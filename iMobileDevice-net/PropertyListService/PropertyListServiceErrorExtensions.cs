// <copyright file="PropertyListServiceErrorExtensions.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.PropertyListService
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public static class PropertyListServiceErrorExtensions
    {
        
        public static void ThrowOnError(this PropertyListServiceError value)
        {
            if ((value != PropertyListServiceError.Success))
            {
                throw new PropertyListServiceException(value);
            }
        }
        
        public static void ThrowOnError(this PropertyListServiceError value, string message)
        {
            if ((value != PropertyListServiceError.Success))
            {
                throw new PropertyListServiceException(value, message);
            }
        }
        
        public static bool IsError(this PropertyListServiceError value)
        {
            return (value != PropertyListServiceError.Success);
        }
    }
}
