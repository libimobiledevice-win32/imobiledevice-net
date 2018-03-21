// <copyright file="RestoreNativeMethods.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Restore
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial class RestoreNativeMethods
    {
        
        public static RestoreError restored_query_type(RestoreClientHandle client, out string type, ref ulong version)
        {
            System.Runtime.InteropServices.ICustomMarshaler typeMarshaler = NativeStringMarshaler.GetInstance(null);
            System.IntPtr typeNative = System.IntPtr.Zero;
            RestoreError returnValue = RestoreNativeMethods.restored_query_type(client, out typeNative, ref version);
            type = ((string)typeMarshaler.MarshalNativeToManaged(typeNative));
            typeMarshaler.CleanUpNativeData(typeNative);
            return returnValue;
        }
    }
}
