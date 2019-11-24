using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.EfCore
{
    public class CleanStateFormatterFactory : IStateFormatterFactory
    {
        public IStateFormatter<TState> Create<TState>(Func<TState, Exception, string> defaultStateFormatter)
        {
            var formatter = new CleanStateFormatter<TState>();
            return formatter;
        }
    }
}
