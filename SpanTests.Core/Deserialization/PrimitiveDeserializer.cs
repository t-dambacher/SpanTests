using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace SpanTests.Core.Deserialization
{
    /// <summary>
    /// Deserializer for primitives
    /// </summary>
    internal static class PrimitiveDeserializer
    {
        private static Dictionary<Type, TypeConverter> converters = new Dictionary<Type, TypeConverter>();

        /// <summary>
        /// Deserialize the given <see cref="ReadOnlySpan{char}"/> into the given primitive type
        /// </summary>
        public static object Deserialize(ref ReadOnlySpan<char> content, Type primitiveType)
        {
            string value = StringDeserializer.Deserialize(ref content);

            if (!converters.TryGetValue(primitiveType, out  TypeConverter converter))
            {
                converter = TypeDescriptor.GetConverter(primitiveType);
                converters.Add(primitiveType, converter);
            }

            return converter.ConvertFromString(value);
        }
    }
}
