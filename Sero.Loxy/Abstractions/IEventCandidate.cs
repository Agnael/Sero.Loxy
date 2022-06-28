using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sero.Loxy
{
    public interface IEventCandidate
    {
        LogLevel Level { get; }
        string Message { get; }
        string Category { get; }
        Exception Exception { get; }

        IEnumerable<string> GetDetails();
    }

    //public interface IEventCandidate<TState> : IEventCandidate
    //{
    //    TState State { get; }
    //    Func<TState, Exception, string> DefaultFormatter { get; }
    //}
}