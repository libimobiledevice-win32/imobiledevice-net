#!/bin/bash
PWD_FILE="$1"
shift 1

export GPG_TTY=`tty`
. /etc/lsb-release
echo Running on $DISTRIB_CODENAME

cat $PWD_FILE | gpg --batch --passphrase-fd 0 --yes $@
