using System;
using System.Runtime.InteropServices;

namespace iMobileDevice.Generator.Clang
{
    internal sealed unsafe class ClangString : IDisposable
    {
        public CXString Struct { get; }

        public ClangString(CXString cxString)
        {
            Struct = cxString;
        }

        private bool disposed;

        public void Dispose()
        {
            if (!disposed)
            {
                Clang.DisposeString(Struct);

                disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        ~ClangString()
        {
            Dispose();
        }

        internal void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(typeof(ClangString).Name);
            }
        }

        public override string ToString()
        {
            ThrowIfDisposed();

            sbyte* cString = Clang.GetCString(Struct);
            return Marshal.PtrToStringAnsi(new IntPtr(cString));
        }
    }
}
