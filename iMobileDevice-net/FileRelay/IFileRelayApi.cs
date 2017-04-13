// <copyright file="IFileRelayApi.cs" company="Quamotion">
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
    
    
    public partial interface IFileRelayApi
    {
        
        /// <summary>
        /// Gets or sets the <see cref="ILibiMobileDeviceApi"/> which owns this <see cref="FileRelay"/>.
        /// </summary>
        ILibiMobileDevice Parent
        {
            get;
        }
        
        /// <summary>
        /// Connects to the file_relay service on the specified device.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="service">
        /// The service descriptor returned by lockdownd_start_service.
        /// </param>
        /// <param name="client">
        /// Reference that will point to a newly allocated
        /// file_relay_client_t upon successful return.
        /// </param>
        /// <returns>
        /// FILE_RELAY_E_SUCCESS on success,
        /// FILE_RELAY_E_INVALID_ARG when one of the parameters is invalid,
        /// or FILE_RELAY_E_MUX_ERROR when the connection failed.
        /// </returns>
        FileRelayError file_relay_client_new(iDeviceHandle device, LockdownServiceDescriptorHandle service, out FileRelayClientHandle client);
        
        /// <summary>
        /// Starts a new file_relay service on the specified device and connects to it.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="client">
        /// Pointer that will point to a newly allocated
        /// file_relay_client_t upon successful return. Must be freed using
        /// file_relay_client_free() after use.
        /// </param>
        /// <param name="label">
        /// The label to use for communication. Usually the program name.
        /// Pass NULL to disable sending the label in requests to lockdownd.
        /// </param>
        /// <returns>
        /// FILE_RELAY_E_SUCCESS on success, or an FILE_RELAY_E_* error
        /// code otherwise.
        /// </returns>
        FileRelayError file_relay_client_start_service(iDeviceHandle device, out FileRelayClientHandle client, string label);
        
        /// <summary>
        /// Disconnects a file_relay client from the device and frees up the file_relay
        /// client data.
        /// </summary>
        /// <param name="client">
        /// The file_relay client to disconnect and free.
        /// </param>
        /// <returns>
        /// FILE_RELAY_E_SUCCESS on success,
        /// FILE_RELAY_E_INVALID_ARG when one of client or client->parent
        /// is invalid, or FILE_RELAY_E_UNKNOWN_ERROR when the was an error
        /// freeing the parent property_list_service client.
        /// </returns>
        FileRelayError file_relay_client_free(System.IntPtr client);
        
        /// <summary>
        /// Request data for the given sources.
        /// </summary>
        /// <param name="client">
        /// The connected file_relay client.
        /// </param>
        /// <param name="sources">
        /// A NULL-terminated list of sources to retrieve.
        /// Valid sources are:
        /// - AppleSupport
        /// - Network
        /// - VPN
        /// - WiFi
        /// - UserDatabases
        /// - CrashReporter
        /// - tmp
        /// - SystemConfiguration
        /// </param>
        /// <param name="connection">
        /// The connection that has to be used for receiving the
        /// data using idevice_connection_receive(). The connection will be closed
        /// automatically by the device, but use file_relay_client_free() to clean
        /// up properly.
        /// </param>
        /// <param name="timeout">
        /// Maximum time in milliseconds to wait for data.
        /// </param>
        /// <returns>
        /// FILE_RELAY_E_SUCCESS on succes, FILE_RELAY_E_INVALID_ARG when one or
        /// more parameters are invalid, FILE_RELAY_E_MUX_ERROR if a communication
        /// error occurs, FILE_RELAY_E_PLIST_ERROR when the received result is NULL
        /// or is not a valid plist, FILE_RELAY_E_INVALID_SOURCE if one or more
        /// sources are invalid, FILE_RELAY_E_STAGING_EMPTY if no data is available
        /// for the given sources, or FILE_RELAY_E_UNKNOWN_ERROR otherwise.
        /// </returns>
        /// <remarks>
        /// WARNING: Don't call this function without reading the data afterwards.
        /// A directory mobile_file_relay.XXXX used for creating the archive will
        /// remain in the /tmp directory otherwise.
        /// </remarks>
        FileRelayError file_relay_request_sources(FileRelayClientHandle client, out string sources, out iDeviceConnectionHandle connection);
        
        /// <summary>
        /// Request data for the given sources. Calls file_relay_request_sources_timeout() with
        /// a timeout of 60000 milliseconds (60 seconds).
        /// </summary>
        /// <param name="client">
        /// The connected file_relay client.
        /// </param>
        /// <param name="sources">
        /// A NULL-terminated list of sources to retrieve.
        /// Valid sources are:
        /// - AppleSupport
        /// - Network
        /// - VPN
        /// - WiFi
        /// - UserDatabases
        /// - CrashReporter
        /// - tmp
        /// - SystemConfiguration
        /// </param>
        /// <param name="connection">
        /// The connection that has to be used for receiving the
        /// data using idevice_connection_receive(). The connection will be closed
        /// automatically by the device, but use file_relay_client_free() to clean
        /// up properly.
        /// </param>
        /// <returns>
        /// FILE_RELAY_E_SUCCESS on succes, FILE_RELAY_E_INVALID_ARG when one or
        /// more parameters are invalid, FILE_RELAY_E_MUX_ERROR if a communication
        /// error occurs, FILE_RELAY_E_PLIST_ERROR when the received result is NULL
        /// or is not a valid plist, FILE_RELAY_E_INVALID_SOURCE if one or more
        /// sources are invalid, FILE_RELAY_E_STAGING_EMPTY if no data is available
        /// for the given sources, or FILE_RELAY_E_UNKNOWN_ERROR otherwise.
        /// </returns>
        /// <remarks>
        /// WARNING: Don't call this function without reading the data afterwards.
        /// A directory mobile_file_relay.XXXX used for creating the archive will
        /// remain in the /tmp directory otherwise.
        /// </remarks>
        FileRelayError file_relay_request_sources_timeout(FileRelayClientHandle client, out string sources, out iDeviceConnectionHandle connection, uint timeout);
    }
}
