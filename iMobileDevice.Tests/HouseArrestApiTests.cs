using iMobileDevice.Afc;
using iMobileDevice.HouseArrest;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;
using iMobileDevice.Plist;
using System;
using Xunit;

namespace iMobileDevice.Tests
{
    /// <summary>
    /// Tests the methods of the <see cref="HouseArrestApi"/> class which can be invoked without connecting to a device, or partial error handling
    /// of methods which should connect to a device. This is a first test to make sure the P/Invoke declarations are correct.
    /// </summary>
    public class HouseArrestApiTests
    {
        private readonly LibiMobileDevice api = new LibiMobileDevice();

        [Fact]
        public void AfcClientNewFromHouseArrestClientZero()
        {
            Assert.Equal(AfcError.InvalidArg, this.api.HouseArrest.afc_client_new_from_house_arrest_client(HouseArrestClientHandle.Zero, out AfcClientHandle afcClient));
        }

        [Fact]
        public void HouseArrestClientFreeZero()
        {
            Assert.Equal(HouseArrestError.InvalidArg, this.api.HouseArrest.house_arrest_client_free(IntPtr.Zero));
        }

        [Fact]
        public void HouseArrestClientNewZero()
        {
            Assert.Equal(HouseArrestError.InvalidArg, this.api.HouseArrest.house_arrest_client_new(iDeviceHandle.Zero, LockdownServiceDescriptorHandle.Zero, out HouseArrestClientHandle client));
        }

        [Fact]
        public void HouseArrestClientStartServiceZero()
        {
            Assert.Equal(HouseArrestError.UnknownError, this.api.HouseArrest.house_arrest_client_start_service(iDeviceHandle.Zero, out HouseArrestClientHandle client, "test"));
        }

        [Fact]
        public void HouseArrestGetResultZero()
        {
            Assert.Equal(HouseArrestError.InvalidArg, this.api.HouseArrest.house_arrest_get_result(HouseArrestClientHandle.Zero, out PlistHandle dict));
        }

        [Fact]
        public void HouseArrestSendCommandZero()
        {
            Assert.Equal(HouseArrestError.InvalidArg, this.api.HouseArrest.house_arrest_send_command(HouseArrestClientHandle.Zero, "test", "appId"));
        }

        [Fact]
        public void HouseArrestSendRequestZero()
        {
            Assert.Equal(HouseArrestError.InvalidArg, this.api.HouseArrest.house_arrest_send_request(HouseArrestClientHandle.Zero, PlistHandle.Zero));
        }
    }
}
