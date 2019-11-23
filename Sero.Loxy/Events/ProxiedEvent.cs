using Microsoft.Extensions.Logging;
using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.Events
{
    public class ProxiedEvent<TState> : AbstractEvent<TState>
    {
        Func<TState, Exception, string> _stateFormatter;
        IStateFormatter _customStateFormatter;

        public ProxiedEvent(LogLevel level, 
                            string category, 
                            string message, 
                            TState state, 
                            Func<TState, Exception, string> stateFormatter) 
            : base(level, category, message, state)
        {
            if (stateFormatter == null) throw new ArgumentNullException("stateFormatter");
            _stateFormatter = stateFormatter;
        }

        public ProxiedEvent(LogLevel level, 
                            string category, 
                            string message, 
                            TState state, 
                            IStateFormatter customStateFormatter)
            : base(level, category, message, state)
        {
            if (customStateFormatter == null) throw new ArgumentNullException("customStateFormatter");
            _customStateFormatter = customStateFormatter;
        }

        protected override IEnumerable<string> FormatState(TState state)
        {
            if(_customStateFormatter == null)
            {
                string defaultResult = _stateFormatter(state, null);
                return new string[] { defaultResult };
            }

            IEnumerable<string> formatted = _customStateFormatter.Format(state);
            return formatted;
        }
    }
}
