// <copyright file="iDeviceActivationApi.cs" company="Quamotion">
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
    
    
    public partial class iDeviceActivationApi : IiDeviceActivationApi
    {
        
        /// <summary>
        /// Backing field for the <see cref="Parent"/> property
        /// </summary>
        private ILibiMobileDevice parent;
        
        /// <summary>
        /// Initializes a new instance of the <see cref"iDeviceActivationApi"/> class
        /// </summary>
        /// <param name="parent">
        /// The <see cref="ILibiMobileDeviceApi"/> which owns this <see cref="iDeviceActivation"/>.
        /// </summary>
        public iDeviceActivationApi(ILibiMobileDevice parent)
        {
            this.parent = parent;
        }
        
        /// <inheritdoc/>
        public ILibiMobileDevice Parent
        {
            get
            {
                return this.parent;
            }
        }
        
        public virtual void idevice_activation_set_debug_level(int level)
        {
            iDeviceActivationNativeMethods.idevice_activation_set_debug_level(level);
        }
        
        public virtual iDeviceActivationError idevice_activation_request_new(iDeviceActivationClientType activationType, out iDeviceActivationRequestHandle request)
        {
            iDeviceActivationError returnValue;
            returnValue = iDeviceActivationNativeMethods.idevice_activation_request_new(activationType, out request);
            request.Api = this.Parent;
            return returnValue;
        }
        
        public virtual iDeviceActivationError idevice_activation_request_new_from_lockdownd(iDeviceActivationClientType activationType, System.IntPtr lockdown, out iDeviceActivationRequestHandle request)
        {
            iDeviceActivationError returnValue;
            returnValue = iDeviceActivationNativeMethods.idevice_activation_request_new_from_lockdownd(activationType, lockdown, out request);
            request.Api = this.Parent;
            return returnValue;
        }
        
        public virtual iDeviceActivationError idevice_activation_drm_handshake_request_new(iDeviceActivationClientType clientType, out iDeviceActivationRequestHandle request)
        {
            iDeviceActivationError returnValue;
            returnValue = iDeviceActivationNativeMethods.idevice_activation_drm_handshake_request_new(clientType, out request);
            request.Api = this.Parent;
            return returnValue;
        }
        
        public virtual void idevice_activation_request_free(System.IntPtr request)
        {
            iDeviceActivationNativeMethods.idevice_activation_request_free(request);
        }
        
        public virtual void idevice_activation_request_get_fields(iDeviceActivationRequestHandle request, out PlistHandle fields)
        {
            iDeviceActivationNativeMethods.idevice_activation_request_get_fields(request, out fields);
        }
        
        public virtual void idevice_activation_request_set_fields(iDeviceActivationRequestHandle request, PlistHandle fields)
        {
            iDeviceActivationNativeMethods.idevice_activation_request_set_fields(request, fields);
        }
        
        public virtual void idevice_activation_request_set_fields_from_response(iDeviceActivationRequestHandle request, iDeviceActivationResponseHandle response)
        {
            iDeviceActivationNativeMethods.idevice_activation_request_set_fields_from_response(request, response);
        }
        
        public virtual void idevice_activation_request_set_field(iDeviceActivationRequestHandle request, string key, string value)
        {
            iDeviceActivationNativeMethods.idevice_activation_request_set_field(request, key, value);
        }
        
        public virtual void idevice_activation_request_get_field(iDeviceActivationRequestHandle request, string key, out string value)
        {
            iDeviceActivationNativeMethods.idevice_activation_request_get_field(request, key, out value);
        }
        
        public virtual void idevice_activation_request_get_url(iDeviceActivationRequestHandle request, out string url)
        {
            iDeviceActivationNativeMethods.idevice_activation_request_get_url(request, out url);
        }
        
        public virtual void idevice_activation_request_set_url(iDeviceActivationRequestHandle request, string url)
        {
            iDeviceActivationNativeMethods.idevice_activation_request_set_url(request, url);
        }
        
        public virtual iDeviceActivationError idevice_activation_response_new(out iDeviceActivationResponseHandle response)
        {
            iDeviceActivationError returnValue;
            returnValue = iDeviceActivationNativeMethods.idevice_activation_response_new(out response);
            response.Api = this.Parent;
            return returnValue;
        }
        
        public virtual iDeviceActivationError idevice_activation_response_new_from_html(string content, out iDeviceActivationResponseHandle response)
        {
            iDeviceActivationError returnValue;
            returnValue = iDeviceActivationNativeMethods.idevice_activation_response_new_from_html(content, out response);
            response.Api = this.Parent;
            return returnValue;
        }
        
        public virtual iDeviceActivationError idevice_activation_response_to_buffer(iDeviceActivationResponseHandle response, out string buffer, ref uint size)
        {
            return iDeviceActivationNativeMethods.idevice_activation_response_to_buffer(response, out buffer, ref size);
        }
        
        public virtual void idevice_activation_response_free(System.IntPtr response)
        {
            iDeviceActivationNativeMethods.idevice_activation_response_free(response);
        }
        
        public virtual void idevice_activation_response_get_field(iDeviceActivationResponseHandle response, string key, out string value)
        {
            iDeviceActivationNativeMethods.idevice_activation_response_get_field(response, key, out value);
        }
        
        public virtual void idevice_activation_response_get_fields(iDeviceActivationResponseHandle response, out PlistHandle fields)
        {
            iDeviceActivationNativeMethods.idevice_activation_response_get_fields(response, out fields);
        }
        
        public virtual void idevice_activation_response_get_label(iDeviceActivationResponseHandle response, string key, out string value)
        {
            iDeviceActivationNativeMethods.idevice_activation_response_get_label(response, key, out value);
        }
        
        public virtual void idevice_activation_response_get_title(iDeviceActivationResponseHandle response, out string title)
        {
            iDeviceActivationNativeMethods.idevice_activation_response_get_title(response, out title);
        }
        
        public virtual void idevice_activation_response_get_description(iDeviceActivationResponseHandle response, out string description)
        {
            iDeviceActivationNativeMethods.idevice_activation_response_get_description(response, out description);
        }
        
        public virtual void idevice_activation_response_get_activation_record(iDeviceActivationResponseHandle response, out PlistHandle activationRecord)
        {
            iDeviceActivationNativeMethods.idevice_activation_response_get_activation_record(response, out activationRecord);
        }
        
        public virtual void idevice_activation_response_get_headers(iDeviceActivationResponseHandle response, out PlistHandle headers)
        {
            iDeviceActivationNativeMethods.idevice_activation_response_get_headers(response, out headers);
        }
        
        public virtual int idevice_activation_response_is_activation_acknowledged(iDeviceActivationResponseHandle response)
        {
            return iDeviceActivationNativeMethods.idevice_activation_response_is_activation_acknowledged(response);
        }
        
        public virtual int idevice_activation_response_is_authentication_required(iDeviceActivationResponseHandle response)
        {
            return iDeviceActivationNativeMethods.idevice_activation_response_is_authentication_required(response);
        }
        
        public virtual int idevice_activation_response_field_requires_input(iDeviceActivationResponseHandle response, string key)
        {
            return iDeviceActivationNativeMethods.idevice_activation_response_field_requires_input(response, key);
        }
        
        public virtual int idevice_activation_response_has_errors(iDeviceActivationResponseHandle response)
        {
            return iDeviceActivationNativeMethods.idevice_activation_response_has_errors(response);
        }
        
        public virtual iDeviceActivationError idevice_activation_send_request(iDeviceActivationRequestHandle request, out iDeviceActivationResponseHandle response)
        {
            iDeviceActivationError returnValue;
            returnValue = iDeviceActivationNativeMethods.idevice_activation_send_request(request, out response);
            response.Api = this.Parent;
            return returnValue;
        }
    }
}
