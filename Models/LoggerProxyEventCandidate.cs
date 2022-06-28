using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public class LoggerProxyEventCandidate<TState> : EventCandidate<TState>
    {
        public LoggerProxyEventCandidate(
            LogLevel lvl,
            string cat,
            string msg,
            TState state,
            Func<TState, Exception, string> formatter)
            : base(lvl, cat, msg, state, formatter)
        {

        }

        public LoggerProxyEventCandidate(
            LogLevel lvl,
            string cat,
            string msg,
            TState state,
            Func<TState, Exception, string> formatter,
            Exception exception)
            : base(lvl, cat, msg, state, formatter, exception)
        {

        }
    }
}
