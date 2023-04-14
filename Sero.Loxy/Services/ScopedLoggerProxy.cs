using Microsoft.Extensions.Logging;
using System;

namespace Sero.Loxy;

public class ScopedLoggerProxy : ILogger
{
   private readonly ILoxy _loxy;
   private readonly string _categoryName;

   public ScopedLoggerProxy(ILoxy loxy, string categoryName)
   {
      _loxy = loxy;
      _categoryName = categoryName;
   }

   public IDisposable BeginScope<TState>(TState state)
   {
      return state as IDisposable;
   }

   public bool IsEnabled(LogLevel logLevel)
   {
      return true;
   }

   public void Log<TState>(
      LogLevel logLevel,
      EventId eventId,
      TState state,
      Exception exception,
      Func<TState, Exception, string> defaultFormatter)
   {
      _loxy.Raise(
         new LoggerProxyEventCandidate<TState>(
            eventId,
            logLevel,
            _categoryName,
            eventId.Name,
            state,
            defaultFormatter,
            exception
         )
      );
   }
}