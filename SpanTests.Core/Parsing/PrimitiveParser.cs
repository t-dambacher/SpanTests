using System;

namespace SpanTests.Core.Parsing
{
    internal static class PrimitiveParser
    {
        /// <summary>
        /// <see cref="Parser.TryParse(ref ReadOnlySpan{char})"/>
        /// </summary>
        public static ReadOnlySpan<char> TryParse(ref ReadOnlySpan<char> content)
        {
            int primitiveEndIndex = 0;
            for (; primitiveEndIndex < content.Length; ++primitiveEndIndex)
            {
                if (!IsPrimitive(content[primitiveEndIndex]))
                {
                    break;
                }
            }

            ReadOnlySpan<char> result = content.Slice(0, primitiveEndIndex);
            content = content.Slice(primitiveEndIndex);

            return result;
        }

        public static bool IsPrimitive(ReadOnlySpan<char> content)
        {
            return IsPrimitive(content[0]);
        }

        public static bool IsPrimitive(char value)
        {
            return char.IsLetterOrDigit(value) || char.IsSurrogate(value);
        }
    }
}
