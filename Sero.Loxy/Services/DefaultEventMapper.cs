using System;
using System.Collections.Generic;
using System.Linq;

namespace Sero.Loxy;

public class DefaultEventMapper : IEventMapper
{
   private readonly IExceptionMapper _exceptionMapper;
   private readonly ILoxyStateFormatterProvider _loxyStateFormatterProvider;

   public DefaultEventMapper(
      IExceptionMapper exceptionMapper,
      ILoxyStateFormatterProvider loxyStateFormatterProvider)
   {
      _exceptionMapper = exceptionMapper;
      _loxyStateFormatterProvider = loxyStateFormatterProvider;
   }

   public IEnumerable<string> GetDefaultEventDetails(IEventCandidate evt)
   {
      return evt.GetDetails() ?? new List<string>();
   }

   public IEvent Map(TimestampedEventCandidate timestampedCandidate)
   {
      IEventCandidate candidate = timestampedCandidate.Candidate;

      Event mapped = new Event();
      mapped.Category = candidate.Category;
      mapped.CreationDtUtc =
          timestampedCandidate.CreationInstant.ToDateTimeUtc();
      mapped.Level = candidate.Level;
      mapped.Message = candidate.Message;
      mapped.TypeFullName = candidate.GetType().GetFriendlyFullName();

      // If this is a candidate that was built by converting a default ILogger.Log() method call it means it
      // DOES have a standard EventId attached, which can be used to look for custom state formatters.
      if (timestampedCandidate.Candidate is IProxyEventCandidate)
      {
         IProxyEventCandidate proxyCandidate = timestampedCandidate.Candidate as IProxyEventCandidate;
         Maybe<LoxyStateFormatter> customFormatterMaybe = 
            _loxyStateFormatterProvider.GetFor(proxyCandidate.EventId);

         customFormatterMaybe.Match(
            customFormatter =>
            {
               object evtState = proxyCandidate.GetState();
               Exception evtException = proxyCandidate.GetException();

               mapped.Details = customFormatter.CustomStateFormatter(evtState, evtException);
            },
            () =>
            {
               mapped.Details = GetDefaultEventDetails(candidate);
            }
         );
      }
      else
      {
         mapped.Details = GetDefaultEventDetails(candidate);
      }

      if (candidate.Exception == null)
      {
         mapped.Exception = Enumerable.Empty<ExceptionOverview>();
      }
      else
      {
         mapped.Exception = _exceptionMapper.Map(candidate.Exception);
      }

      return mapped;
   }
}
