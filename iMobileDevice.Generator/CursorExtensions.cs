// <copyright file="CursorExtensions.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

namespace iMobileDevice.Generator
{
    using ClangSharp;

    internal static class CursorExtensions
    {
        public static bool IsInSystemHeader(this CXCursor cursor)
        {
            return clang.Location_isInSystemHeader(clang.getCursorLocation(cursor)) != 0;
        }
    }
}
