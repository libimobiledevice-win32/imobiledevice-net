using System;
using System.Runtime.InteropServices;

namespace iMobileDevice.Generator.Clang
{
    internal static class Clang
    {
        /// <summary>
        /// The library name, without path or file extension.
        /// </summary>
        public const string LIBRARY = "libclang";

        /// <summary>
        ///     Given a cursor that represents a documentable entity (e.g., declaration), return the
        ///     associated parsed comment as a Comment_FullComment AST node.
        /// </summary>
        [DllImport(LIBRARY, EntryPoint = "clang_Cursor_getParsedComment", CallingConvention = CallingConvention.Cdecl)]
        public static extern Comment CursorGetParsedComment(CXCursor cursor);

        /// <summary>AST node of any kind.</summary>
        [DllImport(LIBRARY, EntryPoint = "clang_Comment_getNumChildren", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint CommentGetNumChildren(Comment comment);

        /// <summary>AST node of any kind.</summary>
        [DllImport(LIBRARY, EntryPoint = "clang_Comment_getKind", CallingConvention = CallingConvention.Cdecl)]
        public static extern CommentKind CommentGetKind(Comment comment);

        /// <summary>
        ///     a Comment_BlockCommand AST node. argument index (zero-based). text of the specified
        ///     word-like argument.
        /// </summary>
        [DllImport(LIBRARY, EntryPoint = "clang_BlockCommandComment_getArgText",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern String BlockCommandCommentGetArgText(Comment comment, uint argIdx);

        /// <summary>AST node of any kind.</summary>
        [DllImport(LIBRARY, EntryPoint = "clang_Comment_getChild", CallingConvention = CallingConvention.Cdecl)]
        public static extern Comment CommentGetChild(Comment comment, uint childIdx);

		/// <summary>A Comment_BlockCommand AST node. name of the block command.</summary>
		[DllImport(LIBRARY, EntryPoint = "clang_BlockCommandComment_getCommandName",
			CallingConvention = CallingConvention.Cdecl)]
		public static extern CXString BlockCommandCommentGetCommandName(Comment comment);

        /// <summary>a Comment_ParamCommand AST node. parameter name.</summary>
        [DllImport(LIBRARY, EntryPoint = "clang_ParamCommandComment_getParamName",
            CallingConvention = CallingConvention.Cdecl)]
        public static extern CXString ParamCommandCommentGetParamName(Comment comment);

        /// <summary>a Comment_Text AST node. text contained in the AST node.</summary>
        [DllImport(LIBRARY, EntryPoint = "clang_TextComment_getText", CallingConvention = CallingConvention.Cdecl)]
        public static extern CXString TextCommentGetText(Comment comment);

        /// <summary>Free the given string.</summary>
        [DllImport(LIBRARY, EntryPoint = "clang_disposeString", CallingConvention = CallingConvention.Cdecl)]
        public static extern void DisposeString(CXString str);

        /// <summary>Retrieve the character data associated with the given string.</summary>
        [DllImport(LIBRARY, EntryPoint = "clang_getCString", CallingConvention = CallingConvention.Cdecl)]
        public static unsafe extern sbyte* GetCString(CXString str);
    }
}
