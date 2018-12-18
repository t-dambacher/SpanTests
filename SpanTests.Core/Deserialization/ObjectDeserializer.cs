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
    internal static class ObjectDeserializer
    {
        public static object Deserialize(ReadOnlySpan<char> content, Type objectType)
        {
            content = ObjectParser.GetBoundaries(ref content);
            var tokenizer = new JsonPropertyTokenizer(ref content);

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
