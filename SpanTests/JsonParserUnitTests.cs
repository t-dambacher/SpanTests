using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SpanTests.Core;
using SpanTests.TestsObjects;
using System;
using System.Diagnostics;
using System.Linq;

namespace SpanTests
{
    [TestClass]
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

        [TestMethod]
        public void JsonParser_Performance_Comparison()
        {
            TimeSpan jsonNet = Mesure(json => JsonConvert.DeserializeObject<Dto>(json));
            TimeSpan mine = Mesure(json => Json.Deserialize<Dto>(json));

            Assert.IsTrue(mine < jsonNet, $"Mine: {mine.TotalMilliseconds}. Newtonsoft's: {jsonNet.TotalMilliseconds}. Still {(int)(mine.TotalMilliseconds / jsonNet.TotalMilliseconds)} times too long...");
        }

        private TimeSpan Mesure(Action<string> deserializer)
        {
            string json = GetTestJson();
            Stopwatch watch = Stopwatch.StartNew();

            for (int i = 0; i < 100000; ++i)
            {
                deserializer(json);
            }

            watch.Stop();
            return watch.Elapsed;
        }

        private static string GetTestJson()
        {
            return  "{"
                + "\"value\": \"TestValue\","
                + "\"identifiers\": ["
                + "   { \"id\": 3 }, "
                + "   { \"id\": 5 } "
                + "]"
            + "}";
        }
    }
}
