using iMobileDevice.Afc;
using iMobileDevice.iDevice;
using iMobileDevice.Lockdown;
using System;
using System.Collections.ObjectModel;
using Xunit;

namespace iMobileDevice.Tests
{
    /// <summary>
    /// Tests the methods of the <see cref="AfcApi"/> class which can be invoked without connecting to a device, or partial error handling
    /// of methods which should connect to a device. This is a first test to make sure the P/Invoke declarations are correct.
    /// </summary>
    public class AfcApiTests
    {
        private readonly LibiMobileDevice api = new LibiMobileDevice();

        [Fact]
        public void AfcClientFreeZero()
        {
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_client_free(IntPtr.Zero));
        }

        [Fact]
        public void AfcClientNewZero()
        {
            AfcClientHandle afc;
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_client_new(iDeviceHandle.Zero, LockdownServiceDescriptorHandle.Zero, out afc));
        }

        [Fact]
        public void AfcClientStartServiceZero()
        {
            AfcClientHandle afc;
            Assert.Equal(AfcError.UnknownError, this.api.Afc.afc_client_start_service(iDeviceHandle.Zero, out afc, "test"));
        }

        [Fact]
        public void AfcDictionaryFreeZero()
        {
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_dictionary_free(IntPtr.Zero));
        }

        [Fact]
        public void AfcFileCloseZero()
        {
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_file_close(AfcClientHandle.Zero, 0));
        }

        [Fact]
        public void AfcFileLockZero()
        {
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_file_lock(AfcClientHandle.Zero, 0, AfcLockOp.LockEx));
        }

        [Fact]
        public void AfcFileOpenZero()
        {
            ulong file = 0;
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_file_open(AfcClientHandle.Zero, "test", AfcFileMode.FopenRdonly, ref file));
        }

        [Fact]
        public void AfcFileReadZero()
        {
            uint bytesRead = 0;
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_file_read(AfcClientHandle.Zero, 0, Array.Empty<byte>(), 0, ref bytesRead));
        }

        [Fact]
        public void AfcFileSeekZero()
        {
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_file_seek(AfcClientHandle.Zero, 0, 0, 0));
        }

        [Fact]
        public void AfcFileTellZero()
        {
            ulong position = 0;
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_file_tell(AfcClientHandle.Zero, 0, ref position));
        }

        [Fact]
        public void AfcFileTruncateZero()
        {
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_file_truncate(AfcClientHandle.Zero, 0, 0));
        }

        [Fact]
        public void AfcFileWriteZero()
        {
            uint bytesWritten = 0;
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_file_write(AfcClientHandle.Zero, 0, Array.Empty<byte>(), 0, ref bytesWritten));
        }

        [Fact]
        public void AfcGetDeviceInfoZero()
        {
            ReadOnlyCollection<string> deviceInformation;
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_get_device_info(AfcClientHandle.Zero, out deviceInformation));
        }

        [Fact]
        public void AfcGetDeviceInfoKeyZero()
        {
            string value;
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_get_device_info_key(AfcClientHandle.Zero, "test", out value));
        }

        [Fact]
        public void AfcGetFileInfoZer()
        {
            ReadOnlyCollection<string> fileInformation;
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_get_file_info(AfcClientHandle.Zero, "test", out fileInformation));
        }

        [Fact]
        public void AfcMakeDirectoryZero()
        {
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_make_directory(AfcClientHandle.Zero, "test"));
        }

        [Fact]
        public void AfcMakeLinkZero()
        {
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_make_link(AfcClientHandle.Zero, AfcLinkType.Hardlink, "/foo", "/bar"));
        }

        [Fact]
        public void AfcReadDirectoryZer()
        {
            ReadOnlyCollection<string> directoryInformation;
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_read_directory(AfcClientHandle.Zero, "/test/", out directoryInformation));
        }

        [Fact]
        public void AfcRemovePathZero()
        {
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_remove_path(AfcClientHandle.Zero, "/test"));
        }

        [Fact]
        public void AfcRemovePathAndContentsZero()
        {
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_remove_path_and_contents(AfcClientHandle.Zero, "/test"));
        }

        [Fact]
        public void AfcRenamePathZero()
        {
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_rename_path(AfcClientHandle.Zero, "/foo", "/bar"));
        }

        [Fact]
        public void AfcSetFileTimeZero()
        {
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_set_file_time(AfcClientHandle.Zero, "/foo", 0));
        }

        [Fact]
        public void AfcTruncateZero()
        {
            Assert.Equal(AfcError.InvalidArg, this.api.Afc.afc_truncate(AfcClientHandle.Zero, "/foo", 0));
        }
    }
}
