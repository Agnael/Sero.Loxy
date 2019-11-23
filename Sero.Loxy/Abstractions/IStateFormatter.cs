using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy.Abstractions
{
    public interface IStateFormatter
    {
        IEnumerable<string> Format<TState>(TState state);
    }
}
