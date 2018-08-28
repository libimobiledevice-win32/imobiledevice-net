using iMobileDevice.iDevice;
using iMobileDevice.InstallationProxy;
using iMobileDevice.Lockdown;
using iMobileDevice.Plist;
using System;
using Xunit;

namespace iMobileDevice.Tests
{
    /// <summary>
    /// Tests the methods of the <see cref="InstallationProxyApi"/> class which can be invoked without connecting to a device, or partial error handling
    /// of methods which should connect to a device. This is a first test to make sure the P/Invoke declarations are correct.
    /// </summary>
    public class InstallationProxyApiTests
    {
        private readonly LibiMobileDevice api = new LibiMobileDevice();

        [Fact]
        public void InstProxyArchiveZero()
        {
            Assert.Equal(InstallationProxyError.InvalidArg, api.InstallationProxy.instproxy_archive(InstallationProxyClientHandle.Zero, "", PlistHandle.Zero, null, IntPtr.Zero));
        }

        [Fact]
        public void InstProxyBrowseZero()
        {
            Assert.Equal(InstallationProxyError.InvalidArg, api.InstallationProxy.instproxy_browse(InstallationProxyClientHandle.Zero, PlistHandle.Zero, out PlistHandle result));
        }

        [Fact]
        public void InstProxyBrowseWithCallbackZero()
        {
            Assert.Equal(InstallationProxyError.InvalidArg, api.InstallationProxy.instproxy_browse_with_callback(InstallationProxyClientHandle.Zero, PlistHandle.Zero, null, IntPtr.Zero));
        }

        [Fact]
        public void InstProxyCheckCapabilitiesMatchZero()
        {
            Assert.Equal(InstallationProxyError.InvalidArg, api.InstallationProxy.instproxy_check_capabilities_match(InstallationProxyClientHandle.Zero, out string capabilities, PlistHandle.Zero, out PlistHandle result));
        }

        [Fact]
        public void InstProxyClientFreeZero()
        {
            Assert.Equal(InstallationProxyError.InvalidArg, api.InstallationProxy.instproxy_client_free(IntPtr.Zero));
        }

        [Fact]
        public void InstProxyClientGetPathForBundleIdentifierZero()
        {
            Assert.Equal(InstallationProxyError.InvalidArg, api.InstallationProxy.instproxy_client_get_path_for_bundle_identifier(InstallationProxyClientHandle.Zero, "", out string path));
        }

        [Fact]
        public void InstProxyClientNewZero()
        {
            Assert.Equal(InstallationProxyError.InvalidArg, api.InstallationProxy.instproxy_client_new(iDeviceHandle.Zero, LockdownServiceDescriptorHandle.Zero, out InstallationProxyClientHandle client));
        }

        [Fact]
        public void InstProxyClientOptionsAdd()
        {
            using (var clientOptions = api.InstallationProxy.instproxy_client_options_new())
            {
                Assert.NotNull(clientOptions);
                Assert.False(clientOptions.IsInvalid);

                api.InstallationProxy.instproxy_client_options_add(clientOptions, "CFBundleIdentifier", "MyBundle", (byte)'\0');
            }
        }

        [Fact]
        public void InstProxyClientStartServiceZero()
        {
            Assert.Equal(InstallationProxyError.UnknownError, api.InstallationProxy.instproxy_client_start_service(iDeviceHandle.Zero, out InstallationProxyClientHandle handle, ""));
        }

        [Fact]
        public void InstProxyCommandGetName()
        {
            using (var command = this.api.Plist.plist_new_dict())
            {
                this.api.Plist.plist_dict_set_item(command, "Command", this.api.Plist.plist_new_string("quamotion"));
                api.InstallationProxy.instproxy_command_get_name(command, out string name);
                Assert.Equal("quamotion", name);
            }
        }

        [Fact]
        public void InstProxyInstallZero()
        {
            Assert.Equal(InstallationProxyError.InvalidArg, api.InstallationProxy.instproxy_install(InstallationProxyClientHandle.Zero, "", PlistHandle.Zero, null, IntPtr.Zero));
        }

        [Fact]
        public void InstProxyLookupZero()
        {
            Assert.Equal(InstallationProxyError.InvalidArg, api.InstallationProxy.instproxy_lookup(InstallationProxyClientHandle.Zero, null, PlistHandle.Zero, out PlistHandle result));
        }

        [Fact]
        public void InstProxyLookupArchivesZero()
        {
            Assert.Equal(InstallationProxyError.InvalidArg, api.InstallationProxy.instproxy_lookup_archives(InstallationProxyClientHandle.Zero, PlistHandle.Zero, out PlistHandle result));
        }

        [Fact]
        public void InstProxyRemoveArchiveZero()
        {
            Assert.Equal(InstallationProxyError.InvalidArg, api.InstallationProxy.instproxy_remove_archive(InstallationProxyClientHandle.Zero, "", PlistHandle.Zero, null, IntPtr.Zero));
        }

        [Fact]
        public void InstProxyRestoreZero()
        {
            Assert.Equal(InstallationProxyError.InvalidArg, api.InstallationProxy.instproxy_restore(InstallationProxyClientHandle.Zero, "", PlistHandle.Zero, null, IntPtr.Zero));
        }

        [Fact]
        public void InstProxyStatusGetCurrentList()
        {
            using (var status = this.api.Plist.plist_new_dict())
            {
                this.api.Plist.plist_dict_set_item(status, "Total", this.api.Plist.plist_new_uint(5));
                this.api.Plist.plist_dict_set_item(status, "CurrentAmount", this.api.Plist.plist_new_uint(6));
                this.api.Plist.plist_dict_set_item(status, "CurrentIndex", this.api.Plist.plist_new_uint(7));

                ulong total = 0;
                ulong currentIndex = 0;
                ulong currentAmount = 0;

                api.InstallationProxy.instproxy_status_get_current_list(status, ref total, ref currentIndex, ref currentAmount, out PlistHandle list);

                Assert.Equal(5u, total);
                Assert.Equal(6u, currentAmount);
                Assert.Equal(7u, currentIndex);
            }
        }

        [Fact]
        public void InstProxyStatusGetError()
        {
            using (var status = this.api.Plist.plist_new_dict())
            {
                this.api.Plist.plist_dict_set_item(status, "Error", this.api.Plist.plist_new_string("oops"));
                this.api.Plist.plist_dict_set_item(status, "ErrorDescription", this.api.Plist.plist_new_string("oops-message"));
                this.api.Plist.plist_dict_set_item(status, "ErrorDetail", this.api.Plist.plist_new_uint(5));

                ulong code = 0;
                api.InstallationProxy.instproxy_status_get_error(status, out string error, out string description, ref code);
                Assert.Equal("oops", error);
                Assert.Equal("oops-message", description);
                Assert.Equal(5u, code);
            }
        }

        [Fact]
        public void InstProxyStatusGetName()
        {
            using (var status = this.api.Plist.plist_new_dict())
            {
                this.api.Plist.plist_dict_set_item(status, "Status", this.api.Plist.plist_new_string("ok"));
                api.InstallationProxy.instproxy_status_get_name(status, out string name);
                Assert.Equal("ok", name);
            }
        }

        [Fact]
        public void InstProxyStatusGetPercentComplete()
        {
            using (var status = this.api.Plist.plist_new_dict())
            {
                this.api.Plist.plist_dict_set_item(status, "PercentComplete", this.api.Plist.plist_new_uint(9));
                int percent = 0;
                api.InstallationProxy.instproxy_status_get_percent_complete(status, ref percent);
                Assert.Equal(9, percent);
            }
        }

        [Fact]
        public void InstProxyUninstallZero()
        {
            Assert.Equal(InstallationProxyError.InvalidArg, api.InstallationProxy.instproxy_uninstall(InstallationProxyClientHandle.Zero, "", PlistHandle.Zero, null, IntPtr.Zero));
        }

        [Fact]
        public void InstProxyUpgradeZero()
        {
            Assert.Equal(InstallationProxyError.InvalidArg, api.InstallationProxy.instproxy_upgrade(InstallationProxyClientHandle.Zero, "", PlistHandle.Zero, null, IntPtr.Zero));
        }
    }
}
