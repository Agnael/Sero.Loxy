using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using NodaTime;

namespace Sero.Loxy;

public class HybridLoggerProxy : ILogger
{
   private readonly IHttpContextAccessor _httpContextAccessor;
   private readonly IEventMapper _eventMapper;
   private readonly IClock _clock;
   private readonly string _categoryName;
   private readonly IEnumerable<ISink> _sinks;

   public HybridLoggerProxy(
       string categoryName,
       IHttpContextAccessor httpContextAccessor,
       IEnumerable<ISink> sinks,
       IEventMapper eventMapper,
       IClock clock)
   {
      _categoryName = categoryName;
      _httpContextAccessor = httpContextAccessor;
      _sinks = sinks;
      _eventMapper = eventMapper;
      _clock = clock;
   }

   public IDisposable BeginScope<TState>(TState state)
   {
      return state as IDisposable;
   }

   public bool IsEnabled(LogLevel logLevel)
   {
      return true;
   }

   public void SendToSinks(HttpContext currentHttpContext, IEventCandidate eventCandidate)
   {
      // TODO: Create a system to catch and transform weird logs into actual
      // events, i.e.:
      //  Category = "Microsoft.AspNetCore.Hosting.Diagnostics"
      //  EventId = { id= 2, name= null }
      // Este log resuelve a un evento con message nulo y eso sería
      // jodido de reconocer en un intento de crear reportes y detectar
      // este evento en particular
      if (currentHttpContext != null && currentHttpContext.RequestServices == null)
      {

      }

      TimestampedEventCandidate timestampedCandidate =
          new TimestampedEventCandidate(_clock.GetCurrentInstant(), eventCandidate);

      IEvent evt = _eventMapper.Map(timestampedCandidate);

      foreach (ISink sink in _sinks)
      {
         sink.Send(evt);
      }
   }

   public void Log<TState>(
      LogLevel logLevel,
      EventId eventId,
      TState state,
      Exception exception,
      Func<TState, Exception, string> formatter)
   {
      LoggerProxyEventCandidate<TState> candidate =
          new LoggerProxyEventCandidate<TState>(
             eventId,
              logLevel,
              _categoryName,
              eventId.Name,
              state,
              formatter,
              exception
         );

      HttpContext currentHttpContext = _httpContextAccessor.HttpContext;

      if (currentHttpContext == null || currentHttpContext.RequestServices == null)
      {
         SendToSinks(currentHttpContext, candidate);
      }
      else
      {
         ILoxy loxy = null;

         try
         {
            loxy = currentHttpContext.RequestServices.GetService<ILoxy>();
         }
         // It is possible for the service provider to be instantiated but no longer usable. This case
         // can only be detected when this IDisposable object is used after its disposition, which is
         // captured here.
         catch (ObjectDisposedException ex)
         {
            SendToSinks(currentHttpContext, candidate);
         }

         if (loxy != null)
         {
            loxy.Raise(candidate);
         }
      }
   }
}
