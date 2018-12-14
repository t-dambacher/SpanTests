using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SpanTests.Core.ObjectModel
{
    internal static class ReflectionExtensions
    {
        private static readonly IDictionary<Type, IReadOnlyDictionary<string, PropertyInfo>> properties = new Dictionary<Type, IReadOnlyDictionary<string, PropertyInfo>>();
        private static readonly IDictionary<Type, IReadOnlyDictionary<string, FieldInfo>> fields = new Dictionary<Type, IReadOnlyDictionary<string, FieldInfo>>();

        private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        public static (Action<object, object> setter, Type expectedType) GetSetter(this object target, string propertyOrFieldName)
        {
            propertyOrFieldName = propertyOrFieldName.ToLowerInvariant();
            Type targetType = target.GetType();

            PropertyInfo? property = GetProperty(targetType, propertyOrFieldName);
            if (property != null)
            {
                return GetSetterOf(property);
            }

            FieldInfo? field = GetField(targetType, propertyOrFieldName);
            if (field != null)
            {
                return GetSetterOf(field);
            }

            throw new InvalidOperationException($"Unable to find a property or field named '{propertyOrFieldName}' on the type '{targetType.Name}'.");
        }
        
        private static PropertyInfo? GetProperty(Type targetType, string propertyName)
        {
            if (!properties.TryGetValue(targetType, out IReadOnlyDictionary<string, PropertyInfo> targetProperties))
            {
                targetProperties = targetType.GetProperties(Flags).ToDictionary(p => p.Name.ToLowerInvariant());
                properties.Add(targetType, targetProperties);
            }

            return targetProperties.TryGetValue(propertyName, out PropertyInfo result) ? result : null;
        }

        private static FieldInfo? GetField(Type targetType, string fieldName)
        {
            if (!fields.TryGetValue(targetType, out IReadOnlyDictionary<string, FieldInfo> targetFields))
            {
                targetFields = targetType.GetFields(Flags).ToDictionary(p => p.Name.ToLowerInvariant());
                fields.Add(targetType, targetFields);
            }

            return targetFields.TryGetValue(fieldName, out FieldInfo result) ? result : null;
        }

        private static (Action<object, object> setter, Type expectedType) GetSetterOf(PropertyInfo property)
        {
            if (!property.CanWrite)
            {
                throw new InvalidOperationException($"The property '{property.Name}' is readonly.");
            }

            return ((target, value) => property.SetValue(target, value), property.PropertyType);
        }

        private static (Action<object, object> setter, Type expectedType) GetSetterOf(FieldInfo field)
        {
            return ((target, value) => field.SetValue(target, value), field.FieldType);
        }
    }
}
