using iMobileDevice.iDeviceActivation;
using iMobileDevice.Plist;
using System;
using System.IO;
using Xunit;

namespace iMobileDevice.Tests
{
    /// <summary>
    /// Tests the methods of the <see cref="iDeviceActivationApi"/> class which can be invoked without connecting to a device, or partial error handling
    /// of methods which should connect to a device. This is a first test to make sure the P/Invoke declarations are correct.
    /// </summary>
    public class iDeviceActivationTests
    {
        private readonly LibiMobileDevice api = new LibiMobileDevice();

        [Fact]
        public void iDeviceActivationDrmHandshakeRequestNew()
        {
            iDeviceActivationRequestHandle request;
            Assert.Equal(iDeviceActivationError.Success, this.api.iDeviceActivation.idevice_activation_drm_handshake_request_new(iDeviceActivationClientType.ClientItunes, out request));
            Assert.NotNull(request);
            request.Dispose();
        }

        [Fact]
        public void iDeviceActivationRequestFreeZero()
        {
            this.api.iDeviceActivation.idevice_activation_request_free(IntPtr.Zero);
        }

        [Fact]
        public void iDeviceActivationRequestGetSetField()
        {
            iDeviceActivationRequestHandle request;
            Assert.Equal(iDeviceActivationError.Success, this.api.iDeviceActivation.idevice_activation_request_new(iDeviceActivationClientType.ClientItunes, out request));
            Assert.NotNull(request);

            this.api.iDeviceActivation.idevice_activation_request_set_field(request, "test", "value");
            this.api.iDeviceActivation.idevice_activation_request_get_field(request, "test", out string value);
            Assert.Equal("value", value);
            request.Dispose();
        }

        [Fact]
        public void iDeviceActivationRequestGetFields()
        {
            iDeviceActivationRequestHandle request;
            Assert.Equal(iDeviceActivationError.Success, this.api.iDeviceActivation.idevice_activation_request_new(iDeviceActivationClientType.ClientItunes, out request));
            Assert.NotNull(request);

            this.api.iDeviceActivation.idevice_activation_request_set_field(request, "field1", "value1");
            this.api.iDeviceActivation.idevice_activation_request_set_field(request, "field2", "value2");
            this.api.iDeviceActivation.idevice_activation_request_get_fields(request, out PlistHandle fields);
            Assert.NotNull(fields);

            Assert.Equal(2u, this.api.Plist.plist_dict_get_size(fields));

            fields.Dispose();
            request.Dispose();
        }

        [Fact]
        public void iDeviceActivationRequestGetSetUrl()
        {
            iDeviceActivationRequestHandle request;
            Assert.Equal(iDeviceActivationError.Success, this.api.iDeviceActivation.idevice_activation_request_new(iDeviceActivationClientType.ClientItunes, out request));
            this.api.iDeviceActivation.idevice_activation_request_set_url(request, "http://quamotion.mobi");
            this.api.iDeviceActivation.idevice_activation_request_get_url(request, out string url);

            Assert.Equal("http://quamotion.mobi", url);
            request.Dispose();
        }

        [Fact]
        public void iDeviceActivationRequestNew()
        {
            Assert.Equal(iDeviceActivationError.Success, this.api.iDeviceActivation.idevice_activation_request_new(iDeviceActivationClientType.ClientItunes, out iDeviceActivationRequestHandle request));
            Assert.NotNull(request);
            Assert.False(request.IsInvalid);
            request.Dispose();
        }

        [Fact]
        public void iDeviceActivationRequestNewFromLockdownd()
        {
            Assert.Equal(iDeviceActivationError.InternalError, this.api.iDeviceActivation.idevice_activation_request_new_from_lockdownd(iDeviceActivationClientType.ClientItunes, IntPtr.Zero, out iDeviceActivationRequestHandle request));
        }

        [Fact]
        public void iDeviceActivationRequestGetSetFields()
        {
            iDeviceActivationRequestHandle request;
            Assert.Equal(iDeviceActivationError.Success, this.api.iDeviceActivation.idevice_activation_request_new(iDeviceActivationClientType.ClientItunes, out request));

            var plist = this.api.Plist.plist_new_dict();
            this.api.Plist.plist_dict_set_item(plist, "field1", this.api.Plist.plist_new_string("value1"));
            this.api.Plist.plist_dict_set_item(plist, "field2", this.api.Plist.plist_new_string("value2"));
            this.api.Plist.plist_dict_set_item(plist, "field3", this.api.Plist.plist_new_string("value3"));
            this.api.iDeviceActivation.idevice_activation_request_set_fields(request, plist);
            this.api.iDeviceActivation.idevice_activation_request_get_fields(request, out PlistHandle fields);
            Assert.NotNull(fields);

            Assert.Equal(3u, this.api.Plist.plist_dict_get_size(fields));

            plist.Dispose();
            request.Dispose();
        }

        [Fact]
        public void iDeviceActivationResponseTests()
        {
            var activation = File.ReadAllText("ideviceactivation.xml");
            Assert.Equal(iDeviceActivationError.Success, this.api.iDeviceActivation.idevice_activation_response_new_from_html(activation, out iDeviceActivationResponseHandle response));

            this.api.iDeviceActivation.idevice_activation_response_get_activation_record(response, out PlistHandle activationRecord);
            Assert.NotNull(activationRecord);
            activationRecord.Dispose();

            this.api.iDeviceActivation.idevice_activation_response_get_description(response, out string description);
            Assert.Null(description);

            this.api.iDeviceActivation.idevice_activation_response_get_field(response, "ActivationInfoComplete", out string field);
            Assert.Null(field);

            this.api.iDeviceActivation.idevice_activation_response_get_fields(response, out PlistHandle fields);
            Assert.NotNull(fields);
            Assert.False(fields.IsInvalid);
            uint length = 0;
            this.api.Plist.plist_to_xml(fields, out string fieldsXml, ref length);
            fields.Dispose();

            this.api.iDeviceActivation.idevice_activation_response_get_headers(response, out PlistHandle headers);
            Assert.NotNull(headers);
            Assert.False(headers.IsInvalid);
            this.api.Plist.plist_to_xml(headers, out string headersXml, ref length);
            headers.Dispose();

            this.api.iDeviceActivation.idevice_activation_response_get_label(response, "test", out string label);
            Assert.Null(label);

            this.api.iDeviceActivation.idevice_activation_response_get_title(response, out string title);
            Assert.Null(title);

            Assert.Equal(1, this.api.iDeviceActivation.idevice_activation_response_has_errors(response));
            Assert.Equal(0, this.api.iDeviceActivation.idevice_activation_response_is_activation_acknowledged(response));
            Assert.Equal(0, this.api.iDeviceActivation.idevice_activation_response_is_authentication_required(response));

            response.Dispose();
        }
    }
}
