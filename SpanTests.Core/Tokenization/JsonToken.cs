using System;
using System.Collections.Generic;

namespace SpanTests.Core.Tokenization
{
    /// <summary>
    /// Helper about json tokens
    /// </summary>
    internal static class JsonToken
    {
        #region Constants

        /// <summary>
        /// Start of an object
        /// </summary>
        public const char ObjectStart = '{';

        /// <summary>
        /// End of an object
        /// </summary>
        public const char ObjectEnd = '}';

        /// <summary>
        /// Start of an array
        /// </summary>
        public const char ArrayStart = '[';

        /// <summary>
        /// End of an array
        /// </summary>
        public const char ArrayEnd = ']';

        /// <summary>
        /// Start or end of a name
        /// </summary>
        public const char NameBoundary = '"';

        /// <summary>
        /// Separator after a name
        /// </summary>
        public const char NameSeparator = ':';

        /// <summary>
        /// Separator after an object
        /// </summary>
        public const char ObjectSeparator = ',';

        #endregion

        #region Static methods

        /// <summary>
        /// Gets the token type at the current position of the <see cref="ReadOnlySpan{char}"/>
        /// </summary>
        public static JsonTokenType GetType(ReadOnlySpan<char> content)
        {
            return GetType(content[0]);
        }

        /// <summary>
        /// Gets the token type of the given input <see cref="char"/>
        /// </summary>
        public static JsonTokenType GetType(char token)
        {
            var values = new Dictionary<char, JsonTokenType>
            {
                { ObjectStart, JsonTokenType.ObjectStart },
                { ObjectEnd, JsonTokenType.ObjectEnd },
                { ArrayStart, JsonTokenType.ArrayStart },
                { ArrayEnd, JsonTokenType.ArrayEnd },
                { NameBoundary, JsonTokenType.NameBoundary },
                { NameSeparator, JsonTokenType.NameSeparator },
                { ObjectSeparator, JsonTokenType.ObjectSeparator }
            };

            if (values.TryGetValue(token, out JsonTokenType knownTokenType))
            {
                return knownTokenType;
            }
            
            if (char.IsWhiteSpace(token))
            {
                return JsonTokenType.Whitespace;
            }

            return JsonTokenType.Unknown;
        }

        #endregion
    }
}