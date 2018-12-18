using SpanTests.Core.Tokenization;
using System;

namespace SpanTests.Core.Parsing
{
    internal static class StringParser
    {
        /// <summary>
        /// <see cref="Parser.TryParse(ref ReadOnlySpan{char})"/>
        /// </summary>
        public static ReadOnlySpan<char> TryParse(ref ReadOnlySpan<char> content)
        {
            int startIndex = 1; // The first char is a "
            int stringEndsAt = content.IndexOf(JsonToken.NameBoundary, startIndex);
            if (stringEndsAt < 0)
            {
                throw new InvalidOperationException("Unable to parse a string");
            }

            ReadOnlySpan<char> result = content.Slice(startIndex, stringEndsAt - startIndex);

            content = content.Slice(stringEndsAt + 1);  // +1 for the trailing "

            return result;
        }
    }
}
