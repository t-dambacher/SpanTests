using SpanTests.Core.ObjectModel;
using System;

namespace SpanTests.Core.Parsing
{
    /// <summary>
    /// Base class for all parsers
    /// </summary>
    internal static class Parser
    {
        #region Static methods

        public static ReadOnlySpan<char> TryParse(ref ReadOnlySpan<char> content, out JsonObjectType type)
        {
            JsonObjectType expectedParserType = JsonObject.GetType(ref content);
            type = expectedParserType;

            switch (expectedParserType)
            {
                case JsonObjectType.Array:
                    return ArrayParser.TryParse(ref content);
                case JsonObjectType.Object:
                    return ObjectParser.TryParse(ref content);
                case JsonObjectType.Primitive:
                    return PrimitiveParser.TryParse(ref content);
                case JsonObjectType.String:
                    return StringParser.TryParse(ref content);
                default:
                    throw new InvalidOperationException("Unsupported object type");
            }
        }

        #endregion
    }
}
