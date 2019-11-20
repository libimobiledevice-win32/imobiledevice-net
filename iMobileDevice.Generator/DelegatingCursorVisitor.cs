using ClangSharp.Interop;
using System;

namespace iMobileDevice.Generator
{
    internal class DelegatingCXCursorVisitor
    {
        private readonly Func<CXCursor, CXCursor, CXChildVisitResult> visitor;

        public DelegatingCXCursorVisitor(Func<CXCursor, CXCursor, CXChildVisitResult> visitor)
        {
            this.visitor = visitor;
        }

        public unsafe CXChildVisitResult Visit(CXCursor cursor, CXCursor parent, void* client_data)
        {
            return visitor(cursor, parent);
        }
    }
}
