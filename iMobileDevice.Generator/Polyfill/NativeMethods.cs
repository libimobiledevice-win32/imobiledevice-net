using System.Runtime.InteropServices;

namespace iMobileDevice.Generator.Polyfill
{
    internal static class NativeMethods
    {
        private const CallingConvention NativeCallingConvention = CallingConvention.StdCall;
        private const string libraryPath = "libclang";

        [DllImport(libraryPath, EntryPoint = "clang_Cursor_getParsedComment")]
        public static extern Comment Cursor_getParsedComment(CXCursor cursor);

        [DllImport(libraryPath, EntryPoint = "clang_Comment_getKind")]
        public static extern CommentKind Comment_getKind(Comment comment);

        [DllImport(libraryPath, EntryPoint = "clang_Comment_getNumChildren")]
        public static extern uint Comment_getNumChildren(Comment comment);

        [DllImport(libraryPath, EntryPoint = "clang_Comment_getChild")]
        public static extern Comment Comment_getChild(Comment comment, uint childIndex);

        [DllImport(libraryPath, EntryPoint = "clang_TextComment_getText")]
        public static extern CXString TextComment_getText(Comment comment);

        [DllImport(libraryPath, EntryPoint = "clang_ParamCommandComment_getParamName")]
        public static extern CXString ParamCommandComment_getParamName(Comment comment);

        [DllImport(libraryPath, EntryPoint = "clang_BlockCommandComment_getCommandName", CallingConvention = CallingConvention.Cdecl)]
        public static extern CXString BlockCommandComment_getCommandName(Comment comment);

        [DllImport(libraryPath, EntryPoint = "clang_disposeString")]
        public static extern void DisposeString(CXString @string);

        [DllImport(libraryPath, EntryPoint = "clang_getCString")]
        public static unsafe extern sbyte* GetCString(CXString @string);
    }
}
