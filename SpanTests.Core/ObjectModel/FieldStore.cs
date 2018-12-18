using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace SpanTests.Core.ObjectModel
{
    sealed internal class FieldStore : ReflectionStore<FieldInfo, FieldStore>
    {
        public static (Action<object, object> setter, Type expectedType)? GetFieldSetter(Type targetType, string propertyName)
        {
            return Instance.GetSetter(targetType, propertyName);
        }

        protected override FieldInfo? TryGetMember(Type targetType, string memberName)
        {
            return targetType.GetField(memberName, Flags);
        }
        protected override Type GetMemberType(FieldInfo member)
        {
            return member.FieldType;
        }

        protected override MemberExpression GetMemberExpression(Expression target, FieldInfo member)
        {
            return Expression.Field(
                target,
                member
            );
        }
    }
}
