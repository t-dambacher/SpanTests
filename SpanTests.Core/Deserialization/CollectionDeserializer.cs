using SpanTests.Core.ObjectModel;
using SpanTests.Core.Parsing;
using SpanTests.Core.Tokenization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SpanTests.Core.Deserialization
{
    /// <summary>
    /// Deserializer for collections
    /// </summary>
    internal sealed class CollectionDeserializer : Deserializer
    {
        /// <summary>
        /// <see cref="IDeserializer.Type"/>
        /// </summary>
        public override JsonObjectType Type => JsonObjectType.Array;

        /// <summary>
        /// <see cref="IDeserializer.Deserialize(ReadOnlySpan{char}, Type)"/>
        /// </summary>
        public override object Deserialize(ReadOnlySpan<char> content, Type arrayType)
        {
            object collection = BuildCollection(arrayType, out Type elementType);

            // Append objects to the collection
            if (!content.IsEmptyOrWhiteSpace())
            {
                content = content.TrimWhitespaces();

                AppendToCollection(collection, GetSubElements(content, elementType));
            }

            return collection;
        }

        private static IEnumerable<object> GetSubElements(ReadOnlySpan<char> content, Type elementType)
        {
            List<object> result = new List<object>();

            var tokenizer = new JsonArrayTokenizer(content);

            while (tokenizer.HasContent)
            {
                JsonObject value = tokenizer.GetNextObject();
                result.Add(value.GetValue(elementType));
            }

            return result;
        }

        private static void AppendToCollection(object collection, IEnumerable<object> elements)
        {
            // Dirty but works for now...
            MethodInfo add = collection.GetType().GetMethod("Add");

            foreach (object element in elements)
            {
                add.Invoke(collection, new[] { element });
            }
        }

        private static object BuildCollection(Type arrayType, out Type elementType)
        {
            if (!arrayType.IsConstructedGenericType)
            {
                throw new InvalidOperationException("The construction of non-generic collections is unsupported.");
            }

            elementType = arrayType.GetGenericArguments().Single();

            if (!arrayType.IsInterface)
            {
                return Activator.CreateInstance(arrayType);
            }

            return Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType)); // Hardcore but works for now...
        }
    }
}
