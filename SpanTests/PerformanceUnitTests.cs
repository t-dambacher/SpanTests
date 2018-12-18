using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SpanTests.Core;
using SpanTests.TestsObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SpanTests
{
    [TestClass, TestCategory("Performance")]
    public class PerformanceUnitTests
    {
        private static TestContext _context;

        [ClassInitialize]
        public static void SetupTests(TestContext context)
        {
            _context = context;
        }

        [TestMethod]
        public void Performance_WholeObjectDeserialization_IsFasterThanNewtonsofts()
        {
            string content = GetWholeTestObjectJson();
            TimeSpan jsonNet = Mesure(json => JsonConvert.DeserializeObject<Dto>(json), content);
            TimeSpan mine = Mesure(json => Json.Deserialize<Dto>(json), content);

            _context.WriteLine($"Mine: {mine.TotalMilliseconds}. Newtonsoft's: {jsonNet.TotalMilliseconds}.");
            Assert.IsTrue(mine < jsonNet, $"Still {(int)(mine.TotalMilliseconds / jsonNet.TotalMilliseconds)} times too long...");
        }

        [TestMethod]
        public void Performance_CollectionDeserialization_IsFasterThanNewtonsofts()
        {
            string content = GetCollectionObjectJson();
            TimeSpan jsonNet = Mesure(json => JsonConvert.DeserializeObject<List<IdentifierDto>>(json), content);
            TimeSpan mine = Mesure(json => Json.Deserialize<List<IdentifierDto>>(json), content);

            _context.WriteLine($"Mine: {mine.TotalMilliseconds}. Newtonsoft's: {jsonNet.TotalMilliseconds}.");
            Assert.IsTrue(mine < jsonNet, $"Mine: {mine.TotalMilliseconds}. Newtonsoft's: {jsonNet.TotalMilliseconds}." + $"Still {(int)(mine.TotalMilliseconds / jsonNet.TotalMilliseconds)} times too long...");
        }

        [TestMethod]
        public void Performance_SimpleDeserialization_IsFasterThanNewtonsofts()
        {
            string content = GetSimpleObjectJson();
            TimeSpan jsonNet = Mesure(json => JsonConvert.DeserializeObject<IdentifierDto>(json), content);
            TimeSpan mine = Mesure(json => Json.Deserialize<IdentifierDto>(json), content);

            _context.WriteLine($"Mine: {mine.TotalMilliseconds}. Newtonsoft's: {jsonNet.TotalMilliseconds}.");
            Assert.IsTrue(mine < jsonNet, $"Still {(int)(mine.TotalMilliseconds / jsonNet.TotalMilliseconds)} times too long...");
        }

        private TimeSpan Mesure(Action<string> deserializer, string json)
        {
            Stopwatch watch = Stopwatch.StartNew();

            for (int i = 0; i < 100000; ++i)
            {
                deserializer(json);
            }

            watch.Stop();
            return watch.Elapsed;
        }

        private static string GetSimpleObjectJson()
        {
            return "   { \"id\": 3 }";
        }

        private static string GetCollectionObjectJson()
        {
            return "["
                + "   { \"id\": 3 }, "
                + "   { \"id\": 5 } "
                + "]";
        }

        private static string GetWholeTestObjectJson()
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
