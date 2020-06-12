using iMobileDevice.DebugServer;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace iMobileDevice.Tests
{
    /// <summary>
    /// Tests the methods of the <see cref="AfcApi"/> class which can be invoked without connecting to a device, or partial error handling
    /// of methods which should connect to a device. This is a first test to make sure the P/Invoke declarations are correct.
    /// </summary>
    public class DebugServerApiTests
    {
        private readonly LibiMobileDevice api = new LibiMobileDevice();

        [Fact]
        public void DebugServerClientFreeZero()
        {
            Assert.Equal(DebugServerError.InvalidArg, this.api.DebugServer.debugserver_client_free(IntPtr.Zero));
        }

        [Fact]
        public void DebugServerClientNewZero()
        {
            Assert.Equal(DebugServerError.InvalidArg, this.api.DebugServer.debugserver_client_new(iDeviceHandle.Zero, LockdownServiceDescriptorHandle.Zero, out DebugServerClientHandle client));
        }

        [Fact]
        public void DebugServerClientReceiveZero()
        {
            uint received = 0;
            Assert.Equal(DebugServerError.InvalidArg, this.api.DebugServer.debugserver_client_receive(DebugServerClientHandle.Zero, Array.Empty<byte>(), 0, ref received));
        }

        [Fact (Skip = "debugserver_client_receive_response does not check for null values")]
        public void DebugServerClientReceiveResponseZero()
        {
            string response;
            uint responseSize = 0;
            Assert.Equal(DebugServerError.InvalidArg, this.api.DebugServer.debugserver_client_receive_response(DebugServerClientHandle.Zero, out response, ref responseSize));
        }

        [Fact]
        public void DebugServerClientReceiveWithTimeoutZero()
        {
            uint received = 0;
            Assert.Equal(DebugServerError.InvalidArg, this.api.DebugServer.debugserver_client_receive_with_timeout(DebugServerClientHandle.Zero, Array.Empty<byte>(), 0, ref received, 0));
        }

        [Fact]
        public void DebugServerClientSendZero()
        {
            uint sent = 0;
            Assert.Equal(DebugServerError.InvalidArg, this.api.DebugServer.debugserver_client_send(DebugServerClientHandle.Zero, Array.Empty<byte>(), 0, ref sent));
        }

        [Fact(Skip = "debugserver_client_send_command does not check for null values")]
        public void DebugServerClientSendCommandZero()
        {
            uint responseSize = 0;
            Assert.Equal(DebugServerError.InvalidArg, this.api.DebugServer.debugserver_client_send_command(DebugServerClientHandle.Zero, DebugServerCommandHandle.Zero, out string response, ref responseSize));
        }

        [Fact]
        public void DebugServerClientSetAckModeZero()
        {
            Assert.Equal(DebugServerError.InvalidArg, this.api.DebugServer.debugserver_client_set_ack_mode(DebugServerClientHandle.Zero, 0));
        }

        [Fact]
        public void DebugServerClientSetArgVZero()
        {
            Assert.Equal(DebugServerError.InvalidArg, this.api.DebugServer.debugserver_client_set_argv(DebugServerClientHandle.Zero, 0, new ReadOnlyCollection<string>(new List<string>()), out string response));
        }

        [Fact]
        public void DebugServerClientSetEnvironmentHexEncodedZero()
        {
            Assert.Equal(DebugServerError.InvalidArg, this.api.DebugServer.debugserver_client_set_environment_hex_encoded(DebugServerClientHandle.Zero, "abc", out string response));
        }

        [Fact]
        public void DebugServerClientStartServiceZero()
        {
            Assert.Equal(DebugServerError.UnknownError, this.api.DebugServer.debugserver_client_start_service(iDeviceHandle.Zero, out DebugServerClientHandle client, "test"));
        }

        [Fact]
        public void DebugServerCommandFreeZero()
        {
            Assert.Equal(DebugServerError.InvalidArg, this.api.DebugServer.debugserver_command_free(IntPtr.Zero));
        }

        [Fact]
        public void DebugServerCommandNew()
        {
            DebugServerCommandHandle command;
            Assert.Equal(DebugServerError.Success, this.api.DebugServer.debugserver_command_new("test", 0, new ReadOnlyCollection<string>(new List<string>()), out command));
            Assert.NotEqual(IntPtr.Zero, command.DangerousGetHandle());
            Assert.Equal(this.api, command.Api);
            command.Dispose();
        }

        [Fact]
        public void DebugServerDecodeString()
        {
            var encodedString = "7175616D6F74696F6E";
            this.api.DebugServer.debugserver_decode_string(encodedString, (uint)encodedString.Length, out string buffer);
            Assert.Equal("quamotion", buffer);
        }

        [Fact]
        public void DebugServerEncodeString()
        {
            var stringToEncode = "quamotion";
            uint encodedLength = 0;
            this.api.DebugServer.debugserver_encode_string(stringToEncode, out string encodedBuffer, ref encodedLength);
            Assert.Equal("7175616D6F74696F6E", encodedBuffer);

            // *encoded_length = (2 * length) + DEBUGSERVER_CHECKSUM_HASH_LENGTH + 1;
            Assert.Equal(22u, encodedLength);
        }
    }
}
