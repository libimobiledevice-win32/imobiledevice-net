#!/bin/bash

path=$(pwd)
out=$path/out/usr/local
host=arm-linux-gnueabihf

export PKG_CONFIG_PATH=$out/lib/pkgconfig/
echo PKG_CONFIG_PATH set to $PKG_CONFIG_PATH

export CFLAGS=-fPIC
# Remove the output folder, if it already exists
rm -rf $out

cd $path/libplist
./autogen.sh --prefix=$out --without-cython --enable-static=yes --enable-shared=no --host=$host
automake -a
make clean
make
make install

cd $path/libusbmuxd
./autogen.sh --prefix=$out --without-cython --enable-static=yes --enable-shared=no --host=$host
make clean
make
make install

cd $path/libimobiledevice
./autogen.sh --prefix=$out --without-cython --enable-static=no --enable-shared=yes --enable-openssl --host=$host
make clean
make
make install

cd $path/usbmuxd
./autogen.sh --prefix=$out --with-udevrulesdir=$out/lib/udev/rules.d --without-systemd --host=$host
make clean
make
make install

cd $path

cp /usr/lib/$host/libusb-1.0.so $out/lib/

# Patch the so dependencies for all executables in the out directory 
# Redirect from libimobiledevice.so.6 to libimobiledevice.so,
# and change the out folder in the rpath to ${ORIGIN}
# (this will be the setup in our target environment)
#
# For more info, see:
# https://github.com/NixOS/patchelf
# http://man7.org/linux/man-pages/man8/ld.so.8.html

patchelf=patchelf-0.9/src/patchelf

for f in $out/*bin/*; do
   chmod +w $f

   $patchelf --set-rpath '${ORIGIN}' $f
   $patchelf --remove-needed libimobiledevice.so.6 $f
   $patchelf --add-needed libimobiledevice.so $f

   # readelf -d $f
done
