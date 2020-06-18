Name:          libideviceactivation 
Version:       1.1.2.build
Release:       6%{?dist}
Summary:       A library to manage the activation process of Apple iOS devices.

License:       LGPLv2+
URL:           http://www.libimobiledevice.org/
Source0:       http://www.libimobiledevice.org/downloads/%{name}-%{version}.tar.gz

BuildRequires: libxml2-devel
BuildRequires: libcurl-devel
BuildRequires: gcc-c++
BuildRequires: gdb
BuildRequires: libplist-devel, libimobiledevice-devel
%description
A library to manage the activation process of Apple iOS devices.

%package devel
Summary: Development package for %{name} 
Requires: %{name} = %{version}-%{release}
Requires: pkgconfig

%description devel
%{name}, development headers and libraries.

%prep
%setup -q -n libideviceactivation

%build
./autogen.sh --prefix=/usr --libdir=/usr/lib64 --enable-static=no --enable-shared=yes
make

%install
make install DESTDIR=$RPM_BUILD_ROOT

rm -rf $RPM_BUILD_ROOT%{_libdir}/*.a
rm -rf $RPM_BUILD_ROOT%{_libdir}/*.la

%post -p /sbin/ldconfig

%postun -p /sbin/ldconfig

%files
%defattr(-,root,root,-)
%{_bindir}/ideviceactivation
%{_libdir}/libideviceactivation-1.0.so.*
%{_mandir}/man1/ideviceactivation.1.gz


%files devel
%defattr(-,root,root,-)
%{_libdir}/libideviceactivation-1.0.so
%{_libdir}/pkgconfig/libideviceactivation-1.0.pc
%{_includedir}/libideviceactivation.h