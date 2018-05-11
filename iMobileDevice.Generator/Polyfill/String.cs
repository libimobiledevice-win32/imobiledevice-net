using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace iMobileDevice.Generator.Polyfill
{
    internal sealed unsafe class String : IDisposable
    {
        public CXString Struct { get; }

        public String(CXString cxString)
        {
            Struct = cxString;
        }

        private bool disposed;

        public void Dispose()
        {
            if (!disposed)
            {
                NativeMethods.DisposeString(Struct);

                disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        ~String()
        {
            Dispose();
        }

        internal void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(typeof(String).Name);
            }
        }

        public override string ToString()
        {
            ThrowIfDisposed();

            var cString = NativeMethods.GetCString(Struct);
            return Marshal.PtrToStringAnsi(new IntPtr(cString));
        }
    }
}
