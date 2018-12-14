using SpanTests.Core.Parsing;
using System;

namespace SpanTests.Core
{
    /// <summary>
    /// Static class helping parsing json strings
    /// </summary>
    public static class Json
    {
        /// <summary>
        /// Parses a json <see cref="string"/>, thant builds and populates a <typeparamref name="T"/> object
        /// </summary>
        /// <typeparam name="T">The object type to deserialize and populate the json to</typeparam>
        /// <param name="json">The json content</param>
        /// <returns>The deserialization result</returns>
        static public T Deserialize<T>(string json)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException(nameof(json));
            }

            return Parse<T>(json.AsSpan());
        }

        /// <summary>
        /// Parses a json <see cref="string"/>, thant builds and populates a <typeparamref name="T"/> object
        /// </summary>
        /// <typeparam name="T">The object type to deserialize and populate the json to</typeparam>
        /// <param name="json">The json content</param>
        /// <returns>The deserialization result</returns>
        static public T Parse<T>(ReadOnlySpan<char> content)
            where T : class
        {
            if (content.IsEmptyOrWhiteSpace())
            {
                throw new ArgumentException(nameof(content));
            }

            return JsonParser<T>.Parse(content);
        }
    }
}
