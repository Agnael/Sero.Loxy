using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public class Event : IEvent
    {
        public string TypeFullName { get; set; }
        public LogLevel Level { get; set; }
        public DateTime CreationDtUtc { get; set; }
        public string Category { get; set; }
        public string Message { get; set; }
        public IEnumerable<ExceptionOverview> Exception { get; set; }
        public IEnumerable<string> Details { get; set; }
    }
}
