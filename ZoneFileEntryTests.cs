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
        private static readonly TestZoneFileEntry TestEntry1 = new TestZoneFileEntry
        {
            Data = FakeDnsServer.Test1IpAddress,
            Name = FakeDnsServer.Test1Hostname,
            TTL = 86400,
            Type = JHSoftware.DnsClient.RecordType.A
        };

        private static readonly TestZoneFileEntry TestEntry2 = new TestZoneFileEntry
        {
            Data = FakeDnsServer.Test2IpAddress,
            Name = FakeDnsServer.Test2Hostname,
            TTL = 86400,
            Type = JHSoftware.DnsClient.RecordType.A
        };

        private static readonly TestZoneFileEntry TestEntry3 = new TestZoneFileEntry
        {
            Data = "9.10.11.12",
            Name = "test3.test",
            TTL = 86400,
            Type = JHSoftware.DnsClient.RecordType.A
        };

        private static readonly TestZoneFileEntry TestEntry4 = new TestZoneFileEntry
        {
            Data = FakeDnsServer.Test1Hostname,
            Name = String.Format("www.{0}", FakeDnsServer.Test1Hostname),
            TTL = 86400,
            Type = JHSoftware.DnsClient.RecordType.CNAME
        };

        private static readonly FakeDnsServer.Servers[] AllServers = new[]
        {
            FakeDnsServer.Servers.Server1, FakeDnsServer.Servers.Server2
        };

        [TestMethod]
        public void GetRecordTest1()
        {
            var result = FakeDnsServer.GetRecords(FakeDnsServer.Servers.Server1, FakeDnsServer.Test1Hostname, JHSoftware.DnsClient.RecordType.A).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(FakeDnsServer.Test1IpAddress, result[0].Data);
            Assert.AreEqual(86400, result[0].TTL);
            Assert.AreEqual(JHSoftware.DnsClient.RecordType.A, result[0].Type);
        }

        [TestMethod]
        public void GetRecordTest2()
        {
            var result = FakeDnsServer.GetRecords(FakeDnsServer.Servers.Server2, FakeDnsServer.Test1Hostname, JHSoftware.DnsClient.RecordType.A).ToList();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ValidateAgainstServerRecordTest1()
        {
            TestEntry1.ValidateAgainstServerRecord(FakeDnsServer.Servers.Server1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidZoneFileEntryException))]
        public void ValidateAgainstServerRecordTest2()
        {
            TestEntry3.ValidateAgainstServerRecord(FakeDnsServer.Servers.Server1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidZoneFileEntryException))]
        public void ValidateAgainstServerRecordTest3()
        {
            var clone = (TestZoneFileEntry)TestEntry1.Clone();

            clone.TTL -= 1;
            clone.ValidateAgainstServerRecord(FakeDnsServer.Servers.Server1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidZoneFileEntryException))]
        public void ValidateAgainstServerRecordTest4()
        {
            var clone = (TestZoneFileEntry)TestEntry1.Clone();

            clone.Data += "1";
            clone.ValidateAgainstServerRecord(FakeDnsServer.Servers.Server1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidZoneFileEntryException))]
        public void ValidateAgainstServerRecordTest5()
        {
            var clone = (TestZoneFileEntry)TestEntry1.Clone();

            clone.Name += "a";
            clone.ValidateAgainstServerRecord(FakeDnsServer.Servers.Server1);
        }

        [TestMethod]
        public void ValidateForAgreementBetweenServersTest1()
        {
            TestEntry2.ValidateForAgreementBetweenServers(AllServers);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidZoneFileEntryException))]
        public void ValidateForAgreementBetweenServersTest2()
        {
            TestEntry1.ValidateForAgreementBetweenServers(AllServers);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidZoneFileEntryException))]
        public void ValidateForAgreementBetweenServersTest3()
        {
            var clone = (TestZoneFileEntry)TestEntry1.Clone();

            clone.TTL -= 1;
            clone.ValidateForAgreementBetweenServers(AllServers);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidZoneFileEntryException))]
        public void ValidateForAgreementBetweenServersTest4()
        {
            var clone = (TestZoneFileEntry)TestEntry1.Clone();

            clone.Data += "1";
            clone.ValidateForAgreementBetweenServers(AllServers);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidZoneFileEntryException))]
        public void ValidateForAgreementBetweenServersTest5()
        {
            TestEntry4.ValidateForAgreementBetweenServers(AllServers);
        }
    }
}
