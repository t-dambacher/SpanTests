using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SpanTests.Core.Deserialization
{
    static internal class CollectionMethodStore
    {
        private static readonly Dictionary<(Type, Type), Action<object, object>> adders = new Dictionary<(Type, Type), Action<object, object>>();

        public static Action<object, object> GetAddMethod(Type collectionType, Type elementType)
        {
            (Type, Type) key = (collectionType, elementType);

            Action<object, object>? result = TryGetAddMethodFromStore(key);
            if (result != null)
            {
                return result;
            }

            result = GetAddMethodFromRuntime(collectionType, elementType);
            Store(key, result);

            return result;
        }

        private static Action<object, object> GetAddMethodFromRuntime(Type collectionType, Type elementType)
        {
            Type objectType = typeof(object);

            // Dirty but works for now...
            MethodInfo addMethod = collectionType.GetMethod("Add");

            ParameterExpression collectionParam = Expression.Parameter(objectType, "collection");
            ParameterExpression elementParam = Expression.Parameter(objectType, "element");

            Expression convertedCollection = Expression.Convert(collectionParam, collectionType);
            Expression convertedElement = Expression.Convert(elementParam, elementType);

            Expression adder = Expression.Call(convertedCollection, addMethod, convertedElement);

            return Expression.Lambda<Action<object, object>>(adder, collectionParam, elementParam).Compile();
        }

        private static void Store((Type, Type) key, Action<object, object> adder)
        {
            adders.Add(key, adder);
        }

        private static Action<object, object>? TryGetAddMethodFromStore((Type, Type) key)
        {
            if (adders.TryGetValue(key, out Action<object, object> result))
            {
                return result;
            }

            return null;
        }
    }
}
