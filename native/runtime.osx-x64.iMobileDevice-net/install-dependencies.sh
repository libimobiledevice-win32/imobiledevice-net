brew install autoconf automake libtool libusb

$dir =`pwd`

cd /usr/local/include/
ln -s ../opt/openssl/include/openssl .

cd $dir
