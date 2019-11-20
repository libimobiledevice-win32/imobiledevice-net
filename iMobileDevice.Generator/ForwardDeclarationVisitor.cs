// <copyright file="ForwardDeclarationVisitor.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using ClangSharp.Interop;

    internal sealed class ForwardDeclarationVisitor
    {
        private readonly CXCursor beginningCXCursor;
        private readonly bool skipSystemHeaderCheck;
        private bool beginningCXCursorReached;

        public ForwardDeclarationVisitor(CXCursor beginningCXCursor, bool skipSystemHeaderCheck = false)
        {
            this.beginningCXCursor = beginningCXCursor;
            this.skipSystemHeaderCheck = skipSystemHeaderCheck;
        }

        public CXCursor ForwardDeclarationCXCursor { get; private set; }

        public unsafe CXChildVisitResult Visit(CXCursor cursor, CXCursor parent, void* client_data)
        {
            if (!this.skipSystemHeaderCheck && cursor.IsInSystemHeader())
            {
                return CXChildVisitResult.CXChildVisit_Continue;
            }

            if (cursor.Equals(this.beginningCXCursor))
            {
                this.beginningCXCursorReached = true;
                return CXChildVisitResult.CXChildVisit_Continue;
            }

            if (this.beginningCXCursorReached)
            {
                this.ForwardDeclarationCXCursor = cursor;
                return CXChildVisitResult.CXChildVisit_Break;
            }

            return CXChildVisitResult.CXChildVisit_Recurse;
        }
    }
}
