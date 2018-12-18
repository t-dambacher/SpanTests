using SpanTests.Core.ObjectModel;
using SpanTests.Core.Tokenization;
using System;

namespace SpanTests.Core.Parsing
{
    internal static class ObjectParser
    {
        /// <summary>
        /// <see cref="Parser.TryParse(ref ReadOnlySpan{char})"/>
        /// </summary>
        public static ReadOnlySpan<char> TryParse(ref ReadOnlySpan<char> content)
        {
            int objectEndIndex = 0;
            for (int openedCurlyBracesCount = 0; objectEndIndex < content.Length; ++objectEndIndex)
            {
                if (content[objectEndIndex] == JsonToken.ObjectStart)
                {
                    ++openedCurlyBracesCount;
                    continue;
                }

                if (content[objectEndIndex] == JsonToken.ObjectEnd)
                {
                    if (openedCurlyBracesCount == 0)
                    {
                        break;
                    }

                    --openedCurlyBracesCount;
                }
            }

            ReadOnlySpan<char> array = content.Slice(0, objectEndIndex); // First char is a {, last one is a }

            content = content.Slice(objectEndIndex);

            return array;
        }

        public static ReadOnlySpan<char> GetBoundaries(ref ReadOnlySpan<char> content)
        {
            return content.GetBoundaries(JsonToken.ObjectStart, JsonToken.ObjectEnd);
        }
    }
}
