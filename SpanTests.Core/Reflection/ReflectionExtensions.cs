using System;

namespace SpanTests.Core.Reflection
{
    internal static class ReflectionExtensions
    {
        public static SetterInfo GetSetter(this object target, string propertyOrFieldName)
        {
            propertyOrFieldName = propertyOrFieldName.ToLowerInvariant();
            Type targetType = target.GetType();

            var result = PropertyStore.GetPropertySetter(targetType, propertyOrFieldName) ?? FieldStore.GetFieldSetter(targetType, propertyOrFieldName);
            return result ?? throw new InvalidOperationException($"Unable to find a property or field named '{propertyOrFieldName}' on the type '{targetType.Name}'.");
        }
    }
}
