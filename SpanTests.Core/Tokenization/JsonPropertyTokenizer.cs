using SpanTests.Core.ObjectModel;
using SpanTests.Core.Parsing;
using System;

namespace SpanTests.Core.Tokenization
{
    /// <summary>
    /// Tokenizer that splits a <see cref="ReadOnlySpan{char}"/> into a collection of <see cref="JsonObject" />
    /// </summary>
    internal ref struct JsonPropertyTokenizer
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
        /// Creates a new instance of a <see cref="JsonPropertyTokenizer"/>
        /// </summary>
        public JsonPropertyTokenizer(ref ReadOnlySpan<char> content)
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
            JsonTokenType tokenType = JsonToken.GetType(content);
            if (tokenType != JsonTokenType.NameBoundary)
            {
                throw new InvalidOperationException("Unable to parse the sub content");
            }

            string name = TryParseName();
            ReadOnlySpan<char> value = TryParseContent(out JsonObjectType type);

            return new JsonObject(name, type, value);
        }

        private string TryParseName()
        {
            int nameStartsAt = content.IndexOf(JsonToken.NameBoundary) + 1;
            if (nameStartsAt <= 0)
            {
                throw new ArgumentException(nameof(content));
            }

            int nameEndsAt = content.IndexOf(JsonToken.NameBoundary, nameStartsAt);
            if (nameEndsAt < 0)
            {
                throw new ArgumentException(nameof(content));
            }

            ReadOnlySpan<char> nameSpan = content.Slice(nameStartsAt, nameEndsAt - nameStartsAt);
            string name = new string(nameSpan);

            int nameSeparatorAt = content.IndexOf(JsonToken.NameSeparator, nameEndsAt) + 1;
            if (nameSeparatorAt <= 0)
            {
                throw new InvalidOperationException("Unable to find a separator in the content");
            }

            content.TrimWhitespaces(nameSeparatorAt);

            return name;
        }

        private ReadOnlySpan<char> TryParseContent(out JsonObjectType type)
        {
            ReadOnlySpan<char> result = Parser.TryParse(ref content, out type);

            int contentIndex = content.GetContentStartIndex();
            if (contentIndex != 0)
            {
                content = content.Slice(contentIndex);
            }

            return result;
        }

        #endregion
    }
}
