using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JHSoftware;
using System.Collections.ObjectModel;

namespace Shelter.ZoneFileParser.Tests
{
    static class FakeDnsServer
    {
        public const string Test1Hostname = "test1.test";
        public const string Test2Hostname = "test2.test";

        public const string Test1IpAddress = "1.2.3.4";
        public const string Test2IpAddress = "5.6.7.8";

        public enum Servers
        {
            Server1,
            Server2
        }

        private static readonly Dictionary<Servers, List<IRecord>> RecordRepository = new Dictionary<Servers, List<IRecord>>
        {
            {
                Servers.Server1,
                new[]
                {
                    (IRecord)new TestRecord
                    {
                        Type = DnsClient.RecordType.A,
                        Data = Test1IpAddress,
                        Name = Test1Hostname,
                        TTL = 86400
                    },
                    (IRecord)new TestRecord
                    {
                        Type = DnsClient.RecordType.A,
                        Data = Test2IpAddress,
                        Name = Test2Hostname,
                        TTL = 86400
                    },
                    (IRecord)new TestRecord
                    {
                        Type = DnsClient.RecordType.CNAME,
                        Data = Test2Hostname,
                        Name = String.Format("www.{0}", Test1Hostname),
                        TTL = 86400
                    }
                }.ToList()
            },
            {
                Servers.Server2,
                new[]
                {
                    (IRecord)new TestRecord
                    {
                        Type = DnsClient.RecordType.A,
                        Data = Test2IpAddress,
                        Name = Test2Hostname,
                        TTL = 86400
                    },
                    (IRecord)new TestRecord
                    {
                        Type = DnsClient.RecordType.CNAME,
                        Data = Test1Hostname,
                        Name = String.Format("www.{0}", Test1Hostname),
                        TTL = 86400
                    }
                }.ToList()
            }
        };

        internal static ReadOnlyCollection<IRecord> GetRecords(Servers server, string recordName, DnsClient.RecordType type)
        {
            List<IRecord> records;

            return new ReadOnlyCollection<IRecord>(
                RecordRepository.TryGetValue(server, out records)
                    ? records.Where(r => r.Name == recordName && r.Type == type).ToList()
                    : new List<IRecord>());
        }
    }
}
