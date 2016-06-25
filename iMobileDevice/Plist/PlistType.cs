// <copyright file="PlistType.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Plist
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    /// <summary>
    /// The enumeration of plist node types.
    ///</summary>
    public enum PlistType : int
    {
        
        Boolean = 0,
        
        Uint = 1,
        
        Real = 2,
        
        String = 3,
        
        Array = 4,
        
        Dict = 5,
        
        Date = 6,
        
        Data = 7,
        
        Key = 8,
        
        Uid = 9,
        
        ///  No type 
        None = 10,
    }
}
