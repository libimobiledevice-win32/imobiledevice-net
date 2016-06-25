// <copyright file="RestoreApi.cs" company="Quamotion">
// Copyright (c) 2016 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Restore
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial class RestoreApi : IRestoreApi
    {
        
        /// <summary>
        /// Creates a new restored client for the device.
        /// </summary>
        /// <param name="device">
        /// The device to create a restored client for
        /// </param>
        /// <param name="client">
        /// The pointer to the location of the new restored_client
        /// </param>
        /// <param name="label">
        /// The label to use for communication. Usually the program name.
        /// </param>
        /// <returns>
        /// RESTORE_E_SUCCESS on success, RESTORE_E_INVALID_ARG when client is NULL
        /// </returns>
        public virtual RestoreError restored_client_new(iDeviceHandle device, out RestoreClientHandle client, string label)
        {
            return RestoreNativeMethods.restored_client_new(device, out client, label);
        }
        
        /// <summary>
        /// Closes the restored client session if one is running and frees up the
        /// restored_client struct.
        /// </summary>
        /// <param name="client">
        /// The restore client
        /// </param>
        /// <returns>
        /// RESTORE_E_SUCCESS on success, RESTORE_E_INVALID_ARG when client is NULL
        /// </returns>
        public virtual RestoreError restored_client_free(System.IntPtr client)
        {
            return RestoreNativeMethods.restored_client_free(client);
        }
        
        /// <summary>
        /// Query the type of the service daemon. Depending on whether the device is
        /// queried in normal mode or restore mode, different types will be returned.
        /// </summary>
        /// <param name="client">
        /// The restored client
        /// </param>
        /// <param name="type">
        /// The type returned by the service daemon. Pass NULL to ignore.
        /// </param>
        /// <param name="version">
        /// The restore protocol version. Pass NULL to ignore.
        /// </param>
        /// <returns>
        /// RESTORE_E_SUCCESS on success, RESTORE_E_INVALID_ARG when client is NULL
        /// </returns>
        public virtual RestoreError restored_query_type(RestoreClientHandle client, out string type, ref ulong version)
        {
            return RestoreNativeMethods.restored_query_type(client, out type, ref version);
        }
        
        /// <summary>
        /// Queries a value from the device specified by a key.
        /// </summary>
        /// <param name="client">
        /// An initialized restored client.
        /// </param>
        /// <param name="key">
        /// The key name to request
        /// </param>
        /// <param name="value">
        /// A plist node representing the result value node
        /// </param>
        /// <returns>
        /// RESTORE_E_SUCCESS on success, RESTORE_E_INVALID_ARG when client is NULL, RESTORE_E_PLIST_ERROR if value for key can't be found
        /// </returns>
        public virtual RestoreError restored_query_value(RestoreClientHandle client, string key, out PlistHandle value)
        {
            return RestoreNativeMethods.restored_query_value(client, key, out value);
        }
        
        /// <summary>
        /// Retrieves a value from information plist specified by a key.
        /// </summary>
        /// <param name="client">
        /// An initialized restored client.
        /// </param>
        /// <param name="key">
        /// The key name to request or NULL to query for all keys
        /// </param>
        /// <param name="value">
        /// A plist node representing the result value node
        /// </param>
        /// <returns>
        /// RESTORE_E_SUCCESS on success, RESTORE_E_INVALID_ARG when client is NULL, RESTORE_E_PLIST_ERROR if value for key can't be found
        /// </returns>
        public virtual RestoreError restored_get_value(RestoreClientHandle client, string key, out PlistHandle value)
        {
            return RestoreNativeMethods.restored_get_value(client, key, out value);
        }
        
        /// <summary>
        /// Sends a plist to restored.
        /// </summary>
        /// <param name="client">
        /// The restored client
        /// </param>
        /// <param name="plist">
        /// The plist to send
        /// </param>
        /// <returns>
        /// RESTORE_E_SUCCESS on success, RESTORE_E_INVALID_ARG when client or
        /// plist is NULL
        /// </returns>
        /// <remarks>
        /// This function is low-level and should only be used if you need to send
        /// a new type of message.
        /// </remarks>
        public virtual RestoreError restored_send(RestoreClientHandle client, PlistHandle plist)
        {
            return RestoreNativeMethods.restored_send(client, plist);
        }
        
        /// <summary>
        /// Receives a plist from restored.
        /// </summary>
        /// <param name="client">
        /// The restored client
        /// </param>
        /// <param name="plist">
        /// The plist to store the received data
        /// </param>
        /// <returns>
        /// RESTORE_E_SUCCESS on success, RESTORE_E_INVALID_ARG when client or
        /// plist is NULL
        /// </returns>
        public virtual RestoreError restored_receive(RestoreClientHandle client, out PlistHandle plist)
        {
            return RestoreNativeMethods.restored_receive(client, out plist);
        }
        
        /// <summary>
        /// Sends the Goodbye request to restored signaling the end of communication.
        /// </summary>
        /// <param name="client">
        /// The restore client
        /// </param>
        /// <returns>
        /// RESTORE_E_SUCCESS on success, RESTORE_E_INVALID_ARG when client is NULL,
        /// RESTORE_E_PLIST_ERROR if the device did not acknowledge the request
        /// </returns>
        public virtual RestoreError restored_goodbye(RestoreClientHandle client)
        {
            return RestoreNativeMethods.restored_goodbye(client);
        }
        
        /// <summary>
        /// Requests to start a restore and retrieve it's port on success.
        /// </summary>
        /// <param name="client">
        /// The restored client
        /// </param>
        /// <param name="options">
        /// PLIST_DICT with options for the restore process or NULL
        /// </param>
        /// <param name="version">
        /// the restore protocol version, see restored_query_type()
        /// </param>
        /// <returns>
        /// RESTORE_E_SUCCESS on success, RESTORE_E_INVALID_ARG if a parameter
        /// is NULL, RESTORE_E_START_RESTORE_FAILED if the request fails
        /// </returns>
        public virtual RestoreError restored_start_restore(RestoreClientHandle client, PlistHandle options, ulong version)
        {
            return RestoreNativeMethods.restored_start_restore(client, options, version);
        }
        
        /// <summary>
        /// Requests device to reboot.
        /// </summary>
        /// <param name="client">
        /// The restored client
        /// </param>
        /// <returns>
        /// RESTORE_E_SUCCESS on success, RESTORE_E_INVALID_ARG if a parameter
        /// is NULL
        /// </returns>
        public virtual RestoreError restored_reboot(RestoreClientHandle client)
        {
            return RestoreNativeMethods.restored_reboot(client);
        }
        
        /// <summary>
        /// Sets the label to send for requests to restored.
        /// </summary>
        /// <param name="client">
        /// The restore client
        /// </param>
        /// <param name="label">
        /// The label to set or NULL to disable sending a label
        /// </param>
        public virtual void restored_client_set_label(RestoreClientHandle client, string label)
        {
            RestoreNativeMethods.restored_client_set_label(client, label);
        }
    }
}
