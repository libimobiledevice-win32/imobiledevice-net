// <copyright file="SpringBoardServicesInterfaceOrientation.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.SpringBoardServices
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public enum SpringBoardServicesInterfaceOrientation : int
    {
        
        InterfaceOrientationUnknown = 0,
        
        InterfaceOrientationPortrait = 1,
        
        InterfaceOrientationPortraitUpsideDown = 2,
        
        InterfaceOrientationLandscapeRight = 3,
        
        InterfaceOrientationLandscapeLeft = 4,
    }
}
