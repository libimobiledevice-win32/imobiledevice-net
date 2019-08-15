#region MIT License

// Copyright 2018 Eric Freed
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 
// Comment.cs created on 2018-09-21

#endregion

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace iMobileDevice.Generator.Clang
{
    /// <summary>A parsed comment.</summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Comment
    {
        public readonly IntPtr AstNode; // const void *
        public readonly IntPtr TranslationUnit;

        /// <summary>Gets the children comments.</summary>
        /// <value>The children.</value>
        public IEnumerable<Comment> Children
        {
            get
            {
                var count = Clang.CommentGetNumChildren(this);
                for (uint i = 0; i < count; i++)
                    yield return Clang.CommentGetChild(this, i);
            }
        }

        /// <summary>Gets the number of children comments.</summary>
        /// <value>The children count.</value>
        public int ChildrenCount => Convert.ToInt32(Clang.CommentGetNumChildren(this));

        /// <summary>Gets the kind of the comment.</summary>
        /// <value>The kind.</value>
        public CommentKind Kind => Clang.CommentGetKind(this);

        /// <summary>Gets the block command argument text of the specified word-like argument.</summary>
        /// <param name="index">The index of the argument.</param>
        /// <returns>The associated text.</returns>
        public string GetBlockCommandArgText(uint index) => Clang.BlockCommandCommentGetArgText(this, index);

        /// <summary>Gets the child comment at the specified index.</summary>
        /// <param name="index">The index of the child comment to retrieve.</param>
        /// <returns>The child comment</returns>
        public Comment GetChildAt(int index) => Clang.CommentGetChild(this, Convert.ToUInt32(index));

        /// <summary>Gets the name of the block command.</summary>
        /// <returns>The name of the block command.</returns>
        public string GetCommandName()
        {
            CXString value = Clang.BlockCommandCommentGetCommandName(this);
            using (ClangString cString = new ClangString(value))
            {
                return cString.ToString();
            }
        }

        /// <summary>Gets the name of the template parameter.</summary>
        /// <returns>The name.</returns>
        public string GetParamName()
        {
            CXString value = Clang.ParamCommandCommentGetParamName(this);
            using (ClangString cString = new ClangString(value))
            {
                return cString.ToString();
            }
        }

        /// <summary>Gets the text for.</summary>
        /// <returns>The text.</returns>
        public string GetText()
        {
            CXString value = Clang.TextCommentGetText(this);
            using(ClangString cString = new ClangString(value))
            {
                return cString.ToString();
            }
        }
    }
}