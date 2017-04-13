// <copyright file="DiagnosticsRelayAction.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.DiagnosticsRelay
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public enum DiagnosticsRelayAction : int
    {
        
        ActionFlagWaitForDisconnect = 2,
        
        ActionFlagDisplayPass = 4,
        
        ActionFlagDisplayFail = 8,
    }
}
