// <copyright file="AfcFileMode.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
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
    /// Flags for afc_file_open 
    /// </summary>
    public enum AfcFileMode : int
    {
        
        FopenRdonly = 1,
        
        FopenRw = 2,
        
        FopenWronly = 3,
        
        FopenWr = 4,
        
        FopenAppend = 5,
        
        /// a+  O_RDWR   | O_APPEND | O_CREAT 
        FopenRdappend = 6,
    }
}
