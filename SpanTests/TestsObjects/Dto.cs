using System.Collections.Generic;

namespace SpanTests.TestsObjects
{
    internal class Dto
    {
        public string? Value { get; set; }
        public ICollection<IdentifierDto>? Identifiers { get; set; }
    }
}
