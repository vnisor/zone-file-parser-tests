using Shelter.ZoneFileParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Shelter.ZoneFileParser.Tests
{
    [TestClass()]
    public class ParserTest
    {
        private static readonly string[] Collection = new[] { "A", "B", "C" };

        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod()]
        public void ParserConstructorTest1()
        {
            var input = "A\nB\nC";

            var parser = new Parser(input);
            Assert.AreEqual(3, parser.InputLines.Length);

            CollectionAssert.AreEquivalent(Collection, parser.InputLines);
        }

        [TestMethod]
        public void ParserConstructorTest2()
        {
            var input = "A\r\nB\r\nC";

            var parser = new Parser(input);
            Assert.AreEqual(3, parser.InputLines.Length);

            CollectionAssert.AreEquivalent(Collection, parser.InputLines);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParserConstructorTest3()
        {
            new Parser(String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParserConstructorTest4()
        {
            string input = null;
            new Parser(input);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParserConstructorTest5()
        {
            new Parser(" ");
        }

        [TestMethod]
        public void ParseTest1()
        {
            var input = String.Format("{0}      86400 IN A      {1}", FakeDnsServer.Test1Hostname, FakeDnsServer.Test1IpAddress);
            var parser = new Parser(input);
            var singleRecord = parser.Parse().Single();

            Assert.AreEqual(singleRecord.Name, FakeDnsServer.Test1Hostname);
            Assert.AreEqual(singleRecord.Data, FakeDnsServer.Test1IpAddress);
            Assert.AreEqual(86400, singleRecord.TTL);
        }

        [TestMethod]
        public void ParseTest2()
        {
            var input = String.Format("{0}      86400 IN A      {1}\r\n{2}      86400 IN A      {3}", // Windows line break
                FakeDnsServer.Test1Hostname,
                FakeDnsServer.Test1IpAddress,
                FakeDnsServer.Test2Hostname,
                FakeDnsServer.Test2IpAddress);

            var parser = new Parser(input);
            var records = parser.Parse();

            Assert.AreEqual(records[0].Name, FakeDnsServer.Test1Hostname);
            Assert.AreEqual(records[0].Data, FakeDnsServer.Test1IpAddress);
            Assert.AreEqual(86400, records[0].TTL);

            Assert.AreEqual(records[1].Name, FakeDnsServer.Test2Hostname);
            Assert.AreEqual(records[1].Data, FakeDnsServer.Test2IpAddress);
            Assert.AreEqual(86400, records[1].TTL);
        }

        [TestMethod]
        public void ParseTest3()
        {
            var input = String.Format("{0}      86400 IN A      {1}\n{2}      86400 IN A      {3}", // *nix line break
                FakeDnsServer.Test1Hostname,
                FakeDnsServer.Test1IpAddress,
                FakeDnsServer.Test2Hostname,
                FakeDnsServer.Test2IpAddress);

            var parser = new Parser(input);
            var records = parser.Parse();

            Assert.AreEqual(records[0].Name, FakeDnsServer.Test1Hostname);
            Assert.AreEqual(records[0].Data, FakeDnsServer.Test1IpAddress);
            Assert.AreEqual(86400, records[0].TTL);

            Assert.AreEqual(records[1].Name, FakeDnsServer.Test2Hostname);
            Assert.AreEqual(records[1].Data, FakeDnsServer.Test2IpAddress);
            Assert.AreEqual(86400, records[1].TTL);
        }

        [TestMethod]
        public void ParseTest4()
        {
            var input = String.Format("{0}      86400 IN A      {1}\n{2}      86400 IN A      {3}\n", // trailing *nix line break
                FakeDnsServer.Test1Hostname,
                FakeDnsServer.Test1IpAddress,
                FakeDnsServer.Test2Hostname,
                FakeDnsServer.Test2IpAddress);

            var parser = new Parser(input);
            var records = parser.Parse();

            Assert.AreEqual(records[0].Name, FakeDnsServer.Test1Hostname);
            Assert.AreEqual(records[0].Data, FakeDnsServer.Test1IpAddress);
            Assert.AreEqual(86400, records[0].TTL);

            Assert.AreEqual(records[1].Name, FakeDnsServer.Test2Hostname);
            Assert.AreEqual(records[1].Data, FakeDnsServer.Test2IpAddress);
            Assert.AreEqual(86400, records[1].TTL);
        }

        [TestMethod]
        public void ParseTest5()
        {
            var input = String.Format("{0}      86400 IN A      {1}\n{2}      86400 IN A      {3}\r\n", // trailing Windows line break
                FakeDnsServer.Test1Hostname,
                FakeDnsServer.Test1IpAddress,
                FakeDnsServer.Test2Hostname,
                FakeDnsServer.Test2IpAddress);

            var parser = new Parser(input);
            var records = parser.Parse();

            Assert.AreEqual(records[0].Name, FakeDnsServer.Test1Hostname);
            Assert.AreEqual(records[0].Data, FakeDnsServer.Test1IpAddress);
            Assert.AreEqual(86400, records[0].TTL);

            Assert.AreEqual(records[1].Name, FakeDnsServer.Test2Hostname);
            Assert.AreEqual(records[1].Data, FakeDnsServer.Test2IpAddress);
            Assert.AreEqual(86400, records[1].TTL);
        }
    }
}
