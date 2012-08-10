using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JHSoftware;

namespace Shelter.ZoneFileParser.Tests
{
    class TestRecord : IRecord
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public int TTL { get; set; }
        public DnsClient.RecordType Type { get; set; }
    }
}
