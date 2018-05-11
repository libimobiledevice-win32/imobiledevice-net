namespace iMobileDevice.Generator.Polyfill
{
    public enum CommentKind : int
    {
        Null = 0,
        Text = 1,
        InlineCommand = 2,
        HTMLStartTag = 3,
        HTMLEndTag = 4,
        Paragraph = 5,
        BlockCommand = 6,
        ParamCommand = 7,
        TParamCommand = 8,
        VerbatimBlockCommand = 9,
        VerbatimBlockLine = 10,
        VerbatimLine = 11,
        FullComment = 12,
    }
}
