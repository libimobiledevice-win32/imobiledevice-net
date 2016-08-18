// <copyright file="iDeviceApi.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.iDevice
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial class iDeviceApi : IiDeviceApi
    {
        
        /// <summary>
        /// Backing field for the <see cref="Parent"/> property
        /// </summary>
        private ILibiMobileDevice parent;
        
        /// <summary>
        /// Initializes a new instance of the <see cref"iDeviceApi"/> class
        /// </summary>
        /// <param name="parent">
        /// The <see cref="ILibiMobileDeviceApi"/> which owns this <see cref="iDevice"/>.
        /// </summary>
        public iDeviceApi(ILibiMobileDevice parent)
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
        /// Sets the callback to invoke when writing out debug messages. If this callback is set, messages
        /// will be written to this callback instead of the standard output.
        /// </summary>
        /// <param name="callback">
        /// The callback which will receive the debug messages. Set to NULL to redirect to stdout.
        /// </param>
        public virtual void idevice_set_debug_callback(iDeviceDebugCallBack callback)
        {
            iDeviceNativeMethods.idevice_set_debug_callback(callback);
        }
        
        /// <summary>
        /// Set the level of debugging.
        /// </summary>
        /// <param name="level">
        /// Set to 0 for no debug output, 1 to enable basic debug output and 2 to enable full debug output.
        /// When set to 2, the values of buffers being sent across the wire are printed out as well, this results in very
        /// verbose output.
        /// </param>
        public virtual void idevice_set_debug_level(int level)
        {
            iDeviceNativeMethods.idevice_set_debug_level(level);
        }
        
        /// <summary>
        /// Register a callback function that will be called when device add/remove
        /// events occur.
        /// </summary>
        /// <param name="callback">
        /// Callback function to call.
        /// </param>
        /// <param name="user_data">
        /// Application-specific data passed as parameter
        /// to the registered callback function.
        /// </param>
        /// <returns>
        /// IDEVICE_E_SUCCESS on success or an error value when an error occured.
        /// </returns>
        public virtual iDeviceError idevice_event_subscribe(iDeviceEventCallBack callback, System.IntPtr userData)
        {
            return iDeviceNativeMethods.idevice_event_subscribe(callback, userData);
        }
        
        /// <summary>
        /// Release the event callback function that has been registered with
        /// idevice_event_subscribe().
        /// </summary>
        /// <returns>
        /// IDEVICE_E_SUCCESS on success or an error value when an error occured.
        /// </returns>
        public virtual iDeviceError idevice_event_unsubscribe()
        {
            return iDeviceNativeMethods.idevice_event_unsubscribe();
        }
        
        /// <summary>
        /// Get a list of currently available devices.
        /// </summary>
        /// <param name="devices">
        /// List of udids of devices that are currently available.
        /// This list is terminated by a NULL pointer.
        /// </param>
        /// <param name="count">
        /// Number of devices found.
        /// </param>
        /// <returns>
        /// IDEVICE_E_SUCCESS on success or an error value when an error occured.
        /// </returns>
        public virtual iDeviceError idevice_get_device_list(out System.Collections.ObjectModel.ReadOnlyCollection<string> devices, ref int count)
        {
            return iDeviceNativeMethods.idevice_get_device_list(out devices, ref count);
        }
        
        /// <summary>
        /// Free a list of device udids.
        /// </summary>
        /// <param name="devices">
        /// List of udids to free.
        /// </param>
        /// <returns>
        /// Always returnes IDEVICE_E_SUCCESS.
        /// </returns>
        public virtual iDeviceError idevice_device_list_free(System.IntPtr devices)
        {
            return iDeviceNativeMethods.idevice_device_list_free(devices);
        }
        
        /// <summary>
        /// Creates an idevice_t structure for the device specified by udid,
        /// if the device is available.
        /// </summary>
        /// <param name="device">
        /// Upon calling this function, a pointer to a location of type
        /// idevice_t. On successful return, this location will be populated.
        /// </param>
        /// <param name="udid">
        /// The UDID to match.
        /// </param>
        /// <returns>
        /// IDEVICE_E_SUCCESS if ok, otherwise an error code.
        /// </returns>
        /// <remarks>
        /// The resulting idevice_t structure has to be freed with
        /// idevice_free() if it is no longer used.
        /// </remarks>
        public virtual iDeviceError idevice_new(out iDeviceHandle device, string udid)
        {
            iDeviceError returnValue;
            returnValue = iDeviceNativeMethods.idevice_new(out device, udid);
            device.Api = this.Parent;
            return returnValue;
        }
        
        /// <summary>
        /// Cleans up an idevice structure, then frees the structure itself.
        /// This is a library-level function; deals directly with the device to tear
        /// down relations, but otherwise is mostly internal.
        /// </summary>
        /// <param name="device">
        /// idevice_t to free.
        /// </param>
        public virtual iDeviceError idevice_free(System.IntPtr device)
        {
            return iDeviceNativeMethods.idevice_free(device);
        }
        
        /// <summary>
        /// Set up a connection to the given device.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="port">
        /// The destination port to connect to.
        /// </param>
        /// <param name="connection">
        /// Pointer to an idevice_connection_t that will be filled
        /// with the necessary data of the connection.
        /// </param>
        /// <returns>
        /// IDEVICE_E_SUCCESS if ok, otherwise an error code.
        /// </returns>
        public virtual iDeviceError idevice_connect(iDeviceHandle device, ushort port, out iDeviceConnectionHandle connection)
        {
            iDeviceError returnValue;
            returnValue = iDeviceNativeMethods.idevice_connect(device, port, out connection);
            connection.Api = this.Parent;
            return returnValue;
        }
        
        /// <summary>
        /// Disconnect from the device and clean up the connection structure.
        /// </summary>
        /// <param name="connection">
        /// The connection to close.
        /// </param>
        /// <returns>
        /// IDEVICE_E_SUCCESS if ok, otherwise an error code.
        /// </returns>
        public virtual iDeviceError idevice_disconnect(System.IntPtr connection)
        {
            return iDeviceNativeMethods.idevice_disconnect(connection);
        }
        
        /// <summary>
        /// Send data to a device via the given connection.
        /// </summary>
        /// <param name="connection">
        /// The connection to send data over.
        /// </param>
        /// <param name="data">
        /// Buffer with data to send.
        /// </param>
        /// <param name="len">
        /// Size of the buffer to send.
        /// </param>
        /// <param name="sent_bytes">
        /// Pointer to an uint32_t that will be filled
        /// with the number of bytes actually sent.
        /// </param>
        /// <returns>
        /// IDEVICE_E_SUCCESS if ok, otherwise an error code.
        /// </returns>
        public virtual iDeviceError idevice_connection_send(iDeviceConnectionHandle connection, byte[] data, uint len, ref uint sentBytes)
        {
            return iDeviceNativeMethods.idevice_connection_send(connection, data, len, ref sentBytes);
        }
        
        /// <summary>
        /// Receive data from a device via the given connection.
        /// This function will return after the given timeout even if no data has been
        /// received.
        /// </summary>
        /// <param name="connection">
        /// The connection to receive data from.
        /// </param>
        /// <param name="data">
        /// Buffer that will be filled with the received data.
        /// This buffer has to be large enough to hold len bytes.
        /// </param>
        /// <param name="len">
        /// Buffer size or number of bytes to receive.
        /// </param>
        /// <param name="recv_bytes">
        /// Number of bytes actually received.
        /// </param>
        /// <param name="timeout">
        /// Timeout in milliseconds after which this function should
        /// return even if no data has been received.
        /// </param>
        /// <returns>
        /// IDEVICE_E_SUCCESS if ok, otherwise an error code.
        /// </returns>
        public virtual iDeviceError idevice_connection_receive_timeout(iDeviceConnectionHandle connection, byte[] data, uint len, ref uint recvBytes, uint timeout)
        {
            return iDeviceNativeMethods.idevice_connection_receive_timeout(connection, data, len, ref recvBytes, timeout);
        }
        
        /// <summary>
        /// Receive data from a device via the given connection.
        /// This function is like idevice_connection_receive_timeout, but with a
        /// predefined reasonable timeout.
        /// </summary>
        /// <param name="connection">
        /// The connection to receive data from.
        /// </param>
        /// <param name="data">
        /// Buffer that will be filled with the received data.
        /// This buffer has to be large enough to hold len bytes.
        /// </param>
        /// <param name="len">
        /// Buffer size or number of bytes to receive.
        /// </param>
        /// <param name="recv_bytes">
        /// Number of bytes actually received.
        /// </param>
        /// <returns>
        /// IDEVICE_E_SUCCESS if ok, otherwise an error code.
        /// </returns>
        public virtual iDeviceError idevice_connection_receive(iDeviceConnectionHandle connection, byte[] data, uint len, ref uint recvBytes)
        {
            return iDeviceNativeMethods.idevice_connection_receive(connection, data, len, ref recvBytes);
        }
        
        /// <summary>
        /// Enables SSL for the given connection.
        /// </summary>
        /// <param name="connection">
        /// The connection to enable SSL for.
        /// </param>
        /// <returns>
        /// IDEVICE_E_SUCCESS on success, IDEVICE_E_INVALID_ARG when connection
        /// is NULL or connection->ssl_data is non-NULL, or IDEVICE_E_SSL_ERROR when
        /// SSL initialization, setup, or handshake fails.
        /// </returns>
        public virtual iDeviceError idevice_connection_enable_ssl(iDeviceConnectionHandle connection)
        {
            return iDeviceNativeMethods.idevice_connection_enable_ssl(connection);
        }
        
        /// <summary>
        /// Disable SSL for the given connection.
        /// </summary>
        /// <param name="connection">
        /// The connection to disable SSL for.
        /// </param>
        /// <returns>
        /// IDEVICE_E_SUCCESS on success, IDEVICE_E_INVALID_ARG when connection
        /// is NULL. This function also returns IDEVICE_E_SUCCESS when SSL is not
        /// enabled and does no further error checking on cleanup.
        /// </returns>
        public virtual iDeviceError idevice_connection_disable_ssl(iDeviceConnectionHandle connection)
        {
            return iDeviceNativeMethods.idevice_connection_disable_ssl(connection);
        }
        
        /// <summary>
        /// Get the underlying file descriptor for a connection
        /// </summary>
        /// <param name="connection">
        /// The connection to get fd of
        /// </param>
        /// <param name="fd">
        /// Pointer to an int where the fd is stored
        /// </param>
        /// <returns>
        /// IDEVICE_E_SUCCESS if ok, otherwise an error code.
        /// </returns>
        public virtual iDeviceError idevice_connection_get_fd(iDeviceConnectionHandle connection, ref int fd)
        {
            return iDeviceNativeMethods.idevice_connection_get_fd(connection, ref fd);
        }
        
        /// <summary>
        /// Gets the handle of the device. Depends on the connection type.
        /// </summary>
        public virtual iDeviceError idevice_get_handle(iDeviceHandle device, ref uint handle)
        {
            return iDeviceNativeMethods.idevice_get_handle(device, ref handle);
        }
        
        /// <summary>
        /// Gets the unique id for the device.
        /// </summary>
        public virtual iDeviceError idevice_get_udid(iDeviceHandle device, out string udid)
        {
            return iDeviceNativeMethods.idevice_get_udid(device, out udid);
        }
    }
}
