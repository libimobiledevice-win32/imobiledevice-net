// <copyright file="FileRelayNativeMethods.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.FileRelay
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial class FileRelayNativeMethods
    {
        
        public static FileRelayError file_relay_request_sources(FileRelayClientHandle client, out string sources, out iDeviceConnectionHandle connection)
        {
            System.Runtime.InteropServices.ICustomMarshaler sourcesMarshaler = NativeStringMarshaler.GetInstance(null);
            System.IntPtr sourcesNative = System.IntPtr.Zero;
            FileRelayError returnValue = FileRelayNativeMethods.file_relay_request_sources(client, out sourcesNative, out connection);
            sources = ((string)sourcesMarshaler.MarshalNativeToManaged(sourcesNative));
            sourcesMarshaler.CleanUpNativeData(sourcesNative);
            return returnValue;
        }
        
        public static FileRelayError file_relay_request_sources_timeout(FileRelayClientHandle client, out string sources, out iDeviceConnectionHandle connection, uint timeout)
        {
            System.Runtime.InteropServices.ICustomMarshaler sourcesMarshaler = NativeStringMarshaler.GetInstance(null);
            System.IntPtr sourcesNative = System.IntPtr.Zero;
            FileRelayError returnValue = FileRelayNativeMethods.file_relay_request_sources_timeout(client, out sourcesNative, out connection, timeout);
            sources = ((string)sourcesMarshaler.MarshalNativeToManaged(sourcesNative));
            sourcesMarshaler.CleanUpNativeData(sourcesNative);
            return returnValue;
        }
    }
}
