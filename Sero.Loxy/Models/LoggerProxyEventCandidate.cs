using Microsoft.Extensions.Logging;
using System;

namespace Sero.Loxy;

public record LoggerProxyEventCandidate<TState>(
   LogLevel Level,
   string Category,
   string Message,
   TState State,
   Func<TState, Exception, string> StateFormatter,
   Exception Exception = null
) : EventCandidate<TState>(Level, Category, Message, State, StateFormatter, Exception);
