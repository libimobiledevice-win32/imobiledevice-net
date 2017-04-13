// <copyright file="AfcLockOp.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Afc
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    /// <summary>
    /// Lock operation flags 
    /// </summary>
    public enum AfcLockOp : int
    {
        
        LockSh = 5,
        
        LockEx = 6,
        
        /// unlock 
        LockUn = 12,
    }
}
