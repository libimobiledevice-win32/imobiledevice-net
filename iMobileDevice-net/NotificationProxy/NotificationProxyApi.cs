// <copyright file="NotificationProxyApi.cs" company="Quamotion">
// Copyright (c) 2016-2017 Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.NotificationProxy
{
    using System.Runtime.InteropServices;
    using System.Diagnostics;
    using iMobileDevice.iDevice;
    using iMobileDevice.Lockdown;
    using iMobileDevice.Afc;
    using iMobileDevice.Plist;
    
    
    public partial class NotificationProxyApi : INotificationProxyApi
    {
        
        /// <summary>
        /// Backing field for the <see cref="Parent"/> property
        /// </summary>
        private ILibiMobileDevice parent;
        
        /// <summary>
        /// Initializes a new instance of the <see cref"NotificationProxyApi"/> class
        /// </summary>
        /// <param name="parent">
        /// The <see cref="ILibiMobileDeviceApi"/> which owns this <see cref="NotificationProxy"/>.
        /// </summary>
        public NotificationProxyApi(ILibiMobileDevice parent)
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
        
        /// <summary>
        /// Connects to the notification_proxy on the specified device.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="service">
        /// The service descriptor returned by lockdownd_start_service.
        /// </param>
        /// <param name="client">
        /// Pointer that will be set to a newly allocated np_client_t
        /// upon successful return.
        /// </param>
        /// <returns>
        /// NP_E_SUCCESS on success, NP_E_INVALID_ARG when device is NULL,
        /// or NP_E_CONN_FAILED when the connection to the device could not be
        /// established.
        /// </returns>
        public virtual NotificationProxyError np_client_new(iDeviceHandle device, LockdownServiceDescriptorHandle service, out NotificationProxyClientHandle client)
        {
            NotificationProxyError returnValue;
            returnValue = NotificationProxyNativeMethods.np_client_new(device, service, out client);
            client.Api = this.Parent;
            return returnValue;
        }
        
        /// <summary>
        /// Starts a new notification proxy service on the specified device and connects to it.
        /// </summary>
        /// <param name="device">
        /// The device to connect to.
        /// </param>
        /// <param name="client">
        /// Pointer that will point to a newly allocated
        /// np_client_t upon successful return. Must be freed using
        /// np_client_free() after use.
        /// </param>
        /// <param name="label">
        /// The label to use for communication. Usually the program name.
        /// Pass NULL to disable sending the label in requests to lockdownd.
        /// </param>
        /// <returns>
        /// NP_E_SUCCESS on success, or an NP_E_* error
        /// code otherwise.
        /// </returns>
        public virtual NotificationProxyError np_client_start_service(iDeviceHandle device, out NotificationProxyClientHandle client, string label)
        {
            NotificationProxyError returnValue;
            returnValue = NotificationProxyNativeMethods.np_client_start_service(device, out client, label);
            client.Api = this.Parent;
            return returnValue;
        }
        
        /// <summary>
        /// Disconnects a notification_proxy client from the device and frees up the
        /// notification_proxy client data.
        /// </summary>
        /// <param name="client">
        /// The notification_proxy client to disconnect and free.
        /// </param>
        /// <returns>
        /// NP_E_SUCCESS on success, or NP_E_INVALID_ARG when client is NULL.
        /// </returns>
        public virtual NotificationProxyError np_client_free(System.IntPtr client)
        {
            return NotificationProxyNativeMethods.np_client_free(client);
        }
        
        /// <summary>
        /// Sends a notification to the device's notification_proxy.
        /// </summary>
        /// <param name="client">
        /// The client to send to
        /// </param>
        /// <param name="notification">
        /// The notification message to send
        /// </param>
        /// <returns>
        /// NP_E_SUCCESS on success, or an error returned by np_plist_send
        /// </returns>
        public virtual NotificationProxyError np_post_notification(NotificationProxyClientHandle client, string notification)
        {
            return NotificationProxyNativeMethods.np_post_notification(client, notification);
        }
        
        /// <summary>
        /// Tells the device to send a notification on the specified event.
        /// </summary>
        /// <param name="client">
        /// The client to send to
        /// </param>
        /// <param name="notification">
        /// The notifications that should be observed.
        /// </param>
        /// <returns>
        /// NP_E_SUCCESS on success, NP_E_INVALID_ARG when client or
        /// notification are NULL, or an error returned by np_plist_send.
        /// </returns>
        public virtual NotificationProxyError np_observe_notification(NotificationProxyClientHandle client, string notification)
        {
            return NotificationProxyNativeMethods.np_observe_notification(client, notification);
        }
        
        /// <summary>
        /// Tells the device to send a notification on specified events.
        /// </summary>
        /// <param name="client">
        /// The client to send to
        /// </param>
        /// <param name="notification_spec">
        /// Specification of the notifications that should be
        /// observed. This is expected to be an array of const char* that MUST have a
        /// terminating NULL entry.
        /// </param>
        /// <returns>
        /// NP_E_SUCCESS on success, NP_E_INVALID_ARG when client is null,
        /// or an error returned by np_observe_notification.
        /// </returns>
        public virtual NotificationProxyError np_observe_notifications(NotificationProxyClientHandle client, out string notificationSpec)
        {
            return NotificationProxyNativeMethods.np_observe_notifications(client, out notificationSpec);
        }
        
        /// <summary>
        /// This function allows an application to define a callback function that will
        /// be called when a notification has been received.
        /// It will start a thread that polls for notifications and calls the callback
        /// function if a notification has been received.
        /// In case of an error condition when polling for notifications - e.g. device
        /// disconnect - the thread will call the callback function with an empty
        /// notification "" and terminate itself.
        /// </summary>
        /// <param name="client">
        /// the NP client
        /// </param>
        /// <param name="notify_cb">
        /// pointer to a callback function or NULL to de-register a
        /// previously set callback function.
        /// </param>
        /// <param name="user_data">
        /// Pointer that will be passed to the callback function as
        /// user data. If notify_cb is NULL, this parameter is ignored.
        /// </param>
        /// <returns>
        /// NP_E_SUCCESS when the callback was successfully registered,
        /// NP_E_INVALID_ARG when client is NULL, or NP_E_UNKNOWN_ERROR when
        /// the callback thread could no be created.
        /// </returns>
        /// <remarks>
        /// Only one callback function can be registered at the same time;
        /// any previously set callback function will be removed automatically.
        /// </remarks>
        public virtual NotificationProxyError np_set_notify_callback(NotificationProxyClientHandle client, NotificationProxyNotifyCallBack notifyCallBack, System.IntPtr userdata)
        {
            return NotificationProxyNativeMethods.np_set_notify_callback(client, notifyCallBack, userdata);
        }
    }
}
