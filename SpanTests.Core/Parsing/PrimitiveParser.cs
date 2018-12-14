using System;
using SpanTests.Core.ObjectModel;

namespace SpanTests.Core.Parsing
{
    internal sealed class PrimitiveParser : Parser
    {
        #region Properties

        /// <summary>
        /// <see cref="Parser.Type"/>
        /// </summary>
        public override JsonObjectType Type => JsonObjectType.Primitive;

        #endregion

        #region Instance methods

        /// <summary>
        /// <see cref="Parser.TryParse(ref ReadOnlySpan{char})"/>
        /// </summary>
        public override ReadOnlySpan<char> TryParse(ref ReadOnlySpan<char> content)
        {
            int primitiveEndIndex = 0;
            for (; primitiveEndIndex < content.Length; ++primitiveEndIndex)
            {
                if (!IsPrimitive(content[primitiveEndIndex]))
                {
                    break;
                }
            }

            ReadOnlySpan<char> result = content.Slice(0, primitiveEndIndex);
            content = content.Slice(primitiveEndIndex);

            return result;
        }

        #endregion

        #region Static methods

        public static bool IsPrimitive(ReadOnlySpan<char> content)
        {
            return IsPrimitive(content[0]);
        }

        public static bool IsPrimitive(char value)
        {
            return char.IsLetterOrDigit(value) || char.IsSurrogate(value);
        }

        #endregion
    }
}
