#!/bin/bash

path=$(pwd)
out=$path/out/usr/local

export PKG_CONFIG_PATH=$out/lib/pkgconfig/
echo PKG_CONFIG_PATH set to $PKG_CONFIG_PATH

export CFLAGS=-fPIC

# OS X ships with openssl 0.9.x, but libimobiledevice requires 1.0. 
# Version 1.0 is installed via homebrew; so we need to manually set
# the path here
export openssl_LIBS="-dy -lssl -lcrypto -L/usr/local/opt/openssl/lib"
export openssl_CFLAGS="-I/usr/local/opt/openssl/include"

# Remove the output folder, if it already exists
rm -rf $out

cd $path/libplist
./autogen.sh --prefix=$out --without-cython --enable-static=yes --enable-shared=no
make clean
make
make install

cd $path/libusbmuxd
./autogen.sh --prefix=$out --without-cython --enable-static=yes --enable-shared=no
make clean
make
make install

cd $path/libimobiledevice
./autogen.sh --prefix=$out --without-cython --enable-static=no --enable-shared=yes
make clean
make
make install

cd $path/usbmuxd
./autogen.sh --prefix=$out --with-udevrulesdir=$out/lib/udev/rules.d
make clean
make
make install

cd $path

# Patch the dylib dependencies for all executables in the out directory
# Redirect from libimobiledevice.6.dylib in the out folder to libimobiledevice.dylib
# in the folder where the executable is located (this will be the setup in our target
# environment)
for f in $out/bin/*; do
   chmod +w $f

   # Skip the first line of the otool output, this is just the header
   dylibs=`otool -L $f | tail -n +2 | grep "libimobiledevice" | awk -F' ' '{ print $1 }'`

   for dylib in $dylibs; do
     echo Patching $dylib in $f

     # https://www.mikeash.com/pyblog/friday-qa-2009-11-06-linking-and-install-names.html
     install_name_tool -change $dylib @loader_path/libimobiledevice.dylib $f
   done;

   otool -L $f
done
