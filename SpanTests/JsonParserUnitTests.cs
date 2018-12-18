using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpanTests.Core;
using SpanTests.TestsObjects;
using System.Linq;

namespace SpanTests
{
    [TestClass, TestCategory("Unit Tests")]
    public class JsonParserUnitTests
    {
        [TestMethod]
        public void JsonParser_Deserialize_Succeeds()
        {
            string json = GetTestJson();

            var dto = Json.Deserialize<Dto>(json);

            Assert.IsNotNull(dto, $"{nameof(dto)} is null");

            Assert.IsNotNull(dto.Value, $"{nameof(dto.Value)} is null");
            Assert.AreEqual("TestValue", dto.Value);

            Assert.IsNotNull(dto.Identifiers, $"{nameof(dto.Identifiers)} is null");
            Assert.AreEqual(2, dto.Identifiers?.Count ?? 0);

            IdentifierDto identifier = dto.Identifiers.First();
            Assert.IsNotNull(identifier, $"{nameof(identifier)} is null");
            Assert.AreEqual(3, identifier.Id);

            identifier = dto.Identifiers.Last();
            Assert.IsNotNull(identifier, $"{nameof(identifier)} is null");
            Assert.AreEqual(5, identifier.Id);
        }

        private static string GetTestJson()
        {
            return "{"
                + "\"value\": \"TestValue\","
                + "\"identifiers\": ["
                + "   { \"id\": 3 }, "
                + "   { \"id\": 5 } "
                + "]"
            + "}";
        }
    }
}
