using System;
using System.ComponentModel;
using SpanTests.Core.ObjectModel;

namespace SpanTests.Core.Deserialization
{
    /// <summary>
    /// Deserializer for primitives
    /// </summary>
    internal static class PrimitiveDeserializer
    {
        /// <summary>
        /// Deserialize the given <see cref="ReadOnlySpan{char}"/> into the given primitive type
        /// </summary>
        public static object Deserialize(ReadOnlySpan<char> content, Type primitiveType)
        {
            string value = StringDeserializer.Deserialize(content);
            return TypeDescriptor.GetConverter(primitiveType).ConvertFromString(value);
        }
    }
}
