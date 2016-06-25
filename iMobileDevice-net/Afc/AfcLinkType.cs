// <copyright file="AfcLinkType.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
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
    /// Type of link for afc_make_link() calls 
    /// </summary>
    public enum AfcLinkType : int
    {
        
        Hardlink = 1,
        
        Symlink = 2,
    }
}
