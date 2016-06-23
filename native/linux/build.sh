#!/bin/bash

path=$(pwd)
out=$path/out/usr/local

export PKG_CONFIG_PATH=$out/lib/pkgconfig/
echo PKG_CONFIG_PATH set to $PKG_CONFIG_PATH

cd $path/libplist
./autogen.sh --prefix=$out --without-cython
make
make install

cd $path/libusbmuxd
./autogen.sh --prefix=$out --without-cython
make
make install

cd $path/libimobiledevice
./autogen.sh --prefix=$out --without-cython
make
make install

cd $path/usbmuxd
./autogen.sh --prefix=$out --with-udevrulesdir=$out/lib/udev/rules.d
make
make install

cd $path

