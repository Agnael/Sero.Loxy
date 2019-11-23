using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public delegate IEnumerable<string> CustomStateFormatter<TState>(TState state, Func<TState, Exception, string> formatter);

    public class LoggerProxyOptions
    {
        public string Category { get; private set; }

        /// <summary>
        /// Useful to track the relation between logs when there are multiple similar clients performing similar tasks (i.e.: Multiple DbContexts performing queries).
        /// </summary>
        public Guid? LoggerProviderId { get; private set; }

        public IExceptionFormatter ExceptionFormatter { get; private set; }

        public IStateFormatter StateFormatterOverride { get; private set; }

        public LoggerProxyOptions()
        {
            Category = "UNCATEGORIZED";
        }

        public LoggerProxyOptions(Guid loggerProviderId)
        {
            Category = "UNCATEGORIZED";
            LoggerProviderId = LoggerProviderId;
        }

        public LoggerProxyOptions WithLoggerProviderId(Guid loggerProviderId)
        {
            if(LoggerProviderId.HasValue)
                return this;

            LoggerProviderId = loggerProviderId;
            return this;
        }

        public LoggerProxyOptions WithCategory(string category)
        {
            Category = category;
            return this;
        }

        public LoggerProxyOptions WithExceptionFormatter(IExceptionFormatter customExceptionFormatter)
        {
            ExceptionFormatter = customExceptionFormatter;
            return this;
        }

        public LoggerProxyOptions WithStateFormatter(IStateFormatter customStateFormatter)
        {
            StateFormatterOverride = customStateFormatter;
            return this;
        }
    }
}
