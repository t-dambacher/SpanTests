using SpanTests.Core.Deserialization;
using SpanTests.Core.ObjectModel;
using System;

namespace SpanTests.Core.Parsing
{
    internal static class JsonParser<T>
    {
        public static T Parse(ReadOnlySpan<char> content)
        {
            try
            {
                content.TrimWhitespaces();
                ReadOnlySpan<char> result = Parser.TryParse(ref content, out JsonObjectType type);

                return (T)Deserializer.Deserialize(type, ref result, typeof(T));
            }
            catch (Exception ex)
            {
                throw new FormatException("Unable to parse the input json", ex);
            }
        }
    }
}
