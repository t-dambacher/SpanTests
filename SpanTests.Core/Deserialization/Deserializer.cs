using SpanTests.Core.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpanTests.Core.Deserialization
{
    /// <summary>
    /// Base class for all deserializers
    /// </summary>
    internal static class Deserializer
    {
        public static object Deserialize(JsonObjectType type, ref ReadOnlySpan<char> content, Type resultType)
        {
            switch (type)
            {
                case JsonObjectType.Array:
                    return CollectionDeserializer.Deserialize(ref content, resultType);
                case JsonObjectType.Object:
                    return ObjectDeserializer.Deserialize(ref content, resultType);
                case JsonObjectType.Primitive:
                    return PrimitiveDeserializer.Deserialize(ref content, resultType);
                case JsonObjectType.String:
                    return StringDeserializer.Deserialize(ref content);
                default:
                    throw new ArgumentException(nameof(type));
            }
        }
    }
}
