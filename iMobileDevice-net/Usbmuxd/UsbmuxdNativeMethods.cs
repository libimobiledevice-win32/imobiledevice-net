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
        
        const string libraryName = "imobiledevice";
        
        /// <summary>
        /// Subscribe a callback function so that applications get to know about
        /// device add/remove events.
        /// </summary>
        /// <param name="callback">
        /// A callback function that is executed when an event occurs.
        /// </param>
        /// <returns>
        /// 0 on success or negative on error.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(UsbmuxdNativeMethods.libraryName, EntryPoint="usbmuxd_subscribe", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern int usbmuxd_subscribe(UsbmuxdEventCallBack callback, System.IntPtr userData);
        
        /// <summary>
        /// Unsubscribe callback.
        /// </summary>
        /// <returns>
        /// only 0 for now.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(UsbmuxdNativeMethods.libraryName, EntryPoint="usbmuxd_unsubscribe", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern int usbmuxd_unsubscribe();
        
        /// <summary>
        /// Contacts usbmuxd and retrieves a list of connected devices.
        /// </summary>
        /// <param name="device_list">
        /// A pointer to an array of usbmuxd_device_info_t
        /// that will hold records of the connected devices. The last record
        /// is a null-terminated record with all fields set to 0/NULL.
        /// </param>
        /// <returns>
        /// number of attached devices, zero on no devices, or negative
        /// if an error occured.
        /// </returns>
        /// <remarks>
        /// The user has to free the list returned.
        /// </remarks>
        [System.Runtime.InteropServices.DllImportAttribute(UsbmuxdNativeMethods.libraryName, EntryPoint="usbmuxd_get_device_list", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern int usbmuxd_get_device_list(ref System.IntPtr deviceList);
        
        /// <summary>
        /// Frees the device list returned by an usbmuxd_get_device_list call
        /// </summary>
        /// <param name="device_list">
        /// A pointer to an array of usbmuxd_device_info_t to free.
        /// </param>
        /// <returns>
        /// 0 on success, -1 on error.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(UsbmuxdNativeMethods.libraryName, EntryPoint="usbmuxd_device_list_free", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern int usbmuxd_device_list_free(System.IntPtr deviceList);
        
        /// <summary>
        /// Gets device information for the device specified by udid.
        /// </summary>
        /// <param name="udid">
        /// A device UDID of the device to look for. If udid is NULL,
        /// This function will return the first device found.
        /// </param>
        /// <param name="device">
        /// Pointer to a previously allocated (or static)
        /// usbmuxd_device_info_t that will be filled with the device info.
        /// </param>
        /// <returns>
        /// 0 if no matching device is connected, 1 if the device was found,
        /// or a negative value on error.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(UsbmuxdNativeMethods.libraryName, EntryPoint="usbmuxd_get_device_by_udid", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern int usbmuxd_get_device_by_udid([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string udid, ref UsbmuxdDeviceInfo device);
        
        /// <summary>
        /// Request proxy connect to
        /// </summary>
        /// <param name="handle">
        /// returned by 'usbmuxd_scan()'
        /// </param>
        /// <param name="tcp_port">
        /// TCP port number on device, in range 0-65535.
        /// common values are 62078 for lockdown, and 22 for SSH.
        /// </param>
        /// <returns>
        /// file descriptor socket of the connection, or -1 on error
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(UsbmuxdNativeMethods.libraryName, EntryPoint="usbmuxd_connect", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern int usbmuxd_connect(int handle, ushort tcpPort);
        
        /// <summary>
        /// Disconnect. For now, this just closes the socket file descriptor.
        /// </summary>
        /// <param name="sfd">
        /// socker file descriptor returned by usbmuxd_connect()
        /// </param>
        /// <returns>
        /// 0 on success, -1 on error.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(UsbmuxdNativeMethods.libraryName, EntryPoint="usbmuxd_disconnect", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern int usbmuxd_disconnect(int sfd);
        
        /// <summary>
        /// Send data to the specified socket.
        /// </summary>
        /// <param name="sfd">
        /// socket file descriptor returned by usbmuxd_connect()
        /// </param>
        /// <param name="data">
        /// buffer to send
        /// </param>
        /// <param name="len">
        /// size of buffer to send
        /// </param>
        /// <param name="sent_bytes">
        /// how many bytes sent
        /// </param>
        /// <returns>
        /// 0 on success, a negative errno value otherwise.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(UsbmuxdNativeMethods.libraryName, EntryPoint="usbmuxd_send", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern int usbmuxd_send(int sfd, byte[] data, uint len, ref uint sentBytes);
        
        /// <summary>
        /// Receive data from the specified socket.
        /// </summary>
        /// <param name="sfd">
        /// socket file descriptor returned by usbmuxd_connect()
        /// </param>
        /// <param name="data">
        /// buffer to put the data to
        /// </param>
        /// <param name="len">
        /// number of bytes to receive
        /// </param>
        /// <param name="recv_bytes">
        /// number of bytes received
        /// </param>
        /// <param name="timeout">
        /// how many milliseconds to wait for data
        /// </param>
        /// <returns>
        /// 0 on success, a negative errno value otherwise.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(UsbmuxdNativeMethods.libraryName, EntryPoint="usbmuxd_recv_timeout", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern int usbmuxd_recv_timeout(int sfd, byte[] data, uint len, ref uint recvBytes, uint timeout);
        
        /// <summary>
        /// Receive data from the specified socket with a default timeout.
        /// </summary>
        /// <param name="sfd">
        /// socket file descriptor returned by usbmuxd_connect()
        /// </param>
        /// <param name="data">
        /// buffer to put the data to
        /// </param>
        /// <param name="len">
        /// number of bytes to receive
        /// </param>
        /// <param name="recv_bytes">
        /// number of bytes received
        /// </param>
        /// <returns>
        /// 0 on success, a negative errno value otherwise.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(UsbmuxdNativeMethods.libraryName, EntryPoint="usbmuxd_recv", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern int usbmuxd_recv(int sfd, byte[] data, uint len, ref uint recvBytes);
        
        /// <summary>
        /// Reads the SystemBUID
        /// </summary>
        /// <param name="buid">
        /// pointer to a variable that will be set to point to a newly
        /// allocated string with the System BUID returned by usbmuxd
        /// </param>
        /// <returns>
        /// 0 on success, a negative errno value otherwise.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(UsbmuxdNativeMethods.libraryName, EntryPoint="usbmuxd_read_buid", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern int usbmuxd_read_buid(out System.IntPtr buid);
        
        /// <summary>
        /// Read a pairing record
        /// </summary>
        /// <param name="record_id">
        /// the record identifier of the pairing record to retrieve
        /// </param>
        /// <param name="record_data">
        /// pointer to a variable that will be set to point to a
        /// newly allocated buffer containing the pairing record data
        /// </param>
        /// <param name="record_size">
        /// pointer to a variable that will be set to the size of
        /// the buffer returned in record_data
        /// </param>
        /// <returns>
        /// 0 on success, a negative error value otherwise.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(UsbmuxdNativeMethods.libraryName, EntryPoint="usbmuxd_read_pair_record", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern int usbmuxd_read_pair_record([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string recordId, out System.IntPtr recordData, ref uint recordSize);
        
        /// <summary>
        /// Save a pairing record
        /// </summary>
        /// <param name="record_id">
        /// the record identifier of the pairing record to save
        /// </param>
        /// <param name="record_data">
        /// buffer containing the pairing record data
        /// </param>
        /// <param name="record_size">
        /// size of the buffer passed in record_data
        /// </param>
        /// <returns>
        /// 0 on success, a negative error value otherwise.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(UsbmuxdNativeMethods.libraryName, EntryPoint="usbmuxd_save_pair_record", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern int usbmuxd_save_pair_record([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string recordId, [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string recordData, uint recordSize);
        
        /// <summary>
        /// Delete a pairing record
        /// </summary>
        /// <param name="record_id">
        /// the record identifier of the pairing record to delete.
        /// </param>
        /// <returns>
        /// 0 on success, a negative errno value otherwise.
        /// </returns>
        [System.Runtime.InteropServices.DllImportAttribute(UsbmuxdNativeMethods.libraryName, EntryPoint="usbmuxd_delete_pair_record", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern int usbmuxd_delete_pair_record([System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPStr)] string recordId);
        
        /// <summary>
        /// Enable or disable the use of inotify extension. Enabled by default.
        /// Use 0 to disable and 1 to enable inotify support.
        /// This only has an effect on linux systems if inotify support has been built
        /// in. Otherwise and on all other platforms this function has no effect.
        /// </summary>
        [System.Runtime.InteropServices.DllImportAttribute(UsbmuxdNativeMethods.libraryName, EntryPoint="libusbmuxd_set_use_inotify", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void libusbmuxd_set_use_inotify(int set);
        
        [System.Runtime.InteropServices.DllImportAttribute(UsbmuxdNativeMethods.libraryName, EntryPoint="libusbmuxd_set_debug_level", CallingConvention=System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static extern void libusbmuxd_set_debug_level(int level);
    }
}
