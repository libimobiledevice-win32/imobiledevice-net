#!/bin/bash
git clone https://github.com/libimobiledevice-win32/libplist --depth 1 --single-branch --branch msvc-master
git clone https://github.com/libimobiledevice-win32/libimobiledevice --depth 1 --single-branch --branch msvc-master
git clone https://github.com/libimobiledevice-win32/libusbmuxd --depth 1 --single-branch --branch msvc-master
git clone https://github.com/libimobiledevice-win32/usbmuxd --depth 1 --single-branch --branch master-msvc
