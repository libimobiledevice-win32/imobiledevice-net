using iMobileDevice.iDevice;
using System;
using System.Collections.ObjectModel;
using Xunit;

namespace iMobileDevice.Tests
{
    /// <summary>
    /// Tests the methods of the <see cref="iDeviceApi"/> class which can be invoked without connecting to a device, or partial error handling
    /// of methods which should connect to a device. This is a first test to make sure the P/Invoke declarations are correct.
    /// </summary>
    public class iDeviceApiTests
    {
        private readonly LibiMobileDevice api = new LibiMobileDevice();

        [Fact]
        public void iDeviceConnectZero()
        {
            Assert.Equal(iDeviceError.InvalidArg, this.api.iDevice.idevice_connect(iDeviceHandle.Zero, 0, out iDeviceConnectionHandle connection));
        }

        [Fact]
        public void iDeviceConnectionDisableSslZero()
        {
            Assert.Equal(iDeviceError.InvalidArg, this.api.iDevice.idevice_connection_disable_ssl(iDeviceConnectionHandle.Zero));
        }

        [Fact]
        public void iDeviceConnectionEnableSslZero()
        {
            Assert.Equal(iDeviceError.InvalidArg, this.api.iDevice.idevice_connection_enable_ssl(iDeviceConnectionHandle.Zero));
        }

        [Fact]
        public void iDeviceConnectionGetFdZero()
        {
            int fd = 0;
            Assert.Equal(iDeviceError.InvalidArg, this.api.iDevice.idevice_connection_get_fd(iDeviceConnectionHandle.Zero, ref fd));
        }

        [Fact]
        public void iDeviceConnectionReceiveZero()
        {
            uint recvBytes = 0;
            Assert.Equal(iDeviceError.InvalidArg, this.api.iDevice.idevice_connection_receive(iDeviceConnectionHandle.Zero, Array.Empty<byte>(), 0, ref recvBytes));
        }

        [Fact]
        public void iDeviceConnectionReceiveTimeoutZero()
        {
            uint recvBytes = 0;
            Assert.Equal(iDeviceError.InvalidArg, this.api.iDevice.idevice_connection_receive_timeout(iDeviceConnectionHandle.Zero, Array.Empty<byte>(), 0, ref recvBytes, 0));
        }

        [Fact]
        public void iDeviceConnectionSendZero()
        {
            uint sentBytes = 0;
            Assert.Equal(iDeviceError.InvalidArg, this.api.iDevice.idevice_connection_send(iDeviceConnectionHandle.Zero, Array.Empty<byte>(), 0, ref sentBytes));
        }

        [Fact]
        public void iDeviceDeviceListFreeZero()
        {
            // Most _free methods return InvalidArg when passed a NULL value, idevice_device_list_free however returns SUCCESS
            // (but does nothing)
            Assert.Equal(iDeviceError.Success, this.api.iDevice.idevice_device_list_free(IntPtr.Zero));
        }

        [Fact]
        public void iDeviceDisconnectZero()
        {
            Assert.Equal(iDeviceError.InvalidArg, this.api.iDevice.idevice_disconnect(IntPtr.Zero));
        }

        [Fact]
        public void iDeviceEventSubscribeUnsubscribe()
        {
            iDeviceEventCallBack callback = null;
            Assert.Equal(iDeviceError.Success, this.api.iDevice.idevice_event_subscribe(callback, IntPtr.Zero));
            Assert.Equal(iDeviceError.Success, this.api.iDevice.idevice_event_unsubscribe());
        }

        [Fact]
        public void iDeviceFreeZero()
        {
            Assert.Equal(iDeviceError.InvalidArg, this.api.iDevice.idevice_free(IntPtr.Zero));
        }

        [Fact (Skip = "Flaky, integration test")]
        public void iDeviceGetDeviceListZero()
        {
            int count = 0;
            Assert.Equal(iDeviceError.NoDevice, this.api.iDevice.idevice_get_device_list(out ReadOnlyCollection<string> devices, ref count));
        }

        [Fact]
        public void iDeviceGetHandleZero()
        {
            uint handle = 0;
            Assert.Equal(iDeviceError.InvalidArg, this.api.iDevice.idevice_get_handle(iDeviceHandle.Zero, ref handle));
        }

        [Fact]
        public void iDeviceGetSetSocketType()
        {
            int socketType = 0;
            Assert.Equal(iDeviceError.Success, this.api.iDevice.idevice_set_socket_type((int)iDeviceSocketType.SocketTypeTcp));
            Assert.Equal(iDeviceError.Success, this.api.iDevice.idevice_get_socket_type(ref socketType));
            Assert.Equal(iDeviceSocketType.SocketTypeTcp, (iDeviceSocketType)socketType);
        }

        [Fact]
        public void iDeviceGetSetTcpEndpoint()
        {
            Assert.Equal(iDeviceError.Success, this.api.iDevice.idevice_set_tcp_endpoint("localhost", 9999));

            ushort port = 0;
            Assert.Equal(iDeviceError.Success, this.api.iDevice.idevice_get_tcp_endpoint(out string host, ref port));
            Assert.Equal("localhost", host);
            Assert.Equal(9999, port);
        }

        [Fact]
        public void iDeviceGetUdidZero()
        {
            Assert.Equal(iDeviceError.InvalidArg, this.api.iDevice.idevice_get_udid(iDeviceHandle.Zero, out string udid));
        }

        [Fact]
        public void iDeviceNewZero()
        {
            Assert.Equal(iDeviceError.NoDevice, this.api.iDevice.idevice_new(out iDeviceHandle handle, string.Empty));
        }

        [Fact(Skip = "No way to unregister callback")]
        public void iDeviceSetDebugCallback()
        {
            iDeviceDebugCallBack debugCallback = null;
            this.api.iDevice.idevice_set_debug_callback(debugCallback);
        }

        [Fact]
        public void iDeviceSetDebugLevel()
        {
            this.api.iDevice.idevice_set_debug_level(1);
        }
    }
}
