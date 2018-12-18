using SpanTests.Core.ObjectModel;
using SpanTests.Core.Parsing;
using SpanTests.Core.Reflection;
using SpanTests.Core.Tokenization;
using System;

namespace SpanTests.Core.Deserialization
{
    /// <summary>
    /// Deserializer for objects
    /// </summary>
    internal sealed class ObjectDeserializer : Deserializer
    {
        /// <summary>
        /// <see cref="IDeserializer.Type"/>
        /// </summary>
        public override JsonObjectType Type => JsonObjectType.Object;

        /// <summary>
        /// <see cref="IDeserializer.Deserialize(ReadOnlySpan{char}, Type)"/>
        /// </summary>
        public override object Deserialize(ReadOnlySpan<char> content, Type objectType)
        {
            content = ObjectParser.GetBoundaries(content);
            var tokenizer = new JsonPropertyTokenizer(content);

            object result = CtorStore.CreateInstance(objectType);

            while (tokenizer.HasContent)
            {
                JsonObject value = tokenizer.GetNextObject();
                value.SetOn(result);
            }

            return result;
        }
    }
}
