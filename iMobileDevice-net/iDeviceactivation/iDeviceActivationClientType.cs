// <copyright file="iDeviceActivationClientType.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.iDeviceActivation
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public enum iDeviceActivationClientType : int
    {
        
        ClientMobileActivation = 0,
        
        ClientItunes = 1,
    }
}
