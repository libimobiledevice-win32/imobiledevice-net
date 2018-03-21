// <copyright file="DebugServerApi.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.DebugServer
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial class DebugServerApi : IDebugServerApi
    {
        
        /// <summary>
        /// Backing field for the <see cref="Parent"/> property
        /// </summary>
        private ILibiMobileDevice parent;
        
        /// <summary>
        /// Initializes a new instance of the <see cref"DebugServerApi"/> class
        /// </summary>
        /// <param name="parent">
        /// The <see cref="ILibiMobileDeviceApi"/> which owns this <see cref="DebugServer"/>.
        /// </summary>
        public DebugServerApi(ILibiMobileDevice parent)
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
        /// Connects to the debugserver service on the specified device.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="service">
        /// The service descriptor returned by lockdownd_start_service.
        /// </param>
        /// <param name="client">
        /// Pointer that will point to a newly allocated
        /// debugserver_client_t upon successful return. Must be freed using
        /// debugserver_client_free() after use.
        /// </param>
        /// <returns>
        /// DEBUGSERVER_E_SUCCESS on success, DEBUGSERVER_E_INVALID_ARG when
        /// client is NULL, or an DEBUGSERVER_E_* error code otherwise.
        /// </returns>
        public virtual DebugServerError debugserver_client_new(iDeviceHandle device, LockdownServiceDescriptorHandle service, out DebugServerClientHandle client)
        {
            DebugServerError returnValue;
            returnValue = DebugServerNativeMethods.debugserver_client_new(device, service, out client);
            client.Api = this.Parent;
            return returnValue;
        }
        
        /// <summary>
        /// Starts a new debugserver service on the specified device and connects to it.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="client">
        /// Pointer that will point to a newly allocated
        /// debugserver_client_t upon successful return. Must be freed using
        /// debugserver_client_free() after use.
        /// </param>
        /// <param name="label">
        /// The label to use for communication. Usually the program name.
        /// Pass NULL to disable sending the label in requests to lockdownd.
        /// </param>
        /// <returns>
        /// DEBUGSERVER_E_SUCCESS on success, or an DEBUGSERVER_E_* error
        /// code otherwise.
        /// </returns>
        public virtual DebugServerError debugserver_client_start_service(iDeviceHandle device, out DebugServerClientHandle client, string label)
        {
            DebugServerError returnValue;
            returnValue = DebugServerNativeMethods.debugserver_client_start_service(device, out client, label);
            client.Api = this.Parent;
            return returnValue;
        }
        
        /// <summary>
        /// Disconnects a debugserver client from the device and frees up the
        /// debugserver client data.
        /// </summary>
        /// <param name="client">
        /// The debugserver client to disconnect and free.
        /// </param>
        /// <returns>
        /// DEBUGSERVER_E_SUCCESS on success, DEBUGSERVER_E_INVALID_ARG when
        /// client is NULL, or an DEBUGSERVER_E_* error code otherwise.
        /// </returns>
        public virtual DebugServerError debugserver_client_free(System.IntPtr client)
        {
            return DebugServerNativeMethods.debugserver_client_free(client);
        }
        
        /// <summary>
        /// Sends raw data using the given debugserver service client.
        /// </summary>
        /// <param name="client">
        /// The debugserver client to use for sending
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
        /// DEBUGSERVER_E_SUCCESS on success,
        /// DEBUGSERVER_E_INVALID_ARG when one or more parameters are
        /// invalid, or DEBUGSERVER_E_UNKNOWN_ERROR when an unspecified
        /// error occurs.
        /// </returns>
        public virtual DebugServerError debugserver_client_send(DebugServerClientHandle client, byte[] data, uint size, ref uint sent)
        {
            return DebugServerNativeMethods.debugserver_client_send(client, data, size, ref sent);
        }
        
        /// <summary>
        /// Receives raw data using the given debugserver client with specified timeout.
        /// </summary>
        /// <param name="client">
        /// The debugserver client to use for receiving
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
        /// DEBUGSERVER_E_SUCCESS on success,
        /// DEBUGSERVER_E_INVALID_ARG when one or more parameters are
        /// invalid, DEBUGSERVER_E_MUX_ERROR when a communication error
        /// occurs, or DEBUGSERVER_E_UNKNOWN_ERROR when an unspecified
        /// error occurs.
        /// </returns>
        public virtual DebugServerError debugserver_client_receive_with_timeout(DebugServerClientHandle client, byte[] data, uint size, ref uint received, uint timeout)
        {
            return DebugServerNativeMethods.debugserver_client_receive_with_timeout(client, data, size, ref received, timeout);
        }
        
        /// <summary>
        /// Receives raw data from the debugserver service.
        /// </summary>
        /// <param name="client">
        /// The debugserver client
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
        /// DEBUGSERVER_E_SUCCESS on success,
        /// DEBUGSERVER_E_INVALID_ARG when client or plist is NULL
        /// </returns>
        /// <remarks>
        /// The default read timeout is 10 seconds.
        /// </remarks>
        public virtual DebugServerError debugserver_client_receive(DebugServerClientHandle client, byte[] data, uint size, ref uint received)
        {
            return DebugServerNativeMethods.debugserver_client_receive(client, data, size, ref received);
        }
        
        /// <summary>
        /// Sends a command to the debugserver service.
        /// </summary>
        /// <param name="client">
        /// The debugserver client
        /// </param>
        /// <param name="command">
        /// Command to process and send
        /// </param>
        /// <param name="response">
        /// Response received for the command (can be NULL to ignore)
        /// </param>
        /// <returns>
        /// DEBUGSERVER_E_SUCCESS on success,
        /// DEBUGSERVER_E_INVALID_ARG when client or command is NULL
        /// </returns>
        public virtual DebugServerError debugserver_client_send_command(DebugServerClientHandle client, DebugServerCommandHandle command, out string response)
        {
            return DebugServerNativeMethods.debugserver_client_send_command(client, command, out response);
        }
        
        /// <summary>
        /// Receives and parses response of debugserver service.
        /// </summary>
        /// <param name="client">
        /// The debugserver client
        /// </param>
        /// <param name="response">
        /// Response received for last command (can be NULL to ignore)
        /// </param>
        /// <returns>
        /// DEBUGSERVER_E_SUCCESS on success,
        /// DEBUGSERVER_E_INVALID_ARG when client is NULL
        /// </returns>
        public virtual DebugServerError debugserver_client_receive_response(DebugServerClientHandle client, out string response)
        {
            return DebugServerNativeMethods.debugserver_client_receive_response(client, out response);
        }
        
        /// <summary>
        /// Controls status of ACK mode when sending commands or receiving responses.
        /// </summary>
        /// <param name="client">
        /// The debugserver client
        /// </param>
        /// <param name="enabled">
        /// A boolean flag indicating whether the internal ACK mode
        /// handling should be enabled or disabled.
        /// </param>
        /// <returns>
        /// DEBUGSERVER_E_SUCCESS on success, or an DEBUGSERVER_E_* error
        /// code otherwise.
        /// </returns>
        public virtual DebugServerError debugserver_client_set_ack_mode(DebugServerClientHandle client, int enabled)
        {
            return DebugServerNativeMethods.debugserver_client_set_ack_mode(client, enabled);
        }
        
        /// <summary>
        /// Sets the argv which launches an app.
        /// </summary>
        /// <param name="client">
        /// The debugserver client
        /// </param>
        /// <param name="argc">
        /// Number of arguments
        /// </param>
        /// <param name="argv">
        /// Array starting with the executable to be run followed by it's arguments
        /// </param>
        /// <param name="response">
        /// Response received for the command (can be NULL to ignore)
        /// </param>
        /// <returns>
        /// DEBUGSERVER_E_SUCCESS on success,
        /// DEBUGSERVER_E_INVALID_ARG when client is NULL
        /// </returns>
        public virtual DebugServerError debugserver_client_set_argv(DebugServerClientHandle client, int argc, System.Collections.ObjectModel.ReadOnlyCollection<string> argv, out string response)
        {
            return DebugServerNativeMethods.debugserver_client_set_argv(client, argc, argv, out response);
        }
        
        /// <summary>
        /// Adds or sets an environment variable.
        /// </summary>
        /// <param name="client">
        /// The debugserver client
        /// </param>
        /// <param name="env">
        /// The environment variable in "KEY=VALUE" notation
        /// </param>
        /// <param name="response">
        /// Response received for the command (can be NULL to ignore)
        /// </param>
        /// <returns>
        /// DEBUGSERVER_E_SUCCESS on success,
        /// DEBUGSERVER_E_INVALID_ARG when client is NULL
        /// </returns>
        public virtual DebugServerError debugserver_client_set_environment_hex_encoded(DebugServerClientHandle client, string env, out string response)
        {
            return DebugServerNativeMethods.debugserver_client_set_environment_hex_encoded(client, env, out response);
        }
        
        /// <summary>
        /// Creates and initializes a new command object.
        /// </summary>
        /// <param name="name">
        /// The name of the command which is sent in plain text
        /// </param>
        /// <param name="argv">
        /// Array of tokens for the command ment to be encoded
        /// </param>
        /// <param name="argc">
        /// Number of items in the token array
        /// </param>
        /// <param name="command">
        /// New command object
        /// </param>
        /// <returns>
        /// DEBUGSERVER_E_SUCCESS on success,
        /// DEBUGSERVER_E_INVALID_ARG when name or command is NULL
        /// </returns>
        public virtual DebugServerError debugserver_command_new(string name, int argc, System.Collections.ObjectModel.ReadOnlyCollection<string> argv, out DebugServerCommandHandle command)
        {
            DebugServerError returnValue;
            returnValue = DebugServerNativeMethods.debugserver_command_new(name, argc, argv, out command);
            command.Api = this.Parent;
            return returnValue;
        }
        
        /// <summary>
        /// Frees memory of command object.
        /// </summary>
        /// <param name="command">
        /// The command object
        /// </param>
        /// <returns>
        /// DEBUGSERVER_E_SUCCESS on success,
        /// DEBUGSERVER_E_INVALID_ARG when command is NULL
        /// </returns>
        public virtual DebugServerError debugserver_command_free(System.IntPtr command)
        {
            return DebugServerNativeMethods.debugserver_command_free(command);
        }
        
        /// <summary>
        /// Encodes a string into hex notation.
        /// </summary>
        /// <param name="buffer">
        /// String to encode into hex notiation
        /// </param>
        /// <param name="encoded_buffer">
        /// The buffer receives a hex encoded string
        /// </param>
        /// <param name="encoded_length">
        /// Length of the hex encoded string
        /// </param>
        public virtual void debugserver_encode_string(string buffer, out string encodedBuffer, ref uint encodedLength)
        {
            DebugServerNativeMethods.debugserver_encode_string(buffer, out encodedBuffer, ref encodedLength);
        }
        
        /// <summary>
        /// Decodes a hex encoded string.
        /// </summary>
        /// <param name="encoded_buffer">
        /// The buffer with a hex encoded string
        /// </param>
        /// <param name="encoded_length">
        /// Length of the encoded buffer
        /// </param>
        /// <param name="buffer">
        /// Decoded string to be freed by the caller
        /// </param>
        public virtual void debugserver_decode_string(string encodedBuffer, uint encodedLength, out string buffer)
        {
            DebugServerNativeMethods.debugserver_decode_string(encodedBuffer, encodedLength, out buffer);
        }
    }
}
