set USBMUXD_VERSION=95
set LIBIMOBILEDEVICE_VERSION=112
wget -nc https://qmcdn.blob.core.windows.net/imobiledevice/usbmuxd-osx-x64-1.1.0-%USBMUXD_VERSION%.tar.gz -O ext\usbmuxd-osx-x64-1.1.0-%USBMUXD_VERSION%.tar.gz
wget -nc https://qmcdn.blob.core.windows.net/imobiledevice/usbmuxd-linux-arm-1.1.0-%USBMUXD_VERSION%.tar.gz -O ext\usbmuxd-linux-arm-1.1.0-%USBMUXD_VERSION%.tar.gz
wget -nc https://qmcdn.blob.core.windows.net/imobiledevice/usbmuxd-linux-arm64-1.1.0-%USBMUXD_VERSION%.tar.gz -O ext\usbmuxd-linux-arm64-1.1.0-%USBMUXD_VERSION%.tar.gz
wget -nc https://qmcdn.blob.core.windows.net/imobiledevice/usbmuxd-linux-x64-1.1.0-%USBMUXD_VERSION%.tar.gz -O ext\usbmuxd-linux-x64-1.1.0-%USBMUXD_VERSION%.tar.gz
wget -nc https://qmcdn.blob.core.windows.net/imobiledevice/libimobiledevice-osx-x64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz -O ext\libimobiledevice-osx-x64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz
wget -nc https://qmcdn.blob.core.windows.net/imobiledevice/libimobiledevice-linux-arm64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz -O ext\libimobiledevice-linux-arm64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz
wget -nc https://qmcdn.blob.core.windows.net/imobiledevice/libimobiledevice-linux-arm-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz -O ext\libimobiledevice-linux-arm-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz
wget -nc https://qmcdn.blob.core.windows.net/imobiledevice/libimobiledevice-linux-x64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz -O ext\libimobiledevice-linux-x64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz
7z x -aos ext\usbmuxd-osx-x64-1.1.0-%USBMUXD_VERSION%.tar.gz -oext\
7z x -aos ext\usbmuxd-linux-arm-1.1.0-%USBMUXD_VERSION%.tar.gz -oext\
7z x -aos ext\usbmuxd-linux-arm64-1.1.0-%USBMUXD_VERSION%.tar.gz -oext\
7z x -aos ext\usbmuxd-linux-x64-1.1.0-%USBMUXD_VERSION%.tar.gz -oext\
7z x -aos ext\libimobiledevice-osx-x64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz -oext\
7z x -aos ext\libimobiledevice-linux-arm64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz -oext\
7z x -aos ext\libimobiledevice-linux-arm-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz -oext\
7z x -aos ext\libimobiledevice-linux-x64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz -oext\
7z x -aos ext\usbmuxd-osx-x64-1.1.0-%USBMUXD_VERSION%.tar -oext\osx-64
7z x -aos ext\usbmuxd-linux-arm-1.1.0-%USBMUXD_VERSION%.tar -oext\linux-arm
7z x -aos ext\usbmuxd-linux-arm64-1.1.0-%USBMUXD_VERSION%.tar -oext\linux-arm64
7z x -aos ext\usbmuxd-linux-x64-1.1.0-%USBMUXD_VERSION%.tar -oext\linux-x64
7z x -aos ext\libimobiledevice-osx-x64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar -oext\osx-x64
7z x -aos ext\libimobiledevice-linux-arm64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar -oext\linux-arm64
7z x -aos ext\libimobiledevice-linux-arm-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar -oext\linux-arm
7z x -aos ext\libimobiledevice-linux-x64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar -oext\linux-x64
