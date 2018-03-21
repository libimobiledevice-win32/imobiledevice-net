// <copyright file="DebugServerNativeMethods.cs" company="Quamotion">
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
    
    
    public partial class DebugServerNativeMethods
    {
        
        public static DebugServerError debugserver_client_send_command(DebugServerClientHandle client, DebugServerCommandHandle command, out string response)
        {
            System.Runtime.InteropServices.ICustomMarshaler responseMarshaler = NativeStringMarshaler.GetInstance(null);
            System.IntPtr responseNative = System.IntPtr.Zero;
            DebugServerError returnValue = DebugServerNativeMethods.debugserver_client_send_command(client, command, out responseNative);
            response = ((string)responseMarshaler.MarshalNativeToManaged(responseNative));
            responseMarshaler.CleanUpNativeData(responseNative);
            return returnValue;
        }
        
        public static DebugServerError debugserver_client_receive_response(DebugServerClientHandle client, out string response)
        {
            System.Runtime.InteropServices.ICustomMarshaler responseMarshaler = NativeStringMarshaler.GetInstance(null);
            System.IntPtr responseNative = System.IntPtr.Zero;
            DebugServerError returnValue = DebugServerNativeMethods.debugserver_client_receive_response(client, out responseNative);
            response = ((string)responseMarshaler.MarshalNativeToManaged(responseNative));
            responseMarshaler.CleanUpNativeData(responseNative);
            return returnValue;
        }
        
        public static DebugServerError debugserver_client_set_argv(DebugServerClientHandle client, int argc, System.Collections.ObjectModel.ReadOnlyCollection<string> argv, out string response)
        {
            System.Runtime.InteropServices.ICustomMarshaler responseMarshaler = NativeStringMarshaler.GetInstance(null);
            System.IntPtr responseNative = System.IntPtr.Zero;
            System.Runtime.InteropServices.ICustomMarshaler argvMarshaler = NativeStringArrayMarshaler.GetInstance(null);
            System.IntPtr argvNative = argvMarshaler.MarshalManagedToNative(argv);
            DebugServerError returnValue = DebugServerNativeMethods.debugserver_client_set_argv(client, argc, argvNative, out responseNative);
            response = ((string)responseMarshaler.MarshalNativeToManaged(responseNative));
            responseMarshaler.CleanUpNativeData(responseNative);
            return returnValue;
        }
        
        public static DebugServerError debugserver_client_set_environment_hex_encoded(DebugServerClientHandle client, string env, out string response)
        {
            System.Runtime.InteropServices.ICustomMarshaler responseMarshaler = NativeStringMarshaler.GetInstance(null);
            System.IntPtr responseNative = System.IntPtr.Zero;
            DebugServerError returnValue = DebugServerNativeMethods.debugserver_client_set_environment_hex_encoded(client, env, out responseNative);
            response = ((string)responseMarshaler.MarshalNativeToManaged(responseNative));
            responseMarshaler.CleanUpNativeData(responseNative);
            return returnValue;
        }
        
        public static DebugServerError debugserver_command_new(string name, int argc, System.Collections.ObjectModel.ReadOnlyCollection<string> argv, out DebugServerCommandHandle command)
        {
            System.Runtime.InteropServices.ICustomMarshaler argvMarshaler = NativeStringArrayMarshaler.GetInstance(null);
            System.IntPtr argvNative = argvMarshaler.MarshalManagedToNative(argv);
            DebugServerError returnValue = DebugServerNativeMethods.debugserver_command_new(name, argc, argvNative, out command);
            return returnValue;
        }
        
        public static void debugserver_encode_string(string buffer, out string encodedBuffer, ref uint encodedLength)
        {
            System.Runtime.InteropServices.ICustomMarshaler encodedBufferMarshaler = NativeStringMarshaler.GetInstance(null);
            System.IntPtr encodedBufferNative = System.IntPtr.Zero;
            DebugServerNativeMethods.debugserver_encode_string(buffer, out encodedBufferNative, ref encodedLength);
            encodedBuffer = ((string)encodedBufferMarshaler.MarshalNativeToManaged(encodedBufferNative));
            encodedBufferMarshaler.CleanUpNativeData(encodedBufferNative);
        }
        
        public static void debugserver_decode_string(string encodedBuffer, uint encodedLength, out string buffer)
        {
            System.Runtime.InteropServices.ICustomMarshaler bufferMarshaler = NativeStringMarshaler.GetInstance(null);
            System.IntPtr bufferNative = System.IntPtr.Zero;
            DebugServerNativeMethods.debugserver_decode_string(encodedBuffer, encodedLength, out bufferNative);
            buffer = ((string)bufferMarshaler.MarshalNativeToManaged(bufferNative));
            bufferMarshaler.CleanUpNativeData(bufferNative);
        }
    }
}
