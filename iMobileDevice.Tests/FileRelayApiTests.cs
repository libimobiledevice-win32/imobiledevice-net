using iMobileDevice.FileRelay;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;
using System;
using Xunit;

namespace iMobileDevice.Tests
{
    /// <summary>
    /// Tests the methods of the <see cref="FileRelayApi"/> class which can be invoked without connecting to a device, or partial error handling
    /// of methods which should connect to a device. This is a first test to make sure the P/Invoke declarations are correct.
    /// </summary>
    public class FileRelayApiTests
    {
        private readonly LibiMobileDevice api = new LibiMobileDevice();

        [Fact]
        public void FileRelayClientFreeZero()
        {
            Assert.Equal(FileRelayError.InvalidArg, this.api.FileRelay.file_relay_client_free(IntPtr.Zero));
        }

        [Fact]
        public void FileRelayClientNewZero()
        {
            Assert.Equal(FileRelayError.InvalidArg, this.api.FileRelay.file_relay_client_new(iDeviceHandle.Zero, LockdownServiceDescriptorHandle.Zero, out FileRelayClientHandle client));
        }

        [Fact]
        public void FileRelayClientStartServiceZero()
        {
            Assert.Equal(FileRelayError.UnknownError, this.api.FileRelay.file_relay_client_start_service(iDeviceHandle.Zero, out FileRelayClientHandle client, "test"));
        }

        [Fact]
        public void FileRelayRequestSourcesZero()
        {
            Assert.Equal(FileRelayError.InvalidArg, this.api.FileRelay.file_relay_request_sources(FileRelayClientHandle.Zero, out string sources, out iDeviceConnectionHandle connection));
        }

        [Fact]
        public void FileRelayRequestSourcesTimeoutZero()
        {
            Assert.Equal(FileRelayError.InvalidArg, this.api.FileRelay.file_relay_request_sources_timeout(FileRelayClientHandle.Zero, out string sources, out iDeviceConnectionHandle connection, 0));
        }
    }
}
