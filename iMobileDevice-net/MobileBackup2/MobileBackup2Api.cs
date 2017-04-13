// <copyright file="MobileBackup2Api.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.MobileBackup2
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial class MobileBackup2Api : IMobileBackup2Api
    {
        
        /// <summary>
        /// Backing field for the <see cref="Parent"/> property
        /// </summary>
        private ILibiMobileDevice parent;
        
        /// <summary>
        /// Initializes a new instance of the <see cref"MobileBackup2Api"/> class
        /// </summary>
        /// <param name="parent">
        /// The <see cref="ILibiMobileDeviceApi"/> which owns this <see cref="MobileBackup2"/>.
        /// </summary>
        public MobileBackup2Api(ILibiMobileDevice parent)
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
        /// Connects to the mobilebackup2 service on the specified device.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="service">
        /// The service descriptor returned by lockdownd_start_service.
        /// </param>
        /// <param name="client">
        /// Pointer that will be set to a newly allocated
        /// mobilebackup2_client_t upon successful return.
        /// </param>
        /// <returns>
        /// MOBILEBACKUP2_E_SUCCESS on success, MOBILEBACKUP2_E_INVALID ARG
        /// if one or more parameter is invalid, or MOBILEBACKUP2_E_BAD_VERSION
        /// if the mobilebackup2 version on the device is newer.
        /// </returns>
        public virtual MobileBackup2Error mobilebackup2_client_new(iDeviceHandle device, LockdownServiceDescriptorHandle service, out MobileBackup2ClientHandle client)
        {
            MobileBackup2Error returnValue;
            returnValue = MobileBackup2NativeMethods.mobilebackup2_client_new(device, service, out client);
            client.Api = this.Parent;
            return returnValue;
        }
        
        /// <summary>
        /// Starts a new mobilebackup2 service on the specified device and connects to it.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="client">
        /// Pointer that will point to a newly allocated
        /// mobilebackup2_client_t upon successful return. Must be freed using
        /// mobilebackup2_client_free() after use.
        /// </param>
        /// <param name="label">
        /// The label to use for communication. Usually the program name.
        /// Pass NULL to disable sending the label in requests to lockdownd.
        /// </param>
        /// <returns>
        /// MOBILEBACKUP2_E_SUCCESS on success, or an MOBILEBACKUP2_E_* error
        /// code otherwise.
        /// </returns>
        public virtual MobileBackup2Error mobilebackup2_client_start_service(iDeviceHandle device, out MobileBackup2ClientHandle client, string label)
        {
            MobileBackup2Error returnValue;
            returnValue = MobileBackup2NativeMethods.mobilebackup2_client_start_service(device, out client, label);
            client.Api = this.Parent;
            return returnValue;
        }
        
        /// <summary>
        /// Disconnects a mobilebackup2 client from the device and frees up the
        /// mobilebackup2 client data.
        /// </summary>
        /// <param name="client">
        /// The mobilebackup2 client to disconnect and free.
        /// </param>
        /// <returns>
        /// MOBILEBACKUP2_E_SUCCESS on success, or MOBILEBACKUP2_E_INVALID_ARG
        /// if client is NULL.
        /// </returns>
        public virtual MobileBackup2Error mobilebackup2_client_free(System.IntPtr client)
        {
            return MobileBackup2NativeMethods.mobilebackup2_client_free(client);
        }
        
        /// <summary>
        /// Sends a backup message plist.
        /// </summary>
        /// <param name="client">
        /// The connected MobileBackup client to use.
        /// </param>
        /// <param name="message">
        /// The message to send. This will be inserted into the request
        /// plist as value for MessageName. If this parameter is NULL,
        /// the plist passed in the options parameter will be sent directly.
        /// </param>
        /// <param name="options">
        /// Additional options as PLIST_DICT to add to the request.
        /// The MessageName key with the value passed in the message parameter
        /// will be inserted into this plist before sending it. This parameter
        /// can be NULL if message is not NULL.
        /// </param>
        public virtual MobileBackup2Error mobilebackup2_send_message(MobileBackup2ClientHandle client, string message, PlistHandle options)
        {
            return MobileBackup2NativeMethods.mobilebackup2_send_message(client, message, options);
        }
        
        /// <summary>
        /// Receives a DL* message plist from the device.
        /// This function is a wrapper around device_link_service_receive_message.
        /// </summary>
        /// <param name="client">
        /// The connected MobileBackup client to use.
        /// </param>
        /// <param name="msg_plist">
        /// Pointer to a plist that will be set to the contents of the
        /// message plist upon successful return.
        /// </param>
        /// <param name="dlmessage">
        /// A pointer that will be set to a newly allocated char*
        /// containing the DL* string from the given plist. It is up to the caller
        /// to free the allocated memory. If this parameter is NULL
        /// it will be ignored.
        /// </param>
        /// <returns>
        /// MOBILEBACKUP2_E_SUCCESS if a DL* message was received,
        /// MOBILEBACKUP2_E_INVALID_ARG if client or message is invalid,
        /// MOBILEBACKUP2_E_PLIST_ERROR if the received plist is invalid
        /// or is not a DL* message plist, or MOBILEBACKUP2_E_MUX_ERROR if
        /// receiving from the device failed.
        /// </returns>
        public virtual MobileBackup2Error mobilebackup2_receive_message(MobileBackup2ClientHandle client, out PlistHandle msgPlist, out string dlmessage)
        {
            MobileBackup2Error returnValue;
            returnValue = MobileBackup2NativeMethods.mobilebackup2_receive_message(client, out msgPlist, out dlmessage);
            msgPlist.Api = this.Parent;
            return returnValue;
        }
        
        /// <summary>
        /// Send binary data to the device.
        /// </summary>
        /// <param name="client">
        /// The MobileBackup client to send to.
        /// </param>
        /// <param name="data">
        /// Pointer to the data to send
        /// </param>
        /// <param name="length">
        /// Number of bytes to send
        /// </param>
        /// <param name="bytes">
        /// Number of bytes actually sent
        /// </param>
        /// <returns>
        /// MOBILEBACKUP2_E_SUCCESS if any data was successfully sent,
        /// MOBILEBACKUP2_E_INVALID_ARG if one of the parameters is invalid,
        /// or MOBILEBACKUP2_E_MUX_ERROR if sending of the data failed.
        /// </returns>
        /// <remarks>
        /// This function returns MOBILEBACKUP2_E_SUCCESS even if less than the
        /// requested length has been sent. The fourth parameter is required and
        /// must be checked to ensure if the whole data has been sent.
        /// </remarks>
        public virtual MobileBackup2Error mobilebackup2_send_raw(MobileBackup2ClientHandle client, byte[] data, uint length, ref uint bytes)
        {
            return MobileBackup2NativeMethods.mobilebackup2_send_raw(client, data, length, ref bytes);
        }
        
        /// <summary>
        /// Receive binary from the device.
        /// </summary>
        /// <param name="client">
        /// The MobileBackup client to receive from.
        /// </param>
        /// <param name="data">
        /// Pointer to a buffer that will be filled with the received data.
        /// </param>
        /// <param name="length">
        /// Number of bytes to receive. The data buffer needs to be large
        /// enough to store this amount of data.
        /// </param>
        /// <param name="bytes">
        /// Number of bytes actually received.
        /// </param>
        /// <returns>
        /// MOBILEBACKUP2_E_SUCCESS if any or no data was received,
        /// MOBILEBACKUP2_E_INVALID_ARG if one of the parameters is invalid,
        /// or MOBILEBACKUP2_E_MUX_ERROR if receiving the data failed.
        /// </returns>
        /// <remarks>
        /// This function returns MOBILEBACKUP2_E_SUCCESS even if no data
        /// has been received (unless a communication error occured).
        /// The fourth parameter is required and must be checked to know how
        /// many bytes were actually received.
        /// </remarks>
        public virtual MobileBackup2Error mobilebackup2_receive_raw(MobileBackup2ClientHandle client, byte[] data, uint length, ref uint bytes)
        {
            return MobileBackup2NativeMethods.mobilebackup2_receive_raw(client, data, length, ref bytes);
        }
        
        /// <summary>
        /// Performs the mobilebackup2 protocol version exchange.
        /// </summary>
        /// <param name="client">
        /// The MobileBackup client to use.
        /// </param>
        /// <param name="local_versions">
        /// An array of supported versions to send to the remote.
        /// </param>
        /// <param name="count">
        /// The number of items in local_versions.
        /// </param>
        /// <param name="remote_version">
        /// Holds the protocol version of the remote on success.
        /// </param>
        /// <returns>
        /// MOBILEBACKUP2_E_SUCCESS on success, or a MOBILEBACKUP2_E_* error
        /// code otherwise.
        /// </returns>
        public virtual MobileBackup2Error mobilebackup2_version_exchange(MobileBackup2ClientHandle client, System.IntPtr localVersions, sbyte count, ref double remoteVersion)
        {
            return MobileBackup2NativeMethods.mobilebackup2_version_exchange(client, localVersions, count, ref remoteVersion);
        }
        
        /// <summary>
        /// Send a request to the connected mobilebackup2 service.
        /// </summary>
        /// <param name="request">
        /// The request to send to the backup service.
        /// Currently, this is one of "Backup", "Restore", "Info", or "List".
        /// </param>
        /// <param name="target_identifier">
        /// UDID of the target device.
        /// </param>
        /// <param name="source_identifier">
        /// UDID of backup data?
        /// </param>
        /// <param name="options">
        /// Additional options in a plist of type PLIST_DICT.
        /// </param>
        /// <returns>
        /// MOBILEBACKUP2_E_SUCCESS if the request was successfully sent,
        /// or a MOBILEBACKUP2_E_* error value otherwise.
        /// </returns>
        public virtual MobileBackup2Error mobilebackup2_send_request(MobileBackup2ClientHandle client, string request, string targetIdentifier, string sourceIdentifier, PlistHandle options)
        {
            return MobileBackup2NativeMethods.mobilebackup2_send_request(client, request, targetIdentifier, sourceIdentifier, options);
        }
        
        /// <summary>
        /// Sends a DLMessageStatusResponse to the device.
        /// </summary>
        /// <param name="client">
        /// The MobileBackup client to use.
        /// </param>
        /// <param name="status_code">
        /// The status code to send.
        /// </param>
        /// <param name="status1">
        /// A status message to send. Can be NULL if not required.
        /// </param>
        /// <param name="status2">
        /// An additional status plist to attach to the response.
        /// Can be NULL if not required.
        /// </param>
        /// <returns>
        /// MOBILEBACKUP2_E_SUCCESS on success, MOBILEBACKUP2_E_INVALID_ARG
        /// if client is invalid, or another MOBILEBACKUP2_E_* otherwise.
        /// </returns>
        public virtual MobileBackup2Error mobilebackup2_send_status_response(MobileBackup2ClientHandle client, int statusCode, string status1, PlistHandle status2)
        {
            return MobileBackup2NativeMethods.mobilebackup2_send_status_response(client, statusCode, status1, status2);
        }
    }
}
