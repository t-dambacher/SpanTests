using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace SpanTests.Core.Reflection
{
    internal static class CtorStore
    {
        private static readonly Dictionary<Type, Func<object>> ctors = new Dictionary<Type, Func<object>>();

        public static object CreateInstance(Type type)
        {
            return GetCtor(type)();
        }

        private static Func<object> GetCtor(Type type)
        {
            Func<object>? ctor = GetCtorFromStore(type);
            if (ctor != null)
            {
                return ctor;
            }

            ctor = GetCtorFromRuntime(type);
            Store(type, ctor);

            return ctor;
        }

        private static void Store(Type type, Func<object> ctor)
        {
            ctors.Add(type, ctor);
        }

        private static Func<object>? GetCtorFromStore(Type type)
        {
            return ctors.TryGetValue(type, out Func<object> result) ? result : null;
        }

        private static Func<object> GetCtorFromRuntime(Type type)
        {
            ConstructorInfo emptyConstructor = type.GetConstructor(Type.EmptyTypes);
            if (type == null)
            {
                throw new InvalidOperationException($"No empty ctor found for type {type.Name}.");
            }

            var dynamicMethod = new DynamicMethod("CreateInstance", type, Type.EmptyTypes, true);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
            ilGenerator.Emit(OpCodes.Ret);
            return (Func<object>)dynamicMethod.CreateDelegate(typeof(Func<object>));
        }
    }
}
