// <copyright file="MobileBackupFlags.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.MobileBackup
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public enum MobileBackupFlags : int
    {
        
        RestoreNotifySpringboard = 1,
        
        RestorePreserveSettings = 2,
        
        RestorePreserveCameraRoll = 4,
    }
}
