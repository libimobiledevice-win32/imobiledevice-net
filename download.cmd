set USBMUXD_VERSION=104
set LIBIMOBILEDEVICE_VERSION=134
wget -nc https://qmcdn.blob.core.windows.net/imobiledevice/usbmuxd-osx-x64-1.1.0-%USBMUXD_VERSION%.tar.gz -O ext\usbmuxd-osx-x64-1.1.0-%USBMUXD_VERSION%.tar.gz
REM wget -nc https://qmcdn.blob.core.windows.net/imobiledevice/usbmuxd-linux-arm-1.1.0-%USBMUXD_VERSION%.tar.gz -O ext\usbmuxd-linux-arm-1.1.0-%USBMUXD_VERSION%.tar.gz
REM wget -nc https://qmcdn.blob.core.windows.net/imobiledevice/usbmuxd-linux-arm64-1.1.0-%USBMUXD_VERSION%.tar.gz -O ext\usbmuxd-linux-arm64-1.1.0-%USBMUXD_VERSION%.tar.gz
wget -nc https://qmcdn.blob.core.windows.net/imobiledevice/usbmuxd-linux-x64-1.1.0-%USBMUXD_VERSION%.tar.gz -O ext\usbmuxd-linux-x64-1.1.0-%USBMUXD_VERSION%.tar.gz
wget -nc https://qmcdn.blob.core.windows.net/imobiledevice/libimobiledevice-osx-x64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz -O ext\libimobiledevice-osx-x64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz
REM wget -nc https://qmcdn.blob.core.windows.net/imobiledevice/libimobiledevice-linux-arm64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz -O ext\libimobiledevice-linux-arm64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz
REM wget -nc https://qmcdn.blob.core.windows.net/imobiledevice/libimobiledevice-linux-arm-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz -O ext\libimobiledevice-linux-arm-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz
wget -nc https://qmcdn.blob.core.windows.net/imobiledevice/libimobiledevice-linux-x64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz -O ext\libimobiledevice-linux-x64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz

REM extract tar file from tar.gz
7z x -aos ext\usbmuxd-osx-x64-1.1.0-%USBMUXD_VERSION%.tar.gz -oext\
REM 7z x -aos ext\usbmuxd-linux-arm-1.1.0-%USBMUXD_VERSION%.tar.gz -oext\
REM 7z x -aos ext\usbmuxd-linux-arm64-1.1.0-%USBMUXD_VERSION%.tar.gz -oext\
7z x -aos ext\usbmuxd-linux-x64-1.1.0-%USBMUXD_VERSION%.tar.gz -oext\
7z x -aos ext\libimobiledevice-osx-x64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz -oext\
REM 7z x -aos ext\libimobiledevice-linux-arm64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz -oext\
REM 7z x -aos ext\libimobiledevice-linux-arm-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz -oext\
7z x -aos ext\libimobiledevice-linux-x64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar.gz -oext\

REM extract files from the tar files
7z x -aos ext\usbmuxd-osx-x64-1.1.0-%USBMUXD_VERSION%.tar -oext\osx-64
REM 7z x -aos ext\usbmuxd-linux-arm-1.1.0-%USBMUXD_VERSION%.tar -oext\debian-arm
REM 7z x -aos ext\usbmuxd-linux-arm64-1.1.0-%USBMUXD_VERSION%.tar -oext\debian-arm64
7z x -aos ext\usbmuxd-linux-x64-1.1.0-%USBMUXD_VERSION%.tar -oext\debian-x64
7z x -aos ext\libimobiledevice-osx-x64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar -oext\osx-x64
REM 7z x -aos ext\libimobiledevice-linux-arm64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar -oext\debian-arm64
REM 7z x -aos ext\libimobiledevice-linux-arm-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar -oext\debian-arm
7z x -aos ext\libimobiledevice-linux-x64-1.2.1-%LIBIMOBILEDEVICE_VERSION%.tar -oext\debian-x64
