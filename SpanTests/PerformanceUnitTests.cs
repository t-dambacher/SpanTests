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
        public void Performance_WholeObjectDeserialization_AreBetterThanNewtonsofts()
        {
            string content = GetTestJson();
            TimeSpan jsonNet = Mesure(json => JsonConvert.DeserializeObject<Dto>(json), content);
            TimeSpan mine = Mesure(json => Json.Deserialize<Dto>(json), content);

            _context.WriteLine($"Mine: {mine.TotalMilliseconds}. Newtonsoft's: {jsonNet.TotalMilliseconds}.");
            Assert.IsTrue(mine < jsonNet, $"Still {(int)(mine.TotalMilliseconds / jsonNet.TotalMilliseconds)} times too long...");
        }

        [TestMethod]
        public void Performance_CollectionDeserialization_AreBetterThanNewtonsofts()
        {
            string content = GetCollectionTestJson();
            TimeSpan jsonNet = Mesure(json => JsonConvert.DeserializeObject<List<IdentifierDto>>(json), content);
            TimeSpan mine = Mesure(json => Json.Deserialize<List<IdentifierDto>>(json), content);

            _context.WriteLine($"Mine: {mine.TotalMilliseconds}. Newtonsoft's: {jsonNet.TotalMilliseconds}.");
            Assert.IsTrue(mine < jsonNet, $"Mine: {mine.TotalMilliseconds}. Newtonsoft's: {jsonNet.TotalMilliseconds}." + $"Still {(int)(mine.TotalMilliseconds / jsonNet.TotalMilliseconds)} times too long...");
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

        private static string GetCollectionTestJson()
        {
            return "["
                + "   { \"id\": 3 }, "
                + "   { \"id\": 5 } "
                + "]";
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
