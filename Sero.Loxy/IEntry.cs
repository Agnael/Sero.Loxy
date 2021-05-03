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
    public abstract class Entry
    {
        public string Level { get; private set; }
        private LogLevel _highestLogLevel;

        private object _internalContext;
        public IEnumerable<IEvent> Events { get; private set; }

        public Entry(object context, IEnumerable<IEvent> events)
        {
            this.Level = "UNDEFINED";
            _internalContext = context;
            this.Events = events;

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

    public class Entry<TContext> : Entry
    {
        public Entry(
            TContext context,
            IEnumerable<IEvent> eventList)
            : base (
                  context, 
                  eventList)
        {
        }
    }
}
