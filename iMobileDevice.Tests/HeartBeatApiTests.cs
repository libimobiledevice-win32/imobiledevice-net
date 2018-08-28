using iMobileDevice.HeartBeat;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;
using iMobileDevice.Plist;
using System;
using Xunit;

namespace iMobileDevice.Tests
{
    /// <summary>
    /// Tests the methods of the <see cref="HeartBeatApi"/> class which can be invoked without connecting to a device, or partial error handling
    /// of methods which should connect to a device. This is a first test to make sure the P/Invoke declarations are correct.
    /// </summary>
    public class HeartBeatApiTests
    {
        private readonly LibiMobileDevice api = new LibiMobileDevice();

        [Fact]
        public void HeartBeatClientFreeZero()
        {
            Assert.Equal(HeartBeatError.InvalidArg, api.HeartBeat.heartbeat_client_free(IntPtr.Zero));
        }

        [Fact]
        public void HeartBeatClientNewZero()
        {
            Assert.Equal(HeartBeatError.InvalidArg, api.HeartBeat.heartbeat_client_new(iDeviceHandle.Zero, LockdownServiceDescriptorHandle.Zero, out HeartBeatClientHandle client));
        }

        [Fact]
        public void HeartBeatClientStartServiceZero()
        {
            Assert.Equal(HeartBeatError.UnknownError, api.HeartBeat.heartbeat_client_start_service(iDeviceHandle.Zero, out HeartBeatClientHandle client, "test"));
        }

        [Fact(Skip = "heartbeat_receive does not check for null values")]
        public void HeartBeatReceiveZero()
        {
            Assert.Equal(HeartBeatError.InvalidArg, api.HeartBeat.heartbeat_receive(HeartBeatClientHandle.Zero, out PlistHandle plist));
        }

        [Fact(Skip = "heartbeat_receive_with_timeout does not check for null values")]
        public void HeartBeatReceiveWithTimeoutZero()
        {
            Assert.Equal(HeartBeatError.InvalidArg, api.HeartBeat.heartbeat_receive_with_timeout(HeartBeatClientHandle.Zero, out PlistHandle plist, 0));
        }

        [Fact(Skip = "heartbeat_send does not check for null values")]
        public void HeartBeatSendZero()
        {
            Assert.Equal(HeartBeatError.InvalidArg, api.HeartBeat.heartbeat_send(HeartBeatClientHandle.Zero, PlistHandle.Zero));
        }
    }
}
