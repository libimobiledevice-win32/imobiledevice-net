// <copyright file="UsbmuxdNativeMethods.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Usbmuxd
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial class UsbmuxdNativeMethods
    {
        
        public static int usbmuxd_get_tcp_endpoint(out string host, ref ushort port)
        {
            System.Runtime.InteropServices.ICustomMarshaler hostMarshaler = NativeStringMarshaler.GetInstance(null);
            System.IntPtr hostNative = System.IntPtr.Zero;
            int returnValue = UsbmuxdNativeMethods.usbmuxd_get_tcp_endpoint(out hostNative, ref port);
            host = ((string)hostMarshaler.MarshalNativeToManaged(hostNative));
            hostMarshaler.CleanUpNativeData(hostNative);
            return returnValue;
        }
        
        public static int usbmuxd_read_buid(out string buid)
        {
            System.Runtime.InteropServices.ICustomMarshaler buidMarshaler = NativeStringMarshaler.GetInstance(null);
            System.IntPtr buidNative = System.IntPtr.Zero;
            int returnValue = UsbmuxdNativeMethods.usbmuxd_read_buid(out buidNative);
            buid = ((string)buidMarshaler.MarshalNativeToManaged(buidNative));
            buidMarshaler.CleanUpNativeData(buidNative);
            return returnValue;
        }
        
        public static int usbmuxd_read_pair_record(string recordId, out string recordData, ref uint recordSize)
        {
            System.Runtime.InteropServices.ICustomMarshaler recordDataMarshaler = NativeStringMarshaler.GetInstance(null);
            System.IntPtr recordDataNative = System.IntPtr.Zero;
            int returnValue = UsbmuxdNativeMethods.usbmuxd_read_pair_record(recordId, out recordDataNative, ref recordSize);
            recordData = ((string)recordDataMarshaler.MarshalNativeToManaged(recordDataNative));
            recordDataMarshaler.CleanUpNativeData(recordDataNative);
            return returnValue;
        }
    }
}
