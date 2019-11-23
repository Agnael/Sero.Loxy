using Microsoft.Extensions.Logging;
using Sero.Loxy.Abstractions;
using Sero.Loxy.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public class LoggerProxy : ILogger
    {
        private readonly ILoxy _loxy;
        public readonly LoggerProxyOptions Options;

        public LoggerProxy(ILoxy eventLogger)
        {
            _loxy = eventLogger;
            Options = new LoggerProxyOptions();
        }

        public LoggerProxy(ILoxy eventLogger, LoggerProxyOptions options)
        {
            _loxy = eventLogger;
            Options = options;
        }

        public LoggerProxy(ILoxy eventLogger, Action<LoggerProxyOptions> config)
        {
            LoggerProxyOptions defaultOptions = new LoggerProxyOptions();
            config(defaultOptions);

            Options = defaultOptions;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            var dummy = state as IDisposable;
            return dummy;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            // Siempre permite TODOS los eventos. Este ILogger solo es un facade porque EFCore fuerza usar esto 
            // pero el que decide realmente qué se escribe y qué no es el ILoxy.
            return true;
        }

        public delegate IEnumerable<string> StateFormatter<TState>(TState state, Func<TState, Exception, string> formatter);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            ProxiedEvent<TState> evt;

            if (Options.StateFormatterOverride != null)
                evt = new ProxiedEvent<TState>(logLevel, Options.Category, eventId.Name, state, Options.StateFormatterOverride);
            else
                evt = new ProxiedEvent<TState>(logLevel, Options.Category, eventId.Name, state, formatter);

            _loxy.RaiseAsync(evt).Wait();
        }
    }
}
