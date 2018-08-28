using iMobileDevice.DiagnosticsRelay;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;
using iMobileDevice.Plist;
using System;
using Xunit;

namespace iMobileDevice.Tests
{
    /// <summary>
    /// Tests the methods of the <see cref="DiagnosticsRelayApi"/> class which can be invoked without connecting to a device, or partial error handling
    /// of methods which should connect to a device. This is a first test to make sure the P/Invoke declarations are correct.
    /// </summary>
    public class DiagnosticsRelayApiTests
    {
        private readonly LibiMobileDevice api = new LibiMobileDevice();

        [Fact]
        public void DiagnosticsRelayClientFreeZero()
        {
            Assert.Equal(DiagnosticsRelayError.InvalidArg, this.api.DiagnosticsRelay.diagnostics_relay_client_free(IntPtr.Zero));
        }

        [Fact]
        public void DiagnosticsRelayClientNewZero()
        {
            Assert.Equal(DiagnosticsRelayError.InvalidArg, this.api.DiagnosticsRelay.diagnostics_relay_client_new(iDeviceHandle.Zero, LockdownServiceDescriptorHandle.Zero, out DiagnosticsRelayClientHandle client));
        }

        [Fact]
        public void DiagnosticsRelayClientStartServiceZero()
        {
            Assert.Equal(DiagnosticsRelayError.UnknownError, this.api.DiagnosticsRelay.diagnostics_relay_client_start_service(iDeviceHandle.Zero, out DiagnosticsRelayClientHandle client, "test"));
        }

        [Fact]
        public void DiagnosticsRelayGoodbyeZero()
        {
            Assert.Equal(DiagnosticsRelayError.InvalidArg, this.api.DiagnosticsRelay.diagnostics_relay_goodbye(DiagnosticsRelayClientHandle.Zero));
        }

        [Fact]
        public void DiagnosticsRelayQueryIORegistryEntryZero()
        {
            Assert.Equal(DiagnosticsRelayError.InvalidArg, this.api.DiagnosticsRelay.diagnostics_relay_query_ioregistry_entry(DiagnosticsRelayClientHandle.Zero, "test", "abc", out PlistHandle result));
        }

        [Fact]
        public void DiagnosticsRelayQueryIORegistryPlaneZero()
        {
            Assert.Equal(DiagnosticsRelayError.InvalidArg, this.api.DiagnosticsRelay.diagnostics_relay_query_ioregistry_plane(DiagnosticsRelayClientHandle.Zero, "test", out PlistHandle result));
        }

        [Fact]
        public void DiagnosticsRelayQueryMobileGestaltZero()
        {
            Assert.Equal(DiagnosticsRelayError.InvalidArg, this.api.DiagnosticsRelay.diagnostics_relay_query_mobilegestalt(DiagnosticsRelayClientHandle.Zero, PlistHandle.Zero, out PlistHandle result));
        }

        [Fact]
        public void DiagnosticsRelayRequestDiagnosticsZero()
        {
            Assert.Equal(DiagnosticsRelayError.InvalidArg, this.api.DiagnosticsRelay.diagnostics_relay_request_diagnostics(DiagnosticsRelayClientHandle.Zero, "type", out PlistHandle diagnostics));
        }

        [Fact]
        public void DiagnosticRelayRestartZero()
        {
            Assert.Equal(DiagnosticsRelayError.InvalidArg, this.api.DiagnosticsRelay.diagnostics_relay_restart(DiagnosticsRelayClientHandle.Zero, DiagnosticsRelayAction.ActionFlagDisplayFail));
        }

        [Fact]
        public void DiagnosticsRelayShutdownZero()
        {
            Assert.Equal(DiagnosticsRelayError.InvalidArg, this.api.DiagnosticsRelay.diagnostics_relay_shutdown(DiagnosticsRelayClientHandle.Zero, DiagnosticsRelayAction.ActionFlagDisplayFail));
        }

        [Fact]
        public void DiagnosticRelaySleepZero()
        { 
            Assert.Equal(DiagnosticsRelayError.InvalidArg, this.api.DiagnosticsRelay.diagnostics_relay_sleep(DiagnosticsRelayClientHandle.Zero));
        }
    }
}
