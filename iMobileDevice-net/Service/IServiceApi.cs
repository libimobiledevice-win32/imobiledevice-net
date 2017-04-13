// <copyright file="IServiceApi.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Service
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial interface IServiceApi
    {
        
        /// <summary>
        /// Gets or sets the <see cref="ILibiMobileDeviceApi"/> which owns this <see cref="Service"/>.
        /// </summary>
        ILibiMobileDevice Parent
        {
            get;
        }
        
        /// <summary>
        /// Creates a new service for the specified service descriptor.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="service">
        /// The service descriptor returned by lockdownd_start_service.
        /// </param>
        /// <param name="client">
        /// Pointer that will be set to a newly allocated
        /// service_client_t upon successful return.
        /// </param>
        /// <returns>
        /// SERVICE_E_SUCCESS on success,
        /// SERVICE_E_INVALID_ARG when one of the arguments is invalid,
        /// or SERVICE_E_MUX_ERROR when connecting to the device failed.
        /// </returns>
        ServiceError service_client_new(iDeviceHandle device, LockdownServiceDescriptorHandle service, out ServiceClientHandle client);
        
        /// <summary>
        /// Starts a new service on the specified device with given name and
        /// connects to it.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="service_name">
        /// The name of the service to start.
        /// </param>
        /// <param name="client">
        /// Pointer that will point to a newly allocated service_client_t
        /// upon successful return. Must be freed using service_client_free() after
        /// use.
        /// </param>
        /// <param name="label">
        /// The label to use for communication. Usually the program name.
        /// Pass NULL to disable sending the label in requests to lockdownd.
        /// </param>
        /// <returns>
        /// SERVICE_E_SUCCESS on success, or a SERVICE_E_* error code
        /// otherwise.
        /// </returns>
        ServiceError service_client_factory_start_service(iDeviceHandle device, string serviceName, ref System.IntPtr client, string label, ref ConstructorFunc constructorFunc, ref int errorCode);
        
        /// <summary>
        /// Frees a service instance.
        /// </summary>
        /// <param name="client">
        /// The service instance to free.
        /// </param>
        /// <returns>
        /// SERVICE_E_SUCCESS on success,
        /// SERVICE_E_INVALID_ARG when client is invalid, or a
        /// SERVICE_E_UNKNOWN_ERROR when another error occured.
        /// </returns>
        ServiceError service_client_free(System.IntPtr client);
        
        /// <summary>
        /// Sends data using the given service client.
        /// </summary>
        /// <param name="client">
        /// The service client to use for sending.
        /// </param>
        /// <param name="data">
        /// Data to send
        /// </param>
        /// <param name="size">
        /// Size of the data to send
        /// </param>
        /// <param name="sent">
        /// Number of bytes sent (can be NULL to ignore)
        /// </param>
        /// <returns>
        /// SERVICE_E_SUCCESS on success,
        /// SERVICE_E_INVALID_ARG when one or more parameters are
        /// invalid, or SERVICE_E_UNKNOWN_ERROR when an unspecified
        /// error occurs.
        /// </returns>
        ServiceError service_send(ServiceClientHandle client, byte[] data, uint size, ref uint sent);
        
        /// <summary>
        /// Receives data using the given service client with specified timeout.
        /// </summary>
        /// <param name="client">
        /// The service client to use for receiving
        /// </param>
        /// <param name="data">
        /// Buffer that will be filled with the data received
        /// </param>
        /// <param name="size">
        /// Number of bytes to receive
        /// </param>
        /// <param name="received">
        /// Number of bytes received (can be NULL to ignore)
        /// </param>
        /// <param name="timeout">
        /// Maximum time in milliseconds to wait for data.
        /// </param>
        /// <returns>
        /// SERVICE_E_SUCCESS on success,
        /// SERVICE_E_INVALID_ARG when one or more parameters are
        /// invalid, SERVICE_E_MUX_ERROR when a communication error
        /// occurs, or SERVICE_E_UNKNOWN_ERROR when an unspecified
        /// error occurs.
        /// </returns>
        ServiceError service_receive_with_timeout(ServiceClientHandle client, byte[] data, uint size, ref uint received, uint timeout);
        
        /// <summary>
        /// Receives data using the given service client.
        /// </summary>
        /// <param name="client">
        /// The service client to use for receiving
        /// </param>
        /// <param name="data">
        /// Buffer that will be filled with the data received
        /// </param>
        /// <param name="size">
        /// Number of bytes to receive
        /// </param>
        /// <param name="received">
        /// Number of bytes received (can be NULL to ignore)
        /// </param>
        /// <returns>
        /// SERVICE_E_SUCCESS on success,
        /// SERVICE_E_INVALID_ARG when one or more parameters are
        /// invalid, SERVICE_E_MUX_ERROR when a communication error
        /// occurs, or SERVICE_E_UNKNOWN_ERROR when an unspecified
        /// error occurs.
        /// </returns>
        ServiceError service_receive(ServiceClientHandle client, byte[] data, uint size, ref uint received);
        
        /// <summary>
        /// Enable SSL for the given service client.
        /// </summary>
        /// <param name="client">
        /// The connected service client for that SSL should be enabled.
        /// </param>
        /// <returns>
        /// SERVICE_E_SUCCESS on success,
        /// SERVICE_E_INVALID_ARG if client or client->connection is
        /// NULL, SERVICE_E_SSL_ERROR when SSL could not be enabled,
        /// or SERVICE_E_UNKNOWN_ERROR otherwise.
        /// </returns>
        ServiceError service_enable_ssl(ServiceClientHandle client);
        
        /// <summary>
        /// Disable SSL for the given service client.
        /// </summary>
        /// <param name="client">
        /// The connected service client for that SSL should be disabled.
        /// </param>
        /// <returns>
        /// SERVICE_E_SUCCESS on success,
        /// SERVICE_E_INVALID_ARG if client or client->connection is
        /// NULL, or SERVICE_E_UNKNOWN_ERROR otherwise.
        /// </returns>
        ServiceError service_disable_ssl(ServiceClientHandle client);
    }
}
