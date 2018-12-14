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

        #region Instance fields

        private readonly Lazy<Deserializer> stringDeserializer = new Lazy<Deserializer>(() => Get(JsonObjectType.String));

        #endregion

        #region Methods

        /// <summary>
        /// Deserialize the given <see cref="ReadOnlySpan{char}"/> into the given primitive type
        /// </summary>
        public override object Deserialize(ReadOnlySpan<char> content, Type primitiveType)
        {
            string value = (string)stringDeserializer.Value.Deserialize(content, typeof(string));
            return TypeDescriptor.GetConverter(primitiveType).ConvertFromString(value);
        }

        #endregion
    }
}
