using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SpanTests.Core.Reflection
{
    sealed internal class PropertyStore : ReflectionStore<PropertyInfo, PropertyStore>
    {
        public static SetterInfo? GetPropertySetter(Type targetType, string propertyName)
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

    }
}