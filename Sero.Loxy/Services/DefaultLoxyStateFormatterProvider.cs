using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Sero.Loxy;

public class DefaultLoxyStateFormatterProvider : ILoxyStateFormatterProvider
{
   private readonly IEnumerable<LoxyStateFormatter> _customFormatters;

   public DefaultLoxyStateFormatterProvider(IEnumerable<LoxyStateFormatter> customFormatters)
   {
      _customFormatters = 
         customFormatters 
         ?? new List<LoxyStateFormatter>();
   }

   public Maybe<LoxyStateFormatter> GetFor(EventId eventId)
   {
      return 
         _customFormatters
         .FirstOrDefault(x => x.TargetEventId.Equals(eventId))
         .ToMaybe();
   }
}
