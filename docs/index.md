# imobiledevice-net: A .NET API for working with iOS devices

[imobiledevice-net](http://github.com/libimobiledevice-win32/imobiledevice-net) is a library which allows you to interact with iOS devices
on Windows using any of the .NET Framework languages (such as C# or Visual Basic).

It is based on the [libimobiledevice](http://libimobiledevice.org) library.

## Installing
You can install imobiledev-net as a [NuGet package](https://www.nuget.org/packages/iMobileDevice-net/)

```
PM> Install-Package imobiledevice-net
```

## Getting started

### Using the library
Before you use the library, you must call `NativeLibraries.Load()` so that `libimobiledevice` is loaded correctly:

```
NativeLibraries.Load();
```

### Listing all iOS devices
The following snippit lists all devices which are currently connected to your PC:

```
ReadOnlyCollection<string> udids;
int count = 0;

var idevice = LibiMobileDevice.Instance.iDevice;
var lockdown = LibiMobileDevice.Instance.Lockdown;

var ret = idevice.idevice_get_device_list(out udids, ref count);

if (ret == iDeviceError.NoDevice)
{
    // Not actually an error in our case
    return;
}

ret.ThrowOnError();

// Get the device name
foreach (var udid in udids)
{
    iDeviceHandle deviceHandle;
    idevice.idevice_new(out deviceHandle, udid).ThrowOnError();

    LockdownClientHandle lockdownHandle;
    lockdown.lockdownd_client_new_with_handshake(deviceHandle, out lockdownHandle, "Quamotion").ThrowOnError();

    string deviceName;
    lockdown.lockdownd_get_device_name(lockdownHandle, out deviceName).ThrowOnError();

    deviceHandle.Dispose();
    lockdownHandle.Dispose();
}
```

## API documentation

For a full overview of the imobiledevice-net API, see [the imobiledevice-net API documentation](api/)

## Consulting, Training and Support

This repository is maintained by [Quamotion](http://quamotion.mobi). Quamotion develops test software for iOS and 
Android applications, based on the WebDriver protocol.

Quamotion offers various technologies related to automating iOS devices using computers running Windows or Linux.
This includes:
* The ability to remotely control iOS devices
* Extensions to libimobiledevice with support for the Instruments protocol
* Running Xcode UI Tests and Facebook WebDriverAgent tests

In certain cases, Quamotion also offers professional services - such as consulting, training and support - related
to imobiledivice-net and libimobiledevice.

Contact us at [info@quamotion.mobi](mailto:info@quamotion.mobi) for more information.