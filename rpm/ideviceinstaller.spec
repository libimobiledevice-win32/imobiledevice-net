Name:          ideviceinstaller
Version:       1.1.3.build
Release:       1%{?dist}
Summary:       Manage applications of an iPhone or iPod Touch
Group:         System/Management
Packager:      Stefano Cotta Ramusino <stefano.cotta@openmamba.org>
URL:           http://cgit.sukimashita.com/ideviceinstaller.git
Source:        https://github.com/libimobiledevice-win32/ideviceinstaller.git/master/ideviceinstaller-%{version}.tar.gz
#Source:        http://www.libimobiledevice.org/downloads/ideviceinstaller-%{version}.tar.bz2
License:       GPL
## AUTOBUILDREQ-BEGIN
BuildRequires: glibc-devel
BuildRequires: libimobiledevice-devel
BuildRequires: openssl-devel
BuildRequires: libplist-devel
BuildRequires: libusbmuxd-devel
BuildRequires: zlib-devel
BuildRequires: libzip-devel
## AUTOBUILDREQ-END
BuildRequires: libplist-devel >= 1.11
BuildRoot:     %{_tmppath}/%{name}-%{version}-root

%description
ideviceinstaller allows to list, install, uninstall and archive apps of iPhone and iPhone Touch.

%prep
%setup -q -n ideviceinstaller

%build
./autogen.sh --prefix=/usr
make

%install
make DESTDIR=$RPM_BUILD_ROOT install

%files
%defattr(-,root,root)
%{_bindir}/ideviceinstaller
%{_mandir}/man1/ideviceinstaller.1.gz
%doc AUTHORS COPYING NEWS README.md

%changelog
* Thu Sep 11 2014 Silvan Calarco <silvan.calarco@mambasoft.it> 1.0.1.20140911git-1mamba
- update to 1.0.1.20140911git

* Thu Sep 11 2014 Automatic Build System <autodist@mambasoft.it> 1.0.1-2mamba
- rebuilt by autoport with build requirements: libplist-devel>=1.11-1mamba

* Thu Jun 07 2012 Stefano Cotta Ramusino <stefano.cotta@openmamba.org> 1.0.1-1mamba
- package created by autospec
