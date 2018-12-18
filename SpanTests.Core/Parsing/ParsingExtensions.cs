using SpanTests.Core.Tokenization;
using System;

namespace SpanTests.Core.Parsing
{
    internal static class ParsingExtensions
    {
        public static int GetContentStartIndex(this ReadOnlySpan<char> content, int startIndex = 0)
        {
            int contentStartsAt = startIndex;
            for (; contentStartsAt < content.Length; ++contentStartsAt)
            {
                char value = content[contentStartsAt];
                if (!char.IsWhiteSpace(value) && value != JsonToken.ObjectSeparator)
                {
                    break;
                }
            }

            return contentStartsAt;
        }

        public static void TrimWhitespaces(this ref ReadOnlySpan<char> content, int startAt = 0)
        {
            content.GetWhitespacesIndexes(out int startIndex, out int endIndex, startAt);
            if (startIndex != startAt || endIndex != content.Length - 1)
            {
                content = content.Slice(startIndex, endIndex - startIndex + 1); 
            }
        }

        public static void GetWhitespacesIndexes(this ref ReadOnlySpan<char> content, out int startIndex, out int endIndex, int start = 0, int? end = null)
        {
            int contentStartsAt = start;
            for (; contentStartsAt < content.Length; ++contentStartsAt)
            {
                if (!char.IsWhiteSpace(content[contentStartsAt]))
                {
                    break;
                }
            }

            int contentEndsAt = end ?? content.Length - 1;
            for (; contentEndsAt > contentStartsAt; --contentEndsAt)
            {
                if (!char.IsWhiteSpace(content[contentEndsAt]))
                {
                    break;
                }
            }

            startIndex = contentStartsAt;
            endIndex = contentEndsAt;
        }

        public static int IndexOf(this ref ReadOnlySpan<char> content, char value, int startIndex)
        {
            for (int i = startIndex; i < content.Length; ++i)
            {
                if (content[i] == value)
                {
                    return i;
                }
            }

            return -1;
        }

        public static ReadOnlySpan<char> GetBoundaries(this ReadOnlySpan<char> content, char start, char end)
        {
            int startsAt = content.IndexOf(start) + 1;
            if (startsAt <= 0)
            {
                throw new ArgumentException(nameof(content));
            }

            int endsAt = content.LastIndexOf(end);
            if (endsAt < 0)
            {
                throw new ArgumentException(nameof(content));
            }

            content.GetWhitespacesIndexes(out int startIndex, out int endIndex, start: startsAt, end: endsAt);
            content = content.Slice(startIndex, endIndex - startIndex);

            return content;
        }
    }
}
