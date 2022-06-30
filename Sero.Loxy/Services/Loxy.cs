using Microsoft.Extensions.Logging;
using NodaTime;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sero.Loxy
{
   public class Loxy<TContext> : ILoxy<TContext>
       where TContext : class
   {
      private readonly IClock _clock;
      private readonly IEnumerable<ISink> _sinks;
      private readonly IEventMapper _eventMapper;
      private readonly IScopeBuilder _scopeBuilder;

      private readonly ConcurrentDictionary<Guid, TimestampedEventCandidate> _keyedCandidateMap;
      private readonly List<Action<TContext>> _contextConfigurators;

      private LogLevel _maxLevel;

      public Loxy(
          IClock clock,
          IEnumerable<ISink> sinks,
          IEventMapper eventMapper,
          IScopeBuilder scopeBuilder)
      {
         _clock = clock;
         _sinks = sinks;
         _eventMapper = eventMapper;
         _scopeBuilder = scopeBuilder;

         _keyedCandidateMap =
             new ConcurrentDictionary<Guid, TimestampedEventCandidate>();

         _contextConfigurators = new List<Action<TContext>>();
         _maxLevel = LogLevel.Trace;
      }

      public void Raise(IEventCandidate evt)
      {
         TimestampedEventCandidate timestampedEvt =
             new TimestampedEventCandidate(_clock.GetCurrentInstant(), evt);

         while (!_keyedCandidateMap.TryAdd(Guid.NewGuid(), timestampedEvt))
         {
            // It's assumed Guids are unique so just keep creating new Guids until there is one that does
            // exist. It doesn't matter if it happens that 2 separate threads raise the same event, that's
            // up to the user to manage.
         }

         if (_maxLevel < evt.Level)
            _maxLevel = evt.Level;
      }

      public async Task Flush()
      {
         // Processed context
         TContext context = null;

         // Map for already processed candidates. This way if a candidate
         // does not pass the criteria for any sink, time is not wasted in
         // mapping it to a complete event (deferred formatting).
         Dictionary<Guid, IEvent> keyedEventMap =
             new Dictionary<Guid, IEvent>();

         LogLevel maxCandidateLevel = LogLevel.Trace;

         if (_keyedCandidateMap.Count > 0)
         {
            maxCandidateLevel =
                _keyedCandidateMap.Max(x => x.Value.Candidate.Level);
         }

         // Converts all IEventCandidate to IEvent and adds them to the map
         void ProcessCandidates(
             IDictionary<Guid, TimestampedEventCandidate> candidateMap)
         {
            foreach (
                KeyValuePair<Guid, TimestampedEventCandidate> keyedCandidate
                in candidateMap)
            {
               if (!keyedEventMap.ContainsKey(keyedCandidate.Key))
               {
                  IEvent mapped = _eventMapper.Map(keyedCandidate.Value);
                  keyedEventMap.Add(keyedCandidate.Key, mapped);
               }
            }
         }

         foreach (ISink sink in _sinks)
         {
            if (maxCandidateLevel >= sink.GetMinimumLevel())
            {
               // Builds context if it´s not created yet
               if (context == null)
               {
                  context = Activator.CreateInstance<TContext>();

                  foreach (Action<TContext> contextConfigurator in _contextConfigurators)
                  {
                     contextConfigurator(context);
                  }
               }

               // Should it accept all candidates?
               if (maxCandidateLevel >= sink.GetDumpLevel())
               {
                  ProcessCandidates(_keyedCandidateMap);
               }
               else
               {
                  // Only processes relevant events
                  Dictionary<Guid, TimestampedEventCandidate> relevantKeyedCandidateMap =
                      _keyedCandidateMap
                      .Where(x => x.Value.Candidate.Level >= sink.GetMinimumLevel())
                      .ToDictionary(x => x.Key, x => x.Value);

                  ProcessCandidates(relevantKeyedCandidateMap);
               }

               IEnumerable<IEvent> events = keyedEventMap.Select(x => x.Value);

               IScope scope = await _scopeBuilder.Build(context, _contextConfigurators.Count, events);

               await sink.Send(scope);
            }
         }
      }

      public void ConfigureContext(Action<TContext> configAction)
      {
         if (configAction == null)
            throw new ArgumentNullException(nameof(configAction));

         _contextConfigurators.Add(configAction);
      }
   }
}