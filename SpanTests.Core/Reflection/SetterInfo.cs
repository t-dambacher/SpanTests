using System;

namespace SpanTests.Core.Reflection
{
    internal sealed class SetterInfo
    {
        public Type ExpectedType { get; }
        public Action<object, object> Setter { get; }

        public SetterInfo(Action<object, object> setter, Type expectedType)
        {
            this.ExpectedType = expectedType;
            this.Setter = setter;
        }
    }
}
