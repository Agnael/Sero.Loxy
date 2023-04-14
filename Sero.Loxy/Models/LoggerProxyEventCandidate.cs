using Microsoft.Extensions.Logging;
using System;

namespace Sero.Loxy;

public interface IProxyEventCandidate
{
   EventId EventId { get; }
   object GetState();
   Exception GetException();
}

public record LoggerProxyEventCandidate<TState>(
   EventId EventId,
   LogLevel Level,
   string Category,
   string Message,
   TState State,
   Func<TState, Exception, string> StateFormatter,
   Exception Exception = null
) :
   EventCandidate<TState>(Level, Category, Message, State, StateFormatter, Exception),
   IProxyEventCandidate
{
   public object GetState()
   {
      return this.State;
   }

   public Exception GetException()
   {
      return this.Exception;
   }
}
