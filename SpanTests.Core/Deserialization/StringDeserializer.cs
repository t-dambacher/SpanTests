using System;
using SpanTests.Core.ObjectModel;

namespace SpanTests.Core.Deserialization
{
    /// <summary>
    /// Deserializer for strings
    /// </summary>
    internal sealed class StringDeserializer : Deserializer
    {
        /// <summary>
        /// <see cref="IDeserializer.Type"/>
        /// </summary>
        public override JsonObjectType Type => JsonObjectType.String;

        /// <summary>
        /// Deserialize the given <see cref="ReadOnlySpan{char}"/> into a string
        /// </summary>
        public override object Deserialize(ReadOnlySpan<char> content, Type type)
        {
            return new string(content);
        }
    }
}
