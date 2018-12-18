using SpanTests.Core.ObjectModel;
using SpanTests.Core.Parsing;
using SpanTests.Core.Reflection;
using SpanTests.Core.Tokenization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpanTests.Core.Deserialization
{
    /// <summary>
    /// Deserializer for collections
    /// </summary>
    internal sealed class CollectionDeserializer : Deserializer
    {
        private static Dictionary<Type, Type> elementTypes = new Dictionary<Type, Type>();

        /// <summary>
        /// <see cref="IDeserializer.Type"/>
        /// </summary>
        public override JsonObjectType Type => JsonObjectType.Array;

        /// <summary>
        /// <see cref="IDeserializer.Deserialize(ReadOnlySpan{char}, Type)"/>
        /// </summary>
        public override object Deserialize(ReadOnlySpan<char> content, Type arrayType)
        {
            object collection = BuildCollection(arrayType, out Type collectionType, out Type elementType);

            // Append objects to the collection
            if (!content.IsEmptyOrWhiteSpace())
            {
                content = content.TrimWhitespaces();

                AppendToCollection(collection, GetSubElements(content, elementType), collectionType, elementType);
            }

            return collection;
        }

        private static List<object> GetSubElements(ReadOnlySpan<char> content, Type elementType)
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

        private static void AppendToCollection(object collection, List<object> elements, Type collectionType, Type elementType)
        {
            Action<object, object> adder = CollectionMethodStore.GetAddMethod(collectionType, elementType);

            foreach (object element in elements)
            {
                adder(collection, element);
            }
        }

        private static object BuildCollection(Type arrayType, out Type collectionType, out Type elementType)
        {
            if (!arrayType.IsConstructedGenericType)
            {
                throw new InvalidOperationException("The construction of non-generic collections is unsupported.");
            }

            elementType = arrayType.GetGenericArguments().Single();

            if (!arrayType.IsInterface)
            {
                collectionType = arrayType;
            }
            else
            {
                collectionType = GetCollectionType(elementType);
            }

            return Activator.CreateInstance(collectionType);
        }

        private static Type GetCollectionType(Type elementType)
        {
            if (elementTypes.TryGetValue(elementType, out Type result))
            {
                return result;
            }

            result = typeof(List<>).MakeGenericType(elementType); // Hardcore but works for now...
            elementTypes.Add(elementType, result);
            return result;
        }
    }
}
