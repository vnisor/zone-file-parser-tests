using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shelter.ZoneFileParser.Tests
{
    class TestZoneFileEntry : ZoneFileEntry, ICloneable
    {
        public override System.Collections.ObjectModel.ReadOnlyCollection<IRecord> GetRecordsFromDnsServer(string dnsServer)
        {
            return FakeDnsServer.GetRecords((FakeDnsServer.Servers)Enum.Parse(typeof(FakeDnsServer.Servers), dnsServer), Name, Type);
        }

        public void ValidateAgainstServerRecord(FakeDnsServer.Servers server)
        {
            base.ValidateAgainstServerRecord(server.ToString());
        }

        public void ValidateForAgreementBetweenServers(IEnumerable<FakeDnsServer.Servers> servers)
        {
            ValidateForAgreementBetweenServers(servers.Select(s => s.ToString()));
        }

        public object Clone()
        {
            return new TestZoneFileEntry
            {
                Data = this.Data,
                Name = this.Name,
                TTL = this.TTL,
                Type = this.Type
            };
        }
    }
}
