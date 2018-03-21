// <copyright file="IiDeviceActivationApi.cs" company="Quamotion">
// Copyright (c) 2016-2018 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.iDeviceActivation
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial interface IiDeviceActivationApi
    {
        
        /// <summary>
        /// Gets or sets the <see cref="ILibiMobileDeviceApi"/> which owns this <see cref="iDeviceActivation"/>.
        /// </summary>
        ILibiMobileDevice Parent
        {
            get;
        }
        
        void idevice_activation_set_debug_level(int level);
        
        iDeviceActivationError idevice_activation_request_new(iDeviceActivationClientType activationType, out iDeviceActivationRequestHandle request);
        
        iDeviceActivationError idevice_activation_request_new_from_lockdownd(iDeviceActivationClientType activationType, System.IntPtr lockdown, out iDeviceActivationRequestHandle request);
        
        iDeviceActivationError idevice_activation_drm_handshake_request_new(iDeviceActivationClientType clientType, out iDeviceActivationRequestHandle request);
        
        void idevice_activation_request_free(System.IntPtr request);
        
        void idevice_activation_request_get_fields(iDeviceActivationRequestHandle request, out PlistHandle fields);
        
        void idevice_activation_request_set_fields(iDeviceActivationRequestHandle request, PlistHandle fields);
        
        void idevice_activation_request_set_fields_from_response(iDeviceActivationRequestHandle request, iDeviceActivationResponseHandle response);
        
        void idevice_activation_request_set_field(iDeviceActivationRequestHandle request, string key, string value);
        
        void idevice_activation_request_get_field(iDeviceActivationRequestHandle request, string key, out string value);
        
        void idevice_activation_request_get_url(iDeviceActivationRequestHandle request, out string url);
        
        void idevice_activation_request_set_url(iDeviceActivationRequestHandle request, string url);
        
        iDeviceActivationError idevice_activation_response_new(out iDeviceActivationResponseHandle response);
        
        iDeviceActivationError idevice_activation_response_new_from_html(string content, out iDeviceActivationResponseHandle response);
        
        iDeviceActivationError idevice_activation_response_to_buffer(iDeviceActivationResponseHandle response, out string buffer, ref uint size);
        
        void idevice_activation_response_free(System.IntPtr response);
        
        void idevice_activation_response_get_field(iDeviceActivationResponseHandle response, string key, out string value);
        
        void idevice_activation_response_get_fields(iDeviceActivationResponseHandle response, out PlistHandle fields);
        
        void idevice_activation_response_get_label(iDeviceActivationResponseHandle response, string key, out string value);
        
        void idevice_activation_response_get_title(iDeviceActivationResponseHandle response, out string title);
        
        void idevice_activation_response_get_description(iDeviceActivationResponseHandle response, out string description);
        
        void idevice_activation_response_get_activation_record(iDeviceActivationResponseHandle response, out PlistHandle activationRecord);
        
        void idevice_activation_response_get_headers(iDeviceActivationResponseHandle response, out PlistHandle headers);
        
        int idevice_activation_response_is_activation_acknowledged(iDeviceActivationResponseHandle response);
        
        int idevice_activation_response_is_authentication_required(iDeviceActivationResponseHandle response);
        
        int idevice_activation_response_field_requires_input(iDeviceActivationResponseHandle response, string key);
        
        int idevice_activation_response_has_errors(iDeviceActivationResponseHandle response);
        
        iDeviceActivationError idevice_activation_send_request(iDeviceActivationRequestHandle request, out iDeviceActivationResponseHandle response);
    }
}
