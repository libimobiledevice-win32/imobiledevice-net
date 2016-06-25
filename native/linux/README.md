# Building libimobiledevice-win32 on Linux

This sounds perhaps like a contradiction, but you can build libimobiledevice-win32 on Linux.
libimobiledevice-win32 deviates slightly from libimobiledevice, even on Linux platforms, because it contains additional
methods (mainly `_free`  methods) which enable P/Invoke support (i.e. ensure the code can be called correctly from .NET).

This folder contains scripts which allow you to build libimobiledevice-win32 in a single to. It's a goal to support cross-
compilation for ARM in the future.

These steps have been verified on Ubuntu 14.04:

1. Install the software required to compile libimobiledevice-win32: `./install-dependencies.sh`
2. Clone the libimobiledevice-win32 repositories: `./get-sources.sh`
3. Build the libimobiledevice-win32 files: `./build.sh` 

The output will be generated in the `out` folder.

If you want to do a quick smoke test, you can:

0. [Disable `usbmuxd` which ships with Ubuntu 14.04](https://www.zeitgeist.se/2015/06/28/mount-an-iphone-inside-a-kvm-guest-by-disabling-usbmuxd/): `$ sudo touch /etc/udev/rules.d/39-usbmuxd.rules`
1. Launch `usbmuxd`: `sudo out/usr/local/sbin/usbmuxd -f`
2. Run `idevice_id` to get a list of all recognized iOS devices: `out/usr/local/bin/idevice_id -l` 
