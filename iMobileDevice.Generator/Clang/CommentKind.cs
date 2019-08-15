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
// CommentKind.cs created on 2018-09-21

#endregion

namespace iMobileDevice.Generator.Clang
{
    /// <summary>
    ///     Describes the type of the comment AST node (<see cref="Comment" />).
    ///     <para>
    ///         A comment node can be considered block content (e. g., paragraph), inline content (plain
    ///         text) or neither (the root AST node).
    ///     </para>
    /// </summary>
    public enum CommentKind
    {
        /// <summary>
        ///     Null comment.  No AST node is constructed at the requested location because there is no
        ///     text or a syntax error.
        /// </summary>
        Null = 0,

        /// <summary>Plain text.  Inline content.</summary>
        Text = 1,

        /// <summary>A command with word-like arguments that is considered inline content.</summary>
        InlineCommand = 2,

        /// <summary>HTML start tag with attributes (name-value pairs).  Considered inline content.</summary>
        HtmlStartTag = 3,

        /// <summary>HTML end tag.  Considered inline content.</summary>
        HtmlEndTag = 4,

        /// <summary>A paragraph, contains inline comment.  The paragraph itself is block content.</summary>
        Paragraph = 5,

        /// <summary>
        ///     A command that has zero or more word-like arguments (number of word-like arguments depends
        ///     on command name) and a paragraph as an argument.
        ///     <para>Block command is block content. Paragraph argument is also a child of the block command.</para>
        /// </summary>
        BlockCommand = 6,

        /// <summary>
        ///     A param or arg command that describes the function parameter (name, passing direction,
        ///     description).
        /// </summary>
        ParamCommand = 7,

        /// <summary>A tparam command that describes a template parameter (name and description).</summary>
        TParamCommand = 8,

        /// <summary>A verbatim block command (e. g., preformatted code).</summary>
        VerbatimBlockCommand = 9,

        /// <summary>A line of text that is contained within a verbatim comment node.</summary>
        VerbatimBlockLine = 10,

        /// <summary>
        ///     A verbatim line command.  Verbatim line has an opening command, a single line of text (up
        ///     to the newline after the opening command) and has no closing command.
        /// </summary>
        VerbatimLine = 11,

        /// <summary>A full comment attached to a declaration, contains block content.</summary>
        FullComment = 12
    }
}