using SpanTests.Core.Tokenization;
using System;

namespace SpanTests.Core.Parsing
{
    internal static class ParsingExtensions
    {
        public static bool IsEmptyOrWhiteSpace(this ReadOnlySpan<char> content)
        {
            return content.IsEmpty || content.IsWhiteSpace();
        }

        public static void TrimStartingSeparatorsAndWhitespaces(this ref ReadOnlySpan<char> content)
        {
            int contentStartsAt = 0;
            for (; contentStartsAt < content.Length; ++contentStartsAt)
            {
                char value = content[contentStartsAt];
                if (!char.IsWhiteSpace(value) && value != JsonToken.ObjectSeparator)
                {
                    break;
                }
            }

            if (contentStartsAt == 0)
            {
                return;
            }

            content = content.Slice(contentStartsAt);
        }

        public static ReadOnlySpan<char> TrimWhitespaces(this ReadOnlySpan<char> content)
        {
            int contentStartsAt = 0;
            for (; contentStartsAt < content.Length; ++contentStartsAt)
            {
                if (!char.IsWhiteSpace(content[contentStartsAt]))
                {
                    break;
                }
            }

            int contentEndsAt = content.Length - 1;
            for (; contentEndsAt > contentStartsAt; --contentEndsAt)
            {
                if (!char.IsWhiteSpace(content[contentEndsAt]))
                {
                    break;
                }
            }

            return content.Slice(contentStartsAt, contentEndsAt - contentStartsAt + 1);
        }

        public static int IndexOf(this ReadOnlySpan<char> content, char value, int startIndex)
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

            content = content.Slice(startsAt, endsAt - startsAt);
            return content.TrimWhitespaces();
        }
    }
}
