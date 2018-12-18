using SpanTests.Core.ObjectModel;
using System;
using System.Linq;
using System.Collections.Generic;

namespace SpanTests.Core.Parsing
{
    /// <summary>
    /// Base class for all parsers
    /// </summary>
    internal abstract class Parser
    {
        #region Properties
        
        /// <summary>
        /// The object type handled by the parser
        /// </summary>
        public abstract JsonObjectType Type { get; }

        #endregion

        #region Instance methods

        /// <summary>
        /// Tries to parse the given input content, returns the value specific to the current type, and updated the <paramref name="content"/> so it can move to the next part
        /// </summary>
        public abstract ReadOnlySpan<char> TryParse(ref ReadOnlySpan<char> content);

        #endregion

        #region Static fields

        private static readonly IReadOnlyDictionary<JsonObjectType, Parser> parsers = new Parser[]
        {
            new ArrayParser(),
            new ObjectParser(),
            new PrimitiveParser(),
            new StringParser()
        }.ToDictionary(p => p.Type);

        #endregion

        #region Static methods

        public static ReadOnlySpan<char> TryParse(ref ReadOnlySpan<char> content, out JsonObjectType type)
        {
            type = JsonObject.GetType(content);
            JsonObjectType expectedParserType = type;

            if (!parsers.TryGetValue(expectedParserType, out Parser parser))
            {
                throw new InvalidOperationException("Unsupported object type");
            }

            return parser.TryParse(ref content);
        }

        #endregion
    }
}
