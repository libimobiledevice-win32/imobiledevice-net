// <copyright file="MisagentApi.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Misagent
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial class MisagentApi : IMisagentApi
    {
        
        /// <summary>
        /// Backing field for the <see cref="Parent"/> property
        /// </summary>
        private ILibiMobileDevice parent;
        
        /// <summary>
        /// Initializes a new instance of the <see cref"MisagentApi"/> class
        /// </summary>
        /// <param name="parent">
        /// The <see cref="ILibiMobileDeviceApi"/> which owns this <see cref="Misagent"/>.
        /// </summary>
        public MisagentApi(ILibiMobileDevice parent)
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
        /// Connects to the misagent service on the specified device.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="service">
        /// The service descriptor returned by lockdownd_start_service.
        /// </param>
        /// <param name="client">
        /// Pointer that will point to a newly allocated
        /// misagent_client_t upon successful return.
        /// </param>
        /// <returns>
        /// MISAGENT_E_SUCCESS on success, MISAGENT_E_INVALID_ARG when
        /// client is NULL, or an MISAGENT_E_* error code otherwise.
        /// </returns>
        public virtual MisagentError misagent_client_new(iDeviceHandle device, LockdownServiceDescriptorHandle service, out MisagentClientHandle client)
        {
            MisagentError returnValue;
            returnValue = MisagentNativeMethods.misagent_client_new(device, service, out client);
            client.Api = this.Parent;
            return returnValue;
        }
        
        /// <summary>
        /// Starts a new misagent service on the specified device and connects to it.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="client">
        /// Pointer that will point to a newly allocated
        /// misagent_client_t upon successful return. Must be freed using
        /// misagent_client_free() after use.
        /// </param>
        /// <param name="label">
        /// The label to use for communication. Usually the program name.
        /// Pass NULL to disable sending the label in requests to lockdownd.
        /// </param>
        /// <returns>
        /// MISAGENT_E_SUCCESS on success, or an MISAGENT_E_* error
        /// code otherwise.
        /// </returns>
        public virtual MisagentError misagent_client_start_service(iDeviceHandle device, out MisagentClientHandle client, string label)
        {
            MisagentError returnValue;
            returnValue = MisagentNativeMethods.misagent_client_start_service(device, out client, label);
            client.Api = this.Parent;
            return returnValue;
        }
        
        /// <summary>
        /// Disconnects an misagent client from the device and frees up the
        /// misagent client data.
        /// </summary>
        /// <param name="client">
        /// The misagent client to disconnect and free.
        /// </param>
        /// <returns>
        /// MISAGENT_E_SUCCESS on success, MISAGENT_E_INVALID_ARG when
        /// client is NULL, or an MISAGENT_E_* error code otherwise.
        /// </returns>
        public virtual MisagentError misagent_client_free(System.IntPtr client)
        {
            return MisagentNativeMethods.misagent_client_free(client);
        }
        
        /// <summary>
        /// Installs the given provisioning profile. Only works with valid profiles.
        /// </summary>
        /// <param name="client">
        /// The connected misagent to use for installation
        /// </param>
        /// <param name="profile">
        /// The valid provisioning profile to install. This has to be
        /// passed as a PLIST_DATA, otherwise the function will fail.
        /// </param>
        /// <returns>
        /// MISAGENT_E_SUCCESS on success, MISAGENT_E_INVALID_ARG when
        /// client is invalid, or an MISAGENT_E_* error code otherwise.
        /// </returns>
        public virtual MisagentError misagent_install(MisagentClientHandle client, PlistHandle profile)
        {
            return MisagentNativeMethods.misagent_install(client, profile);
        }
        
        /// <summary>
        /// Retrieves all installed provisioning profiles (iOS 9.2.1 or below).
        /// </summary>
        /// <param name="client">
        /// The connected misagent to use.
        /// </param>
        /// <param name="profiles">
        /// Pointer to a plist_t that will be set to a PLIST_ARRAY
        /// if the function is successful.
        /// </param>
        /// <returns>
        /// MISAGENT_E_SUCCESS on success, MISAGENT_E_INVALID_ARG when
        /// client is invalid, or an MISAGENT_E_* error code otherwise.
        /// </returns>
        /// <remarks>
        /// This API call only works with iOS 9.2.1 or below.
        /// For newer iOS versions use misagent_copy_all() instead.
        /// If no provisioning profiles are installed on the device, this function
        /// still returns MISAGENT_E_SUCCESS and profiles will just point to an
        /// empty array.
        /// </remarks>
        public virtual MisagentError misagent_copy(MisagentClientHandle client, out PlistHandle profiles)
        {
            MisagentError returnValue;
            returnValue = MisagentNativeMethods.misagent_copy(client, out profiles);
            profiles.Api = this.Parent;
            return returnValue;
        }
        
        /// <summary>
        /// Retrieves all installed provisioning profiles (iOS 9.3 or higher).
        /// </summary>
        /// <param name="client">
        /// The connected misagent to use.
        /// </param>
        /// <param name="profiles">
        /// Pointer to a plist_t that will be set to a PLIST_ARRAY
        /// if the function is successful.
        /// </param>
        /// <returns>
        /// MISAGENT_E_SUCCESS on success, MISAGENT_E_INVALID_ARG when
        /// client is invalid, or an MISAGENT_E_* error code otherwise.
        /// </returns>
        /// <remarks>
        /// This API call only works with iOS 9.3 or higher.
        /// For older iOS versions use misagent_copy() instead.
        /// If no provisioning profiles are installed on the device, this function
        /// still returns MISAGENT_E_SUCCESS and profiles will just point to an
        /// empty array.
        /// </remarks>
        public virtual MisagentError misagent_copy_all(MisagentClientHandle client, out PlistHandle profiles)
        {
            MisagentError returnValue;
            returnValue = MisagentNativeMethods.misagent_copy_all(client, out profiles);
            profiles.Api = this.Parent;
            return returnValue;
        }
        
        /// <summary>
        /// Removes a given provisioning profile.
        /// </summary>
        /// <param name="client">
        /// The connected misagent to use.
        /// </param>
        /// <param name="profileID">
        /// Identifier of the provisioning profile to remove.
        /// This is a UUID that can be obtained from the provisioning profile data.
        /// </param>
        /// <returns>
        /// MISAGENT_E_SUCCESS on success, MISAGENT_E_INVALID_ARG when
        /// client is invalid, or an MISAGENT_E_* error code otherwise.
        /// </returns>
        public virtual MisagentError misagent_remove(MisagentClientHandle client, string profileid)
        {
            return MisagentNativeMethods.misagent_remove(client, profileid);
        }
        
        /// <summary>
        /// Retrieves the status code from the last operation.
        /// </summary>
        /// <param name="client">
        /// The misagent to use.
        /// </param>
        /// <returns>
        /// -1 if client is invalid, or the status code from the last operation
        /// </returns>
        public virtual int misagent_get_status_code(MisagentClientHandle client)
        {
            return MisagentNativeMethods.misagent_get_status_code(client);
        }
    }
}
