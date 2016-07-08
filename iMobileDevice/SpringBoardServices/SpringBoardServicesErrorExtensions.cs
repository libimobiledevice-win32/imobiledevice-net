// <copyright file="SpringBoardServicesErrorExtensions.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.SpringBoardServices
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public static class SpringBoardServicesErrorExtensions
    {
        
        public static void ThrowOnError(this SpringBoardServicesError value)
        {
            if ((value != SpringBoardServicesError.Success))
            {
                throw new SpringBoardServicesException(value);
            }
        }
        
        public static void ThrowOnError(this SpringBoardServicesError value, string message)
        {
            if ((value != SpringBoardServicesError.Success))
            {
                throw new SpringBoardServicesException(value, message);
            }
        }
        
        public static bool IsError(this SpringBoardServicesError value)
        {
            return (value != SpringBoardServicesError.Success);
        }
    }
}
