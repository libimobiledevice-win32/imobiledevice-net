// <copyright file="InstallationProxyStatusCallBack.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.InstallationProxy
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.Cdecl)]
    public delegate void InstallationProxyStatusCallBack([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(PlistHandleDelegateMarshaler))] PlistHandle command, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(PlistHandleDelegateMarshaler))] PlistHandle status, System.IntPtr userData);
}
