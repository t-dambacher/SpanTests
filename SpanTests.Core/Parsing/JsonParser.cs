using SpanTests.Core.Deserialization;
using SpanTests.Core.ObjectModel;
using System;

namespace SpanTests.Core.Parsing
{
    internal sealed class JsonParser<T>
    {
        private static readonly JsonParser<T> instance = new JsonParser<T>();

        public T GetResult(ReadOnlySpan<char> content)
        {
            try
            {
                content = content.TrimWhitespaces();
                ReadOnlySpan<char> result = Parser.TryParse(ref content, out JsonObjectType type);

                Deserializer deserializer = Deserializer.Get(type);
                return (T)deserializer.Deserialize(result, typeof(T));
            }
            catch (Exception ex)
            {
                throw new FormatException("Unable to parse the input json", ex);
            }
        }

        public static T Parse(ReadOnlySpan<char> content)
        {
            return instance.GetResult(content);
        }
    }
}
