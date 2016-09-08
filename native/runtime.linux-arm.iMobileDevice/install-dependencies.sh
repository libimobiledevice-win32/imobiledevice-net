# You need to configure Ubuntu for multi-arch (armhf)
dpkg --add-architecture armhf

# and add the following to sources.conf:
# deb [arch=armhf] http://ports.ubuntu.com/ xenial main universe

apt-get install -y libicu-dev:armhf gcc-arm-linux-gnueabihf g++-arm-linux-gnueabihf libxml2-dev:armhf libusb-1.0-0:armhf libusb-1.0-0-dev:armhf libssl-dev:armhf

# We need patchelf 0.9; Ubuntu 16.04 includes 0.8 (sigh)
wget https://nixos.org/releases/patchelf/patchelf-0.9/patchelf-0.9.tar.gz
tar -xvzf patchelf-0.9.tar.gz
cd patchelf-0.9
./configure
make
cd ..
