using SpanTests.Core.ObjectModel;
using SpanTests.Core.Parsing;
using System;

namespace SpanTests.Core.Tokenization
{
    /// <summary>
    /// Tokenizer that splits a <see cref="ReadOnlySpan{char}"/> into a collection of <see cref="JsonObject" />
    /// </summary>
    internal ref struct JsonArrayTokenizer
    {
        #region Properties

        /// <summary>
        /// True if we still have some content to parse
        /// </summary>
        public bool HasContent => content.Length > 0;

        #endregion

        #region Instance variables

        /// <summary>
        /// A reference to the json content
        /// </summary>
        private ReadOnlySpan<char> content;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a <see cref="JsonArrayTokenizer"/>
        /// </summary>
        public JsonArrayTokenizer(ReadOnlySpan<char> content)
        {
            this.content = content;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the next <see cref="JsonObject"/> from the current position in the json content span, or throws if its retrieval was not successful.
        /// </summary>
        public JsonObject GetNextObject()
        {
            ReadOnlySpan<char> value = TryParseContent(out JsonObjectType type);

            return new JsonObject(string.Empty, type, value);
        }

        private ReadOnlySpan<char> TryParseContent(out JsonObjectType type)
        {
            type = JsonObject.GetType(ref content);
            int separatorIndex = ArrayParser.GetNextArraySeparatorIndex(ref content, type);

            ReadOnlySpan<char> result = content.Slice(0, separatorIndex);
            content = content.Slice(content.GetContentStartIndex(separatorIndex));

            result = Parser.TryParse(ref result, out type);

            return result;
        }

        #endregion
    }
}
