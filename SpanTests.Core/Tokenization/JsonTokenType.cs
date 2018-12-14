namespace SpanTests.Core.Tokenization
{
    /// <summary>
    /// Various json token types
    /// </summary>
    internal enum JsonTokenType
    {
        /// <summary>
        /// Unkwown json token
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Start of an object
        /// </summary>
        ObjectStart,

        /// <summary>
        /// End of an object
        /// </summary>
        ObjectEnd,

        /// <summary>
        /// Start of an array
        /// </summary>
        ArrayStart,

        /// <summary>
        /// End of an array
        /// </summary>
        ArrayEnd,

        /// <summary>
        /// Start or end of a name
        /// </summary>
        NameBoundary,

        /// <summary>
        /// Separator after a name
        /// </summary>
        NameSeparator,

        /// <summary>
        /// Separator after an object
        /// </summary>
        ObjectSeparator,

        /// <summary>
        /// Whitespace
        /// </summary>
        Whitespace
    }
}
