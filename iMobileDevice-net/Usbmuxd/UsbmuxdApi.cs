// <copyright file="UsbmuxdApi.cs" company="Quamotion">
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
    
    
    public partial class UsbmuxdApi : IUsbmuxdApi
    {
        
        /// <summary>
        /// Backing field for the <see cref="Parent"/> property
        /// </summary>
        private ILibiMobileDevice parent;
        
        /// <summary>
        /// Initializes a new instance of the <see cref"UsbmuxdApi"/> class
        /// </summary>
        /// <param name="parent">
        /// The <see cref="ILibiMobileDeviceApi"/> which owns this <see cref="Usbmuxd"/>.
        /// </summary>
        public UsbmuxdApi(ILibiMobileDevice parent)
        {
            this.parent = parent;
        }
        
        /// <inheritdoc/>
        public ILibiMobileDevice Parent
        {
            get
            {
                return this.parent;
            }
        }
        
        /// <summary>
        /// Sets the socket type (Unix socket or TCP socket) libusbmuxd should use when connecting
        /// to usbmuxd.
        /// </summary>
        /// <param name="value">
        /// SOCKET_TYPE_UNIX or SOCKET_TYPE_TCP
        /// </param>
        /// <returns>
        /// 0 on success or negative on error
        /// </returns>
        public virtual int usbmuxd_set_socket_type(int value)
        {
            return UsbmuxdNativeMethods.usbmuxd_set_socket_type(value);
        }
        
        /// <summary>
        /// Gets the socket type (Unix socket or TCP socket) libusbmuxd should use when connecting
        /// to usbmuxd.
        /// </summary>
        /// <param name="value">
        /// A pointer to an integer which will reveive the current socket type
        /// </param>
        /// <returns>
        /// 0 on success or negative on error
        /// </returns>
        public virtual int usbmuxd_get_socket_type(ref int value)
        {
            return UsbmuxdNativeMethods.usbmuxd_get_socket_type(ref value);
        }
        
        /// <summary>
        /// Sets the TCP endpoint to which usbmuxd will connect if the socket type is set to
        /// SOCKET_TYPE_TCP
        /// </summary>
        /// <param name="host">
        /// The hostname or IP address to which to connect
        /// </param>
        /// <param name="port">
        /// The port to which to connect.
        /// </param>
        /// <returns>
        /// 0 on success or negative on error
        /// </returns>
        public virtual int usbmuxd_set_tcp_endpoint(string host, ushort port)
        {
            return UsbmuxdNativeMethods.usbmuxd_set_tcp_endpoint(host, port);
        }
        
        /// <summary>
        /// Gets the TCP endpoint to which usbmuxd will connect if th esocket type is set to
        /// SOCKET_TYPE_TCP
        /// </summary>
        /// <param name="host">
        /// A pointer which will be set to the hostname or IP address to which to connect.
        /// The caller must free this string.
        /// </param>
        /// <param name="port">
        /// The port to which to connect
        /// </param>
        /// <returns>
        /// 0 on success or negative on error
        /// </returns>
        public virtual int usbmuxd_get_tcp_endpoint(out string host, ref ushort port)
        {
            return UsbmuxdNativeMethods.usbmuxd_get_tcp_endpoint(out host, ref port);
        }
        
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
        public virtual int usbmuxd_subscribe(UsbmuxdEventCallBack callback, System.IntPtr userData)
        {
            return UsbmuxdNativeMethods.usbmuxd_subscribe(callback, userData);
        }
        
        /// <summary>
        /// Unsubscribe callback.
        /// </summary>
        /// <returns>
        /// only 0 for now.
        /// </returns>
        public virtual int usbmuxd_unsubscribe()
        {
            return UsbmuxdNativeMethods.usbmuxd_unsubscribe();
        }
        
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
        public virtual int usbmuxd_get_device_list(ref System.IntPtr deviceList)
        {
            return UsbmuxdNativeMethods.usbmuxd_get_device_list(ref deviceList);
        }
        
        /// <summary>
        /// Frees the device list returned by an usbmuxd_get_device_list call
        /// </summary>
        /// <param name="device_list">
        /// A pointer to an array of usbmuxd_device_info_t to free.
        /// </param>
        /// <returns>
        /// 0 on success, -1 on error.
        /// </returns>
        public virtual int usbmuxd_device_list_free(System.IntPtr deviceList)
        {
            return UsbmuxdNativeMethods.usbmuxd_device_list_free(deviceList);
        }
        
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
        public virtual int usbmuxd_get_device_by_udid(string udid, ref UsbmuxdDeviceInfo device)
        {
            return UsbmuxdNativeMethods.usbmuxd_get_device_by_udid(udid, ref device);
        }
        
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
        public virtual int usbmuxd_connect(int handle, ushort tcpPort)
        {
            return UsbmuxdNativeMethods.usbmuxd_connect(handle, tcpPort);
        }
        
        /// <summary>
        /// Disconnect. For now, this just closes the socket file descriptor.
        /// </summary>
        /// <param name="sfd">
        /// socker file descriptor returned by usbmuxd_connect()
        /// </param>
        /// <returns>
        /// 0 on success, -1 on error.
        /// </returns>
        public virtual int usbmuxd_disconnect(int sfd)
        {
            return UsbmuxdNativeMethods.usbmuxd_disconnect(sfd);
        }
        
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
        public virtual int usbmuxd_send(int sfd, byte[] data, uint len, ref uint sentBytes)
        {
            return UsbmuxdNativeMethods.usbmuxd_send(sfd, data, len, ref sentBytes);
        }
        
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
        public virtual int usbmuxd_recv_timeout(int sfd, byte[] data, uint len, ref uint recvBytes, uint timeout)
        {
            return UsbmuxdNativeMethods.usbmuxd_recv_timeout(sfd, data, len, ref recvBytes, timeout);
        }
        
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
        public virtual int usbmuxd_recv(int sfd, byte[] data, uint len, ref uint recvBytes)
        {
            return UsbmuxdNativeMethods.usbmuxd_recv(sfd, data, len, ref recvBytes);
        }
        
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
        public virtual int usbmuxd_read_buid(out string buid)
        {
            return UsbmuxdNativeMethods.usbmuxd_read_buid(out buid);
        }
        
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
        public virtual int usbmuxd_read_pair_record(string recordId, out string recordData, ref uint recordSize)
        {
            return UsbmuxdNativeMethods.usbmuxd_read_pair_record(recordId, out recordData, ref recordSize);
        }
        
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
        public virtual int usbmuxd_save_pair_record(string recordId, string recordData, uint recordSize)
        {
            return UsbmuxdNativeMethods.usbmuxd_save_pair_record(recordId, recordData, recordSize);
        }
        
        /// <summary>
        /// Delete a pairing record
        /// </summary>
        /// <param name="record_id">
        /// the record identifier of the pairing record to delete.
        /// </param>
        /// <returns>
        /// 0 on success, a negative errno value otherwise.
        /// </returns>
        public virtual int usbmuxd_delete_pair_record(string recordId)
        {
            return UsbmuxdNativeMethods.usbmuxd_delete_pair_record(recordId);
        }
        
        /// <summary>
        /// Enable or disable the use of inotify extension. Enabled by default.
        /// Use 0 to disable and 1 to enable inotify support.
        /// This only has an effect on linux systems if inotify support has been built
        /// in. Otherwise and on all other platforms this function has no effect.
        /// </summary>
        public virtual void libusbmuxd_set_use_inotify(int set)
        {
            UsbmuxdNativeMethods.libusbmuxd_set_use_inotify(set);
        }
        
        public virtual void libusbmuxd_set_debug_level(int level)
        {
            UsbmuxdNativeMethods.libusbmuxd_set_debug_level(level);
        }
    }
}
