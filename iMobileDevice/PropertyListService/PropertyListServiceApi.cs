// <copyright file="PropertyListServiceApi.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.PropertyListService
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial class PropertyListServiceApi : IPropertyListServiceApi
    {
        
        /// <summary>
        /// Creates a new property list service for the specified port.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="service">
        /// The service descriptor returned by lockdownd_start_service.
        /// </param>
        /// <param name="client">
        /// Pointer that will be set to a newly allocated
        /// property_list_service_client_t upon successful return.
        /// </param>
        /// <returns>
        /// PROPERTY_LIST_SERVICE_E_SUCCESS on success,
        /// PROPERTY_LIST_SERVICE_E_INVALID_ARG when one of the arguments is invalid,
        /// or PROPERTY_LIST_SERVICE_E_MUX_ERROR when connecting to the device failed.
        /// </returns>
        public virtual PropertyListServiceError property_list_service_client_new(iDeviceHandle device, LockdownServiceDescriptorHandle service, out PropertyListServiceClientHandle client)
        {
            return PropertyListServiceNativeMethods.property_list_service_client_new(device, service, out client);
        }
        
        /// <summary>
        /// Frees a PropertyList service.
        /// </summary>
        /// <param name="client">
        /// The property list service to free.
        /// </param>
        /// <returns>
        /// PROPERTY_LIST_SERVICE_E_SUCCESS on success,
        /// PROPERTY_LIST_SERVICE_E_INVALID_ARG when client is invalid, or a
        /// PROPERTY_LIST_SERVICE_E_UNKNOWN_ERROR when another error occured.
        /// </returns>
        public virtual PropertyListServiceError property_list_service_client_free(System.IntPtr client)
        {
            return PropertyListServiceNativeMethods.property_list_service_client_free(client);
        }
        
        /// <summary>
        /// Sends an XML plist.
        /// </summary>
        /// <param name="client">
        /// The property list service client to use for sending.
        /// </param>
        /// <param name="plist">
        /// plist to send
        /// </param>
        /// <returns>
        /// PROPERTY_LIST_SERVICE_E_SUCCESS on success,
        /// PROPERTY_LIST_SERVICE_E_INVALID_ARG when client or plist is NULL,
        /// PROPERTY_LIST_SERVICE_E_PLIST_ERROR when dict is not a valid plist,
        /// or PROPERTY_LIST_SERVICE_E_UNKNOWN_ERROR when an unspecified error occurs.
        /// </returns>
        public virtual PropertyListServiceError property_list_service_send_xml_plist(PropertyListServiceClientHandle client, PlistHandle plist)
        {
            return PropertyListServiceNativeMethods.property_list_service_send_xml_plist(client, plist);
        }
        
        /// <summary>
        /// Sends a binary plist.
        /// </summary>
        /// <param name="client">
        /// The property list service client to use for sending.
        /// </param>
        /// <param name="plist">
        /// plist to send
        /// </param>
        /// <returns>
        /// PROPERTY_LIST_SERVICE_E_SUCCESS on success,
        /// PROPERTY_LIST_SERVICE_E_INVALID_ARG when client or plist is NULL,
        /// PROPERTY_LIST_SERVICE_E_PLIST_ERROR when dict is not a valid plist,
        /// or PROPERTY_LIST_SERVICE_E_UNKNOWN_ERROR when an unspecified error occurs.
        /// </returns>
        public virtual PropertyListServiceError property_list_service_send_binary_plist(PropertyListServiceClientHandle client, PlistHandle plist)
        {
            return PropertyListServiceNativeMethods.property_list_service_send_binary_plist(client, plist);
        }
        
        /// <summary>
        /// Receives a plist using the given property list service client with specified
        /// timeout.
        /// Binary or XML plists are automatically handled.
        /// </summary>
        /// <param name="client">
        /// The property list service client to use for receiving
        /// </param>
        /// <param name="plist">
        /// pointer to a plist_t that will point to the received plist
        /// upon successful return
        /// </param>
        /// <param name="timeout">
        /// Maximum time in milliseconds to wait for data.
        /// </param>
        /// <returns>
        /// PROPERTY_LIST_SERVICE_E_SUCCESS on success,
        /// PROPERTY_LIST_SERVICE_E_INVALID_ARG when connection or *plist is NULL,
        /// PROPERTY_LIST_SERVICE_E_PLIST_ERROR when the received data cannot be
        /// converted to a plist, PROPERTY_LIST_SERVICE_E_MUX_ERROR when a
        /// communication error occurs, or PROPERTY_LIST_SERVICE_E_UNKNOWN_ERROR when
        /// an unspecified error occurs.
        /// </returns>
        public virtual PropertyListServiceError property_list_service_receive_plist_with_timeout(PropertyListServiceClientHandle client, out PlistHandle plist, uint timeout)
        {
            return PropertyListServiceNativeMethods.property_list_service_receive_plist_with_timeout(client, out plist, timeout);
        }
        
        /// <summary>
        /// Receives a plist using the given property list service client.
        /// Binary or XML plists are automatically handled.
        /// This function is like property_list_service_receive_plist_with_timeout
        /// using a timeout of 10 seconds.
        /// </summary>
        /// <param name="client">
        /// The property list service client to use for receiving
        /// </param>
        /// <param name="plist">
        /// pointer to a plist_t that will point to the received plist
        /// upon successful return
        /// </param>
        /// <returns>
        /// PROPERTY_LIST_SERVICE_E_SUCCESS on success,
        /// PROPERTY_LIST_SERVICE_E_INVALID_ARG when client or *plist is NULL,
        /// PROPERTY_LIST_SERVICE_E_PLIST_ERROR when the received data cannot be
        /// converted to a plist, PROPERTY_LIST_SERVICE_E_MUX_ERROR when a
        /// communication error occurs, or PROPERTY_LIST_SERVICE_E_UNKNOWN_ERROR when
        /// an unspecified error occurs.
        /// </returns>
        public virtual PropertyListServiceError property_list_service_receive_plist(PropertyListServiceClientHandle client, out PlistHandle plist)
        {
            return PropertyListServiceNativeMethods.property_list_service_receive_plist(client, out plist);
        }
        
        /// <summary>
        /// Enable SSL for the given property list service client.
        /// </summary>
        /// <param name="client">
        /// The connected property list service client for which SSL
        /// should be enabled.
        /// </param>
        /// <returns>
        /// PROPERTY_LIST_SERVICE_E_SUCCESS on success,
        /// PROPERTY_LIST_SERVICE_E_INVALID_ARG if client or client->connection is
        /// NULL, PROPERTY_LIST_SERVICE_E_SSL_ERROR when SSL could not be enabled,
        /// or PROPERTY_LIST_SERVICE_E_UNKNOWN_ERROR otherwise.
        /// </returns>
        public virtual PropertyListServiceError property_list_service_enable_ssl(PropertyListServiceClientHandle client)
        {
            return PropertyListServiceNativeMethods.property_list_service_enable_ssl(client);
        }
        
        /// <summary>
        /// Disable SSL for the given property list service client.
        /// </summary>
        /// <param name="client">
        /// The connected property list service client for which SSL
        /// should be disabled.
        /// </param>
        /// <returns>
        /// PROPERTY_LIST_SERVICE_E_SUCCESS on success,
        /// PROPERTY_LIST_SERVICE_E_INVALID_ARG if client or client->connection is
        /// NULL, or PROPERTY_LIST_SERVICE_E_UNKNOWN_ERROR otherwise.
        /// </returns>
        public virtual PropertyListServiceError property_list_service_disable_ssl(PropertyListServiceClientHandle client)
        {
            return PropertyListServiceNativeMethods.property_list_service_disable_ssl(client);
        }
    }
}
