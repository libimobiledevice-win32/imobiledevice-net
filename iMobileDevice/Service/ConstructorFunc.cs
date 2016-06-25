// <copyright file="ConstructorFunc.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Service
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    [System.Runtime.InteropServices.UnmanagedFunctionPointerAttribute(System.Runtime.InteropServices.CallingConvention.Cdecl)]
    public delegate int ConstructorFunc([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(iDeviceHandleDelegateMarshaler))] iDeviceHandle param0, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.CustomMarshaler, MarshalTypeRef=typeof(LockdownServiceDescriptorHandleDelegateMarshaler))] LockdownServiceDescriptorHandle param1, ref System.IntPtr param2);
}
