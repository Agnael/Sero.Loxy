using Sero.Loxy.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.Proxies
{
    class ProxyStateFormatterFactory : IStateFormatterFactory
    {
        IStateFormatter<TState> IStateFormatterFactory.Create<TState>(Func<TState, Exception, string> defaultStateFormatter)
        {
            var stateFormatter = new ProxyStateFormatter<TState>(defaultStateFormatter);
            return stateFormatter;
        }
    }
}
