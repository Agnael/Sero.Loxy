using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public interface ILoxyLoggerFormatter
    {
        IEnumerable<string> Format(object state);
    }

    public interface ILoxyLoggerFormatterProvider
    {

    }


    public class ScopedLoggerProxy : ILogger
    {
        private readonly ILoxy _loxy;
        private readonly string _categoryName;
        private readonly IClock _clock;

        public ScopedLoggerProxy(
            ILoxy loxy,
            IClock clock,
            string categoryName)
        {
            _loxy = loxy;
            _categoryName = categoryName;
            _clock = clock;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return state as IDisposable;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            LoggerProxyEventCandidate<TState> evt =
                new LoggerProxyEventCandidate<TState>(
                    logLevel,
                    _categoryName,
                    eventId.Name,
                    state,
                    formatter,
                    exception);

            _loxy.Raise(evt);
        }
    }

    public class LoxyLoggerProxy<TCategoryName>
        : ILogger<TCategoryName>
    {
        private readonly ILoxy _loxy;
        //private readonly ILoxyLoggerFormatter _formatter;
        private readonly string _categoryName;
        private readonly ISystemClock _clock;

        public LoxyLoggerProxy(
            ILoxy loxy,
            ISystemClock clock)
        {
            _loxy = loxy;
            //_formatter = formatter;
            _categoryName = typeof(TCategoryName).FullName;
            _clock = clock;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return state as IDisposable;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            //IStateFormatter<TState> stateFormatter = Options.StateFormatterFactory.Create<TState>(eventId, formatter);

            LoggerProxyEventCandidate<TState> evt =
                new LoggerProxyEventCandidate<TState>(
                    logLevel,
                    _categoryName,
                    eventId.Name,
                    state,
                    formatter,
                    exception);

            _loxy.Raise(evt);
        }
    }
}
