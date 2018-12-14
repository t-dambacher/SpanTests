using SpanTests.Core.ObjectModel;
using SpanTests.Core.Tokenization;
using System;

namespace SpanTests.Core.Parsing
{
    internal sealed class ArrayParser : Parser
    {
        #region Properties

        /// <summary>
        /// <see cref="Parser.Type"/>
        /// </summary>
        public override JsonObjectType Type => JsonObjectType.Array;

        #endregion

        #region Methods

        /// <summary>
        /// <see cref="Parser.TryParse(ref ReadOnlySpan{char})"/>
        /// </summary>
        public override ReadOnlySpan<char> TryParse(ref ReadOnlySpan<char> content)
        {
            int arrayEndIndex = 0;
            for (int openedBracketsCount = 0; arrayEndIndex < content.Length; ++arrayEndIndex)
            {
                if (content[arrayEndIndex] == JsonToken.ArrayStart)
                {
                    ++openedBracketsCount;
                    continue;
                }

                if (content[arrayEndIndex] == JsonToken.ArrayEnd)
                {
                    if (openedBracketsCount == 0)
                    {
                        break;
                    }

                    --openedBracketsCount;
                }
            }

            ReadOnlySpan<char> array = content.Slice(1, arrayEndIndex - 2); // First char is a [, last one is a ]

            content = content.Slice(arrayEndIndex);

            return array;
        }

        #endregion

        #region Static methods

        public static int GetNextArraySeparatorIndex(ReadOnlySpan<char> content, JsonObjectType expectedType)
        {
            switch (expectedType)
            {
                case JsonObjectType.Primitive:
                    return content.IndexOf(JsonToken.ObjectSeparator);
                case JsonObjectType.String:
                    return GetNextArraySeparatorIndexForString(content);
                case JsonObjectType.Object:
                    return GetNextArraySeparatorIndexForObject(content);
                case JsonObjectType.Array:
                    throw new InvalidOperationException("Currently not supported"); // difficult with the current impl, needs recursion...
            }

            throw new ArgumentException(nameof(expectedType));
        }

        private static int GetNextArraySeparatorIndexForObject(ReadOnlySpan<char> content)
        {
            // difficult with the current impl, needs recursion...
            for (int separatorIndex = 0, curlyBracesCount = 0; separatorIndex < content.Length; ++separatorIndex)
            {
                if (content[separatorIndex] == JsonToken.ObjectStart)
                {
                    ++curlyBracesCount;
                }
                else if (content[separatorIndex] == JsonToken.ObjectEnd)
                {
                    --curlyBracesCount;
                }
                else if (content[separatorIndex] == JsonToken.ObjectSeparator && curlyBracesCount == 0)
                {
                    return separatorIndex;
                }
            }

            return content.Length;
        }

        private static int GetNextArraySeparatorIndexForString(ReadOnlySpan<char> content)
        {
            bool nameBoundaryFound = false;
            for (int separatorIndex = 0; separatorIndex < content.Length; ++separatorIndex)
            {
                if (content[separatorIndex] == JsonToken.NameBoundary)
                {
                    nameBoundaryFound = !nameBoundaryFound;
                }
                else if (content[separatorIndex] == JsonToken.ObjectSeparator && !nameBoundaryFound)
                {
                    return separatorIndex;
                }
            }

            return content.Length;
        }

        #endregion
    }
}
