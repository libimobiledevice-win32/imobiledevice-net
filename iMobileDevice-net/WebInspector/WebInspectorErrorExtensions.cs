// <copyright file="WebInspectorErrorExtensions.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.WebInspector
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public static class WebInspectorErrorExtensions
    {
        
        public static void ThrowOnError(this WebInspectorError value)
        {
            if ((value != WebInspectorError.Success))
            {
                throw new WebInspectorException(value);
            }
        }
        
        public static void ThrowOnError(this WebInspectorError value, string message)
        {
            if ((value != WebInspectorError.Success))
            {
                throw new WebInspectorException(value, message);
            }
        }
        
        public static bool IsError(this WebInspectorError value)
        {
            return (value != WebInspectorError.Success);
        }
    }
}
