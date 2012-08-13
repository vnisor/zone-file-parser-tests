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
                        Data = "1.2.3.4",
                        Name = "test1.test",
                        TTL = 86400
                    },
                    (IRecord)new TestRecord
                    {
                        Type = DnsClient.RecordType.A,
                        Data = "5.6.7.8",
                        Name = "test2.test",
                        TTL = 86400
                    },
                    (IRecord)new TestRecord
                    {
                        Type = DnsClient.RecordType.CNAME,
                        Data = "test2.test",
                        Name = "www.test1.test",
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
                        Data = "5.6.7.8",
                        Name = "test2.test",
                        TTL = 86400
                    },
                    (IRecord)new TestRecord
                    {
                        Type = DnsClient.RecordType.CNAME,
                        Data = "test1.test",
                        Name = "www.test1.test",
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
