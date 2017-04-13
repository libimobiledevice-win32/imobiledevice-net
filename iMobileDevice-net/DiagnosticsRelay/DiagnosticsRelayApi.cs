// <copyright file="DiagnosticsRelayApi.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.DiagnosticsRelay
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial class DiagnosticsRelayApi : IDiagnosticsRelayApi
    {
        
        /// <summary>
        /// Backing field for the <see cref="Parent"/> property
        /// </summary>
        private ILibiMobileDevice parent;
        
        /// <summary>
        /// Initializes a new instance of the <see cref"DiagnosticsRelayApi"/> class
        /// </summary>
        /// <param name="parent">
        /// The <see cref="ILibiMobileDeviceApi"/> which owns this <see cref="DiagnosticsRelay"/>.
        /// </summary>
        public DiagnosticsRelayApi(ILibiMobileDevice parent)
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
        /// Connects to the diagnostics_relay service on the specified device.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="service">
        /// The service descriptor returned by lockdownd_start_service.
        /// </param>
        /// <param name="client">
        /// Reference that will point to a newly allocated
        /// diagnostics_relay_client_t upon successful return.
        /// </param>
        /// <returns>
        /// DIAGNOSTICS_RELAY_E_SUCCESS on success,
        /// DIAGNOSTICS_RELAY_E_INVALID_ARG when one of the parameters is invalid,
        /// or DIAGNOSTICS_RELAY_E_MUX_ERROR when the connection failed.
        /// </returns>
        public virtual DiagnosticsRelayError diagnostics_relay_client_new(iDeviceHandle device, LockdownServiceDescriptorHandle service, out DiagnosticsRelayClientHandle client)
        {
            DiagnosticsRelayError returnValue;
            returnValue = DiagnosticsRelayNativeMethods.diagnostics_relay_client_new(device, service, out client);
            client.Api = this.Parent;
            return returnValue;
        }
        
        /// <summary>
        /// Starts a new diagnostics_relay service on the specified device and connects to it.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="client">
        /// Pointer that will point to a newly allocated
        /// diagnostics_relay_client_t upon successful return. Must be freed using
        /// diagnostics_relay_client_free() after use.
        /// </param>
        /// <param name="label">
        /// The label to use for communication. Usually the program name.
        /// Pass NULL to disable sending the label in requests to lockdownd.
        /// </param>
        /// <returns>
        /// DIAGNOSTICS_RELAY_E_SUCCESS on success, or an DIAGNOSTICS_RELAY_E_* error
        /// code otherwise.
        /// </returns>
        public virtual DiagnosticsRelayError diagnostics_relay_client_start_service(iDeviceHandle device, out DiagnosticsRelayClientHandle client, string label)
        {
            DiagnosticsRelayError returnValue;
            returnValue = DiagnosticsRelayNativeMethods.diagnostics_relay_client_start_service(device, out client, label);
            client.Api = this.Parent;
            return returnValue;
        }
        
        /// <summary>
        /// Disconnects a diagnostics_relay client from the device and frees up the
        /// diagnostics_relay client data.
        /// </summary>
        /// <param name="client">
        /// The diagnostics_relay client to disconnect and free.
        /// </param>
        /// <returns>
        /// DIAGNOSTICS_RELAY_E_SUCCESS on success,
        /// DIAGNOSTICS_RELAY_E_INVALID_ARG when one of client or client->parent
        /// is invalid, or DIAGNOSTICS_RELAY_E_UNKNOWN_ERROR when the was an
        /// error freeing the parent property_list_service client.
        /// </returns>
        public virtual DiagnosticsRelayError diagnostics_relay_client_free(System.IntPtr client)
        {
            return DiagnosticsRelayNativeMethods.diagnostics_relay_client_free(client);
        }
        
        /// <summary>
        /// Sends the Goodbye request signaling the end of communication.
        /// </summary>
        /// <param name="client">
        /// The diagnostics_relay client
        /// </param>
        /// <returns>
        /// DIAGNOSTICS_RELAY_E_SUCCESS on success,
        /// DIAGNOSTICS_RELAY_E_INVALID_ARG when client is NULL,
        /// DIAGNOSTICS_RELAY_E_PLIST_ERROR if the device did not acknowledge the
        /// request
        /// </returns>
        public virtual DiagnosticsRelayError diagnostics_relay_goodbye(DiagnosticsRelayClientHandle client)
        {
            return DiagnosticsRelayNativeMethods.diagnostics_relay_goodbye(client);
        }
        
        /// <summary>
        /// Puts the device into deep sleep mode and disconnects from host.
        /// </summary>
        /// <param name="client">
        /// The diagnostics_relay client
        /// </param>
        /// <returns>
        /// DIAGNOSTICS_RELAY_E_SUCCESS on success,
        /// DIAGNOSTICS_RELAY_E_INVALID_ARG when client is NULL,
        /// DIAGNOSTICS_RELAY_E_PLIST_ERROR if the device did not acknowledge the
        /// request
        /// </returns>
        public virtual DiagnosticsRelayError diagnostics_relay_sleep(DiagnosticsRelayClientHandle client)
        {
            return DiagnosticsRelayNativeMethods.diagnostics_relay_sleep(client);
        }
        
        /// <summary>
        /// Restart the device and optionally show a user notification.
        /// </summary>
        /// <param name="client">
        /// The diagnostics_relay client
        /// </param>
        /// <param name="flags">
        /// A binary flag combination of
        /// DIAGNOSTICS_RELAY_ACTION_FLAG_WAIT_FOR_DISCONNECT to wait until
        /// diagnostics_relay_client_free() disconnects before execution and
        /// DIAGNOSTICS_RELAY_ACTION_FLAG_DISPLAY_FAIL to show a "FAIL" dialog
        /// or DIAGNOSTICS_RELAY_ACTION_FLAG_DISPLAY_PASS to show an "OK" dialog
        /// </param>
        /// <returns>
        /// DIAGNOSTICS_RELAY_E_SUCCESS on success,
        /// DIAGNOSTICS_RELAY_E_INVALID_ARG when client is NULL,
        /// DIAGNOSTICS_RELAY_E_PLIST_ERROR if the device did not acknowledge the
        /// request
        /// </returns>
        public virtual DiagnosticsRelayError diagnostics_relay_restart(DiagnosticsRelayClientHandle client, DiagnosticsRelayAction flags)
        {
            return DiagnosticsRelayNativeMethods.diagnostics_relay_restart(client, flags);
        }
        
        /// <summary>
        /// Shutdown of the device and optionally show a user notification.
        /// </summary>
        /// <param name="client">
        /// The diagnostics_relay client
        /// </param>
        /// <param name="flags">
        /// A binary flag combination of
        /// DIAGNOSTICS_RELAY_ACTION_FLAG_WAIT_FOR_DISCONNECT to wait until
        /// diagnostics_relay_client_free() disconnects before execution and
        /// DIAGNOSTICS_RELAY_ACTION_FLAG_DISPLAY_FAIL to show a "FAIL" dialog
        /// or DIAGNOSTICS_RELAY_ACTION_FLAG_DISPLAY_PASS to show an "OK" dialog
        /// </param>
        /// <returns>
        /// DIAGNOSTICS_RELAY_E_SUCCESS on success,
        /// DIAGNOSTICS_RELAY_E_INVALID_ARG when client is NULL,
        /// DIAGNOSTICS_RELAY_E_PLIST_ERROR if the device did not acknowledge the
        /// request
        /// </returns>
        public virtual DiagnosticsRelayError diagnostics_relay_shutdown(DiagnosticsRelayClientHandle client, DiagnosticsRelayAction flags)
        {
            return DiagnosticsRelayNativeMethods.diagnostics_relay_shutdown(client, flags);
        }
        
        /// <summary>
        /// Shutdown of the device and optionally show a user notification.
        /// </summary>
        /// <param name="client">
        /// The diagnostics_relay client
        /// </param>
        /// <param name="flags">
        /// A binary flag combination of
        /// DIAGNOSTICS_RELAY_ACTION_FLAG_WAIT_FOR_DISCONNECT to wait until
        /// diagnostics_relay_client_free() disconnects before execution and
        /// DIAGNOSTICS_RELAY_ACTION_FLAG_DISPLAY_FAIL to show a "FAIL" dialog
        /// or DIAGNOSTICS_RELAY_ACTION_FLAG_DISPLAY_PASS to show an "OK" dialog
        /// </param>
        /// <returns>
        /// DIAGNOSTICS_RELAY_E_SUCCESS on success,
        /// DIAGNOSTICS_RELAY_E_INVALID_ARG when client is NULL,
        /// DIAGNOSTICS_RELAY_E_PLIST_ERROR if the device did not acknowledge the
        /// request
        /// </returns>
        public virtual DiagnosticsRelayError diagnostics_relay_request_diagnostics(DiagnosticsRelayClientHandle client, string type, out PlistHandle diagnostics)
        {
            DiagnosticsRelayError returnValue;
            returnValue = DiagnosticsRelayNativeMethods.diagnostics_relay_request_diagnostics(client, type, out diagnostics);
            diagnostics.Api = this.Parent;
            return returnValue;
        }
        
        public virtual DiagnosticsRelayError diagnostics_relay_query_mobilegestalt(DiagnosticsRelayClientHandle client, PlistHandle keys, out PlistHandle result)
        {
            DiagnosticsRelayError returnValue;
            returnValue = DiagnosticsRelayNativeMethods.diagnostics_relay_query_mobilegestalt(client, keys, out result);
            result.Api = this.Parent;
            return returnValue;
        }
        
        public virtual DiagnosticsRelayError diagnostics_relay_query_ioregistry_entry(DiagnosticsRelayClientHandle client, string name, string classname, out PlistHandle result)
        {
            DiagnosticsRelayError returnValue;
            returnValue = DiagnosticsRelayNativeMethods.diagnostics_relay_query_ioregistry_entry(client, name, classname, out result);
            result.Api = this.Parent;
            return returnValue;
        }
        
        public virtual DiagnosticsRelayError diagnostics_relay_query_ioregistry_plane(DiagnosticsRelayClientHandle client, string plane, out PlistHandle result)
        {
            DiagnosticsRelayError returnValue;
            returnValue = DiagnosticsRelayNativeMethods.diagnostics_relay_query_ioregistry_plane(client, plane, out result);
            result.Api = this.Parent;
            return returnValue;
        }
    }
}
