#!/bin/bash

sudo apt-get install -y build-essential autotools-dev libtool automake libxml2 libxml2-dev pkg-config python-dev libusb-1.0-0 libusb-1.0-0-dev libssl-dev

# We need patchelf 0.9; Ubuntu 16.04 includes 0.8 (sigh)
wget https://nixos.org/releases/patchelf/patchelf-0.9/patchelf-0.9.tar.gz
tar -xvzf patchelf-0.9.tar.gz
cd patchelf
./configure
make
cd ..

