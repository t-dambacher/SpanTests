using System;

namespace SpanTests.Core.Deserialization
{
    /// <summary>
    /// Deserializer for strings
    /// </summary>
    internal static class StringDeserializer
    {
        /// <summary>
        /// Deserialize the given <see cref="ReadOnlySpan{char}"/> into a string
        /// </summary>
        public static string Deserialize(ReadOnlySpan<char> content)
        {
            return new string(content);
        }
    }
}
