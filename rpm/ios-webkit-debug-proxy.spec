Name:          ios-webkit-debug-proxy
Version:       1.8.6.build
Release:       1%{?dist}
Summary:       A DevTools proxy for iOS devices.
Group:         System/Management
Packager:      Frederik Carlier <frederik.carlier@quamotion.mobi>
URL:           https://github.com/google/ios-webkit-debug-proxy/
Source:        https://github.com/google/ios-webkit-debug-proxy/master/ios-webkit-debug-proxy-%{version}.tar.gz
License:       BSD

BuildRequires: glibc-devel
BuildRequires: libimobiledevice-devel
BuildRequires: openssl-devel
BuildRequires: libplist-devel
BuildRequires: gdb

%description
A DevTools proxy (Chrome Remote Debugging Protocol) for iOS devices (Safari Remote Web Inspector).

%package devel
Summary: Development package for ios-webkit-debug-proxy
Group: Development/Libraries
Requires: ios-webkit-debug-proxy = %{version}-%{release}
Requires: pkgconfig

%description devel
%{name}, development headers and libraries.

%prep
%setup -q -n ios-webkit-debug-proxy

%build
./autogen.sh --prefix=/usr --libdir=/usr/lib64
make

%install
make DESTDIR=$RPM_BUILD_ROOT install

%files
%defattr(-,root,root)
%{_bindir}/ios_webkit_debug_proxy
%{_libdir}/libios_webkit_debug_proxy.so.*
%doc README.md LICENSE.md

%files devel
%defattr(-,root,root,-)
%{_libdir}/libios_webkit_debug_proxy.a
%{_libdir}/libios_webkit_debug_proxy.la
%{_libdir}/libios_webkit_debug_proxy.so
%{_includedir}/ios-webkit-debug-proxy/*.h

%changelog
* Thu Oct 10 2019 Frederik Carlier <frederik.carlier@quamotion.mobi> 1.8.6
- Package created
