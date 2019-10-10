Name:          ios-webkit-debug-proxy
Version:       1.8.6.build
Release:       1%{?dist}
Summary:       A DevTools proxy for iOS devices.
Group:         System/Management
Vendor:        openmamba
Distribution:  openmamba
Packager:      Frederik Carlier <frederik.carlier@quamotion.mobi>
URL:           https://github.com/google/ios-webkit-debug-proxy/
Source:        https://github.com/google/ios-webkit-debug-proxy/master/ios-webkit-debug-proxy-%{version}.tar.gz
License:       BSD

BuildRequires: glibc-devel
BuildRequires: libimobiledevice-devel
BuildRequires: openssl-devel
BuildRequires: libplist-devel

%description
A DevTools proxy (Chrome Remote Debugging Protocol) for iOS devices (Safari Remote Web Inspector).

%prep
%setup -q -n ios-webkit-debug-proxy

%build
./autogen.sh --prefix=/usr
make

%install
make DESTDIR=$RPM_BUILD_ROOT install

%files
%defattr(-,root,root)
%{_bindir}/ios_webkit_debug_proxy
%doc README.md LICENSE.md

%changelog
* Thu Oct 10 2019 Frederik Carlier <frederik.carlier@quamotion.mobi> 1.8.6
- Package created
