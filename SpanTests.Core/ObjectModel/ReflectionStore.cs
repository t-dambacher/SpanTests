using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace SpanTests.Core.ObjectModel
{
    internal abstract class ReflectionStore<TMemberInfo, TStore>
        where TMemberInfo : MemberInfo
        where TStore : ReflectionStore<TMemberInfo, TStore>, new()
    {
        protected static TStore Instance { get; } = new TStore();
        private static readonly Dictionary<Type, Dictionary<string, (Action<object, object> setter, Type expectedType)>> setters = new Dictionary<Type, Dictionary<string, (Action<object, object> setter, Type expectedType)>>();
        protected const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.IgnoreCase;

        protected private static (Action<object, object> setter, Type expectedType)? TryGetSetterFromStore(Type targetType, string memberName)
        {
            if (setters.TryGetValue(targetType, out Dictionary<string, (Action<object, object> setter, Type expectedType)> targetTypeMembers) && targetTypeMembers.TryGetValue(memberName, out (Action<object, object> setter, Type expectedType) result))
            {
                return result;
            }

            return null;
        }

        protected private static void Store(string memberName, Type targetType, (Action<object, object> setter, Type expectedType) result)
        {
            if (!setters.TryGetValue(targetType, out Dictionary<string, (Action<object, object> setter, Type expectedType)> targetTypeMembers))
            {
                targetTypeMembers = new Dictionary<string, (Action<object, object> setter, Type expectedType)>();
                setters.Add(targetType, targetTypeMembers);
            }

            targetTypeMembers.Add(memberName, result);
        }

        protected (Action<object, object> setter, Type expectedType)? GetSetter(Type targetType, string memberName)
        {
            (Action<object, object> setter, Type expectedType)? result = TryGetSetterFromStore(targetType, memberName);
            if (result != null)
            {
                return result;
            }

            result = TryGetSetterFromRuntime(targetType, memberName);
            if (result != null)
            {
                Store(memberName, targetType, result.Value);
            }

            return result;
        }

        private (Action<object, object> setter, Type expectedType)? TryGetSetterFromRuntime(Type targetType, string memberName)
        {
            TMemberInfo? member = TryGetMember(targetType, memberName);
            if (member == null)
            {
                return null;
            }

            /* Trying to automate the following lambda expression
                (target, value) => ((targetType)target).MyProperty  = (propertyType)value; */

            Type memberType = GetMemberType(member);

            ParameterExpression targetParam = Expression.Parameter(typeof(object), "target");
            ParameterExpression valueParam = Expression.Parameter(typeof(object), "value");

            UnaryExpression convertedTarget = Expression.Convert(targetParam, targetType);
            MemberExpression getter = GetMemberExpression(convertedTarget, member);
                
            var setter = Expression.Assign(getter, Expression.Convert(valueParam, memberType));
            return (Expression.Lambda<Action<object, object>>(setter, targetParam, valueParam).Compile(), memberType);
        }

        protected abstract TMemberInfo? TryGetMember(Type targetType, string memberName);
        protected abstract Type GetMemberType(TMemberInfo member);

        protected abstract MemberExpression GetMemberExpression(Expression target, TMemberInfo member);
    }
}
