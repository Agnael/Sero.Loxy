using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public interface IEvent
    {
        string TypeFullName { get; }
        LogLevel Level { get; }
        DateTime CreationDtUtc { get; }
        string Category { get; }
        string Message { get; }
        IEnumerable<ExceptionOverview> Exception { get; }
        IEnumerable<string> Details { get; }
    }
}
