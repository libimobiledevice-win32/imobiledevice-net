// <copyright file="IHeartBeatApi.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.HeartBeat
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial interface IHeartBeatApi
    {
        
        /// <summary>
        /// Gets or sets the <see cref="ILibiMobileDeviceApi"/> which owns this <see cref="HeartBeat"/>.
        /// </summary>
        ILibiMobileDevice Parent
        {
            get;
        }
        
        /// <summary>
        /// Connects to the heartbeat service on the specified device.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="service">
        /// The service descriptor returned by lockdownd_start_service.
        /// </param>
        /// <param name="client">
        /// Pointer that will point to a newly allocated
        /// heartbeat_client_t upon successful return. Must be freed using
        /// heartbeat_client_free() after use.
        /// </param>
        /// <returns>
        /// HEARTBEAT_E_SUCCESS on success, HEARTBEAT_E_INVALID_ARG when
        /// client is NULL, or an HEARTBEAT_E_* error code otherwise.
        /// </returns>
        HeartBeatError heartbeat_client_new(iDeviceHandle device, LockdownServiceDescriptorHandle service, out HeartBeatClientHandle client);
        
        /// <summary>
        /// Starts a new heartbeat service on the specified device and connects to it.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="client">
        /// Pointer that will point to a newly allocated
        /// heartbeat_client_t upon successful return. Must be freed using
        /// heartbeat_client_free() after use.
        /// </param>
        /// <param name="label">
        /// The label to use for communication. Usually the program name.
        /// Pass NULL to disable sending the label in requests to lockdownd.
        /// </param>
        /// <returns>
        /// HEARTBEAT_E_SUCCESS on success, or an HEARTBEAT_E_* error
        /// code otherwise.
        /// </returns>
        HeartBeatError heartbeat_client_start_service(iDeviceHandle device, out HeartBeatClientHandle client, string label);
        
        /// <summary>
        /// Disconnects a heartbeat client from the device and frees up the
        /// heartbeat client data.
        /// </summary>
        /// <param name="client">
        /// The heartbeat client to disconnect and free.
        /// </param>
        /// <returns>
        /// HEARTBEAT_E_SUCCESS on success, HEARTBEAT_E_INVALID_ARG when
        /// client is NULL, or an HEARTBEAT_E_* error code otherwise.
        /// </returns>
        HeartBeatError heartbeat_client_free(System.IntPtr client);
        
        /// <summary>
        /// Sends a plist to the service.
        /// </summary>
        /// <param name="client">
        /// The heartbeat client
        /// </param>
        /// <param name="plist">
        /// The plist to send
        /// </param>
        /// <returns>
        /// HEARTBEAT_E_SUCCESS on success,
        /// HEARTBEAT_E_INVALID_ARG when client or plist is NULL
        /// </returns>
        HeartBeatError heartbeat_send(HeartBeatClientHandle client, PlistHandle plist);
        
        /// <summary>
        /// Receives a plist from the service.
        /// </summary>
        /// <param name="client">
        /// The heartbeat client
        /// </param>
        /// <param name="plist">
        /// The plist to store the received data
        /// </param>
        /// <returns>
        /// HEARTBEAT_E_SUCCESS on success,
        /// HEARTBEAT_E_INVALID_ARG when client or plist is NULL
        /// </returns>
        HeartBeatError heartbeat_receive(HeartBeatClientHandle client, out PlistHandle plist);
        
        /// <summary>
        /// Receives a plist using the given heartbeat client.
        /// </summary>
        /// <param name="client">
        /// The heartbeat client to use for receiving
        /// </param>
        /// <param name="plist">
        /// pointer to a plist_t that will point to the received plist
        /// upon successful return
        /// </param>
        /// <param name="timeout">
        /// Maximum time in milliseconds to wait for data.
        /// </param>
        /// <returns>
        /// HEARTBEAT_E_SUCCESS on success,
        /// HEARTBEAT_E_INVALID_ARG when client or *plist is NULL,
        /// HEARTBEAT_E_PLIST_ERROR when the received data cannot be
        /// converted to a plist, HEARTBEAT_E_MUX_ERROR when a
        /// communication error occurs, or HEARTBEAT_E_UNKNOWN_ERROR
        /// when an unspecified error occurs.
        /// </returns>
        HeartBeatError heartbeat_receive_with_timeout(HeartBeatClientHandle client, out PlistHandle plist, uint timeoutMs);
    }
}
