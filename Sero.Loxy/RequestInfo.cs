using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sero.Core;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Sero.Loxy
{
    public class RequestInfo : IRequestInfo
    {
        public string AppName { get; private set; }
        public string AppVersion { get; private set; }
        public string Environment { get; private set; }
        public string Level { get; private set; }
        public string Verb { get; private set; }
        public string Url { get; private set; }
        public string QueryString { get; private set; }
        public string UserAgent { get; set; }
        public string AcceptLanguage { get; private set; }
        public string CultureCodeSelected { get; private set; }
        public string RequestTraceIdentitfier { get; private set; }
        public DateTime Datetime { get; private set; }
        public long UnixTimestamp { get; private set; }
        public int IdProcess { get; private set; }
        public string LocalIpAddress { get; private set; }
        public string RemoteIpAddress { get; private set; }

        public IEnumerable<IEvent> Events { get; private set; }

        private LogLevel _highestLogLevel;

        public RequestInfo(IApplicationInfoService app,
                            IRequestInfoService req,
                            IEnumerable<IEvent> eventList)
        {
            this.AppName = app.GetAppName();
            this.AppVersion = app.GetAppVersion();
            this.Environment = app.GetAppEnvironment();

            this.Url = req.GetUrl();
            this.QueryString = req.GetQueryString();
            this.Verb = req.GetVerb();

            this.UserAgent = req.GetUserAgentHeader();
            this.AcceptLanguage = req.GetAcceptLanguageHeader();
            this.RequestTraceIdentitfier = req.GetRequestId();

            this.Datetime = req.GetDateUtc();
            this.UnixTimestamp = req.GetUnixTimestamp();
            this.IdProcess = req.GetIdProcess();

            this.LocalIpAddress = req.GetLocalIp();
            this.RemoteIpAddress = req.GetRemoteIp();
            this.Events = eventList;

            this.Level = "UNDEFINED";

            if (this.Events != null && this.Events.Count() > 0)
            {
                _highestLogLevel = this.Events.Max(x => x.GetLogLevel());
                this.Level = _highestLogLevel.ToString();
            }
        }

        public LogLevel GetHighestLevel()
        {
            return _highestLogLevel;
        }
    }
}
