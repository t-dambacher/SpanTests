using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace SpanTests.Core.Reflection
{
    internal abstract class ReflectionStore<TMemberInfo, TStore>
        where TMemberInfo : MemberInfo
        where TStore : ReflectionStore<TMemberInfo, TStore>, new()
    {
        private static readonly Type objectType = typeof(object);
        protected static TStore Instance { get; } = new TStore();
        private static readonly Dictionary<Type, Dictionary<string, SetterInfo>> setters = new Dictionary<Type, Dictionary<string, SetterInfo>>();
        protected const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.IgnoreCase;

        protected private static SetterInfo? TryGetSetterFromStore(Type targetType, string memberName)
        {
            if (setters.TryGetValue(targetType, out Dictionary<string, SetterInfo> targetTypeMembers) && targetTypeMembers.TryGetValue(memberName, out SetterInfo result))
            {
                return result;
            }

            return null;
        }

        protected private static void Store(string memberName, Type targetType, SetterInfo result)
        {
            if (!setters.TryGetValue(targetType, out Dictionary<string, SetterInfo> targetTypeMembers))
            {
                targetTypeMembers = new Dictionary<string, SetterInfo>();
                setters.Add(targetType, targetTypeMembers);
            }

            targetTypeMembers.Add(memberName, result);
        }

        protected SetterInfo? GetSetter(Type targetType, string memberName)
        {
            SetterInfo? result = TryGetSetterFromStore(targetType, memberName);
            if (result != null)
            {
                return result;
            }

            result = TryGetSetterFromRuntime(targetType, memberName);
            if (result != null)
            {
                Store(memberName, targetType, result);
            }

            return result;
        }

        private SetterInfo? TryGetSetterFromRuntime(Type targetType, string memberName)
        {
            TMemberInfo? member = TryGetMember(targetType, memberName);
            if (member == null)
            {
                return null;
            }

            /* Trying to automate the following lambda expression
                (target, value) => ((targetType)target).MyProperty  = (propertyType)value; */

            Type memberType = GetMemberType(member);

            ParameterExpression targetParam = Expression.Parameter(objectType, "target");
            ParameterExpression valueParam = Expression.Parameter(objectType, "value");

            UnaryExpression convertedTarget = Expression.Convert(targetParam, targetType);
            MemberExpression getter = GetMemberExpression(convertedTarget, member);
                
            var setter = Expression.Assign(getter, Expression.Convert(valueParam, memberType));
            return new SetterInfo(Expression.Lambda<Action<object, object>>(setter, targetParam, valueParam).Compile(), memberType);
        }

        protected abstract TMemberInfo? TryGetMember(Type targetType, string memberName);
        protected abstract Type GetMemberType(TMemberInfo member);

        protected abstract MemberExpression GetMemberExpression(Expression target, TMemberInfo member);
    }
}
