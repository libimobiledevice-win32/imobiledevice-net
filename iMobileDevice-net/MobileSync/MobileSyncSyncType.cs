// <copyright file="MobileSyncSyncType.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.MobileSync
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    /// <summary>
    /// The sync type of the current sync session. 
    /// </summary>
    public enum MobileSyncSyncType : int
    {
        
        SyncTypeFast = 0,
        
        SyncTypeSlow = 1,
        
        /// Reset-sync signals that the computer should send all data again. 
        SyncTypeReset = 2,
    }
}
