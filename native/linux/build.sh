#!/bin/bash

path=$(pwd)
out=$path/out/usr/local

export PKG_CONFIG_PATH=$out/lib/pkgconfig/
echo PKG_CONFIG_PATH set to $PKG_CONFIG_PATH

# Remove the output folder, if it already exists
rm -rf $out

cd $path/libplist
./autogen.sh --prefix=$out --without-cython
make clean
make
make install

cd $path/libusbmuxd
./autogen.sh --prefix=$out --without-cython
make clean
make
make install

cd $path/libimobiledevice
./autogen.sh --prefix=$out --without-cython
make clean
make
make install

cd $path/usbmuxd
./autogen.sh --prefix=$out --with-udevrulesdir=$out/lib/udev/rules.d
make clean
make
make install

cd $path

