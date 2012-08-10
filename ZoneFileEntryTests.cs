using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shelter.ZoneFileParser.Tests
{
    [TestClass]
    public class ZoneFileEntryTests
    {
        private TestZoneFileEntry TestEntry1 = new TestZoneFileEntry
        {
            Data = "1.2.3.4",
            Name = "test1.test",
            TTL = 86400,
            Type = JHSoftware.DnsClient.RecordType.A
        };

        private TestZoneFileEntry TestEntry2 = new TestZoneFileEntry
        {
            Data = "5.6.7.8",
            Name = "test2.test",
            TTL = 86400,
            Type = JHSoftware.DnsClient.RecordType.A
        };

        private TestZoneFileEntry TestEntry3 = new TestZoneFileEntry
        {
            Data = "9.10.11.12",
            Name = "test3.test",
            TTL = 86400,
            Type = JHSoftware.DnsClient.RecordType.A
        };

        [TestMethod]
        public void Test1()
        {
            var result = FakeDnsServer.GetRecords(FakeDnsServer.Servers.Server1, "test1.test", JHSoftware.DnsClient.RecordType.A).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("1.2.3.4", result[0].Data);
        }

        [TestMethod]
        public void ValidateAgainstServerRecordTest_Simple1()
        {
            TestEntry1.ValidateAgainstServerRecord(FakeDnsServer.Servers.Server1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidZoneFileEntryException))]
        public void ValidateAgainstServerRecordTest_Simple2()
        {
            TestEntry3.ValidateAgainstServerRecord(FakeDnsServer.Servers.Server1);
        }

        [TestMethod]
        public void ValidateForAgreementBetweenServersTest1()
        {
            TestEntry2.ValidateForAgreementBetweenServers(new[] { FakeDnsServer.Servers.Server1, FakeDnsServer.Servers.Server2 });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidZoneFileEntryException))]
        public void ValidateForAgreementBetweenServersTest2()
        {
            TestEntry1.ValidateForAgreementBetweenServers(new[] { FakeDnsServer.Servers.Server1, FakeDnsServer.Servers.Server2 });
        }
    }
}
