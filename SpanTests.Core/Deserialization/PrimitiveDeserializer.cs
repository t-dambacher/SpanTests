using System;
using System.ComponentModel;
using SpanTests.Core.ObjectModel;

namespace SpanTests.Core.Deserialization
{
    /// <summary>
    /// Deserializer for primitives
    /// </summary>
    internal sealed class PrimitiveDeserializer : Deserializer
    {
        #region Properties

        /// <summary>
        /// <see cref="IDeserializer.Type"/>
        /// </summary>
        public override JsonObjectType Type => JsonObjectType.Primitive;

        #endregion

        #region Methods

        /// <summary>
        /// Deserialize the given <see cref="ReadOnlySpan{char}"/> into the given primitive type
        /// </summary>
        public override object Deserialize(ReadOnlySpan<char> content, Type primitiveType)
        {
            string value = StringDeserializer.Deserialize(content);
            return TypeDescriptor.GetConverter(primitiveType).ConvertFromString(value);
        }

        #endregion
    }
}
