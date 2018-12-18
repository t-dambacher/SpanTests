using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace SpanTests.Core.ObjectModel
{
    sealed internal class PropertyStore : ReflectionStore<PropertyInfo, PropertyStore>
    {
        public static (Action<object, object> setter, Type expectedType)? GetPropertySetter(Type targetType, string propertyName)
        {
            return Instance.GetSetter(targetType, propertyName);
        }

        protected override PropertyInfo? TryGetMember(Type targetType, string memberName)
        {
            return targetType.GetProperty(memberName, Flags);
        }

        protected override Type GetMemberType(PropertyInfo member)
        {
            return member.PropertyType;
        }

        protected override MemberExpression GetMemberExpression(Expression target, PropertyInfo member)
        {
            return Expression.Property(
                target,
                member
            );
        }

        private static (Action<object, object> setter, Type expectedType)? TryGetPropertySetterFromRuntime(Type targetType, string propertyName)
        {
            PropertyInfo? property = targetType.GetProperty(propertyName, Flags);
            if (property == null)
            {
                return null;
            }

            /* Trying to automate the following lambda expression
                (target, value) => ((targetType)target).MyProperty  = (propertyType)value; */

            Type propertyType = property.PropertyType;

            ParameterExpression targetParam = Expression.Parameter(typeof(object), "target");
            ParameterExpression valueParam = Expression.Parameter(typeof(object), "value");

            MemberExpression getter = Expression.Property(
                Expression.Convert(targetParam, targetType),
                property
            );

            var setter = Expression.Assign(getter, Expression.Convert(valueParam, propertyType));
            return (Expression.Lambda<Action<object, object>>(setter, targetParam, valueParam).Compile(), propertyType);
        }
    }
}
