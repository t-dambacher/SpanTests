using SpanTests.Core.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpanTests.Core.Deserialization
{
    /// <summary>
    /// Base class for all deserializers
    /// </summary>
    internal abstract class Deserializer
    {
        #region Properties

        /// <summary>
        /// The json type handled
        /// </summary>
        public abstract JsonObjectType Type { get; }

        #endregion

        #region Instance methods

        /// <summary>
        /// Deserialize the <paramref name="content"/> as a <paramref name="resultType"/> object
        /// </summary>
        public abstract object Deserialize(ReadOnlySpan<char> content, Type resultType);

        #endregion

        #region Static fields

        /// <summary>
        /// All known deserializers
        /// </summary>
        private static readonly IEnumerable<Deserializer> deserializers = new Deserializer[]
        {
            new CollectionDeserializer(),
            new ObjectDeserializer(),
            new PrimitiveDeserializer(),
            new StringDeserializer()
        };

        #endregion

        #region Static methods

        /// <summary>
        /// Get a new <see cref="IDeserializer"/> able to work with the given <see cref="JsonObjectType"/>
        /// </summary>
        public static Deserializer Get(JsonObjectType type)
        {
            return deserializers.FirstOrDefault(d => d.Type == type) ?? throw new ArgumentException(nameof(type));
        }

        #endregion
    }
}
