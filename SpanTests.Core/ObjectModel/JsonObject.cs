using SpanTests.Core.Deserialization;
using SpanTests.Core.Parsing;
using SpanTests.Core.Reflection;
using SpanTests.Core.Tokenization;
using System;

namespace SpanTests.Core.ObjectModel
{
    internal ref struct JsonObject
    {
        #region Properties

        public string Name { get; }
        public JsonObjectType Type { get; }
        public ReadOnlySpan<char> Value { get; }

        #endregion

        #region Constructors

        public JsonObject(string name, JsonObjectType type, ReadOnlySpan<char> value)
        {
            this.Name = name;
            this.Type = type;
            this.Value = value;
        }

        #endregion

        #region Methods

        public void SetOn(object target)
        {
            SetterInfo setter = target.GetSetter(this.Name);
            object value = GetValue(setter.ExpectedType);
            setter.Setter(target, value);
        }

        public object GetValue(Type expectedType)
        {
            ReadOnlySpan<char> value = this.Value;
            return Deserializer.Deserialize(this.Type, ref value, expectedType);
        }

        #endregion

        #region Static methods

        public static JsonObjectType GetType(ReadOnlySpan<char> content)
        {
            JsonTokenType tokenType = JsonToken.GetType(content);

            switch (tokenType)
            {
                case JsonTokenType.ArrayStart:
                    return JsonObjectType.Array;
                case JsonTokenType.ObjectStart:
                    return JsonObjectType.Object;
                case JsonTokenType.NameBoundary:
                    return JsonObjectType.String;
                case JsonTokenType.Unknown:
                    if (PrimitiveParser.IsPrimitive(content))
                    {
                        return JsonObjectType.Primitive;
                    }
                    break;
            }

            throw new InvalidOperationException("Unsupported token type.");
        }

        #endregion
    }
}
