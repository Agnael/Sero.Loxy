using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public class OperatingSystemOverview
    {
        public string Platform { get; set; }
        public string Version { get; set; }
        public string ServicePack { get; set; }
    }

    public class HostOverview
    {
        public string Name { get; private set; }
        public string Ipv4 { get; private set; }
        public string Mac { get; private set; }
        public OperatingSystemOverview OperatingSystem { get; private set; }
        public int IdProcess { get; private set; }
    }

    public class AppOverview
    {
        public string Name { get; private set; }
        public string Version { get; private set; }
        public string Environment { get; private set; }
    }

    public class RequestOverview
    {
        public string HttpMethod { get; private set; }
        public string Url { get; private set; }
        public string Body { get; private set; }
        public string RequesterIpv4 { get; private set; }
        public string TraceIdentitfier { get; private set; }
        public string AcceptLanguageHeaderValue { get; private set; }
    }

    public class SimpleWebLoxyContext
    {
        public string Level { get; private set; }
        public DateTime RaisedDt { get; private set; }

        public HostOverview Host { get; set; }
        public AppOverview App { get; set; }
        public RequestOverview Request { get; set; }
    }
}
