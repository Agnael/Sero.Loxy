using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sero.Loxy
{
    public class EventCandidate : IEventCandidate
    {
        public string Message { get; private set; }
        public LogLevel Level { get; private set; }
        public string Category { get; private set; }
        public Exception Exception { get; private set; }

        public EventCandidate(LogLevel lvl, string cat, string msg)
        {
            this.Level = lvl;
            this.Category = cat;
            this.Message = msg;
        }

        public EventCandidate(LogLevel lvl, string cat, string msg, Exception ex)
            : this(lvl, cat, msg)
        {
            this.Exception = ex;
        }

        public EventCandidate(EventCandidate other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            this.Level = other.Level;
            this.Category = other.Category;
            this.Message = other.Message;
            this.Exception = other.Exception;
        }

        public virtual IEnumerable<string> GetDetails()
        {
            return Enumerable.Empty<string>();
        }
    }

    public class EventCandidate<TState> : EventCandidate
    {
        public TState State { get; private set; }
        public Func<TState, Exception, string> DefaultFormatter { get; private set; }

        public EventCandidate(
            LogLevel lvl, 
            string cat, 
            string message,
            TState state,
            Func<TState, Exception, string> stateFormatter)
            : base(
                  lvl,
                  cat,
                  message)
        {
            State = state;
            DefaultFormatter = stateFormatter;
        }

        public EventCandidate(
            LogLevel lvl,
            string cat,
            string message,
            TState state,
            Func<TState, Exception, string> stateFormatter, 
            Exception ex)
            : base(
                  lvl, 
                  cat, 
                  message,
                  ex)
        {
            State = state;
            DefaultFormatter = stateFormatter;
        }

        public EventCandidate(EventCandidate<TState> other)
            : base (other)
        {
            this.State = other.State;
            this.DefaultFormatter = other.DefaultFormatter;
        }

        public override IEnumerable<string> GetDetails()
        {
            return new string[] { DefaultFormatter(this.State, this.Exception) };
        }
    }
}
