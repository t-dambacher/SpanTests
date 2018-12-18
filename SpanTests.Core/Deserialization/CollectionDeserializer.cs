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
    internal static class CollectionDeserializer
    {
        private static readonly Dictionary<Type, Type> elementTypes = new Dictionary<Type, Type>();

        public static object Deserialize(ref ReadOnlySpan<char> content, Type arrayType)
        {
            object collection = BuildCollection(arrayType, out Type collectionType, out Type elementType);

            // Append objects to the collection
            if (!content.IsEmptyOrWhiteSpace())
            {
                content.TrimWhitespaces();

                AppendToCollection(collection, ref content, collectionType, elementType);
            }

            return collection;
        }

        private static void AppendToCollection(object collection, ref ReadOnlySpan<char> content, Type collectionType, Type elementType)
        {
            Action<object, object> adder = CollectionMethodStore.GetAddMethod(collectionType, elementType);
            var tokenizer = new JsonArrayTokenizer(content);

            while (tokenizer.HasContent)
            {
                JsonObject value = tokenizer.GetNextObject();
                object subValue = value.GetValue(elementType);
                adder(collection, subValue);
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

            return CtorStore.CreateInstance(collectionType);
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
