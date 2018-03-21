// <copyright file="IHouseArrestApi.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.HouseArrest
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial interface IHouseArrestApi
    {
        
        /// <summary>
        /// Gets or sets the <see cref="ILibiMobileDeviceApi"/> which owns this <see cref="HouseArrest"/>.
        /// </summary>
        ILibiMobileDevice Parent
        {
            get;
        }
        
        /// <summary>
        /// Connects to the house_arrest service on the specified device.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="service">
        /// The service descriptor returned by lockdownd_start_service.
        /// </param>
        /// <param name="client">
        /// Pointer that will point to a newly allocated
        /// housearrest_client_t upon successful return.
        /// </param>
        /// <returns>
        /// HOUSE_ARREST_E_SUCCESS on success, HOUSE_ARREST_E_INVALID_ARG when
        /// client is NULL, or an HOUSE_ARREST_E_* error code otherwise.
        /// </returns>
        HouseArrestError house_arrest_client_new(iDeviceHandle device, LockdownServiceDescriptorHandle service, out HouseArrestClientHandle client);
        
        /// <summary>
        /// Starts a new house_arrest service on the specified device and connects to it.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="client">
        /// Pointer that will point to a newly allocated
        /// house_arrest_client_t upon successful return. Must be freed using
        /// house_arrest_client_free() after use.
        /// </param>
        /// <param name="label">
        /// The label to use for communication. Usually the program name.
        /// Pass NULL to disable sending the label in requests to lockdownd.
        /// </param>
        /// <returns>
        /// HOUSE_ARREST_E_SUCCESS on success, or an HOUSE_ARREST_E_* error
        /// code otherwise.
        /// </returns>
        HouseArrestError house_arrest_client_start_service(iDeviceHandle device, out HouseArrestClientHandle client, string label);
        
        /// <summary>
        /// Disconnects an house_arrest client from the device and frees up the
        /// house_arrest client data.
        /// </summary>
        /// <param name="client">
        /// The house_arrest client to disconnect and free.
        /// </param>
        /// <returns>
        /// HOUSE_ARREST_E_SUCCESS on success, HOUSE_ARREST_E_INVALID_ARG when
        /// client is NULL, or an HOUSE_ARREST_E_* error code otherwise.
        /// </returns>
        /// <remarks>
        /// After using afc_client_new_from_house_arrest_client(), make sure
        /// you call afc_client_free() before calling this function to ensure
        /// a proper cleanup. Do not call this function if you still need to
        /// perform AFC operations since it will close the connection.
        /// </remarks>
        HouseArrestError house_arrest_client_free(System.IntPtr client);
        
        /// <summary>
        /// Sends a generic request to the connected house_arrest service.
        /// </summary>
        /// <param name="client">
        /// The house_arrest client to use.
        /// </param>
        /// <param name="dict">
        /// The request to send as a plist of type PLIST_DICT.
        /// </param>
        /// <returns>
        /// HOUSE_ARREST_E_SUCCESS if the request was successfully sent,
        /// HOUSE_ARREST_E_INVALID_ARG if client or dict is invalid,
        /// HOUSE_ARREST_E_PLIST_ERROR if dict is not a plist of type PLIST_DICT,
        /// HOUSE_ARREST_E_INVALID_MODE if the client is not in the correct mode,
        /// or HOUSE_ARREST_E_CONN_FAILED if a connection error occured.
        /// </returns>
        /// <remarks>
        /// If this function returns HOUSE_ARREST_E_SUCCESS it does not mean
        /// that the request was successful. To check for success or failure you
        /// need to call house_arrest_get_result().
        /// </remarks>
        HouseArrestError house_arrest_send_request(HouseArrestClientHandle client, PlistHandle dict);
        
        /// <summary>
        /// Send a command to the connected house_arrest service.
        /// Calls house_arrest_send_request() internally.
        /// </summary>
        /// <param name="client">
        /// The house_arrest client to use.
        /// </param>
        /// <param name="command">
        /// The command to send. Currently, only VendContainer and
        /// VendDocuments are known.
        /// </param>
        /// <param name="appid">
        /// The application identifier to pass along with the .
        /// </param>
        /// <returns>
        /// HOUSE_ARREST_E_SUCCESS if the command was successfully sent,
        /// HOUSE_ARREST_E_INVALID_ARG if client, command, or appid is invalid,
        /// HOUSE_ARREST_E_INVALID_MODE if the client is not in the correct mode,
        /// or HOUSE_ARREST_E_CONN_FAILED if a connection error occured.
        /// </returns>
        /// <remarks>
        /// If this function returns HOUSE_ARREST_E_SUCCESS it does not mean
        /// that the command was successful. To check for success or failure you
        /// need to call house_arrest_get_result().
        /// </remarks>
        HouseArrestError house_arrest_send_command(HouseArrestClientHandle client, string command, string appid);
        
        /// <summary>
        /// Retrieves the result of a previously sent house_arrest_request_* request.
        /// </summary>
        /// <param name="client">
        /// The house_arrest client to use
        /// </param>
        /// <param name="dict">
        /// Pointer that will be set to a plist containing the result to
        /// the last performed operation. It holds a key 'Status' with the value
        /// 'Complete' on success or a key 'Error' with an error description as
        /// value. The caller is responsible for freeing the returned plist.
        /// </param>
        /// <returns>
        /// HOUSE_ARREST_E_SUCCESS if a result plist was retrieved,
        /// HOUSE_ARREST_E_INVALID_ARG if client is invalid,
        /// HOUSE_ARREST_E_INVALID_MODE if the client is not in the correct mode,
        /// or HOUSE_ARREST_E_CONN_FAILED if a connection error occured.
        /// </returns>
        HouseArrestError house_arrest_get_result(HouseArrestClientHandle client, out PlistHandle dict);
        
        /// <summary>
        /// Creates an AFC client using the given house_arrest client's connection
        /// allowing file access to a specific application directory requested by
        /// functions like house_arrest_request_vendor_documents().
        /// </summary>
        /// <param name="client">
        /// The house_arrest client to use.
        /// </param>
        /// <param name="afc_client">
        /// Pointer that will be set to a newly allocated afc_client_t
        /// upon successful return.
        /// </param>
        /// <returns>
        /// AFC_E_SUCCESS if the afc client was successfully created,
        /// AFC_E_INVALID_ARG if client is invalid or was already used to create
        /// an afc client, or an AFC_E_* error code returned by
        /// afc_client_new_with_service_client().
        /// </returns>
        /// <remarks>
        /// After calling this function the house_arrest client will go in an
        /// AFC mode that will only allow calling house_arrest_client_free().
        /// Only call house_arrest_client_free() if all AFC operations have
        /// completed since it will close the connection.
        /// </remarks>
        AfcError afc_client_new_from_house_arrest_client(HouseArrestClientHandle client, out AfcClientHandle afcClient);
    }
}
